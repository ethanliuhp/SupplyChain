using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSProject=Microsoft.Office.Interop.MSProject;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Component.Util;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;


namespace Application.Business.Erp.SupplyChain.Client.Util
{
    public class MSProjectUtil
    {
        public static List<ProductionScheduleDetail> ReadMPP(string fileName)
        {
            List<ProductionScheduleDetail> list = new List<ProductionScheduleDetail>();
            object missing = Type.Missing;
            MSProject.ApplicationClass prj = new MSProject.ApplicationClass();

            prj.FileOpen(fileName, true, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, MSProject.PjPoolOpen.pjPoolReadOnly, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            string sID = string.Empty;
            MSProject.Project proj = prj.ActiveProject;
            foreach (MSProject.Task task in proj.Tasks)
            {
                //ProductionScheduleDetail detail = new ProductionScheduleDetail();
                //detail.Id = task.Text1;
                //int duration = (int)((int)task.Duration / (proj.HoursPerDay * 60));
                //detail.PlannedDuration = duration;
                //detail.PlannedBeginDate = (DateTime)task.Start;
                //detail.PlannedEndDate = (DateTime)task.Finish;
                //list.Add(detail);
                sID = task.Contact;
                if (!string.IsNullOrEmpty(sID))
                {
                    if (string.Equals(sID, "root"))
                    {
                        sID = "";
                    }
                    ProductionScheduleDetail detail = new ProductionScheduleDetail();
                    detail.Id = sID;
                    int duration = (int)((int)task.Duration / (proj.HoursPerDay * 60));
                    detail.PlannedDuration = duration;
                    detail.PlannedBeginDate = (DateTime)task.Start;
                    detail.PlannedEndDate = (DateTime)task.Finish;
                    list.Add(detail);
                }

            }
            CloseProject(prj);
            return list;
        }
        /// <summary>
        /// 创建project应用程序
        /// </summary>
        /// <param name="Handle"></param>
        /// <returns></returns>
        public static Microsoft.Office.Interop.MSProject.ApplicationClass CreateAppPorject(IntPtr Handle)
        {
            Microsoft.Office.Interop.MSProject.ApplicationClass appProject = null;
            try
            {
                ShellExecute(Handle, "open", ProjectPath.GetPath(), null, null, (int)ShowWindowCommands.SW_HIDE);//shell调用
                Thread.Sleep(3000);//延时
                appProject = new MSProject.ApplicationClass();
            }
            catch
            {
                MessageBox.Show("无法打开project应用程序，检查是否安装project软件");
                appProject = null;
            }
            return appProject;
        }
        public static void  CloseProject( MSProject.ApplicationClass  oAppProject )
        {
            
            try
            {
                //AppProject.FileCloseAll (Microsoft.Office.Interop.MSProject.PjSaveType.pjDoNotSave );
                oAppProject.FileExit(Microsoft.Office.Interop.MSProject.PjSaveType.pjDoNotSave);
               // oAppProject.FileQuit(Microsoft.Office.Interop.MSProject.PjSaveType.pjDoNotSave);
                Thread.Sleep(1000);
            }
            catch
            {
                
            }
           
        }
        /// <summary>
        /// 打开一个project
        /// </summary>
        /// <param name="appProject"></param>
        /// <param name="sFilePath"></param>
        /// <param name="isReadOnly"></param>
        /// <returns></returns>
        public static bool OpenProejct(MSProject.Application appProject, string sFilePath, bool isReadOnly,bool visible)
        {
            bool bFlag = false;
            object oMissing=System .Type .Missing ;
            try
            {
                if (File.Exists(sFilePath))
                {
                    MSProject.PjPoolOpen openStyle = isReadOnly ? MSProject.PjPoolOpen.pjPoolReadOnly : MSProject.PjPoolOpen.pjPoolReadWrite;
                    appProject.FileOpen(sFilePath, isReadOnly, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, openStyle, oMissing, oMissing, oMissing, oMissing);
                    //appProject.Visible = visible;
                    bFlag = true ;
                }
                else
                {
                    bFlag = false;
                    MessageBox.Show("文件不存在");
                }
            }
            catch
            {
                bFlag = false;
            }
            return bFlag;

        }
        public static bool GetProjectFileXml(MSProject.Application appProject, string sXmlPath)
        {
            bool bFlag = false;
            object missing = System.Type.Missing;
            try
            {
                if (File.Exists(sXmlPath))
                {
                    File.Delete(sXmlPath);
                }
                // appProject .FileSaveAs (sXmlPath ,FileSaveAs(sXmlFilePath, Microsoft.Office.Interop.MSProject.PjFileFormat.pjMPP, missing, missing, missing, missing, missing, missing, missing, "MSProject.xml", missing, missing, missing, missing, missing, missing, missing, missing, missing);//导出xml
                appProject.FileSaveAs(sXmlPath, Microsoft.Office.Interop.MSProject.PjFileFormat.pjMPP, missing, missing, missing, missing, missing, missing, missing, "MSProject.xml", missing, missing, missing, missing, missing, missing, missing, missing, missing);
                bFlag = true;
            }
            catch
            {
                bFlag = false;
            }
            return bFlag;
        }
        public static string GetXmlPath(string sProjectPath)
        {
            string sXmlPath = string.Empty;
            if (sProjectPath.LastIndexOf(".") > 0)
            {
                sXmlPath = sProjectPath.Substring(0, sProjectPath.LastIndexOf("."));

            }
            sXmlPath += ".xml";
            return sXmlPath;
        }
        public static List<ProductionScheduleDetail> ReadMPPByXml(string sFilePath, IntPtr Handle)
        {
            List<ProductionScheduleDetail> list = new List<ProductionScheduleDetail>();
            object missing = Type.Missing;
            MSProject.ApplicationClass oAppProject = null;
            string sXmlPath = string.Empty;
            oAppProject= CreateAppPorject(Handle);
            if (OpenProejct(oAppProject, sFilePath, true, false))
            {
                sXmlPath = GetXmlPath(sFilePath);
                if (GetProjectFileXml(oAppProject, sXmlPath))
                {
                    UpdateProject update = new UpdateProject(sXmlPath);
                    list = update.GetSchedualDetail();
                    CloseProject(oAppProject);
                    if (File.Exists(sXmlPath))
                    {
                        File.Delete(sXmlPath);
                    }
                }
            }

           
             return list;
        }
        public void UpdateMPP(string fileName, IList scheduleList)
        {
            object missing = Type.Missing;
            MSProject.ApplicationClass prj = new MSProject.ApplicationClass();
            prj.FileOpen(fileName, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, MSProject.PjPoolOpen.pjPoolReadWrite, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            MSProject.Project proj = prj.ActiveProject;
            foreach (ProductionScheduleDetail detail in scheduleList)
            {
                MSProject.Task task = FindTask(proj, detail.Id);
                if (task != null)
                {
                    if (task.OutlineChildren.Count == 0)
                    {
                        //只能修改最明细的任务
                        UpdateTask(task, detail);
                    }
                }
                else
                {
                    //新增的任务，查找任务的父任务
                    MSProject.Task parentTask = FindTask(proj, detail.ParentNode.Id);
                    if (parentTask != null)
                    {
                        //如果父任务不为空,如果父任务的的子任务为空，则则插入到父任务的下一个任务的前面
                        int parentTaskIndex = GetTaskIndex(proj, parentTask);
                        if (parentTask.OutlineChildren == null || parentTask.OutlineChildren.Count == 0)
                        {
                            InsertTask(proj, detail, parentTaskIndex + 1);
                        }
                        else
                        { 
                            //父任务的的子任务不为空,则按进度计划的orderNo顺序插入
                            int orderNoIndex = GetTaskIndex(parentTask.OutlineChildren, detail);
                            if (orderNoIndex <= proj.Tasks.Count)
                            {
                                InsertTask(proj, detail, orderNoIndex);
                            }
                            else
                            {
                                InsertTask(proj, detail, Type.Missing);
                            }
                        }
                        
                    }
                    else
                    { 
                        //父任务为空，则直接插入
                        InsertTask(proj, detail, Type.Missing);
                    }
                }
            }
            prj.FileSave();
        }

        /// <summary>
        /// 进度计划的orderNo顺序,查找任务的插入位置
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        private int GetTaskIndex(MSProject.Tasks tasks, ProductionScheduleDetail detail)
        {
            int result = 0;
            MSProject.Task task = null;
            if (tasks.Count == 1)
            {
                task = tasks[1];
                int taskIndex = GetTaskIndex((MSProject.Project)task.Parent, task);
                if (int.Parse(task.Text2) >= detail.OrderNo)
                {
                    return taskIndex;
                }
                else
                {
                    return taskIndex + 1;
                }
            }
            else
            {
                for (int j = 1; j < tasks.Count; j++)
                {
                    MSProject.Task fisrt = tasks[j];
                    MSProject.Task second = tasks[j+1];
                    if (detail.OrderNo >= int.Parse(fisrt.Text2) && detail.OrderNo <= int.Parse(second.Text2))
                    {
                        result = GetTaskIndex((MSProject.Project)second.Parent, second);
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 查询任务在任务列表中的索引序号
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="task4search"></param>
        /// <returns></returns>
        private int GetTaskIndex(MSProject.Project proj, MSProject.Task task4search)
        {
            int result=0;
            for (int i = 1; i <= proj.Tasks.Count; i++)
            {
                MSProject.Task task = proj.Tasks[i];
                if (task.Guid == task4search.Guid)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }
        
        /// <summary>
        /// 插入一个新任务
        /// </summary>
        /// <param name="proj">项目</param>
        /// <param name="detail">进度计划明细</param>
        /// <param name="before">任务在任务列表中的的索引</param>
        private void InsertTask(MSProject.Project proj, ProductionScheduleDetail detail, object before)
        {
            MSProject.Task task = null;
            task = proj.Tasks.Add(detail.GWBSTreeName, before);

            if (detail.PlannedBeginDate != new DateTime(1900, 1, 1))
            {
                DateTime dtStart = new DateTime(detail.PlannedBeginDate.Year, detail.PlannedBeginDate.Month, detail.PlannedBeginDate.Day, ((DateTime)((MSProject.Project)task.Parent).DefaultStartTime).Hour, 0, 0);
                task.Start = dtStart;

            }
            if (detail.PlannedEndDate != new DateTime(1900, 1, 1))
            {
                DateTime dtFinish = new DateTime(detail.PlannedEndDate.Year, detail.PlannedEndDate.Month, detail.PlannedEndDate.Day, ((DateTime)((MSProject.Project)task.Parent).DefaultFinishTime).Hour, 0, 0);
                task.Finish = dtFinish;
            }
            task.Text1 = detail.Id;
            task.Text2 = ClientUtil.ToString(detail.OrderNo);
            task.OutlineLevel = (short)detail.Level;
        }

        private void UpdateTask(MSProject.Task task,ProductionScheduleDetail detail)
        {
            if (detail.PlannedBeginDate != new DateTime(1900, 1, 1))
            {
                DateTime dtStart = new DateTime(detail.PlannedBeginDate.Year, detail.PlannedBeginDate.Month, detail.PlannedBeginDate.Day, ((DateTime)((MSProject.Project)task.Parent).DefaultStartTime).Hour, 0, 0);
                //task.SetField(MSProject.PjField.pjTaskStart, dtStart.ToLongDateString());
                task.Start = dtStart;

            }
            if (detail.PlannedEndDate != new DateTime(1900, 1, 1))
            {
                DateTime dtFinish = new DateTime(detail.PlannedEndDate.Year, detail.PlannedEndDate.Month, detail.PlannedEndDate.Day, ((DateTime)((MSProject.Project)task.Parent).DefaultFinishTime).Hour, 0, 0);
                task.Finish = dtFinish;
            }
        }

        /// <summary>
        /// 根据进度计划明细Id查找project任务
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="scheduleDetailid"></param>
        /// <returns></returns>
        private MSProject.Task FindTask(MSProject.Project proj,string scheduleDetailid)
        {
            foreach (MSProject.Task task in proj.Tasks)
            {
                if (task.Text1 == scheduleDetailid) return task;
            }
            return null;
        }
        public static void CreateMPP(string fileName, IList scheduleList)
        {
            object missing = Type.Missing;
            MSProject.ApplicationClass prj = new MSProject.ApplicationClass();

            prj.Visible = true;
            prj.FileNew(Type.Missing, Type.Missing, Type.Missing, false);
            MSProject.Project myProject = prj.ActiveProject;

            if (scheduleList != null && scheduleList.Count > 0)
            {
                foreach (ProductionScheduleDetail detail in scheduleList)
                {
                    if (detail.State == EnumScheduleDetailState.失效)
                    {
                        continue;
                    }
                    string taskName = (detail.GWBSTreeName == null) ? "进度计划" : detail.GWBSTreeName;
                    MSProject.Task task = null;
                    task = myProject.Tasks.Add(taskName, missing);
                    //task.SetField(Microsoft.Office.Interop.MSProject.PjField.
                    if (detail.PlannedBeginDate != new DateTime(1900, 1, 1))
                    {
                        DateTime dtStart = new DateTime(detail.PlannedBeginDate.Year, detail.PlannedBeginDate.Month, detail.PlannedBeginDate.Day, ((DateTime)myProject.DefaultStartTime).Hour, 0, 0);
                        task.Start = dtStart;

                    }
                    if (detail.PlannedEndDate != new DateTime(1900, 1, 1))
                    {
                        DateTime dtFinish = new DateTime(detail.PlannedEndDate.Year, detail.PlannedEndDate.Month, detail.PlannedEndDate.Day, ((DateTime)myProject.DefaultFinishTime).Hour, 0, 0);
                        task.Finish = dtFinish;
                    }
                    //task.Duration = detail.PlannedDuration;
                    //task.SetField(MSProject.PjField.pjTaskGuid, detail.Id);
                    task.Text1 = detail.Id;
                    task.Text2 = ClientUtil.ToString(detail.OrderNo);
                    task.OutlineLevel = (short)detail.Level;
                }
            }

            prj.FileSaveAs(fileName,
                MSProject.PjFileFormat.pjMPP, missing, false, missing, missing, missing, missing,
                missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
        }
        public static   void CreateMPP(string fileName, IList scheduleList,IntPtr Handle)
        {
           

            ExportTask(scheduleList, fileName, Handle);
           
        }
        public static void CreateMPP(string fileName, IList scheduleList,List<string> lstID, IntPtr Handle)
        {
             
                ExportTask(scheduleList,lstID, fileName, Handle);

        }
        [DllImport("shell32.dll")]
        public extern static IntPtr ShellExecute(IntPtr hwnd,
                                                 string lpOperation,
                                                 string lpFile,
                                                 string lpParameters,
                                                 string lpDirectory,
                                                 int nShowCmd
                                                );
        public enum ShowWindowCommands : int
        {

            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_MAX = 10
        }

        /// <summary>
        /// 导出task集合
        /// </summary>
        /// <param name="scheduleList">需要导出的任务集合</param>
        /// <param name="sFilePath">存放路径</param>
        public static void ExportTask(IList scheduleList, string sFilePath, IntPtr Handle)
        {
            object missing = Type.Missing;
            string sXmlFilePath = @"d:\\temp\\1.xml";
            string sID = string.Empty;
            string Level = string.Empty;
            int iCount = 0;

            try
            {
                //获取xml路径
                if (sFilePath.LastIndexOf(".") > 0)
                {
                    sXmlFilePath = sFilePath.Substring(0, sFilePath.LastIndexOf(".")) + ".xml";
                }
                else
                {
                    sXmlFilePath = sFilePath + ".xml";
                }
                if (File.Exists(sXmlFilePath))//删除原有的xml文件
                {
                    File.Delete(sXmlFilePath);
                }
                if (scheduleList != null && scheduleList.Count > 0)
                {
                    #region 创建task集合
                    //string sMsg = string.Empty;
                    //for (int i = 0; i < 5; i++)
                    //{
                    //    sMsg += "1111111111111111111111111111111111111111";
                    //}
                    Project2007.ProjectTask[] arrTask = new Project2007.ProjectTask[scheduleList.Count];
                    string sParentID = string.Empty;
                    foreach (ProductionScheduleDetail detail in scheduleList)
                    {
                        if (detail.State != EnumScheduleDetailState.失效)
                        {
                            Project2007.ProjectTask task = new Project2007.ProjectTask();
                            task.Name = (detail.GWBSTreeName == null) ? "进度计划" : detail.GWBSTreeName;
                            task.Start111 = detail.PlannedBeginDate;
                            task.Finish111 = detail.PlannedEndDate;
                            task.ID = detail.Id;
                            task.OutlineLevel = detail.Level.ToString ();
                            sParentID = detail.ParentNode == null ? "" : detail.ParentNode.Id;
                            task.Notes = sParentID + "|" + string.Format(task.Start111.ToString("G")) + "|" + string.Format(task.Finish111.ToString("G")); ;
                            task.Contact =detail.GWBSTreeName == null? "root": detail.Id;
                           
                            //task.UID = detail.Id;
                            arrTask[iCount++] = task;
                           
                        }
                    }
                    #endregion
                    #region 创建一个project2007的类
                    Project2007.Project p = new Project2007.Project();
                    p.Tasks = arrTask;
                    XmlSerializer xs = new XmlSerializer(typeof(Project2007.Project));

                    Stream stream = new FileStream(sXmlFilePath, FileMode.OpenOrCreate);
                    xs.Serialize(stream, p);//序列化
                    stream.Close();
                    #endregion
                    #region 创建project
                    Microsoft.Office.Interop.MSProject.Application appProject = null;
                     //try
                     //{
                     //    appProject = new Microsoft.Office.Interop.MSProject.Application();
                     //}
                     //catch {
                     //    ShellExecute(Handle, "open", @"C:\Program Files\Microsoft Office\Office12\WINPROJ.EXE", null, null, (int)ShowWindowCommands.SW_SHOW);

                     //    Thread.Sleep(3000);
                     //    appProject = new Microsoft.Office.Interop.MSProject.Application();
                     //}
                    ShellExecute(Handle, "open", ProjectPath.GetPath (), null, null, (int)ShowWindowCommands.SW_SHOW);

                    Thread.Sleep(3000);
                    appProject = new Microsoft.Office.Interop.MSProject.Application();
                    appProject.Visible = true;
                    appProject.FileNew(missing, missing, missing, false);//新建一个project
                    appProject.FileOpen(sXmlFilePath, true, Microsoft.Office.Interop.MSProject.PjMergeType.pjAppend, missing, missing, missing, missing, missing, missing, "MSProject.mpp", missing, Microsoft.Office.Interop.MSProject.PjPoolOpen.pjPoolReadWrite, missing, missing, false, missing);//建数据加载到文件中

                    ///
                  ///  appProject.FileSaveAs(@"C:\Documents and Settings\Administrator\桌面\db\xml1.xml", Microsoft.Office.Interop.MSProject.PjFileFormat.pjXLS, missing, missing, missing, missing, missing, missing, missing, "MSProject.xml", missing, missing, missing, missing, missing, missing, missing, missing, missing);
                   // appProject.FileSaveAs(@"C:\Documents and Settings\Administrator\桌面\db\xml2.xml", Microsoft.Office.Interop.MSProject.PjFileFormat.pjTXT , missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                    ///
                    appProject.FileSaveAs(sFilePath, Microsoft.Office.Interop.MSProject.PjFileFormat.pjMPP, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);//保存
                    #endregion
                    if (File.Exists(sXmlFilePath))//删除原有的xml文件
                    {
                        File.Delete(sXmlFilePath);
                    }
                }

            }
            catch (System.Exception ex)
            {
            }
        }
        public static void UpdateProject(string sFilePath, IList scheduleList, List<string> lstID, IntPtr Handle)
        {
            try
            {
                if (File.Exists(sFilePath))
                {
                    object missing = Type.Missing;
                    string sXmlFilePath = @"d:\\temp\\1.xml";
                    string sDestFilePath = string.Empty;
                    string sID = string.Empty;
                    string Level = string.Empty;
                    int iCount = 0;
                    Microsoft.Office.Interop.MSProject.Application appProject = null;
                    //获取xml路径
                    if (sFilePath.LastIndexOf(".") > 0)
                    {
                        sXmlFilePath = sFilePath.Substring(0, sFilePath.LastIndexOf(".")) + ".xml";
                    }
                    else
                    {
                        sXmlFilePath = sFilePath + ".xml";
                    }
                    if (File.Exists(sXmlFilePath))//删除原有的xml文件
                    {
                        File.Delete(sXmlFilePath);
                    }
                   


                    ShellExecute(Handle, "open", ProjectPath.GetPath(), null, null, (int)ShowWindowCommands.SW_SHOW);

                    Thread.Sleep(3000);
                    appProject = new Microsoft.Office.Interop.MSProject.Application();//创建一个进程
                    appProject.FileOpen(sFilePath, false , missing, missing, missing, missing, missing, missing, missing,
                missing, missing, MSProject.PjPoolOpen.pjPoolReadWrite , missing, missing, missing, missing);//打开project文件
                    appProject.Visible = false;//隐藏project
                    appProject.FileSaveAs(sXmlFilePath, Microsoft.Office.Interop.MSProject.PjFileFormat.pjMPP, missing, missing, missing, missing, missing, missing, missing, "MSProject.xml", missing, missing, missing, missing, missing, missing, missing, missing, missing);//导出xml

                    appProject.FileClose(Microsoft.Office.Interop.MSProject.PjSaveType.pjSave, missing);//关闭当前文件

                    UpdateProject update = new UpdateProject(sXmlFilePath);
                    update.UpdateXmlTree(scheduleList, lstID);//修改project文件数据文件xml
                    Thread.Sleep(3000);//延时为了删除源文件做准备

                    File.Delete(sFilePath);//删除源文件



                    appProject.FileNew(missing, missing, missing, false);//新建一个project
                    appProject.Visible = true;//显示project
                    appProject.FileOpen(sXmlFilePath, false, Microsoft.Office.Interop.MSProject.PjMergeType.pjAppend, missing, missing, missing, missing, missing, missing, "MSProject.mpp", missing, Microsoft.Office.Interop.MSProject.PjPoolOpen.pjPoolReadWrite, missing, missing, true, missing);//加载xml
                    appProject.FileSaveAs(sFilePath, Microsoft.Office.Interop.MSProject.PjFileFormat.pjMPP, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);//保存新的project文件
                    File.Delete(sXmlFilePath);//删除xml
                    //MSProject.Project myProject = appProject.ActiveProject;//删除原来数据
                    //appProject.Visible = false;
                    //for (int i = myProject.Tasks.Count; i > 0; i--)
                    //{
                    //    myProject.Tasks[i].Delete();
                   

                    //}
                    //for (int i = myProject.TaskTables.Count; i > 0; i--)
                    //{
                    //    myProject.TaskTables[i].Delete();
                    //    myProject.TaskTables[i].Delete();
                    //    myProject .TaskTables.Add (
                    //}
                     
                 
                    //appProject.FileSaveAs(sDestFilePath, Microsoft.Office.Interop.MSProject.PjFileFormat.pjMPP, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);//保存
                   
                  ////  appProject.FileQuit(Microsoft.Office.Interop.MSProject.PjSaveType.pjDoNotSave);
                    //app.FileSaveAs(sFileAsSavePath, Microsoft.Office.Interop.MSProject.PjFileFormat.pjMPP, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);//保存
                    

                    //ShellExecute(Handle, "open", ProjectPath.GetPath(), null, null, (int)ShowWindowCommands.SW_SHOW);

                    ////Thread.Sleep(3000);

                    ////File.Delete(sFilePath);

                    ////File.Copy(sDestFilePath, sFilePath);
                    ////File.Delete(sDestFilePath);
                   
                    


                //appProject = new Microsoft.Office.Interop.MSProject.Application();//创建一个进程
                //    appProject.FileOpen(sFilePath, false, missing, missing, missing, missing, missing, missing, missing,
                //missing, missing, MSProject.PjPoolOpen.pjPoolReadWrite, missing, missing, missing, missing);//打开project文件
                //    appProject.Visible = true;
                }
            }
            catch(System .Exception ex)
            {
                MessageBox.Show("错误提示："+ex.Message);
            }
        }
        public static void ExportTask(IList scheduleList,List<string> lstID, string sFilePath, IntPtr Handle)
        {
            object missing = Type.Missing;
            string sXmlFilePath = @"d:\\temp\\1.xml";
            string sID = string.Empty;
            string Level = string.Empty;
            int iCount = 0;
            string sNewTag = "(最新)";
            try
            {
                //获取xml路径
                if (sFilePath.LastIndexOf(".") > 0)
                {
                    sXmlFilePath = sFilePath.Substring(0, sFilePath.LastIndexOf(".")) + ".xml";
                }
                else
                {
                    sXmlFilePath = sFilePath + ".xml";
                }
                if (File.Exists(sXmlFilePath))//删除原有的xml文件
                {
                    File.Delete(sXmlFilePath);
                }
                if (scheduleList != null && scheduleList.Count > 0)
                {
                    #region 创建task集合
                    //string sMsg = string.Empty;
                    //for (int i = 0; i < 5; i++)
                    //{
                    //    sMsg += "1111111111111111111111111111111111111111";
                    //}
                    Project2007.ProjectTask[] arrTask = new Project2007.ProjectTask[scheduleList.Count];
                    string sParentID = string.Empty;

                    if (scheduleList[0].GetType() == typeof(ProductionScheduleDetail))
                    {
                        foreach (ProductionScheduleDetail detail in scheduleList)
                        {
                            if (detail.State != EnumScheduleDetailState.失效 && lstID.Contains(detail.Id))
                            {
                                Project2007.ProjectTask task = new Project2007.ProjectTask();
                                task.Name = (detail.GWBSTreeName == null) ? "进度计划" : detail.GWBSTreeName + sNewTag;
                                task.Start111 = detail.PlannedBeginDate;
                                task.Finish111 = detail.PlannedEndDate;
                                task.ID = detail.Id;
                                task.OutlineLevel = detail.Level.ToString();
                                sParentID = detail.ParentNode == null ? "" : detail.ParentNode.Id;
                                task.Notes = sParentID + "|" + string.Format(task.Start111.ToString("G")) + "|" + string.Format(task.Finish111.ToString("G")); ;
                                // task.Notes = sParentID + "|" + "" + "|" + ""; 
                                task.Contact = detail.GWBSTreeName == null ? "root" : detail.Id;
                                task.WBS = detail.GWBSTreeName == null ? "1" : "2";
                                //task.UID = detail.Id;
                                arrTask[iCount++] = task;

                            }
                        }
                    }
                    else if (scheduleList[0].GetType() == typeof(WeekScheduleDetail))
                    {
                        foreach (WeekScheduleDetail detail in scheduleList)
                        {
                            if ( lstID.Contains(detail.Id))
                            {
                                Project2007.ProjectTask task = new Project2007.ProjectTask();
                                task.Name = (detail.GWBSTreeName == null) ? "进度计划" : detail.GWBSTreeName + sNewTag;
                                task.Start111 = detail.PlannedBeginDate;
                                task.Finish111 = detail.PlannedEndDate;
                                task.ID = detail.Id;
                                task.OutlineLevel = detail.Level.ToString();
                                sParentID = detail.ParentNode == null ? "" : detail.ParentNode.Id;
                                task.Notes = sParentID + "|" + string.Format(task.Start111.ToString("G")) + "|" + string.Format(task.Finish111.ToString("G")); ;
                                // task.Notes = sParentID + "|" + "" + "|" + ""; 
                                task.Contact = detail.GWBSTreeName == null ? "root" : detail.Id;
                                task.WBS = detail.GWBSTreeName == null ? "1" : "2";
                                //task.UID = detail.Id;
                                arrTask[iCount++] = task;

                            }
                        }
                    
                    }
                    #endregion
                    #region 创建一个project2007的类
                    Project2007.Project p = new Project2007.Project();
                    p.Tasks = arrTask;
                    XmlSerializer xs = new XmlSerializer(typeof(Project2007.Project));

                    Stream stream = new FileStream(sXmlFilePath, FileMode.OpenOrCreate);
                    xs.Serialize(stream, p);//序列化
                    stream.Close();
                    #endregion
                    #region 创建project
                    Microsoft.Office.Interop.MSProject.Application appProject = null;
                    //try
                    //{
                    //    appProject = new Microsoft.Office.Interop.MSProject.Application();
                    //}
                    //catch {
                    //    ShellExecute(Handle, "open", @"C:\Program Files\Microsoft Office\Office12\WINPROJ.EXE", null, null, (int)ShowWindowCommands.SW_SHOW);

                    //    Thread.Sleep(3000);
                    //    appProject = new Microsoft.Office.Interop.MSProject.Application();
                    //}
                    ShellExecute(Handle, "open", ProjectPath.GetPath(), null, null, (int)ShowWindowCommands.SW_SHOW);

                    Thread.Sleep(3000);
                    appProject = new Microsoft.Office.Interop.MSProject.Application();
                    appProject.Visible = true;
                    appProject.FileNew(missing, missing, missing, false);//新建一个project
                    appProject.FileOpen(sXmlFilePath, true, Microsoft.Office.Interop.MSProject.PjMergeType.pjAppend, missing, missing, missing, missing, missing, missing, "MSProject.mpp", missing, Microsoft.Office.Interop.MSProject.PjPoolOpen.pjPoolReadWrite, missing, missing, false, missing);//建数据加载到文件中

                    ///
                    ///  appProject.FileSaveAs(@"C:\Documents and Settings\Administrator\桌面\db\xml1.xml", Microsoft.Office.Interop.MSProject.PjFileFormat.pjXLS, missing, missing, missing, missing, missing, missing, missing, "MSProject.xml", missing, missing, missing, missing, missing, missing, missing, missing, missing);
                    // appProject.FileSaveAs(@"C:\Documents and Settings\Administrator\桌面\db\xml2.xml", Microsoft.Office.Interop.MSProject.PjFileFormat.pjTXT , missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                    ///
                    appProject.FileSaveAs(sFilePath, Microsoft.Office.Interop.MSProject.PjFileFormat.pjMPP, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);//保存
                    #endregion
                    if (File.Exists(sXmlFilePath))//删除原有的xml文件
                    {
                        File.Delete(sXmlFilePath);
                    }
                }

            }
            catch (System.Exception ex)
            {
            }
        }
        public DateTime GetDate(MSProject.Project oProject, DateTime time)
        {
            DateTime ConstDateTime = DateTime.Parse("1984-1-1");
            if (time >= ConstDateTime)
            {
                time = new DateTime(time.Year, time.Month, time.Day, ((DateTime)oProject.DefaultStartTime).Hour, 0, 0);
            }
            {
                time = ConstDateTime;
            }
            return time;
        }
        public static void CreateTask(MSProject.Project  oProject, ProductionScheduleDetail detail, string sName)
        {
            if (detail != null)
            {
                if (detail.State == EnumScheduleDetailState.失效)
                {
                }
                else
                {
                    DateTime startTime = detail.PlannedBeginDate;
                    DateTime endTime = detail.PlannedEndDate;
                    string sStartTime = string.Empty;
                    string sEndTime = string.Empty;
                    string sID = string.Empty;
                    string sOrderNo = string.Empty;
                    int iDuration = 0;
                    int iLevel = 0;
                    object missing = System.Type.Missing;
                    if (startTime != new DateTime(1900, 1, 1))
                    {
                        startTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, ((DateTime)oProject.DefaultStartTime).Hour, 0, 0);
                    }
                  
                    if (endTime != new DateTime(1900, 1, 1))
                    {
                        endTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, ((DateTime)oProject.DefaultStartTime).Hour, 0, 0);   
                    }
                    sID = detail.Id;//Text1
                    sOrderNo = ClientUtil.ToString(detail.OrderNo);//Text2
                   iDuration = (endTime - startTime).Days + 1;
                   iLevel = (short)detail.Level;

                   
                   //Test(sName As String, startTime As Date, endTime As Date, iDuration As Integer, iLevel As Integer,sID as string ,sOrderNo as string )
                   //oProject.Application.Run("Test", sName, startTime.ToShortDateString(), endTime.ToShortDateString(), iDuration.ToString(), iLevel.ToString(), sID, sOrderNo, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);

                   //sName As String, sID As String, startTime As String, endTime As String, iLevel As String
                  // oProject.Application.Run("Test", sName, sID, iLevel.ToString(), missing, missing, missing,  missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                   oProject.Application.Run("Test1", sName, sID, DateTime.Now.AddDays(-1).ToString("d"), DateTime.Now.ToString("d"), iLevel.ToString(),   missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);


                  //oProject.Application.Run("Test", sName, sID ,iLevel, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing); 
                  // oProject.GetType().InvokeMember("Test", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null, oProject, new object[] { sName });

                   //oProject.Application.Run("Test", sName, sID, iLevel.ToString(), missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                 return;
                }
                
            }
        }



        #region 文档上传
        /// <summary>
        /// 文档上传
        /// </summary>
        /// <param name="OptProjectInfo">项目信息</param>
        /// <param name="sMasterID">单据的ID</param>
        /// <param name="sMasterType">单据类型</param>
        /// <param name="sUserName">用户名</param>
        /// <param name="sJobId">JobID</param>
        /// <param name="sPathFile">文件路径</param>
        /// <returns></returns>
        public static ProObjectRelaDocument UpLoadFile(CurrentProjectInfo OptProjectInfo, string sMasterID, string sMasterType, string sUserName, string sJobId, string sPathFile)
        {
            #region 上传文件
            //ProductionScheduleMaster oMaster,
            //CurrentProjectInfo OptProjectInfo = projectInfo;
            IList listDoc = new List<ProObjectRelaDocument>();
            List<PLMWebServices.ProjectDocument> docList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
            PLMWebServices.ProjectDocument[] resultList = null;
            PLMWebServices.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument();
            FileInfo file = new FileInfo(sPathFile);


            FileStream fileStream = file.OpenRead();
            int FileLen = (int)file.Length;
            Byte[] FileData = new Byte[FileLen];
            ////将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
            fileStream.Read(FileData, 0, FileLen);
            if (FileData.Length == 0)
            {
                MessageBox.Show("该文件长度为0,请检查!");
                return null;
            }
            doc.ExtendName = Path.GetExtension(sPathFile); //文档扩展名*******************************
            doc.FileDataByte = FileData; //文件二进制流
            doc.FileName = file.Name;//文件名称

            doc.ProjectCode = OptProjectInfo.Code; //所属项目代码*
            doc.ProjectName = OptProjectInfo.Name; //所属项目名称*

            List<string> listDocParam = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
            string docObjTypeName = listDocParam[0];
            string docCateLinkTypeName = listDocParam[2];

            doc.CategoryRelaDocType = docCateLinkTypeName;//文档分类类型
            doc.ObjectTypeName = docObjTypeName;//文档对象类型

            doc.Category = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.CategoryNode();
            doc.Category.CategoryCode = string.Empty;//row.Cells[BrownFileCateCode.Name].Value.ToString();//"CSFL";//文档分类代码
            doc.Category.CategoryName = string.Empty;// row.Cells[BrownFileCateName.Name].Value.ToString();//"测试分类"; //文档分类名称

            PLMWebServices.DocumentInfoType docInfoType = 0;
            foreach (PLMWebServices.DocumentInfoType type in Enum.GetValues(typeof(PLMWebServices.DocumentInfoType)))
            {
                //question
                if (type.ToString() == "文本")
                {
                    docInfoType = type;
                    break;
                }
            }

            PLMWebServices.DocumentState docState = 0;
            foreach (PLMWebServices.DocumentState state in Enum.GetValues(typeof(PLMWebServices.DocumentState)))
            {
                //question
                if (state.ToString() == "编制")
                {
                    docState = state;
                    break;
                }
            }
            doc.DocType = docInfoType;//文档信息类型
            doc.State = docState;//文档状态

            //doc.Code = row.Cells[BrownFileCode.Name].Value.ToString();//文档代码
            //doc.Description = row.Cells[BrownFileDesc.Name].Value.ToString();//文档说明
            //doc.KeyWords = row.Cells[BrownFileKeyWord.Name].Value.ToString();//文档关键字
            doc.Name = doc.FileName.Replace(doc.ExtendName, "");//文档名称
            //doc.Title = row.Cells[BrownFileTitle.Name].Value.ToString(); ;//文档标题
            //doc.Author = row.Cells[BrownFileAuthor.Name].Value.ToString();//文档作者*
            //doc.CategorySysCode = "";//文档分类层次码
            //string docObjTypeName= StaticMethod.GetMBPUploadFileParams()[0]; 
            //doc.OwnerID = "";//责任人
            //doc.OwnerName = "";//责任人名称
            //doc.OwnerOrgSysCode = "";// 责任人组织层次码
            //doc.Revision = "";//文档版次 
            //doc.Version = "";//文档版本
            //doc.ExtendInfoNames = "";//扩展属性名
            //doc.ExtendInfoValues = "";//扩展属性值
            docList.Add(doc);

            PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.AddDocumentByIRP(docList.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentSaveMode.一个文件生成一个文档对象,
                null, sUserName, sJobId, null, out resultList);
            if (es != null)
            {
                MessageBox.Show("文件“" + file.Name + "”上传到服务器失败，异常信息：" + es.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }
            fileStream.Close();
            #endregion 上传文件

            #region 保存MBP对象关联文档信息
            if (resultList != null)
            {
                listDoc.Clear();
                string fileId = resultList[0].ID;
                ProObjectRelaDocument rdoc = new ProObjectRelaDocument();

                rdoc.TheProjectGUID = OptProjectInfo.Id;
                rdoc.TheProjectName = OptProjectInfo.Name;
                rdoc.TheProjectCode = OptProjectInfo.Code;

                string projectObjectName = sMasterType;
                string projectObjectGUID = sMasterID;
                if (projectObjectName != null && projectObjectName != "")
                {
                    rdoc.ProObjectName = projectObjectName;
                    rdoc.ProObjectGUID = projectObjectGUID;
                }
                else
                {
                    rdoc.ProObjectName = OptProjectInfo.Name;
                    rdoc.ProObjectGUID = OptProjectInfo.Id;
                }

                rdoc.DocumentOwner = ConstObject.LoginPersonInfo;
                rdoc.DocumentOwnerName = ConstObject.LoginPersonInfo.Name;
                rdoc.DocumentGUID = resultList[0].EntityID;
                rdoc.DocumentName = resultList[0].Name;
                rdoc.DocumentDesc = resultList[0].Description;
                rdoc.DocumentCode = resultList[0].Code;
                rdoc.SubmitTime = resultList[0].UpdateTime;
                if (resultList[0].Category != null)
                {
                    rdoc.DocumentCateCode = resultList[0].Category.CategoryCode;
                    rdoc.DocumentCateName = resultList[0].Category.CategoryName;
                }
                rdoc.FileURL = sPathFile;

                listDoc.Add(rdoc);
                MPBSTree model = new MPBSTree();
                IList result = model.SaveOrUpdate(listDoc);
                ProObjectRelaDocument pord = result[0] as ProObjectRelaDocument;
                // addResultDocList.Add(pord);
                if (pord == null) return null;
                else
                {
                    return pord;
                }


                //int rowIndex = gridDocument.Rows.Add();
                //gridDocument[DocumentName.Name, rowIndex].Value = pord.DocumentName;
                //gridDocument[DocumentCateCode.Name, rowIndex].Value = pord.DocumentCateCode;
                //gridDocument[DocumentCateName.Name, rowIndex].Value = pord.DocumentCateName;
                //gridDocument[DocumentCode.Name, rowIndex].Value = pord.DocumentCode;
                //gridDocument[DocumentDesc.Name, rowIndex].Value = pord.DocumentDesc;
                //gridDocument.Rows[rowIndex].Tag = pord;

            }
            else
            {
                return null;
            }
            #endregion 保存MBP对象关联文档信息
        }
        #endregion
        //public static MSProject.ApplicationClass CreateProject()
        //{
        //    MSProject.ApplicationClass prj = null;
        //    int iCount = 0;
        //    int iTotal = 10;
        //    while (true)
        //    {
        //        try
        //        {
        //            prj = new MSProject.ApplicationClass();
        //            break;
        //        }
        //        catch
        //        {
        //            iCount++;
        //            if (iCount >= iTotal)
        //            {
        //                prj = null;
        //                System.Windows.Forms.MessageBox.Show("com 端口出错");
        //                break;
        //            }
        //        }
        //    }
        //    return prj;
        //}
    }
    public class ProjectPath
    {
        public static string sExecuteFileName = "WINPROJ.EXE";
        /// <summary>
        /// 获取project执行路径
        /// </summary>
        /// <returns></returns>
        public static string GetPath()
        {
            string sPath = string.Empty;
            sPath = GetProject2003Install();//获取03project路径
            if (string.IsNullOrEmpty(sPath))
            {
                sPath = GetProject2007Install();

            }
            if (!string.IsNullOrEmpty(sPath))
            {
                sPath += @"\" + sExecuteFileName;
            }
            else
            {
                sPath = string.Empty;
            }
            return sPath;
        }
        /// <summary>
        /// 获取03
        /// </summary>
        /// <returns></returns>
        public static string GetProject2007Install()
        {
            string sMsg = string.Empty;
            try
            {
                RegistryKey machineKey = Registry.LocalMachine;

                sMsg = machineKey.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Office").OpenSubKey("12.0").OpenSubKey("Project").OpenSubKey("InstallRoot").GetValue("Path").ToString();
                //MessageBox.Show("word安装目录：" + sMsg + "应用执行目录:" + Application.ExecutablePath);
                //MessageBox.Show(Application.ExecutablePath);
                return sMsg.Substring(0, sMsg.LastIndexOf(@"\"));
            }
            catch (Exception ee)
            {

            }
            //移除最后的斜杠
            return string.Empty;
        }
        /// <summary>
        /// 获取07
        /// </summary>
        /// <returns></returns>
        public static string GetProject2003Install()
        {
            string sMsg = string.Empty;
            try
            {
                RegistryKey machineKey = Registry.LocalMachine;

                sMsg = machineKey.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Office").OpenSubKey("12.0").OpenSubKey("Project").OpenSubKey("InstallRoot").GetValue("Path").ToString();
                //MessageBox.Show("word安装目录：" + sMsg + "应用执行目录:" + Application.ExecutablePath);
                //MessageBox.Show(Application.ExecutablePath);
                return sMsg.Substring(0, sMsg.LastIndexOf(@"\"));
            }
            catch (Exception ee)
            {

            }
            //移除最后的斜杠
            return string.Empty;
        }

     

    }
}
