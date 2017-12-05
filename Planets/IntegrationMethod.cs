using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets
{
    public abstract class IntegrationMethod
    {
        public abstract void calculateAccelerations(List<Body> bodies, float deltaT);
        public abstract void move(List<Body> bodies, float deltaT);
    }
}
