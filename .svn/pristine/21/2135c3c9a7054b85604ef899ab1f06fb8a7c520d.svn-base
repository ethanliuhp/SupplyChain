using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Collections;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace ImportIntegration
{
    public class PartConfigureLogic
    {
        #region Public

        //Read PartConfigure By Config File
        public static bool ReadPartConfigureResult(Hashtable ObjectTypeConfigure, string strFileName)
        {
            if (ObjectTypeConfigure.Count > 0)
                return true;

            string assemName = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
            string strPath = System.IO.Path.GetDirectoryName(assemName);
            string configFolder = Path.Combine(strPath, CONFIG_FOLDER);
            if (System.IO.Directory.Exists(configFolder) == false)
                configFolder = Path.Combine(strPath, CONFIG_FOLDER2);

            string strConfigureFile = Path.Combine(configFolder, strFileName);

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(strConfigureFile);

            XmlNodeList topM = xmldoc.DocumentElement.ChildNodes;
            foreach (XmlElement element in topM)
            {
                if (element.Name.ToLower() == "partloader")
                {
                    XmlNodeList nodelist = element.ChildNodes;

                    if (nodelist.Count > 0)
                    {
                        foreach (XmlElement el in nodelist)
                        {
                            if (el.Name.ToLower() == "objecttype")
                            {
                                string sValue = "", sName = "", sType = ""; ;
                                Hashtable htObjType = new Hashtable();

                                List<PropertyType> partProperties = new List<PropertyType>();
                                List<PropertyType> bomProperties = new List<PropertyType>();
                                List<PropertyType> docProperties = new List<PropertyType>();

                                PartTypeStr = el.GetAttribute("Name") == null ? "PART" : el.GetAttribute("Name");
                                FileTypeStr = el.GetAttribute("FileType") == null ? "FILE" : el.GetAttribute("FileType");
                                BomTypeStr = el.GetAttribute("BomType") == null ? "PARTEBOM" : el.GetAttribute("BomType");
                                FileStrTypeStr = el.GetAttribute("FileStrType") == null ? "FILESTRUCTURE" : el.GetAttribute("FileStrType");
                                PartToFileTypeStr = el.GetAttribute("PartToFileType") == null ? "PARTTOFILELINK" : el.GetAttribute("PartToFileType");
                                StartPartEntityStr = el.GetAttribute("StartPartEntity") == null ? "PART" : el.GetAttribute("StartPartEntity");
                                StartFileEntityStr = el.GetAttribute("StartFileEntity") == null ? "FILE" : el.GetAttribute("StartFileEntity");


                                NodeDisplayRuleStr = el.GetAttribute("NodeDisplayRule") == null ? "CODE" : el.GetAttribute("NodeDisplayRule");
                                BindRuleStr = el.GetAttribute("BindRule") == null ? "CODE" : el.GetAttribute("BindRule");
                                XmlNodeList chlidList = el.ChildNodes;
                                if (chlidList.Count > 0)
                                {
                                    foreach (XmlElement child in chlidList)
                                    {
                                        if (child.Name.ToLower() == "fitchar")
                                        {
                                            sValue = child.InnerText;
                                            htObjType.Add("FitChar", sValue);
                                        }
                                        else if (child.Name.ToLower() == "bomparentcolumn")
                                        {
                                            sValue = child.InnerText;
                                            htObjType.Add("BOMParentColumn", sValue);
                                        }
                                        else if (child.Name.ToLower() == "bomchildcolumn")
                                        {
                                            sValue = child.InnerText;
                                            htObjType.Add("BOMChildColumn", sValue);
                                        }
                                        else if (child.Name.ToLower() == "filecolumn")
                                        {
                                            sValue = child.InnerText;
                                            htObjType.Add("FileColumn", sValue);
                                        }
                                        else if (child.Name.ToLower() == "conditioncolumn")
                                        {
                                            sName = child.GetAttribute("Name");
                                            sValue = child.GetAttribute("Value");
                                            Hashtable ht = new Hashtable();
                                            ht.Add("Name", sName);
                                            ht.Add("Value", sValue);
                                            htObjType.Add("ConditionColumn", ht);
                                        }
                                        else if (child.Name.ToLower() == "partpropertyname")
                                        {
                                            sName = child.GetAttribute("Name");
                                            sValue = child.GetAttribute("Value");
                                            sType = child.GetAttribute("Type");

                                            PropertyType newType = new PropertyType();
                                            newType.Name = sName;
                                            newType.Value = sValue;
                                            newType.Type = sType;
                                            partProperties.Add(newType);
                                        }
                                        else if (child.Name.ToLower() == "bompropertyname")
                                        {
                                            sName = child.GetAttribute("Name");
                                            sValue = child.GetAttribute("Value");
                                            sType = child.GetAttribute("Type");

                                            PropertyType newType = new PropertyType();
                                            newType.Name = sName;
                                            newType.Value = sValue;
                                            newType.Type = sType;


                                            bomProperties.Add(newType);
                                        }
                                        else if (child.Name.ToLower() == "docpropertyname")
                                        {
                                            sName = child.GetAttribute("Name");
                                            sValue = child.GetAttribute("Value");
                                            sType = child.GetAttribute("Type");

                                            PropertyType newType = new PropertyType();
                                            newType.Name = sName;
                                            newType.Value = sValue;
                                            newType.Type = sType;
                                            docProperties.Add(newType);
                                        }
                                    }

                                }

                                htObjType.Add("PartPropertyName", partProperties);
                                htObjType.Add("BOMPropertyName", bomProperties);
                                htObjType.Add("DocPropertyName", docProperties);
                                ObjectTypeConfigure.Add(PartTypeStr, htObjType);




                            }
                        }
                    }
                }
                else if (element.Name.ToLower() == "docloader")
                {
                }
            }

            return true;

        }

        public static List<string> GetObjectTypeList(Hashtable hsObjectTypeConfigure)
        {
            List<string> lsObjectType = new List<string>();
            IEnumerator keys = hsObjectTypeConfigure.Keys.GetEnumerator();
            while (keys.MoveNext())
            {
                string key = (string)keys.Current;
                lsObjectType.Add(key);
            }
            return lsObjectType;
        }

        public static bool AnalysePartConfigue(Hashtable hsObjectTypeConfigure, string strObjectType)
        {
            IEnumerator keys = hsObjectTypeConfigure.Keys.GetEnumerator();
            while (keys.MoveNext())
            {
                string key = (string)keys.Current;
                if (string.Compare(key, strObjectType, true) == 0)
                {
                    Hashtable ht = (Hashtable)hsObjectTypeConfigure[key];
                    IEnumerator ckeys = ht.Keys.GetEnumerator();
                    while (ckeys.MoveNext())
                    {
                        string ckey = (string)ckeys.Current;
                        if (ckey == "FitChar")
                        {
                            FitChar = ht[ckey].ToString();
                        }
                        else if (ckey == "BOMParentColumn")
                        {
                            BOMParentColumn = ht[ckey].ToString();
                        }
                        else if (ckey == "BOMChildColumn")
                        {
                            BOMChildColumn = ht[ckey].ToString();
                        }
                        else if (ckey == "FileColumn")
                        {
                            FileColumn = ht[ckey].ToString();
                        }
                        else if (ckey == "ConditionColumn")
                        {
                            Hashtable hsCondition = (Hashtable)ht[ckey];
                            CondColumnName = hsCondition["Name"].ToString();
                            CondColumnValue = hsCondition["Value"].ToString();
                        }
                        else if (ckey == "PartPropertyName")
                        {
                            PartProperties = (List<PropertyType>)ht[ckey];

                        }
                        else if (ckey == "BOMPropertyName")
                        {
                            BOMProperties = (List<PropertyType>)ht[ckey];
                        }
                        else if (ckey == "DocPropertyName")
                        {
                            DocProperties = (List<PropertyType>)ht[ckey];
                        }
                    }
                }
                else
                {

                    return false;
                }

            }
            return true;
        }

        public static DataTable CreatSourceDataTable(bool isLoadPart, bool isLoadBOM, bool isLoadDoc)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductID", System.Type.GetType("System.String"));
            dt.Columns.Add("TempParent", System.Type.GetType("System.String"));
            dt.Columns.Add("TempKey", System.Type.GetType("System.String"));
            dt.Columns.Add("TempFile", System.Type.GetType("System.String"));

            dt.Columns.Add("PartType", System.Type.GetType("System.String"));
            dt.Columns.Add("BomType", System.Type.GetType("System.String"));
            dt.Columns.Add("FileType", System.Type.GetType("System.String"));
            dt.Columns.Add("FileStructureType", System.Type.GetType("System.String"));
            dt.Columns.Add("PartToFileType", System.Type.GetType("System.String"));

            dt.Columns.Add("ManageType", System.Type.GetType("System.String"));
            dt.Columns.Add("NodeDisplayRule", System.Type.GetType("System.String"));
            dt.Columns.Add("BindRule", System.Type.GetType("System.String"));

            //如果有必填字段加上必填字段
            dt.Columns.Add("FILENAME", System.Type.GetType("System.String"));

            if (isLoadPart)
            {
                foreach (PropertyType newType in PartProperties)
                {
                    if (!dt.Columns.Contains(newType.Name))
                        dt.Columns.Add(newType.Name, System.Type.GetType(newType.Type));
                }
            }

            if (isLoadBOM)
            {
                foreach (PropertyType newType in BOMProperties)
                {
                    if (!dt.Columns.Contains(newType.Name))
                        dt.Columns.Add(newType.Name, System.Type.GetType(newType.Type));
                }
            }

            if (isLoadDoc)
            {
                foreach (PropertyType newType in DocProperties)
                {
                    if (!dt.Columns.Contains(newType.Name))
                        dt.Columns.Add(newType.Name, System.Type.GetType(newType.Type));
                }
            }
            return dt;
        }

        /// <summary>
        /// Initial part row .
        /// Set column value for TempKey,TempParent,TempFile
        /// </summary>
        /// <param name="row"></param>
        /// <param name="newRow"></param>
        /// <param name="strProductID"></param>
        /// <param name="strTempParent"></param>
        /// <param name="strTempKey"></param>
        /// <param name="strFile"></param>
        /// <param name="strFileNameColumn"></param>
        /// <param name="strFileFullNameColumn"></param>
        public static void AnalyseDataRow(DataRow row, DataRow newRow, string strProductID, string strTempParent, string strTempKey, string strFile, ref string strFileNameColumn, ref string strFileFullNameColumn)
        {
            string sValue = "";


            foreach (PropertyType newType in PartProperties)
            {
                sValue = GetValueByKeys(newType.Value, row);
                newRow[newType.Name] = sValue;
            }

            foreach (PropertyType newType in BOMProperties)
            {
                sValue = GetValueByKeys(newType.Value, row);
                newRow[newType.Name] = sValue;
            }

            newRow["ProductID"] = strProductID;
            newRow["TempParent"] = strTempParent;
            newRow["TempKey"] = strTempKey;
            newRow["TempFile"] = strFile;


            if (newRow.Table.Columns.Contains("FILENAME"))
            {
                newRow["FILENAME"] = Path.GetFileNameWithoutExtension(strFile);
            }

            if (newRow.Table.Columns.Contains("FILEFULLNAME"))
            {
                newRow["FILEFULLNAME"] = Path.GetFileName(strFile);

            }

            foreach (PropertyType newType in DocProperties)
            {
                sValue = GetValueByKeys(newType.Value, row);
                newRow[newType.Name] = sValue;
            }

            

            string managetype = BatchImportLocalize.GetLocalizedText("ManageType");
            string changed = BatchImportLocalize.GetLocalizedText("Changed");

            if (row[managetype].ToString() == changed)
                newRow["ManageType"] = "Changed";
            else
                newRow["ManageType"] = "Unchanged";
        }

        /// <summary>
        /// set file path to this part
        /// </summary>
        /// <param name="newRow"></param>
        /// <param name="strFileName"></param>
        public static void AnalyseDataRow(DataRow newRow, string strFileName, string strFileType, string strFileStruType, string strFileLinkPartType)
        {
            newRow["TempFile"] = strFileName;
            newRow["FileType"] = strFileType;
            newRow["FileStructureType"] = strFileStruType;
            newRow["PartToFileType"] = strFileLinkPartType;


            if (newRow.Table.Columns.Contains("FILENAME"))
            {
                newRow["FILENAME"] = Path.GetFileNameWithoutExtension(strFileName);
            }

            if (newRow.Table.Columns.Contains("FILEFULLNAME"))
            {
                newRow["FILEFULLNAME"] = Path.GetFileName(strFileName);

            }


        }

        /// <summary>
        /// Set part type and bom type
        /// </summary>
        /// <param name="newRow"></param>
        /// <param name="strPartType"></param>
        /// <param name="strBomType"></param>
        public static void AnalyseDataRow(DataRow newRow, string strPartType, string strBomType)
        {
            newRow["PartType"] = strPartType;
            newRow["BomType"] = strBomType;

        }
        /// <summary>
        /// Get BOMParentColumn name and BOMChildColumn name and FileColumn name from configration
        /// </summary>
        /// <param name="row"></param>
        /// <param name="strParent"></param>
        /// <param name="strKey"></param>
        /// <param name="strFile"></param>
        public static void GetParentFromDataTable(DataRow row, ref string strParent, ref string strKey, ref string strFile)
        {
            strParent = GetValueByKeys(BOMParentColumn, row);
            strKey = GetValueByKeys(BOMChildColumn, row);
            strFile = GetValueByKeys(FileColumn, row);
            string sValue = "";
            foreach (DataColumn dc in row.Table.Columns)
            {
                sValue = GetValueByKeys(dc.ColumnName, row);
                if (dc.DataType != System.Type.GetType("System.String"))
                {
                    sValue=sValue == "" ? "0" : sValue;
                }
                row[dc.ColumnName] = sValue;
            }
        }
        public static void SetSourceDataStructure(DataTable sourceDataTable, DataTable dt)
        {
            foreach (DataColumn dc in dt.Columns)
            {
                if (!sourceDataTable.Columns.Contains(dc.ColumnName))
                    sourceDataTable.Columns.Add(dc.ColumnName);
            }
        }

        public static void PopulateSourceData(DataRow rowSource, DataRow newRow)
        {


            foreach (DataColumn dcNew in newRow.Table.Columns)
            {
                if (rowSource.Table.Columns.Contains(dcNew.ColumnName))
                    newRow[dcNew.ColumnName] = rowSource[dcNew.ColumnName];
            }
        }

        public static void SaveData(string strXmlData)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strXmlData);
            string xmlpath = "C:\\tempFile\\";
            if (!Directory.Exists(xmlpath))
            {
                Directory.CreateDirectory(xmlpath);
            }
            xmlDoc.Save(xmlpath + "\\BatchImport" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml");
        }

        public static string PartTypeStr = null;
        public static string FileTypeStr = null;
        public static string BomTypeStr = null;
        public static string FileStrTypeStr = null;
        public static string PartToFileTypeStr = null;
        public static string StartPartEntityStr = null;
        public static string StartFileEntityStr = null;

        public static string NodeDisplayRuleStr = null;
        public static string BindRuleStr = null;
        #endregion

        #region private
        private static string FitChar;
        private static string CondColumnName;
        private static string CondColumnValue;
        private static string BOMParentColumn;
        private static string BOMChildColumn;
        private static string FileColumn;
        private static List<PropertyType> PartProperties = new List<PropertyType>();
        private static List<PropertyType> BOMProperties = new List<PropertyType>();
        private static List<PropertyType> DocProperties = new List<PropertyType>();
        private const string CONFIG_FOLDER = @"..\Config";
        private const string CONFIG_FOLDER2 = @"FileUploadConfig";//配置目录2（当第一个目录不存在时使用）

        public static string ObjectTypeStr = null;

        private static string GetValueByKeys(string strKeys, DataRow row)
        {
            if (strKeys == string.Empty)
                return "";
            string strResult = "";

            ArrayList KeyArray = new ArrayList();
            int nIndex = strKeys.IndexOf(FitChar);
            while (nIndex > 0)
            {
                string strTemp = strKeys.Substring(0, nIndex);
                KeyArray.Add(strTemp);
                strKeys = strKeys.Substring(nIndex + FitChar.Length);
                nIndex = strKeys.IndexOf(FitChar);
            }
            KeyArray.Add(strKeys);

            int iSize = KeyArray.Count;
            int i = 0;
            for (i = 0; i < iSize; i++)
            {
                string strValue = "";
                string sTemp = KeyArray[i].ToString();

                strValue = GetFieldData(sTemp, row);

                if (strValue != string.Empty)
                {
                    if (strResult == string.Empty)
                    {
                        strResult = strValue;
                    }
                    else
                    {
                        strResult = strResult + FitChar + strValue;
                    }
                }
            }
            strResult = strResult.Trim();
            return strResult;

        }
        private static string GetFieldData(string strKey, DataRow row)
        {
            string strData = "";
            if (row.Table.Columns.Contains(strKey))
            {
                if (row[strKey] != null)
                    strData = row[strKey].ToString();
            }
            strData = strData.Trim();
            return strData;
        }
        #endregion
    }

    public class PropertyType
    {
        public PropertyType()
        {
        }

        public string Name
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }
        public string Type
        {
            get;
            set;
        }
    }

}
