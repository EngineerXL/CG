using System;

namespace lab6
{
    public static class Misc
    {
        public const double MAX_RGB = 255;
        public const double EPS = 1e-6;
        public const double INF = 1e18;
        public const double PI_DEG = 180;
        public const double MAX_DEG = 360;

        public static double ToRadians(double deg)
        {
            return Math.PI * deg / PI_DEG;
        }

        public static double NormalizeAngle(double phi)
        {
            if (phi > MAX_DEG) {
                return phi - MAX_DEG;
            }
            if (phi < -MAX_DEG) {
                return phi + MAX_DEG;
            }
            return phi;
        }

        public class Colour
        {
            public double r;
            public double g;
            public double b;

            public Colour()
            {
                r = 0;
                g = 0;
                b = 0;
            }

            public Colour(double _r, double _g, double _b)
            {
                r = _r;
                g = _g;
                b = _b;
            }

            public Colour(Colour other)
            {
                r = other.r;
                g = other.g;
                b = other.b;
            }

            public static Colour operator + (Colour lhs, Colour rhs)
            {
                return new Colour(lhs.r + rhs.r, lhs.g + rhs.g, lhs.b + rhs.b);
            }

            public static Colour operator * (Colour col, double lambda)
            {
                return new Colour(col.r * lambda, col.g * lambda, col.b * lambda);
            }

            public static Colour operator * (double lambda, Colour col)
            {
                return new Colour(col.r * lambda, col.g * lambda, col.b * lambda);
            }

            public static Colour operator / (Colour col, double lambda)
            {
                return new Colour(col.r / lambda, col.g / lambda, col.b / lambda);
            }

            public static Colour operator * (Colour lhs, Colour rhs)
            {
                return new Colour(lhs.r * rhs.r, lhs.g * rhs.g, lhs.b * rhs.b);
            }

            public static bool ApproxEqual(Colour lhs, Colour rhs)
            {
                return Math.Abs(lhs.r - rhs.r) < EPS && Math.Abs(lhs.g - rhs.g) < EPS && Math.Abs(lhs.b - rhs.b) < EPS;
            }

            public void Clamp()
            {
                r = Math.Min(1, Math.Max(0, r));
                g = Math.Min(1, Math.Max(0, g));
                b = Math.Min(1, Math.Max(0, b));
            }

            unsafe public float[] ToFloatArray()
            {
                float[] res = new float[3];
                res[0] = (float)r;
                res[1] = (float)g;
                res[2] = (float)b;
                return res;
            }

            public override String ToString()
            {
                return new String("{" + r.ToString() + ", " + g.ToString() + ", " + b.ToString() + "}");
            }
        }
    }
}
