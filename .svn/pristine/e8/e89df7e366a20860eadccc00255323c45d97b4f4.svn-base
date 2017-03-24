using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.Util
{
    class UpdateProject
    {
        private XmlDocument m_doc=null;
        private string sXmlPath = string.Empty;
        private XmlNode oXmlNodeTasks = null;
        private string sNewTag = "(最新)";
        private string sDeleteTag = "(删除)";
        public  void  GetLoadDocument(string sXMlPath)
        {
            
            try
            {
                this.sXmlPath = sXMlPath;
                m_doc = new XmlDocument();
                m_doc.Load(sXMlPath);
                oXmlNodeTasks = FindNodeTasks();
            }
            catch
            {
            }
            
        }
        public UpdateProject(string sXmlPath)
        {
            GetLoadDocument(sXmlPath);
        }
        /// <summary>
        /// 直接插到该节点后面
        /// </summary>
        /// <param name="oNode"></param>
        /// <param name="detail"></param>
        public XmlNode AfterInsertXmlNode(XmlNode oNode, ProductionScheduleDetail detail,int iLevel)
        {
            XmlNode oNewNode = null;
            if (oNode != null && oXmlNodeTasks != null)
            {
                try
                {
                    oNewNode = CreateNodes(detail, iLevel);

                    oXmlNodeTasks.InsertAfter(oNewNode, oNode);
                }
                catch (System.Exception ex)
                {
                }
            }
            return oNewNode;
        }
        /// <summary>
        /// 作为该节点的孩子插到该节点后面
        /// </summary>
        /// <param name="oNode"></param>
        /// <param name="sParentID"></param>
        /// <param name="detail"></param>
        public XmlNode AfterInsertAsChild( XmlNode oNode1, ProductionScheduleDetail detail, int iLevel)
        {
            XmlNode oNewNode = null;
            XmlNode oNode = null;
            try
            {
                oNewNode = CreateNodes(detail, iLevel);
                if (oNode1 == null)
                {

                }
                //XmlNodeList oNodeList = FindNodeByParentID(sParentID);
                //if (oNodeList != null && oNodeList.Count > 0)
                //{
                //    oNode = oNodeList[oNodeList.Count - 1];
                //}
                oXmlNodeTasks.InsertAfter(oNewNode, oNode);
            }
            catch
            {

            }
            return oNewNode;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="sXmlPath"></param>
        public XmlElement  CreateNodes(  ProductionScheduleDetail detail,int iLevel)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(sXmlPath);
            //doc.SelectNodes (
            //<UID>2</UID>
            //<ID>2</ID>
            //<Name>武汉中心施工总承包工程</Name>
            //<Type>1</Type>
            //<IsNull>0</IsNull>
            //<CreateDate>2012-11-07T17:56:00</CreateDate>
            //<WBS>1.1</WBS>
            //<OutlineNumber>1.1</OutlineNumber>
            //<OutlineLevel>2</OutlineLevel>
            string sName = string.Empty;
            string sValue = string.Empty;

            XmlElement oTask = CreateNode(null, "Task", "");


           //名称
            sName = "Name";
            sValue = detail.GWBSTreeName == null ? "" : detail.GWBSTreeName + sNewTag;
           // CreateNode(oTask, sName, sValue+"New");
            CreateNode(oTask, sName, sValue );
            //记录
            sName = "Notes";
            sValue = (detail.ParentNode == null ? "" : detail.ParentNode.Id) + "|" + DateTimeToString(detail.PlannedBeginDate) + "|" + DateTimeToString(detail.PlannedEndDate); ;
            CreateNode(oTask, sName, sValue);


            //id
            sName = "Contact";
            sValue = detail.Id; ;
            CreateNode(oTask, sName, sValue);
            
            //
            //sName = "Start";
            //sValue = DateTimeToString(detail.PlannedBeginDate);
            //CreateNode(oTask, sName, sValue);

            //sName = "Finish";
            //sValue = DateTimeToString(detail.PlannedEndDate);
            //CreateNode(oTask, sName, sValue);

            sName = "OutlineLevel";
            sValue = (detail.Level - iLevel+1).ToString();
            CreateNode(oTask, sName, sValue);


            sName = "OutlineNumber";
            sValue = "";
            CreateNode(oTask, sName, sValue);


            sName = "WBS";
            sValue = detail.GWBSTreeName == null ? "1" : "2";
            CreateNode(oTask, sName, sValue);


            XmlAttribute att = oTask.Attributes["xmlns"];
            if (att != null)
            {
                oTask.Attributes.Remove(att);
            }
            return oTask;
        }
        public XmlElement CreateNode(XmlNode oParentNode, string sName, string sValue)
        {
             
             XmlElement oChildNode = m_doc.CreateElement(sName,XmlNameSpace);
            oChildNode.InnerText = sValue;
            if (oParentNode != null)
            {
                oParentNode.AppendChild(oChildNode);
            }
            return oChildNode;
        }
        public void UpdateNodes(XmlNode oNode, ProductionScheduleDetail detail,int iLevel)
        {
            if (oNode != null)
            {
                string sName =string.Empty ;
                string sValue=string.Empty ;

                sName ="Name";
                sValue = detail.GWBSTreeName;
                UpdateNode(oNode, sName, sValue );

                sName = "Notes";
                sValue = (detail.ParentNode == null ? "" : detail.ParentNode.Id) + "|" + DateTimeToString(detail.PlannedBeginDate) + "|" + DateTimeToString(detail.PlannedEndDate);
                UpdateNode(oNode, sName, sValue);

                sName = "Start";
                sValue = DateTimeToString(detail.PlannedBeginDate);
                UpdateNode(oNode, sName, sValue);

                sName = "Finish";
                sValue = DateTimeToString(detail.PlannedEndDate);
                UpdateNode(oNode, sName, sValue);

                  sName = "OutlineLevel";
                  sValue = (detail.Level - iLevel+1).ToString();
                UpdateNode(oNode, sName, sValue);

                sName = "OutlineNumber";
                sValue = "";
                UpdateNode(oNode, sName, sValue);
                sName = "WBS";
                sValue = detail.GWBSTreeName ==null ? "1":"2" ;
                UpdateNode(oNode, sName, sValue);

            }
        }
        public void UpdateNode(XmlNode oNode, string sName, string sValue)
        {
            XmlNamespaceManager msg = GetXmlNamespaceManager();
            sName = "d:" + sName;
           XmlNode oChildNode= oNode.SelectSingleNode(sName, msg);
          
           if (oChildNode != null)
           {
               oChildNode.InnerText = sValue;
           }
           else
           {
               CreateNode(oNode, sName, sValue);
           }
        }
        public string XmlNameSpace = "http://schemas.microsoft.com/project";
        public XmlNamespaceManager GetXmlNamespaceManager( )
        {
            XmlNamespaceManager msg = new XmlNamespaceManager(m_doc.NameTable);
            msg.AddNamespace("d", XmlNameSpace);
            return msg;
        }
        public XmlNode FindNodeByID(  string sID)
        {
            XmlNode oNode = null;
            //string sXmlPath = "d:Project/d:Tasks/d:Task[starts-with(d:Contact ,'{0}')]";
            string sXmlPath = "d:Project/d:Tasks/d:Task[d:Contact ='{0}']";
            sXmlPath = string.Format(sXmlPath, sID);
            XmlNamespaceManager msg = GetXmlNamespaceManager( );
            oNode = m_doc.SelectSingleNode(sXmlPath, msg);
            return oNode;
        }
        public XmlNodeList FindNodeByParentID(  string sParentID)
        {
            XmlNodeList oNodeList = null;
            string sXmlPath = "d:Project/d:Tasks/d:Task[starts-with(d:Notes ,'{0}')]";
            //string sXmlPath = "d:Project/d:Tasks/d:Task[d:Contact ='{0}']";
            sXmlPath = string.Format(sXmlPath, sParentID);
            XmlNamespaceManager msg = GetXmlNamespaceManager( );
            oNodeList = m_doc.SelectNodes (sXmlPath, msg);
            return oNodeList;
        }
        public XmlNode FindNodeRoot()
        {
            XmlNode oNode = null;
            //string sXmlPath = "d:Project/d:Tasks/d:Task[starts-with(d:Notes ,'{0}')]";
            string sXmlPath = "d:Project/d:Tasks/d:Task[d:Contact ='root']";
             
            XmlNamespaceManager msg = GetXmlNamespaceManager();
            oNode = m_doc.SelectSingleNode(sXmlPath, msg);
            return oNode;
        }
        public XmlNode FindNodeTasks()
        {
            XmlNode oNode = null;
            //string sXmlPath = "d:Project/d:Tasks/d:Task[starts-with(d:Notes ,'{0}')]";
            string sXmlPath = "d:Project/d:Tasks";

            XmlNamespaceManager msg = GetXmlNamespaceManager();
            oNode = m_doc.SelectSingleNode(sXmlPath, msg);
            return oNode;
        }
        public void Save()
        {
            m_doc.Save(sXmlPath);
        }
        public string DateTimeToString(DateTime oDate)
        {
            string sValue = string.Format(oDate.ToString("G"));
            return sValue;
        }
        public bool DeleteXmlFirstNode()
        {
            bool bFlag = false;
            try
            {

                XmlNode oNodeTasks = FindNodeTasks();
                XmlNamespaceManager msg = GetXmlNamespaceManager();
                XmlNode oNode = oNodeTasks.SelectSingleNode("d:Task", msg);
                oNodeTasks.RemoveChild(oNode);
                bFlag = true;
            }
            catch
            {
            }
            return bFlag;
        }
        public void SetAllNodeOldTag()
        {
            try
            {
                 

                XmlNamespaceManager msg = GetXmlNamespaceManager();
                XmlNode oNodeTasks = FindNodeTasks();
                XmlNodeList oNodeLst = oNodeTasks.SelectNodes("d:Task", msg);
                string sName = "d:Name";
              string sWBSName="d:WBS"; 
            
              XmlNode oNodeWBS = null;
             
                foreach (XmlNode oNode in oNodeLst)
                {

                    oNodeWBS = oNode.SelectSingleNode(sWBSName, msg);

                    if (oNodeWBS != null)
                    {
                        string sWBS = oNodeWBS.InnerText;

                        if (!string.Equals (sWBS,"1"))
                        {
                            XmlNode oChildNode = oNode.SelectSingleNode(sName, msg);
                            if (oChildNode != null)
                            {
                                oChildNode.InnerText = oChildNode.InnerText.Replace(sNewTag, "") + sDeleteTag;
                            }
                            else
                            {
                                CreateNode(oNode, sName, sDeleteTag);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
        public bool DeleteNameNullCalendar()
        {
            bool bFlag = false;
            try
            {

                
                XmlNamespaceManager msg = GetXmlNamespaceManager();
                string sXmlPath1 = "d:Project/d:Calendars";
                string sXmlPath2 = "d:Calendar";
                string sXmlPath3 = "d:Name";
                XmlNode oCalendars = m_doc.SelectSingleNode(sXmlPath1, msg);
                if (oCalendars != null)
                {
                    XmlNodeList oCalendarList = oCalendars.SelectNodes(sXmlPath2, msg);
                    if (oCalendarList != null && oCalendarList.Count > 0)
                    {
                        for (int i = oCalendarList.Count - 1; i > -1; i--)
                        {
                            XmlNode oNode = oCalendarList[i].SelectSingleNode(sXmlPath3, msg);
                            string sValue = oNode.InnerText;
                            if (oNode == null)
                            {
                                oCalendars.RemoveChild(oCalendarList[i]);
                            }
                        }
                    }
                }
                bFlag = true;
            }
            catch(System .Exception ex)
            {
            }
            return bFlag;
        }
        public XmlNodeList GetNodeTaskList()
        {
            XmlNodeList oNodeList = null;
            string sXmlPath = "d:Project/d:Tasks/d:Task";
            XmlNamespaceManager msg = GetXmlNamespaceManager();
            oNodeList = m_doc.SelectNodes (sXmlPath, msg);
            return oNodeList;
        }
        public XmlNode GetNode(XmlNode oXmlParentNode,string sName)
        {
            XmlNamespaceManager msg = GetXmlNamespaceManager();
            sName = "d:" + sName;
            XmlNode oChildNode = oXmlParentNode.SelectSingleNode(sName, msg);
            //XmlNode oNode = oXmlParentNode.SelectSingleNode(sXmlPath, msg);
            return oChildNode;
        }
        public string GetValue(XmlNode oXmlParentNode, string sName)
        {
            XmlNode oNode=GetNode(  oXmlParentNode,   sName);
            string sValue = string.Empty;
            if (oNode != null)
            {
                sValue = oNode.InnerText;
            }
            return sValue;
        }
        public List<ProductionScheduleDetail> GetSchedualDetail()
        {
            List<ProductionScheduleDetail> lstSchedual = new List<ProductionScheduleDetail>();
            XmlNodeList oTaskList=GetNodeTaskList();
            string sIDName = "Contact"; ;
            string sID = string.Empty;
            string sStartTimeName = "Start";
            DateTime  StartTime ;
            string sFinishTime = "Finish";
            DateTime FinishTime;
            string sTemp = string.Empty;
            string sDefaultTime = "1700-1-1";
            foreach (XmlNode oChildNode in oTaskList)
            {
                sTemp = GetValue(oChildNode, sIDName);
                sID = sTemp;
                if (!string.IsNullOrEmpty(sID))
                {
                    if (string.Equals(sID, "root"))
                    {
                        sID = "";
                    }
                    sTemp = GetValue(oChildNode, sStartTimeName);
                    if (string.IsNullOrEmpty(sTemp))
                    {
                        sTemp = sDefaultTime;
                    }
                    StartTime = DateTime.Parse(sTemp);

                    sTemp = GetValue(oChildNode, sFinishTime);
                    if (string.IsNullOrEmpty(sTemp))
                    {
                        sTemp = sDefaultTime;
                    }
                    FinishTime = DateTime.Parse(sTemp);
                    ProductionScheduleDetail oSchedual = new ProductionScheduleDetail();
                    oSchedual.Id = sID;
                    oSchedual.PlannedBeginDate = StartTime;
                    oSchedual.PlannedEndDate = FinishTime;
                    lstSchedual.Add(oSchedual);
                }
            }
            Save();
            return lstSchedual;
        }

        public void UpdateXmlTree(IList scheduleList,List<string > lstID)
        {
            XmlNode oNode = null;
            string sParentID = string.Empty;
            XmlNode oNode1 = null; 
            int iLevel=GetMinLevel(scheduleList,lstID);
            bool bFirst = true;
            this.SetAllNodeOldTag();
            DeleteXmlFirstNode();
         //   DeleteNameNullCalendar();
            foreach (ProductionScheduleDetail detail in scheduleList)
            {
                if (lstID != null && lstID.Count != 0 && detail.GWBSTreeName != null && (!string.IsNullOrEmpty(detail.Id)) && lstID.Contains(detail.Id))
                {
                    string sName = detail.GWBSTreeName;
                    oNode = FindNodeByID(detail.Id);//根据节点ID在Project中查找
                    if (oNode != null)//找到就更新
                    {
                        UpdateNodes(oNode, detail, iLevel);//更新project中该节点
                        oNode1 = oNode;
                    }
                    else//找父节点
                    {
                        sParentID = detail.ParentNode == null ? "" : detail.ParentNode.Id;//找到新树的父节点
                        oNode = FindNodeByID(sParentID);//查找该节点的父节点
                        if (oNode != null)
                        {
                           // oNode1 = AfterInsertAsChild(oNode, detail, iLevel);
                            //if (oNode1 != null)
                            //{
                            //    oNode1=AfterInsertAsChild( sParentID, detail,iLevel );
                            //}
                            //else
                            //{
                            //   oNode1= AfterInsertAsChild(  sParentID, detail,iLevel );//在父节点后面插入该节点  作为他的孩子
                            //    //AfterInsertXmlNode(oNode, detail);//在父节点后面插入该节点
                            //}
                            if (oNode1 == null)
                            {
                                oNode1 = AfterInsertXmlNode(oNode, detail, iLevel);//直接插到父节点的下
                            }
                            else
                            {
                                oNode1 = AfterInsertXmlNode(oNode1, detail, iLevel);//直接插到父节点的下
                            }
                        }
                        else//在为空就插到root节点（进度计划节点）下 这种情况实际只会发生一次
                        {
                            if (oNode1==null)
                            {
                                oNode = FindNodeRoot();//获取进度计划这个节点
                                bFirst = false;
                            }
                            else
                            {
                                
                                    oNode = oNode1;
                               
                                
                            }
                            if (oNode != null)
                            {
                                oNode1=AfterInsertXmlNode(oNode, detail,iLevel );//直接插到根节点后面
                            }
                        }
                    }
                }

            }
            Save();
        }
        public int GetMinLevel(IList scheduleList, List<string> lstID)
        {
            int iMin=int.MaxValue ;
            foreach (ProductionScheduleDetail detail in scheduleList)
            {
                if (lstID != null && lstID.Count != 0 && detail.GWBSTreeName != null && (!string.IsNullOrEmpty(detail.Id)) && lstID.Contains(detail.Id))
                {
                    if (detail.Level < iMin)
                    {
                        iMin = detail.Level;
                    }
                }
            }
            return iMin;
        }
       
    }
}
