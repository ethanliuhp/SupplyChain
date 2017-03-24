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
using System.Collections;
//测试
//using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VDocumentDetail : TBasicDataView
    {
        public VDocumentDetail()
        {
            InitializeComponent();
            ObjectLock.Lock(pnlFloor, true);
        }

        public VDocumentDetail(IList list,string isIRPOrKB)
        {
            InitializeComponent();
            ObjectLock.Lock(pnlFloor, true);
            if (isIRPOrKB != "KB")
            {
                PLMWebServices.ProjectDocument doc = list[0] as PLMWebServices.ProjectDocument;
                ShowDocumentDetail(doc);
            }
            else
            {
                PLMWebServicesByKB.ProjectDocument doc = list[0] as PLMWebServicesByKB.ProjectDocument;
                ShowDocumentDetail(doc);
            }
        }
        /// <summary>
        /// 显示文档详细信息
        /// </summary>
        /// <param name="ow"></param>
        void ShowDocumentDetail(PLMWebServices.ProjectDocument doc)
        {
            //【文档在文件系统中GUID】、【所属项目】、【文档名称】、【文档扩展名】、
            //【文档对象类型】、【文档信息类型】、【文档版本号】、【文档分类编码】、
            //【文档分类名称】、【文档代码】、【文档标题】、【文档作者】、【文档关键字】、
            //【文档说明】、【文档状态】、【责任人】、【创建时间】
            txtGUID.Text = doc.EntityID;
            txtResideProject.Text = doc.ProjectName;
            txtDocumentName.Text = doc.Name;
            txtDocumentExpandedName.Text = doc.ExtendName;
            txtDocumentObjectType.Text = doc.ObjectTypeName;
            txtInforType.Text = doc.DocType.ToString();
            txtDocumentVersion.Text = doc.Version;
            if (doc.Category != null)
            {
                txtDocumentSortEncode.Text = doc.Category.CategoryCode;
                txtDocumentSortName.Text = doc.Category.CategoryName;
            }
            txtDocumentCode.Text = doc.Code;
            txtDocumentTitle.Text = doc.Title;
            txtDocumentAuthor.Text = doc.Author;
            txtDocumentKeywords.Text = doc.KeyWords;
            txtDocumentExplain.Text = doc.Description;
            txtDocumentStatus.Text = doc.State.ToString();
            txtDutyPerson.Text = doc.OwnerName;
            txtCreateTime.Text = doc.CreateTime.ToString();
        }
        void ShowDocumentDetail(PLMWebServicesByKB.ProjectDocument doc)
        {
            txtGUID.Text = doc.EntityID;
            txtResideProject.Text = doc.ProjectName;
            txtDocumentName.Text = doc.Name;
            txtDocumentExpandedName.Text = doc.ExtendName;
            txtDocumentObjectType.Text = doc.ObjectTypeName;
            txtInforType.Text = doc.DocType.ToString();
            txtDocumentVersion.Text = doc.Version;
            if (doc.Category != null)
            {
                txtDocumentSortEncode.Text = doc.Category.CategoryCode;
                txtDocumentSortName.Text = doc.Category.CategoryName;
            }
            txtDocumentCode.Text = doc.Code;
            txtDocumentTitle.Text = doc.Title;
            txtDocumentAuthor.Text = doc.Author;
            txtDocumentKeywords.Text = doc.KeyWords;
            txtDocumentExplain.Text = doc.Description;
            txtDocumentStatus.Text = doc.State.ToString();
            txtDutyPerson.Text = doc.OwnerName;
            txtCreateTime.Text = doc.CreateTime.ToString();
        }
    }
}
