using System;
using System.Collections.Generic;

namespace lab1
{
    public class Vector2D
    {
        public double X;
        public double Y;

        public Vector2D()
        {
            X = 0;
            Y = 0;
        }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2D(Vector2D another)
        {
            X = another.X;
            Y = another.Y;
        }

        public void ToCartesian()
        {
            double ro = X;
            double phi = Y;
            X = ro * Math.Cos(phi);
            Y = ro * Math.Sin(phi);
        }

        public void Rotate(double theta, Vector2D shift)
        {
            double vecX = X;
            double vecY = Y;
            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);
            X = (vecX - shift.X) * cos - (vecY - shift.Y) * sin + shift.X;
            Y = (vecX - shift.X) * sin + (vecY - shift.Y) * cos + shift.Y;
        }

        public static Vector2D operator + (Vector2D lhs, Vector2D rhs)
        {
            return new Vector2D(lhs.X + rhs.X, lhs.Y + rhs.Y);
		}

        public static Vector2D operator - (Vector2D lhs, Vector2D rhs)
        {
            return new Vector2D(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}

        public static Vector2D operator * (Vector2D vec, double lambda)
        {
            return new Vector2D(vec.X * lambda, vec.Y * lambda);
		}

        public static Vector2D operator * (double lambda, Vector2D vec)
        {
            return new Vector2D(vec.X * lambda, vec.Y * lambda);
		}
    }

    public class Misc
    {
        public const double INF = 1e18;

        public static double ToRadians(double deg)
        {
            return Math.PI * deg / 180.0;
        }

        public static List<Vector2D> GenFunctionValues(double a, double k, double B, double step)
        {
            List<Vector2D> res = new List<Vector2D>();
            for (double phi = 0; phi < B; phi = phi + step) {
                res.Add(MyPlotCartesian(phi, a, k));
            }
            res.Add(MyPlotCartesian(B, a, k));
            return res;
        }

        public static Vector2D MyPlotCartesian(double phi, double a, double k)
        {
            double ro = Misc.MyPlotFunction(phi, a, k);
            Vector2D point = new Vector2D(ro, phi);
            point.ToCartesian();
            return point;
        }

        public static double MyPlotFunction(double phi, double a, double k)
        {
            return a * Math.Exp(k * phi);
        }

        public static String NumToString(int i, int degree)
        {
            switch (degree)
            {
                case 0:
                return i.ToString();
                case 1:
                return i.ToString() + "0";
                case 2:
                return i.ToString() + "00";
            }
            return i.ToString() + "e" + degree.ToString();
        }
    }
}
