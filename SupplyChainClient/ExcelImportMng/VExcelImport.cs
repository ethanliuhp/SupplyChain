using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using System.IO;
using System.Data.OleDb;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Core;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ExcelImportMng
{
    public partial class VExcelImport: TBasicDataView
    {
        private MExcelImportMng model = new MExcelImportMng();
        public VExcelImport()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnExcelImport.Focus();
        }       

        private void InitEvent()
        {
            this.btn_fileView.Click += new EventHandler(btn_fileView_Click);
            this.btnExcelImport.Click += new EventHandler(btnExcelImport_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);

            this.btn_fileView1.Click += new EventHandler(btn_fileView1_Click);
            this.btnExcelImport1.Click += new EventHandler(btnExcelImport1_Click);
            this.btnCancel1.Click += new EventHandler(btnCancel1_Click);

            this.btnProjectType.Click +=new EventHandler(btnProjectType_Click);
            this.btnProjectTypeCancel.Click +=new EventHandler(btnProjectTypeCancel_Click);
            this.btnProjectTypeIn.Click +=new EventHandler(btnProjectTypeIn_Click);
            


        }

        protected string SearchExcel()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Microsoft Office Excel 2003(*.xls)|*.mdb|压缩文件 (*.zip)|*.zip|所有文件(*.*)|*.*";
            openFile.ShowDialog();
            string fileName = openFile.FileName;
            if (fileName.Equals("") || !System.IO.File.Exists(fileName))
            {
            }
            else
            {
                FileInfo finfo = new FileInfo(fileName);
                if (finfo.Length > int.MaxValue)
                {
                    MessageBox.Show("文件太大！系统加载失败！", "系统提示", MessageBoxButtons.OK);
                }
            }
            return fileName;
        }
        #region 选择文件

        void btn_fileView_Click(object sender, EventArgs e)
        {
            //SearchExcel();
            txtfileName.Text = SearchExcel();
        }
        void btn_fileView1_Click(object sender, EventArgs e)
        {
            
            txtfileName1.Text = SearchExcel();
        }
        void btnProjectType_Click(object sender, EventArgs e)
        {

            txtProjectType.Text = SearchExcel();
        }
        #endregion

        #region 导入信息
        //导入信息
        void btnExcelImport_Click(object sender, EventArgs e)
        {
            string strFilePash = txtfileName.Text;
            string extFile = Path.GetExtension(strFilePash).ToUpper();//获得Fielupload的值
            if (extFile == ".XLS" || extFile == ".XLSX")//如果文件是.XLS格式的
            {
                string str = System.IO.Path.GetFileName(strFilePash);
                ImportExcel(str);
            }
            else
            {
                MessageBox.Show("文件格式不正确，请查证后再操作！");
            }
        }
        //导入信息
        void btnExcelImport1_Click(object sender, EventArgs e)
        {
            string strFilePash = txtfileName1.Text;
            string extFile = Path.GetExtension(strFilePash).ToUpper();//获得Fielupload的值
            if (extFile == ".XLS" || extFile == ".XLSX")//如果文件是.XLS格式的
            {
                string str = System.IO.Path.GetFileName(strFilePash);
                ImportExcel1(str);
            }
            else
            {
                MessageBox.Show("文件格式不正确，请查证后再操作！");
            }
        }
        //导入信息
        void btnProjectTypeIn_Click(object sender, EventArgs e)
        {
            string strFilePash = txtProjectType.Text;
            string extFile = Path.GetExtension(strFilePash).ToUpper();//获得Fielupload的值
            if (extFile == ".XLS" || extFile == ".XLSX")//如果文件是.XLS格式的
            {
                string str = System.IO.Path.GetFileName(strFilePash);
                ImportExcel2(str);
            }
            else
            {
                MessageBox.Show("文件格式不正确，请查证后再操作！");
            }
        }
        #endregion

        #region Id生成方法
        private IFCGuidGenerator billIdRuleSrv;
        public IFCGuidGenerator BillIdRuleSrv
        {
            get { return billIdRuleSrv; }
            set { billIdRuleSrv = value; }
        }

        public string GetGuid()
        {
            string guid = "";
            IFCGuidGenerator gen = new IFCGuidGenerator();
            guid = gen.GeneratorIFCGuid();
            return guid;
        }
        #endregion


        #region 基本登陆信息
        DateTime strDate = DateTime.Now;
        string strOperOrgInfoId = ConstObject.LoginPersonInfo.Id;
        string strOperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name.ToString();
        string strOperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo.Name.ToString();
        string strOpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode.ToString();
        CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
        //string strHandlePerson = ConstObject.LoginPersonInfo.ToString();
        //string strProjectId = ClientUtil.ToString(projectInfo.Id);
        //string strProjectName = ClientUtil.ToString(projectInfo.Name);  
        #endregion

        protected void ImportExcel(string SExcelFileName)
        {
             try
             { 
                 DataSet OleDsExcle = ExploreFile.OpenExcel(SExcelFileName);

                 //打开数据库连接。返回command
                 //IDbConnection conn = model.ExcelImportSrv.OpenConn();

                 Hashtable hashtableUnit = new Hashtable();
                 string strSearchUint = "select STANDUNITID,STANDUNITNAME from RESSTANDUNIT";
                 DataTable strUnitdt = model.ExcelImportSrv.SearchSql(strSearchUint);
                 for (int k = 0; k < strUnitdt.Rows.Count; k++)
                 {
                     string strUnitId = strUnitdt.Rows[k][0].ToString();
                     string strUnitName = strUnitdt.Rows[k][1].ToString();
                     hashtableUnit.Add(strUnitId, strUnitName);
                 }

                 Hashtable hashtableMaterial = new Hashtable();
                 Hashtable hashtableMaterialType = new Hashtable();
                 string strSearchMaterial = "select ID,CODE,SYSCODE from RESMATERIALCATEGORY";
                 DataTable strdtMaterial = model.ExcelImportSrv.SearchSql(strSearchMaterial);
                 for (int t = 0; t < strdtMaterial.Rows.Count; t++)
                 {
                     string strUnitId = strdtMaterial.Rows[t][0].ToString();
                     string strUnitCode = strdtMaterial.Rows[t][1].ToString();
                     string strUnitName = strdtMaterial.Rows[t][2].ToString();
                     hashtableMaterialType.Add(strUnitId, strUnitName);
                     hashtableMaterial.Add(strUnitId, strUnitCode);
                 }
                 

                 if (OleDsExcle.Tables[0].Rows.Count != 0)
                 {

                     for (int i = 0; i < OleDsExcle.Tables[0].Rows.Count; i++)//循环读取临时表的行
                     {
                         string strCode = OleDsExcle.Tables[0].Rows[i][0].ToString();//读取临时表第i行第j列的数据
                         if (strCode.Length == 4)
                         {
                             //长度为4，RESMATERIALCATEGORY
                             string strGuid = GetGuid();//生成Id
                             string strId = "1.";
                             string strSYSTEMCODE = strId + strGuid + ".";
                             string strsql = "insert into RESMATERIALCATEGORY (ID,NAME,NODETYPE,CODE,CREATEDATE,SYSCODE,STATE,numLEVEL,PARENTNODEID,THETREEID,MATCATATTRUBUTE,PraStateControlRuleID,ManStateControlRuleID,PERID,OrderNo,MatCatKind,OperationOrg,OpgSysCode)";
                             strsql += "values ('" + strGuid + "','" + OleDsExcle.Tables[0].Rows[i][1].ToString() + "','1','" + OleDsExcle.Tables[0].Rows[i][0].ToString() + "','" + strDate + "','" + strSYSTEMCODE + "','1','2','30','7','0','2','14','" + strOperOrgInfoName + "','1','0','" + strOperOrgInfo + "','" + strOpgSysCode + "')";
                             int res = model.ExcelImportSrv.SaveSql(strsql);
                             if (res != 0)
                             {
                                 hashtableMaterial.Add(strGuid, OleDsExcle.Tables[0].Rows[i][0].ToString());
                                 hashtableMaterialType.Add(strGuid, strSYSTEMCODE);
                             }
                         }
                         if (strCode.Length == 6)
                         {
                             //长度为6，RESMATERIALCATEGORY
                             string strCodeThree = OleDsExcle.Tables[0].Rows[i][2].ToString();
                             if (strCodeThree == "")
                             {
                                 string strfour = strCode.Substring(0, 4);//截取前四位
                                 string strId = null;
                                 string strParentId = null;
                                 foreach (System.Collections.DictionaryEntry objDE in hashtableMaterial)
                                 {
                                     if (objDE.Value.ToString().Equals(strfour))
                                     {
                                         strId = objDE.Key.ToString();
                                         foreach (System.Collections.DictionaryEntry objType in hashtableMaterialType)
                                         {
                                             if (objType.Key.ToString().Equals(strId))
                                             {
                                                 strParentId = objType.Value.ToString();
                                             }
                                         }
                                         break;
                                     }
                                 }
                                 string strGuid = GetGuid();//生成Id
                                 string strSYSTEMCODE = strParentId + strGuid + ".";
                                 string strsql = "insert into RESMATERIALCATEGORY (ID,NAME,NODETYPE,CODE,CREATEDATE,SYSCODE,STATE,numLEVEL,PARENTNODEID,THETREEID,MATCATATTRUBUTE,PraStateControlRuleID,ManStateControlRuleID,PERID,OrderNo,MatCatKind,OperationOrg,OpgSysCode)";
                                 strsql += "values ('" + strGuid + "','" + OleDsExcle.Tables[0].Rows[i][1].ToString() + "','1','" + OleDsExcle.Tables[0].Rows[i][0].ToString() + "','" + strDate + "','" + strSYSTEMCODE + "','1','3','" + strId + "','7','0','2','14','" + strOperOrgInfoName + "','1','0','" + strOperOrgInfo + "','" + strOpgSysCode + "')";
                                 int res = model.ExcelImportSrv.SaveSql(strsql);
                                 if (res != 0)
                                 {
                                     hashtableMaterial.Add(strGuid, OleDsExcle.Tables[0].Rows[i][0].ToString());
                                     hashtableMaterialType.Add(strGuid, strSYSTEMCODE);
                                 }
                             }
                             else
                             {
                                //编号不为空
                                 string strUnit = OleDsExcle.Tables[0].Rows[i][8].ToString();
                                 if (strUnit != "")
                                 {//excel表中有计量单位信息，无计量单位的信息不保存
                                     string strMaterialCategoryId = null;
                                     foreach (System.Collections.DictionaryEntry objDE in hashtableMaterial)
                                     {
                                         if (objDE.Value.ToString().Equals(strCode))
                                         {
                                             strMaterialCategoryId = objDE.Key.ToString();
                                             break;
                                         }
                                     }
                                     string strGuid = GetGuid();//生成Id
                                     //有计量单位信息
                                     string strUnitId = null;
                                     foreach (System.Collections.DictionaryEntry value in hashtableUnit)
                                     {
                                         if (value.Value.ToString().Equals(strUnit))
                                         {
                                             strUnitId = value.Key.ToString();
                                         }
                                     }
                                     string strsql = "insert into ResMaterial (Version,MATERIALID,MATCODE,MATNAME,MaterialCategoryId,MATSPECIFICATION,Quality,PracticalityStateControlRuleId,ManageStateControlRuleId,MATATTRIBUTE,Requirement,StandardUnitID,States,CreatedDate,PERID,ModifyPerson,OperationOrg,OpgSysCode,IsAuto)";
                                     strsql += "values('0','" + strGuid + "','" + OleDsExcle.Tables[0].Rows[i][2].ToString() + "','" + OleDsExcle.Tables[0].Rows[i][3].ToString() + "','" + strMaterialCategoryId + "','" + OleDsExcle.Tables[0].Rows[i][4].ToString() + "','" + OleDsExcle.Tables[0].Rows[i][6].ToString() + "','2','14','0','0','" + strUnitId + "','1','" + strDate + "','" + strOperOrgInfoId + "','" + strOperOrgInfoName + "','" + strOperOrgInfo + "','" + strOpgSysCode + "','" + false + "')";
                                     int res = model.ExcelImportSrv.SaveSql(strsql);
                                     if (res != 0)
                                     {

                                     }
                                 }
                             }
                         }
                         if (strCode.Length == 8)
                         {
                             string strRowCon = OleDsExcle.Tables[0].Rows[i][1].ToString();
                             if (strRowCon.Equals("") || strRowCon.Equals("null"))
                             {
                                 //第二列信息为空
                                 string strUnit = OleDsExcle.Tables[0].Rows[i][8].ToString();
                                 if (strUnit != "")
                                 {//excel表中有计量单位信息，无计量单位的信息不保存
                                     //string streigth = strCode.Substring(0, 8);//截取前八位
                                     string strMaterialCategoryId = null;
                                     foreach (System.Collections.DictionaryEntry objDE in hashtableMaterial)
                                     {
                                         if (objDE.Value.ToString().Equals(strCode))
                                         {
                                             strMaterialCategoryId = objDE.Key.ToString();
                                             break;
                                         }
                                     }
                                     string strGuid = GetGuid();//生成Id
                                     //有计量单位信息
                                     string strUnitId = null;
                                     foreach (System.Collections.DictionaryEntry value in hashtableUnit)
                                     {
                                         if (value.Value.ToString().Equals(strUnit))
                                         {
                                             strUnitId = value.Key.ToString();
                                         }
                                     }
                                     string strsql = "insert into ResMaterial (Version,MATERIALID,MATCODE,MATNAME,MaterialCategoryId,MATSPECIFICATION,Quality,PracticalityStateControlRuleId,ManageStateControlRuleId,MATATTRIBUTE,Requirement,StandardUnitID,States,CreatedDate,PERID,ModifyPerson,OperationOrg,OpgSysCode,IsAuto)";
                                     strsql += "values('0','" + strGuid + "','" + OleDsExcle.Tables[0].Rows[i][2].ToString() + "','" + OleDsExcle.Tables[0].Rows[i][3].ToString() + "','" + strMaterialCategoryId + "','" + OleDsExcle.Tables[0].Rows[i][4].ToString() + "','" + OleDsExcle.Tables[0].Rows[i][6].ToString() + "','2','14','0','0','" + strUnitId + "','1','" + strDate + "','" + strOperOrgInfoId + "','" + strOperOrgInfoName + "','" + strOperOrgInfo + "','" + strOpgSysCode + "','" + false + "')";
                                     int res = model.ExcelImportSrv.SaveSql(strsql);
                                     if (res != 0)
                                     {
                                         
                                     }
                                 }
                             }
                             else
                             {
                                 //长度为8，RESMATERIALCATEGORY
                                 string strsix = strCode.Substring(0, 6);//截取前六位
                                 string strId = null;
                                 string strParentId = null;
                                 foreach (System.Collections.DictionaryEntry objDE in hashtableMaterial)
                                 {
                                     if (objDE.Value.ToString().Equals(strsix))
                                     {
                                         strId = objDE.Key.ToString();
                                         foreach (System.Collections.DictionaryEntry objType in hashtableMaterialType)
                                         {
                                             if (objType.Key.ToString().Equals(strId))
                                             {
                                                 strParentId = objType.Value.ToString();
                                             }
                                         }
                                         break;
                                     }
                                 }
                                 string strGuid = GetGuid();//生成Id
                                 string strSYSTEMCODE = strParentId + strGuid + ".";
                                 string strsql = "insert into RESMATERIALCATEGORY (ID,NAME,NODETYPE,CODE,CREATEDATE,SYSCODE,STATE,numLEVEL,PARENTNODEID,THETREEID,MATCATATTRUBUTE,PraStateControlRuleID,ManStateControlRuleID,PERID,OrderNo,MatCatKind,OperationOrg,OpgSysCode)";
                                 strsql += "values ('" + strGuid + "','" + OleDsExcle.Tables[0].Rows[i][1].ToString() + "','2','" + OleDsExcle.Tables[0].Rows[i][0].ToString() + "','" + strDate + "','" + strSYSTEMCODE + "','1','4','" + strId + "','7','0','2','14','" + strOperOrgInfoName + "','1','0','" + strOperOrgInfo + "','" + strOpgSysCode + "')";
                                 //int res = savesql(command, sql);
                                 int res = model.ExcelImportSrv.SaveSql(strsql);
                                 if (res != 0)
                                 {
                                     hashtableMaterial.Add(strGuid, OleDsExcle.Tables[0].Rows[i][0].ToString());
                                     hashtableMaterialType.Add(strGuid, strSYSTEMCODE);
                                 }
                             }
                         }
                     }
                 }


                 ExploreFile.CloseExcel();
             }
             catch (Exception err)
             {
                 MessageBox.Show("数据绑定Excel失败!失败原因：" + err.Message, "提示信息",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
             }
        }


        protected void ImportExcel1(string SExcelFileName)
        {
            try
            {
               
                DataSet OleDsExcle = ExploreFile.OpenExcel(SExcelFileName);//打开Excel连接，获取Excel数据
                //所连接的数据库表为THD_BasicDataOptr

                //Hashtable hashtableData = new Hashtable();
                Hashtable hashtableName = new Hashtable();
                string strSearchData = "select * from THD_BasicDataOptr";
                DataTable strDatadt = model.ExcelImportSrv.SearchSql(strSearchData);
                for (int k = 0; k < strDatadt.Rows.Count; k++)
                {
                    string strDataId = strDatadt.Rows[k][0].ToString();
                    //string strDataParentId = strDatadt.Rows[k][1].ToString();
                    string strDataName = strDatadt.Rows[k][3].ToString();
                    hashtableName.Add(strDataId, strDataName);
                    //hashtableData.Add(strDataId, strDataParentId);
                }

                if (OleDsExcle.Tables[0].Rows.Count != 0)
                {

                    for (int i = 0; i < OleDsExcle.Tables[0].Rows.Count; i++)//循环读取临时表的行
                    {
                        int flag = 1;//1父信息，2子信息，0重复信息
                        string strId = null;
                        string strOwn = OleDsExcle.Tables[0].Rows[i][0].ToString();//读取临时表归属信息
                        string strCode = OleDsExcle.Tables[0].Rows[i][1].ToString();//读取临时表编号
                        string strName = OleDsExcle.Tables[0].Rows[i][2].ToString();//读取临时表名称
                        if (strCode != "" && strName != "" && strOwn != "")
                        {
                            foreach (System.Collections.DictionaryEntry objDE in hashtableName)
                            {
                                if (objDE.Value.ToString().Equals(strOwn))
                                {
                                    //哈希表中存在读取的归属信息,
                                    strId = objDE.Key.ToString();//获得父Id
                                    flag = 2;
                                    break;
                                }
                            }
                            foreach (System.Collections.DictionaryEntry objdt in hashtableName)
                            {
                                if (objdt.Value.ToString().Equals(strName))
                                {
                                    flag = 0;
                                    break;
                                }
                            }



                            if (flag == 1)
                            {
                                //保存父信息
                                string strGuid = GetGuid();//生成Id
                                string strsql = "insert into THD_BasicDataOptr(Id,BasicName,Descript,State)";
                                strsql += "values ('" + strGuid + "','" + strOwn + "','" + OleDsExcle.Tables[0].Rows[i][3].ToString() + "','-1 ')";

                                int res = model.ExcelImportSrv.SaveSql(strsql);
                                if (res != 0)
                                {
                                    //hashtableData.Add(strGuid, OleDsExcle.Tables[0].Rows[i][1].ToString());
                                    hashtableName.Add(strGuid, strOwn);
                                }

                                if (strId == "" || strId == null)
                                {
                                    //foreach (System.Collections.DictionaryEntry objDE1 in hashtableName)
                                    //{
                                    //    if (objDE1.Value.ToString().Equals(strOwn))
                                    //    {
                                    //        //哈希表中存在读取的归属信息,
                                    //        strId = objDE1.Key.ToString();//获得父Id
                                    //        foreach (System.Collections.DictionaryEntry objdt1 in hashtableName)
                                    //        {
                                    //            if (objdt1.Value.ToString().Equals(strName))
                                    //            {
                                    //                flag = 0;
                                    //                break;
                                    //            }
                                    //        }
                                    //        flag = 2;
                                    //        break;
                                    //    }
                                    //}
                                    foreach (System.Collections.DictionaryEntry objDE1 in hashtableName)
                                    {
                                        if (objDE1.Value.ToString().Equals(strOwn))
                                        {
                                            //哈希表中存在读取的归属信息,
                                            strId = objDE1.Key.ToString();//获得父Id
                                            flag = 2;
                                            break;
                                        }
                                    }
                                    foreach (System.Collections.DictionaryEntry objdt1 in hashtableName)
                                    {
                                        if (objdt1.Value.ToString().Equals(strName))
                                        {
                                            flag = 0;
                                            break;
                                        }
                                    }
                                }
                            }
                            if(flag == 2)
                            {
                                string strGuid = GetGuid();//生成Id
                                string strsql = "insert into THD_BasicDataOptr(Id,ParentId,BasicCode,BasicName,Descript,STATE)";
                                strsql += "values ('" + strGuid + "','" + strId + "','" + strCode + "','" + strName + "','" + OleDsExcle.Tables[0].Rows[i][3].ToString() + "','0 ')";

                                int res = model.ExcelImportSrv.SaveSql(strsql);
                                if (res != 0)
                                {
                                    //hashtableData.Add(strGuid, OleDsExcle.Tables[0].Rows[i][1].ToString());
                                    hashtableName.Add(strGuid, strName);
                                } 
                            }
                        }
                    }
                }
                ExploreFile.CloseExcel();
            }
            catch (Exception err)
            {
                MessageBox.Show("数据绑定Excel失败!失败原因：" + err.Message, "提示信息",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void ImportExcel2(string SExcelFileName)
        {
            try
            {
                DataSet OleDsExcle = ExploreFile.OpenExcel(SExcelFileName);//打开Excel连接，读取excel信息
                //读取数据表中的信息，将信息保存到hashtable中
                Hashtable hashtableProject = new Hashtable();
                string strSearchProject = "select ID,Code from THD_ProjectTaskTypeTree";
                DataTable strProjectdt = model.ExcelImportSrv.SearchSql(strSearchProject);
                for (int k = 0; k < strProjectdt.Rows.Count; k++)
                {
                    string strProjectId = strProjectdt.Rows[k][0].ToString();
                    string strProjectName = strProjectdt.Rows[k][1].ToString();
                    hashtableProject.Add(strProjectId, strProjectName);
                }

                Hashtable hashtableSysCode = new Hashtable();
                string strSearchCode = "select ID,SysCode from THD_ProjectTaskTypeTree";
                DataTable strCodedt = model.ExcelImportSrv.SearchSql(strSearchCode);
                for (int k = 0; k < strCodedt.Rows.Count; k++)
                {
                    string strSysId = strProjectdt.Rows[k][0].ToString();
                    string strSysCode = strProjectdt.Rows[k][1].ToString();
                    hashtableSysCode.Add(strSysId, strSysCode);
                }
                int strOrderNo = 0;//子节点排序的编号
                string strParent = null;//父节点名称
                int Rows = 0;
                if (OleDsExcle.Tables[0].Rows.Count != 0)
                {
                    int Columns = OleDsExcle.Tables[0].Columns.Count;
                    if (Columns < 8)
                    {
                        MessageBox.Show("Excel格式不正确！");
                        return;
                    }
                    for (int i = 1; i < OleDsExcle.Tables[0].Rows.Count; i++)//循环读取临时表的行，第一行的信息不读取
                    {
                        string strMasageOne = OleDsExcle.Tables[0].Rows[i][1].ToString();//获得工程任务类型编码
                        string strMasageTew = OleDsExcle.Tables[0].Rows[i][2].ToString();//获得工程任务类型名称
                        string strmasageThree = OleDsExcle.Tables[0].Rows[i][3].ToString();//获得遵循标准
                        string strMasageSix = OleDsExcle.Tables[0].Rows[i][6].ToString();//获得检查要求
                        if (strMasageOne.Equals("") && strMasageSix.Equals("") && strMasageTew.Equals("") && strmasageThree.Equals(""))
                        {}
                        else
                        {
                            if (strMasageOne.Length < 9)
                            {
                                MessageBox.Show((i + 2) + "行有错误，工程任务类型编码长度应为9");
                                return;
                            }
                            if (strMasageTew.Equals(""))
                            {
                                MessageBox.Show((i + 2) + "行有错误，工程任务类型名称不能为空");
                                return;
                            }
                            if (strmasageThree.Equals(""))
                            {
                                MessageBox.Show((i + 2) + "行有错误，遵循标准不能为空");
                                return;
                            }
                            if (strMasageSix.Equals("") || strMasageSix.Length < 6)
                            {
                                MessageBox.Show((i + 2) + "行有错误，检查要求不能为空并且长度为6");
                                return;
                            }
                        }
                    }
                }
                if (OleDsExcle.Tables[0].Rows.Count != 0)
                {
                    for (int i = 1; i < OleDsExcle.Tables[0].Rows.Count; i++)//循环读取临时表的行，第一行的信息不读取
                    {
                        string strCode = OleDsExcle.Tables[0].Rows[i][1].ToString();//读取临时表第i行第j列的数据
                        if (strCode != "")
                        {
                            //第二列有信息，无信息不处理
                            string strGuid = GetGuid();//生成Id
                            string strName = OleDsExcle.Tables[0].Rows[i][2].ToString();
                            string strTypeStandard = OleDsExcle.Tables[0].Rows[i][3].ToString();
                            string strCheckRequire = OleDsExcle.Tables[0].Rows[i][6].ToString();
                            string strId = null;
                            string strSysCode = null;
                            string strTow = strCode.Remove(0, 2);//去掉前两个字符
                            if (strTow.Equals("0000000"))
                            {
                                string strSysSql = "select SysCode from THD_ProjectTaskTypeTree where CategoryNodeType = '0'";
                                DataTable Sysdt = model.ExcelImportSrv.SearchSql(strSysSql);
                                strSysCode = Sysdt.Rows[0][0].ToString();
                                strSysCode += strGuid + '.';
                                //截取字符串，截去前两位后面几位都是零，第一级
                                string strsql = "insert into THD_ProjectTaskTypeTree (ID,CategoryNodeType,Name,Code,CreateDate,SysCode,State,TLevel,Author,TheTree,OrderNo,TypeLevel,TypeStandard,CheckRequire,TheProjectGUID,TheProjectName)";
                                strsql += "values('" + strGuid + "','1','" + strName + "','" + strCode + "','" + strDate + "','" + strSysCode + "','1','1','" + strOperOrgInfoId + "','11','0','1','" + strTypeStandard + "','" + strCheckRequire + "','','')";
                                int res = model.ExcelImportSrv.SaveSql(strsql);
                                if (res != 0)
                                {
                                    Rows += 1;
                                    hashtableProject.Add(strGuid, strCode);
                                    hashtableSysCode.Add(strGuid, strSysCode);
                                }
                            }
                            else
                            {
                                string strFour = strCode.Remove(0, 4);//去点前4个字符
                                if (strFour.Equals("00000"))
                                {
                                    //截去前四位，后几位都是零，第二级
                                    string strNew = strCode.Substring(0, 2) + "0000000";
                                    //从hashtable中查找编号为strnew的信息
                                    foreach (System.Collections.DictionaryEntry objDE in hashtableProject)
                                    {
                                        if (objDE.Value.ToString().Equals(strNew))
                                        {
                                            strId = objDE.Key.ToString();//获得ID
                                            foreach (System.Collections.DictionaryEntry objCode in hashtableSysCode)
                                            {
                                                if (objCode.Key.ToString().Equals(strId))
                                                {
                                                    strSysCode = objCode.Value.ToString();
                                                }
                                            }
                                            break;
                                        }
                                        //没有找到相应的信息跳出循环
                                    }
                                    strSysCode += strGuid + '.';
                                    string strsql = "insert into THD_ProjectTaskTypeTree (ID,CategoryNodeType,Name,Code,CreateDate,SysCode,State,ParentNodeID,TLevel,Author,TheTree,OrderNo,TypeLevel,TypeStandard,CheckRequire,TheProjectGUID,TheProjectName)";
                                    strsql += "values('" + strGuid + "','1','" + strName + "','" + strCode + "','" + strDate + "','" + strSysCode + "','1','" + strId + "','2','" + strOperOrgInfoId + "','11','0','4','" + strTypeStandard + "','" + strCheckRequire + "','','')";
                                    int res = model.ExcelImportSrv.SaveSql(strsql);
                                    if (res != 0)
                                    {
                                        Rows += 1;
                                        hashtableProject.Add(strGuid, strCode);
                                        hashtableSysCode.Add(strGuid, strSysCode);
                                    }

                                }
                                else
                                {

                                    string strSix = strCode.Remove(0, 6);//去掉前六位
                                    if (strSix.Equals("000"))
                                    {
                                        //截去前六位，后几位都是零，第三级
                                        string strNew = strCode.Substring(0, 4) + "00000";
                                        //从hashtable中查找编号为strnew的信息
                                        foreach (System.Collections.DictionaryEntry objDE in hashtableProject)
                                        {
                                            if (objDE.Value.ToString().Equals(strNew))
                                            {
                                                strId = objDE.Key.ToString();//获得ID
                                                foreach (System.Collections.DictionaryEntry objCode in hashtableSysCode)
                                                {
                                                    if (objCode.Key.ToString().Equals(strId))
                                                    {
                                                        strSysCode = objCode.Value.ToString();
                                                    }
                                                }
                                                break;
                                            }
                                            //没有找到相应的信息跳出循环
                                        }
                                        strSysCode += strGuid + '.';
                                        string strsql = "insert into THD_ProjectTaskTypeTree (ID,CategoryNodeType,Name,Code,CreateDate,SysCode,State,ParentNodeID,TLevel,Author,TheTree,OrderNo,TypeLevel,TypeStandard,CheckRequire,TheProjectGUID,TheProjectName)";
                                        strsql += "values('" + strGuid + "','1','" + strName + "','" + strCode + "','" + strDate + "','" + strSysCode + "','1','" + strId + "','3','" + strOperOrgInfoId + "','11','0','5','" + strTypeStandard + "','" + strCheckRequire + "','','')";
                                        int res = model.ExcelImportSrv.SaveSql(strsql);
                                        if (res != 0)
                                        {
                                            Rows += 1;
                                            hashtableProject.Add(strGuid, strCode);
                                            hashtableSysCode.Add(strGuid, strSysCode);
                                        }
                                    }
                                    else
                                    {
                                        //剩下的位第四级
                                        string strNew = strCode.Substring(0, 6) + "000";
                                        //从hashtable中查找编号为strnew的信息
                                        foreach (System.Collections.DictionaryEntry objDE in hashtableProject)
                                        {
                                            if (objDE.Value.ToString().Equals(strNew))
                                            {
                                                strId = objDE.Key.ToString();//获得ID
                                                foreach (System.Collections.DictionaryEntry objCode in hashtableSysCode)
                                                {
                                                    if (objCode.Key.ToString().Equals(strId))
                                                    {
                                                        strSysCode = objCode.Value.ToString();
                                                    }
                                                }
                                                break;
                                            }
                                            //没有找到相应的信息跳出循环
                                        }
                                        strSysCode += strGuid + '.';
                                        string strsql = "insert into THD_ProjectTaskTypeTree (ID,CategoryNodeType,Name,Code,CreateDate,SysCode,State,ParentNodeID,TLevel,Author,TheTree,OrderNo,TypeLevel,TypeStandard,CheckRequire,TheProjectGUID,TheProjectName)";
                                        strsql += "values('" + strGuid + "','2','" + strName + "','" + strCode + "','" + strDate + "','" + strSysCode + "','1','" + strId + "','4','" + strOperOrgInfoId + "','11','0','6','" + strTypeStandard + "','" + strCheckRequire + "','','')";
                                        int res = model.ExcelImportSrv.SaveSql(strsql);
                                        if (res != 0)
                                        {
                                            Rows += 1;
                                            hashtableProject.Add(strGuid, strCode);
                                            hashtableSysCode.Add(strGuid, strSysCode);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                MessageBox.Show(Rows + "条信息保存成功！");
                ExploreFile.CloseExcel();
            }
            catch (Exception err)
            {
                MessageBox.Show("数据绑定Excel失败!失败原因：" + err.Message, "提示信息",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        #region 取消操作
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtfileName.Text = "";
        }
        void btnCancel1_Click(object sender, EventArgs e)
        {
            this.txtfileName1.Text = "";
        }
        void btnProjectTypeCancel_Click(object sender, EventArgs e)
        {
            this.txtProjectType.Text = "";
        }
        #endregion

    }
}
