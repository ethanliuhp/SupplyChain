using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonClass
{
    public class FileUploadUtil
    {
        public static bool UploadFiles(List<string> files, string dirName)
        {
            var serverUrl = ConfigurationManager.AppSettings["RankingSysUrl"];
            if (string.IsNullOrEmpty(serverUrl))
            {
                return false;
            }

            try
            {
                var endIndex = serverUrl.LastIndexOf('/');
                var uploadUrl = string.Concat(serverUrl.Substring(0, endIndex), "/UploadFiles?dn=", dirName);

                using(System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    foreach (var file in files)
                    {
                        if (!System.IO.File.Exists(file))
                        {
                            continue;
                        }

                        //upload to 3080
                        webClient.UploadFile(uploadUrl, "POST", file);

                        //upload to 56
                        webClient.UploadFile(uploadUrl.Replace("3080", "56"), "POST", file);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
