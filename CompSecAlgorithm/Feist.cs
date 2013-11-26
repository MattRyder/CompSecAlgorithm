using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CompSecAlgorithm
{
    /// <summary>
    /// The class used to handle the encryption scheme
    /// </summary>
    public class Feist
    {
        /// <summary>
        /// Size of each block in bytes
        /// </summary>
        public int BlockSize
        {
            get;
            private set;
        }

        /// <summary>
        /// The master passcode to encrypt/decrypt
        /// </summary>
        public string Passcode
        {
            get;
            set;
        }

        /// <summary>
        /// Constructs a new instance of the Feist class
        /// </summary>
        /// <param name="passcode">Passcode used in this instance</param>
        public Feist(string passcode)
        {
            this.Passcode = passcode;
            this.BlockSize = 16;
        }

        /// <summary>
        /// Constructs a new instance of the Feist class, with a custom blocksize
        /// </summary>
        /// <param name="passcode">Passcode used in this instance</param>
        /// <param name="blockSize">Size of the blocks to use in this instance</param>
        public Feist(string passcode, int blockSize) : this(passcode)
        {
            this.BlockSize = blockSize;
        }

        /// <summary>
        /// Encrypts the given plaintext based on the Passcode
        /// </summary>
        /// <param name="plaintext">The string to encrypt</param>
        /// <returns>An encrypted string</returns>
        public string Encrypt(string plaintext)
        {
            return PerformCipher(plaintext, true);
        }

        /// <summary>
        /// Decrypts the given ciphertext based on the Passcode
        /// </summary>
        /// <param name="cipherText">The string to decrypt</param>
        /// <returns>A decrypted string</returns>
        public string Decrypt(string cipherText)
        {
            return PerformCipher(cipherText, false);
        }

        /// <summary>
        /// Performs a Feistel-like encryption cipher on the text
        /// </summary>
        /// <param name="text">The plain or cipher text to encrypt/decrypt</param>
        /// <param name="isPlaintext">Whether the text given is a plaintext string</param>
        /// <returns>A string of plain or ciphered text</returns>
        private string PerformCipher(string text, bool isPlaintext)
        {
            int blockPointer = 0;
            string postCipherText = "";
            List<ulong> roundKeys = new Key(Passcode).GetRoundKeys();

            // Pad the string with '\0' to make it a multiple of <blocksize>
            while(text.Length % BlockSize != 0)
                text += new char();

            // Reverse the keys if it's decrypting
            if(!isPlaintext)
                roundKeys.Reverse();

            byte[] textBytes = Encoding.UTF8.GetBytes(text);

            // Iterate through the text, moving <blocksize> bytes per iteration
            while(blockPointer < textBytes.Length)
            {
                byte[] blockBytes = new byte[BlockSize];
                Array.Copy(textBytes, blockPointer, blockBytes, 0, BlockSize);
                Block textBlock = new Block(blockBytes);

                // Swap the halves if it's ciphertext
                if(!isPlaintext)
                    textBlock.Swap();

                // Apply each of the round keys to the text
                foreach(ulong roundKey in roundKeys)
                    textBlock = Round(textBlock, roundKey);

                // Append to the output string
                if(!isPlaintext) textBlock.Swap();
                postCipherText += textBlock.ToString();

                blockPointer += BlockSize;
            }
            return postCipherText.Trim('\0');
        }

        /// <summary>
        /// Performs a single round encryption on a block
        /// </summary>
        /// <param name="block">The block to encrypt/decrypt</param>
        /// <param name="roundKey">Round key to apply as the round function</param>
        /// <returns>The next block in the round sequence</returns>
        private Block Round(Block block, ulong roundKey)
        {
            ulong roundFunction = 0;

            Block roundBlock = new Block(block.BlockSize);
            BitArray keyBits = new BitArray(BitConverter.GetBytes(roundKey)),
                     funcBits  = block.RightBlockBits.Xor(keyBits);

            roundBlock.Left = block.Right;

            // Cast the AND round function bits to an int, set R(i+1) as L(i) XOR f
            roundFunction = ToInteger64(funcBits);
            roundBlock.Right = BitConverter.GetBytes(ToInteger64(block.LeftBlockBits) ^ roundFunction);

            return roundBlock;
        }

        /// <summary>
        /// Helper method to convert BitArray to integer representation
        /// </summary>
        /// <param name="array">BitArray to convert</param>
        /// <returns>A 64-bit integer value of the array</returns>
        private ulong ToInteger64(BitArray array)
        {
            byte[] byteArray = new byte[8];
            array.CopyTo(byteArray, 0);
            return BitConverter.ToUInt64(byteArray, 0);
        }
    }
}
