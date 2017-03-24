using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using System.Data;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng
{
    public class MAppPlatMng
    {

        private static IAppSrv service = null;
        public IAppSrv Service
        {
            get { return service; }
            set { service = value; }
        }
        public MAppPlatMng()
        {
            if (service == null)
            {
                service = StaticMethod.GetService("RefAppSrv") as IAppSrv;
            }
        }
        public IList Save(IList lst)
        {
            return service.Save(lst);
        }
        public IList GetObjects(Type t, ObjectQuery oq)
        {
            return service.GetObjects(t, oq);
        }
        public bool Delete(IList lst)
        {
            return service.Delete(lst);
        }

        public IList GetOpeOrgsByInstance()
        {
            return service.GetOpeOrgsByInstance();
        }

        public object GetDomain(Type t, ObjectQuery l)
        {
            return service.GetDomain(t, l);
        }
        public IList GetDomainByCondition(string ClassName, ObjectQuery oq)
        {
            return service.GetDomainByCondition(ClassName, oq);
        }
        public IList GetDetailProperties(string DetailClassName)
        {
            return service.GetDetailProperties(DetailClassName);
        }
        public IList GetMasterProperties(string MasterClassName)
        {
            return service.GetMasterProperties(MasterClassName);
        }

        public IList GetAppMasterProperties(string parentId)
        {
            return service.GetAppMasterProperties(parentId);
        }

        public IList GetAppDetailProperties(string parentId)
        {
            return service.GetAppDetailProperties(parentId);
        }
        //System.Configuration.ConfigurationSettings.AppSettings["IfMessage"]
        //        declare
        //  sID varchar2(100):='3aT4Y6cwDBwQ_CNW4EyhoL';
        //  sClassName varchar2(100):='';
        //  sErrMsg varchar2(1000):='';
        //  ds appfun_GetPersonByName.DataSet ;
        //begin
        //  appfun_GetPersonByName.GetPerson(sBillID => sID,sClassName => sClassName,sErrMsg => sErrMsg,Persons => ds);
        //  dbms_output.put_line('错误:'||sErrMsg);
        //  for str in (select * from temp_resperson ) loop
        //      dbms_output.put_line( '姓名:'||str.pername||';岗位:'||str.jobname);
        //  end loop;
        //end;
        public bool SendMessage(string sBillID, string sClassName)
        {
            string sSender = "系统管理员";
            string sReceiver = string.Empty;
            string sJobName = string.Empty;
            string sContent = string.Empty;
            try
            {
                if (Service.IfSendMessage() == false)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(sBillID))
                {
                    throw new Exception("单据ID不能为空");
                }
                //if (string.IsNullOrEmpty(sClassName))
                //{
                //    throw new Exception("单据所属的类名不能为空");
                //}
                DataSet ds = Service.GetSubmitBillPerson(sBillID, sClassName);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                   // throw new Exception("该单据没有找到审批人");
                }
                else
                {
                    SendMessageUtil sUtil = new SendMessageUtil();
                    int rows = ds.Tables[0].Rows.Count;
                    foreach (DataRow oRow in ds.Tables[0].Rows)
                    {
                        sReceiver = ClientUtil.ToString(oRow["PERNAME"]);
                        sJobName = ClientUtil.ToString(oRow["JOBNAME"]);
                        sContent = string.Format("在【{0}】岗位下有单据需要您审批，请及时查看", sJobName);
                        sUtil.SendICUMsg(sSender, sReceiver, sContent, false, true);
                        if (rows < 40)
                        {
                            LogData log = new LogData();
                            log.BillId = sBillID;
                            log.BillType = "手机短消息";
                            log.Code = "";
                            log.Descript = "接收者[" + sReceiver + "]岗位[" + sJobName + "]类名[" + sClassName + "]";
                            log.OperPerson = sReceiver;
                            StaticMethod.InsertLogData(log);
                        }
                    }
                    if (rows >= 40)
                    {
                        LogData log = new LogData();
                        log.BillId = sBillID;
                        log.BillType = "手机短消息";
                        log.Code = "";
                        log.Descript = "超过40个发送者,发送人数[" + rows + "]人,类名[" + sClassName + "]";
                        log.OperPerson = sReceiver;
                        StaticMethod.InsertLogData(log);
                    } 
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return true;
        }

    }
}
