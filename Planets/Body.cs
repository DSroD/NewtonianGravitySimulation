using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Planets
{

    public struct Vector
    {
        private float x;
        private float y;

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector operator + (Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y);
        }

        public static Vector operator * (float a, Vector b)
        {
            return new Vector(a * b.x, a * b.y);
        }

        public static Vector operator * (Vector a, float b)
        {
            return b * a;
        }

        public static Vector operator - (Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y);
        }

        public static Vector operator / (Vector a, int b)
        {
            return new Vector(a.x / b, a.y / b);
        }

        public static double scalar(Vector a, Vector b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static double length(Vector v)
        {
            return Math.Sqrt(Math.Pow(v.x, 2) + Math.Pow(v.y, 2));
        }

        public static double distance(Vector a, Vector b)
        {
            return Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));
        }

        public static double angle(Vector a, Vector b)
        {
            double cos = scalar(a, b) / (length(a) * length(b));
            return Math.Acos(cos);
        }

        public static Vector Zero
        {
            get { return new Vector(0, 0); }
        }

        public float X
        {
            get { return x; }
        }

        public float Y
        {
            get { return y; }
        }
    }

    public class Body
    {
        private float mass;
        public Vector accel;
        public Vector tendency;
        public Vector position;
        public Vector prev;
        string name;
        Color c;
        public Vector[] p;
        int maxpoints = 200;
        
        public Body(string name,  float mass, Vector tendency, Vector position, Color c)
        {
            this.c = c;
            this.mass = mass;
            this.tendency = tendency;
            this.position = position;
            this.name = name;
            p = new Vector[0];
        }

        public float Mass
        {
            get { return mass; }
        }

        private Vector[] shiftRight(Vector[] arr, Vector newVal)
        {
            var result = new Vector[arr.Length];
            Array.Copy(arr, 1, result, 0, arr.Length - 1);
            result[arr.Length - 1] = newVal;
            return result;
        }

        private Vector[] addPoint(Vector pt)
        {
            var result = new Vector[p.Length + 1];
            Array.Copy(p, 0, result, 0, p.Length);
            result[result.Length - 1] = pt;
            return result;

        }

        public void doStep(float deltaT)
        {
            prev = position;
            tendency += accel * deltaT;
            position += tendency * deltaT;
            if(p.Length < maxpoints/Vector.length(this.tendency) + 0.9/deltaT + 100*mass)
            {
                p = addPoint(position);
            }
            else
            {
                p = shiftRight(p, position);
            }
        }

        public void calculateAccel(List<Body> bodies, float g)
        {
            this.accel = Vector.Zero;
            foreach (Body b in bodies)
            {
                if(b != this)
                {
                    float r = (float)Math.Sqrt(Math.Pow(position.X - b.position.X, 2) + Math.Pow(position.Y - b.position.Y, 2));
                    float rinv = 1 / (float)Math.Pow(r, 3);
                    float constant = g * b.mass;
                    this.accel += new Vector(-(position.X - b.position.X), -(position.Y-b.position.Y)) * rinv * constant;
                }
            }
        }

        private void ShiftRight<T>(T[] arr, int shifts)
        {
            Array.Copy(arr, 0, arr, shifts, arr.Length - shifts);
            Array.Clear(arr, 0, shifts);
        }

        public Color Color
        {
            get { return c; }
        }

        public string Name
        {
            get { return name; }
        }
    }
}
