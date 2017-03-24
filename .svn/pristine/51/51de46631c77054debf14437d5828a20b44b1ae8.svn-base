using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.BasicData.Service;
using Application.Business.Erp.SupplyChain.BasicData.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.BasicData
{
    public class MBasicData
    {
        private IBasicDataService basicDataSrv;

        public IBasicDataService BasicDataSrv
        {
            get { return basicDataSrv; }
            set { basicDataSrv = value; }
        }

        public MBasicData()
        {
            if (basicDataSrv == null)
            {
                basicDataSrv = ConstMethod.GetService(typeof(IBasicDataService)) as IBasicDataService;
            }
        }

        /// <summary>
        /// 根据基础数据表名查询数据并形成下拉框
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="view"></param>
        /// <param name="selected"></param>
        public void InitialBasicData(string tableName, ComboBox view, bool selected)
        {
            IList list = new ArrayList();
            try
            {
                list = BasicDataSrv.GetDetailByBasicTableName(tableName);
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("查询基础数据表{"+tableName+"}出错。", ex);
            }
            if (selected == false)
            {
                BasicDataDetail detail = new BasicDataDetail();
                detail.Id = 0;
                detail.Version = 0;
                detail.Code = "";
                detail.Name = "--请选择--";
                list.Insert(0, detail);
            }

            view.DataSource = list;
            view.DisplayMember = "Name";
            view.ValueMember = "Code";
        }

        /// <summary>
        /// 空的下拉框
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="view"></param>
        /// <param name="selected"></param>
        public void InitialNoBasicData(ComboBox view, bool selected)
        {
            IList list = new ArrayList();
            if (selected == false)
            {
                BasicDataDetail detail = new BasicDataDetail();
                detail.Id = 0;
                detail.Version = 0;
                detail.Code = "";
                detail.Name = "--请选择--";
                list.Insert(0, detail);
            }

            view.DataSource = list;
            view.DisplayMember = "Name";
            view.ValueMember = "Code";
        }

    }
}
