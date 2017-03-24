using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;

namespace Cscec.Organization
{
    public static class OrganizationManager
    {
        private static List<Organization> companyOrganization;
        private static List<User> companyUser;
        //private static List<DepartmentJob> companyDepartmentJob;
        //private static List<PersonJob> companyPersonJob;
        static OrganizationManager()
        {
            // 部门初始化
            companyOrganization = GlobalClass.CommonMethodSrv.GetData("select opgid as ObjectID,opglevel as Grade,opgname as Name,opgcode as Code,parentnodeid as ParentID from resoperationorg").Tables[0].Select().Select(a => new Organization() { ObjectID = a["ObjectID"].ToString(), Code = a["Code"].ToString(), Level = Convert.ToInt32(a["Grade"]), Name = a["Name"].ToString(), ParentID = a["ParentID"].ToString() }).ToList();
            //// 部门岗位初始化
            //companyDepartmentJob = GlobalClass.CommonMethodSrv.GetData("select opjid as ObjectID,opjname as Name,opjcode as Code,opjorgid as DepartmentID from resoperationjob").Tables[0].Select().Select(a => new DepartmentJob() { ObjectID = a["ObjectID"].ToString(), Name = a["Name"].ToString(), Code = a["Code"].ToString(), DepartmentID = a["DepartmentID"].ToString() }).ToList();
            //// 个人岗位初始化
            //companyPersonJob = GlobalClass.CommonMethodSrv.GetData("select peronjobid as ObjectID,operationjobid as DepartmentJobID,perid as UserID from respersononjob").Tables[0].Select().Select(a => new PersonJob() { ObjectID = a["ObjectID"].ToString(), DepartmentJobID = a["DepartmentJobID"].ToString(), UserID = a["UserID"].ToString() }).ToList();
            // 人员初始化
            companyUser = GlobalClass.CommonMethodSrv.GetData("select A.Perid as ObjectID,A.PERNAME as Name,A.PERCODE as Code,C.Opjorgid as ParentID,C.OPJID as JobID from resperson A inner join respersononjob B on A.Perid=B.Perid inner join resoperationjob C on B.Operationjobid=C.Opjid").Tables[0].Select().Select(a => new User() { ObjectID = a["ObjectID"].ToString(), Name = a["Name"].ToString(), Code = a["Code"].ToString(), ParentID = a["ParentID"].ToString(), JobID = a["JobID"].ToString() }).ToList();

        }

        public static User GetUserByCode(string code)
        {
            return companyUser.Where(a => a.Code == code).FirstOrDefault();
        }

        public static List<User> GetUserList(string code)
        {
            return companyUser.Where(a => a.Code.Contains(code)).ToList();
        }

    }
}
