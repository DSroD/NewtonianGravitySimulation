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
        private float g;

        public BodySystem(Body[] bodies, float g)
        {
            this.bodies = bodies.ToList();
            this.g = g;
        }

        public void addBody(Body b)
        {
            bodies.Add(b);
        }

        public void setg(float g)
        {
            this.g = g;
        }

        public void removeBody(Body b)
        {
            bodies.Remove(b);
        }

        public void simulateStep(float delta_t)
        {
            foreach(Body b in bodies)
            {
                b.calculateAccel(bodies, g);
            }
            foreach(Body b in bodies)
            {
                b.doStep(delta_t);
            }
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
