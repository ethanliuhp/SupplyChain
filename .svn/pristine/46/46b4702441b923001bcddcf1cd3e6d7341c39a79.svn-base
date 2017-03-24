using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Util.Ftp
{
    public class FTPFactory
    {
        #region Arrbuite
        private string remoteHost, remotePath, remoteUser, remotePass, mes;
        private int remotePort, bytes;
        private Socket clientSocket;

        private int retValue;
        private Boolean debug;
        private Boolean logined;
        private string reply;
        private static int BLOCK_SIZE = 1024;
        //Encoding ASCII = Encoding.Default;
        Encoding ASCII = Encoding.GetEncoding("gb2312");
        //Encoding.GetEncoding( "gb2312 ").GetString(buffer,   0,   iBytes); 
        #endregion

        public FTPFactory()
        {
            remoteHost = "192.168.1.105";
            remotePath = ".";
            remoteUser = "Administrator";
            remotePass = "chen";
            FTPSetup();
        }

        private void FTPSetup()
        {
            remotePort = 21;
            debug = false;
            logined = false;
        }

        //public IList getFileList(string mask)
        //{
        //    if (!logined)
        //    {
        //        login();
        //    }

        //    Socket cSocket = createDataSocket();
        //    sendCommand("NLST " + mask);
        //    if (!(retValue == 150 || retValue == 125))
        //    {
        //        throw new IOException(reply.Substring(4));
        //    }

        //    //mes = "";
        //    StringBuilder sb = new StringBuilder();

        //    while (true)
        //    {

        //        int bytes = cSocket.Receive(buffer, buffer.Length, 0);
        //        //mes += Encoding.ASCII.GetString(buffer, 0, bytes);
        //        sb.Append( ASCII.GetString(buffer, 0, bytes));

        //        if (bytes <= 0)
        //        {
        //            break;
        //        }
        //    }

        //    char[] seperator = { '\n' };
        //    string[] mess = sb.ToString().Split(seperator);

        //    cSocket.Close();

        //    ArrayList list = new ArrayList();

        //    foreach (string m in mess)
        //    {
        //        if (!"".Equals(m.Replace("\r", "")))
        //            list.Add(m.Replace("\r", ""));
        //    }

        //    readReply();

        //    if (retValue != 226)
        //    {
        //        throw new IOException(reply.Substring(4));
        //    }
        //    return list;        
        //}

        public string[] getFileList(string mask)
        {

            if (!logined)
            {
                login();
            }

            Socket cSocket = createDataSocket();
            sendCommand("NLST " + mask);
            if (!(retValue == 150 || retValue == 125))
            {
                throw new IOException(reply.Substring(4));
            }

            mes = "";
            Byte[] buffer = new Byte[BLOCK_SIZE];
            while (true)
            {

                int bytes = cSocket.Receive(buffer, buffer.Length, 0);
                //mes += Encoding.ASCII.GetString(buffer, 0, bytes);
                mes += ASCII.GetString(buffer, 0, bytes);

                if (bytes <= 0)
                {
                    break;
                }
            }

            char[] seperator = { '\n' };
            string[] mess = mes.Split(seperator);

            cSocket.Close();

            readReply();

            if (retValue != 226)
            {
                throw new IOException(reply.Substring(4));
            }
            return mess;

        }

        public long getFileSize(string fileName)
        {

            if (!logined)
            {
                login();
            }

            sendCommand("SIZE " + fileName);
            long size = 0;

            if (retValue == 213)
            {
                size = Int64.Parse(reply.Substring(4));
            }
            else
            {
                throw new IOException(reply.Substring(4));
            }

            return size;

        }

        public void login()
        {

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(Dns.GetHostEntry(remoteHost).AddressList[0], remotePort);
            try
            {
                clientSocket.Connect(ep);
            }
            catch
            {
                //throw new IOException("Couldn't connect to remote server");;
            }

            readReply();
            if (retValue != 220)
            {
                Close();
                //throw new IOException(reply.Substring(4));
            }
            if (debug)
                Console.WriteLine("USER " + remoteUser);

            sendCommand("USER " + remoteUser);
            if (!(retValue == 331 || retValue == 230))
            {
                cleanup();
                //throw new IOException(reply.Substring(4));
            }

            if (retValue != 230)
            {
                if (debug)
                    Console.WriteLine("PASS xxx");

                sendCommand("PASS " + remotePass);
                if (!(retValue == 230 || retValue == 202))
                {
                    cleanup();
                    //throw new IOException(reply.Substring(4));
                }
            }
            logined = true;
            Console.WriteLine("Connected to " + remoteHost);
            ChDir(remotePath);
        }

        #region setBinaryMode

        /**/
        /// <summary>
        /// If the value of mode is true, set binary mode for downloads.
        /// Else, set Ascii mode.
        /// </summary>
        /// <param name="mode"></param>
        public void setBinaryMode(Boolean mode)
        {

            if (mode)
            {
                sendCommand("TYPE I");
            }
            else
            {
                sendCommand("TYPE A");
            }
            if (retValue != 200)
            {
                throw new IOException(reply.Substring(4));
            }
        }
        #endregion

        #region Download
        /**/
        /// <summary>
        /// Download a file to the Assembly's local directory,
        /// keeping the same file name.
        /// </summary>
        /// <param name="remFileName"></param>
        /// <returns></returns>
        public bool Download(string remFileName)
        {
            return Download(remFileName, "", false);
        }

        /**/
        /// <summary>
        /// Download a remote file to the Assembly's local directory,
        /// keeping the same file name, and set the resume flag.
        /// </summary>
        /// <param name="remFileName"></param>
        /// <param name="resume"></param>
        /// <returns></returns>
        public bool Download(string remFileName, Boolean resume)
        {
            return Download(remFileName, "", resume);
        }

        /**/
        /// <summary>
        /// Download a remote file to a local file name which can include
        /// a path. The local file name will be created or overwritten,
        /// but the path must exist.
        /// </summary>
        /// <param name="remFileName"></param>
        /// <param name="locFileName"></param>
        /// <returns></returns>
        public bool Download(string remFileName, string locFileName)
        {
            return Download(remFileName, locFileName, false);
        }



        /**/
        /// <summary>
        /// Download a remote file to a local file name which can include
        /// a path, and set the resume flag. The local file name will be
        /// created or overwritten, but the path must exist.
        /// </summary>
        /// <param name="remFileName"></param>
        /// <param name="locFileName"></param>
        /// <param name="resume"></param>
        /// <returns></returns>
        public bool Download(string remFileName, string locFileName, Boolean resume)
        {

            bool Finish = false;
            bool FileExists = false;
            if (!logined)
            {
                login();
            }
            setBinaryMode(true);
            try
            {
                FileInfo fileInfo = new FileInfo(locFileName);
                if (fileInfo.Exists)
                {
                    fileInfo.IsReadOnly = false;
                    fileInfo.Delete();
                }
            }
            catch
            {
                return false;
            }
            //634173062643593750广州钢铁交易中心钢坯GJ-BN-2010-042.docx
            //634173062643593750广州钢铁交易中心钢坯GJ-BN-2010-042.docx
            //string[] fileList = getFileList(".");
            string[] fileList = getFileList(".");
            foreach (string str in fileList)
            {
                if (str.IndexOf(remFileName.ToUpper(), StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    FileExists = true;
                    break;
                }
            }
            if (!FileExists)
            {
                //Console.WriteLine("文件不存在!");
                return false;
            }
            Console.WriteLine("Downloading file " + remFileName + " from " + remoteHost + "/" + remotePath);

            if (locFileName.Equals(""))
            {
                locFileName = remFileName;
            }

            if (!File.Exists(locFileName))
            {
                Stream st = File.Create(locFileName);
                st.Close();
            }
            FileStream output = null;
            try
            {
                output = new FileStream(locFileName, FileMode.Open);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            Socket cSocket = createDataSocket();
            long offset = 0;
            if (resume)
            {
                offset = output.Length;
                if (offset > 0)
                {
                    sendCommand("REST " + offset);
                    if (retValue != 350)
                    {
                        //throw new IOException(reply.Substring(4));
                        //Some servers may not support resuming.
                        Finish = false;
                        offset = 0;
                    }
                }

                if (offset > 0)
                {
                    if (debug)
                    {
                        Console.WriteLine("seeking to " + offset);
                    }
                    long npos = output.Seek(offset, SeekOrigin.Begin);
                    Console.WriteLine("new pos=" + npos);
                }
            }

            sendCommand("RETR " + remFileName);
            if (!(retValue == 150 || retValue == 125))
            {
                throw new IOException(reply.Substring(4));
            }
            Byte[] buffer = new Byte[BLOCK_SIZE];
            while (true)
            {
                bytes = cSocket.Receive(buffer, buffer.Length, 0);
                output.Write(buffer, 0, bytes);
                if (bytes <= 0)
                {
                    Finish = true;
                    break;
                }
            }

            output.Close();
            if (cSocket.Connected)
            {
                cSocket.Close();
            }
            readReply();
            if (!(retValue == 226 || retValue == 250))
            {
                throw new IOException(reply.Substring(4));
            }
            return Finish;
        }
        #endregion

        #region Upload
        /**/
        /// <summary>
        /// Upload a file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Upload a file is success!</returns>
        public bool Upload(string fileName)
        {
            return Upload(fileName, false);
        }

        public bool Upload(string fileName, string newFileName)
        {
            return Upload(fileName, newFileName, false);
        }

        public bool Upload(string fileName, string newFileName, Boolean resume)
        {
            if (!logined)
            {
                login();
            }
            Socket cSocket = createDataSocket();
            long offset = 0;
            if (resume)
            {
                try
                {
                    setBinaryMode(true);
                    offset = getFileSize(fileName);

                }
                catch (Exception)
                {
                    offset = 0;
                }
            }

            if (offset > 0)
            {
                sendCommand("REST " + offset);
                if (retValue != 350)
                {
                    //throw new IOException(reply.Substring(4));
                    //Remote server may not support resuming.
                    offset = 0;
                }
            }

            //sendCommand("STOR " + Path.GetFileName(fileName));
            if (newFileName == null || newFileName.Equals(""))
            {
                newFileName = Path.GetFileName(fileName);
            }
            sendCommand("STOR " + newFileName);

            if (!(retValue == 125 || retValue == 150))
            {
                throw new IOException(reply.Substring(4));
            }

            // open input stream to read source file
            FileStream input = new FileStream(fileName, FileMode.Open);
            if (offset != 0)
            {

                if (debug)
                {
                    Console.WriteLine("seeking to " + offset);
                }
                input.Seek(offset, SeekOrigin.Begin);
            }

            Console.WriteLine("Uploading file " + fileName + " to " + remotePath);
            Byte[] buffer = new Byte[BLOCK_SIZE];
            while ((bytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                cSocket.Send(buffer, bytes, 0);
            }
            input.Close();
            Console.WriteLine("");
            if (cSocket.Connected)
            {
                cSocket.Close();
            }
            readReply();
            if (!(retValue == 226 || retValue == 250))
            {
                throw new IOException(reply.Substring(4));
            }
            return true;
        }


        /**/
        /// <summary>
        /// Upload a file and set the resume flag.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="resume"></param>
        /// <returns></returns>
        public bool Upload(string fileName, Boolean resume)
        {
            if (!logined)
            {
                login();
            }
            Socket cSocket = createDataSocket();
            long offset = 0;
            if (resume)
            {
                try
                {
                    setBinaryMode(true);
                    offset = getFileSize(fileName);

                }
                catch (Exception)
                {
                    offset = 0;
                }
            }

            if (offset > 0)
            {
                sendCommand("REST " + offset);
                if (retValue != 350)
                {
                    //throw new IOException(reply.Substring(4));
                    //Remote server may not support resuming.
                    offset = 0;
                }
            }

            sendCommand("STOR " + Path.GetFileName(fileName));

            if (!(retValue == 125 || retValue == 150))
            {
                throw new IOException(reply.Substring(4));
            }

            // open input stream to read source file
            FileStream input = new FileStream(fileName, FileMode.Open);
            if (offset != 0)
            {

                if (debug)
                {
                    Console.WriteLine("seeking to " + offset);
                }
                input.Seek(offset, SeekOrigin.Begin);
            }

            Console.WriteLine("Uploading file " + fileName + " to " + remotePath);
            Byte[] buffer = new Byte[BLOCK_SIZE];
            while ((bytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                cSocket.Send(buffer, bytes, 0);
            }
            input.Close();
            Console.WriteLine("");
            if (cSocket.Connected)
            {
                cSocket.Close();
            }
            readReply();
            if (!(retValue == 226 || retValue == 250))
            {
                throw new IOException(reply.Substring(4));
            }
            return true;
        }
        #endregion

        #region DeleteRemoteFile
        /**/
        /// <summary>
        /// Delete a file from the remote FTP server.
        /// </summary>
        /// <param name="fileName"></param>
        public void DeleteRemoteFile(string fileName)
        {

            if (!logined)
            {
                login();
            }

            sendCommand("DELE " + fileName);
            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }

        }
        #endregion

        #region RenameRemoteFile
        /**/
        /// <summary>
        /// Rename a file on the remote FTP server.
        /// </summary>
        /// <param name="oldFileName"></param>
        /// <param name="newFileName"></param>
        public void RenameRemoteFile(string oldFileName, string newFileName)
        {

            if (!logined)
            {
                login();
            }

            sendCommand("RNFR " + oldFileName);

            if (retValue != 350)
            {
                throw new IOException(reply.Substring(4));
            }

            //  known problem
            //  rnto will not take care of existing file.
            //  i.e. It will overwrite if newFileName exist
            sendCommand("RNTO " + newFileName);
            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }

        }
        #endregion

        #region MkDir
        /**/
        /// <summary>
        /// Create a directory on the remote FTP server.
        /// </summary>
        /// <param name="dirName"></param>
        public void MkDir(string dirName)
        {

            if (!logined)
            {
                login();
            }

            sendCommand("MKD " + dirName);

            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }

        }
        #endregion

        #region RmDir
        /**/
        /// <summary>
        /// Delete a directory on the remote FTP server
        /// </summary>
        /// <param name="dirName"></param>
        public void RmDir(string dirName)
        {

            if (!logined)
            {
                login();
            }

            sendCommand("RMD " + dirName);

            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }

        }
        #endregion

        #region ChDir
        /**/
        /// <summary>
        /// Change the current working directory on the remote FTP server.
        /// </summary>
        /// <param name="dirName"></param>
        public void ChDir(string dirName)
        {

            if (dirName.Equals("."))
            {
                return;
            }

            if (!logined)
            {
                login();
            }

            sendCommand("CWD " + dirName);

            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }

            this.remotePath = dirName;

            Console.WriteLine("Current directory is " + remotePath);

        }
        #endregion

        #region Close()
        /**/
        /// <summary>
        ///  Close the FTP connection.
        /// </summary>
        public void Close()
        {

            if (clientSocket != null)
            {
                sendCommand("QUIT");
            }

            cleanup();
            Console.WriteLine("Closing");
        }
        #endregion

        #region OTHER
        /**/
        /// <summary>
        /// Set debug mode.
        /// </summary>
        /// <param name="debug"></param>
        public void setDebug(Boolean debug)
        {
            this.debug = debug;
        }

        private void readReply()
        {
            mes = "";
            reply = readLine();
            retValue = Int32.Parse(reply.Substring(0, 3));
        }

        private void cleanup()
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
            }
            logined = false;
        }
        #endregion

        #region readLine
        private string readLine()
        {
            Byte[] buffer = new Byte[BLOCK_SIZE];
            while (true)
            {
                bytes = clientSocket.Receive(buffer, buffer.Length, 0);
                mes += ASCII.GetString(buffer, 0, bytes);
                if (bytes < buffer.Length)
                {
                    break;
                }
            }

            char[] seperator = { '\n' };
            string[] mess = mes.Split(seperator);

            if (mes.Length > 2)
            {
                mes = mess[mess.Length - 2];
            }
            else
            {
                mes = mess[0];
            }

            if (!mes.Substring(3, 1).Equals(" "))
            {
                return readLine();
            }

            if (debug)
            {
                for (int k = 0; k < mess.Length - 1; k++)
                {
                    Console.WriteLine(mess[k]);
                }
            }
            return mes;
        }
        #endregion

        #region sendCommand
        private void sendCommand(String command)
        {

            Byte[] cmdBytes = this.ASCII.GetBytes((command + "\r\n").ToCharArray());
            clientSocket.Send(cmdBytes, cmdBytes.Length, 0);
            readReply();
        }
        #endregion

        #region createDataSocket
        private Socket createDataSocket()
        {
            sendCommand("PASV");
            if (retValue != 227)
            {
                throw new IOException(reply.Substring(4));
            }

            int index1 = reply.IndexOf('(');
            int index2 = reply.IndexOf(')');
            string ipData = reply.Substring(index1 + 1, index2 - index1 - 1);
            int[] parts = new int[6];
            int len = ipData.Length;
            int partCount = 0;
            string buf = "";
            for (int i = 0; i < len && partCount <= 6; i++)
            {

                char ch = Char.Parse(ipData.Substring(i, 1));
                if (Char.IsDigit(ch))
                    buf += ch;
                else if (ch != ',')
                {
                    throw new IOException("Malformed PASV reply: " + reply);
                }

                if (ch == ',' || i + 1 == len)
                {

                    try
                    {
                        parts[partCount++] = Int32.Parse(buf);
                        buf = "";
                    }
                    catch (Exception)
                    {
                        throw new IOException("Malformed PASV reply: " + reply);
                    }
                }
            }
            string ipAddress = parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[3];
            int port = (parts[4] << 8) + parts[5];
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(Dns.GetHostEntry(ipAddress).AddressList[0], port);
            try
            {
                s.Connect(ep);
            }
            catch (Exception)
            {
                throw new IOException("Can't connect to remote server");
            }
            return s;
        }
        #endregion


    }
}
