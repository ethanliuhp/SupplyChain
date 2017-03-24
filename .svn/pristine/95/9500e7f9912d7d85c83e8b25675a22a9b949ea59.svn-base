using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Security.Cryptography.X509Certificates;

/// <summary>
///MyPolicy 的摘要说明
/// </summary>
public class MyPolicy : ICertificatePolicy
{
    public bool CheckValidationResult(
          ServicePoint srvPoint
        , X509Certificate certificate
        , WebRequest request
        , int certificateProblem)
    {

        //Return True to force the certificate to be accepted.
        return true;

    } // end CheckValidationResult
}
