using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Collections;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using System.Data;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Util;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service
{
    /// <summary>
    /// ��ⵥ����
    /// </summary>
    public interface IStockInSrv : IBaseService
    {
        //IDao Dao { get; set; }

        void UpdateBillPrintTimes(int billType, string billId);
        int QueryBillPrintTimes(int billType, string billId);

        #region ���ϵ�����
        /// <summary>
        /// ��������ͳ����ⵥ
        /// </summary>
        /// <param name="condition">����</param>
        /// <returns>��ѯ���</returns>
        DataSet StockInStateSearch(string condition);
        IList ObjectQuery(Type entityType, ObjectQuery oq);

        /// <summary>
        /// ��ѯ��ⵥ
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetStockIn(ObjectQuery objectQuery);
        /// <summary>
        /// ���ս��㵥��ѯ
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetStockInBal(ObjectQuery objectQuery);

        /// <summary>
        /// ͨ��Code��ѯ��ⵥ
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        StockIn GetStockInByCode(string Code, string special, string projectId);
        StockIn GetStockInByCode(string Code,  string projectId);
        /// <summary>
        /// ͨ��ID��ѯ��ⵥ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StockIn GetStockInById(string id);

        string GetStockInID(string id);
        DateTime GetServerDateTime();
        bool CheckServerDateTime();

        StockInDtl GetStockInDtl(string stockInDtlId);

        /// <summary>
        /// ��ѯ���ϵ���ϸ
        /// </summary>
        /// <param name="stockInBalDetail">���ս��㵥��ϸ</param>
        /// <returns></returns>
        IList GetStockInDtlLst(StockInBalDetail stockInBalDetail);

        /// <summary>
        /// ��ѯ���ϵ���ϸ
        /// </summary>
        /// <param name="stockInBalDetailLst">���ս��㵥��ϸ����</param>
        /// <returns></returns>
        IList GetStockInDtlLst(List<StockInBalDetail> stockInBalDetailLst);

        /// <summary>
        /// ��ѯ��ⵥ��ϸ
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetStockInDtl(ObjectQuery oq);

        /// <summary>
        /// ����ѯ
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataSet StockInQuery(string condition);

        /// <summary>
        /// ��ѯ�����ʱ��
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetStockInDtlSeq(ObjectQuery oq);

        /// <summary>
        /// ɾ��������ⵥ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteStockIn(StockIn obj);

        /// <summary>
        /// ����������ⵥ
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockIn SaveStockIn(StockIn obj, IList movedDtlList);
        /// <summary>
        /// ɾ��������ⵥ�����ս��㵥
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
         bool DeleteStockInBalMaster(StockIn obj);
        #endregion

        #region ������ⵥ�쵥����
        /// <summary>
        /// ��ѯ���쵥
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetStockInRed(ObjectQuery oq);

        /// <summary>
        /// ����Id��ѯ���쵥
        /// </summary>
        /// <param name="stockInRedId"></param>
        /// <returns></returns>
        StockInRed GetStockInRedById(string stockInRedId);

        /// <summary>
        /// ����Code��ѯ���쵥
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        StockInRed GetStockInRedByCode(string code, string special, string projectId);
        StockInRed GetStockInRedByCode(string code, string projectId);
        /// <summary>
        /// �����������쵥
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockInRed SaveStockInRed(StockInRed obj, IList movedDtlList);

        /// <summary>
        /// ɾ���������쵥
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteStockInRed(StockInRed obj);
        #endregion

        #region �����˷���
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="hashLst">key StockIn;value id List;key StockInRed;value id List;</param>
        /// <param name="hashCode">key StockIn;value Code List;key StockInRed;value Code List;</param>
        /// <param name="year">������</param>
        /// <param name="month">������</param>
        /// <param name="tallyPersonId">������ID</param>
        /// <param name="tallyPersonName">����������</param>
        /// <param name="projectId">��Ŀ</param>
        /// <returns></returns>
        Hashtable TallyStockIn(Hashtable hashLst, Hashtable hashCode, int year, int month, string tallyPersonId, string tallyPersonName, string projectId);
        #endregion
        /// <summary>
        /// �������ս��㵥���Ҹõ����²������
        /// </summary>
        /// <param name="oStockInBalMaster"></param>
        /// <returns></returns>
        Hashtable GetDiffMonthAdjustByStockInBal(IList lstStockInBalDtlID);

        #region ���ս��㵥����
        /// <summary>
        /// ��ѯ���ս��㵥
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetStockInBalMaster(ObjectQuery oq);
        Hashtable GetStockInBal(ObjectQuery oq, CurrentProjectInfo ProjectInfo);

        Hashtable GetStockInBal(string materialCode, DateTime beginDate, DateTime endDate, CurrentProjectInfo ProjectInfo);

        IList GetStockInDtlByBal(ObjectQuery oq, string projectId, string supplierId);

        /// <summary>
        /// ͨ��Code��ѯ���ս��㵥
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        StockInBalMaster GetStockInBalMasterByCode(string code, string projectId);

        /// <summary>
        /// ͨ��id��ѯ���ս��㵥
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StockInBalMaster GetStockInBalMasterById(string id);

        /// <summary>
        /// �������ս��㵥
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockInBalMaster SaveStockInBalMaster(StockInBalMaster obj, IList movedDtlList);

        /// <summary>
        /// ���ս������ʱ�ı���
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockInBalMaster SaveStockInBalMaster2(StockInBalMaster obj, IList movedDtlList);

        /// <summary>
        /// ɾ�����ս��㵥
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteStockInBalMaster(StockInBalMaster obj);

        /// <summary>
        /// ���ս��㵥��ѯ
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataSet StockInBalQuery(string condition);

        /// <summary>
        /// ����ID��ѯ���ս��㵥��ϸ
        /// </summary>
        /// <param name="detailId"></param>
        /// <returns></returns>
        StockInBalDetail GetStockInBalDetail(string detailId);

        /// <summary>
        /// �ύ���ս��㵥
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockInBalMaster SubmitStockInBalMaster(StockInBalMaster obj, IList movedDtlList);

        /// <summary>
        /// �ύ���ս��㵥 ���ܺ󷽷�
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockInBalMaster SubmitStockInBalMaster2(StockInBalMaster obj, IList movedDtlList);
        #endregion

        #region ���ս��㵥�쵥����
        /// <summary>
        /// ��ѯ���ս��㵥�쵥
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetStockInBalRedMaster(ObjectQuery oq);

        /// <summary>
        /// ����Code��ѯ���ս��㵥�쵥
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        StockInBalRedMaster GetStockInBalRedMasterByCode(string code, string projectId);

        /// <summary>
        /// ����ID��ѯ���ս��㵥�쵥
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StockInBalRedMaster GetStockInBalRedMasterById(string id);

        /// <summary>
        /// �������ս��㵥�쵥
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockInBalRedMaster SaveStockInBalRedMaster(StockInBalRedMaster obj, IList movedDtlList);

        /// <summary>
        /// ɾ�����ս��㵥�쵥
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteStockInBalRedMaster(StockInBalRedMaster obj);
        #endregion

        #region �������ݷ���
        /// <summary>
        /// ��ѯ��������
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetBasicData(ObjectQuery objectQuery);

        /// <summary>
        /// ͨ�������������Ʋ�ѯ��������
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList GetBasicDataByParentName(string name);

        /// <summary>
        /// ͨ�������������ƺ���Ŀ��ѯ��������
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList GetBasicDataByParentNameAndProjectName(string name,string projectName);

        /// <summary>
        /// ͨ��SQLɾ����������
        /// </summary>
        /// <param name="model"></param>
        void DelBasicDataBySql(BasicDataOptr model);

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        BasicDataOptr SaveBasicData(BasicDataOptr obj);
        #endregion

        #region ��־����
        IList GetLogStatReport(string condition);
        IList GetLogData(string condition);
        /// <summary>
        /// ������־
        /// </summary>
        bool SaveLogData(LogData model);

        /// <summary>
        /// ������־ ��������˳��Ϊ BillId;OperType;Code;OperPerson;BillType;Descript
        /// </summary>
        /// <param name="args">��������˳��Ϊ BillId;OperType;Code;OperPerson;BillType;Descript</param>
        /// <returns></returns>
        bool SaveLogData(params object[] args);
        #endregion

        #region ��Ŀ��Ϣ
        /// <summary>
        /// ������Ŀ��Ϣ
        /// </summary>
        /// <returns></returns>
        CurrentProjectInfo GetProjectInfo();

        /// <summary>
        /// ������Ŀ��Ϣ
        /// </summary>
        /// <param name="ownerorgsyscode">��֯�����</param>
        /// <returns></returns>
        CurrentProjectInfo GetProjectInfo(string ownerorgsyscode);
        OperationOrgInfo GetSubCompanyOrgInfo(string ownerorgsyscode);
        /// <summary>
        /// ����ID������Ŀ
        /// </summary>
        /// <param name="projectId">��ĿID</param>
        /// <returns></returns>
        CurrentProjectInfo GetProjectInfoById(string projectId);

        /// <summary>
        /// ���湤����Ŀ��Ϣ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        CurrentProjectInfo SaveCurrentProjectInfo(CurrentProjectInfo obj);
        /// <summary>
        /// ɾ��������Ŀ��Ϣ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteCurrentProjectInfo(CurrentProjectInfo obj);

        /// <summary>
        ///�ж��Ƿ�ʹ��SQL Server���ݿ�
        /// </summary>
        /// <returns></returns>
        bool IsUseSQLServer();
        #endregion

        #region ��ȡflxģ�巽��
        bool IfExistFileInServer(string fileName);

        byte[] ReadTempletByServer(string fileName);
        #endregion

        #region ��ѯ�ֿ�
        /// <summary>
        /// ��ѯ�ֿ�
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        StationCategory GetStationCategory(string projectId);

        string GetIRPAddress();
        #endregion

        #region �ճ�����ƻ�����ͬ����
        /// <summary>
        /// ��ѯ�ճ�����ƻ�
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetDailyPLanMaster(ObjectQuery oq);

        /// <summary>
        /// ����Id��ѯ�ճ�����ƻ���ϸ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DailyPlanDetail GetDailyPlanDetailById(string id);

        /// <summary>
        /// ��ѯ�ɹ���ͬ����
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetSupplyOrderMaster(ObjectQuery oq);
        #endregion

        object GetObjectById(Type type, string id);
        CurrentProjectInfo GetProjectInfo(ObjectQuery oq);

        /// <summary>
        /// ��Ҫ��԰�װ���ս�����ϸ��id ͨ����ⵥ��ϸ��id�������ս��㵥��ϸ��id
        /// </summary>
        /// <param name="sStockInBalDtlForworkID"></param>
        /// <returns></returns>
        string GetStockInBalDtlID(string sStockInBalDtlForworkID);
        /// <summary>
        /// ��Ҫ��԰�װ���ս��㵥��id   ͨ����ⵥ��id�������ս��㵥��id
        /// </summary>
        /// <param name="sStockInBalForworkID"></param>
        /// <returns></returns>
        DataSet  GetStockInBalID(string sStockInBalForworkID);
       /// <summary>
       /// �ж�CreateDateʱ��ĵ����Ƿ��Ѿ�����
       /// </summary>
       /// <param name="CreateDate">����ʱ��</param>
       /// <param name="sProjectID">��Ŀid</param>
       /// <returns></returns>
        string CheckAccounted(OperationOrgInfo oAccountOrg,DateTime CreateDate,   string sProjectID);

        string CheckBusinessDate(OperationOrgInfo oAccountOrg, DateTime CreateDate, IList materialList, string sProjectID);

        /// <summary>
        /// z��Խ��㵥���ۺ����Ľ���쵥���
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        StockInRed SaveStockInRed(StockInRed obj);
         /// <summary>
        /// �ύ���۵�
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <param name="lstStock">���˳�����쵥  �������  ������۵�  �����۵�</param>
        /// <returns></returns>
   
        StockInBalMaster SubmitStockInBalMaster3(StockInBalMaster obj, IList movedDtlList, IList lstStock);
        void SubmitStockInBalMasterByAudit(StockInBalMaster stockInBalMaster);
     /// <summary>
     /// ����
     /// </summary>
        /// <param name="lstStock">���˳�����쵥  �������  ������۵�  �����۵�</param>
     /// <returns></returns>
        string TallyList(IList lstStock, StockInBalMaster oStockInBalMaster, PersonInfo oPersonInfo);
        /// <summary>
        /// ���һ����
        /// </summary>
        /// <returns></returns>
        IList GetFiscalYear();
        /// <summary>
        /// ��ѯ�ɱ�����
        /// </summary>
        /// <param name="sProfessionCategory"></param>
        /// <param name="sProjectId"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <returns></returns>
         IList  QueryStockCost( string  sProjectId, int   iYear,int  iMonth); 
        /// <summary>
        /// ��ȡ�����̵������Ϣ
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sUserPartID"></param>
        /// <param name="sUserRand"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <returns></returns>
         DataTable QueryStoreInventory(string sProjectID, string sUserPartID,string sAccountTaskSysCode, string sUserRand,string sProfessionCat, int iYear, int iMonth);
        /// <summary>
        /// ��ȡ���̺�����ϸ��Ϣ
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sUserPartID"></param>
        /// <param name="sUserRand"></param>
        /// <param name="sProfessionCat"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <returns></returns>
         DataTable QueryStoreBalance(string sProjectID, string sUserPartID, string sUserRand,  int iYear, int iMonth);
        /// <summary>
        /// ��ѯ����̨��
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sStartDate"></param>
        /// <param name="sEndDate"></param>
        /// <returns></returns>
         DataTable QueryMaterialAccount(string sProjectID, string sStartDate, string sEndDate);


          /// <summary>
        /// ����ҵ��ʱ�������������
        /// </summary>
        /// <param name="CreateDate"></param>
        /// <returns></returns>
         DataTable GetFiscaDate(DateTime CreateDate);

         /// <summary>
         /// ���� ��Ŀ����  ����ID��ȡû�м��˵ĵ���
         /// </summary>
         /// <param name="sProjectName"></param>
         /// <param name="sCode"></param>
         /// <returns></returns>
         DataSet QueryListStockInNotHS(string sProjectName, string sCode);
         DataSet QueryListStockInBalNotHS(string sProjectName, string sCode);
         bool UpdateStockInNotHS(string sID, DateTime Time, string sDescript, int iYear, int iMonth);
         bool UpdateStockInBalNotHS( string  sID,  DateTime  Time,  string sDescript,  int iYear, int  iMonth);
         decimal GetRemainQty(string sDailyPlanDetialID);

        /// <summary>
        /// ��������ѯ
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="dStart"></param>
        /// <param name="dEnd"></param>
        /// <returns></returns>
         IList<WeightBillDetail> QueryWeightBill(string projectCode, DateTime dStart, DateTime dEnd, string sSupplyCode, string sMaterialCode);

    }
}
