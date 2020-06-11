using MongoDBConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParametersTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var pers = new MongoContext();
            var a = new ClientParameters(pers);
            a.LoadAll();

            a.Name = "diego";
            a.SaveAll();

            Console.WriteLine("program finished.");
            Console.ReadLine();
        }
    }
}
