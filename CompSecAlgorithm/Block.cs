﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompSecAlgorithm
{
    class Block
    {
        /// <summary>
        /// The data held by the Block
        /// </summary>
        public byte[] Data
        {
            get;
            set;
        }

        /// <summary>
        /// Left half of the Block data
        /// </summary>
        public byte[] Left
        {
            get { return Data.Take(Data.Length / 2).ToArray();  }
            set { Array.Copy(value, Data, Data.Length / 2); }
        }

        /// <summary>
        /// Right half of the Block data
        /// </summary>
        public byte[] Right
        {
            get { return Data.Skip(Data.Length / 2).ToArray(); }
            set { Array.Copy(value, 0, Data, Data.Length / 2, Data.Length / 2); }
        }

        /// <summary>
        /// Returns the Left half as a BitArray
        /// </summary>
        public BitArray LeftBlockBits
        {
            get { return new BitArray(Left); }
        }

        /// <summary>
        /// Returns the Right half as a BitArray
        /// </summary>
        public BitArray RightBlockBits
        {
            get { return new BitArray(Right); }
        }

        /// <summary>
        /// Size in bytes of the Block
        /// </summary>
        public int BlockSize
        {
            get { return Data.Length; }
        }

        /// <summary>
        /// A block of data
        /// </summary>
        public Block(int blockSize)
        {
            Data = new byte[blockSize];
        }

        /// <summary>
        /// A block of data
        /// </summary>
        /// <param name="blockdata">Data stored by this block</param>
        public Block(byte[] blockdata)
        {
            Data = blockdata;
        }

        /// <summary>
        /// Swaps the left and right halves of the block
        /// </summary>
        public void Swap()
        {
            byte[] temp = Left;
            Left = Right;
            Right = temp;
        }
    }
}
