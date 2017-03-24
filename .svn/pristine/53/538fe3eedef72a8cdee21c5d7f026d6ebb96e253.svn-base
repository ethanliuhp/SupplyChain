function BasePath() {
    var location = (window.location + '').split('/');
    var basePath1 = location[0] + '//' + location[2] + '/' + location[3];
    return basePath1;
}
var sOperationOrgUrl = BasePath() + "/Show/ShowOrg.aspx";
var sOperationPersonUrl = BasePath() + "/Show/ShowPerson.aspx";
var sMaterialCategoryUrl = BasePath() + "/Show/ShowMaterialCata.aspx";
var sMaterialUrl = BasePath() + "/Show/ShowMaterial.aspx";
var sWorkPlaceUrl = BasePath() + "/Show/ShowWorkPlace.aspx";
var sCostAccountSubjectUrl = BasePath() + "/Show/ShowCostAccountSubject.aspx";

var sStandardUnitUrl = BasePath() + "/Show/ShowStandardUnit.aspx"

var sCourseTypeUrl = BasePath() + "/Show/ShowLessionType.aspx";
var sSubjectUrl = BasePath() + "/Show/ShowSubject.aspx";
var sDocumentCate = BasePath() + "/Show/ShowDocumentCate.aspx";

var Feater = "center:yes;help:no;status:no;location:no;";

//返回物资信息  ID ,Name ,Specification ,Stuff ,Code   josn格式
//IsSingle=true 选择一个物资信息 否则多个

function GetMaterial(IsSingle) {
    var sUrl = sMaterialUrl;
    sUrl = GetUrl(sUrl, IsSingle)
    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:800px "));
}
//返回物资分类信息  ID ,Name ,Specification ,Stuff ,Code   josn格式
//IsSingle=true 选择一个物资分类信息 否则多个
function GetMaterialCategory(IsSingle) {
    var sUrl = sMaterialCategoryUrl;
    sUrl = GetUrl(sUrl, IsSingle)
    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:600px; " + Feater));
}
//返回计量单位
function GetStandardUnit(IsSingle) {
    var sUrl = sStandardUnitUrl;
    sUrl = GetUrl(sUrl, IsSingle)
    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:900px; " + Feater));
}
//返回组织信息 ID Name SysCode josn格式
//IsSingle=true 选择一个组织信息 否则多个
function GetOperationOrg(IsSingle) {
    var sUrl = sOperationOrgUrl;
    sUrl = GetUrl(sUrl, IsSingle)
    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:700px; " + Feater));

}
//返回人员信息 ID Name 是josn格式的
//IsSingle=true 选择一个人员信息 否则多个
function GetPerson(IsSingle) {
    var sUrl = sOperationPersonUrl;
    sUrl=GetUrl(sUrl, IsSingle)
    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:700px; " + Feater));
}
//        教师 = 0, 
//        教务管理 = 1, 
//        教辅人员 = 2, 
//        销售人员 = 3, 
//        财务人员 = 4, 
//        系统管理员 = 5, 
//        学员 = 20, 
//        家长 = 21, 
//        注册用户 = 22, 
//        其他 = 50
//选择注册用户ID Name
function GetRegister(IsSingle) {
    var sUrl = sOperationPersonUrl;
    sUrl = GetUrl(sUrl, IsSingle)
    sUrl = sUrl + "&PersonType=22";
    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:600px; " + Feater));
}
//选择学生ID Name
function GetStudent(IsSingle) {
    var sUrl = sOperationPersonUrl;
    sUrl = GetUrl(sUrl, IsSingle)
    sUrl = sUrl + "&PersonType=20";
    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:700px; " + Feater));
}
//选择家长ID Name
function GetParent(IsSingle) {
    var sUrl = sOperationPersonUrl;
    sUrl = GetUrl(sUrl, IsSingle)
    sUrl = sUrl + "&PersonType=21";
    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:700px; " + Feater));
}
//选择老师ID Name
function GetTeach(IsSingle) {
    var sUrl = sOperationPersonUrl;
    sUrl = GetUrl(sUrl, IsSingle)
    sUrl = sUrl + "&PersonType=0";
    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:700px; " + Feater));
}
//选择学生ID Name
function GetStudentAndTeach(IsSingle) {
    var sUrl = sOperationPersonUrl;
    sUrl = GetUrl(sUrl, IsSingle)
    sUrl = sUrl + "&PersonType=0,20";
    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:700px; " + Feater));
}
///教师类型(教师 = 0,管理人员 = 1,辅助人员 = 2,销售人员 = 3,其他 = 4)
function GetPersonByTeachType(IsSingle,TeachType) {
    var sUrl = sOperationPersonUrl;
    sUrl = GetUrl(sUrl, IsSingle)
    sUrl = sUrl + "&TeachType=" + TeachType;
    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:700px; " + Feater));
}
//人员类型(注册用户 = 0,学员 = 1,家长 = 2,教辅人员 = 3)
function GetPersonByPersonType(IsSingle, PersonType) {
    var sUrl = sOperationPersonUrl;
    sUrl = GetUrl(sUrl, IsSingle)
    sUrl = sUrl + "&PersonType=" + PersonType;
    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:700px; " + Feater));
}
//教师类型(教师 :PersonType=0,管理人员 :PersonType=1,辅助人员 :PersonType=2,销售人员 :PersonType=3,其他 :PersonType=4)
//人员类型(注册用户 : PersonType=0,学员 :PersonType= 1,家长:PersonType = 2,教辅人员:PersonType =3)
function GetPersonByUrl(IsSingle, para) {
    var sUrl = sOperationPersonUrl;
    sUrl = GetUrl(sUrl, IsSingle)
    sUrl = sUrl + "&" + para;
    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:700px; " + Feater));
}
//培训场所 ID Name SysCode
function GetWorkPlace(IsSingle) {
    var sUrl = sWorkPlaceUrl;
    sUrl = GetUrl(sUrl, IsSingle)

    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:600px; " + Feater));
}

