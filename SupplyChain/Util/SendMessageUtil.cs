using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SendMsgWebservice;

namespace Application.Business.Erp.SupplyChain.Util
{
    public class SendMessageUtil
    {
        private string _Url = "http://www.cscec3b.com:800/MsgSender/MainService.asmx";
        public string WebServicesUrl
        {
            get
            {
                return this._Url;
            }
            set
            {
                this._Url = value;
            }
        }
        private string _UserId = "adminCA!$!";
        private string _Password = "adminMSG@@@ACAD$";
        public string WebServicesUserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                this._UserId = value;
            }
        }
        public string WebServicesPassword
        {
            get
            {
                return this._Password;
            }
            set
            {
                this._Password = value;
            }
        }

        public int SendSMS(string Sender, string Reciver, string Content, bool isInSystem)
        {
            MainService ms = new MainService();
            ms.Url = this._Url;
            ThinkSoftSoapHeader header = new ThinkSoftSoapHeader();
            header.UserID = this._UserId;
            header.PassWord = this._Password;
            ms.ThinkSoftSoapHeaderValue = header;
            int result = 0;
            if (isInSystem)
            {
                result = ms.SendSMSMsgInSystem(Sender, Content, Reciver);
            }
            else
            {
                result = ms.SendSMSMsgNotInSystem(Sender, Content, Reciver);
            }
            ms.Dispose();
            return result;
        }
        /// <summary>
        /// 向门户的即时通发送消息
        /// </summary>
        /// <param name="Sender">消息发送者(系统默认)</param>
        /// <param name="Reciver">即时通消息接收者,即项目管理系统的人员名称</param>
        /// <param name="Content">内容描述,系统默认为“项目管理系统审批平台消息”</param>
        /// <param name="CheckIsOnline">是否在线,系统默认为false</param>
        /// <param name="isSendSMS">是否发送手机短消息,默认true</param>
        public int SendICUMsg(string Sender, string Reciver, string Content, bool CheckIsOnline, bool isSendSMS)
        {
            MainService ms = new MainService();
            ms.Url = this._Url;
            ThinkSoftSoapHeader header = new ThinkSoftSoapHeader();
            header.UserID = this._UserId;
            header.PassWord = this._Password;
            ms.ThinkSoftSoapHeaderValue = header;
            string err = string.Empty;
            int result = 0;
            try
            {
                result = ms.SendICUMsg(Sender, 7, Content, "项目业务基础管理", Reciver, ref err, CheckIsOnline, isSendSMS);
            }
            catch (Exception e)
            {
                throw new Exception("向门户发送消息失败！");
                return 0;
            }
            ms.Dispose();
            return result;

            //调用示例
            //SendMessageUtil sUtil = new SendMessageUtil();
            //sUtil.SendICUMsg("桑子敏", "梦龙测试", "项目管理测试11", false, true);
        }
       
    }
}
