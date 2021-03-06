using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.BasicData.UnitMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using System.IO;
using Application.Business.Erp.SupplyChain.Client.Util;


namespace Application.Business.Erp.SupplyChain.Client.ExcelImportMng
{
    public partial class VExcelImportMng : TBasicDataView
    {
        private MExcelImportMng model = new MExcelImportMng();
        private IList lstInstance;
        public VExcelImportMng()
        {
            InitializeComponent();
            InitForm();
        }

        private void InitForm()
        {
            InitEvents();
            txtCategory.CheckBoxes = false;
            LoadLeftWindowTree();
        }

        private void InitEvents()
        {
            txtCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            btn_fileView.Click += new EventHandler(btn_fileView_Click);
            btnExcelImport.Click += new EventHandler(btnExcelImport_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnAdd.Click += new EventHandler(btnAdd_Click);
        }

        private void LoadLeftWindowTree()
        {
            txtCategory.Nodes.Add("物料分类基础数据");
            txtCategory.Nodes.Add("工程WBS任务类型");
            txtCategory.Nodes.Add("基础数据");
            txtCategory.Nodes.Add("成本核算科目");
            txtCategory.Nodes.Add("成本项数据");
            txtCategory.Nodes.Add("成本项分类数据");
            txtCategory.Nodes.Add("会计科目");
            txtCategory.Nodes.Add("成本项安装分类数据");
        }

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                groupBox1.Text = txtCategory.SelectedNode.Text;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        void btn_fileView_Click(object sender, EventArgs e)
        {
            txtfileName.Text = SearchExcel();
        }

        protected string SearchExcel()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "所有文件(*.*)|*.*";
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
        string strOperOrgInfoId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Id;
        string strPersonName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
        string strOperOrgInfoName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheOperationOrgInfo.Name.ToString();
        string strOperOrgInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheOperationOrgInfo.Name.ToString();
        string strOpgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheOperationOrgInfo.SysCode.ToString();
        string OperOrgInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheOperationOrgInfo.Id.ToString();
        CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
        string strHandlePerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.ToString();
        #endregion

        void btnExcelImport_Click(object sender, EventArgs e)
        {
            string strFilePash = txtfileName.Text;
            string extFile = Path.GetExtension(strFilePash).ToUpper();//获得Fielupload的值
            if (extFile == ".XLS" || extFile == ".XLSX")//如果文件是.XLS格式的
            {
                //string str = System.IO.Path.GetFileName(strFilePash);
                string str = strFilePash;
                string strName = txtCategory.SelectedNode.Text;
                if (strName.Equals("物料分类基础数据"))
                {
                    ImportExcel(str);
                }
                if (strName.Equals("工程WBS任务类型"))
                {
                    ImportExcel2(str);
                }
                if (strName.Equals("基础数据"))
                {
                    ImportExcel1(str);
                }
                if (strName.Equals("成本核算科目"))
                {
                    ImportExcel3(str);
                }
                if (strName.Equals("成本项数据"))
                {
                    ImportExcel4(str);
                }
                if (strName.Equals("成本项分类数据"))
                {
                    ImportExcel5(str);
                }
                if (strName.Equals("会计科目"))
                {
                    ImportExcel6(str);
                }
                if (strName.Equals("成本项安装分类数据"))
                {
                    ImportExcelInstall(str);
                }
            }
            else
            {
                MessageBox.Show("文件格式不正确，请查证后再操作！");
            }
        }

        #region 会计科目
        private void ImportExcel6(string SExcelFileName)
        {
            try
            {
                DataSet OleDsExcle = ExploreFile.OpenExcel(SExcelFileName);
                model.ExcelImportSrv.SaveFiacctitle(OleDsExcle, projectInfo, strOperOrgInfoId);
                ExploreFile.CloseExcel();
            }
            catch (Exception err)
            {
                MessageBox.Show("数据绑定Excel失败!失败原因：" + err.Message, "提示信息",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 成本项分类
        protected void ImportExcel5(string SExcelFileName)
        {
            try
            {
                DataSet OleDsExcle = ExploreFile.OpenExcel(SExcelFileName);
                model.ExcelImportSrv.SaveCostCatagry(OleDsExcle, projectInfo);
                ExploreFile.CloseExcel();
            }
            catch (Exception err)
            {
                MessageBox.Show("数据绑定Excel失败!失败原因：" + err.Message, "提示信息",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 成本项
        protected void ImportExcel4(string SExcelFileName)
        {
            try
            {
                DataSet OleDsExcle = ExploreFile.OpenExcel(SExcelFileName);
                model.ExcelImportSrv.SaveCostItem(OleDsExcle, projectInfo);
                ExploreFile.CloseExcel();
            }
            catch (Exception err)
            {
                MessageBox.Show("数据绑定Excel失败!失败原因：" + err.Message, "提示信息",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 成本项安装分类
        protected void ImportExcelInstall(string SExcelFileName)
        {
            try
            {
                DataSet OleDsExcle = ExploreFile.OpenExcel(SExcelFileName);
                model.ExcelImportSrv.SaveCostInstall(OleDsExcle, projectInfo);
                ExploreFile.CloseExcel();
            }
            catch (Exception err)
            {
                MessageBox.Show("数据绑定Excel失败!失败原因：" + err.Message, "提示信息",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 成本核算科目
        protected void ImportExcel3(string SExcelFileName)
        {
            try
            {
                DataSet OleDsExcle = ExploreFile.OpenExcel(SExcelFileName);
                model.ExcelImportSrv.SaveCostAccountSubject(OleDsExcle, projectInfo, strOperOrgInfoId, strPersonName, strOpgSysCode);
                ExploreFile.CloseExcel();
            }
            catch (Exception err)
            {
                MessageBox.Show("数据绑定Excel失败!失败原因：" + err.Message, "提示信息",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 物料分类基础数据
        protected void ImportExcel(string SExcelFileName)
        {
            try
            {
                DataSet OleDsExcle = ExploreFile.OpenExcel(SExcelFileName);
                model.ExcelImportSrv.SaveMaterial(OleDsExcle, projectInfo, strOperOrgInfoId, strOperOrgInfoName, strOperOrgInfo, strOpgSysCode);
                //打开数据库连接。返回command
                //IDbConnection conn = model.ExcelImportSrv.OpenConn();

                //Hashtable hashtableUnit = new Hashtable();
                //string strSearchUint = "select STANDUNITID,STANDUNITNAME from RESSTANDUNIT";
                //DataTable strUnitdt = model.ExcelImportSrv.SearchSql(strSearchUint);
                //for (int k = 0; k < strUnitdt.Rows.Count; k++)
                //{
                //    string strUnitId = strUnitdt.Rows[k][0].ToString();
                //    string strUnitName = strUnitdt.Rows[k][1].ToString();
                //    hashtableUnit.Add(strUnitId, strUnitName);
                //}

                //Hashtable hashtableMaterial = new Hashtable();
                //Hashtable hashtableMaterialType = new Hashtable();
                //string strSearchMaterial = "select ID,CODE,SYSCODE from RESMATERIALCATEGORY";
                //DataTable strdtMaterial = model.ExcelImportSrv.SearchSql(strSearchMaterial);
                //for (int t = 0; t < strdtMaterial.Rows.Count; t++)
                //{
                //    string strUnitId = strdtMaterial.Rows[t][0].ToString();
                //    string strUnitCode = strdtMaterial.Rows[t][1].ToString();
                //    string strUnitName = strdtMaterial.Rows[t][2].ToString();
                //    hashtableMaterialType.Add(strUnitId, strUnitName);
                //    hashtableMaterial.Add(strUnitId, strUnitCode);
                //}

                //int Rows = 0;
                //if (OleDsExcle.Tables[0].Rows.Count != 0)
                //{

                //    for (int i = 1; i < OleDsExcle.Tables[0].Rows.Count; i++)//循环读取临时表的行
                //    {
                //        string strCode = OleDsExcle.Tables[0].Rows[i][0].ToString();//读取临时表第i行第j列的数据
                //        string strTreeCode = OleDsExcle.Tables[0].Rows[i][2].ToString();
                //        string strName2 = OleDsExcle.Tables[0].Rows[i][1].ToString();
                //        string strGuid = GetGuid();//生成Id
                //        if (strCode.Length == 4)
                //        {
                //            //长度为4，RESMATERIALCATEGORY
                //            //string strGuid = GetGuid();//生成Id
                //            string strId1 = "1.";
                //            string strSYSTEMCODE = strId1 + strGuid + ".";
                //            string strsql = "insert into RESMATERIALCATEGORY (ID,NAME,NODETYPE,CODE,CREATEDATE,SYSCODE,STATE,numLEVEL,PARENTNODEID,THETREEID,MATCATATTRUBUTE,PraStateControlRuleID,ManStateControlRuleID,PERID,OrderNo,MatCatKind,OperationOrg,OpgSysCode,ABBREVIATION)";
                //            strsql += "values ('" + strGuid + "','" + strName2 + "','1','" + OleDsExcle.Tables[0].Rows[i][0].ToString() + "',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strSYSTEMCODE + "','1','3','29BioV9QP5T9tJmw1VKARN','7','0','2','14','" + strOperOrgInfoName + "','1','0','" + OperOrgInfo + "','" + strOpgSysCode + "','" + strName2 + "')";
                //            int res = model.ExcelImportSrv.SaveSql(strsql);
                //            if (res != 0)
                //            {
                //                Rows += 1;
                //                hashtableMaterial.Add(strGuid, OleDsExcle.Tables[0].Rows[i][0].ToString());
                //                hashtableMaterialType.Add(strGuid, strSYSTEMCODE);
                //            }
                //        }
                //        string strId = null;
                //        string strParentId = null;
                //        if (strCode.Length == 6)
                //        {
                //            //长度为6，RESMATERIALCATEGORY
                //            string strCodeThree = OleDsExcle.Tables[0].Rows[i][2].ToString();
                //            if (strCodeThree == "")
                //            {
                //                string strfour = strCode.Substring(0, 4);//截取前四位
                //                foreach (System.Collections.DictionaryEntry objDE in hashtableMaterial)
                //                {
                //                    if (objDE.Value.ToString().Equals(strfour))
                //                    {
                //                        strId = objDE.Key.ToString();
                //                        foreach (System.Collections.DictionaryEntry objType in hashtableMaterialType)
                //                        {
                //                            if (objType.Key.ToString().Equals(strId))
                //                            {
                //                                strParentId = objType.Value.ToString();
                //                            }
                //                        }
                //                        break;
                //                    }
                //                }
                //                //string strGuid = GetGuid();//生成Id
                //                string strSYSTEMCODE = strParentId + strGuid + ".";
                //                string strsql = "insert into RESMATERIALCATEGORY (ID,NAME,NODETYPE,CODE,CREATEDATE,SYSCODE,STATE,numLEVEL,PARENTNODEID,THETREEID,MATCATATTRUBUTE,PraStateControlRuleID,ManStateControlRuleID,PERID,OrderNo,MatCatKind,OperationOrg,OpgSysCode,ABBREVIATION)";
                //                strsql += "values ('" + strGuid + "','" + strName2 + "','1','" + OleDsExcle.Tables[0].Rows[i][0].ToString() + "',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strSYSTEMCODE + "','1','4','" + strId + "','7','0','2','14','" + strOperOrgInfoName + "','1','0','" + OperOrgInfo + "','" + strOpgSysCode + "','" + strName2 + "')";
                //                int res = model.ExcelImportSrv.SaveSql(strsql);
                //                if (res != 0)
                //                {
                //                    Rows += 1;
                //                    hashtableMaterial.Add(strGuid, OleDsExcle.Tables[0].Rows[i][0].ToString());
                //                    hashtableMaterialType.Add(strGuid, strSYSTEMCODE);
                //                }
                //            }
                //            else
                //            {
                //                //编号不为空
                //                string strUnit = OleDsExcle.Tables[0].Rows[i][8].ToString();
                //                if (strUnit != "")
                //                {//excel表中有计量单位信息，无计量单位的信息不保存
                //                    string strMaterialCategoryId = null;
                //                    foreach (System.Collections.DictionaryEntry objDE in hashtableMaterial)
                //                    {
                //                        if (objDE.Value.ToString().Equals(strCode))
                //                        {
                //                            strMaterialCategoryId = objDE.Key.ToString();
                //                            break;
                //                        }
                //                    }
                //                    //string strGuid = GetGuid();//生成Id
                //                    //有计量单位信息
                //                    string strUnitId = null;
                //                    foreach (System.Collections.DictionaryEntry value in hashtableUnit)
                //                    {
                //                        if (value.Value.ToString().Equals(strUnit))
                //                        {
                //                            strUnitId = value.Key.ToString();
                //                        }
                //                    }
                //                    string strNewCode = strCode + strTreeCode;
                //                    string strsql = "insert into ResMaterial (Version,MATERIALID,MATCODE,MATNAME,ALIAS,MaterialCategoryId,MATSPECIFICATION,Quality,PracticalityStateControlRuleId,ManageStateControlRuleId,MATATTRIBUTE,Requirement,StandardUnitID,States,CreatedDate,PERID,ModifyPerson,OperationOrg,OpgSysCode,IsAuto,IFCATRESOURCE)";
                //                    strsql += "values('0','" + strGuid + "','" + strNewCode + "','" + OleDsExcle.Tables[0].Rows[i][3].ToString() + "','" + OleDsExcle.Tables[0].Rows[i][3].ToString() + "','" + strMaterialCategoryId + "','" + OleDsExcle.Tables[0].Rows[i][4].ToString() + "','" + OleDsExcle.Tables[0].Rows[i][6].ToString() + "','2','14','0','0','" + strUnitId + "','1',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strOperOrgInfoId + "','" + strOperOrgInfoName + "','" + OperOrgInfo + "','" + strOpgSysCode + "','0','0')";
                //                    int res = model.ExcelImportSrv.SaveSql(strsql);
                //                    if (res != 0)
                //                    {
                //                        Rows += 1;
                //                    }
                //                }
                //            }
                //        }
                //        if (strCode.Length == 8)
                //        {
                //            string strRowCon = OleDsExcle.Tables[0].Rows[i][1].ToString();
                //            if (strRowCon.Equals("") || strRowCon.Equals("null"))
                //            {
                //                //第二列信息为空
                //                string strUnit = OleDsExcle.Tables[0].Rows[i][8].ToString();
                //                if (strUnit != "")
                //                {//excel表中有计量单位信息，无计量单位的信息不保存
                //                    //string streigth = strCode.Substring(0, 8);//截取前八位
                //                    string strMaterialCategoryId = null;
                //                    foreach (System.Collections.DictionaryEntry objDE in hashtableMaterial)
                //                    {
                //                        if (objDE.Value.ToString().Equals(strCode))
                //                        {
                //                            strMaterialCategoryId = objDE.Key.ToString();
                //                            break;
                //                        }
                //                    }
                //                    //string strGuid = GetGuid();//生成Id
                //                    //有计量单位信息
                //                    string strUnitId = null;
                //                    foreach (System.Collections.DictionaryEntry value in hashtableUnit)
                //                    {
                //                        if (value.Value.ToString().Equals(strUnit))
                //                        {
                //                            strUnitId = value.Key.ToString();
                //                        }
                //                    }
                //                    string strNewCode = strCode + strTreeCode;
                //                    string strsql = "insert into ResMaterial (Version,MATERIALID,MATCODE,MATNAME,ALIAS,MaterialCategoryId,MATSPECIFICATION,Quality,PracticalityStateControlRuleId,ManageStateControlRuleId,MATATTRIBUTE,Requirement,StandardUnitID,States,CreatedDate,PERID,ModifyPerson,OperationOrg,OpgSysCode,IsAuto,IFCATRESOURCE)";
                //                    strsql += "values('0','" + strGuid + "','" + strNewCode + "','" + OleDsExcle.Tables[0].Rows[i][3].ToString() + "','" + OleDsExcle.Tables[0].Rows[i][3].ToString() + "','" + strMaterialCategoryId + "','" + OleDsExcle.Tables[0].Rows[i][4].ToString() + "','" + OleDsExcle.Tables[0].Rows[i][6].ToString() + "','2','14','0','0','" + strUnitId + "','1',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strOperOrgInfoId + "','" + strOperOrgInfoName + "','" + OperOrgInfo + "','" + strOpgSysCode + "','0','0')";
                //                    int res = model.ExcelImportSrv.SaveSql(strsql);
                //                    if (res != 0)
                //                    {
                //                        Rows += 1;
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                //长度为8，RESMATERIALCATEGORY
                //                string strsix = strCode.Substring(0, 6);//截取前六位
                //                foreach (System.Collections.DictionaryEntry objDE in hashtableMaterial)
                //                {
                //                    if (objDE.Value.ToString().Equals(strsix))
                //                    {
                //                        strId = objDE.Key.ToString();
                //                        foreach (System.Collections.DictionaryEntry objType in hashtableMaterialType)
                //                        {
                //                            if (objType.Key.ToString().Equals(strId))
                //                            {
                //                                strParentId = objType.Value.ToString();
                //                            }
                //                        }
                //                        break;
                //                    }
                //                }
                //                //string strGuid = GetGuid();//生成Id
                //                string strSYSTEMCODE = strParentId + strGuid + ".";
                //                string strsql = "insert into RESMATERIALCATEGORY (ID,NAME,NODETYPE,CODE,CREATEDATE,SYSCODE,STATE,numLEVEL,PARENTNODEID,THETREEID,MATCATATTRUBUTE,PraStateControlRuleID,ManStateControlRuleID,PERID,OrderNo,MatCatKind,OperationOrg,OpgSysCode,ABBREVIATION)";
                //                strsql += "values ('" + strGuid + "','" + strName2 + "','2','" + OleDsExcle.Tables[0].Rows[i][0].ToString() + "',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strSYSTEMCODE + "','1','5','" + strId + "','7','0','2','14','" + strOperOrgInfoName + "','1','0','" + OperOrgInfo + "','" + strOpgSysCode + "','" + strName2 + "')";
                //                //int res = savesql(command, sql);
                //                int res = model.ExcelImportSrv.SaveSql(strsql);
                //                if (res != 0)
                //                {
                //                    Rows += 1;
                //                    hashtableMaterial.Add(strGuid, OleDsExcle.Tables[0].Rows[i][0].ToString());
                //                    hashtableMaterialType.Add(strGuid, strSYSTEMCODE);
                //                }
                //            }
                //        }
                //        if (strTreeCode.Equals("") && strCode != "" && strName2 != "")
                //        {
                //            //将分类信息保存到ResMaterial数据表中，只保存ID和名称
                //            string strNewCode = strCode + "00000";
                //            string strNewGuid = GetGuid();
                //            string strNewUnitId = null;
                //            string strNewUnitName = "个";
                //            foreach (System.Collections.DictionaryEntry value in hashtableUnit)
                //            {
                //                if (value.Value.ToString().Equals(strNewUnitName))
                //                {
                //                    strNewUnitId = value.Key.ToString();
                //                }
                //            }


                //            string strsql = "insert into ResMaterial (Version,MATERIALID,MATCODE,MATNAME,MaterialCategoryId,PracticalityStateControlRuleId,ManageStateControlRuleId,MATATTRIBUTE,Requirement,States,CreatedDate,PERID,ModifyPerson,OperationOrg,OpgSysCode,IsAuto,IFCATRESOURCE,ALIAS,STANDARDUNITID)";
                //            strsql += "values('0','" + strNewGuid + "','" + strNewCode + "','" + strName2 + "','" + strGuid + "','2','14','0','0','1',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strOperOrgInfoId + "','" + strOperOrgInfoName + "','" + OperOrgInfo + "','" + strOpgSysCode + "','0','1','" + strName2 + "','" + strNewUnitId + "')";
                //            int res = model.ExcelImportSrv.SaveSql(strsql);

                //        }
                //    }
                //}

                //MessageBox.Show(Rows + "条信息保存成功！");
                ExploreFile.CloseExcel();
            }
            catch (Exception err)
            {
                MessageBox.Show("数据绑定Excel失败!失败原因：" + err.Message, "提示信息",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 基础数据
        protected void ImportExcel1(string SExcelFileName)
        {
            try
            {

                DataSet OleDsExcle = ExploreFile.OpenExcel(SExcelFileName);//打开Excel连接，获取Excel数据
                //所连接的数据库表为THD_BasicDataOptr
                model.ExcelImportSrv.SaveBasic(OleDsExcle, projectInfo, strOperOrgInfoId);
                ExploreFile.CloseExcel();
            }
            catch (Exception err)
            {
                MessageBox.Show("数据绑定Excel失败!失败原因：" + err.Message, "提示信息",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 工程WBS任务类型
        private void ImportExcel2(string SExcelFileName)
        {
            try
            {
                DataSet OleDsExcle = ExploreFile.OpenExcel(SExcelFileName);//打开Excel连接，读取excel信息
                model.ExcelImportSrv.SaveGWBS(OleDsExcle, projectInfo, strOperOrgInfoId);
                ExploreFile.CloseExcel();
            }
            catch (Exception err)
            {
                MessageBox.Show("数据绑定Excel失败!失败原因：" + err.Message, "提示信息",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        void btnCancel_Click(object sender, EventArgs e)
        {
            txtfileName.Text = "";
        }

        #region 批量添加
        void btnAdd_Click(object sender, EventArgs e)
        {
            string strName = txtCategory.SelectedNode.Text;
            VFlexCellImport frm = new VFlexCellImport(strName);
            frm.ShowDialog();
        }
        #endregion
    }
}

