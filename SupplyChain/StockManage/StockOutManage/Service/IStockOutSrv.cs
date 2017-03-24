using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Data;
using System.Collections;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Service
{
    /// <summary>
    /// ���ⵥ����
    /// </summary>
    public interface IStockOutSrv : IBaseService
    {
        IList GetStockOut(ObjectQuery oq);

        StockOut GetStockOutById(string id);

        StockOut SaveStockOut(StockOut obj);
        StockOut SaveStockOut1(StockOut obj);
        StockOut GetStockOutByCode(string code, string special, string projectId);
        StockOut GetStockOutByCode(string code, string projectId);
        IList GetSporadicMoney(string projectID, DateTime startDate, DateTime endDate);
        /// <summary>
        /// ������ϸId��ѯ���ϳ��ⵥ��ϸ
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        StockOutDtl GetStockOutDtlById(string stockOutDtlId);

        /// <summary>
        /// �����ѯ
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataSet StockOutQuery(string condition);
        DataSet StockOutMasterQuery(string condition);

        IList GetStockOutRed(ObjectQuery oq);

        StockOutRed GetStockOutRedById(string id);

        /// <summary>
        /// �������ϳ���쵥
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockOutRed SaveStockOutRed(StockOutRed obj, IList movedDtlList);
        /// <summary>
        /// ������ս���쵥�г��ⵥ�ĵ��۳��
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        StockOutRed SaveStockOutRed(StockOutRed obj);
        /// <summary>
        /// ɾ�����ϳ���쵥
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteStockOutRed(StockOutRed obj);

        StockOutRed GetStockOutRedByCode(string code, string special, string projectId);
        StockOutRed GetStockOutRedByCode(string code, string projectId);
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="hashLst">key StockIn;value id List;key StockInRed;value id List;</param>
        /// <param name="hashCode">key StockIn;value Code List;key StockInRed;value Code List;</param>
        /// <param name="year">������</param>
        /// <param name="month">������</param>
        /// <param name="tallyPersonId">������ID</param>
        /// <param name="tallyPersonName">����������</param>
        /// <returns></returns>
        Hashtable TallyStockOut(Hashtable hashLst, Hashtable hashCode, int year, int month, string tallyPersonId, string tallyPersonName, string projectId);

        /// <summary>
        /// ��ѯ����ʱ��
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetStockOutDtlSeq(ObjectQuery oq);

        /// <summary>
        /// ���������ϸ��ѯ����ʱ��
        /// </summary>
        /// <param name="stockInDtlId"></param>
        /// <returns></returns>
        IList GetStockOutDtlSeqByStockInDtlId(string stockInDtlId);

        /// <summary>
        /// ���ݳ���Id��ѯ����ʱ��(�����ʱ������)
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        IList GetStockOutDtlSeqByStockOutDtlId(string stockOutDtlId);

        /// <summary>
        /// ��ѯ������ϸ(��������ʱ��)
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        StockOutDtl GetStockOutDtlByIdWithDetails(string stockOutDtlId);

        /// <summary>
        /// ������ϸʱ��������ϸID��ѯ������ϸ
        /// </summary>
        /// <param name="stockInDtlId"></param>
        /// <returns></returns>
        IList GetStockOutDtlByStockInDtlId(string stockInDtlId);

        /// <summary>
        /// ������ϸʱ��������ϸID��ѯ������ϸ
        /// </summary>
        /// <param name="stockInDtlId"></param>
        /// <returns></returns>
        DataSet QueryStockOutDtl(string stockInDtlId);
         /// <summary>
        /// ������ⵥ��ϸ��ID��ȡ����ʱ��� ���ⵥ��ϸ ���ⵥ
        /// </summary>
        /// <param name="sStockInDtlID"></param>
        /// <returns></returns>
        IList GetStockOutDtlSeqByStockInDtlID1(string sStockInDtlID);
        /// <summary>
        /// ������ϸ��ID��ȡ����������
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        StockOut GetStockOutByStockOutDtlID(string sID);
             /// <summary>
        /// ��ѯ���ⵥ ������������ȷ�ϼ۸�
        /// </summary>
        /// <param name="ssStockOutDtlID"></param>
        /// <returns></returns>
        DataSet QueryStockInQuantityAndOutPrice(string sStockOutDtlID);
        IList GetMaterialLst(IList list);

        // <summary>
        /// ��ȡ��װ�ĺ���ڵ�
        /// </summary>
        /// <returns></returns>
        IList GetSetUpCostAccountSubject();
        IList ObjectQuery(Type entityType, ObjectQuery oq);
        IList GetMaterial(string sID);

        /// <summary>
        /// ���� ��Ŀ����  ����ID��ȡû�м��˵ĵ���
        /// </summary>
        /// <param name="sProjectName"></param>
        /// <param name="sCode"></param>
        /// <returns></returns>
        DataSet  QueryListStockOutNotHS(string sProjectName, string sCode);
        bool UpdateStockOutNotHS(string sID, DateTime Time, string sDescript, int iYear, int iMonth);
        IList GetStockMoveOut(ObjectQuery oq);
        IList GetStockMoveOutRed(ObjectQuery oq);

        string GetUnitDiffMaterial(IList lstMaterial, string sProjectID, string sSpecial, string sProfessioncategory, IList lstDiagramNum);


        DataSet GetStockMatByUnit(string sProjectID, string sSpec, string sCode, string sName, string sSpecial, string sProfession, MaterialCategory oMaterialCategory, bool IsMoveOut);
    }
}