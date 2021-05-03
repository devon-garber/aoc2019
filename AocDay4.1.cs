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
            string[] input = System.IO.File.ReadLines("input4.1.txt").ToArray();
            int matchCount = 0;
            if (input.Length == 1)
            {
                string[] range = input.First().Split('-');
                if (range.Length == 2)
                {
                    for (int i = Int32.Parse(range.First()); i <= Int32.Parse(range[1]); ++i)
                    {
                        bool twoAdj = false;
                        bool neverDecrease = true;
                        string s = i.ToString();
                        for (int j = 0; j < s.Length - 1; ++j)
                        {
                            if (s[j] > s[j + 1])
                            {
                                neverDecrease = false;
                            }
                            if (s[j] == s[j + 1])
                            {
                                twoAdj = true;
                            }
                        }

                        if (twoAdj && neverDecrease)
                        {
                            matchCount++;
                        }
                    }
                }
            }
            Console.WriteLine(matchCount);
        }
    }
}