using System;
using System.Data;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using System.Collections;
using VirtualMachine.Notice.Domain;

namespace Application.Business.Erp.SupplyChain.Client.NoticeMng
{
    public partial class VNotice : Form
    {
        private MNotice theMEdiDept = new MNotice();
        private Notice theEdiDept = new Notice();
        private IList lst = new ArrayList();
        public VNotice()
        {
            InitializeComponent();
            this.InitEvent();
            this.InitData();
        }

        private void InitEvent()
        {
            this.lnkAdd.Click += new EventHandler(lnkAdd_Click);
            this.lnkEdit.Click += new EventHandler(lnkEdit_Click);
            this.lnkDelete.Click += new EventHandler(lnkDelete_Click);
        }
        private void InitData()
        {
            this.grdEdiDept.Rows.Count = 1;
            lst = theMEdiDept.getobj();
            foreach (Notice var in lst)
            {
                Row row = this.grdEdiDept.Rows.Add();
                row["Kind"] = var.Kind;
                row["Content"] = var.Content;
                row["CreateDate"] = var.CreateDate.ToShortDateString();
                row["BeginDate"] = var.BeginDate.ToShortDateString();
                row["Createperson"] = var.Createperson;
                row["Cycle"] = var.Cycle.ToString();
                row["Level"] = var.Level.ToString();
                row["State"] = var.State == 0 ? "执行中" : "结束";
                row.UserData = var;
            }
            if (lst.Count > 0)
            {
                this.grdEdiDept.Rows[1].Selected = true;
            }
        }

        void lnkDelete_Click(object sender, EventArgs e)
        {

            if (this.grdEdiDept.Rows.Selected.Count == 0)
            {
                MessageBox.Show("请选择要删除的公告信息");
                return;
            }
            Notice obj = this.grdEdiDept.Rows[this.grdEdiDept.Row].UserData as Notice;
            DialogResult dr = MessageBox.Show("是否删除？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.No)
                return;
            bool result = false;
            result = theMEdiDept.Delete(obj);
            this.grdEdiDept.RemoveItem(this.grdEdiDept.Row);
        }

        void lnkEdit_Click(object sender, EventArgs e)
        {
            if (this.grdEdiDept.Rows.Selected.Count == 0)
            {
                MessageBox.Show("请选择要编辑的公告信息");
                return;
            }
            Notice EditDate = this.grdEdiDept.Rows[this.grdEdiDept.Row].UserData as Notice;
            Row row = this.grdEdiDept.Rows[this.grdEdiDept.Row];
            VAddNotice theUpdate = new VAddNotice();
            EditDate = theUpdate.Show(EditDate) as Notice;
            row.UserData = EditDate;
            if (theUpdate.IsSuccess)
            {
                row["Kind"] = EditDate.Kind;
                row["Content"] = EditDate.Content;
                row["BeginDate"] = EditDate.BeginDate.ToShortDateString();
                row["CreateDate"] = EditDate.CreateDate.ToShortDateString();
                row["Createperson"] = EditDate.Createperson;
                row["Cycle"] = EditDate.Cycle.ToString();
                row["Level"] = EditDate.Level.ToString();
                row["State"] = EditDate.State == 0 ? "执行中" : "结束";
                row.UserData = EditDate;
            }
        }

        void lnkAdd_Click(object sender, EventArgs e)
        {
            Notice theEdiDept = new Notice();
            VAddNotice theVAddEdiDept = new VAddNotice();

            theEdiDept = theVAddEdiDept.Show(theEdiDept) as Notice;
            if (theVAddEdiDept.IsSuccess)
            {
                Row row = this.grdEdiDept.Rows.Add();
                this.grdEdiDept.Row = this.grdEdiDept.Rows.Count - 2;//当前行指向最后一行

                row["Kind"] = theEdiDept.Kind;
                row["Content"] = theEdiDept.Content;
                row["CreateDate"] = theEdiDept.CreateDate.ToShortDateString();
                row["BeginDate"] = theEdiDept.BeginDate.ToShortDateString();
                row["Createperson"] = theEdiDept.Createperson;
                row["Cycle"] = theEdiDept.Cycle.ToString();
                row["Level"] = theEdiDept.Level.ToString();
                row["Level"] = theEdiDept.Level.ToString();
                row["State"] = theEdiDept.State == 0 ? "执行中" : "结束";
                row.UserData = theEdiDept;
            }

        }
      
        /// <summary>
        /// 从数据表导出数据到EXCEL
        /// </summary>
        /// <param name="datetable">传入数据的DataTable名字</param>
        /// <param name="columstart"></param>
        public void dgvToExcel(DataTable datetable)
        {
            try
            {
                Microsoft.Office.Interop.Excel.ApplicationClass MyExcel = new Microsoft.Office.Interop.Excel.ApplicationClass();
                MyExcel.Visible = true;
                if (MyExcel == null)
                {
                    //manage.writerLogFile("BaseForm-dgvToExcel(DataTable datetable) EXCEL没有安装！ ");
                    MessageBox.Show("EXCEL没有安装！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                Microsoft.Office.Interop.Excel.Workbooks MyWorkBooks = MyExcel.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook MyWorkBook = MyWorkBooks.Add(System.Reflection.Missing.Value);
                Microsoft.Office.Interop.Excel.Worksheet MyWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)MyWorkBook.Worksheets[1];
                Microsoft.Office.Interop.Excel.Range MyRange = MyWorkSheet.get_Range(MyWorkSheet.Cells[1, 1], MyWorkSheet.Cells[1, datetable.Columns.Count]);
                object[] MyHeader = new object[datetable.Columns.Count];
                for (int k = 0; k < datetable.Columns.Count; k++)
                {
                    MyHeader[k] = datetable.Columns[k].ToString();
                }
                MyRange.Value2 = MyHeader;
                if (datetable.Rows.Count > 0)
                {
                    MyRange = MyWorkSheet.get_Range("A2", System.Reflection.Missing.Value);
                    String[,] MyData = new String[datetable.Rows.Count, datetable.Columns.Count];
                    for (int i = 0; i < datetable.Rows.Count; i++)
                    {
                        for (int j = 0; j < datetable.Columns.Count; j++)
                        {
                            MyData[i, j] = datetable.Rows[i][j].ToString();
                            //MyData[i,j] = dategridview.Rows[i].Cells[j].Value.ToString();//							
                        }
                    }
                    MyRange = MyRange.get_Resize(datetable.Rows.Count, datetable.Columns.Count);
                    MyRange.Value2 = MyData;
                    MyRange.EntireColumn.AutoFit();
                }
                MyExcel = null;

            }
            catch (Exception Err)
            {
                //manage.writerLogFile("BaseForm-dgvToExcel(DataTable datetable)   " + Err.Message);
                MessageBox.Show("导出EXCEL时出现错误！" + Err.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

