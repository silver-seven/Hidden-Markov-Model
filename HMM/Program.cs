using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMM
{
    class Program
    {
        static void Main(string[] args)
        {
            Robot rob = new Robot("input.csv");
            rob.printMap();
            rob.sensorUpdate('o', 'o', 'o', 'o');
            rob.printMap();
            rob.printPriorMap();

        }
    }
}
