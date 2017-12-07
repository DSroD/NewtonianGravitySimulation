using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets
{
    static class VOSP87
    {
        public static PreciseVector getPlanetPosition(string abb, float JDE)
        {
            if(File.Exists("VSOP87/VSOP87C." + abb))
            {
                string[] lines = File.ReadAllLines("VSOP87/VSOP87C." + abb);
                double x0 = 0;
                double x1 = 0;
                double x2 = 0;
                double x3 = 0;
                double x4 = 0;
                double x5 = 0;

                double y0 = 0;
                double y1 = 0;
                double y2 = 0;
                double y3 = 0;
                double y4 = 0;
                double y5 = 0;

                float t = (JDE - 2451545) / 365250;

                foreach (string line in lines)
                {
                    if (line[1] != '3' || line[3] == '3') continue;
                    switch (line[4])
                    {
                        case '0':
                            if(line[3] == '1') //X COORD
                            {
                                x0 += getTerm(line, t);
                            }
                            else if(line[3] == '2') //Y COORD
                            {
                                y0 += getTerm(line, t);
                            }
                            break;
                        case '1':
                            if (line[3] == '1') //X COORD
                            {
                                x1 += getTerm(line, t);
                            }
                            else if (line[3] == '2') //Y COORD
                            {
                                y1 += getTerm(line, t);
                            }
                            break;
                        case '2':
                            if (line[3] == '1') //X COORD
                            {
                                x2 += getTerm(line, t);
                            }
                            else if (line[3] == '2') //Y COORD
                            {
                                y2 += getTerm(line, t);
                            }
                            break;
                        case '3':
                            if (line[3] == '1') //X COORD
                            {
                                x3 += getTerm(line, t);
                            }
                            else if (line[3] == '2') //Y COORD
                            {
                                y3 += getTerm(line, t);
                            }
                            break;
                        case '4':
                            if (line[3] == '1') //X COORD
                            {
                                x4 += getTerm(line, t);
                            }
                            else if (line[2] == '2') //Y COORD
                            {
                                y4 += getTerm(line, t);
                            }
                            break;
                        case '5':
                            if (line[3] == '1') //X COORD
                            {
                                x5 += getTerm(line, t);
                            }
                            else if (line[3] == '2') //Y COORD
                            {
                                y5 += getTerm(line, t);
                            }
                            break;
                    }
                }

                double x = x0 + x1 * t + x2 * t * t + x3 * t * t * t + x4 * t * t * t * t + x5 * t * t * t * t * t;
                double y = y0 + y1 * t + y2 * t * t + y3 * t * t * t + y4 * t * t * t * t + y5 * t * t * t * t * t;

                return new PreciseVector(x, y);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public static PreciseVector getPlanetVelocity(string abb, float JDE, float deltaT)
        {
            PreciseVector l0 = getPlanetPosition(abb, JDE);
            PreciseVector dl = getPlanetPosition(abb, JDE + deltaT);
            return ((dl - l0) * (58.14f/deltaT)); //convert from au/days - 58.1 because wolfram said so (1 / (earth average velocity in au/day))
        }

        private static double getTerm(string line, float t)
        {
            string[] spl1 = line.Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
            float A = (float)Convert.ToDouble(spl1[16], System.Globali­zation.Culture­Info.Invarian­tCulture);
            float B = (float)Convert.ToDouble(spl1[17], System.Globali­zation.Culture­Info.Invarian­tCulture);
            float C = (float)Convert.ToDouble(spl1[18], System.Globali­zation.Culture­Info.Invarian­tCulture);

            return A * (float)Math.Cos(B + C * t);
        }

    }
}
