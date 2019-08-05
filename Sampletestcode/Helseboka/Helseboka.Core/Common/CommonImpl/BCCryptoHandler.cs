using System;
using System.Text;
using Helseboka.Core.Common.Interfaces;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace Helseboka.Core.Common.CommonImpl
{
    public class BCCryptoHandler : ICrypto
    {
        private readonly int iterations = 1024;
        private readonly String salt = "Xp~a=YWtH#^F/bpKzkC=,_?*%bVJ%Bjm";
        private readonly int hashByteSize = 128;


        public string Hash(string password)
        {
            try
            {
                var pdb = new Pkcs5S2ParametersGenerator(new Org.BouncyCastle.Crypto.Digests.Sha512Digest());
                pdb.Init(PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()), Encoding.UTF8.GetBytes(salt),
                             iterations);
                var key = (KeyParameter)pdb.GenerateDerivedMacParameters(hashByteSize * 8);
                return Convert.ToBase64String(key.GetKey());
            }
            catch
            {
                return password;
            }
        }

        public bool Verify(string password, string hash)
        {
            var passwordHash = Hash(password);
            return passwordHash.Equals(hash);
        }
    }
}
