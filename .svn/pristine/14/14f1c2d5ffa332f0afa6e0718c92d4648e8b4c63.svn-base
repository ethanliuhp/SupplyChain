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
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using VirtualMachine.Core;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS
{
    public partial class VProductManageChangeQuery : TBasicDataView
    {
        InspectionRecord insRecord;
        public MGWBSTree model = new MGWBSTree();
        public VProductManageChangeQuery(InspectionRecord record)
        {
            InitializeComponent();
            insRecord = record;
            InitData(insRecord);
        }        

        private void InitData(InspectionRecord inspection)
        {
            int rowIndex = dgDetail.Rows.Add();
            dgDetail[colCorrectiveSign.Name, rowIndex].Value = inspection.CorrectiveSign;
            dgDetail[colCheckSpecial.Name, rowIndex].Value = inspection.InspectionSpecial;
            dgDetail[colCheckConclusion.Name, rowIndex].Value = inspection.InspectionConclusion;
            dgDetail[colContent.Name, rowIndex].Value = inspection.InspectionStatus;
            dgDetail[colCheckPerson.Name, rowIndex].Value = inspection.CreatePersonName;
            dgDetail[colCheckPerson.Name, rowIndex].Tag = inspection.CreatePerson;
            dgDetail[colBearName.Name, rowIndex].Value = inspection.BearTeamName;
            dgDetail[colBearName.Name, rowIndex].Tag = inspection.BearTeam;
            dgDetail[colInspectionDate.Name, rowIndex].Value = inspection.CreateDate.ToShortDateString();
            if (inspection.Id != null)
            {
                if (inspection.DocState == DocumentState.InExecute || inspection.DocState == DocumentState.InAudit)
                {
                    dgDetail[colRecordState.Name, rowIndex].Value = "有效";
                }
                else
                {
                    dgDetail[colRecordState.Name, rowIndex].Value = "编辑";
                }
            }
            else
            {
                dgDetail[colRecordState.Name, rowIndex].Value = "编辑";
            }
            string strDeductionSign = ClientUtil.ToString(inspection.DeductionSign);
            string strCorrectiveSign = ClientUtil.ToString(inspection.CorrectiveSign);
            if (strCorrectiveSign.Equals("0"))
            {
                dgDetail[colCorrectiveSign.Name, rowIndex].Value = "不需整改";
            }
            if (strCorrectiveSign.Equals("1"))
            {
                dgDetail[colCorrectiveSign.Name, rowIndex].Value = "需要整改，未进行处理";
            }
            if (strCorrectiveSign.Equals("2"))
            {
                dgDetail[colCorrectiveSign.Name, rowIndex].Value = "需要整改，已进行处理";
            }
            dgDetail.Rows[rowIndex].Tag = inspection;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", inspection.Id));
            IList listDocument = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);
            if (listDocument != null && listDocument.Count > 0)
            {
                gridDocument.Rows.Clear();
                foreach (ProObjectRelaDocument doc in listDocument)
                {
                    InsertIntoGridDocument(doc);
                }
            }
        }
        private void InsertIntoGridDocument(ProObjectRelaDocument doc)
        {
            int index = gridDocument.Rows.Add();
            DataGridViewRow row = gridDocument.Rows[index];
            row.Cells[DocumentName.Name].Value = doc.DocumentName;
            row.Cells[DocumentCode.Name].Value = doc.DocumentCode;
            row.Cells[DocumentCateCode.Name].Value = doc.DocumentCateCode;
            row.Cells[DocumentDesc.Name].Value = doc.DocumentDesc;
            row.Tag = doc;
        }

    }
}