//核算科目 ID Name SysCode
function GetCostAccountSubject(IsSingle) {
    var sUrl = sCostAccountSubjectUrl;
    sUrl = GetUrl(sUrl, IsSingle)

    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:600px; " + Feater));
}

//课程类型 ID Name
function GetCourseType(IsSingle) {
    var sUrl = sCourseTypeUrl;
    sUrl = GetUrl(sUrl, IsSingle)

    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:700px; " + Feater));
}

//课程 ID Name
function GetSubject(IsSingle) {
    var sUrl = sSubjectUrl;
    sUrl = GetUrl(sUrl, IsSingle)

    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:900px; " + Feater));
}
function GetDocumentCate(IsSingle) {
    var sUrl = sDocumentCate;
    sUrl = GetUrl(sUrl, IsSingle)

    return SetNull(openModel(sUrl, "", "dialogHeight:400px;dialogWidth:400px; " + Feater));
}
function GetUrl(sUrl, IsSingle) {
    //  var sUrl = sOperationPersonUrl;
     
    var d = new Date();
    var val = Math.random();
    val = "random=" + val;
    if (IsSingle != null) {
        sUrl += "?IsSingle=" + IsSingle + "&" + val;
    }
    else {
        sUrl += "?" + val;
    }
    return sUrl;
}

function SetNull(vReturnValue) {
    if (vReturnValue == undefined || vReturnValue == null || vReturnValue.length == 0) {
        vReturnValue = null;
    }
    return vReturnValue;
}
function openModel1(sUrl) {
    openModel(sUrl, "", "dialogHeight:400px;dialogWidth:400px; " + Feater);
}
function openModel(sUrl, vArguments, sFeatures) {
    var vReturnValue = "";
    var vValue = window.showModalDialog(sUrl, vArguments, sFeatures);
    //debugger;
    if (vValue != undefined) {
        vReturnValue = vValue;
    }
    else {
        vReturnValue = window.returnValue;
    }
    if (vReturnValue == undefined) {
        vReturnValue = null;
    }
    return vReturnValue;
}

//删除选择文本时删除对应的id
function OnSelectObjTextChange(control) {
    if (control.value == "") {
        var hiddenControl = control.nextSibling;
        while (hiddenControl && hiddenControl.type != "hidden") {
            hiddenControl = hiddenControl.nextSibling;
        }
        if (hiddenControl)
            hiddenControl.value = "";
    }
}