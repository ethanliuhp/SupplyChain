using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public partial class VFilialeFundPlan : TBasicDataView
    {
        private string reportJHHZ = "计划汇总封面";
        private string reportXMSB1 = "项目申报汇总表1";
        private string reportXMSB2 = "项目申报汇总表2";
        private string reportFGSJG = "分公司机关";

        private CurrentProjectInfo projectInfo;

        public VFilialeFundPlan()
        {
            InitializeComponent();
            InitEvent();
            InitData();
            LoadData(reportJHHZ);
        }

        private void InitEvent()
        {
            tabControl.SelectedIndexChanged += new EventHandler(tabControl_SelectedIndexChanged);
        }

        private void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            this.LoadFLXFile(reportJHHZ + ".flx", reportJHHZ);
            this.LoadFLXFile(reportXMSB1 + ".flx", reportXMSB1);
            this.LoadFLXFile(reportXMSB2 + ".flx", reportXMSB2);
            this.LoadFLXFile(reportFGSJG + ".flx", reportFGSJG);
        }

        void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Text == reportJHHZ)
            {
                this.LoadData(reportJHHZ);
            }
            if (tabControl.SelectedTab.Text == reportXMSB1)
            {
                this.LoadData(reportXMSB1);
            }
            if (tabControl.SelectedTab.Text == reportXMSB2)
            {
                this.LoadData(reportXMSB2);
            }
            if (tabControl.SelectedTab.Text == reportFGSJG)
            {
                this.LoadData(reportFGSJG);
            }
        }

        private void LoadData(string type)
        {
            try 
            {
                if (type == reportJHHZ)
                {
                    fGridDetail.AutoRedraw = false;

                    this.comYear.Items.Clear();
                    this.comMonth.Items.Clear();

                    for (int i = 0; i < 13; i++)
                    {
                        this.comYear.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 6));
                    }
                    for (int i = 1; i < 13; i++)
                    {
                        this.comMonth.Items.Add(i);
                    }

                    this.comYear.Text = DateTime.Now.Year.ToString();
                    this.comMonth.Text = DateTime.Now.Month.ToString();

                    
                }
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
            finally 
            {
                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
                fGridDetail1.AutoRedraw = true;
                fGridDetail1.Refresh();
                fGridDetail2.AutoRedraw = true;
                fGridDetail2.Refresh();
                fGridDetail3.AutoRedraw = true;
                fGridDetail3.Refresh();
            }
        }

        private void LoadFLXFile(string flxname, string type)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(flxname))
            {
                eFile.CreateTempleteFileFromServer(flxname);
                if (type == reportJHHZ)
                {
                    fGridDetail.OpenFile(path + "\\" + flxname);//载入格式
                }
                if (type == reportXMSB1)
                {
                    fGridDetail1.OpenFile(path + "\\" + flxname);//载入格式
                }
                if (type == reportXMSB2)
                {
                    fGridDetail2.OpenFile(path + "\\" + flxname);//载入格式
                }
                if (type == reportFGSJG)
                {
                    fGridDetail3.OpenFile(path + "\\" + flxname);//载入格式
                }
            }
        }
    }
}
