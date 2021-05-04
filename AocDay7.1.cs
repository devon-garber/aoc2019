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
            List<int> possiblePhases = new List<int>() { 0, 1, 2, 3, 4 };
            List<IEnumerable<int>> allPerm = GetPermutations(possiblePhases, possiblePhases.Count).ToList();
            int maxOutput = 0;
            foreach (var list in allPerm)
            {
                List<int> indexable = list.ToList();

                int inputData = 0;
                for (int i = 0; i < 5; ++i)
                {
                    inputData = RunProgram(indexable[i].ToString(), inputData.ToString());
                }

                if (inputData > maxOutput)
                {
                    maxOutput = inputData;
                }
            }

            Console.WriteLine(maxOutput);
        }

        private static int RunProgram(string phaseSetting, string inputData)
        {
            string[] input = System.IO.File.ReadLines("input7.1.txt").ToArray();
            bool firstInput = true;
            if (input.Count() == 1)
            {
                string fullText = input.First();
                string[] program = fullText.Split(',');

                int programCounter = 0;
                Instruction lastInstruction = new Instruction();
                while (true)
                {
                    Instruction ins = GetInstruction(programCounter, program);
                    if (ins.Opcode == 1)
                    {
                        PerformOperation(program, ins, OperationType.Addition);
                        programCounter += 4;
                    }
                    else if (ins.Opcode == 2)
                    {
                        PerformOperation(program, ins, OperationType.Multiplication);
                        programCounter += 4;
                    }
                    else if (ins.Opcode == 3)
                    {
                        if (firstInput)
                        {
                            program[ins.Param1] = phaseSetting;
                            firstInput = false;
                        }
                        else
                        {
                            program[ins.Param1] = inputData;
                        }
                        programCounter += 2;
                    }
                    else if (ins.Opcode == 4)
                    {
                        if (Int32.Parse(program[ins.Param1]) == 0)
                        {
                            // Don't do anything yet
                        }
                        programCounter += 2;
                    }
                    else if (ins.Opcode == 5)
                    {
                        if (GetAppropriateValue(program, ins.Param1, ins.Param1Mode) != 0)
                        {
                            programCounter = GetAppropriateValue(program, ins.Param2, ins.Param2Mode);
                        }
                        else
                        {
                            programCounter += 3;
                        }
                    }
                    else if (ins.Opcode == 6)
                    {
                        if (GetAppropriateValue(program, ins.Param1, ins.Param1Mode) == 0)
                        {
                            programCounter = GetAppropriateValue(program, ins.Param2, ins.Param2Mode);
                        }
                        else
                        {
                            programCounter += 3;
                        }
                    }
                    else if (ins.Opcode == 7)
                    {
                        if (GetAppropriateValue(program, ins.Param1, ins.Param1Mode) < GetAppropriateValue(program, ins.Param2, ins.Param2Mode))
                        {
                            program[ins.Param3] = "1";
                        }
                        else
                        {
                            program[ins.Param3] = "0";
                        }
                        programCounter += 4;
                    }
                    else if (ins.Opcode == 8)
                    {
                        if (GetAppropriateValue(program, ins.Param1, ins.Param1Mode) == GetAppropriateValue(program, ins.Param2, ins.Param2Mode))
                        {
                            program[ins.Param3] = "1";
                        }
                        else
                        {
                            program[ins.Param3] = "0";
                        }
                        programCounter += 4;
                    }
                    else if (ins.Opcode == 99)
                    {
                        if (lastInstruction.Opcode == 4)
                        {
                            return Int32.Parse(program[lastInstruction.Param1]);
                        }
                        return -1;
                    }
                    lastInstruction = ins;
                }
            }
            else
            {
                throw new DataMisalignedException();
            }
        }

        private static Instruction GetInstruction(int programCounter, string[] split)
        {
            Instruction instruction = new Instruction();
            string opcodeData = Int32.Parse(split[programCounter]).ToString("D5");
            instruction.Opcode = Int32.Parse(opcodeData.Substring(3));
            if (instruction.Opcode == 1 || instruction.Opcode == 2 || instruction.Opcode == 7 || instruction.Opcode == 8)
            {
                instruction.Param1 = Int32.Parse(split[programCounter + 1]);
                instruction.Param2 = Int32.Parse(split[programCounter + 2]);
                instruction.Param3 = Int32.Parse(split[programCounter + 3]);
                instruction.Param1Mode = GetParameterMode(opcodeData[2]);
                instruction.Param2Mode = GetParameterMode(opcodeData[1]);
                instruction.Param3Mode = GetParameterMode(opcodeData[0]);
            }
            else if (instruction.Opcode == 3 || instruction.Opcode == 4)
            {
                instruction.Param1 = Int32.Parse(split[programCounter + 1]);
            }
            else if (instruction.Opcode == 5 || instruction.Opcode == 6)
            {
                instruction.Param1 = Int32.Parse(split[programCounter + 1]);
                instruction.Param2 = Int32.Parse(split[programCounter + 2]);
                instruction.Param1Mode = GetParameterMode(opcodeData[2]);
                instruction.Param2Mode = GetParameterMode(opcodeData[1]);
            }
            else if (instruction.Opcode == 99)
            {
            }
            return instruction;
        }

        private static ParameterMode GetParameterMode(char parameterMode)
        {
            if (parameterMode == '0')
            {
                return ParameterMode.PositionMode;
            }
            else if (parameterMode == '1')
            {
                return ParameterMode.ImmediateMode;
            }
            else
            {
                return ParameterMode.PositionMode;
            }
        }

        private static void PerformOperation(string[] program, Instruction ins, OperationType type)
        {
            int value1 = GetAppropriateValue(program, ins.Param1, ins.Param1Mode);
            int value2 = GetAppropriateValue(program, ins.Param2, ins.Param2Mode);
            if (type == OperationType.Addition)
            {
                program[ins.Param3] = (value1 + value2).ToString();
            }
            else if (type == OperationType.Multiplication)
            {
                program[ins.Param3] = (value1 * value2).ToString();
            }
        }

        private static int GetAppropriateValue(string[] program, int param, ParameterMode mode)
        {
            if (mode == ParameterMode.PositionMode)
            {
                return Int32.Parse(program[param]);
            }
            else if (mode == ParameterMode.ImmediateMode)
            {
                return param;
            }
            else
            {
                // Do nothing
                return -1;
            }
        }

        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1)
            {
                return list.Select(t => new T[] { t });
            }

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        private class Instruction
        { 
            public int Opcode { get; set; }
            public ParameterMode Param1Mode { get; set; }
            public ParameterMode Param2Mode { get; set; }
            public ParameterMode Param3Mode { get; set; }
            public int Param1 { get; set; }
            public int Param2 { get; set; }
            public int Param3 { get; set; }

            public Instruction()
            {
                this.Opcode = 0;
                this.Param1 = 0;
                this.Param2 = 0;
                this.Param3 = 0;
            }
        }

        private enum ParameterMode
        { 
            PositionMode,
            ImmediateMode
        }

        private enum OperationType
        {
            Multiplication,
            Addition
        }
    }
}
