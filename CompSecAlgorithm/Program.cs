using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompSecAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Feist feist = new Feist("1234567890ABCDEF");
            feist.Encrypt("This is my plaintext block of text, isn't it great?");
        }
    
    }
}
