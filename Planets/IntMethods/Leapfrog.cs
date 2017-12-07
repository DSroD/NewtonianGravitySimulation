using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.IntMethods
{
    class Leapfrog : IntegrationMethod
    {
        private float g;

        public Leapfrog(float G) //Kick - drift - kick metoda
        {
            this.g = G;
        }

        public override void calculateAccelerations(List<Body> bodies, float deltaT)
        {
            Vector[] paccs = new Vector[bodies.Count];
            for(int i = 0; i < bodies.Count; i++)
            {
                paccs[i] = bodies.ElementAt(i).accel; //uloží a_i
                bodies.ElementAt(i).accel = Vector.Zero; //resetuje accel pro vypočtení a_(i+1)
            }
            for(int i = 0; i < bodies.Count; i++)
            {
                for (int j = i+1; j < bodies.Count; j++) //spočte a_(i+1)
                {
                    float r = Vector.distance(bodies.ElementAt(i).position, bodies.ElementAt(j).position);
                    float rinv = 1 / (float)Math.Pow(r, 3); // 1/r^3
                    bodies.ElementAt(i).accel += (bodies.ElementAt(j).position - bodies.ElementAt(i).position)  * rinv * g * bodies.ElementAt(j).mass;
                    bodies.ElementAt(j).accel += new Vector((bodies.ElementAt(i).position.X - bodies.ElementAt(j).position.X), (bodies.ElementAt(i).position.Y - bodies.ElementAt(j).position.Y)) * rinv * g * bodies.ElementAt(i).mass;
                }
                bodies.ElementAt(i).velocity += 0.5f * (paccs[i] + bodies.ElementAt(i).accel) * deltaT;  //v_(i+1) = v_i + (a_i + a_(i+1)) * deltaT/2
            }
        }

        public override void move(List<Body> bodies, float deltaT)
        {
            foreach (Body b in bodies)
            {
                b.position += b.velocity * deltaT + 0.5f * b.accel * (float)Math.Pow(deltaT, 2);
                b.savePoint(deltaT);
            }
        }
    }
}
