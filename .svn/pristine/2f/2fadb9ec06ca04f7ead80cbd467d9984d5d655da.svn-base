using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ImportIntegration
{
    public class BusinessDataRow
    {
        public BusinessDataRow()
        { }
        public BusinessDataRow(DataRow dr)
        {
            currentRow = dr;
        }
        public string ProductID
        {
            get
            {
                return currentRow["ProductID"].ToString();
            }
            set
            {
                currentRow["ProductID"] = value;
            }
        }
        public string Parent
        {
            get
            {
                return currentRow["TempParent"].ToString();
            }
            set
            {
                currentRow["TempParent"] = value;
            }
        }
        public string PartName
        {
            get
            {
                return currentRow["TempKey"].ToString();
            }
            set
            {
                currentRow["TempKey"] = value;
            }
        }
        public string FileName
        {
            get
            {
                return currentRow["TempFile"].ToString();
            }
            set
            {
                currentRow["TempFile"] = value;
            }
        }
        public string PartType
        {
            get
            {
                return currentRow["PartType"].ToString();
            }
            set
            {
                currentRow["PartType"] = value;
            }
        }
        public string BomType
        {
            get
            {
                return currentRow["BomType"].ToString();
            }
            set
            {
                currentRow["BomType"] = value;
            }
        }
        public string FileType
        {
            get
            {
                return currentRow["FileType"].ToString();
            }
            set
            {
                currentRow["FileType"] = value;
            }
        }
        public string FileStructureType
        {
            get
            {
                return currentRow["FileStructureType"].ToString();
            }
            set
            {
                currentRow["FileStructureType"] = value;
            }
        }
        public string PartToFileType
        {
            get
            {
                return currentRow["PartToFileType"].ToString();
            }
            set
            {
                currentRow["PartToFileType"] = value;
            }
        }
        public string ManageType
        {
            get
            {
                return currentRow["ManageType"].ToString();
            }
            set
            {
                currentRow["ManageType"] = value;
            }
        }
        public string NodeDisplayRule
        {
            get
            {
                return currentRow["NodeDisplayRule"].ToString();
            }
            set
            {
                currentRow["NodeDisplayRule"] = value;
            }
        }
        public string BindRule
        {
            get
            {
                return currentRow["BindRule"].ToString();
            }
            set
            {
                currentRow["BindRule"] = value;
            }
        }

        public DataRow currentRow
        {
            get;
            set;
        }


    }
}
