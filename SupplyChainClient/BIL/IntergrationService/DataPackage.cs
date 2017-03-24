using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using com.think3.PLM.Integration.Client;
using com.think3.PLM.Integration.DataTransfer;
using com.think3.PLM.Integration.DataTransferXml;
using System.Collections;

namespace ImportIntegration
{
    public class DataPackage : ClientIntegrationClass
    {
        #region ctor
        public DataPackage()
        {
        }

        public DataPackage(object app)
        {
            editorApp = app;
        }

        public DataPackage(object app, string slanguage)
        {
            editorApp = app;
            this.language = slanguage;
        }
        #endregion

        #region public
        public bool bOnlySaveFile = false;

        public void SetSourceDataTable(DataTable table)
        {
            sourceDataTable = table;
            lstFiles.Clear();
        }
        public void SetType(string parttype, string filetype, string bomtype, string filestrutype, string parttofiletype)
        {
            sPartType = parttype;
            sFileType = filetype;
            sBOMType = bomtype;
            sFileStrType = filestrutype;
            sPartToFileType = parttofiletype;
        }
        #endregion

        #region private
        private object editorApp;
        private const string ENVIRONMENT = "BATCHIMPORT";
        private const string LANGUAGE = "eng";
        private string language = "en";
        private string sPartType = "PART";
        private string sBOMType = "PARTEBOM";
        private string sFileType = "DOCUMENT";
        private string sFileStrType = "FILESTRUCTURE";
        private string sPartToFileType = "PARTTOFILELINK";
        private string sManageType = "CHANGED";
        private DataTable sourceDataTable = new DataTable();
        private List<string> lstFiles = new List<string>();
        private List<string> lstCurrentPartFiles = new List<string>();

        /// <summary>
        /// Get child part 
        /// </summary>
        /// <param name="strParentID"></param>
        /// <param name="level"></param>
        /// <returns>child part ids</returns>
        private string[] GetReferenceByParent(string strParentID, SearchLevel level)
        {
            List<string> childids = new List<string>();
            List<string> tempids = new List<string>();

            int nCount = sourceDataTable.Rows.Count;
            for (int j = 0; j < nCount; j++)
            {
                if (sourceDataTable.Rows[j]["ManageType"].ToString() == "Unchanged")
                {
                    tempids.Add(sourceDataTable.Rows[j]["TempKey"].ToString());
                }

            }
            for (int i = 0; i < nCount; i++)
            {
                if (level == SearchLevel.OneBelow)
                {
                    if (sourceDataTable.Rows[i]["TempParent"].ToString() == strParentID && !String.IsNullOrEmpty(sourceDataTable.Rows[i]["TempKey"].ToString()))// && !tempids.Contains(strParentID)
                    {
                        string tempKeyStr = sourceDataTable.Rows[i]["TempKey"].ToString();
                        if (!childids.Contains(tempKeyStr))
                            childids.Add(tempKeyStr);
                    }
                }
                else if (level == SearchLevel.AllBelow)
                {
                    if (!String.IsNullOrEmpty(sourceDataTable.Rows[i]["TempKey"].ToString()) && !tempids.Contains(sourceDataTable.Rows[i]["TempParent"].ToString()))
                    {
                        childids.Add(sourceDataTable.Rows[i]["TempKey"].ToString());
                    }
                }
            }
            return childids.ToArray();
        }
        #endregion

        #region implement EditorIntegrationBase

