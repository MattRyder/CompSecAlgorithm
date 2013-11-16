using System;
using System.Collections.Generic;

namespace CompSecAlgorithm
{
    /// <summary>
    /// A class used to store and modify the key
    /// </summary>
    public class Key
    {
        /// <summary>
        /// A byte array representation of the key
        /// </summary>
        public byte[] KeyBytes 
        {
            get; set;
        }

        /// <summary>
        /// The key used to encrypt and decrypt the text
        /// </summary>
        /// <param name="keyString">The key to use for this instance</param>
        public Key(string keyString) 
        {
            KeyBytes = System.Text.Encoding.UTF8.GetBytes(keyString);
        }

        /// <summary>
        /// Generates the keys used for each round
        /// </summary>
        /// <returns></returns>
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
