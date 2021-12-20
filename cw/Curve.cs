using System;
using System.Collections.Generic;

namespace cw
{
    public class Curve
    {
        public List<Vector4D> Points = null;
        public List<double> W = null;
        public int CountPoints = 0;

        public List<Vector4D> Data = null;
        public int CountData = 0;

        private int steps = 0;
        private double step = 0;

        private int DEGREE = 4;
        private const double POINT_DIST = 0.03;

        private List<double> segments = null;
        private int countSegments = 0;

        public Curve(int n, List<Vector4D> controlPoints, List<double> weights)
        {
            steps = n;
            step = 1.0 / (double)steps;
            Points = new List<Vector4D>();
            W = new List<double>();
            for (int i = 0; i < controlPoints.Count; ++i)
            {
                Points.Add(controlPoints[i]);
                W.Add(weights[i]);
            }
            CountPoints = Points.Count;
            CalcCurve();
        }

        public void AddPoint(Vector2D vec)
        {
            Points.Add(new Vector4D(vec.X, vec.Y, 0, 0));
            ++CountPoints;
            CalcCurve();
        }

        private double u(int i)
        {
            return segments[i];
        }

        private double Nip(int i, int p, double t)
        {
            if (p == 0)
            {
                return ((u(i) <= t) && (t < u(i + 1))) ? 1.0 : 0.0;
            }
            else
            {
                return (t - u(i)) / (u(i + p) - u(i)) * Nip(i, p - 1, t) + (u(i + p + 1) - t) / (u(i + p + 1) - u(i + 1)) * Nip(i + 1, p - 1, t);
            }
        }

        private void GenSegments()
        {
            countSegments = CountPoints + DEGREE + 1;
            segments = new List<double>(countSegments);
            for (int i = 0; i < countSegments; ++i)
            {
                segments.Add(i);
            }
        }

        public void CalcCurve()
        {
            GenSegments();
            CountData = steps * (CountPoints - DEGREE + 2);
            Data = new List<Vector4D>(CountData);
            double t = DEGREE - 1;
            for (int i = 0; i < CountPoints - DEGREE + 2; ++i)
            {
                for (int j = 0; j < steps; ++j)
                {
                    Vector4D tt = new Vector4D();
                    double qq = 0;
                    for (int ii = 0; ii < CountPoints; ++ii)
                    {
                        double curNip = Nip(ii, DEGREE, t);
                        tt = tt + curNip * W[ii] * Points[ii];
                        qq = qq + curNip * W[ii];
                    }
                    Data.Add(tt / qq);
                    t += step;
                }
            }
        }
    }
}
