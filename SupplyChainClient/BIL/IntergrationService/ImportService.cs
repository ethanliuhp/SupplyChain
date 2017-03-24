using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using com.think3.PLM.Integration.DataTransfer;
using com.think3.PLM.Integration.Client;

namespace ImportIntegration
{
    public class ImportService
    {
        #region server
        public void Save()
        {
        }

        /// <summary>
        /// Get all Entities by enetity name
        /// </summary>
        /// <returns>Entity collectio</returns>
        public static DataTable GetEntities(string name)
        {
            try
            {
                EntityWebMethods ews = new EntityWebMethods();
                return ews.GetEntities(name);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Get LinkType Entities
        /// </summary>
        /// <param name="entityName">Entity Name</param>
        /// <returns>Entity collection</returns>
        public static DataTable GetLinkTypeEntities(string entityName)
        {
            try
            {
                EntityWebMethods ews = new EntityWebMethods();
                return ews.GetLinkTypeEntities(entityName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        public static void Initional(bool isClient)
        {
            if(isClient)
                source= PartConfigureLogic.CreatSourceDataTable(false, true, false);
        }

        public static void Add(bool isClient, string filePath, string partType, string bomType, string fileType,string fileStructureType,string partTofileType, string producetId)
        {
            string managetype = BatchImportLocalize.GetLocalizedText("ManageType");
            string changed = BatchImportLocalize.GetLocalizedText("Changed");
            string partTypeStr = BatchImportLocalize.GetLocalizedText("PartType");
            string bomTypeStr = BatchImportLocalize.GetLocalizedText("BOMType");

            PartTable.Columns.Add(managetype);
            PartTable.Columns.Add(partTypeStr);
            PartTable.Columns.Add(bomTypeStr);

            for (int i = 0; i < PartTable.Rows.Count; i++)
            {
                PartTable.Rows[i][managetype] = changed;
                PartTable.Rows[i][partTypeStr] = partType;
                PartTable.Rows[i][bomTypeStr] = bomType;
            }
            if (PartTable != null)
            {
                if (isClient)
                {
                    PartTable.Rows.Clear();
                }
                string strFileNameCol = "";
                string strFileFullNameCol = "";
                if (PartTable != null && PartTable.Rows.Count >= 0)
                {
                    PartConfigureLogic.SetSourceDataStructure(source, PartTable);

                    int nSize = PartTable.Rows.Count;

                    for (int i = 0; i < nSize; i++)
                    {
                        DataRow newRow = source.NewRow();
                        string strParent = "";
                        string strTempKey = "";
                        string strFile = "";

                        PartConfigureLogic.GetParentFromDataTable(PartTable.Rows[i], ref strParent, ref strTempKey, ref strFile);
                        PartConfigureLogic.AnalyseDataRow(PartTable.Rows[i], newRow, producetId, strParent, strTempKey, strFile, ref strFileNameCol, ref strFileFullNameCol);
                        PartConfigureLogic.PopulateSourceData(PartTable.Rows[i], newRow);
                        PartConfigureLogic.AnalyseDataRow(newRow, partType, bomType);
                        if (!string.IsNullOrEmpty(strFile))
                            PartConfigureLogic.AnalyseDataRow(newRow, strFile, fileType,fileStructureType, partTofileType);

                        source.Rows.Add(newRow);
                    }
                }
            }
            SetExpresion(); 
        }

        public static DataRow Add(IInfos inf)
        {
            string strPartId = "";
            DataRow drCurrent = null;
            if (inf.GetInfo("PLMEID") != null)
            {
                strPartId = inf.GetInfo("PLMEID").Value;

                drCurrent = GetPart(strPartId);
                if (drCurrent == null)
                {
                    drCurrent = source.NewRow();
                    drCurrent["TempKey"] = strPartId;
                    drCurrent["ManageType"] = "Changed";//UnChanged

                    for (int k = 0; k < inf.Info.Length; k++)
                    {
                        if (!source.Columns.Contains(inf.Info[k].PrpName))
                        {
                            source.Columns.Add(inf.Info[k].PrpName);
                        }
                        drCurrent[inf.Info[k].PrpName] = inf.Info[k].Value;
                        if (inf.Info[k].PrpName == "TMM_COMPONENT")
                        {
                            drCurrent["PartType"] = inf.Info[k].Value;
                        }
                    }
                    source.Rows.Add(drCurrent);
                }
            }
            return drCurrent;
 
        }

        public static void Add(ArrayList files,string fileType,string fileStructureType,string partTofileType)
        {
            for (int i = 0; i < files.Count; i++)
            {
            
            DataRow newRow = source.NewRow();
            PartConfigureLogic.AnalyseDataRow(newRow, files[i].ToString(), fileType, fileStructureType, partTofileType);
            BindingToPart(newRow);
            source.Rows.Add(newRow);
            }
        }

        public static void Add(DataRow drCurrent,string fileName, string partName)
        {
            DataRow dr = source.NewRow();
                    dr.ItemArray = drCurrent.ItemArray;
                    dr["TempKey"] = partName;
                    BindAttribute(dr);
                    if (GetFilesCount(fileName,partName) == 0)
                        source.Rows.Add(dr);
        }

        public static void Remove(DataRow dr)
        {
             source.Rows.Remove(dr);
        }

        public static void BindAttribute(DataRow dr,IInfos inf)
        {
            dr["TempKey"] = inf.GetInfo("PLMEID").Value;
            for (int k = 0; k < inf.Info.Length; k++)
            {
                if (dr.Table.Columns.Contains(inf.Info[k].PrpName))
                {
                    dr[inf.Info[k].PrpName] = inf.Info[k].Value;
                }
            }
        }

        public static DataRow Clone(DataRow dr)
        {
            DataRow drCurrent = source.NewRow();
                drCurrent.ItemArray = dr.ItemArray;
                source.Rows.Remove(dr);
            return drCurrent;
        }

        public static void Clone(DataRow drCurrent, string fileType, string fileStructureType, string partTofileType)
        {
            DataRow newRow = source.NewRow();
            PartConfigureLogic.AnalyseDataRow(newRow, drCurrent["TempFile"].ToString(), fileType, fileStructureType, partTofileType);
            BindAttribute(newRow);
            source.Rows.Add(newRow);
            source.Rows.Remove(drCurrent);
        }

        public static List<string> GetParts()
        {
            List<string> lst = new List<string>();

            for (int i = 0; i < source.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(source.Rows[i]["TempKey"].ToString()) && !string.IsNullOrEmpty(source.Rows[i]["PartType"].ToString()))
                {
                    lst.Add(source.Rows[i]["TempFile"].ToString());
                }
            }
            return lst;
        }

        public static void SetChildPartsInfo()
        {
            int nCount = source.Rows.Count;
            for (int i = 0; i < nCount; i++)
            {
                if (!string.IsNullOrEmpty(source.Rows[i]["TempKey"].ToString()))
                {
                    string parentid = source.Rows[i]["TempKey"].ToString();
                    EntityObj[] objs = cFuntions.SelectChildParts(parentid);
                    if (objs.Length > 0)
                    {
                        for (int j = 0; j < objs.Length; j++)
                        {
                            if (((IInfos)objs[j]).GetInfo("PLMEID") != null)
                            {
                                string childid = ((IInfos)objs[j]).GetInfo("PLMEID").Value;
                                for (int k = 0; k < nCount; k++)
                                {
                                    if (source.Rows[k]["TempKey"].ToString() == childid)
                                    {
                                        source.Rows[k]["TempParent"] = parentid;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    source.Rows[i]["TempKey"] = source.Rows[i]["TempFile"].ToString();
                }
            }
        }

        /// <summary>
        /// Get value of part displayRule
        /// when fill data in filelist contreler
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        public static string GetNodeDisplayRule(string partId)
        {
            string strPartId = null;
            string strNodeDisplay = null;
            foreach (DataRow dr in source.Rows)
            {
                strPartId = dr["TempKey"].ToString();
                if (!string.IsNullOrEmpty(strPartId) && !string.IsNullOrEmpty(dr["PartType"].ToString()))
                {
                    if (partId == strPartId)
                    {
                        strNodeDisplay = dr["NodeDisplayRule"].ToString();
                        return strNodeDisplay;
                    }
                }
            }
            return strNodeDisplay;
        }

        public void UpdataChildPartBomType(DataRow parentDataRow, string bomType)
        {
            List<DataRow> childPart = new List<DataRow>();
            string tempKey = parentDataRow["TempKey"].ToString();
            foreach (DataRow dr in source.Rows)
            {
                if (dr["TempParent"].ToString() == tempKey)
                {
                    dr["BOMType"] = parentDataRow["BOMType"];
                    dr[BatchImportLocalize.GetLocalizedText("BOMType")] = bomType;
                    UpdataChildPartBomType(dr, bomType);
                }
            }
        }

        public static IntergrationFrameWork cFuntions;
        /// <summary>
        /// Data collection for part and file
        /// </summary>
        public static DataTable Source
        {
            get
            {
                return source;
            }
        }

        #region private


        /// <summary>
        /// Get files in data collection with same file name
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        private static int GetFilesCount(string fileId)
        {
            List<DataRow> filesRows = new List<DataRow>();
            foreach (DataRow dr in source.Rows)
            {
                if (!string.IsNullOrEmpty(dr["TempFile"].ToString()) && dr["TempFile"].ToString() == fileId)
                {
                    filesRows.Add(dr);
                }
            }

            return filesRows.Count;
        }

        private static int GetFilesCount(string fileId, string partId)
        {
            List<DataRow> filesRows = new List<DataRow>();
            foreach (DataRow dr in source.Rows)
            {
                if (!string.IsNullOrEmpty(dr["TempFile"].ToString()) && dr["TempFile"].ToString() == fileId && !string.IsNullOrEmpty(dr["TempKey"].ToString()) && dr["TempKey"].ToString() == partId)
                {
                    filesRows.Add(dr);
                }
            }
            return filesRows.Count;
        }

        private List<DataRow> GetFiles()
        {
            List<DataRow> filesRows = new List<DataRow>();
            foreach (DataRow dr in source.Rows)
            {
                if (!string.IsNullOrEmpty(dr["TempFile"].ToString()))
                {
                    filesRows.Add(dr);
                }
            }

            return filesRows;
        }

        private static void SetExpresion()
        {
            if (source.Columns.Contains("NodeDisplayRule"))
            {
                DataColumn dc = source.Columns["NodeDisplayRule"];
                dc.Expression = PartConfigureLogic.NodeDisplayRuleStr;
            }

            if (source.Columns.Contains("BindRule"))
            {
                DataColumn dc = source.Columns["BindRule"];
                dc.Expression = PartConfigureLogic.BindRuleStr;
            }
        }


        /// <summary>
        /// Get part data form sourceDataTable
        /// </summary>
        /// <param name="partEid"></param>
        /// <returns></returns>
        private static DataRow GetPart(string partId)
        {
            foreach (DataRow dr in source.Rows)
            {
                if (dr["TempKey"].ToString() == partId && !string.IsNullOrEmpty(dr["PartType"].ToString()))
                {
                    return dr;
                }
            }
            return null;
        }

        
                /// <summary>
        /// Binding part attriute into file object
        /// </summary>
        /// <param name="dr"></param>
        private static void BindingToPart(DataRow dr)
        {
            foreach (DataRow drTemp in source.Rows)
            {
                if (dr.Table.Columns.Contains("FileName"))
                {
                    if (dr["FileName"].ToString() == drTemp["BindRule"].ToString())
                    {
                        dr["TempKey"] = drTemp["TempKey"];
                        BindAttribute(dr);
                    }
                }
            }
        }

        /// <summary>
        /// Set attribute value to file object from part object
        /// in order to share part infos for file object
        /// </summary>
        /// <param name="drFile"></param>
        private static void BindAttribute(DataRow drFile)
        {
            DataRow dr = GetPart(drFile["TempKey"].ToString());
            if (dr != null)
            {
                int x = dr.Table.Columns.Count;
                for (int i = 12; i < x; i++)
                {
                        drFile[i] = dr[i];
                }
            }
        }

        private static DataTable source = new DataTable();
        private static DataTable PartTable = new DataTable();

        #endregion

    }
}
