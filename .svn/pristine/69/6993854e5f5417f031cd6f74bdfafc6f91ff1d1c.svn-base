using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace PortalIntegrationConsole
{
    public partial class FrmSetXML : Form
    {
        public FrmSetXML()
        {
            InitializeComponent();
        }

        private void btnSelXMLFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();     //显示选择文件对话框
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "xml files (*.xml)|*.xml";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = false;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtXMLFile.Text = openFileDialog1.FileName;
            }
        }

        private void btnSelXMLDirFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();     //显示选择文件对话框
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "xml files (*.xml)|*.xml";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtXMLDirFile.Text = openFileDialog1.FileName;
            }
        }

        private void btnUserCodeToUpper_Click(object sender, EventArgs e)
        {
            try
            {
                string strXmlFile = txtXMLFile.Text.Trim();
                string strXmlFileDictionary = txtXMLDirFile.Text.Trim();
                if (File.Exists(strXmlFile) == false || File.Exists(strXmlFile) == false)
                {
                    MessageBox.Show("文件路径不正确！");
                    return;
                }

                XmlDocument objXmlDoc = new XmlDocument();
                XmlDocument objXmlDirDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);
                objXmlDirDoc.Load(strXmlFileDictionary);

                string UsersDirectoryDictionaryNodeStr = "DictionarySerializer";

                XmlNodeList theUserXmlNodeList = objXmlDoc.GetElementsByTagName("User"); //objXmlDoc.SelectSingleNode(UsersDirectoryNodeStr).ChildNodes;

                XmlNodeList UserDirHeaderNodeList = objXmlDirDoc.SelectSingleNode(UsersDirectoryDictionaryNodeStr).FirstChild.ChildNodes;
                XmlNodeList UserDirCaptionNodeList = objXmlDirDoc.SelectSingleNode(UsersDirectoryDictionaryNodeStr).LastChild.ChildNodes;

                for (int i = theUserXmlNodeList.Count - 1; i > -1; i--)
                {
                    XmlNode theXmlNode = theUserXmlNodeList[i];
                    XmlElement theXmlElement = (XmlElement)theXmlNode;
                    string userCode = theXmlElement.GetAttribute("Name");
                    theXmlElement.SetAttribute("Name", userCode.ToUpper());

                    //更新用户字典
                    //更新头部
                    for (int j = UserDirHeaderNodeList.Count - 1; j > -1; j--)
                    {
                        XmlNode xmlNode = UserDirHeaderNodeList[j];

                        XmlElement theElement = (XmlElement)xmlNode;
                        string userCode2 = theElement.GetAttribute("Name");
                        if (userCode2.Equals(userCode, StringComparison.OrdinalIgnoreCase))
                        {
                            theElement.SetAttribute("Name", userCode2.ToUpper());
                            break;
                        }
                    }
                    //更新标题
                    for (int j = UserDirCaptionNodeList.Count - 1; j > -1; j--)
                    {
                        XmlNode xmlNode = UserDirCaptionNodeList[j];

                        XmlElement theElement = (XmlElement)xmlNode;
                        string userCode2 = theElement.GetAttribute("Name");
                        if (userCode2.Equals(userCode, StringComparison.OrdinalIgnoreCase))
                        {
                            theElement.SetAttribute("Name", userCode2.ToUpper());
                            break;
                        }
                    }
                }

                objXmlDoc.Save(strXmlFile);
                objXmlDirDoc.Save(strXmlFileDictionary);

                MessageBox.Show("转换成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(VirtualMachine.Component.Util.ExceptionUtil.ExceptionMessage(ex));
            }
        }
    }
}
