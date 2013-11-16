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
            while(keyString.Length % 4 != 0)
                keyString += new char();

            KeyBytes = System.Text.Encoding.UTF8.GetBytes(keyString);
        }

        public List<uint> GetRoundKeys()
        {
            // Rounds = How many 32-bit keys found in the Key string
            int roundCount = KeyBytes.Length / 4;
            List<uint> roundKeys = new List<uint>();

            for(int i = 0; i < roundCount; i++)
            {
                byte[] roundKeyBytes = new byte[4];
                uint roundKey = 0;

                Array.Copy(KeyBytes, i * 4, roundKeyBytes, 0, 4);
                Array.Reverse(roundKeyBytes);

                roundKey = BitConverter.ToUInt32(roundKeyBytes, 0);
                roundKeys.Add(roundKey);
            }
            return roundKeys;
        }
    }
}
