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
            for (int i = 0; i <= 99; ++i)
            {
                for (int j = 0; j <= 99; ++j)
                {
                    if (RunProgram(i.ToString(), j.ToString(), "19690720"))
                    {
                        Console.WriteLine(100 * i + j);
                        return;
                    }
                }
            }
        }

        private static bool RunProgram(string noun, string verb, string target)
        {
            string[] input = System.IO.File.ReadLines("input2.1.txt").ToArray();
            if (input.Count() == 1)
            {
                string fullText = input.First();
                string[] split = fullText.Split(',');
                split[1] = noun;
                split[2] = verb;
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
                            if (split[0] == target)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        default:
                            break;
                    }
                }
                if (split[0] == target)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new DataMisalignedException();
            }
        }
    }
}