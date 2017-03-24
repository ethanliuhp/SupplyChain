using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ImportIntegration
{
    public class PartLinkFiles
    {
        /// <summary>
        /// current part id
        /// </summary>
        public string PartId
        {
            get;
            set;
        }

        /// <summary>
        /// Add files
        /// </summary>
        /// <param name="fileName"></param>
        public void Add(string fileName, string Type,string StruvtureType)
        {
            if (!files.Contains(fileName) && !string.IsNullOrEmpty(fileName))
            {
                LinkFile lf = new LinkFile();
                lf.Name = fileName;
                lf.Type = Type;
                lf.StruvtureType = StruvtureType;
                files.Add(fileName, lf);
            }
        }

        public List<LinkFile> GetFiles()
        {
            List<LinkFile> filesTemp = new List<LinkFile>();
            IEnumerator myEnumerator =files.Values.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                filesTemp.Add(myEnumerator.Current as LinkFile);
            }
            return filesTemp;           
        }

        public void Remove(string name)
        {
            files.Remove(name);
 
        }



        public LinkFile GetFile(string name)
        {
            return (LinkFile)files[name];
        }

        private Hashtable files = new Hashtable();
    }

    public class LinkFile
    {
        public string Name
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public string StruvtureType
        {
            get;
            set;
        }
    }

}
