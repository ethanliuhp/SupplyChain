using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using com.think3.PLM.Infrastructure.ImageHandler;
using com.think3.PLM.Integration;
using com.think3.PLM.Integration.Client;
using com.think3.PLM.Integration.DataTransfer;
using com.think3.PLM.Integration.DataTransferXml;
using com.think3.PLM.Integration.Client.WS;

namespace ImportIntegration
{
    public class IntergrationFrameWork : PLMIntegrationClass
    {
        #region ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorPLMIntegration"/> class.
        /// </summary>
        public IntergrationFrameWork()
            : base()
        {
        }
        public IntergrationFrameWork(bool isEnabledAutoLogin, string userName, string orgId)
            : base()
        {
            PLMMain.IsEnabledAutoLogin = isEnabledAutoLogin;
            PLMMain.UserName = userName;
            PLMMain.OrgId = orgId;
        }
        #endregion

        #region public
        public override void Save()
        {
            Log.WriteIn();

            SaveSession saveSession = null;
            try
            {
                // Get the current open file id.
                string rootID = cadIntegrObj.GetCurrentID();

                Data data = base.plmMain.CreateDataContainer();

                //cadIntegrObj.GatherInfos(data);
                cadIntegrObj.GatherInfosByFile(data);
                //int nObj = data.Files.Length + data.Entities.Length;


                // create a new save session and set data to be saved
                saveSession = base.plmMain.CreateSaveSession();
                saveSession.SetData(data);

                // Checks the gathered data
                data = saveSession.Check();

                // Writes the properties to file (only for new entities or new versions)
                WriteToFile(data);

                // Save files
                SaveFiles(data);

                // Save images
                cadIntegrObj.SaveImages(data);

                // Save only files that need to be uploaded
                // saveSession.Upload();

                // Commits the save operation.
                // saveSession.Commit();

                // Save the files in an asynchronous mode
                if (cadIntegrObj.SaveIntoArchiveASync)
                    saveSession.SaveAsync();
                else
                    saveSession.Save();

                // ie=RemoveImages(data)
                // RemoveImages( data );

                // Set the files as changed
                // Get the references one level below for the file and create link
                //string partID = cadIntegrObj.GetCurrentID();

                //string[] subIds = new string[0];
                //cadIntegrObj.GetReferences(partID, SearchLevel.OneBelow, ref subIds);

                DownloadState ds = PLMMain.GetDownloadState();
                FileObj[] files = data.Files;
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].State == "Changed")
                    {
                        long lastUpdate = cadIntegrObj.GetFileLastWriteTimeUtc(files[i].FullName);
                        DateTime dt = DateTime.FromFileTimeUtc(lastUpdate);
                        ds.AddOrUpdate(files[i].FullName, dt, null);
                    }
                    //cadIntegrObj.ResetFileModifiedDate( files[ i ].FullName );
                }
                ds.Save("");
            }
            catch (System.Exception se)
            {
                if (saveSession != null)
                    saveSession.Rollback();

                cadIntegrObj.PromptMessage(MessageType.Error, this.MessageCaption, se.Message);
                base.Log.WriteOut();
                return;
            }

            base.Log.WriteOut();
            string Text = BatchImportLocalize.GetLocalizedText("FinishSaveText");
            string Caption = BatchImportLocalize.GetLocalizedText("FinishSaveCaption");
            MessageBox.Show(Text, Caption);
        }

        public bool UpdateFilePath(string fileID)
        {
            Query query = plmMain.CreateQuery();
            Data dd1 = query.GetObjectInfosByID(fileID, null);
            //Info i= dd1.File.GetInfo("");
            //dd1.File.SetInfos();
            return true;
        }

        public IInfos SelectPart(string filter)
        {
            try
            {
                Data data = null;
                data = plmQuery.SelectEntities("PART", filter, "BATCHIMPORT");
                if (data == null)
                    return null;
                else if (data.Entities.Length == 0)
                    return null;
                else
                    return (IInfos)data.Entities[0];
            }
            catch (System.Exception se)
            {
                cadIntegrObj.PromptMessage(MessageType.Error, this.MessageCaption, se.Message);
            }
            return null;
        }

        public EntityObj[] SelectChildParts(string id)
        {
            try
            {
                Data data = null;
                data = plmQuery.SelectChilds(id, "PARTEBOM", "BATCHIMPORT");
                if (data == null)
                    return null;

                return data.Entities;
            }
            catch (System.Exception se)
            {
                cadIntegrObj.PromptMessage(MessageType.Error, this.MessageCaption, se.Message);
            }
            return null;
        }
        #endregion

        #region 文件操作 2012-5-10 刘友民
        public FilesToTransfer SaveFiles(ref string message)
        {
            Log.WriteIn();

            FileObj[] files = null;
            FilesToTransfer fileTransfer = null;

            SaveSession saveSession = null;
            try
            {
                // Get the current open file id.
                string rootID = cadIntegrObj.GetCurrentID();

                Data data = base.plmMain.CreateDataContainer();

                //cadIntegrObj.GatherInfos(data);
                cadIntegrObj.GatherInfosByFile(data);
                //int nObj = data.Files.Length + data.Entities.Length;


                // create a new save session and set data to be saved
                saveSession = base.plmMain.CreateSaveSession();
                saveSession.SetData(data);

                // Checks the gathered data
                data = saveSession.Check();

                // Writes the properties to file (only for new entities or new versions)
                WriteToFile(data);

                // Save files
                SaveFiles(data);

                // Save images
                cadIntegrObj.SaveImages(data);

                // Save only files that need to be uploaded
                // saveSession.Upload();

                // Commits the save operation.
                // saveSession.Commit();

                // Save the files in an asynchronous mode
                if (cadIntegrObj.SaveIntoArchiveASync)
                    fileTransfer = saveSession.SaveFilesAsync();
                else
                    saveSession.Save();

                // ie=RemoveImages(data)
                // RemoveImages( data );

                // Set the files as changed
                // Get the references one level below for the file and create link
                //string partID = cadIntegrObj.GetCurrentID();

                //string[] subIds = new string[0];
                //cadIntegrObj.GetReferences(partID, SearchLevel.OneBelow, ref subIds);

                DownloadState ds = PLMMain.GetDownloadState();
                files = data.Files;
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].State == "Changed")
                    {
                        long lastUpdate = cadIntegrObj.GetFileLastWriteTimeUtc(files[i].FullName);
                        DateTime dt = DateTime.FromFileTimeUtc(lastUpdate);
                        ds.AddOrUpdate(files[i].FullName, dt, null);
                    }
                    //cadIntegrObj.ResetFileModifiedDate( files[ i ].FullName );
                }
                ds.Save("");
            }
            catch (System.Exception se)
            {
                if (saveSession != null)
                    saveSession.Rollback();

                //cadIntegrObj.PromptMessage(MessageType.Error, this.MessageCaption, se.Message);
                base.Log.WriteOut();

                message = se.Message;

                if (message.IndexOf("不在个人工作区") > -1)
                {
                    string pwsPath = GetPrivateWorkspacePath();
                    if (!System.IO.Directory.Exists(pwsPath))
                    {
                        System.IO.Directory.CreateDirectory(pwsPath);
                    }
                    //System.Diagnostics.Process.Start(pwsPath.Trim());
                }
            }

            base.Log.WriteOut();
            string Text = BatchImportLocalize.GetLocalizedText("FinishSaveText");
            string Caption = BatchImportLocalize.GetLocalizedText("FinishSaveCaption");

            //MessageBox.Show(Text, Caption);

            return fileTransfer;
        }

        public ErrorStack DeleteEntities(string entityTypeName, string[] listId)
        {
            try
            {
                EntityWebMethods ewm = new EntityWebMethods();
                return ewm.DeleteEntities(entityTypeName, listId);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Exception ex1 = ex.InnerException;
                while (ex1 != null)
                {
                    message += ex1.Message;

                    ex1 = ex1.InnerException;
                }

                ErrorStack err = new ErrorStack();
                err.Message = message;

                return err;
            }
        }
        #endregion

        private string MessageCaption
        {
            get
            {
                string strkey = "MESSAGETITLE";
                if (ClientConfiguration.ClientMessages.ContainsKey(strkey))
                    return ClientConfiguration.ClientMessages[strkey];
                return "TD PLM";
            }
        }
    }
}
