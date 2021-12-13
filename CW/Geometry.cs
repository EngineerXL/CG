using System;
using System.Collections.Generic;

namespace cw
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

        public double Len()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public static bool ApproxEqual(Vector2D a, Vector2D b)
        {
            return Math.Abs(a.X - b.X) < Misc.EPS && Math.Abs(a.Y - b.Y) < Misc.EPS;
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

        unsafe public float[] ToFloatArray()
        {
            float[] res = new float[3];
            res[0] = (float)X;
            res[1] = (float)Y;
            res[2] = (float)Z;
            return res;
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

        public override String ToString()
        {
            return new String("{" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + "}");
        }

        public static bool ApproxEqual(Vector4D a, Vector4D b)
        {
            return Math.Abs(a.X - b.X) < Misc.EPS && Math.Abs(a.Y - b.Y) < Misc.EPS && Math.Abs(a.Z - b.Z) < Misc.EPS;
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

        private List< List<double> > Data;

        public Matrix4D()
        {
            Data = new List< List<double> >(4);
            for (int i = 0; i < DIM; ++i)
            {
                List<double> row = new List<double>(4);
                for (int j = 0; j < DIM; ++j)
                {
                    row.Add(0);
                }
                Data.Add(row);
                Data[i][i] = 1;
            }
        }

        public Matrix4D(Matrix4D another) : this()
        {
            for (int i = 0; i < DIM; ++i)
            {
                for (int j = 0; j < DIM; ++j)
                {
                    Data[i][j] = another[i, j];
                }
            }
        }

        public double this[int i, int j]
        {
            get
            {
                return Data[i][j];
            }
            set
            {
                Data[i][j] = value;
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
                double res_i = 0;
                for (int j = 0; j < DIM; ++j)
                {
                    res_i = res_i + lst[j] * matr[i, j];
                }
                res.Add(res_i);
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

        public static Matrix4D RotFromAxisAngle(Vector4D axis, double phi)
        {
            double cos = Math.Cos(phi);
            double sin = Math.Sin(phi);
            double x = axis.X;
            double y = axis.Y;
            double z = axis.Z;
            Matrix4D res = new Matrix4D();
            res[0, 0] = cos + (1 - cos) * x * x;
            res[1, 1] = cos + (1 - cos) * y * y;
            res[2, 2] = cos + (1 - cos) * z * z;
            res[0, 1] = (1 - cos) * x * y - sin * z;
            res[0, 2] = (1 - cos) * x * z + sin * y;
            res[1, 0] = (1 - cos) * y * x + sin * z;
            res[1, 2] = (1 - cos) * y * z - sin * x;
            res[2, 0] = (1 - cos) * z * x - sin * y;
            res[2, 1] = (1 - cos) * z * y + sin * x;
            return res;
        }

        public static Matrix4D Scale(double x, double y, double z)
        {
            Matrix4D res = new Matrix4D();
            res[0, 0] = x;
            res[1, 1] = y;
            res[2, 2] = z;
            return res;
        }

        public static Matrix4D Shift(double dx, double dy, double dz)
        {
            Matrix4D res = new Matrix4D();
            res[0, 3] = dx;
            res[1, 3] = dy;
            res[2, 3] = dz;
            return res;
        }

        unsafe public float[] ToFloatArray()
        {
            float[] res = new float[16];
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    res[4 * i + j] = (float)Data[i][j];
                }
            }
            return res;
        }
    }
}
