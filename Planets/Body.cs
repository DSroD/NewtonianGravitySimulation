using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Planets
{
    #region MATH FUNCTIONS - TODO: VYTVOŘIT SAMOSTATNOU TŘÍDU
    public struct PreciseVector //TODO: přepsat vše do double precision?
    {
        private double x;
        private double y;

        public PreciseVector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static PreciseVector operator +(PreciseVector a, PreciseVector b)
        {
            return new PreciseVector(a.x + b.x, a.y + b.y);
        }

        public static PreciseVector operator -(PreciseVector a, PreciseVector b)
        {
            return new PreciseVector(a.x - b.x, a.y - b.y);
        }

        public static PreciseVector operator /(PreciseVector a, float b)
        {
            return new PreciseVector(a.x / b, a.y / b);
        }

        public Vector toVector()
        {
            return new Vector((float)this.x, (float)this.y);
        }
    }

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

        public static Vector operator / (Vector a, float b)
        {
            return new Vector(a.x / b, a.y / b);
        }

        public static float scalar(Vector a, Vector b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static float length(Vector v)
        {
            return (float)Math.Sqrt(Math.Pow(v.x, 2) + Math.Pow(v.y, 2));
        }

        public static float distance(Vector a, Vector b)
        {
            return (float)Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));
        }

        public static float angle(Vector a, Vector b)
        {
            float cos = scalar(a, b) / (length(a) * length(b));
            return (float)Math.Acos(cos);
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

    #endregion

    public class Body
    {
        public float mass;
        public float radius;
        public Vector accel;
        public Vector velocity;
        public Vector position;
        public Vector prev;
        string name;
        Color c;
        public Vector[] p; //předchozí body pro vykreslení trajektorie
        int maxpoints = 300; //počet bodů pro vykreslení trajektorie
        
        public Body(string name,  float mass, Vector velocity, Vector position, Color c)
        {
            this.c = c;
            this.mass = mass;
            this.velocity = velocity;
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

        public void setMaxPoints(int maxPoints)
        {
            this.maxpoints = maxPoints;
        }

        public void savePoint(float deltaT)
        {
            if (p.Length < maxpoints + 0.1 / deltaT)
            {
                p = addPoint(position);
            }
            else
            {
                p = shiftRight(p, position);
            }
        }

        #region OLD INTEGRATION FUNCTIONS - NO LONGER IN USE

        public void doStep(float deltaT)
        {
            //prev = position;
            position += velocity * deltaT + 0.5f * this.accel * (float)Math.Pow(deltaT, 2);
            savePoint(deltaT);
        }

        public void calculateAccel(List<Body> bodies, float g, float deltaT)
        {
            Vector pacc = this.accel;
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
            this.velocity += 0.5f * (pacc + this.accel) * deltaT;
        }

        #endregion

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
