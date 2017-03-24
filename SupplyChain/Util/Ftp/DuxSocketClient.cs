using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Collections;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Specialized;
using System.Threading;

namespace Application.Business.Erp.SupplyChain.Util.Ftp
{
    public class DuxSocketClient
    {
        #region ˽���ֶ�
        /// <summary>
        /// �������ݻ�������С Ĭ��1024
        /// </summary>
        private static int m_maxpacket = 1024 * 4;
        public delegate void SendFileProgress(int progress);
        public delegate void ReceiveFileProgress(int progress);
        #endregion
        #region ����������
        /// <summary>
        /// �������������� ����null��˵��û��������
        /// </summary>
        /// <returns>����һ���׽���(Socket)</returns>
        public static Socket ListenerSocket(TcpListener listener)
        {
            try
            {
                Socket socket = listener.AcceptSocket();
                return socket;
            } catch
            {
                return null;
            }
        }
        /// <summary>
        /// �������������� ����null��˵��û��������
        /// </summary>
        /// <param name="listener"></param>
        /// <returns>����һ��������</returns>
        public static NetworkStream ListenerStream(TcpListener listener)
        {
            try
            {
                TcpClient client = listener.AcceptTcpClient();
                return client.GetStream();
            } catch
            {
                return null;
            }
        }
        #endregion
        #region �ͻ�������
        public static Socket ConnectSocket(TcpClient tcpclient, IPEndPoint ipendpoint)
        {
            try
            {
                tcpclient.Connect(ipendpoint);
                return tcpclient.Client;
            } catch
            {
                return null;
            }
        }
        public static Socket ConnectSocket(TcpClient tcpclient, IPAddress ipadd, int port)
        {
            try
            {
                tcpclient.Connect(ipadd, port);
                return tcpclient.Client;
            } catch
            {
                return null;
            }
        }
        public static NetworkStream ConnectStream(TcpClient tcpclient, IPEndPoint ipendpoint)
        {
            try
            {
                tcpclient.Connect(ipendpoint);
                return tcpclient.GetStream();
            } catch
            {
                return null;
            }
        }
        public static NetworkStream ConnectStream(TcpClient tcpclient, IPAddress ipadd, int port)
        {
            try
            {
                tcpclient.Connect(ipadd, port);
                return tcpclient.GetStream();
            } catch
            {
                return null;
            }
        }
        #endregion
        #region Socket��������
        /// <summary>
        /// ���̶ܹ������ַ���
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] ReceiveFixData(Socket socket, int size)
        {
            int offset = 0;
            int recv = 0;
            int dataleft = size;
            byte[] msg = new byte[size];
            while (dataleft > 0)
            {
                recv = socket.Receive(msg, offset, dataleft, 0);
                if (recv == 0)
                {
                    break;
                }
                offset += recv;
                dataleft -= recv;
            }
            return msg;
        }
        /// <summary>
        /// ���ձ䳤�ַ���
        /// Ϊ�˴���ճ������ ,ÿ�η�������ʱ ��ͷ(�����ֽڳ���) + ����
        /// �������С����
        /// ���ð�ͷ���ֽ�Ϊ8,���ܳ���8λ�����ֽ�����
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>byte[]����</returns>
        public static byte[] ReceiveVarData(Socket socket)
        {
            //ÿ�ν�������ʱ,���չ̶����ȵİ�ͷ,��ͷ����Ϊ8
            byte[] lengthbyte = ReceiveFixData(socket, 8);
            //length�õ��ַ����� Ȼ��ӹ�����õ�����
            int length = GetPacketLength(lengthbyte);
            //�õ�����
            return ReceiveFixData(socket, length);
        }
        /// <summary>
        /// ����T�����,�����л�
        /// </summary>
        /// <typeparam name="T">����T�����,T�������һ�������л���</typeparam>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static T ReceiveVarData<T>(Socket socket)
        {
            //�Ƚ��հ�ͷ���� �̶�8���ֽ�
            byte[] lengthbyte = ReceiveFixData(socket, 8);
            //�õ��ֽڳ���
            int length = GetPacketLength(lengthbyte);
            byte[] bytecoll = new byte[m_maxpacket];
            IFormatter format = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            int offset = 0;  //�����ֽڸ���
            int lastdata = length; //��ʣ�¶���û�н���,��ʼ��С����ʵ�ʴ�С
            int receivedata = m_maxpacket;  //ÿ�ν��մ�С
            //ѭ������
            int mark = 0; //��Ǽ��ν��յ�������Ϊ0����
            while (true)
            {
                //ʣ�µ��ֽ����Ƿ�С�ڻ����С
                if (lastdata < m_maxpacket)
                {
                    receivedata = lastdata;  //��ֻ����ʣ�µ��ֽ���
                }
                int count = socket.Receive(bytecoll, 0, receivedata, 0);
                if (count > 0)
                {
                    stream.Write(bytecoll, 0, count);
                    offset += count;
                    lastdata -= count;
                    mark = 0;
                } else
                {
                    mark++;
                    if (mark == 10)
                    {
                        break;
                    }
                }
                if (offset == length)
                {
                    break;
                }
            }
            stream.Seek(0, SeekOrigin.Begin); //����Ҫ��� ����stream.Position = 0;
            T t = (T)format.Deserialize(stream);
            stream.Close();
            return t;
        }
        /// <summary>
        /// ��Ԥ�ȵõ��ļ����ļ����ʹ�С
        /// ���ô˷��������ļ�
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="path">·���������</param>
        public static bool ReceiveFile(Socket socket, string path, string filename, long size, ReceiveFileProgress progress)
        {
            bool ret = false;
            if (Directory.Exists(path))
            {
                //��Ҫ�Ƿ�ֹ�������ļ�
                string savepath = GetPath(path, filename); //�õ��ļ�·��
                //������
                byte[] file = new byte[m_maxpacket];
                int count = 0;  //ÿ�ν��յ�ʵ�ʳ���
                int receivedata = m_maxpacket; //ÿ��Ҫ���յĳ���
                long offset = 0;  //ѭ�����յ��ܳ���
                long lastdata = size;  //ʣ����ٻ�û����
                int mark = 0;
                using (FileStream fs = new FileStream(savepath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    if (size > 0)
                    {
                        while (true)
                        {
                            if (lastdata < receivedata)
                            {
                                receivedata = Convert.ToInt32(lastdata);
                            }
                            count = socket.Receive(file, 0, receivedata, SocketFlags.None);
                            if (count > 0)
                            {
                                fs.Write(file, 0, count);
                                offset += count;
                                lastdata -= count;
                                mark = 0;
                            } else
                            {
                                mark++;  //����5�ν���Ϊ0�ֽ� ������ѭ��
                                if (mark == 10)
                                {
                                    break;
                                }
                            }
                            //���ս���
                            if (progress != null)
                            {
                                progress(Convert.ToInt32(((Convert.ToDouble(offset) / Convert.ToDouble(size)) * 100)));
                            }
                            //�������
                            if (offset == size)
                            {
                                ret = true;
                                break;
                            }
                        }
                    }
                    fs.Close();
                }
            }
            return ret;
        }
        public static bool ReceiveFile(Socket socket, string path, string filename, long size)
        {
            return ReceiveFile(socket, path, filename, size, null);
        }
        /// <summary>
        /// Ԥ�Ȳ�֪���ļ������ļ���С �ô˷�������
        /// �˷������ڵķ��ͷ�����SendFile()
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="path">Ҫ�����Ŀ¼</param>
        public static void ReceiveFile(Socket socket, string path)
        {
            //�õ���ͷ��Ϣ�ֽ����� (�ļ��� + �ļ���С ���ַ�������)
            //ȡǰ8λ
            byte[] info_bt = ReceiveFixData(socket, 8);
            //�õ���ͷ��Ϣ�ַ�����
            int info_length = GetPacketLength(info_bt);
            //��ȡ��ͷ��Ϣ,(�ļ��� + �ļ���С ���ַ�������)
            byte[] info = ReceiveFixData(socket, info_length);
            //�õ��ļ���Ϣ�ַ��� (�ļ��� + �ļ���С)
            string info_str = System.Text.Encoding.UTF8.GetString(info);
            string[] strs = info_str.Split('|');
            string filename = strs[0];  //�ļ���
            long length = Convert.ToInt64(strs[1]); //�ļ���С
            //��ʼ�����ļ�
            ReceiveFile(socket, path, filename, length);
        }
        private static int GetPacketLength(byte[] length)
        {
            string str = System.Text.Encoding.UTF8.GetString(length);
            str = str.TrimEnd('*'); ;//("*", "");
            int _length = 0;
            if (int.TryParse(str, out _length))
            {
                return _length;
            } else
            {
                return 0;
            }
        }
        /// <summary>
        /// �õ��ļ�·��(��ֹ���ļ����ظ�)
        ///  ��:aaa.txt�Ѿ���directoryĿ¼�´���,���õ��ļ�aaa(1).txt
        /// </summary>
        /// <param name="directory">Ŀ¼��</param>
        /// <param name="file">�ļ���</param>
        /// <returns>�ļ�·��</returns>
        static int i = 0;
        static string markPath = String.Empty;
        public static string GetPath(string directory, string file)
        {
            if (markPath == String.Empty)
            {
                markPath = Path.Combine(directory, file);
            }
            string path = Path.Combine(directory, file);
            if (File.Exists(path))
            {
                i++;
                string filename = Path.GetFileNameWithoutExtension(markPath) + "(" + i.ToString() + ")";
                string extension = Path.GetExtension(markPath);
                return GetPath(directory, filename + extension);
            } else
            {
                i = 0;
                markPath = String.Empty;
                return path;
            }
        }
        #endregion
        #region Socket��������
        /// <summary>
        /// ���͹̶�������Ϣ
        /// �����ֽ������ܴ���int�����ֵ
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="msg"></param>
        /// <returns>���ط����ֽڸ���</returns>
        public static int SendFixData(Socket socket, byte[] msg)
        {
            int size = msg.Length;  //Ҫ�����ֽڳ���
            int offset = 0;         //�Ѿ����ͳ���
            int dataleft = size;    //ʣ���ַ�
            int senddata = m_maxpacket;  //ÿ�η��ʹ�С
            while (true)
            {
                //���ʣ�µ��ֽ��� С�� ÿ�η����ֽ���
                if (dataleft < senddata)
                {
                    senddata = dataleft;
                }
                int count = socket.Send(msg, offset, senddata, SocketFlags.None);
                offset += count;
                dataleft -= count;
                if (offset == size)
                {
                    break;
                }
            }
            return offset;
        }
        /// <summary>
        /// ���ͱ䳤��Ϣ ��ʽ ��ͷ(��ͷռ8λ) + ����
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="contact">�����ı�</param>
        /// <returns></returns>
        public static int SendVarData(Socket socket, string contact)
        {
            //�õ��ַ�����
            int size = System.Text.Encoding.UTF8.GetBytes(contact).Length;
            //��ͷ�ַ�
            string length = GetSendPacketLengthStr(size);
            //��ͷ + ����
            byte[] sendbyte = System.Text.Encoding.UTF8.GetBytes(length + contact);
            //����
            return SendFixData(socket, sendbyte);
        }
        /// <summary>
        /// ���ͱ����Ϣ
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int SendVarData(Socket socket, byte[] bytes)
        {
            //�õ���ͷ�ֽ�
            int size = bytes.Length;
            string length = GetSendPacketLengthStr(size);
            byte[] lengthbyte = System.Text.Encoding.UTF8.GetBytes(length);
            //���Ͱ�ͷ
            SendFixData(socket, lengthbyte);   //��Ϊ��֪��������ʲô��������û�кϲ�
            //��������
            return SendFixData(socket, bytes);
        }
        /// <summary>
        /// ����T���Ͷ���,���л�
        /// </summary>
        /// <typeparam name="T">T����</typeparam>
        /// <param name="socket"></param>
        /// <param name="obj">T���Ͷ���,�����ǿ����л���</param>
        /// <returns></returns>
        public static int SendSerializeObject<T>(Socket socket, T obj)
        {
            byte[] bytes = SerializeObject(obj);
            return SendVarData(socket, bytes);
        }
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="socket">socket����</param>
        /// <param name="path">�ļ�·��</param>
        /// <param name="issend">�Ƿ����ļ�(ͷ)��Ϣ,�����ǰ֪���ļ�[��С,����]��Ϊfalse</param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static bool SendFile(Socket socket, string path, bool issend, SendFileProgress progress)
        {
            bool ret = false;
            if (File.Exists(path))
            {
                FileInfo fileinfo = new FileInfo(path);
                string filename = fileinfo.Name;
                long length = fileinfo.Length;
                //�����ļ���Ϣ
                if (issend)
                {
                    SendVarData(socket, filename + "|" + length);
                }
                //�����ļ�
                long offset = 0;
                byte[] b = new byte[m_maxpacket];
                int mark = 0;
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    int senddata = b.Length;
                    long i = length;
                    //ѭ����ȡ����
                    while (true)
                    {
                        int count = fs.Read(b, 0, senddata);
                        if (count > 0)
                        {
                            socket.Send(b, 0, count, SocketFlags.None);
                            offset += count;
                            mark = 0;
                        } else
                        {
                            mark++;
                            if (mark == 10)
                            {
                                break;
                            }
                        }
                        if (progress != null)
                        {
                            progress(Convert.ToInt32(((Convert.ToDouble(offset) / Convert.ToDouble(length)) * 100)));
                        }
                        if (offset == length)
                        {
                            break;
                        }
                        Thread.Sleep(50); //���õȴ�ʱ��,����ճ��
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// �����ļ�,����Ҫ������Ϣ
        /// </summary>
        /// <param name="socket">socket����</param>
        /// <param name="path">�ļ�·��</param>
        /// <param name="issend">�Ƿ���(ͷ)��Ϣ</param>
        /// <returns></returns>
        public static bool SendFile(Socket socket, string path, bool issend)
        {
            return SendFile(socket, path, issend, null);
        }
        /// <summary>
        /// �����ļ�,����Ҫ������Ϣ��(ͷ)��Ϣ
        /// </summary>
        /// <param name="socket">socket����</param>
        /// <param name="path">�ļ�·��</param>
        /// <returns></returns>
        public static bool SendFile(Socket socket, string path)
        {
            return SendFile(socket, path, false, null);
        }
        private static byte[] SerializeObject(object obj)
        {
            IFormatter format = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            format.Serialize(stream, obj);
            byte[] ret = stream.ToArray();
            stream.Close();
            return ret;
        }
        private static string GetSendPacketLengthStr(int size)
        {
            string length = size.ToString() + "********"; //�õ�size�ĳ���
            return length.Substring(0, 8); //��ȡǰǰ8λ
        }
        #endregion
        #region NetworkStream��������
        //ûд
        #endregion
        #region NetworkStream��������
        //ûд
        #endregion
    }
}
