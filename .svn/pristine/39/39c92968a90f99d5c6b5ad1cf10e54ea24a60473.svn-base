using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service;
using System.Collections;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using NHibernate;
using Application.Business.Erp.Financial.InitialData.Domain;
using Application.Business.Erp.SupplyChain.BasicData.ExpenseItemMng.Domain;


namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.CostProjectUI
{
    /// <summary>
    ///成本项目分类
    /// </summary>
    public class MCostProject
    {
        public string ErrorInfo; //校验提示信息
        public ICostProjectSrv titleService;

        /// <summary>
        /// 获取成本项目节点集合
        /// </summary>
        /// <returns></returns>
        virtual public IList GetCostProjects()
        {
            IList lsAllTitle = titleService.GetCostProjects(null);
            return lsAllTitle;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public CostProject SaveCostProject(CostProject title)
        {
            title = titleService.SaveCostProject(title);
            return title;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public CostProject DeleteCostProject(CostProject title)
        {
            if (!titleService.IsReferByCostAccount(title.Id))
            {
                titleService.DeleteCostProject(title);
                return title.ParentNode as CostProject;
            }
            else
            {

                return title;
            }
        }

        /// <summary>
        /// 获取成本项目分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CostProject GetCostProject1(string id)
        {
            return titleService.GetCostProject(id);
        }

        /// <summary>
        /// 获取当前节点的所有子节点
        /// </summary>
        /// <returns></returns>
        public IList GetCostProjectChilds(ObjectQuery oq)
        {
            return titleService.GetCostProjects(oq);
        }

        public IList GetAllExpenseItem()
        {
            return titleService.GetObjects(typeof(ExpenseItem));
        }

        /// <summary>
        /// 校验科目编码
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool ValidateCostProjectCode(CostProject title)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", title.Code));
            oq.AddCriterion(Expression.Not(Expression.Eq("Id", title.Id)));
            IList lst = titleService.GetCostProjects(oq);
            if (lst != null && lst.Count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查科目是否已被成本计算所引用
        /// </summary>
        /// <param name="titleId"></param>
        /// <returns></returns>
        public bool IsReferByCostAccount(string CostProjectId)
        {
            return titleService.IsReferByCostAccount(CostProjectId);
        }

    }
}
