using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var rn = new Random();
            var r = rn.Next(0, 10000).ToString();
            Console.WriteLine("Hello World!");
        }
    }
}
