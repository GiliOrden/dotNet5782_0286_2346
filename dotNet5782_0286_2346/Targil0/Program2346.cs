using System;

namespace Targil0
{
   partial class Program
    {
        static void Main(string[] args)
        {
            NewMethod();

            Console.ReadKey();
        }

        private static void NewMethod()
        {
            Console.Write("Enter your name: ");
            string Welcome2346 = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", Welcome2346);
        }
        static partial void Welcome0286();
       
    }
}
