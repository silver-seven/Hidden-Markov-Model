using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMM
{
    class Robot
    {
        private double[,] map;
        private double[,] prior_map;
        private double[,] cur_map;
        private int height;
        private int width;
        private double wall_prob_c;
        private double open_prob_c;
        private double drift_prob;
        private double forward_prob;

  
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
            temp_map.getMap(ref map);
            updatePriorMapFromMap();

            //default initial values
            wall_prob_c = 0.9;
            open_prob_c = 0.95;
            forward_prob = 0.8;
            drift_prob = 0.1;
        }


        //setters
        public void setWallProb(double val)
        {
            wall_prob_c = val;
        }

        public void setOpenProb(double val)
        {
            open_prob_c = val;
        }

        public void setDriftProb(double val)
        {
            drift_prob = val;
        }

        public void setForwardProb(double val)
        {
            forward_prob = val;
        }


        //public methods
        public void printMap()
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    double val = cur_map[w, h] * 100;
                    if (val >= 10)
                    {
                        Console.Write(String.Format("{0:f2}  ", val));
                    }
                    else
                    {
                        Console.Write(" " + String.Format("{0:f2}  ", val));
                    }
                }
                Console.WriteLine("");
            }
            Console.WriteLine("Current Probability");
        }

        public void printRealMap()
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    double val = map[w, h] * 100;
                    if (val >= 10)
                    {
                        Console.Write(String.Format("{0:f2}  ", val));
                    }
                    else
                    {
                        Console.Write(" " + String.Format("{0:f2}  ", val));
                    }
                }
                Console.WriteLine("");
            }
            Console.WriteLine("Real Map");
        }

        public void printPriorMap()
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    double val = prior_map[w, h] * 100;
                    if (val >= 10)
                    {
                        Console.Write(String.Format("{0:f2}  ", val));
                    }
                    else
                    {
                        Console.Write(" " + String.Format("{0:f2}  ", val));
                    }
                }
                Console.WriteLine("");
            }
            Console.WriteLine("Prior Probability");
        }

        public void sensorUpdate(char cw, char cn, char ce, char cs)
        {

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    cur_map[w, h] = getWestSensor(w, h, cw) * getNorthSensor(w, h, cn) * getEastSensor(w, h, ce) * getSouthSensor(w, h, cs) * (prior_map[w, h]);
                }
            }
            normalize();
            updatePriorMap();
            Console.WriteLine("\nSensor Update (" + cw + ", " + cn + ", " + ce + ", " + cs + ")");
        }

        public void motionUpdate(string dir, bool showSum = false)
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    double prior = prior_map[w, h];
                    cur_map[w, h] = (getWestMotion(w, h, dir) + getNorthMotion(w, h, dir) + getEastMotion(w, h, dir) + getSouthMotion(w, h, dir));
                }
            }
            Console.WriteLine("\nMotion Update: " + dir);
            if (showSum)
            {
                printSumOfCurrent();
            }
            updatePriorMap();
        }


        //private methods
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
                    cur_map[w, h] = (double) cur_map[w, h]/sum;
                }
            }
        }

        private double getWestSensor(int x, int y, char sensor)
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
                    temp = wall_prob_c;
                }
                else                // '-' open field
                {
                    temp = open_prob_c;
                }

            }
            else                //sensor is incorrect
            {
                if(sensor == 'o') //actual is an open field
                {
                    temp = 1-open_prob_c;
                }
                else
                {
                    temp = 1-wall_prob_c;
                }
            }
            string coord = x + ", " + y;
            return temp;
        }

        private double getNorthSensor(int x, int y, char sensor)
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
                    temp = wall_prob_c;
                }
                else                // '-' open field
                {
                    temp = open_prob_c;
                }

            }
            else                //sensor is incorrect
            {
                if (sensor == 'o') //actual is an open field
                {
                    temp = 1-open_prob_c;
                }
                else
                {
                    temp = 1-wall_prob_c;
                }
            }
            return temp;
        }

        private double getEastSensor(int x, int y, char sensor)
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
                    temp = wall_prob_c;
                }
                else                // '-' open field
                {
                    temp = open_prob_c;
                }

            }
            else                //sensor is incorrect
            {
                if (sensor == 'o') //actual is an open field
                {
                    temp = 1-open_prob_c;
                }
                else
                {
                    temp = 1-wall_prob_c;
                }
            }
            return temp;
        }

        private double getSouthSensor(int x, int y, char sensor)
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
                    temp = wall_prob_c;
                }
                else                // '-' open field
                {
                    temp = open_prob_c;
                }

            }
            else                //sensor is incorrect
            {
                if (sensor == 'o') //actual is an open field
                {
                    temp = 1-open_prob_c;
                }
                else
                {
                    temp = 1-wall_prob_c;
                }
            }
            return temp;
        }

        private void updatePriorMap()
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    prior_map[w, h] = cur_map[w, h];
                }
            }
        }

        private void updatePriorMapFromMap()
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    prior_map[w, h] = map[w, h];
                }
            }
        }

        private void printSumOfCurrent()
        {
            double sum = 0;
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    sum += cur_map[w, h];
                }
            }
            Console.WriteLine("Sum: " + sum);
        }

        private double getWestMotion(int x, int y, string dir)
        {
            bool isWall = false;
            int testPos = x - 1;
            
            if(testPos < 0) //determine if the tile is a wall
            {
                isWall = true;
            }
            else
            {
                if(map[testPos, y] <= 0)
                {
                    isWall = true;
                }
            }

            //determine probability
            double prob = 0;
            if(isWall)  
            {
                prob = prior_map[x, y];
                if(dir == "north")
                {
                    prob *= drift_prob; //drift
                }
                else if(dir == "east")
                {
                    prob = 0;
                }
                else if(dir == "west")
                {
                    prob *= forward_prob;
                }
                else
                {
                    prob *= drift_prob;
                }
            }
            else
            {
                prob = prior_map[testPos, y];
                if(dir == "north")
                {
                    prob *= drift_prob;
                }
                else if(dir == "east")
                {
                    prob *= forward_prob;
                }
                else if(dir == "west")
                {
                    prob = 0;
                }
                else
                {
                    prob *= drift_prob;
                }
            }
            return prob;
        }

        private double getNorthMotion(int x, int y, string dir)
        {
            bool isWall = false;
            int testPos = y - 1;

            if (testPos < 0) //determine if the tile is a wall
            {
                isWall = true;
            }
            else
            {
                if (map[x, testPos] <= 0)
                {
                    isWall = true;
                }
            }

            //determine probability
            double prob = 0;
            if (isWall)
            {
                prob = prior_map[x, y];
                if (dir == "north")
                {
                    prob *= forward_prob;
                }
                else if (dir == "east")
                {
                    prob *= drift_prob;
                }
                else if (dir == "west")
                {
                    prob *= drift_prob;
                }
                else
                {
                    prob = 0;
                }
            }
            else
            {
                prob = prior_map[x, testPos];
                if (dir == "north")
                {
                    prob = 0;
                }
                else if (dir == "east")
                {
                    prob *= drift_prob;
                }
                else if (dir == "west")
                {
                    prob *= drift_prob;
                }
                else
                {
                    prob *= forward_prob;
                }
            }
            return prob;
        }

        private double getEastMotion(int x, int y, string dir)
        {
            bool isWall = false;
            int testPos = x + 1;

            if (testPos >= width) //determine if the tile is a wall
            {
                isWall = true;
            }
            else
            {
                if (map[testPos, y] <= 0)
                {
                    isWall = true;
                }
            }

            //determine probability
            double prob = 0;
            if (isWall)
            {
                prob = prior_map[x, y];
                if (dir == "north")
                {
                    prob *= drift_prob;
                }
                else if (dir == "east")
                {
                    prob *= forward_prob;
                }
                else if (dir == "west")
                {
                    prob = 0;
                }
                else
                {
                    prob *= drift_prob;
                }
            }
            else
            {
                prob = prior_map[testPos, y];
                if (dir == "north")
                {
                    prob *= drift_prob;
                }
                else if (dir == "east")
                {
                    prob = 0;
                }
                else if (dir == "west")
                {
                    prob *= forward_prob;
                }
                else
                {
                    prob *= drift_prob;
                }
            }
            return prob;
        }

        private double getSouthMotion(int x, int y, string dir)
        {
            bool isWall = false;
            int testPos = y + 1;

            if (testPos >= height) //determine if the tile is a wall
            {
                isWall = true;
            }
            else
            {
                if (map[x, testPos] <= 0)
                {
                    isWall = true;
                }
            }

            //determine probability
            double prob = 0;
            if (isWall)
            {
                prob = prior_map[x, y];
                if (dir == "north")
                {
                    prob = 0;
                }
                else if (dir == "east")
                {
                    prob *= drift_prob;
                }
                else if (dir == "west")
                {
                    prob *= drift_prob;
                }
                else
                {
                    prob *= forward_prob;
                }
            }
            else
            {
                prob = prior_map[x, testPos];
                if (dir == "north")
                {
                    prob *= forward_prob;
                }
                else if (dir == "east")
                {
                    prob *= drift_prob;
                }
                else if (dir == "west")
                {
                    prob *= drift_prob;
                }
                else
                {
                    prob = 0;
                }
            }
            return prob;
        }
    }
}
