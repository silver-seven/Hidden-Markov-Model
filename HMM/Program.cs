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
            rob.setOpenProb(.95);
            rob.setWallProb(.9);
            rob.setForwardProb(.8);
            rob.setDriftProb(.1);

            rob.printPriorMap();
            rob.sensorUpdate('-', '-', '-', '-');
            Console.WriteLine("jquejada & nlenze");
            rob.printMap();
            rob.motionUpdate("north");
            Console.WriteLine("jquejada & nlenze");
            rob.printMap();

            rob.sensorUpdate('-', '-', '-', '-');
            Console.WriteLine("jquejada & nlenze");
            rob.printMap();
            rob.motionUpdate("north");
            Console.WriteLine("jquejada & nlenze");
            rob.printMap();

            rob.sensorUpdate('-', '-', 'o', '-');
            Console.WriteLine("jquejada & nlenze");
            rob.printMap();
            rob.motionUpdate("north");
            Console.WriteLine("jquejada & nlenze");
            rob.printMap();

            rob.sensorUpdate('-', '-', '-', '-');
            Console.WriteLine("jquejada & nlenze");
            rob.printMap();
            rob.motionUpdate("east");
            Console.WriteLine("jquejada & nlenze");
            rob.printMap();

            rob.sensorUpdate('-', '-', 'o', 'o');
            Console.WriteLine("jquejada & nlenze");
            rob.printMap();
            rob.motionUpdate("north");
            Console.WriteLine("jquejada & nlenze");
            rob.printMap();

            rob.sensorUpdate('-', '-', 'o', '-');
            Console.WriteLine("jquejada & nlenze");
            rob.printMap();

        }
    }
}
