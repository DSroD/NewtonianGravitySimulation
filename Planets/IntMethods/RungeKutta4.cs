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
        PreciseVector[] k0r;  //k factors
        PreciseVector[] k0v;
        PreciseVector[] k1r;
        PreciseVector[] k1v;
        PreciseVector[] k2r;
        PreciseVector[] k2v;
        PreciseVector[] k3r;
        PreciseVector[] k3v;
        PreciseVector[] hlp;

        public RungeKutta4(float G, int bodyCount)
        {
            this.g = G;
            hlp = new PreciseVector[bodyCount];
            k0r = new PreciseVector[bodyCount];
            k0v = new PreciseVector[bodyCount];
            k1r = new PreciseVector[bodyCount];
            k1v = new PreciseVector[bodyCount];
            k2r = new PreciseVector[bodyCount];
            k2v = new PreciseVector[bodyCount];
            k3r = new PreciseVector[bodyCount];
            k3v = new PreciseVector[bodyCount];
        }


        public override void calculateAccelerations(List<Body> bodies, float deltaT)
        {

            for (int i = 0; i < bodies.Count; i++)
            {
                hlp[i] = bodies.ElementAt(i).position;
                k0r[i] = PreciseVector.Zero;
                k0v[i] = PreciseVector.Zero;
                k1r[i] = PreciseVector.Zero;
                k1v[i] = PreciseVector.Zero;
                k2r[i] = PreciseVector.Zero;
                k2v[i] = PreciseVector.Zero;
                k3r[i] = PreciseVector.Zero;
                k3v[i] = PreciseVector.Zero;

            }
            PreciseVector[][] krs = { k0r, k1r, k2r, k3r };
            PreciseVector[][] kvs = { k0v, k1v, k2v, k3v };
            for (int k = 0; k < 4; k++)
            {
                for (int i = 0; i < bodies.Count; i++)
                {
                    bodies.ElementAt(i).accel = Vector.Zero;
                    for (int j = 0; j < bodies.Count; j++)
                    {
                        if (i != j)
                        {
                            double r = PreciseVector.distance(hlp[i], hlp[j]);
                            float r3 = 1 / (float)Math.Abs(Math.Pow(r, 3));
                            PreciseVector ptr = g * (hlp[j] - hlp[i]) * r3;
                            bodies.ElementAt(i).accel += (Vector)ptr * (bodies.ElementAt(j).mass);
                        }
                    }
                    kvs[k][i] = bodies.ElementAt(i).accel;
                    switch (k)
                    {
                        case 0:
                            krs[k][i] = bodies.ElementAt(i).velocity;
                            hlp[i] = bodies.ElementAt(i).position + k0r[i] * (deltaT / 2); //připravení pro výpočet k+1
                            break;
                        case 1:
                            krs[k][i] = bodies.ElementAt(i).velocity + k0v[i] * (deltaT / 2);
                            hlp[i] = bodies.ElementAt(i).position + k1r[i] * (deltaT / 2);
                            break;
                        case 2:
                            krs[k][i] = bodies.ElementAt(i).velocity + k1v[i] * (deltaT / 2);
                            hlp[i] = bodies.ElementAt(i).position + k2r[i] * (deltaT);
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
                bodies.ElementAt(i).velocity += (Vector)((deltaT / 6) * (k0v[i] + 2 * k1v[i] + 2 * k2v[i] + k3v[i]));
                bodies.ElementAt(i).position += (Vector)((deltaT / 6) * (k0r[i] + 2 * k1r[i] + 2 * k2r[i] + k3r[i]));
                bodies.ElementAt(i).savePoint(deltaT);
            }
        }
    }
}
