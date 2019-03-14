using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMM
{
    class Robot
    {
        public double[,] map;
        public double[,] prior_map;
        public double[,] cur_map;
        public double[,] sensing_prob;
        public int height;
        public int width;
        public double wall_prob_c;
        public double open_prob_c;

        public Robot()
        {

        }

        public Robot(string file)
        {
            MapIO temp_map = new MapIO(file);
            height = temp_map.getHeight();
            width = temp_map.getWidth();

            map = new double[width, height];
            cur_map = new double[width, height];
            prior_map = new double[width, height];
            sensing_prob = new double[width, height];
            temp_map.getMap(ref map);
            temp_map.getMap(ref prior_map);
            wall_prob_c = 0.9;
            open_prob_c = 0.95;
        }


        //methods
        public void printMap()
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    Console.Write(String.Format("{0:F2}  ", cur_map[w, h]));
                }
                Console.WriteLine("");
            }
            Console.WriteLine("Current Probability");
        }
        public void printPriorMap()
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    Console.Write(String.Format("{0:F2}  ", prior_map[w, h]));
                }
                Console.WriteLine("");
            }
            Console.WriteLine("Prior Probability");
        }

        private void normalize()
        {
            double sum = 0;
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    sum += cur_map[w, h];
                }
            }

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    cur_map[w, h] = cur_map[w, h]/sum;
                }
            }
        }

        public void sensorUpdate(char cw, char cn, char ce, char cs)
        {

            for (int h = 0; h < height; h++)
            {
                for(int w = 0; w < width; w++)
                {
                    cur_map[w, h] = getWestSensor(w,h, cw) * getNorthSensor(w, h, cn) * getEastSensor(w, h, ce) * getSouthSensor(w, h, cs) * prior_map[w, h];
                }
            }
            normalize();
        }

        public double getWestSensor(int x, int y, char sensor)
        {
            char actual = '-'; //open field
            double temp = 0;
            int adj_pos = x - 1;

            //determine if actual position is a wall
            if(adj_pos < 0) //
            {
                actual = 'o';
            }
            else
            {
                if(map[adj_pos, y] <= 0)
                {
                    actual = 'o';
                }
            }

            //get actual probability
            if(sensor == actual) //sensor is correct
            {
                if(sensor == 'o')   //wall
                {
                    temp = .9;
                }
                else                // '-' open field
                {
                    temp = .95;
                }

            }
            else                //sensor is incorrect
            {
                if(sensor == 'o') //actual is an open field
                {
                    temp = .05;
                }
                else
                {
                    temp = .1;
                }
            }
            return temp;
        }

        public double getNorthSensor(int x, int y, char sensor)
        {
            char actual = '-'; //open field
            double temp = 0;
            int adj_pos = y - 1;

            //determine if actual position is a wall
            if (adj_pos < 0)
            {
                actual = 'o';
            }
            else
            {
                if (map[x, adj_pos] <= 0)
                {
                    actual = 'o';
                }
            }

            //get actual probability
            if (sensor == actual) //sensor is correct
            {
                if (sensor == 'o')   //wall
                {
                    temp = .9;
                }
                else                // '-' open field
                {
                    temp = .95;
                }

            }
            else                //sensor is incorrect
            {
                if (sensor == 'o') //actual is an open field
                {
                    temp = .05;
                }
                else
                {
                    temp = .1;
                }
            }
            return temp;
        }

        public double getEastSensor(int x, int y, char sensor)
        {
            char actual = '-'; //open field
            double temp = 0;
            int adj_pos = x + 1;

            //determine if actual position is a wall
            if (adj_pos >= width)
            {
                actual = 'o';
            }
            else
            {
                if (map[adj_pos, y] <= 0)
                {
                    actual = 'o';
                }
            }

            //get actual probability
            if (sensor == actual) //sensor is correct
            {
                if (sensor == 'o')   //wall
                {
                    temp = .9;
                }
                else                // '-' open field
                {
                    temp = .95;
                }

            }
            else                //sensor is incorrect
            {
                if (sensor == 'o') //actual is an open field
                {
                    temp = .05;
                }
                else
                {
                    temp = .1;
                }
            }
            return temp;
        }

        public double getSouthSensor(int x, int y, char sensor)
        {
            char actual = '-'; //open field
            double temp = 0;
            int adj_pos = y + 1;

            //determine if actual position is a wall
            if (adj_pos >= height)
            {
                actual = 'o';
            }
            else
            {
                if (map[x, adj_pos] <= 0)
                {
                    actual = 'o';
                }
            }

            //get actual probability
            if (sensor == actual) //sensor is correct
            {
                if (sensor == 'o')   //wall
                {
                    temp = .9;
                }
                else                // '-' open field
                {
                    temp = .95;
                }

            }
            else                //sensor is incorrect
            {
                if (sensor == 'o') //actual is an open field
                {
                    temp = .05;
                }
                else
                {
                    temp = .1;
                }
            }
            return temp;
        }

        public void motionUpdate()
        {

        }
    }
}
