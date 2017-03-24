using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;

namespace ImportIntegration
{
    public class BusinessData
    {
        public BusinessData()
        {
            SetExpresion();
        }
        public static DataTable sourceDataTable;

        public static void AddFiles(string strFileName, string strFileType, string strFileStruType, string strFileLinkPartType)
        {
            BusinessDataRow bdr = new BusinessDataRow();
            bdr.FileName = strFileName;
            bdr.FileType = strFileType;
            bdr.FileStructureType = strFileStruType;
            bdr.PartToFileType = strFileLinkPartType;
            entities.Add(bdr);
        }

        public static void AddFiles(DataRow newRow)
        {
            BusinessDataRow bdr = new BusinessDataRow(newRow);
            entities.Add(bdr);
        }


        public static void RemoveFile(string fileId)
        {
            int iCount = entities.Count;
            for (int j = 0; j < iCount; j++)
            {
                if (entities[j].FileName == fileId)
                {
                    entities.RemoveAt(j);
                }
            } 

            //foreach (BusinessDataRow bdr in entities)
            //{
            //    if (bdr.FileName== fileId)
            //    {
            //        entities.Remove(bdr);
            //    }
            //}            //for (int j = 0; j < sourceDataTable.Rows.Count; j++)
            //{
            //    if (sourceDataTable.Rows[j]["TempFile"].ToString() == fileId)
            //    {
            //        sourceDataTable.Rows.RemoveAt(j);
            //    }
            //} 
        }


        public static void LinkPart(string fileId,ICollection selectedNodes)
        {
           // BusinessDataRow bdr = new BusinessDataRow(newRow);

            DataRow drCurrent = sourceDataTable.NewRow();
            drCurrent.ItemArray = GetFile(fileId).ItemArray;
            sourceDataTable.Rows.Remove(GetFile(fileId));

            foreach (TreeNode tn in selectedNodes)
            {
                DataRow dr = sourceDataTable.NewRow();
                dr.ItemArray = drCurrent.ItemArray;
                dr["TempKey"] = tn.Name;
                sourceDataTable.Rows.Add(dr);
            }

        }

        public static void UndoLink(string fileId)
        {
            DataRow drCurrent = GetFile(fileId);
            DataRow drTemp = sourceDataTable.NewRow();
            drTemp.ItemArray =drCurrent.ItemArray;
            RemoveFile(fileId);
            drTemp["TempKey"] ="";
            sourceDataTable.Rows.Add(drTemp);
 
        }


        private static DataRow GetFile(string fileId)
        {
            for (int j = 0; j < sourceDataTable.Rows.Count; j++)
            {
                if (sourceDataTable.Rows[j]["TempFile"].ToString() == fileId)
                {
                    return sourceDataTable.Rows[j];
                }
            }
            return null;
        }

        private void SetExpresion()
        {
            if (sourceDataTable.Columns.Contains("NodeDisplayRule"))
            {
                DataColumn dc = sourceDataTable.Columns["NodeDisplayRule"];
                dc.Expression = PartConfigureLogic.NodeDisplayRuleStr;
            }

            if (sourceDataTable.Columns.Contains("BindRule"))
            {
                DataColumn dc = sourceDataTable.Columns["BindRule"];
                dc.Expression = PartConfigureLogic.BindRuleStr;
            }
        }

        static List<BusinessDataRow> entities = new List<BusinessDataRow>();

    }
}
