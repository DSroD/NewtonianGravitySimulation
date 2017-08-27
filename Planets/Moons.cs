using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets
{
    static class Moons
    {
        public static Vector getPosition(Vector parentPos, float avgdist)
        {
            return parentPos + avgdist * parentPos / Vector.length(parentPos);
        }

        public static Vector getVelocity(Vector parentVelocity, float avgvelocity)
        {
            return parentVelocity + avgvelocity * parentVelocity / Vector.length(parentVelocity);
        }
    }
}
