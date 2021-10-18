﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCashier
{
    class MenuItem
    {
        public ConsoleKey Key { get; set; }
        public string Label { get; set; }

        public MenuItem(ConsoleKey key, string label)
        {
            Key = key;
            Label = label;
        }

        public void Print()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(Key);
            Console.Write(" ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Label);
        }
    }
}