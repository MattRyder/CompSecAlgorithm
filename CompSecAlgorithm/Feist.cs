using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompSecAlgorithm
{
    class Feist
    {
        public byte[] PlaintextData
        {
            get;
            set;
        }

        public int BlockSize
        {
            get;
            private set;
        }

        public string Passcode
        {
            get;
            set;
        }

        public Feist(string passcode)
        {
            this.Passcode = passcode;
            this.BlockSize = 8;
        }

        public Feist(string passcode, int blockSize) : this(passcode)
        {
            this.BlockSize = blockSize;
        }

        public void Encrypt(string plaintext)
        {
            int blockPtr = 0;

            Key encryptionKey = new Key(Passcode);
            List<uint> roundKeys = encryptionKey.GetRoundKeys();
            List<byte> plaintextBytes = Encoding.UTF8.GetBytes(plaintext).ToList();

            // Pad the plaintext to a multiple of the block size
            while(plaintextBytes.Count % BlockSize != 0)
                plaintextBytes.Add(0);
            this.PlaintextData = plaintextBytes.ToArray();

            // Iterate through, encrypt each block
            while(blockPtr < PlaintextData.Length)
            {
                // Get the next block
                byte[] ptBytes = new byte[BlockSize];
                Array.Copy(PlaintextData, blockPtr, ptBytes, 0, BlockSize);
                Block plaintextBlock = new Block(ptBytes);

                foreach(uint roundKey in roundKeys)
                {
                    plaintextBlock = Round(plaintextBlock, roundKey);
                }

                plaintextBlock.Data.ToList().ForEach(x => System.Diagnostics.Debug.Write(x));
                System.Diagnostics.Debug.Write(Environment.NewLine);

                blockPtr += BlockSize;
            }
        }

        private Block Round(Block block, uint roundKey)
        {
            int roundFunction = 0;

            Block roundBlock = new Block(block.BlockSize);
            BitArray keyBits = new BitArray(BitConverter.GetBytes(roundKey)),
                     funcBits  = block.RightBlockBits.And(keyBits);

            roundBlock.Left = block.Right;

            // Cast the AND round function bits to an int, set R(i+1) as L(i) XOR f
            roundFunction = ToInteger(funcBits);
            roundBlock.Right = BitConverter.GetBytes(ToInteger(block.LeftBlockBits) ^ roundFunction);

            return roundBlock;
        }

        private int ToInteger(BitArray array)
        {
            int[] tempIntArray = new int[1];
            array.CopyTo(tempIntArray, 0);
            return tempIntArray[0];
        }

    }
}
