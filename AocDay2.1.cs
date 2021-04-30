using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc
{
    public class Program
    {
        static void Main(string[] args)
        {
            string[] input = System.IO.File.ReadLines("input2.1.txt").ToArray();
            if (input.Count() == 1)
            {
                string fullText = input.First();
                string[] split = fullText.Split(',');
                split[1] = "12";
                split[2] = "2";
                for (int i = 0; i < split.Length; i += 4)
                {
                    int opcode = Int32.Parse(split[i]);
                    int index1 = 0;
                    int index2 = 0;
                    int index3 = 0;
                    switch (opcode)
                    {
                        case 1:
                            index1 = Int32.Parse(split[i + 1]);
                            index2 = Int32.Parse(split[i + 2]);
                            index3 = Int32.Parse(split[i + 3]);
                            split[index3] = (Int32.Parse(split[index1]) + Int32.Parse(split[index2])).ToString();
                            break;
                        case 2:
                            index1 = Int32.Parse(split[i + 1]);
                            index2 = Int32.Parse(split[i + 2]);
                            index3 = Int32.Parse(split[i + 3]);
                            split[index3] = (Int32.Parse(split[index1]) * Int32.Parse(split[index2])).ToString();
                            break;
                        case 99:
                            Console.WriteLine(split[0]);
                            return;
                        default:
                            break;
                    }
                }
                Console.WriteLine(split[0]);
            }
            else
            {
                throw new DataMisalignedException();
            }
        }
    }
}