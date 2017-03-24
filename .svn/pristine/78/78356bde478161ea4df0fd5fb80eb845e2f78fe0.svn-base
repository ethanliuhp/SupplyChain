using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.CustomProperties;
using DocumentFormat.OpenXml.VariantTypes;
using System.IO;
//using DSOFile;
using System.Configuration;
using System.Text;
using DSOFile;

/// <summary>
///FilePropertyUtility 的摘要说明
/// </summary>
public static class FilePropertyUtility
{
    static FilePropertyUtility()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //

        char[] chs = { '|' };

        string isEnabledWriteCustomPropertyStr = ConfigurationSettings.AppSettings["IsEnabledWriteCustomProperty"];
        if (!string.IsNullOrEmpty(isEnabledWriteCustomPropertyStr))
        {
            IsEnabledWriteCustomProperty = Convert.ToBoolean(isEnabledWriteCustomPropertyStr);
        }

        string AllowCustomPropertyExtFileNames = ConfigurationSettings.AppSettings["AllowCustomPropertyExtFileNames"];
        if (!string.IsNullOrEmpty(AllowCustomPropertyExtFileNames))
        {
            ListAllowExtFileNames.AddRange(AllowCustomPropertyExtFileNames.Split(chs, StringSplitOptions.RemoveEmptyEntries));
        }

        string CommonFileExtFileNames = ConfigurationSettings.AppSettings["CommonFileExtFileNames"];
        if (!string.IsNullOrEmpty(CommonFileExtFileNames))
        {
            ListCommonFile.AddRange(CommonFileExtFileNames.Split(chs, StringSplitOptions.RemoveEmptyEntries));
        }

