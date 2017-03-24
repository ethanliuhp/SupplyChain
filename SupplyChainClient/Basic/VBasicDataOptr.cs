using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonForm;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;

namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    public partial class VBasicDataOptr : TBasicDataView
    {
        static CurrentProjectInfo projectInfo = null;

        MStockMng mStockIn = new MStockMng();
        BasicDataOptr currBasicData = null;
        Hashtable query_ht = new Hashtable();

        #region ������������
        /// <summary>
        /// ��ά��ʹ������
        /// </summary>
        public static string BASICDATANAME_QRCODETYPE = "��ά��ʹ������";
        /// <summary>
        /// רҵ����
        /// </summary>
        public static string BASICDATANAME_BillTYPE = "Ʊ������";
        /// <summary>
        /// ר���������
        /// </summary>
        public static string BASICDATANAME_ZXCOSTTYPE = "ר���������";
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public static string BASICDATANAME_PROJECTTYPE = "��Ŀ����";
        /// <summary>
        /// ��Ŀʩ���׶�
        /// </summary>
        public static string BASICDATANAME_PROJECTLIFTCYCLE = "������Ŀʩ���׶�";
        /// <summary>
        /// ������Ŀִ��״̬
        /// </summary>
        public static string BASICDATANAME_PROJECTCURRSTATE = "������Ŀִ��״̬";
        /// <summary>
        /// ����OBS����
        /// </summary>
        public static string BASICDATANAME_OBSTYPE = "����OBS����";
        /// <summary>
        /// �а���ʽ
        /// </summary>
        public static string BASICDATANAME_CONTRACTWAY = "�а���ʽ";
        /// <summary>
        /// ������ص�λ����
        /// </summary>
        public static string BASICDATANAME_PROJECTDEPARTTYPE = "������ص�λ����";
        /// <summary>
        /// רҵ����
        /// </summary>
        public static string BASICDATANAME_PROFESSIONALCATEGORY = "רҵ����";
        /// <summary>
        /// ������Ŀ�����ȼ�
        /// </summary>
        public static string BASICDATANAME_PROJECTLIVEL = "������Ŀ�����ȼ�";
        /// <summary>
        /// ������Ŀ��ȫ�ȼ�
        /// </summary>
        public static string BASICDATANAME_PROJECTSAFTY = "������Ŀ��ȫ�ȼ�";
        /// <summary>
        /// רҵ�߻�����
        /// </summary>
        public static string BASICDATANAME_ENGINNERTYPE = "רҵ�߻�����";
        /// <summary>
        /// רҵ�ְ���������
        /// </summary>
        public static string BASICDATANAME_SUBCONTRACTPEOJECT = "רҵ�ְ���������";
        /// <summary>
        /// ���ʵ���
        /// </summary>
        public static string BASICDATANAME_MATERIALGRADE = "���ʵ���";
        /// <summary>
        /// ��������
        /// </summary>
        public static string BASICDATANAME_COSTTYPE = "��������";
        /// <summary>
        /// ��λ����
        /// </summary>
        public static string BASICDATANAME_POSTTYPE = "��λ����";
        /// <summary>
        /// ������������
        /// </summary>
        public static string BASICDATANAME_EXPENSECOSTTYPE = "������������";
        /// <summary>
        /// �������
        /// </summary>
        public static string BASICDATANAME_WOKERTYPE = "����";
        /// <summary>
        /// ��������
        /// </summary>
        public static string BASICDATANAME_SAFTY = "��������";
        /// <summary>
        /// �������
        /// </summary>
        public static string BASICDATANAME_OBSSERVICE = "OBS����״̬";
        /// <summary>
        /// ������Ŀ������ʽ
        /// </summary>
        public static string BASIC_FORM = "������Ŀ������ʽ";
        /// <summary>
        /// ������Ŀ�ṹ��ʽ
        /// </summary>
        public static string STRUCT_FORM = "������Ŀ�ṹ��ʽ";
   
        /// <summary>
        /// �豸���޷���
        /// </summary>
        public static string BASICDATANAME_MATERIALRENTAL = "�豸���޷���";
        /// <summary>
        /// �����������
        /// </summary>
        public static string BASICDATANAME_USEDRANKTYPE = "�����������";
        /// <summary>
        /// �����������
        /// </summary>
        public static string BASICDATANAME_CONSTRACTTYPE = "����ְ��а���ʽ";
        /// <summary>
        /// ������λ
        /// </summary>
        public static string BASICDATANAME_MOVEPROJECT = "������λ";
        #region ��Ŀ���Ͼ�
        /// <summary>
        /// �Ͼ߷�������
        /// </summary>
        public static string BASICDATENAME_MATERIALCOSTTYPE = "�Ͼ߷�������";
        /// <summary>
        /// �۸�����
        /// </summary>
        public static string BASICDATENAME_PRICETYPE = "�۸�����";
        /// <summary>
        /// �Ͼ�ά������
        /// </summary>
        public static string BASICDATENAME_MATREPAIRCON = "�Ͼ�ά������";
        #endregion
        #region �Ͼ�վ
        /// <summary>
        /// �Ͼ߷�������
        /// </summary>
        public static string BASICDATENAME_StationMATERIALCOSTTYPE = "�Ͼ�վ��������";
        /// <summary>
        /// �۸�����
        /// </summary>
        public static string BASICDATENAME_StationPRICETYPE = "�Ͼ�վ�۸�����";
        /// <summary>
        /// �Ͼ�ά������
        /// </summary>
        public static string BASICDATENAME_StationMATREPAIRCON = "�Ͼ�վ������ά������";
        /// <summary>
        /// �Ͼ�վ�ߴ�ֶ�ͳ������
        /// </summary>
        public static string BASICDATENAME_StationMaterialSize = "�Ͼ�վ�ߴ�ֶ�ͳ������";
        /// <summary>
        /// �Ͼ�վ֧����ϵ�ֲ�������
        /// </summary>
        public static string BASICDATENAME_StationMaterialDistribute = "�Ͼ�վ֧����ϵ�ֲ�������";
        #endregion
        /// <summary>
        /// ��������
        /// </summary>
        public static string BASICDATANAME_COMPILATION = "��������";
        /// <summary>
        /// ����״̬
        /// </summary>
        public static string BASICDATANAME_PROJECTSTATUS = "����״̬";
        public static string BASICDATANAME_PROJECTSTATE = "��Ŀ����״̬";
        /// <summary>
        /// ֧����ʽ
        /// </summary>
        public static string BASICDATANAME_PAYMENT = "֧����ʽ";
        /// <summary>
        /// ���ս��㵥������ܷ���
        /// </summary>
        public static string BASICDATANAME_StockBalNoSumCategory = "���ս��㵥������ܷ���";

        /// <summary>
        /// Ԥ�����ʷ���
        /// </summary>
        public static string BASICDATANAME_WarningCategory = "Ԥ�����ʷ���";

        /// <summary>
        /// ʩ��רҵ
        /// </summary>
        public static string BASICDATANAME_ProfessionalConstruction = "ʩ��רҵ";
        #endregion

        #region �ɱ����㲿��
        /// <summary>
        /// PBS�ṹ����
        /// </summary>
        public static string PBS_StructType = "�ṹ����";
         /// <summary>
        /// ��������
        /// </summary>
        public static string Letters_Style = "��������";
        /// �շ�������
        /// </summary>
        public static string Send_Style = "�շ�������";
        /// <summary>
        /// WBS�����������ͼ��Ҫ��
        /// </summary>
        public static string WBS_CheckRequire = "���Ҫ��";
        /// <summary>
        /// ����ģʽ���ɱ���������ģʽ��
        /// </summary>
        public static string CostItem_ManagementMode = "����ģʽ";

        /// <summary>
        /// ��Լ������
        /// </summary>
        public static string WBS_ContractGroupType = "��Լ������";

        /// <summary>
        /// ��Լ��ϸ����
        /// </summary>
        public static string WBS_ContractGroupDetailType = "��Լ��ϸ����";

        /// <summary>
        /// ���ȼƻ��汾
        /// </summary>
        public static string PLAN_ScheduleVersion = "���ȼƻ��汾";

        /// <summary>
        /// ���ȼƻ��ھ�
        /// </summary>
        public static string PLAN_ScheduleCaliber = "���ȼƻ��ھ�";

        /// <summary>
        /// �ܽ��ȼƻ�����
        /// </summary>
        public static string PLAN_ScheduleType = "�ܽ��ȼƻ�����";

        /// <summary>
        /// �ܹ������ȼƻ�����
        /// </summary>
        public static string PLAN_ScheduleTypeRolling = "�ܹ������ȼƻ�����";

        /// <summary>
        /// ���ڼ�����λ
        /// </summary>
        public static string PLAN_ScheduleUnit = "���ڼ�����λ";

        /// <summary>
        /// ��Դ����ƻ�����
        /// </summary>
        public static string PLAN_ResourceRequirePlanType = "��Դ����ƻ�����";

        /// <summary>
        /// ִ�н��ȼƻ�����
        /// </summary>
        public static string PLAN_ExecScheduleType = "ִ�н��ȼƻ�����";

        public static string PLAN_MonthScheduleDefaultDateArea= "�½��ȼƻ�Ĭ��ʱ���";

        public static string PLAN_ResourceRequirePlanTypeCate = "��Դ����ƻ����ͱ���";
        public static string SelFeeTemplateSpecialType = "ȡ��רҵ����";
        #endregion

        #region ָ������������
        /// <summary>
        /// ָ����������λ
        /// </summary>
        public static string INDICATOR_Unit = "ָ����������λ";
        /// <summary>
        /// ָ�����������
        /// </summary>
        public static string INDICATOR_ReportType = "ָ�����������";

        /// <summary>
        /// ��ʼ�� ָ����������� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitIndicatorReportType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.INDICATOR_ReportType);
            InitDropDownObjectByDataSource(dropDownObject, addBlank, list, "--��ѡ��--");
        }

        /// <summary>
        /// ��ʼ�� ָ����������λ ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitIndicatorUnit(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.INDICATOR_Unit);
            InitDropDownObjectByDataSource(dropDownObject, addBlank, list, "--��ѡ��--");
        }
        #endregion

        #region �������ݳ�ʼ�������𷽷�

        /// <summary>
        /// ��ʼ�� ������ ͨ����DataSource��ֵ�ķ�ʽ
        /// </summary>
        /// <param name="dropDownObject">���������</param>
        /// <param name="addBlank">Ϊtrueʱ����һ����</param>
        /// <param name="list">Ҫ��ʾ�Ļ�������</param>
        /// <param name="blankValueName">���һ�п���ʱ�����д�������ʾ������</param>
        private static void InitDropDownObjectByDataSource(object dropDownObject, bool addBlank, IList list, string blankValueName)
        {
            if (list != null)
            {
                if (addBlank)
                {
                    BasicDataOptr bdo = new BasicDataOptr();
                    bdo.BasicCode = "";
                    bdo.BasicName = blankValueName;
                    bdo.Id = "";
                    bdo.ParentId = "";
                    bdo.State = 0;
                    list.Insert(0, bdo);
                }
                if (dropDownObject.GetType() == typeof(DataGridViewComboBoxColumn))
                {
                    ((DataGridViewComboBoxColumn)dropDownObject).DataSource = list;
                    ((DataGridViewComboBoxColumn)dropDownObject).DisplayMember = "BasicName";
                    ((DataGridViewComboBoxColumn)dropDownObject).ValueMember = "BasicCode";

                }
                if (dropDownObject.GetType() == typeof(CustomComboBox))
                {
                    ((CustomComboBox)dropDownObject).DataSource = list;
                    ((CustomComboBox)dropDownObject).DisplayMember = "BasicName";
                    ((CustomComboBox)dropDownObject).ValueMember = "BasicCode";
                }
                if (dropDownObject.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)dropDownObject).DataSource = list;
                    ((ComboBox)dropDownObject).DisplayMember = "BasicName";
                    ((ComboBox)dropDownObject).ValueMember = "BasicCode";
                }
            }
        }

        /// <summary>
        /// ��ʼ�� ������
        /// </summary>
        /// <param name="dropDownObject"></param>
        /// <param name="addBlank"></param>
        /// <param name="list"></param>
        private static void InitDropDownObject(object dropDownObject, bool addBlank, IList list)
        {
            if (list != null)
            {
                if (dropDownObject.GetType() == typeof(DataGridViewComboBoxColumn))
                {
                    if (addBlank) ((DataGridViewComboBoxColumn)dropDownObject).Items.Add("");
                    foreach (BasicDataOptr bdo in list)
                    {
                        ((DataGridViewComboBoxColumn)dropDownObject).Items.Add(bdo.BasicName);
                    }
                }
                if (dropDownObject.GetType() == typeof(CustomComboBox))
                {
                    if (addBlank) ((CustomComboBox)dropDownObject).Items.Add("");
                    foreach (BasicDataOptr bdo in list)
                    {
                        ((CustomComboBox)dropDownObject).Items.Add(bdo.BasicName);
                    }
                }
                if (dropDownObject.GetType() == typeof(ComboBox))
                {
                    if (addBlank) ((ComboBox)dropDownObject).Items.Add("");
                    foreach (BasicDataOptr bdo in list)
                    {
                        ((ComboBox)dropDownObject).Items.Add(bdo.BasicName);
                    }
                }
            }
        }
        /// <summary>
        /// ��ʼ�� ��ά��ʹ������
        /// </summary
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitQRCodeType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_QRCODETYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// ��ʼ���ṹ����������
        /// </summary>
        /// <param name="dropDownObject"></param>
        /// <param name="addBlank"></param>
        public static void InitImplantType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PBS_StructType);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// ��ʼ�� ִ�н��ȼƻ����� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitExecScheduleType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ExecScheduleType);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// ȡ��רҵ����
        /// </summary>
        /// <returns></returns>
        public static List<BasicDataOptr> GetSelFeeTemplateSpecialType()
        {
            IList lst = StaticMethod.GetBasicDataByName(VBasicDataOptr.SelFeeTemplateSpecialType);
            return lst == null || lst.Count == 0 ? null : lst.OfType < BasicDataOptr>().ToList();
        }
        /// <summary>
        /// ��ʼ���շ�������
        /// </summary>
        /// <param name="dropDownObject"></param>
        /// <param name="addBlank"></param>
        public static void SendStyleType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.Send_Style);
            InitDropDownObject(dropDownObject, addBlank, list);
        }


        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="dropDownObject"></param>
        /// <param name="addBlank"></param>
        public static void LettersStyleType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.Letters_Style);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� ���ڼ�����λ ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitScheduleUnit(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ScheduleUnit);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� ������Ŀ������ʽ ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitBasicFrom(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASIC_FORM);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// ��ʼ�� ʩ���׶� ������
        /// </summary
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitProjectConstractStage(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTLIFTCYCLE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
         
        /// <summary>
        /// ��ʼ�� ������Ŀִ��״̬ ������
        /// </summary
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitProjectCurrState(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTCURRSTATE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// ��ʼ�� ��Ŀ���� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitProjectType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// ��ʼ�� ����OBS���� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitOBSType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_OBSTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// ��ʼ�� ��Դ����ƻ����� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitResReqirePlan(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ResourceRequirePlanType);
            InitDropDownObject(dropDownObject, addBlank, list);
        }


        /// <summary>
        /// ��ʼ�� �а���ʽ ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitContractWay(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_CONTRACTWAY);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// ��ʼ�� ר��������� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitZXCostType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_ZXCOSTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        
        /// <summary>
        /// ��ʼ�� ������Ŀ�ṹ��ʽ ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitStructFrom(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.STRUCT_FORM);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� �ܽ��ȼƻ����� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitScheduleType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByNameAndProjectName(VBasicDataOptr.PLAN_ScheduleType, projectInfo.Name);
            if (list != null && list.Count > 0)
            {
                InitDropDownObject(dropDownObject, addBlank, list);
            }
            else
            {
                if (dropDownObject.GetType() == typeof(CustomComboBox))
                {
                    if (addBlank) ((CustomComboBox)dropDownObject).Items.Add("");
                    ((CustomComboBox)dropDownObject).Items.Add("�������ȼƻ�");
                    ((CustomComboBox)dropDownObject).Items.Add("��װ���ȼƻ�");
                }
                if (dropDownObject.GetType() == typeof(ComboBox))
                {
                    if (addBlank) ((ComboBox)dropDownObject).Items.Add("");
                    ((ComboBox)dropDownObject).Items.Add("�������ȼƻ�");
                    ((ComboBox)dropDownObject).Items.Add("��װ���ȼƻ�");
                }
            }
        }

        /// <summary>
        /// ��ʼ�� �ܹ������ȼƻ����� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitScheduleTypeRolling(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByNameAndProjectName(VBasicDataOptr.PLAN_ScheduleTypeRolling, projectInfo.Name);
            if (list != null && list.Count > 0)
            {
                InitDropDownObject(dropDownObject, addBlank, list);
            }
            else
            {
                if (dropDownObject.GetType() == typeof(CustomComboBox))
                {
                    if (addBlank) ((CustomComboBox)dropDownObject).Items.Add("");
                    ((CustomComboBox)dropDownObject).Items.Add("�����ܹ������ȼƻ�");
                    ((CustomComboBox)dropDownObject).Items.Add("��װ�ܹ������ȼƻ�");
                }
                if (dropDownObject.GetType() == typeof(ComboBox))
                {
                    if (addBlank) ((ComboBox)dropDownObject).Items.Add("");
                    ((ComboBox)dropDownObject).Items.Add("�����ܹ������ȼƻ�");
                    ((ComboBox)dropDownObject).Items.Add("��װ�ܹ������ȼƻ�");
                }
            }
        }

        /// <summary>
        /// ���ݵ�ǰ��Ŀ��ʼ����������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitBasicDataByCurrProjectInfo(string basicName, object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByNameAndProjectName(basicName, projectInfo.Name);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� ֧����ʽ ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitPayment(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PAYMENT);
            InitDropDownObject(dropDownObject, addBlank, list);
        }


        /// <summary>
        /// ��ʼ�� ���ȼƻ��ھ� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitScheduleCaliber(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ScheduleCaliber);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� ���ȼƻ��汾 ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitScheduleVersion(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ScheduleVersion);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        //
        /// <summary>
        /// ��ʼ�� ������ص�λ���� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitProjectDepartType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTDEPARTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� רҵ���� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitProfessionCategory(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROFESSIONALCATEGORY);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        public static void InitBillType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_BillTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// ��ʼ�� ������Ŀ�����ȼ� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitProjectLivel(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTLIVEL);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� ������Ŀ��ȫ�ȼ� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitProjectSafty(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTSAFTY);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� רҵ�߻����� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitEnginnerType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_ENGINNERTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        
        /// <summary>
        /// ��ʼ�� ���� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitWokerType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_WOKERTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        
        /// <summary>
        /// ��ʼ�� �������� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitDangerType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_SAFTY);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� OBS����״̬ ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitOBSService(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_OBSSERVICE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� �豸���޷��� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitMaterialRental(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_MATERIALRENTAL);
            InitDropDownObject(dropDownObject, addBlank, list);
        }


        /// <summary>
        /// ��ʼ�� ������������ ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitExpensesCose(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_EXPENSECOSTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� ����������� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitUsedRankType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_USEDRANKTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� רҵ�ְ��������� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitSubContractProject(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_SUBCONTRACTPEOJECT);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� ����ְ��а���ʽ ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitContractType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_CONSTRACTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� ����״̬ ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitProjectStatus(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTSTATUS);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        public static void InitProjectState(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTSTATE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� ���ʵ��� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitMaterialGrade(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_MATERIALGRADE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� �������� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitDemandCompilation(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_COMPILATION);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� �������� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitCostType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_COSTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// ��ʼ�� ��λ���� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitPostType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_POSTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }


        /// <summary>
        /// ��ʼ�� ������λ ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitMoveProject(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_MOVEPROJECT);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        #region ��Ŀ���Ͼ�
        /// <summary>
        /// ��ʼ �Ͼ߷������� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitMatCostType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_MATERIALCOSTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        
        /// <summary>
        /// ��ʼ �۸����� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitPriceType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_PRICETYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// ��ʼ �Ͼ�ά������ ������(�����Ͼ����ͼ���ά������)
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitMatRepairCon(object dropDownObject, bool addBlank, Material material)
        {
            IList list_BasicData = new ArrayList();
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_MATREPAIRCON);
            foreach (BasicDataOptr basicData in list)
            {
                if (basicData.BasicCode == material.Name)
                {
                    list_BasicData.Add(basicData);
                }
            }
            InitDropDownObject(dropDownObject, addBlank, list_BasicData);
        }
        #endregion
        #region �Ͼ�վ���Ͼ�
        public static  IList GetData(IList lstData,string sMaterialName)
        {//1����עΪ�� ���������κ�����ƥ�� ѡ��    
            //2����ע�а�����������  ѡ�� 
            //3�������һ�ֺ͵ڶ��ֻ�ȡ�Ķ��ǿ� ���б�ע��ϢΪ[����]�Ķ���ѡ��
            IList lstResult = null;
            string sOtherMaterial = "����";
            IEnumerable<BasicDataOptr> lstTemp=null;
            if (lstData != null)
            {
                lstTemp=lstData.OfType<BasicDataOptr>();
                lstResult = lstTemp.Where(a =>
                    string.IsNullOrEmpty(a.Descript) ||(a.Descript!=sOtherMaterial && a.Descript.Split('|').Contains(sMaterialName))).ToList();
                if (lstResult == null || lstResult.Count == 0)
                {
                    lstResult = lstTemp.Where(a=>a.Descript == sOtherMaterial).ToList();
                }
            }
            return lstResult;
        }
        /// <summary>
        /// ��ʼ �Ͼ�վ �Ͼ߷������� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitStationMatCostType(object dropDownObject, bool addBlank, string sMaterianName)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_StationMATERIALCOSTTYPE);
            if (!string.IsNullOrEmpty(sMaterianName))
            {
                list = GetData(list, sMaterianName);
            }
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// �����������ƻ�ȡ
        /// </summary>
        /// <param name="sMaterianName"></param>
        /// <returns></returns>
        public static IList GetStationMatCostType(string sMaterianName)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_StationMATERIALCOSTTYPE);
            if (!string.IsNullOrEmpty(sMaterianName))
            {
                list = GetData(list, sMaterianName);
            }
            return list;
        }
        /// <summary>
        /// ��ȡ �Ͼ�վ�ߴ�ֶ�ͳ������
        /// </summary>
        /// <returns></returns>
        public static IList GetStationMaterialSize()
        {
            return StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_StationMaterialSize);
        }
        /// <summary>
        /// ��ȡ �Ͼ�վ֧����ϵ�ֲ�������
        /// </summary>
        /// <returns></returns>
        public static IList GetStationMaterialDistribute()
        {
            return StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_StationMaterialDistribute);
        }
        /// <summary>
        /// ��ʼ �Ͼ�վ �۸����� ������
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitStationPriceType(object dropDownObject, bool addBlank, string sMaterianName)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_StationPRICETYPE);
            if (!string.IsNullOrEmpty(sMaterianName))
            {
                list = GetData(list, sMaterianName);
            }
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// ��ʼ �Ͼ�վ �Ͼ�ά������ ������(�����Ͼ����ͼ���ά������)
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn��CustomComboBox</param>
        /// <param name="addBlank">true ����һ�п�ֵ</param>
        public static void InitStationMatRepairCon(object dropDownObject, bool addBlank, string sMaterianName)
        {
            string sOtherMaterial="����";
            //IList list_BasicData = new ArrayList();
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_StationMATREPAIRCON);
            if (!string.IsNullOrEmpty(sMaterianName))
            {
                list = GetData(list, sMaterianName);
            }
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        #endregion
        
        /// <summary>
        /// ���Ҫ��(���רҵ)
        /// </summary>
        /// <param name="dropDownObject"></param>
        /// <param name="addBlank"></param>
        public static void InitWBSCheckRequir(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_CheckRequire);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        #endregion

        public VBasicDataOptr()
        {
            InitializeComponent();
            InitialEvents();

            InitData();
        }
        private void InitData()
        {
            LoadBasicDefine();
            LoadBasicDetail();
        }
        static VBasicDataOptr()
        {
            if (projectInfo == null)
                projectInfo = StaticMethod.GetProjectInfo();
        }

        internal void Start()
        {

        }

        private void InitialEvents()
        {
            btnDelJob.Click += new EventHandler(btnDelJob_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnHelp.Click += new EventHandler(btnHelp_Click);

            listBoxType.SelectedIndexChanged += new EventHandler(listBoxType_SelectedIndexChanged);

            btnAddType.Click += new EventHandler(btnAddType_Click);
            btnUpdateType.Click += new EventHandler(btnUpdateType_Click);
            btnDeleteType.Click += new EventHandler(btnDeleteType_Click);

            dgvOptr.CellDoubleClick += new DataGridViewCellEventHandler(dgvOptr_CellDoubleClick);
        }

        void btnDeleteType_Click(object sender, EventArgs e)
        {
            currBasicData = listBoxType.SelectedItem as BasicDataOptr;
            if (currBasicData == null)
            {
                MessageBox.Show("��ѡ��Ҫɾ�������ͣ�");
                return;
            }
            if (MessageBox.Show("��ȷ��Ҫɾ����" + currBasicData.BasicName + "��?", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Or(Expression.Eq("Id", currBasicData.Id), Expression.Eq("ParentId", currBasicData.Id)));

                IList list = mStockIn.StockInSrv.GetObjects(typeof(BasicDataOptr), oq);

                mStockIn.StockInSrv.DeleteByDao(list);

                listBoxType.Items.RemoveAt(listBoxType.SelectedIndex);

                dgvOptr.Rows.Clear();
                query_ht.Clear();
            }
        }

        void btnUpdateType_Click(object sender, EventArgs e)
        {
            if (currBasicData == null)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵ����ͣ�");
                listBoxType.Focus();
                return;
            }

            string typeName = txtTypeName.Text.Trim();
            if (typeName == "")
            {
                MessageBox.Show("�������������ƣ�");
                txtTypeName.Focus();
                return;
            }

            if (currBasicData.BasicName != typeName)
            {
                currBasicData.BasicName = typeName;

                currBasicData = mStockIn.StockInSrv.SaveBasicData(currBasicData);

                listBoxType.DisplayMember = "BasicName";
                listBoxType.Items[listBoxType.SelectedIndex] = currBasicData;
                listBoxType.SelectedItem = currBasicData;
            }

        }

        void btnAddType_Click(object sender, EventArgs e)
        {
            string typeName = txtTypeName.Text.Trim();
            if (typeName == "")
            {
                MessageBox.Show("�������������ƣ�");
                txtTypeName.Focus();
                return;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BasicName", typeName));
            oq.AddCriterion(Expression.Eq("State", -1));
            IList list = mStockIn.StockInSrv.GetObjects(typeof(BasicDataOptr), oq);
            if (list.Count > 0)
            {
                MessageBox.Show("�������Ѵ��ڣ�");
                txtTypeName.Focus();
                return;
            }

            BasicDataOptr addBasic = new BasicDataOptr();
            addBasic.BasicName = typeName;
            addBasic.State = -1;

            addBasic = mStockIn.StockInSrv.SaveBasicData(addBasic);

            listBoxType.Items.Add(addBasic);
            listBoxType.SelectedItem = addBasic;
            currBasicData = addBasic;

            dgvOptr.Rows.Clear();
            query_ht.Clear();
        }

        void dgvOptr_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dgvOptr.Columns[e.ColumnIndex].Name == colExtendField1.Name)
            {
                VDepartSelector frm = new VDepartSelector("1");

                frm.ShowDialog();

                if (frm.Result != null && frm.Result.Count > 0)
                {
                    CurrentProjectInfo selectProject = frm.Result[0] as CurrentProjectInfo;
                    if (selectProject != null)
                    {
                        dgvOptr.Rows[e.RowIndex].Cells[colExtendField1.Name].Value = selectProject.Name;
                    }
                }
            }
        }

        private void LoadBasicDefine()
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("State", -1));
            objectQuery.AddOrder(Order.Asc("BasicName"));

            IList list = mStockIn.StockInSrv.GetBasicData(objectQuery);
            foreach (BasicDataOptr obj in list)
            {
                listBoxType.Items.Add(obj);
            }

            try
            {
                //listBoxType.DataSource = list;
                listBoxType.DisplayMember = "BasicName";
            }
            catch (Exception e)
            {
                string a = "";
            }

            if (listBoxType.Items.Count > 0)
            {
                listBoxType.SelectedIndex = 0;
            }
        }

        private void LoadBasicDetail()
        {
            dgvOptr.Rows.Clear();
            query_ht.Clear();
            currBasicData = listBoxType.SelectedItem as BasicDataOptr;

            if (currBasicData == null) return;
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ParentId", currBasicData.Id));
            objectQuery.AddOrder(Order.Asc("Id"));
            IList list = mStockIn.StockInSrv.GetBasicData(objectQuery);
            foreach (BasicDataOptr model in list)
            {
                int rowIndex = dgvOptr.Rows.Add();
                DataGridViewRow row = dgvOptr.Rows[rowIndex];
                row.Tag = model;
                row.Cells[colExtendField1.Name].Value = model.ExtendField1;
                row.Cells["Code"].Value = model.BasicCode;
                row.Cells["BName"].Value = model.BasicName;
                row.Cells["Remark"].Value = model.Descript;
            }
        }

        void listBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {

            dgvOptr.Rows.Clear();
            query_ht.Clear();

            //��ӻ���������ϸ�б�
            try
            {
                if (currBasicData != null)
                {
                    ObjectQuery objectQuery = new ObjectQuery();
                    objectQuery.AddCriterion(Expression.Eq("ParentId", currBasicData.Id));
                    objectQuery.AddOrder(Order.Asc("Id"));
                    IList list = mStockIn.StockInSrv.GetBasicData(objectQuery);

                    foreach (BasicDataOptr mx in list)
                    {
                        int i = this.dgvOptr.Rows.Add();
                        DataGridViewRow row = dgvOptr.Rows[i];
                        row.Tag = mx;

                        row.Cells[colExtendField1.Name].Value = mx.ExtendField1;
                        row.Cells["Code"].Value = mx.BasicCode;
                        row.Cells["BName"].Value = mx.BasicName;
                        row.Cells["Remark"].Value = mx.Descript;
                        query_ht.Add(mx.Id, mx.BasicCode + "_" + mx.BasicName + "_" + mx.Descript + "_" + mx.ExtendField1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("���һ���������ϸ����", ExceptionUtil.ExceptionMessage(ex));
            }

        }

        void btnHelp_Click(object sender, EventArgs e)
        {
            //VHelp vHelp = new VHelp();
            //vHelp.helpInfo = " 1:������Ȩ�ޡ�������:¼��������  \n 2:����Ա���ơ�������ϵͳע�����Ա���� \n 3:����ע�����ɲ��������� ";
            //vHelp.ShowDialog();
        }

        void btnDelJob_Click(object sender, EventArgs e)
        {
            if (dgvOptr.CurrentRow == null || dgvOptr.CurrentRow.Tag == null)
            {
                MessageBox.Show("�������ݱ�û����ϸ��");
                return;
            }

            DialogResult dr = MessageBox.Show("ȷ��Ҫɾ����", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            try
            {

                BasicDataOptr model = dgvOptr.CurrentRow.Tag as BasicDataOptr;
                mStockIn.StockInSrv.DelBasicDataBySql(model);
                dgvOptr.Rows.Remove(dgvOptr.CurrentRow);

                ////д����־
                //LogData model_1 = new LogData();
                //model_1.Code = model.BasicName;
                //model_1.Descript = "[ɾ����������][ID:" + model.Id + "]";
                //model_1.OperPerson = LoginInfomation.LoginInfo.ThePerson.Name;
                //mOrder.InsertLogDate(model_1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ɾ������������ϸ����" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            currBasicData = listBoxType.SelectedItem as BasicDataOptr;

            //foreach (DataGridViewRow row in dgvOptr.Rows)
            //{
            //    if ((row.Cells["BName"].Value == null || "".Equals(row.Cells["BName"].Value)) && !row.IsNewRow)
            //    {
            //        string no = row.Cells["No"].Value.ToString();
            //        MessageBox.Show("���Ϊ[" + no + "]�����Ʋ���Ϊ�գ�");
            //        return;
            //    }
            //    if (!row.IsNewRow && currBasicData != null && currBasicData.Id == 4 && ClientUtil.ToString(row.Cells["Code"].Value) != "¼��" && ClientUtil.ToString(row.Cells["Code"].Value) != "���")
            //    {
            //        MessageBox.Show("�������������Ϊ[¼��]��[���]��");
            //        return;
            //    }
            //}

            foreach (DataGridViewRow row in dgvOptr.Rows)
            {
                if (!row.IsNewRow)
                {

                    if (row.Cells["BName"].Value == null || "".Equals(row.Cells["BName"].Value))
                    {
                        string no = row.Cells["No"].Value.ToString();
                        MessageBox.Show("���Ϊ[" + no + "]�����Ʋ���Ϊ�գ�");
                        return;
                    }
                    if (currBasicData != null && currBasicData.Id == "4" && ClientUtil.ToString(row.Cells["Code"].Value) != "¼��" && ClientUtil.ToString(row.Cells["Code"].Value) != "���")
                    {
                        MessageBox.Show("�������������Ϊ[¼��]��[���]��");
                        return;
                    }

                    BasicDataOptr model = row.Tag as BasicDataOptr;
                    if (model == null)
                    {
                        model = new BasicDataOptr();
                    }

                    model.BasicCode = ClientUtil.ToString(row.Cells["Code"].Value);
                    model.BasicName = ClientUtil.ToString(row.Cells["BName"].Value);
                    model.Descript = ClientUtil.ToString(row.Cells["Remark"].Value);
                    model.ParentId = currBasicData.Id;

                    object ExtendField1Value = row.Cells[colExtendField1.Name].Value;
                    model.ExtendField1 = ExtendField1Value != null ? ExtendField1Value.ToString() : "";

                    try
                    {
                        if (model.Id != null)
                        {
                            if (query_ht.Contains(model.Id))
                            {
                                string str = ClientUtil.ToString(query_ht[model.Id]);
                                if (str != model.BasicCode + "_" + model.BasicName + "_" + model.Descript + "_" + model.ExtendField1)
                                {
                                    model = mStockIn.StockInSrv.SaveBasicData(model);
                                }
                            }
                        }
                        else
                        {
                            model = mStockIn.StockInSrv.SaveBasicData(model);
                        }
                        row.Tag = model;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("�������\n" + ExceptionUtil.ExceptionMessage(ex));
                        return;
                    }
                }
            }
            ////д����־
            //LogData model_1 = new LogData();
            //model_1.Code = currBasicData.BasicName;
            //model_1.Descript = "[�����������][ID:" + currBasicData.Id + "]";
            //model_1.OperPerson = LoginInfomation.LoginInfo.ThePerson.Name;
            //mOrder.InsertLogDate(model_1);

            MessageBox.Show("����ɹ���");
        }

        private void listBoxType_MouseClick(object sender, MouseEventArgs e)
        {
            currBasicData = listBoxType.SelectedItem as BasicDataOptr;
            txtTypeName.Text = currBasicData.BasicName;

            this.btnHelp.Visible = false;
            if (currBasicData != null && currBasicData.Id == "4")
            {
                dgvOptr.Columns[this.Code.Name].HeaderText = "����Ȩ��";
                dgvOptr.Columns[this.BName.Name].HeaderText = "��Ա����";
                dgvOptr.Columns[this.Remark.Name].HeaderText = "��ע(����������[¼��]��[���])";
                this.btnHelp.Visible = true;
            }

            else
            {
                dgvOptr.Columns[colExtendField1.Name].HeaderText = "��չ�ֶ�1";
                dgvOptr.Columns[this.Code.Name].HeaderText = "����";
                dgvOptr.Columns[this.BName.Name].HeaderText = "����";
                dgvOptr.Columns[this.Remark.Name].HeaderText = "��ע";
            }
        }

        internal static void InitPBS_StructType(ComboBox txtConstructionStyle, bool p)
        {
            throw new NotImplementedException();
        }

        internal static void SendStyleType()
        {
            throw new NotImplementedException();
        }
    }
}