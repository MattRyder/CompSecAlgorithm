using System;
using System.Collections.Generic;

namespace CompSecAlgorithm
{
    /// <summary>
    /// A class used to store and modify the key
    /// </summary>
    public class Key
    {
        // Initial Permutation table, used to permutate the key bytes
        private byte[] IP1 = {
            06, 30, 13, 07, 05, 35, 15, 14,
            12, 18, 03, 38, 09, 10, 22, 25,
            16, 04, 21, 08, 39, 37, 36, 02,
            24, 11, 28, 27, 29, 23, 33, 01,
            32, 17, 31, 00, 26, 34, 20, 19
        };

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
            int i = 0, j = keyString.Length;

            // Expand the key out to the max of 40 bytes
            while(keyString.Length < 40)
                keyString += keyString[i++];

            KeyBytes = System.Text.Encoding.UTF8.GetBytes(keyString);

            // Permutate the keybytes with IP1 byte table
            for(i = 0; i < KeyBytes.Length; i++)
                KeyBytes[i] = KeyBytes[IP1[i]];

            System.Diagnostics.Debug.WriteLine("Post-perm Key: " + System.Text.Encoding.UTF8.GetString(KeyBytes));
        }

        /// <summary>
        /// Produce the keys to be used in the round function
        /// </summary>
        /// <returns>A list of 64-bit keys in ulong format</returns>
        public List<ulong> GetRoundKeys()
        {
            // Rounds = How many 64-bit keys found in the Key string
            int roundCount = KeyBytes.Length / 8;
            List<ulong> roundKeys = new List<ulong>();

            for(int i = 0; i < roundCount; i++)
            {
                byte[] roundKeyBytes = new byte[8];
                ulong roundKey = 0;

                Array.Copy(KeyBytes, i * 8, roundKeyBytes, 0, 8);
                Array.Reverse(roundKeyBytes);

                roundKey = BitConverter.ToUInt64(roundKeyBytes, 0);
                roundKeys.Add(roundKey);
            }
            return roundKeys;
        }
    }
}
