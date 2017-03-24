using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;

namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    public partial class VShowPictrue : TBasicDataView   
    {
        private int picIndex = -1;
        private string[] picList = null;
        public VShowPictrue(int index,string[] list)
        {
            InitializeComponent();
            picIndex = index;
            picList = list;
            InitData();
            InitEvent();
        }

        private void InitData()
        {
            timerPic.Enabled = false;
            ShowPic(picIndex);

        }
        private void InitEvent()
        {
            timerPic.Tick += new EventHandler(timerPic_Tick);
            btnPrevious.Click += new EventHandler(btnPrevious_Click);
            btnNext.Click += new EventHandler(btnNext_Click);
            btnStart.Click += new EventHandler(btnStart_Click);
            btnStop.Click += new EventHandler(btnStop_Click);

        }

        //暂停
        void btnStop_Click(object sender, EventArgs e)
        {
            timerPic.Enabled = false;
        }
        //播放
        void btnStart_Click(object sender, EventArgs e)
        {
            timerPic.Enabled = true;
        }
        //下一张
        void btnNext_Click(object sender, EventArgs e)
        {
            timerPic.Enabled = false;
            //picIndex += 1;
            picIndex -= 1;
            ShowPic(picIndex);
        }
        //上一张
        void btnPrevious_Click(object sender, EventArgs e)
        {
            timerPic.Enabled = false;
            //picIndex -= 1;
            picIndex += 1;
            ShowPic(picIndex);
        }
        //定时器 循环播放
        void timerPic_Tick(object sender, EventArgs e)
        {
            try
            {
                //picIndex++;
                //if (picIndex >= picList.Count())
                //{
                //    picIndex = 0;
                //}

                picIndex--;
                //if (picIndex < 0)
                //{
                //    picIndex = picList.Count() - 1;
                //}
                ShowPic(picIndex);

            }
            catch (Exception)
            {
                timerPic.Enabled = false;
            }

        }

        void ShowPic(int indexPic)
        {
            if (picIndex < 0)
            {
                picIndex = picList.Count() - 1;
                indexPic = picIndex;
            }
            //if (picIndex >= picList.Count())
            //{
            //    picIndex = 0;
            //}
            string path = picList[indexPic];
            path = path.Substring(1, path.Length - 2);
            picVirtual.Image = Image.FromStream(System.Net.WebRequest.Create(path).GetResponse().GetResponseStream());

            if (picIndex == 0 || picIndex == picList.Count() - 1)
            {
                if (picList.Count() == 1)
                {
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                }
                else
                {
                    if (picIndex != 0)
                    {
                        btnPrevious.Enabled = false;
                        btnNext.Enabled = true;
                    }
                    else
                    {
                        btnNext.Enabled = false;
                        btnPrevious.Enabled = true;
                    }
                }
            }
            else
            {
                if (!(btnPrevious.Enabled == true && btnNext.Enabled == true))
                {
                    btnPrevious.Enabled = true;
                    btnNext.Enabled = true;
                }
            }
        }
    }
}
