using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himsa.Samples.WebHookEventReciever
{
    public class SignatureValidationUtil
    {
        public static bool ValidateSignature(string body, string signature, string secret)
        {
            var hash = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var computedSignature = Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(body)));
            return computedSignature == signature;
        }
    }
}
