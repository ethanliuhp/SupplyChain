using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonClass
{
    public class AutomaticSize
    {
        /* 
             在需要自适应大小的窗体中添加如下事件 
             *如果是容器,最好将Anchor和Dock属性修改过来. 
 
             private AutomaticSize automaticSize = new AutomaticSize(); 
 
             private void FrmPwd_Load(object sender, EventArgs e) 
             { 
                 automaticSize.Width = this.Width; 
                 automaticSize.Height = this.Height; 
                 automaticSize.SetTag(this); 
             } 
             private void FrmPwd_Resize(object sender, EventArgs e) 
             { 
                 automaticSize.ControlResize(this); 
             }     
             */

        private float width;
        private float height;

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public float Width
        {
            get { return width; }
            set { width = value; }
        }
        /// <summary>   
        /// 设置Tag标签   
        /// </summary>   
        /// <param name="controls">控件</param>   
        public void SetTag(Control controls)
        {
            foreach (Control control in controls.Controls)
            {
                control.Tag = control.Width + ":" + control.Height + ":" + control.Left + ":" + control.Top + ":" + control.Font.Size;
                if (control.Controls.Count > 0)
                {
                    SetTag(control);
                }
            }
        }

        /// <summary>   
        /// 设置控件大小   
        /// </summary>   
        /// <param name="newX">X坐标</param>   
        /// <param name="newY">Y坐标</param>   
        /// <param name="controls">控件</param>   
        public void SetControls(float newX, float newY, Control controls)
        {
            foreach (Control control in controls.Controls)
            {
                if (control.Tag == null)
                    continue;

                string[] myTag = control.Tag.ToString().Split(':');
                //控件的宽   
                float length = Convert.ToSingle(myTag[0]) * newX;
                if (length == 0)
                    continue;
                control.Width = (int)length;
                //控件的高   
                length = Convert.ToSingle(myTag[1]) * newY;
                control.Height = (int)length;
                //控件的X坐标   
                length = Convert.ToSingle(myTag[2]) * newX;
                control.Left = (int)length;
                //控件的Y坐标   
                length = Convert.ToSingle(myTag[3]) * newY;
                control.Top = (int)length;

                Single currentSize = Convert.ToSingle(myTag[4]) * newY;
                if (currentSize == 0)
                    continue;

                control.Font = new System.Drawing.Font(control.Font.Name, currentSize, control.Font.Style, control.Font.Unit);

                if (control.Controls.Count > 0)
                {
                    SetControls(newX, newY, control);
                }

            }
        }
        /// <summary>   
        /// 调整控件的大小   
        /// </summary>   
        public void ControlResize(Control control)
        {
            if (this.height != 0 && this.width != 0)
            {
                float newX = control.Width / this.width;
                float newY = control.Height / this.height;
                SetControls(newX, newY, control);
            }
        }
    }
}
