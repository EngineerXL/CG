using Cairo;
using System;

namespace lab3
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

            public double this[int i]
            {
                get
                {
                    if (i == 0)
                    {
                        return r;
                    }
                    else if (i == 1)
                    {
                        return g;
                    }
                    else if (i == 2)
                    {
                        return b;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                set
                {
                    if (i == 0)
                    {
                        r = value;
                    }
                    else if (i == 1)
                    {
                        g = value;
                    }
                    else if (i == 2)
                    {
                        b = value;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
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

            public override String ToString()
            {
                return new String("{" + r.ToString() + ", " + g.ToString() + ", " + b.ToString() + "}");
            }

            public Cairo.Color ToCairo()
            {
                return new Cairo.Color(r, g, b);
            }

            public static Colour GenRandColor()
            {
                /* How to Generate Random Colors Programmatically - Martin Ankerl */
                Random rnd = new Random();
                Colour res = new Colour();
                res.HsvToRgb(rnd.NextDouble(), 0.5, 0.99);
                return res;
            }

            private void HsvToRgb(double h, double s, double v)
            {
                int h_ = (int)(h * 6);
                double f = h * 6 - h_;
                double p = v * (1 - s);
                double q = v * (1 - f * s);
                double t = v * (1 - (1 - f) * s);
                switch (h_)
                {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;
                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;
                    case 2:
                        r = p;
                        g = q;
                        b = v;
                        break;
                    case 3:
                        r = q;
                        g = q;
                        b = v;
                        break;
                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = p;
                        b = q;
                        break;
                }
            }
        }
    }
}
