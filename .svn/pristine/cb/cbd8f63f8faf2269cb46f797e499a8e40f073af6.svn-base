using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;

namespace IRPServiceModel.Basic
{
    public class MyX509Validator : X509CertificateValidator
    {
        public override void Validate(X509Certificate2 certificate)
        {
            // validate argument
            if (certificate == null)
                throw new ArgumentNullException("X509认证证书为空！");
        }
    }
}
