using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HMM
{
    class MapIO
    {
        public double[,] map;
        private int width;
        private int height;

        public MapIO()
        {

        }

        public MapIO(string file)
        {
            int h_count = 0;
            foreach (string line in File.ReadLines(file))   
            {
                string[] temp_line = line.Split(',');
                width = temp_line.Length;
                h_count++;
            }
            height = h_count;

            //init map matrix
            map = new double[width, height];

            h_count = 0;
            foreach (string line in File.ReadLines(file))
            {
                string[] temp_line = line.Split(',');
                for (int w = 0; w < width; w++)
                {
                    map[w, h_count] = Convert.ToDouble(temp_line[w]);
                }
                h_count++;
            }
        }

        public void print()
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    Console.Write(String.Format("{0:F2}  ", map[w, h]));
                }
                Console.WriteLine("");
            }
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public void getMap(ref double[,] map_out)
        {
            map_out = map;
        }
    }
}
