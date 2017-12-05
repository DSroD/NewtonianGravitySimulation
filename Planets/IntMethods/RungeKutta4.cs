using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.IntMethods
{
    class RungeKutta4 : IntegrationMethod
    {
        float g;
        Vector[] k0r;  //k factors
        Vector[] k0v;
        Vector[] k1r;
        Vector[] k1v;
        Vector[] k2r;
        Vector[] k2v;
        Vector[] k3r;
        Vector[] k3v;

        
        Vector[][] kvs;

        public RungeKutta4(float G, int bodyCount)
        {
            this.g = G;
            k0r = new Vector[bodyCount];
            k0v = new Vector[bodyCount];
            k1r = new Vector[bodyCount];
            k1v = new Vector[bodyCount];
            k2r = new Vector[bodyCount];
            k2v = new Vector[bodyCount];
            k3r = new Vector[bodyCount];
            k3v = new Vector[bodyCount];
        }


        public override void calculateAccelerations(List<Body> bodies, float deltaT)
        {

            for (int i = 0; i < bodies.Count; i++)
            {
                k0r[i] = bodies.ElementAt(i).position;
                k0v[i] = Vector.Zero;
                k1r[i] = Vector.Zero;
                k1v[i] = Vector.Zero;
                k2r[i] = Vector.Zero;
                k2v[i] = Vector.Zero;
                k3r[i] = Vector.Zero;
                k3v[i] = Vector.Zero;

            }
            Vector[][] krs = { k0r, k1r, k2r, k3r };
            Vector[][] kvs = { k0v, k1v, k2v, k3v };
            for (int k = 0; k < 4; k++)
            {
                for (int i = 0; i < bodies.Count; i++)
                {
                    if(i == 0) bodies.ElementAt(i).accel = Vector.Zero;
                    for (int j = i + 1; j < bodies.Count; j++)
                    {
                        if(i == 0) bodies.ElementAt(j).accel = Vector.Zero;
                        float r = Vector.distance(krs[k][i], bodies.ElementAt(j).position);
                        float r3 = 1 / (float)Math.Pow(r, 3);
                        Vector ptr = g * (krs[k][i] - bodies.ElementAt(j).position) * r3;
                        bodies.ElementAt(i).accel += ptr * bodies.ElementAt(j).mass;
                        bodies.ElementAt(j).accel += ptr * bodies.ElementAt(i).mass;
                    }
                    kvs[k][i] = bodies.ElementAt(i).accel;
                    switch(k)
                    {
                        case 0:
                            krs[k][i] = bodies.ElementAt(i).velocity;
                            krs[k + 1][i] = bodies.ElementAt(i).position + k0v[i] * (deltaT / 2);
                            break;
                        case 1:
                            krs[k][i] = bodies.ElementAt(i).velocity + k0v[i] * (deltaT / 2);
                            krs[k + 1][i] = bodies.ElementAt(i).position + k1r[i] * (deltaT / 2);
                            break;
                        case 2:
                            krs[k][i] = bodies.ElementAt(i).velocity + k1v[i] * (deltaT / 2);
                            krs[k + 1][i] = bodies.ElementAt(i).position + k2r[i] * (deltaT);
                            break;
                        case 3:
                            krs[k][i] = bodies.ElementAt(i).velocity + k2v[i] * deltaT;
                            break;
                    }
                }
            }
        }

        public override void move(List<Body> bodies, float deltaT) // http://www.docstubo.net/48209192/Final-paper-uw324-pdf/
        {
            for(int i = 0; i < bodies.Count; i++)
            {
                bodies.ElementAt(i).velocity += (deltaT / 6) * (k0v[i] + 2 * k1v[i] + 2 * k2v[i] + k3v[i]);
                bodies.ElementAt(i).position += (deltaT / 6) * (k0r[i] + 2 * k1r[i] + 2 * k2r[i] + k3r[i]);
                bodies.ElementAt(i).savePoint(deltaT);
            }
        }
    }
}
