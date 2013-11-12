using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompSecAlgorithm
{
    class PublicKeyEncryption
    {
        private string _password;

        public string PrivateKey
        {
            get;
            set;
        }

        public string PublicKey
        {
            get;
            set;
        }

        public PublicKeyEncryption(string password = null)
        {
            _password = password;
        }

        public void Encrypt() 
        {

        }


    }
}