        string OfficeFileExtFileNames = ConfigurationSettings.AppSettings["OfficeFileExtFileNames"];
        if (!string.IsNullOrEmpty(OfficeFileExtFileNames))
        {
            ListOfficeFile.AddRange(OfficeFileExtFileNames.Split(chs, StringSplitOptions.RemoveEmptyEntries));
        }
    }

    private static bool _isEnabledWriteCustomProperty = false;
    private static List<string> _listAllowExtFileNames = new List<string>();
    private static List<string> _listCommonFile = new List<string>();//普通文件
    private static List<string> _listOfficeFile = new List<string>();//Office文件

    /// <summary>
    /// 是否启用写文件扩展属性功能
    /// </summary>
    public static bool IsEnabledWriteCustomProperty
    {
        get { return FilePropertyUtility._isEnabledWriteCustomProperty; }
        set { FilePropertyUtility._isEnabledWriteCustomProperty = value; }
    }
    /// <summary>
    /// 允许写扩展属性的文件扩展名
    /// </summary>
    public static List<string> ListAllowExtFileNames
    {
        get { return FilePropertyUtility._listAllowExtFileNames; }
        set { FilePropertyUtility._listAllowExtFileNames = value; }
    }
    /// <summary>
    /// 普通文件（根据不同文件类型使用不同二进制转换方式）
    /// </summary>
    public static List<string> ListCommonFile
    {
        get { return FilePropertyUtility._listCommonFile; }
        set { FilePropertyUtility._listCommonFile = value; }
    }
    /// <summary>
    /// Office文件（根据不同文件类型使用不同二进制转换方式）
    /// </summary>
    public static List<string> ListOfficeFile
    {
        get { return FilePropertyUtility._listOfficeFile; }
        set { FilePropertyUtility._listOfficeFile = value; }
    }

    public static bool SetCustomProperty(string filePath, Dictionary<string, object> dicKeyValue)
    {
        if (string.IsNullOrEmpty(filePath) || dicKeyValue == null || dicKeyValue.Count == 0)
            return false;

        string extName = Path.GetExtension(filePath).ToLower();

        if (ListAllowExtFileNames.Contains(extName) == false)
            return false;

        DocumentType type = getDocumentType(extName);

        if (type == DocumentType.Word2007 || type == DocumentType.Excel2007 || type == DocumentType.PowerPoint2007)
        {
            foreach (string key in dicKeyValue.Keys)
            {
                object value = dicKeyValue[key];

                if (value == null)
                    value = "";

                SetCustomPropertyByOffice2007(type, filePath, key, value, PropertyTypes.Text);
            }
        }
        else
        {
            SetCustomPropertyByCommonFile(filePath, dicKeyValue);
        }

        string WriteFilePropertyLogDir = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["WriteFilePropertyLogDir"];
        if (!string.IsNullOrEmpty(WriteFilePropertyLogDir))
        {
            object value = FilePropertyUtility.GetPropertyByDocumentType(type, filePath, "文档GUID");

            if (value != null)
            {
                if (!string.IsNullOrEmpty(WriteFilePropertyLogDir) && Directory.Exists(WriteFilePropertyLogDir) == false)
                {
                    Directory.CreateDirectory(WriteFilePropertyLogDir);
                }

                StreamWriter write = new StreamWriter(WriteFilePropertyLogDir + @"\\WriteFileWindowsPropertyLog.txt", true, Encoding.Default);
                write.WriteLine(DateTime.Now.ToString() + ",文件名称：" + Path.GetFileName(filePath) + ",扩展属性(文档GUID)：" + value);
                write.Close();
                write.Dispose();
            }
        }

        return true;
    }

    private static bool IsExistsCustomProperty(CustomProperties properties, string customPropertyName)
    {
        bool flag = false;

        foreach (CustomProperty cp in properties)
        {
            if (cp.Name == customPropertyName)
            {
                return true;
            }
        }

        return flag;
    }

    public static DocumentType getDocumentType(string sExtension)
    {
        sExtension = sExtension.ToLower();

        DocumentType type = DocumentType.CommonFile;

        if (sExtension == ".docx")
        {
            type = DocumentType.Word2007;
        }
        else if (sExtension == ".xlsx")
        {
            type = DocumentType.Excel2007;
        }
        else if (sExtension == ".pptx")
        {
            type = DocumentType.PowerPoint2007;
        }
        else
        {
            type = DocumentType.CommonFile;
        }

        return type;
    }

    /// <summary>
    /// 设置windows文件扩展属性
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="propertyName"></param>
    /// <param name="propertyValue"></param>
    /// <param name="propertyType"></param>
    /// <returns></returns>
    public static string SetCustomPropertyByOffice2007(DocumentType docType,
    string fileName,
    string propertyName,
    object propertyValue,
    PropertyTypes propertyType)
    {
        // Given a document name, a property name/value, and the property type, 
        // add a custom property to a document. The method returns the original
        // value, if it existed.

        string returnValue = null;
        try
        {
            var newProp = new CustomDocumentProperty();
            bool propSet = false;

            // Calculate the correct type.
            switch (propertyType)
            {
                case PropertyTypes.DateTime:

                    // Be sure you were passed a real date, 
                    // and if so, format in the correct way. 
                    // The date/time value passed in should 
                    // represent a UTC date/time.
                    if ((propertyValue) is DateTime)
                    {
                        newProp.VTFileTime =
                            new VTFileTime(string.Format("{0:s}Z",
                                Convert.ToDateTime(propertyValue)));
                        propSet = true;
                    }

                    break;

                case PropertyTypes.NumberInteger:
                    if ((propertyValue) is int)
                    {
                        newProp.VTInt32 = new VTInt32(propertyValue.ToString());
                        propSet = true;
                    }

                    break;

                case PropertyTypes.NumberDouble:
                    if (propertyValue is double)
                    {
                        newProp.VTFloat = new VTFloat(propertyValue.ToString());
                        propSet = true;
                    }

                    break;

                case PropertyTypes.Text:
                    newProp.VTLPWSTR = new VTLPWSTR(propertyValue.ToString());
                    propSet = true;

                    break;

                case PropertyTypes.YesNo:
                    if (propertyValue is bool)
                    {
                        // Must be lowercase.
                        newProp.VTBool = new VTBool(
                          Convert.ToBoolean(propertyValue).ToString().ToLower());
                        propSet = true;
                    }
                    break;
            }

            if (!propSet)
            {
                // If the code was not able to convert the 
                // property to a valid value, throw an exception.
                throw new InvalidDataException("propertyValue");
            }

            // Now that you have handled the parameters, start
            // working on the document.
            newProp.FormatId = "{D5CDD505-2E9C-101B-9397-08002B2CF9AE}";
            newProp.Name = propertyName;


            if (docType == DocumentType.Word2007)
            {
                using (var document = WordprocessingDocument.Open(fileName, true))
                {
                    var customProps = document.CustomFilePropertiesPart;
                    if (customProps == null)
                    {
                        // No custom properties? Add the part, and the
                        // collection of properties now.
                        customProps = document.AddCustomFilePropertiesPart();
                        customProps.Properties =
                            new DocumentFormat.OpenXml.CustomProperties.Properties();
                    }

                    var props = customProps.Properties;
                    if (props != null)
                    {
                        // This will trigger an exception if the property's Name 
                        // property is null, but if that happens, the property is damaged, 
                        // and probably should raise an exception.
                        var prop = props.Where(p => ((CustomDocumentProperty)p).Name.Value == propertyName).FirstOrDefault();

                        // Does the property exist? If so, get the return value, 
                        // and then delete the property.
                        if (prop != null)
                        {
                            returnValue = prop.InnerText;
                            prop.Remove();
                        }

                        // Append the new property, and 
                        // fix up all the property ID values. 
                        // The PropertyId value must start at 2.
                        props.AppendChild(newProp);
                        int pid = 2;
                        foreach (CustomDocumentProperty item in props)
                        {
                            item.PropertyId = pid++;
                        }
                        props.Save();
                    }
                }
            }
            else if (docType == DocumentType.Excel2007)
            {
                using (var document = SpreadsheetDocument.Open(fileName, true))
                {
                    var customProps = document.CustomFilePropertiesPart;
                    if (customProps == null)
                    {
                        // No custom properties? Add the part, and the
                        // collection of properties now.
                        customProps = document.AddCustomFilePropertiesPart();
                        customProps.Properties =
                            new DocumentFormat.OpenXml.CustomProperties.Properties();
                    }

                    var props = customProps.Properties;
                    if (props != null)
                    {
                        // This will trigger an exception if the property's Name 
                        // property is null, but if that happens, the property is damaged, 
                        // and probably should raise an exception.
                        var prop = props.Where(p => ((CustomDocumentProperty)p).Name.Value == propertyName).FirstOrDefault();

                        // Does the property exist? If so, get the return value, 
                        // and then delete the property.
                        if (prop != null)
                        {
                            returnValue = prop.InnerText;
                            prop.Remove();
                        }

                        // Append the new property, and 
                        // fix up all the property ID values. 
                        // The PropertyId value must start at 2.
                        props.AppendChild(newProp);
                        int pid = 2;
                        foreach (CustomDocumentProperty item in props)
                        {
                            item.PropertyId = pid++;
                        }
                        props.Save();
                    }
                }
            }
            else if (docType == DocumentType.PowerPoint2007)
            {
                using (var document = PresentationDocument.Open(fileName, true))
                {
                    var customProps = document.CustomFilePropertiesPart;
                    if (customProps == null)
                    {
                        // No custom properties? Add the part, and the
                        // collection of properties now.
                        customProps = document.AddCustomFilePropertiesPart();
                        customProps.Properties =
                            new DocumentFormat.OpenXml.CustomProperties.Properties();
                    }

                    var props = customProps.Properties;
                    if (props != null)
                    {
                        // This will trigger an exception if the property's Name 
                        // property is null, but if that happens, the property is damaged, 
                        // and probably should raise an exception.
                        var prop = props.Where(p => ((CustomDocumentProperty)p).Name.Value == propertyName).FirstOrDefault();

                        // Does the property exist? If so, get the return value, 
                        // and then delete the property.
                        if (prop != null)
                        {
                            returnValue = prop.InnerText;
                            prop.Remove();
                        }

                        // Append the new property, and 
                        // fix up all the property ID values. 
                        // The PropertyId value must start at 2.
                        props.AppendChild(newProp);
                        int pid = 2;
                        foreach (CustomDocumentProperty item in props)
                        {
                            item.PropertyId = pid++;
                        }
                        props.Save();
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
        return returnValue;
    }

    /// <summary>
    /// 设置文件扩展属性
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="dicKeyValue"></param>
    /// <returns></returns>
    public static bool SetCustomPropertyByCommonFile(string filePath, Dictionary<string, object> dicKeyValue)
    {
        if (string.IsNullOrEmpty(filePath) || dicKeyValue == null || dicKeyValue.Count == 0)
            return false;

        try
        {
            DSOFile.OleDocumentPropertiesClass sd = new OleDocumentPropertiesClass();
            sd.Open(filePath, false, dsoFileOpenOptions.dsoOptionDefault);

            foreach (string key in dicKeyValue.Keys)
            {
                object value = dicKeyValue[key];

                if (value == null)
                    value = "";

                if (IsExistsCustomProperty(sd.CustomProperties, key) == false)
                    sd.CustomProperties.Add(key, ref value);
                else
                    sd.CustomProperties[key].set_Value(ref value);
            }


            sd.Save();
            sd.Close(true);
        }
        catch (Exception ex)
        {
            string msg = GetExceptionMessage(ex);
            string WriteFilePropertyLogDir = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["WriteFilePropertyLogDir"];
            StreamWriter write = new StreamWriter(WriteFilePropertyLogDir + "/WriteFilePropertyErrorLog.txt", true, Encoding.Default);
            write.WriteLine(DateTime.Now.ToString() + ",调用SetCustomPropertyByCommonFile方法执行异常，写扩展属性的文件：" + filePath + "，异常信息：" + msg);
            write.Close();
            write.Dispose();

            throw ex;
        }

        return true;
    }

    private static string GetExceptionMessage(Exception ex)
    {
        if (ex == null || string.IsNullOrEmpty(ex.Message))
            return "";

        string message = ex.Message;
        Exception exTemp = ex.InnerException;
        while (exTemp != null && !string.IsNullOrEmpty(exTemp.Message))
        {
            message += "||||" + exTemp.Message;

            exTemp = exTemp.InnerException;
        }

        return message;
    }

    public static object GetPropertyByDocumentType(DocumentType docType, string filePath, string propertyName)
    {
        if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(propertyName))
            return null;

        propertyName = propertyName.ToLower();

        object value = null;

        if (docType == DocumentType.Word2007)
        {
            using (WordprocessingDocument document = WordprocessingDocument.Open(filePath, true))
            {
                if (document.CustomFilePropertiesPart != null)
                {
                    foreach (var oProperty in document.CustomFilePropertiesPart.Properties.Elements<CustomDocumentProperty>())
                    {
                        if (!string.IsNullOrEmpty(oProperty.Name) && oProperty.Name.ToString().ToLower() == propertyName)
                        {
                            value = oProperty.VTLPWSTR.Text;
                            break;
                        }
                    }
                }
            }
        }
        else if (docType == DocumentType.Excel2007)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, true))
            {
                if (document.CustomFilePropertiesPart != null)
                {
                    foreach (var oProperty in document.CustomFilePropertiesPart.Properties.Elements<CustomDocumentProperty>())
                    {
                        if (!string.IsNullOrEmpty(oProperty.Name) && oProperty.Name.ToString().ToLower() == propertyName)
                        {
                            value = oProperty.VTLPWSTR.Text;
                            break;
                        }
                    }
                }
            }
        }
        else if (docType == DocumentType.PowerPoint2007)
        {
            using (PresentationDocument document = PresentationDocument.Open(filePath, true))
            {
                if (document.CustomFilePropertiesPart != null)
                {
                    foreach (var oProperty in document.CustomFilePropertiesPart.Properties.Elements<CustomDocumentProperty>())
                    {
                        if (!string.IsNullOrEmpty(oProperty.Name) && oProperty.Name.ToString().ToLower() == propertyName)
                        {
                            value = oProperty.VTLPWSTR.Text;
                            break;
                        }
                    }
                }
            }
        }
        else if (docType == DocumentType.CommonFile)
        {
            DSOFile.OleDocumentPropertiesClass sd = new OleDocumentPropertiesClass();
            sd.Open(filePath, false, dsoFileOpenOptions.dsoOptionDefault);

            if (sd != null)
            {
                foreach (CustomProperty cus in sd.CustomProperties)
                {
                    if (!string.IsNullOrEmpty(cus.Name) && cus.Name.ToLower() == propertyName)
                    {
                        value = cus.get_Value();
                        break;
                    }
                }
            }
            sd.Close(false);
        }

        return value;
    }

    /// <summary>
    /// windows文件类型
    /// </summary>
    public enum DocumentType
    {
        CommonFile = 1,
        Word2007 = 2,
        Excel2007 = 3,
        PowerPoint2007 = 4,
        None = 99
    }
    /// <summary>
    /// windows文件属性值类型
    /// </summary>
    public enum PropertyTypes : int
    {
        YesNo,
        Text,
        DateTime,
        NumberInteger,
        NumberDouble
    }
}
