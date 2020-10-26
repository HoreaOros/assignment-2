using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int inputType=1;
            bool ok = false;
            HashAlgorithm alg = HashAlgorithm.Create();
            Console.WriteLine("Select the input type:\n\t1. string\n\t2. byte[]\n\t3. file path");
            while (!ok)
            {
                ok = int.TryParse(Console.ReadLine(), out inputType);
                if (ok) 
                {
                    if (inputType < 0 || inputType > 4)
                    {
                        Console.WriteLine("Not a number within the bounds. Select a number between 1 and 3");
                        ok = false;
                    }
                }
                else
                    Console.WriteLine("Type in a number between 1 and 3");
            }
            ok = false;
            string aux = "";
            switch (inputType)
            {
                case 1:
                    aux = "string";
                    break;

                case 2:
                    aux = "byte array, each byte separated by a space (' ')";
                    break;

                case 3:
                    aux = "path";
                    break;

                default:
                    break;
            }
            Console.WriteLine("Type in a {0}", aux);
            string input = Console.ReadLine();

            Console.WriteLine("Type in a hash algorithm");
            while (!ok)
            {
                try
                {
                    alg = HashAlgorithm.Create(Console.ReadLine());
                    ok = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Not a valid hash algorithm");
                    ok = false;
                }
            }


            byte[] hash= new byte[1];
            switch (inputType)
            {
                case 1:

                    hash = await GetHashAsync(alg, input);
                    break;

                case 2:

                    string[] bytesAsText = input.Split(' ');
                    byte[] byteArr=new byte[bytesAsText.Length];
                    for (int i = 0; i < bytesAsText.Length; i++)
                    {
                        try
                        {
                            byteArr[i] = Convert.ToByte(bytesAsText[i]);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Invalid byte array");
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        hash = await GetHashAsync(alg, byteArr);
                    }

                    break;

                case 3:

                    try
                    {
                        hash = await GetHashAsync(alg, new FileStream(input, FileMode.Open));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    break;

                default:
                    break;
            }

            for (int i = 0; i < hash.Length; i++)
            {
                Console.Write($"{hash[i]:X2}");
                if ((i % 4) == 3) Console.Write(" ");
            }
            Console.WriteLine();
            Console.ReadKey();



        }




        static async Task<byte[]> GetHashAsync(HashAlgorithm algorithm, string input)
        {
            byte[] data;
            data = Encoding.UTF8.GetBytes(input as string);
            return algorithm.ComputeHash(data);
        }
        static async Task<byte[]> GetHashAsync(HashAlgorithm algorithm, byte[] input)
        {
            return algorithm.ComputeHash(input);
        }
        static async Task<byte[]> GetHashAsync(HashAlgorithm algorithm, FileStream fs)
        {
            return algorithm.ComputeHash(fs);
        }
    }
}
