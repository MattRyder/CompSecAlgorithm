using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompSecAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            string keyword = "", input = "", output = "", fileData = "";
            
            Feist feist;
            StreamReader fileReader;
            StreamWriter fileWriter;

            const string usage =
            @"
Computer Systems Security Algorithm

Usage:
    private [-option] keyword input_file output_file

Options:
    -e Encrypts the input_file with the keyword
    -d Decrypts the input_file with the keyword";

            // Output the usage if there's less than 4 args
            if(args.Length < 4)
            {
                Console.WriteLine(usage);
                return;
            }

            keyword = args[1];
            input   = args[2];
            output  = args[3];

            // Setup the program for IO, encryption
            feist = new Feist(keyword);
            fileReader = new StreamReader(input);
            fileWriter = new StreamWriter(output);

            // Read the data from the input file
            fileData = fileReader.ReadToEnd();
            fileReader.Close();

            if (args[0] == "-e")
            {
                // Encrypt argument selected
                string ciphertext = feist.Encrypt(fileData);
                System.Diagnostics.Debug.WriteLine(ciphertext);
                fileWriter.Write(ciphertext);
                Console.WriteLine("File Encrypted. Saved to: " + output);
            }
            else if(args[0] == "-d")
            {
                // Decrypt argument selected
                string plaintext = feist.Decrypt(fileData);
                System.Diagnostics.Debug.WriteLine(plaintext);
                fileWriter.Write(plaintext);
                Console.WriteLine("File Decrypted. Saved to: " + output);
            }
            else
            {
                // Invalid option selected, show usage and suspend
                Console.Write("Invalid option selected\n" + usage);
            }

            fileWriter.Close();
        }
    
    }
}
