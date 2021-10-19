using System;
using System.Collections.Generic;

namespace lab3
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

        public static Vector2D operator / (Vector2D vec, double lambda)
        {
            return new Vector2D(vec.X / lambda, vec.Y / lambda);
		}
    }

    public class Vector4D
    {
        public double X;
        public double Y;
        public double Z;
        public double W;

        public Vector4D()
        {
            X = 0;
            Y = 0;
            Z = 0;
            W = 0;
        }

        public Vector4D(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public void Assign(Vector4D another)
        {
            X = another.X;
            Y = another.Y;
            Z = another.Z;
            W = another.W;
        }

        public Vector4D(Vector4D another)
        {
            X = another.X;
            Y = another.Y;
            Z = another.Z;
            W = another.W;
        }

        public bool IsNull()
        {
            return Math.Abs(X) < Misc.EPS && Math.Abs(Y) < Misc.EPS && Math.Abs(Z) < Misc.EPS;
        }

        public static Vector4D operator + (Vector4D lhs, Vector4D rhs)
        {
            return new Vector4D(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);
		}

        public static Vector4D operator - (Vector4D lhs, Vector4D rhs)
        {
            return new Vector4D(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);
		}

        public static Vector4D operator * (Vector4D vec, double lambda)
        {
            return new Vector4D(vec.X * lambda, vec.Y * lambda, vec.Z * lambda, vec.W * lambda);
		}

        public static Vector4D operator / (Vector4D vec, double lambda)
        {
            return new Vector4D(vec.X / lambda, vec.Y / lambda, vec.Z / lambda, vec.W / lambda);
		}

        public static Vector4D operator * (double lambda, Vector4D vec)
        {
            return new Vector4D(vec.X * lambda, vec.Y * lambda, vec.Z * lambda, vec.W * lambda);
		}

        public Vector2D Proj()
        {
            return new Vector2D(X, Y);
        }

        public static double Dot(Vector4D a, Vector4D b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public void Normalize()
        {
            double vecLen = Len();
            X = X / vecLen;
            Y = Y / vecLen;
            Z = Z / vecLen;
        }

        public static double Cos(Vector4D a, Vector4D b)
        {
            return Dot(a, b);
        }

        public double Len()
        {
            return Math.Sqrt(Dot(this, this));
        }

        public static Vector4D Cross(Vector4D a, Vector4D b)
        {
            return new Vector4D(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X, 0);
        }
    }

    public class Matrix4D
    {
        private const int DIM = 4;

        private List< List<double> > _data;

        public Matrix4D()
        {
            _data = new List< List<double> >(4);
            for (int i = 0; i < DIM; ++i)
            {
                List<double> row = new List<double>(4);
                for (int j = 0; j < DIM; ++j)
                {
                    row.Add(0);
                }
                _data.Add(row);
                _data[i][i] = 1;
            }
        }

        public Matrix4D(Matrix4D another) : this()
        {
            for (int i = 0; i < DIM; ++i)
            {
                for (int j = 0; j < DIM; ++j)
                {
                    _data[i][j] = another[i, j];
                }
            }
        }

        public double this[int i, int j]
        {
            get
            {
                return _data[i][j];
            }
            set
            {
                _data[i][j] = value;
            }
        }

        public static Matrix4D operator + (Matrix4D lhs, Matrix4D rhs)
        {
            Matrix4D res = new Matrix4D();
            for (int i = 0; i < DIM; ++i)
            {
                for (int j = 0; j < DIM; ++j)
                {
                    res[i, j] = lhs[i, j] + rhs[i, j];
                }
            }
            return res;
		}

        public static Matrix4D operator - (Matrix4D lhs, Matrix4D rhs)
        {
            Matrix4D res = new Matrix4D();
            for (int i = 0; i < DIM; ++i)
            {
                for (int j = 0; j < DIM; ++j)
                {
                    res[i, j] = lhs[i, j] - rhs[i, j];
                }
            }
            return res;
		}

        public static Matrix4D operator * (Matrix4D lhs, Matrix4D rhs)
        {
            Matrix4D res = new Matrix4D();
            for (int i = 0; i < DIM; ++i)
            {
                for (int j = 0; j < DIM; ++j)
                {
                    res[i, j] = 0;
                    for (int k = 0; k < DIM; ++k)
                    {
                        res[i, j] = res[i, j] + lhs[i, k] * rhs[k, j];
                    }
                }
            }
            return res;
		}

        public static Matrix4D operator * (Matrix4D matr, double lambda)
        {
            Matrix4D res = new Matrix4D();
            for (int i = 0; i < DIM; ++i)
            {
                for (int j = 0; j < DIM; ++j)
                {
                    res[i, j] = matr[i, j] * lambda;
                }
            }
            return res;
		}

        public static Matrix4D operator * (double lambda, Matrix4D matr)
        {
            Matrix4D res = new Matrix4D();
            for (int i = 0; i < DIM; ++i)
            {
                for (int j = 0; j < DIM; ++j)
                {
                    res[i, j] = matr[i, j] * lambda;
                }
            }
            return res;
		}

        public static Vector4D operator * (Vector4D vec, Matrix4D matr)
        {
            List<double> lst = new List<double>(4);
            lst.Add(vec.X);
            lst.Add(vec.Y);
            lst.Add(vec.Z);
            lst.Add(vec.W);
            List<double> res = new List<double>(4);
            for (int i = 0; i < DIM; ++i)
            {
                double summary = 0;
                for (int j = 0; j < DIM; ++j)
                {
                    summary = summary + lst[j] * matr[j, i];
                }
                res.Add(summary);
            }
            return new Vector4D(res[0], res[1], res[2], res[3]);
		}

        public static Vector4D operator * (Matrix4D matr, Vector4D vec)
        {
            List<double> lst = new List<double>(4);
            lst.Add(vec.X);
            lst.Add(vec.Y);
            lst.Add(vec.Z);
            lst.Add(vec.W);
            List<double> res = new List<double>(4);
            for (int i = 0; i < DIM; ++i)
            {
                for (int j = 0; j < DIM; ++i)
                {
                    res[i] = res[i] + lst[j] * matr[j, i];
                }
            }
            return new Vector4D(res[0], res[1], res[2], res[3]);
		}

        public static Matrix4D RotX(double phi)
        {
            Matrix4D res = new Matrix4D();
            res[1, 1] = Math.Cos(phi);
            res[1, 2] = -Math.Sin(phi);
            res[2, 1] = Math.Sin(phi);
            res[2, 2] = Math.Cos(phi);
            return res;
        }

        public static Matrix4D RotY(double phi)
        {
            Matrix4D res = new Matrix4D();
            res[0, 0] = Math.Cos(phi);
            res[0, 2] = Math.Sin(phi);
            res[2, 0] = -Math.Sin(phi);
            res[2, 2] = Math.Cos(phi);
            return res;
        }

        public static Matrix4D RotZ(double phi)
        {
            Matrix4D res = new Matrix4D();
            res[0, 0] = Math.Cos(phi);
            res[0, 1] = -Math.Sin(phi);
            res[1, 0] = Math.Sin(phi);
            res[1, 1] = Math.Cos(phi);
            return res;
        }

        public static Matrix4D ScaleX(double lambda)
        {
            Matrix4D res = new Matrix4D();
            res[0, 0] = lambda;
            return res;
        }

        public static Matrix4D ScaleY(double lambda)
        {
            Matrix4D res = new Matrix4D();
            res[1, 1] = lambda;
            return res;
        }

        public static Matrix4D ScaleZ(double lambda)
        {
            Matrix4D res = new Matrix4D();
            res[2, 2] = lambda;
            return res;
        }

        public static Matrix4D ShiftX(double lambda)
        {
            Matrix4D res = new Matrix4D();
            res[3, 0] = lambda;
            return res;
        }

        public static Matrix4D ShiftY(double lambda)
        {
            Matrix4D res = new Matrix4D();
            res[3, 1] = lambda;
            return res;
        }

        public static Matrix4D ShiftZ(double lambda)
        {
            Matrix4D res = new Matrix4D();
            res[3, 2] = lambda;
            return res;
        }

        public static Matrix4D ProjX()
        {
            Matrix4D res = new Matrix4D();
            res[0, 0] = 0;
            return res;
        }

        public static Matrix4D ProjY()
        {
            Matrix4D res = new Matrix4D();
            res[1, 1] = 0;
            return res;
        }

        public static Matrix4D ProjZ()
        {
            Matrix4D res = new Matrix4D();
            res[2, 2] = 0;
            return res;
        }
    }
}
