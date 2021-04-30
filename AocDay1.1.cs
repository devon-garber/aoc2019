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
            string[] input = System.IO.File.ReadLines("input1.1.txt").ToArray();
            int total = 0;
            foreach (int num in input.Select(x => Int32.Parse(x)))
            {
                int div = num / 3;
                total += div - 2;
            }

            Console.WriteLine(total);
        }
    }
}