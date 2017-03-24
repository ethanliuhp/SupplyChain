using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;  
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.FinancialResource;
using System.Data;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Basic.Service;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using IRPServiceModel.Services.Document;

namespace Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng
{
    public class MProjectDepartment
    {
        private ICurrentProjectSrv currentSrv;
        private  IDocumentSrv docSrv;
        public ICurrentProjectSrv CurrentSrv
        {
            get { return currentSrv; }
            set { currentSrv = value; }
        }

        public MProjectDepartment()
        {
            if (currentSrv == null)
            {
                currentSrv = StaticMethod.GetService("CurrentProjectSrv") as ICurrentProjectSrv;
            }
            if (docSrv == null)
                docSrv = StaticMethod.GetService("DocumentSrv") as IDocumentSrv;
        }

        #region 项目部基本信息
        /// <summary>
        /// 保存项目部基本信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public OperationOrg SaveOperationOrg(OperationOrg obj)
        {
            return currentSrv.SaveOperationOrg(obj);
        }

        /// <summary>
        /// 获取业务组织节点集合(返回有权限和无权限的集合)
        /// </summary>
        //public IList GetCurProByInstance()
        //{
        //    //return currentSrv.GetCurProByInstance();
        //}

        #endregion


        /// <summary>
        /// 上传照片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileDataByte"></param>
        /// <param name="personCode"></param>
        /// <returns></returns>
        public Hashtable UpdatePicture(string fileName, byte[] fileDataByte, string personCode)
        {
            return docSrv.UpdatePicture(fileName, fileDataByte, personCode);
        }
        /// <summary>
        /// 得到照片路径（显示用）
        /// </summary>
        /// <param name="theFileCabinetId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetPicturePath(string theFileCabinetId, string fileName)
        {
            return docSrv.GetPicturePath(theFileCabinetId, fileName);
        }

        /// <summary>
        /// 得到照片存储路径（删除）
        /// </summary>
        /// <param name="theFileCabinetId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetPictureStoragePath(string theFileCabinetId, string fileName)
        {
            return docSrv.GetPictureStoragePath(theFileCabinetId, fileName);
        }
      
    }
}
