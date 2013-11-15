using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompSecAlgorithm
{
    class Key
    {
        public byte[] KeyBytes 
        {
            get; set;
        }

        public Key(string keyString) 
        {
            KeyBytes = System.Text.Encoding.UTF8.GetBytes(keyString);
        }

        public Key(byte[] keyByteArray) 
        {
            KeyBytes = keyByteArray;
        }

        public List<uint> GetRoundKeys()
        {
            int bytesPerKey = KeyBytes.Length / 4;
            List<uint> roundKeys = new List<uint>();

            for(int i = 0; i < 4; i++)
            {
                byte[] roundKeyBytes = new byte[bytesPerKey];
                uint roundKey = 0;

                Array.Copy(KeyBytes, bytesPerKey * i, roundKeyBytes, 0, bytesPerKey);
                Array.Reverse(roundKeyBytes);
                roundKey = BitConverter.ToUInt32(roundKeyBytes, 0);
                roundKeys.Add(roundKey);
            }

            return roundKeys;
        }
    }
}
