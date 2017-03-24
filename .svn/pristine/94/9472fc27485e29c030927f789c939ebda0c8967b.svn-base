using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Service;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using System.Web.UI.MobileControls;
using System.Text;
using System.Collections.Generic;
using VirtualMachine.Patterns.BusinessEssence.Domain;

public partial class MoneyManage_FactoringDataManage_Main : System.Web.UI.Page
{
    /// <summary>
    /// 服务对象
    /// </summary>
    IFinanceMultDataSrv service = GlobalClass.FinanceMultDataSrv;

    /// <summary>
    /// 页面加载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 页面初始化
            // 时间初始化
            txtStartDate.Text = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            // 数据绑定
            BindData();
        }
    }
    /// <summary>
    /// 页面渲染之前
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRender(object sender, EventArgs e)
    {
        // 设置上一页、下一页是否可用
        if (PageIndex < AllPageNumber) btnNext.Enabled = true; else btnNext.Enabled = false;
        if (PageIndex > 1) btnPrev.Enabled = true; else btnPrev.Enabled = false;
    }

    #region 属性

    private List<FactoringDataMaster> _listData;
    /// <summary>
    /// 主表数据列表
    /// </summary>
    public List<FactoringDataMaster> ListData
    {
        get
        {
            if (ViewState["ListData"] == null)
            {
                BindData();
            }
            else
            {
                _listData = (List<FactoringDataMaster>)ViewState["ListData"];
            }
            return _listData;
        }
        set
        {
            _listData = value;
            ViewState["ListData"] = _listData;
        }
    }
    private int _pageIndex;
    /// <summary>
    /// 页码
    /// </summary>
    public int PageIndex
    {
        set
        {
            _pageIndex = value;
            ViewState["PageIndex"] = _pageIndex;
        }
        get
        {
            if (ViewState["PageIndex"] == null)
            {
                _pageIndex = 1;
                ViewState["PageIndex"] = _pageIndex;
            }
            else
            {
                _pageIndex = (int)ViewState["PageIndex"];
            }

            return _pageIndex;
        }
    }
    private int _pageSize;
    /// <summary>
    /// 页大小
    /// </summary>
    public int PageSize
    {
        set
        {
            _pageSize = value;
            ViewState["PageSize"] = _pageSize;
        }
        get
        {
            if (ViewState["PageSize"] == null)
            {
                _pageSize = 15;
                ViewState["PageSize"] = _pageSize;
            }
            else
            {
                _pageSize = (int)ViewState["PageSize"];
            }
            return _pageSize;
        }
    }
    private int _recordCount;
    /// <summary>
    /// 总记录数
    /// </summary>
    public int RecordCount
    {
        get
        {
            if (ViewState["RecordCount"] == null)
            {
                _recordCount = 0;
                ViewState["RecordCount"] = 0;
            }
            else
            {
                _recordCount = (int)ViewState["RecordCount"];
            }
            return _recordCount;
        }
        set
        {
            _recordCount = value;
            ViewState["RecordCount"] = _recordCount;
        }
    }
    private int _allPageNumber;
    /// <summary>
    /// 总页数
    /// </summary>
    public int AllPageNumber
    {
        get
        {
            if (ViewState["AllPageNumber"] == null)
            {
                _allPageNumber = 0;
                ViewState["AllPageNumber"] = 0;
            }
            else
            {
                _allPageNumber = (int)ViewState["AllPageNumber"];
            }
            return _allPageNumber;
        }
        set
        {
            _allPageNumber = value;
            ViewState["AllPageNumber"] = _allPageNumber;
        }
    }
    private List<FactoringDataMaster> _newMasterList = new List<FactoringDataMaster>();
    /// <summary>
    /// 新增主表记录
    /// </summary>
    public List<FactoringDataMaster> NewMasterList
    {
        get
        {
            if (ViewState["NewMasterList"] == null)
            {
                ViewState["NewMasterList"] = _newMasterList;
            }
            else
            {
                _newMasterList = (List<FactoringDataMaster>)ViewState["NewMasterList"];
            }
            return _newMasterList;
        }
        set
        {
            _newMasterList = value;
            ViewState["NewMasterList"] = value;
        }
    }

    #endregion

    /// <summary>
    /// 主表数据绑定
    /// </summary>
    private void BindData()
    {
        // 获取截止日期
        var startDate = string.IsNullOrEmpty(txtStartDate.Text) ? "1900-01-01" : txtStartDate.Text;
        var endDate = string.IsNullOrEmpty(txtEndDate.Text) ? "2060-01-01" : txtEndDate.Text;
        // 页码开始位置与结束位置
        var startPageIndex = (PageIndex - 1) * PageSize + 1;
        var endPageIndex = PageIndex * PageSize;

        // 时间
        StringBuilder sb = new StringBuilder();
        string dateFormat = "{0} between to_date('{1} 00:00:00','yyyy-mm-dd hh24:mi:ss') AND to_date('{2} 23:59:59','yyyy-mm-dd hh24:mi:ss')";
        sb.Append(string.Format(dateFormat, "createdate", startDate, endDate));
        // 格式化sql查询
        string sqlText = string.Format("select * from (select rownum as rn, id, code, createpersonname, handlepersonname, createdate, state, descript from (select id, code, createpersonname, handlepersonname, createdate , state, descript from thd_factoringdatamaster where {0} order by createdate desc)) where rn between {1} and {2}", sb.ToString(), startPageIndex, endPageIndex);

        DataSet ds = GlobalClass.CommonMethodSrv.GetData(sqlText);

        var result = ds.Tables[0].Select().Select(a => new FactoringDataMaster() { Temp2 = a["rn"] + "", Id = a["id"] + string.Empty, Code = a["code"] + string.Empty, CreatePersonName = a["createpersonname"] + string.Empty, HandlePersonName = a["handlepersonname"] + string.Empty, CreateDate = Convert.ToDateTime(a["createdate"]), Temp1 = CommonHelper.stateDescripe[Convert.ToInt32(a["state"])], Descript = a["descript"] + string.Empty }).ToList();
        // 记录总记录数
        string sqlRecord = string.Format("select count(*) as recordcount from thd_factoringdatamaster where {0}", sb.ToString());
        DataSet dsRecord = GlobalClass.CommonMethodSrv.GetData(sqlRecord);
        RecordCount = dsRecord.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dsRecord.Tables[0].Rows[0]["recordcount"]) : 0;
        // 总页数
        AllPageNumber = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(RecordCount) / Convert.ToDouble(PageSize)));

        ListData = result;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    protected void btnSearch_Click(object sender, EventArgs args)
    {
        PageIndex = 1;
        BindData();
    }

    /// <summary>
    /// 上一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrev_Click(object sender, EventArgs e)
    {
        if (PageIndex > 1)
        {
            PageIndex--;
            BindData();
        }
    }

    /// <summary>
    /// 下一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (PageIndex < AllPageNumber)
        {
            PageIndex++;
            BindData();
        }
    }

    /// <summary>
    /// 点击页码跳转
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnJump_Click(object sender, EventArgs e)
    {
        int curPage = Convert.ToInt32(hidPageIndex.Value);
        if (curPage != PageIndex)
        {
            PageIndex = curPage;
            BindData();
        }
    }

    /// <summary>
    /// 主表数据删除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        var a = ListData[Convert.ToInt32(hidMasterDeleteId.Value)];
        a.Code = string.IsNullOrEmpty(a.Code) ? "a" : a.Code;

        service.Delete(a);
        BindData();
    }

    /// <summary>
    /// 添加主表记录
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    protected void btn_Add_Master_Click(object sender, EventArgs args)
    {
        if (hid_operateState.Value == "Edit")
        {
            var model = service.GetFactoringDataById(hid_edit_id.Value);
            model.Descript = txtDescrip.Text;
            model.LastModifyDate = DateTime.Now;
            model.HandlePersonName = txtHandler.Text;
            service.Update(model);
            var item = ListData.Where(a => a.Id == hid_edit_id.Value).FirstOrDefault();
            item.Descript = txtDescrip.Text;
            item.HandlePersonName = txtHandler.Text;
        }
        else
        {
            var model = new FactoringDataMaster();
            var user = Session[PublicClass.sUserInfoID] as SessionInfo;
            model.Descript = txtDescrip.Text;
            model.LastModifyDate = DateTime.Now;
            model.HandlePersonName = txtHandler.Text;
            //model.Id = Guid.NewGuid().ToString();
            model.Code = txtCode.Text;
            model.CreateDate = DateTime.Now;
            model.CreateYear = DateTime.Now.Year;
            model.CreateMonth = DateTime.Now.Month;
            model.CreatePerson = user.CurrentPersinInfo;
            model.CreatePersonName = user.CurrentPerson.Name;
            model.OperOrgInfo = user.CurrentOrgInfo;
            model.OperOrgInfoName = user.CurrentOrgInfo.Name;
            model.DocState = DocumentState.Edit;
            model.Temp1 = CommonHelper.stateDescripe[Convert.ToInt32(DocumentState.Edit)];
            AddMaster(model);
        }
    }

    /// <summary>
    /// 新增主表记录
    /// </summary>
    /// <param name="model"></param>
    private void AddMaster(FactoringDataMaster model)
    {
        NewMasterList.Add(model);
        ViewState["NewMasterList"] = NewMasterList;
    }

    /// <summary>
    /// 保存新增主表记录
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        foreach (var item in NewMasterList)
        {
            item.DocState = DocumentState.Completed;
        }
        service.SaveList(NewMasterList);
        NewMasterList.Clear();
        BindData();
    }


}
