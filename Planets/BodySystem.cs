using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets
{
    public class BodySystem
    {

        private List<Body> bodies;

        public BodySystem(Body[] bodies)
        {
            this.bodies = bodies.ToList();
        }

        public void addBody(Body b)
        {
            bodies.Add(b);
        }

        public void removeBody(Body b)
        {
            bodies.Remove(b);
        }

        public void simulateStep(float delta_t, IntegrationMethod i)
        {
            i.calculateAccelerations(bodies, delta_t);
            i.move(bodies, delta_t);
        }

        public List<Body> Bodies
        {
            get { return bodies; }
        }

        public Body getBody(string name, bool caseDependant)
        {
            foreach(Body b in bodies)
            {
                if(b.Name == name && caseDependant == true)
                {
                    return b;
                }
                else if(b.Name.ToLower() == name.ToLower() && caseDependant == false)
                {
                    return b;
                }
            }
            return null;
        }

    }
}
