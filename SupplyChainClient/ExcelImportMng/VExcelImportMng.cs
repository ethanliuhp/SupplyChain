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
            txtCategory.Nodes.Add("���Ϸ����������");
            txtCategory.Nodes.Add("����WBS��������");
            txtCategory.Nodes.Add("��������");
            txtCategory.Nodes.Add("�ɱ������Ŀ");
            txtCategory.Nodes.Add("�ɱ�������");
            txtCategory.Nodes.Add("�ɱ����������");
            txtCategory.Nodes.Add("��ƿ�Ŀ");
            txtCategory.Nodes.Add("�ɱ��װ��������");
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
            openFile.Filter = "�����ļ�(*.*)|*.*";
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
                    MessageBox.Show("�ļ�̫��ϵͳ����ʧ�ܣ�", "ϵͳ��ʾ", MessageBoxButtons.OK);
                }
            }
            return fileName;
        }

        #region Id���ɷ���
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


        #region ������½��Ϣ
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
            string extFile = Path.GetExtension(strFilePash).ToUpper();//���Fielupload��ֵ
            if (extFile == ".XLS" || extFile == ".XLSX")//����ļ���.XLS��ʽ��
            {
                //string str = System.IO.Path.GetFileName(strFilePash);
                string str = strFilePash;
                string strName = txtCategory.SelectedNode.Text;
                if (strName.Equals("���Ϸ����������"))
                {
                    ImportExcel(str);
                }
                if (strName.Equals("����WBS��������"))
                {
                    ImportExcel2(str);
                }
                if (strName.Equals("��������"))
                {
                    ImportExcel1(str);
                }
                if (strName.Equals("�ɱ������Ŀ"))
                {
                    ImportExcel3(str);
                }
                if (strName.Equals("�ɱ�������"))
                {
                    ImportExcel4(str);
                }
                if (strName.Equals("�ɱ����������"))
                {
                    ImportExcel5(str);
                }
                if (strName.Equals("��ƿ�Ŀ"))
                {
                    ImportExcel6(str);
                }
                if (strName.Equals("�ɱ��װ��������"))
                {
                    ImportExcelInstall(str);
                }
            }
            else
            {
                MessageBox.Show("�ļ���ʽ����ȷ�����֤���ٲ�����");
            }
        }

        #region ��ƿ�Ŀ
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
                MessageBox.Show("���ݰ�Excelʧ��!ʧ��ԭ��" + err.Message, "��ʾ��Ϣ",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region �ɱ������
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
                MessageBox.Show("���ݰ�Excelʧ��!ʧ��ԭ��" + err.Message, "��ʾ��Ϣ",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region �ɱ���
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
                MessageBox.Show("���ݰ�Excelʧ��!ʧ��ԭ��" + err.Message, "��ʾ��Ϣ",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region �ɱ��װ����
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
                MessageBox.Show("���ݰ�Excelʧ��!ʧ��ԭ��" + err.Message, "��ʾ��Ϣ",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region �ɱ������Ŀ
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
                MessageBox.Show("���ݰ�Excelʧ��!ʧ��ԭ��" + err.Message, "��ʾ��Ϣ",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region ���Ϸ����������
        protected void ImportExcel(string SExcelFileName)
        {
            try
            {
                DataSet OleDsExcle = ExploreFile.OpenExcel(SExcelFileName);
                model.ExcelImportSrv.SaveMaterial(OleDsExcle, projectInfo, strOperOrgInfoId, strOperOrgInfoName, strOperOrgInfo, strOpgSysCode);
                //�����ݿ����ӡ�����command
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

                //    for (int i = 1; i < OleDsExcle.Tables[0].Rows.Count; i++)//ѭ����ȡ��ʱ������
                //    {
                //        string strCode = OleDsExcle.Tables[0].Rows[i][0].ToString();//��ȡ��ʱ����i�е�j�е�����
                //        string strTreeCode = OleDsExcle.Tables[0].Rows[i][2].ToString();
                //        string strName2 = OleDsExcle.Tables[0].Rows[i][1].ToString();
                //        string strGuid = GetGuid();//����Id
                //        if (strCode.Length == 4)
                //        {
                //            //����Ϊ4��RESMATERIALCATEGORY
                //            //string strGuid = GetGuid();//����Id
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
                //            //����Ϊ6��RESMATERIALCATEGORY
                //            string strCodeThree = OleDsExcle.Tables[0].Rows[i][2].ToString();
                //            if (strCodeThree == "")
                //            {
                //                string strfour = strCode.Substring(0, 4);//��ȡǰ��λ
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
                //                //string strGuid = GetGuid();//����Id
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
                //                //��Ų�Ϊ��
                //                string strUnit = OleDsExcle.Tables[0].Rows[i][8].ToString();
                //                if (strUnit != "")
                //                {//excel�����м�����λ��Ϣ���޼�����λ����Ϣ������
                //                    string strMaterialCategoryId = null;
                //                    foreach (System.Collections.DictionaryEntry objDE in hashtableMaterial)
                //                    {
                //                        if (objDE.Value.ToString().Equals(strCode))
                //                        {
                //                            strMaterialCategoryId = objDE.Key.ToString();
                //                            break;
                //                        }
                //                    }
                //                    //string strGuid = GetGuid();//����Id
                //                    //�м�����λ��Ϣ
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
                //                //�ڶ�����ϢΪ��
                //                string strUnit = OleDsExcle.Tables[0].Rows[i][8].ToString();
                //                if (strUnit != "")
                //                {//excel�����м�����λ��Ϣ���޼�����λ����Ϣ������
                //                    //string streigth = strCode.Substring(0, 8);//��ȡǰ��λ
                //                    string strMaterialCategoryId = null;
                //                    foreach (System.Collections.DictionaryEntry objDE in hashtableMaterial)
                //                    {
                //                        if (objDE.Value.ToString().Equals(strCode))
                //                        {
                //                            strMaterialCategoryId = objDE.Key.ToString();
                //                            break;
                //                        }
                //                    }
                //                    //string strGuid = GetGuid();//����Id
                //                    //�м�����λ��Ϣ
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
                //                //����Ϊ8��RESMATERIALCATEGORY
                //                string strsix = strCode.Substring(0, 6);//��ȡǰ��λ
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
                //                //string strGuid = GetGuid();//����Id
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
                //            //��������Ϣ���浽ResMaterial���ݱ��У�ֻ����ID������
                //            string strNewCode = strCode + "00000";
                //            string strNewGuid = GetGuid();
                //            string strNewUnitId = null;
                //            string strNewUnitName = "��";
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

                //MessageBox.Show(Rows + "����Ϣ����ɹ���");
                ExploreFile.CloseExcel();
            }
            catch (Exception err)
            {
                MessageBox.Show("���ݰ�Excelʧ��!ʧ��ԭ��" + err.Message, "��ʾ��Ϣ",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region ��������
        protected void ImportExcel1(string SExcelFileName)
        {
            try
            {

                DataSet OleDsExcle = ExploreFile.OpenExcel(SExcelFileName);//��Excel���ӣ���ȡExcel����
                //�����ӵ����ݿ��ΪTHD_BasicDataOptr
                model.ExcelImportSrv.SaveBasic(OleDsExcle, projectInfo, strOperOrgInfoId);
                ExploreFile.CloseExcel();
            }
            catch (Exception err)
            {
                MessageBox.Show("���ݰ�Excelʧ��!ʧ��ԭ��" + err.Message, "��ʾ��Ϣ",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region ����WBS��������
        private void ImportExcel2(string SExcelFileName)
        {
            try
            {
                DataSet OleDsExcle = ExploreFile.OpenExcel(SExcelFileName);//��Excel���ӣ���ȡexcel��Ϣ
                model.ExcelImportSrv.SaveGWBS(OleDsExcle, projectInfo, strOperOrgInfoId);
                ExploreFile.CloseExcel();
            }
            catch (Exception err)
            {
                MessageBox.Show("���ݰ�Excelʧ��!ʧ��ԭ��" + err.Message, "��ʾ��Ϣ",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        void btnCancel_Click(object sender, EventArgs e)
        {
            txtfileName.Text = "";
        }

        #region ��������
        void btnAdd_Click(object sender, EventArgs e)
        {
            string strName = txtCategory.SelectedNode.Text;
            VFlexCellImport frm = new VFlexCellImport(strName);
            frm.ShowDialog();
        }
        #endregion
    }
}