        public override void FilesToUpload(Data data, StringList ids)
        {
            //Log.WriteIn();

            string pwsPath = base.plmMainObj.GetPrivateWorkspacePath().Replace("/", @"\");
            Log.Write(ClientLogger.LogType.Debug, "Private workspace path is: " + pwsPath);

            List<string> nonPwsFiles = new List<string>();
            Dictionary<string, string> uniqueFiles = new Dictionary<string, string>();
            FileObj fObj = null;

            for (int i = 0; i < ids.Count; i++)
            {
                if (string.IsNullOrEmpty(ids[i]))
                    continue;

                string partID = ids[i];

                lstCurrentPartFiles.Clear();
                lstCurrentPartFiles = GetAllFiles(partID);

                #region build file object
                foreach (string path in lstCurrentPartFiles)
                {
                    // Check if the files is in pws path
                    //string path = GetFileName(partID);
                    if (String.IsNullOrEmpty(path))
                        continue;
                    if (System.IO.Path.IsPathRooted(path) == false || path.Replace("/", @"\").StartsWith(pwsPath) == false)// || path.Replace("/", @"\").StartsWith(pwsPath) == false
                    {
                        nonPwsFiles.Add(path);
                        Log.Write(ClientLogger.LogType.Error, "文件“" + path + "”不在个人工作区“" + pwsPath + "”下.");
                    }

                    if (System.IO.Path.IsPathRooted(path) == false || uniqueFiles.ContainsKey(path))
                        continue;

                    // Populate unique dictionary to avoid duplicate files
                    uniqueFiles.Add(path, partID);

                    //Dictionary<string, string> param = new Dictionary<string,string>();
                    //param.Add("FILENAME", path);
                    //Progressbar.SetProgress( 20 + (int)( i * 20 / ids.Count ), "ADDFILE", param );
                    //End Check if the files is in pws path

                    GetObjectTypeParamenters(partID, path);
                    fObj = data.AddFile(path, path);
                    fObj.ObjectType = sFileType;

                    // Gets which infos have to read as file data.
                    Info[] fileInfos = CloneInfos(sFileType);

                    //fill
                    ReadFileProperties(fileInfos, path, "", path);
                    //fill into data with file type entity
                    SetInfos(fObj, fileInfos);
                    //fill entity into data
                    SetEntityIDs(fObj);


                    #region Build File reference

                    // Skip children of the file which doesnt have permissions to update
                    if (!string.IsNullOrEmpty(path))
                    {
                        // Get the references one level below for the file and create link
                        string[] subIds = new string[0];
                        GetReferences(partID, SearchLevel.OneBelow, ref subIds);
                        if (subIds.Length <= 0)
                            continue;

                        Dictionary<string, string> uniqueSubFiles = new Dictionary<string, string>();

                        // Check if parent is a drawing file
                        bool parentIsLayout = IsLayout(partID);
                        if (sPartToFileType == "PARTTOFILELINK")
                        {
                            // We dont have to search recursively as 
                            // children are handled in the same loop                
                            for (int j = 0; j < subIds.Length; j++)
                            {
                                string subPartID = subIds[j];
                                // Gets only xRef components
                                string subPath = GetFileName(subPartID);
                                if (String.IsNullOrEmpty(subPath) || uniqueSubFiles.ContainsKey(subPath))
                                    continue;

                                // Check if the files is in pws path
                                if (!subPath.Replace("/", @"\").StartsWith(pwsPath))
                                {
                                    nonPwsFiles.Add(subPath);
                                    Log.Write(ClientLogger.LogType.Error, "File is not in PWS location: " + subPath);
                                }

                                // Populate unique dictionary to avoid duplicate files
                                uniqueSubFiles.Add(subPath, subPartID);

                                // create DRAWINGTOMODELLINK if parent is a drawing
                                if (parentIsLayout)
                                {
                                    // Reference link between drawing file and component files("DRAWINGTOMODELLINK")
                                    Reference r = fObj.AddReferenceToSource(subPath);
                                    r.ObjectType = "DRAWINGTOMODELLINK";
                                }
                                else
                                {
                                    GetObjectTypeParamenters(subPartID, subPath);
                                    if (sPartToFileType == "PARTTOFILELINK")
                                    {
                                        Reference r = fObj.AddReferenceToSource(subPath);
                                        r.ObjectType = sFileStrType;
                                    }
                                }

                            }
                        }
                    #endregion


                        string errMsg = "";
                        bool canCheckIn = plmOF.IsCheckOutByMe(path) || plmOF.CanCheckOut(path, out errMsg);
                        if (!canCheckIn)
                            continue;
                    }

                }
                #endregion


                #region check file path
                // If files are not in private workspace path show an error.
                if (nonPwsFiles.Count > 0)
                {
                    string errMsg = "The following files are not in private workspace path: ";
                    Log.Write(ClientLogger.LogType.Debug, errMsg + nonPwsFiles.ToString());
                    if (ClientMessages.ContainsKey("FILESNOTINPWS"))
                    {
                        //errMsg = ClientMessages["FILESNOTINPWS"];
                        errMsg = "以下文件不在个人工作区“" + pwsPath + "”路径下：";
                        
                        if (!System.IO.Directory.Exists(pwsPath))
                        {
                            System.IO.Directory.CreateDirectory(pwsPath);
                        }
                        //System.Diagnostics.Process.Start(pwsPath.Trim());
                    }


                    // Concatenate all the missing files
                    for (int x = 0; x < nonPwsFiles.Count; x++)
                        errMsg = errMsg + "\n" + nonPwsFiles[x];


                    throw new Exception(errMsg);
                }
                #endregion

            }

            //Log.WriteOut();
        }

        public override void PartsToUpload(Data data, StringList ids)
        {
            //Log.WriteIn();

            data.Environment = Environment;
            Info[] infos = null;
            Info info = null;
            List<string> partCurrent = new List<string>();

            for (int i = 0; i < ids.Count; i++)
            {
                #region Build Part object
                string partID = ids[i];
                if (partID == "")
                    continue;

                if (!partCurrent.Contains(partID))
                    partCurrent.Add(partID);
                else
                    continue;


                GetPartObjectTypeParamenters(partID);
                infos = new Info[1];

                infos[0] = CloneCustomInfos(sPartType);

                base.ReadProperties(infos, partID, "");
                // If custom name info is not set skip it
                if (infos[0].Value == "")
                    continue;
                //}


                /*********************** set part infos *************************  */
                // Add part entity
                EntityObj partObj = data.AddEntity(partID, partID);
                string managetype = GetManageType(partID);

                partObj.State = managetype;
                partObj.ObjectType = sPartType;

                //get all infos for current part                
                Info[] partInfos = CloneInfos(sPartType);

                //read value for infos
                base.ReadProperties(partInfos, partID, "");

                base.SetInfos(partObj, partInfos);
                SetEntityIDs(partObj);
                /*********************** End set part infos *************************  */
                #endregion

                #region Add Part and part reference
                // Create "PARTEBOM"
                // handle for bom data from client
                // We dont have to search recursively as 
                // children are handled in the same loop
                // Get the references one level below for the part and create link
                if (!bOnlySaveFile)
                {
                    string[] subIds = new string[0];
                    GetReferences(partID, SearchLevel.OneBelow, ref subIds);
                    for (int j = 0; j < subIds.Length; j++)
                    {
                        string subPartID = subIds[j];

                        Info[] bomInfos = CloneInfos(sBOMType);
                        ReadProperties(bomInfos, subPartID, partID);

                        Reference partBomRef = partObj.AddReference(Environment, subPartID, subPartID);
                        partBomRef.ObjectType = sBOMType;
                        SetInfos(partBomRef, bomInfos);
                        base.SetLinkIDs(partBomRef);
                    }
                }
                #endregion
            }

            #region Add Part and part reference
            partCurrent.Clear();
            for (int i = 0; i < ids.Count; i++)
            {

                string partID = ids[i];
                if (partID == "")
                    continue;

                if (!partCurrent.Contains(partID))
                    partCurrent.Add(partID);
                else
                    continue;

                GetPartObjectTypeParamenters(partID);
                infos = new Info[1];
                infos[0] = CloneCustomInfos(sPartType);
                base.ReadProperties(infos, partID, "");
                // If custom name info is not set skip it
                if (infos[0].Value == "")
                    continue;
                //}

                #region Add Part and file reference
                List<string> lst = GetAllFiles(partID);
                if (lst.Count > 0)
                {
                    for (int k = 0; k < lst.Count; k++)
                    {
                        /*********************** set part infos *************************  */
                        // Add part entity
                        EntityObj partObj = data.AddEntity(partID, partID);
                        string managetype = GetManageType(partID);

                        partObj.State = "Changed";
                        partObj.ObjectType = sPartType;

                        //get all infos for current part                
                        Info[] partInfos = CloneInfos(sPartType);

                        //read value for infos
                        base.ReadProperties(partInfos, partID, "");

                        base.SetInfos(partObj, partInfos);
                        SetEntityIDs(partObj);
                        /*********************** End set part infos *************************  */


                        if (string.IsNullOrEmpty(lst[k]))
                            continue;
                        partObj.Source = lst[k];
                        Reference link = partObj.AddReferenceToSource(lst[k]);
                        link.ObjectType = GetPartToFileType(partID, lst[k]);
                    }
                }
                #endregion
            }

            #endregion

            PartConfigureLogic.SaveData(data.ToString());
            //Log.WriteOut();
        }


        /// <summary>
        /// Read the custom properties from the file and populate into Info
        /// </summary>
        /// <param name="info">Data holder</param>
        /// <param name="id">Current file ID</param>
        /// <param name="parentId">Reference file Id. Used by custom link types, else neglected.</param>
        public override void ReadProperty(IInfo info, string id, string parentId)
        {
            //Log.WriteIn();
            if (String.IsNullOrEmpty(id))
                return;
            string strTempKey = null, strTempParent = null;
            for (int i = 0; i < sourceDataTable.Rows.Count; i++)
            {
                //Only handle part
                if (!string.IsNullOrEmpty(sourceDataTable.Rows[i]["PartType"].ToString()) || sourceDataTable.Rows[i]["TempKey"].ToString() != sourceDataTable.Rows[i]["TempFile"].ToString())
                {
                    strTempKey = sourceDataTable.Rows[i]["TempKey"].ToString();
                    strTempParent = sourceDataTable.Rows[i]["TempParent"].ToString();
                    if ((strTempKey == id && strTempParent == parentId) || (strTempKey == id && parentId == ""))
                    {
                        if (sourceDataTable.Columns.Contains(info.PrpName))
                        {
                            if (!string.IsNullOrEmpty(sourceDataTable.Rows[i][info.PrpName].ToString()))
                            {
                                info.Value = sourceDataTable.Rows[i][info.PrpName].ToString();
                                break;
                            }
                        }
                        else if (sourceDataTable.Columns.Contains(info.Name) && info.Name.ToUpper() == "PRODUCTID")
                        {
                            info.Value = sourceDataTable.Rows[i]["ProductID"].ToString();
                            break;
                        }
                    }
                }
            }
            //Log.WriteOut();
        }

        public override string GetFileName(string currID)
        {
            for (int i = 0; i < sourceDataTable.Rows.Count; i++)
            {
                if (sourceDataTable.Rows[i]["TempKey"].ToString() == currID && !string.IsNullOrEmpty(sourceDataTable.Rows[i]["TempFile"].ToString()))
                {
                    if (!lstFiles.Contains(sourceDataTable.Rows[i]["TempFile"].ToString()))
                    {
                        lstFiles.Add(sourceDataTable.Rows[i]["TempFile"].ToString());
                    }
                    return sourceDataTable.Rows[i]["TempFile"].ToString();
                }
            }
            return "";
        }


        /// <summary>
        /// Get child object
        /// </summary>
        /// <param name="currID"></param>
        /// <param name="level"></param>
        /// <param name="allIds"></param>
        public override void GetReferences(string currID, SearchLevel level, ref string[] allIds)
        {
            allIds = GetReferenceByParent(currID, level);
        }
        public override string Language
        {
            get
            {
                return language;
            }
        }

        public override void PostInit()
        {
            //Log.WriteIn();

            base.PartType = sPartType;
            base.FileType = sFileType;
            base.PartBOMType = sBOMType;
            base.FileStructureType = sFileStrType;
            base.PartToFileLink = sPartToFileType;

            // Overwrite values with values from config file if specified
            if (integrProps.ContainsKey("PartType"))
                base.PartType = integrProps["PartType"].ToString();
            if (integrProps.ContainsKey("FileType"))
                base.FileType = integrProps["FileType"].ToString();
            if (integrProps.ContainsKey("PartBOMType"))
                base.PartBOMType = integrProps["PartBOMType"].ToString();
            if (integrProps.ContainsKey("FileStructureType"))
                base.FileStructureType = integrProps["FileStructureType"].ToString();
            if (integrProps.ContainsKey("PartToFileLink"))
                base.PartToFileLink = integrProps["PartToFileLink"].ToString();
            if (integrProps.ContainsKey("SaveIntoArchiveASync"))
                base.SaveIntoArchiveASync = Convert.ToBoolean(integrProps["SaveIntoArchiveASync"]);

            // Add default entity/ebom class types
            if (integrProps.ContainsKey("PartTypeClass") == false)
                integrProps.Add("PartTypeClass", "TMM_COMPONENT");
            if (integrProps.ContainsKey("PartTypeClass") == false)
                integrProps.Add("BomTypeClass", "TMM_RELATION");

            //Log.WriteOut();
        }
        public override void PromptMessage(MessageType mt, string caption, string message)
        {
            if (message == "No files to upload")
                return;
            if (string.IsNullOrEmpty(message))
                return;
            //if (message == "Execute Success.")
            //{
            //    if (this.language == "zh" || this.language == "zhs")
            //        message = "注销成功.";
            //}
            message.Replace("<br>", "");
            System.Windows.Forms.MessageBox.Show(message, caption);
        }

        public override void MarkAsHaveToSave(Data data)
        {

            //Log.WriteIn();

            // Set the files which are modified as have to save.
            // This is useful to optimize the Save operation by avoiding
            // unmodified entites/files.
            FileObj[] selIds = data.Files;
            int modifiedFile = 0;
            string invalidPermission = "";
            for (int i = 0; i < selIds.Length; i++)
            {
                string fileName = selIds[i].FullName;
                string errMsg = null;
                bool canCheckIn = plmOF.IsCheckOutByMe(fileName) || plmOF.CanCheckOut(fileName, out errMsg);
                if (!string.IsNullOrEmpty(errMsg))
                    invalidPermission += errMsg + "\n";

                //if (canCheckIn && IsFileModifiedEx(fileName))
                {
                    selIds[i].State = "Changed";
                    modifiedFile++;
                }
                //else
                //{
                //    selIds[i].State = "Unchanged";
                //}
            }

            //Log.WriteOut();
        }

        /// <summary>
        /// Overridden environment property.
        /// Each integration will return their corresponding environment name
        /// </summary>
        public override string Environment
        {
            get
            {
                return ENVIRONMENT;
            }
        }

        /// <summary>
        /// Cast the application object to Microsoft.Office.Interop.Excel.Application object
        /// </summary>
        public override Object Application
        {
            get
            {
                return editorApp;
            }
        }

        private bool IsFileModifiedEx(string fileName)
        {
            bool dirty = IsFileDirty(fileName);
            long lastModified = GetFileLastWriteTimeUtc(fileName);
            DateTime dt = DateTime.FromFileTimeUtc(lastModified);

            DownloadState ds = PLMMain.GetDownloadState();
            bool modified = ds == null ? (true) : (ds.IsFileModified(fileName, dt));

            return (dirty || modified);
        }

        /// <summary>
        /// Get files of current part
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        private List<string> GetAllFiles(string partId)
        {
            List<string> lst = new List<string>();

            for (int i = 0; i < sourceDataTable.Rows.Count; i++)
            {
                if (sourceDataTable.Rows[i]["TempKey"].ToString() == partId)
                {
                    if (!string.IsNullOrEmpty(sourceDataTable.Rows[i]["TempFile"].ToString()))
                        lst.Add(sourceDataTable.Rows[i]["TempFile"].ToString());
                }
            }
            return lst;
        }

        private List<string> GetAllParts()
        {
            List<string> lst = new List<string>();

            for (int i = 0; i < sourceDataTable.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(sourceDataTable.Rows[i]["TempKey"].ToString()) && !string.IsNullOrEmpty(sourceDataTable.Rows[i]["PartType"].ToString()))
                {
                    lst.Add(sourceDataTable.Rows[i]["TempFile"].ToString());
                }
            }
            return lst;
        }

        /// <summary>
        /// Get Type of current part and file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetPartToFileType(string partId, string fileName)
        {
            for (int i = 0; i < sourceDataTable.Rows.Count; i++)
            {
                if (sourceDataTable.Rows[i]["TempFile"].ToString() == fileName && sourceDataTable.Rows[i]["TempKey"].ToString() == partId)
                {
                    return sourceDataTable.Rows[i]["PartToFileType"].ToString();
                }
            }
            return null;
        }

        private void ReadFileProperties(Info[] infos, string id, string parentid, string filename)
        {
            //Log.WriteIn();

            for (int i = 0; i < infos.Length; i++)
            {
                // Each of the editor integration will implement this pure abstrace method
                ReadFileProperty(infos[i], id, parentid, filename);
            }

            //Log.WriteOut();
        }
        private void ReadFileProperty(IInfo info, string id, string parentId, string filename)
        {
            //Log.WriteIn();
            if (String.IsNullOrEmpty(id))
                return;
            for (int i = 0; i < sourceDataTable.Rows.Count; i++)
            {
                if (sourceDataTable.Rows[i]["TempFile"].ToString() == id)//TempKey
                {
                    if (sourceDataTable.Columns.Contains(info.PrpName))
                    {
                        info.Value = sourceDataTable.Rows[i][info.PrpName].ToString();
                        break;
                    }
                }
            }
            //Log.WriteOut();
        }

        private string GetManageType(string currID)
        {
            for (int i = 0; i < sourceDataTable.Rows.Count; i++)
            {
                if (sourceDataTable.Rows[i]["TempKey"].ToString() == currID)
                {
                    return sourceDataTable.Rows[i]["ManageType"].ToString();
                }
            }
            return "";
        }

        /// <summary>
        /// Get ObjectType Paramenters
        /// </summary>
        /// <param name="partId"></param>
        private void GetObjectTypeParamenters(string partId, string fileId)
        {
            for (int i = 0; i < sourceDataTable.Rows.Count; i++)
            {
                if (sourceDataTable.Rows[i]["TempKey"].ToString() == partId && sourceDataTable.Rows[i]["TempFile"].ToString() == fileId)
                {
                    sPartType = sourceDataTable.Rows[i]["PartType"].ToString() == "" ? sPartType : sourceDataTable.Rows[i]["PartType"].ToString();
                    sFileType = sourceDataTable.Rows[i]["FileType"].ToString() == "" ? sFileType : sourceDataTable.Rows[i]["FileType"].ToString();
                    sBOMType = sourceDataTable.Rows[i]["BomType"].ToString() == "" ? sBOMType : sourceDataTable.Rows[i]["BomType"].ToString();
                    sFileStrType = sourceDataTable.Rows[i]["FileStructureType"].ToString() == "" ? sFileStrType : sourceDataTable.Rows[i]["FileStructureType"].ToString();
                    sPartToFileType = sourceDataTable.Rows[i]["PartToFileType"].ToString() == "" ? sPartToFileType : sourceDataTable.Rows[i]["PartToFileType"].ToString();
                    sManageType = sourceDataTable.Rows[i]["ManageType"].ToString() == "" ? sManageType : sourceDataTable.Rows[i]["ManageType"].ToString();
                    break;
                }
            }
        }

        private void GetPartObjectTypeParamenters(string partId)
        {
            for (int i = 0; i < sourceDataTable.Rows.Count; i++)
            {
                if (sourceDataTable.Rows[i]["TempKey"].ToString() == partId && !string.IsNullOrEmpty(sourceDataTable.Rows[i]["PartType"].ToString()))
                {
                    sPartType = sourceDataTable.Rows[i]["PartType"].ToString() == "" ? sPartType : sourceDataTable.Rows[i]["PartType"].ToString();
                    sFileType = sourceDataTable.Rows[i]["FileType"].ToString() == "" ? sFileType : sourceDataTable.Rows[i]["FileType"].ToString();
                    sBOMType = sourceDataTable.Rows[i]["BomType"].ToString() == "" ? sBOMType : sourceDataTable.Rows[i]["BomType"].ToString();
                    sFileStrType = sourceDataTable.Rows[i]["FileStructureType"].ToString() == "" ? sFileStrType : sourceDataTable.Rows[i]["FileStructureType"].ToString();
                    sPartToFileType = sourceDataTable.Rows[i]["PartToFileType"].ToString() == "" ? sPartToFileType : sourceDataTable.Rows[i]["PartToFileType"].ToString();
                    sManageType = sourceDataTable.Rows[i]["ManageType"].ToString() == "" ? sManageType : sourceDataTable.Rows[i]["ManageType"].ToString();
                    break;
                }
            }
        }

        /// <summary>
        /// Cache infos of eneity type 
        /// </summary>
        /// <param name="sPartType">Object type</param>
        /// <returns>Infos</returns>
        private Info[] CloneInfos(string sPartType)
        {
            if (!CacheInfos.Contains(sPartType))
            {
                Info[] partInfosTemp = plmOF.GetInfos(sPartType, Environment);
                CacheInfos.Add(sPartType, partInfosTemp);
            }

            int intInfos = ((Info[])CacheInfos[sPartType]).Length;
            Info[] partInfos = new Info[intInfos];
            for (int x = 0; x < intInfos; x++)
            {
                partInfos[x] = new Info();
                partInfos[x].PrpName = ((Info[])CacheInfos[sPartType])[x].PrpName;
                partInfos[x].Name = ((Info[])CacheInfos[sPartType])[x].Name;
                partInfos[x].Value = ((Info[])CacheInfos[sPartType])[x].Value;
            }
            return partInfos;
        }

        private Info CloneCustomInfos(string sPartType)
        {
            if (!CacheCustomNameInfo.Contains(sPartType))
            {
                Info partInfosTemp = plmOF.GetCustomNameInfo(sPartType, Environment);
                CacheCustomNameInfo.Add(sPartType, partInfosTemp);
            }

            Info partCustomInfo = new Info();
            partCustomInfo.PrpName = ((Info)CacheCustomNameInfo[sPartType]).PrpName;
            partCustomInfo.Name = ((Info)CacheCustomNameInfo[sPartType]).Name;
            partCustomInfo.Value = ((Info)CacheCustomNameInfo[sPartType]).Value;

            return partCustomInfo;
        }
        private Hashtable CacheInfos = new Hashtable();
        private Hashtable CacheCustomNameInfo = new Hashtable();



        #endregion
    }
}
