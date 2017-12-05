using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.IntMethods
{
    class RungeKutta : IntegrationMethod
    {
        float g;
        Vector[] k1r;  //k factors
        Vector[] k1v;
        Vector[] k2r;
        Vector[] k2v;
        Vector[] k3r;
        Vector[] k3v;
        Vector[] k4r;
        Vector[] k4v;

        
        Vector[][] kvs;

        public RungeKutta(float G)
        {
            this.g = G;
        }


        public override void calculateAccelerations(List<Body> bodies, float deltaT)
        {
            for (int i = 0; i < bodies.Count; i++)
            {
                bodies.ElementAt(i).accel = Vector.Zero; //resetuje accel pro vypočtení a_(i+1)
                k1r[i] = bodies.ElementAt(i).position;
                k1v[i] = bodies.ElementAt(i).velocity;
                k2r[i] = Vector.Zero;
                k2v[i] = Vector.Zero;
                k3r[i] = Vector.Zero;
                k3v[i] = Vector.Zero;
                k4r[i] = Vector.Zero;
                k4v[i] = Vector.Zero;

            }
            Vector[][] krs = { k1r, k2r, k3r, k4r };
            Vector[][] kvs = { k1v, k2v, k3v, k4v };
            for (int k = 0; k < 4; k++)
            {
                for (int i = 0; i < bodies.Count; i++)
                {
                    for (int j = i + 1; j < bodies.Count; j++)
                    {
                        float r = Vector.distance(bodies.ElementAt(i).position, bodies.ElementAt(j).position);
                        float r3 = 1 / (float)Math.Pow(r, 3);
                        Vector ptr = g * (bodies.ElementAt(i).position - bodies.ElementAt(j).position) * r3;
                        bodies.ElementAt(i).accel += ptr * bodies.ElementAt(j).mass;
                        bodies.ElementAt(j).accel += ptr * bodies.ElementAt(i).mass;
                    }
                }
            }
        }

        public override void move(List<Body> bodies, float deltaT) // http://www.docstubo.net/48209192/Final-paper-uw324-pdf/ - násobení sloupců ???
        {
            foreach (Body b in bodies)
            {
                Vector[] Um = { b.position, b.velocity };
                Vector[] k1 = { Um[1], b.accel};
                Vector[] v1 = { Um[0] + deltaT / 2 * k1[0],  Um[1] + deltaT/2 * k1[1]};
                
                Vector[] k2 = { Vector.Zero, Vector.Zero };
                Vector[] k3 = { Vector.Zero, Vector.Zero };
                Vector[] k4 = { Vector.Zero, Vector.Zero };
            }
        }
    }
}
