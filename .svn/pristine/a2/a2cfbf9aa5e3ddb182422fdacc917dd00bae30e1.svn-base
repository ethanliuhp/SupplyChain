using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using com.think3.PLM.Localization;


namespace ImportIntegration
{
    class BatchImportLocalize
    {
        #region Public
        public enum LocalContext
        {
            DisplayText,
            DescriptionText,
            MessageText
        }
        public BatchImportLocalize()
        {

        }
        public static string language = "en";
        /// <summary>
        /// Load the dictionary 
        /// </summary>
        public static void Load(string strlanguage)
        {
            language = strlanguage;

            string assemName = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
            string applicationPath = System.IO.Path.GetDirectoryName(assemName);
            string configFolder = Path.Combine(applicationPath, CONFIG_FOLDER);
            if (System.IO.Directory.Exists(configFolder) == false)
                configFolder = Path.Combine(applicationPath, CONFIG_FOLDER2);

            string config = Path.Combine(configFolder, CONFIG_FILE);

            Dictionary.LoadFromFile(config);
            
        }
        /// <summary>
        /// Gets the locaized text displyed on controls 
        /// by searching the "DisplayText" context by default
        /// </summary>
        /// <returns>Localized string which is to be displyed on the control</returns>
        public static string GetLocalizedText( string labelName )
        {
            return GetLocalizedText(BatchImportLocalize.LocalContext.DisplayText, labelName);

        }
        /// <summary>
        /// Gets the localized display string for the given label name
        /// </summary>
        /// <param name="ContextName">Context name enum value from AdminLocalize.LocalContext</param>
        /// <param name="labelName">String value of the Label to be localized</param>
        /// <returns>Localized string</returns>
        public static string GetLocalizedText(BatchImportLocalize.LocalContext ctx, string labelName)
        {
            DictionaryContext context;
            string contextName = ctx.ToString();
            try
            {
                context = Dictionary.GetContext( contextName );
                ContextLabel lbl = context[ labelName ];//"UnChanged"
                if (lbl.Count == 0)
                    return "";
                ContextLocalizedLabel ll = lbl[language];
                return ll.LocalizedString;
            }
            catch ( System.Exception ex )
            {
                //throw ex;
                return "";
            }

        }
#endregion Public

        #region Private
        

        /// <summary>
        /// Xml dictionary file name.
        /// </summary>
        /// 
        private const string CONFIG_FOLDER = @"..\Config";
        private const string CONFIG_FOLDER2 = @"FileUploadConfig";//配置目录2（当第一个目录不存在时使用）
        private const string CONFIG_FILE = "BatchImportDictionary.xml";
        #endregion Private
    }
}
