using System;
using SharpGL;
using System.Collections.Generic;

namespace lab7
{
    public class Curve
    {
        public List<Vector4D> Points = null;
        public int CountPoints = 0;

        public List<Vector4D> Data = null;
        public int CountData = 0;

        private const int STEPS = 100;
        private const double STEP = 0.01;
        private const double POINT_DIST = 0.03;

        private Matrix4D basisMatrix = null;

        public Curve()
        {
            Points = new List<Vector4D>();
            CountPoints = 6;
            Points.Add(new Vector4D(-0.25, 0.5, 0, 0));
            Points.Add(new Vector4D(0.25, 0.5, 0, 0));
            Points.Add(new Vector4D(0.25, 0, 0, 0));
            Points.Add(new Vector4D(-0.25, 0, 0, 0));
            Points.Add(new Vector4D(-0.25, -0.5, 0, 0));
            Points.Add(new Vector4D(0.25, -0.5, 0, 0));
            basisMatrix = createBasisMatrix();
            CalcCurve();
        }

        private Matrix4D createBasisMatrix()
        {
            Matrix4D res = new Matrix4D();
            res[0, 0] = 1;
            res[0, 1] = -3;
            res[0, 2] = 3;
            res[0, 3] = -1;

            res[1, 0] = 4;
            res[1, 1] = 0;
            res[1, 2] = -6;
            res[1, 3] = 3;

            res[2, 0] = 1;
            res[2, 1] = 3;
            res[2, 2] = 3;
            res[2, 3] = -3;

            return res / 6.0;
        }

        public void AddPoint(Vector2D vec)
        {
            Points.Add(new Vector4D(vec.X, vec.Y, 0, 0));
            ++CountPoints;
            CalcCurve();
        }

        Matrix4D ConvertToMatrix(Vector4D v1, Vector4D v2, Vector4D v3, Vector4D v4)
        {
            List<Vector4D> tmp = new List<Vector4D>(4);
            tmp.Add(v1);
            tmp.Add(v2);
            tmp.Add(v3);
            tmp.Add(v4);
            Matrix4D res = new Matrix4D();
            for (int i = 0; i < 4; ++i)
            {
                res[0, i] = tmp[i].X;
                res[1, i] = tmp[i].Y;
                res[2, i] = tmp[i].Z;
                res[3, i] = tmp[i].W;
            }
            return res;
        }

        public void CalcCurve()
        {
            CountData = STEPS * (CountPoints - 3);
            Data = new List<Vector4D>(CountData);
            for (int i = 0; i < CountPoints - 3; ++i)
            {
                Vector4D p0 = Points[i];
                Vector4D p1 = Points[i + 1];
                Vector4D p2 = Points[i + 2];
                Vector4D p3 = Points[i + 3];
                Matrix4D tmp = ConvertToMatrix(p0, p1, p2, p3) * basisMatrix;

                for (int j = 0; j < STEPS; ++j)
                {
                    double t = STEP * j;
                    Vector4D tt = new Vector4D(1, t, Math.Pow(t, 2), Math.Pow(t, 3));
                    Data.Add(tmp * tt);
                }
            }
        }

        public int FindPoint(Vector2D vec)
        {
            Vector4D v = new Vector4D(vec.X, vec.Y, 0, 0);
            for (int i = 0; i < CountPoints; ++i)
            {
                Vector4D p = Points[i];
                double d = (p - v).Len();
                if (d < POINT_DIST)
                {
                    return i;
                }
            }
            return -1;
        }

        public void RemovePoint(int id)
        {
            if (CountPoints > 4) {
                Points.Remove(Points[id]);
                --CountPoints;
                CalcCurve();
            }
        }

        unsafe public void WritePointsData(float * ptr)
        {
            int i = 0;
            foreach (Vector4D p in Points)
            {
                ptr[i + 0] = (float)p.X;
                ptr[i + 1] = (float)p.Y;
                ptr[i + 2] = (float)p.Z;
                i = i + 3;
            }
            foreach (Vector4D p in Data)
            {
                ptr[i + 0] = (float)p.X;
                ptr[i + 1] = (float)p.Y;
                ptr[i + 2] = (float)p.Z;
                i = i + 3;
            }
        }

        unsafe public void WritePointsIndeces(int* ptr)
        {
            for (int i = 0; i < CountPoints + CountData; ++i)
            {
                ptr[i] = i;
            }
        }
    }
}
