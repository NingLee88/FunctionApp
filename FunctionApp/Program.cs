using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string Details= JsonFileChecker.CheckFile();
            Console.WriteLine(Details);
            Console.WriteLine("Fininshed!");
            Console.Read();
        }
    }
}
