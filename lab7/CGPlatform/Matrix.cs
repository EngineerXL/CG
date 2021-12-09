using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CGPlatform.Internal;

namespace CGPlatform.Internal
{
    internal static class __CONST
    {
        internal const string VectorX_Name = "[0] X";
        internal const string VectorX_Desc = "First element of vector";
        internal const string VectorY_Name = "[1] Y";
        internal const string VectorY_Desc = "Second element of vector";
        internal const string VectorZ_Name = "[2] Z";
        internal const string VectorZ_Desc = "Third element of vector";
        internal const string VectorW_Name = "[3] W";
        internal const string VectorW_Desc = "Forth element of vector";
    }
}

namespace CGPlatform
{
    using Math = System.Math;

    

    [Serializable]
    [TypeConverter(typeof(ValueTypeTypeConverter<Vector2f>))]
    [StructLayout(LayoutKind.Explicit, Size = __SIZE)]
	public struct Vector2f : ICloneable, IEquatable<Vector2f>, IFormattable
	{
        public const int __SIZE = 8;
		[FieldOffset( 0)]  public float X;
		[FieldOffset( 4)]  public float Y;

        #region Конструкторы

        public Vector2f(float x, float y) {
			X = x;  Y = y;
		}

		public Vector2f(float[] coordinates) {
			Debug.Assert(coordinates != null && coordinates.Length == 2);
			X = coordinates[0];  Y = coordinates[1];
		}

		public Vector2f(List<float> coordinates) {
			Debug.Assert(coordinates != null && coordinates.Count == 2);
			X = coordinates[0];  Y = coordinates[1];
		}

        public Vector2f(decimal[] elements) : this(elements.Select(e => (float)e).ToArray()) { }

		public Vector2f(Vector2f vector) {
			X = vector.X;  Y = vector.Y;
		}

        public Vector2f(PointF point) {
            X = point.X; Y = point.Y;
		}

        public Vector2f(Point point) {
            X = point.X; Y = point.Y;
		}
        #endregion

        #region Константы

        /// <summary>
        /// Получает вектор, три элемента которого равны нулю.
		/// </summary>
        public static readonly Vector2f Zero  = new Vector2f(0f, 0f);
        /// <summary>
		/// Получает вектор, три элемента которого равны единице.
		/// </summary>
        public static readonly Vector2f One   = new Vector2f(1f, 1f);
		/// <summary>
		/// Получает вектор (1,0).
		/// </summary>
        public static readonly Vector2f UnitX = new Vector2f(1f, 0f);
		/// <summary>
		/// Получает вектор (0,1).
		/// </summary>
        public static readonly Vector2f UnitY = new Vector2f(0f, 1f);
        #endregion

        #region Статические методы

        public static Vector2f Add(Vector2f left, Vector2f right) {
            return new Vector2f(left.X + right.X, left.Y + right.Y);
		}

        public static Vector2f Add(Vector2f vector, float scalar) {
            return new Vector2f(vector.X + scalar, vector.Y + scalar);
		}

        public static void Add(Vector2f left, Vector2f right, ref Vector2f result) {
			result.X = left.X + right.X;
			result.Y = left.Y + right.Y;
		}

        public static void Add(Vector2f vector, float scalar, ref Vector2f result) {
			result.X = vector.X + scalar;
			result.Y = vector.Y + scalar;
		}

        public static Vector2f Subtract(Vector2f left, Vector2f right) {
            return new Vector2f(left.X - right.X, left.Y - right.Y);
		}

        public static Vector2f Subtract(Vector2f vector, float scalar) {
            return new Vector2f(vector.X - scalar, vector.Y - scalar);
		}

        public static Vector2f Subtract(float scalar, Vector2f vector) {
            return new Vector2f(scalar - vector.X, scalar - vector.Y);
		}

        public static void Subtract(Vector2f left, Vector2f right, ref Vector2f result) {
			result.X = left.X - right.X;
			result.Y = left.Y - right.Y;
		}

        public static void Subtract(Vector2f vector, float scalar, ref Vector2f result) {
			result.X = vector.X - scalar;
			result.Y = vector.Y - scalar;
		}

        public static void Subtract(float scalar, Vector2f vector, ref Vector2f result) {
			result.X = scalar - vector.X;
			result.Y = scalar - vector.Y;
		}

        public static Vector2f Divide(Vector2f left, Vector2f right) {
            return new Vector2f(left.X / right.X, left.Y / right.Y);
		}

        public static Vector2f Divide(Vector2f vector, float scalar) {
            return new Vector2f(vector.X / scalar, vector.Y / scalar);
		}

        public static Vector2f Divide(float scalar, Vector2f vector) {
            return new Vector2f(scalar / vector.X, scalar / vector.Y);
		}

        public static void Divide(Vector2f left, Vector2f right, ref Vector2f result) {
			result.X = left.X / right.X;
			result.Y = left.Y / right.Y;
		}

        public static void Divide(Vector2f vector, float scalar, ref Vector2f result) {
			result.X = vector.X / scalar;
			result.Y = vector.Y / scalar;
		}

        public static void Divide(float scalar, Vector2f vector, ref Vector2f result) {
			result.X = scalar / vector.X;
			result.Y = scalar / vector.Y;
		}

        public static Vector2f Multiply(Vector2f vector, float scalar) {
            return new Vector2f(vector.X * scalar, vector.Y * scalar);
		}

        public static void Multiply(Vector2f vector, float scalar, ref Vector2f result) {
			result.X = vector.X * scalar;
			result.Y = vector.Y * scalar;
		}

        public static Vector2f Multiply(Vector2f left, Vector2f right) {
            return new Vector2f(left.X * right.X, left.Y * right.Y);
        }

        public static void Multiply(ref Vector2f left, Vector2f right) {
            left.X *= right.X;
            left.Y *= right.Y;
        }

        public static float CrossProduct(Vector2f left, Vector2f right) {
            return (left.X * right.Y) - (left.Y * right.X);
        }

        public static float DotProduct(Vector2f left, Vector2f right) {
			return (left.X * right.X) + (left.Y * right.Y);
		}

        public static Vector2f Negate(Vector2f vector) {
            return new Vector2f(-vector.X, -vector.Y);
		}

        public static bool ApproxEqual(Vector2f left, Vector2f right) {
			return ApproxEqual(left, right, Single.Epsilon);
		}

        public static bool ApproxEqual(Vector2f left, Vector2f right, double tolerance) {
			return ((System.Math.Abs(left.X - right.X) <= tolerance) &&
				    (System.Math.Abs(left.Y - right.Y) <= tolerance) );
		}

        /// <summary>
        /// Угол между векторами vec1 и vec2 в диапазоне (-π; +π]<para/>
        /// Угол больше нуля, если vec1 повернут по часовой стрелке по отношению к vec2<para/>
        /// и меньше нуля в обратном случае (имеется ввиду наименьший угол на который<para/>
        /// надо повернуть вектор vec2, чтобы он совпал по направлению с вектором vec1).
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <returns></returns>
        public static float GetAngle(Vector2f vec1, Vector2f vec2) {
            return (float)Math.Atan2(CrossProduct(vec2, vec1), DotProduct(vec2, vec1));
        }

        /// <summary>
        /// Точка пересечения двух прямых (не отрезков!) на плоскости в декартовой системе координат<para/>
        /// Если две прямые не пересекаются, то компоненты возвращаемого вектора будут не действительными<para/>
        /// числами, равными ±Infinity или NaN в случае если прямые параллельны оси. Например точка<para/>
        /// (NaN, ±Infinity) паралельна оси Y.
        /// </summary>
        /// <param name="l1_p1">Первая точка, принадлежащей первой прямой</param>
        /// <param name="l1_p2">Вторая точка, принадлежащей первой прямой</param>
        /// <param name="l2_p1">Первая точка, принадлежащей второй прямой</param>
        /// <param name="l2_p2">Вторая точка, принадлежащей второй прямой</param>
        public static Vector2f GetLinesCross(Vector2f l1_p1, Vector2f l1_p2, Vector2f l2_p1, Vector2f l2_p2) {
            // Уравнение прямой в декартовой системе координат: A∙⒳ + B∙⒴ + C = 0
            var l1 = new Vector3f(l1_p1, 1) * new Vector3f(l1_p2, 1); // l₁X∙⒳ + l₁Y∙⒴ + l₁Z = 0
            var l2 = new Vector3f(l2_p1, 1) * new Vector3f(l2_p2, 1); // l₂X∙⒳ + l₂Y∙⒴ + l₂Z = 0
            var lc = l1 * l2;
            return (Vector2f)lc / lc.Z;
        }
		#endregion

        #region Свойства и Методы

        [DisplayName(__CONST.VectorX_Name)]
        [Description(__CONST.VectorX_Desc)]
        public float ValueX {
            get { return X; }
            set { X = value; }
	    }

        [DisplayName(__CONST.VectorY_Name)]
        [Description(__CONST.VectorY_Desc)]
        public float ValueY {
            get { return Y; }
            set { Y = value; }
	    }

        public bool IsReal() {
            return !Single.IsInfinity(X) && !Single.IsNaN(X) &&
                   !Single.IsInfinity(Y) && !Single.IsNaN(Y);
        }

	    public Vector2f Multiply(Vector2f vector) {
	        return Multiply(this, vector);
	    }

        public Vector2f Multiplied(Vector2f vector) {
	        Multiply(ref this, vector);
            return this;
        }

        public double DotProduct(Vector2f vector) {
            return DotProduct(this, vector);
	    }

        public void Normalize() {
			float length = GetLength();
			if (length == 0)
                throw new DivideByZeroException("Невозможно нормализовать вектор нулевой длинны");
			X /= length;
			Y /= length;
		}

	    public Vector2f Normalized() {
	        Normalize();
	        return this;
	    }

		public float GetLength() {
			return (float)System.Math.Sqrt(X * X + Y * Y);
		}

		public double GetLengthSquared() {
			return (X * X + Y * Y);
		}

		public void ClampZero(float tolerance) {
            X = (tolerance > Math.Abs(X)) ? 0 : X;
            Y = (tolerance > Math.Abs(Y)) ? 0 : Y;
		}

		public void ClampZero() {
            X = (Double.Epsilon > Math.Abs(X)) ? 0 : X;
            Y = (Double.Epsilon > Math.Abs(Y)) ? 0 : Y;
		}

        public bool ApproxEqual(Vector2f vector) {
            return ApproxEqual(this, vector);
        }

        public bool ApproxEqual(Vector2f vector, float tolerance) {
            return ApproxEqual(this, vector, tolerance);
        }

        public float[] ToArray() {
	        return new float[2] { X, Y };
	    }

        public float[] ToFloatArray() {
            return ToArray().Select(v => (float)v).ToArray();
	    }

        public decimal[] ToDecimalArray() {
            return ToArray().Select(v => (decimal)v).ToArray();
	    }

        public PointF ToPointF() {
            return new PointF((float)X, (float)Y);
        }

        public Point ToPoint() {
            return new Point((int)X, (int)Y);
        }

        public Point ToPointRound() {
            return new Point((int)Math.Round(X), (int)Math.Round(Y));
        }

        public Point ToPointFloor() {
            return new Point((int)Math.Floor(X), (int)Math.Floor(Y));
        }

        public Point ToPointCeiling() {
            return new Point((int)Math.Ceiling(X), (int)Math.Ceiling(Y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public Vector3f ToVector3f(float z) { unchecked {
	        return new Vector3f( X, Y, z );
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public Vector4f ToVector4f(float z, float w) { unchecked {
	        return new Vector4f( X, Y, z, w );
	    }}
        #endregion

        #region Реализация интерфейса ICloneable

        object ICloneable.Clone() {
            return new Vector2f(this);
        }

        public Vector2f Clone() {
            return new Vector2f(this);
        }
        #endregion

        #region Реализация интерфейса IEquatable<Vector4>

        bool IEquatable<Vector2f>.Equals(Vector2f v) {
            return (X == v.X) && (Y == v.Y);
        }

        public bool Equals(Vector2f v) {
            return (X == v.X) && (Y == v.Y);
        }
        #endregion

        #region Реализация интерфейса IFormattable

        public string ToString(string format) {
            return String.Format("({0:}, {1})", X.ToString(format, null),
                Y.ToString(format, null));
        }

        string IFormattable.ToString(string format, IFormatProvider provider) {
            return ToString(format, provider);
        }

        public string ToString(string format, IFormatProvider provider) {
            return String.Format("({0:}, {1})", X.ToString(format, provider), 
                Y.ToString(format, provider));
        }
        #endregion

        #region Перегрузки

        public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public override bool Equals(object obj) {
            if (obj is Vector3f) {
                Vector3f v = (Vector3f)obj;
				return (X == v.X) && (Y == v.Y);
			}
			if (obj is Vector4f) {
                Vector4f v = (Vector4f)obj;
				return (X == v.X) && (Y == v.Y);
			}
			return false;
		}

		public override string ToString() {
			return string.Format("({0:}, {1})", X, Y);
		}
		#endregion

        #region Операторы

        public static bool operator ==(Vector2f left, Vector2f right) {
            return ((left.X == right.X) && (left.Y == right.Y));
		}

        public static bool operator !=(Vector2f left, Vector2f right) {
            return ((left.X != right.X) || (left.Y != right.Y));
		}

		public static bool operator >(Vector2f left, Vector2f right) {
			return ((left.X > right.X) && (left.Y > right.Y));
		}

        public static bool operator <(Vector2f left, Vector2f right) {
			return ((left.X < right.X) && (left.Y < right.Y));
		}

        public static bool operator >=(Vector2f left, Vector2f right) {
			return ((left.X >= right.X) && (left.Y >= right.Y));
		}

        public static bool operator <=(Vector2f left, Vector2f right) {
			return ((left.X <= right.X) && (left.Y <= right.Y));
		}

        public static Vector2f operator -(Vector2f vector) {
            return Vector2f.Negate(vector);
		}

        public static Vector2f operator +(Vector2f left, Vector2f right) {
            return Vector2f.Add(left, right);
		}

        public static Vector2f operator +(Vector2f vector, float scalar) {
            return Vector2f.Add(vector, scalar);
		}

        public static Vector2f operator +(float scalar, Vector2f vector) {
            return Vector2f.Add(vector, scalar);
		}

        public static Vector2f operator -(Vector2f left, Vector2f right) {
            return Vector2f.Subtract(left, right);
		}

		public static Vector2f operator -(Vector2f vector, float scalar) {
            return Vector2f.Subtract(vector, scalar);
		}

        public static Vector2f operator -(float scalar, Vector2f vector) {
            return Vector2f.Subtract(scalar, vector);
		}

		public static Vector2f operator *(Vector2f vector, float scalar) {
            return Vector2f.Multiply(vector, scalar);
		}

        public static Vector2f operator *(float scalar, Vector2f vector) {
            return Vector2f.Multiply(vector, scalar);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator ^(Vector2f left, Vector2f right) { unchecked {
            return Vector2f.Multiply(right, left);
		}}

        public static Vector2f operator /(Vector2f vector, float scalar) {
            return Vector2f.Divide(vector, scalar);
		}

        public static Vector2f operator /(float scalar, Vector2f vector) {
            return Vector2f.Divide(scalar, vector);
		}

        public float this[int index] {
			get { switch (index) {
					case  0: return X;
					case  1: return Y;
					default: throw new IndexOutOfRangeException();
			}}
			set { switch (index) {
					case  0: X = value; break;
					case  1: Y = value; break;
					default: throw new IndexOutOfRangeException();
			}}
		}

		public static explicit operator float[](Vector2f vector) {
            return new float[] { vector.X, vector.Y };
		}

		public static explicit operator List<float>(Vector2f vector) {
			List<float> list = new List<float>(2);
			list.Add(vector.X);  list.Add(vector.Y);  
			return list;
		}

		public static explicit operator LinkedList<float>(Vector2f vector) {
			LinkedList<float> list = new LinkedList<float>();
			list.AddLast(vector.X);  list.AddLast(vector.Y); 
			return list;
		}

        public static explicit operator PointF(Vector2f vector) {
            return new PointF((float)vector.X, (float)vector.Y);
		}

        public static explicit operator Point(Vector2f vector) {
            return new Point((int)vector.X, (int)vector.Y);
		}

        public static explicit operator Vector2d(Vector2f vector) {
            return new Vector2d((double)vector.X, (double)vector.Y);
		}
        #endregion
    }
    

    [Serializable]
    [TypeConverter(typeof(ValueTypeTypeConverter<Vector2d>))]
    [StructLayout(LayoutKind.Explicit, Size = __SIZE)]
	public struct Vector2d : ICloneable, IEquatable<Vector2d>, IFormattable
	{
        public const int __SIZE = 16;
		[FieldOffset( 0)]  public double X;
		[FieldOffset( 8)]  public double Y;

        #region Конструкторы
        
        public Vector2d(double x, double y) {
			X = x;  Y = y;
		}

		public Vector2d(double[] coordinates) {
			Debug.Assert(coordinates != null && coordinates.Length == 2);
			X = coordinates[0];  Y = coordinates[1];
		}

		public Vector2d(List<double> coordinates) {
			Debug.Assert(coordinates != null && coordinates.Count == 2);
			X = coordinates[0];  Y = coordinates[1];
		}

        public Vector2d(decimal[] elements) : this(elements.Select(e => (double)e).ToArray()) { }

		public Vector2d(Vector2d vector) {
			X = vector.X;  Y = vector.Y;
		}

        public Vector2d(PointF point) {
            X = point.X; Y = point.Y;
		}

        public Vector2d(Point point) {
            X = point.X; Y = point.Y;
		}
        #endregion

        #region Константы

        /// <summary>
        /// Получает вектор, три элемента которого равны нулю.
		/// </summary>
        public static readonly Vector2d Zero  = new Vector2d(0d, 0d);
        /// <summary>
		/// Получает вектор, три элемента которого равны единице.
		/// </summary>
        public static readonly Vector2d One   = new Vector2d(1d, 1d);
		/// <summary>
		/// Получает вектор (1,0).
		/// </summary>
        public static readonly Vector2d UnitX = new Vector2d(1d, 0d);
		/// <summary>
		/// Получает вектор (0,1).
		/// </summary>
        public static readonly Vector2d UnitY = new Vector2d(0d, 1d);
        #endregion

        #region Статические методы

        public static Vector2d Add(Vector2d left, Vector2d right) {
            return new Vector2d(left.X + right.X, left.Y + right.Y);
		}

        public static Vector2d Add(Vector2d vector, double scalar) {
            return new Vector2d(vector.X + scalar, vector.Y + scalar);
		}

        public static void Add(Vector2d left, Vector2d right, ref Vector2d result) {
			result.X = left.X + right.X;
			result.Y = left.Y + right.Y;
		}

        public static void Add(Vector2d vector, double scalar, ref Vector2d result) {
			result.X = vector.X + scalar;
			result.Y = vector.Y + scalar;
		}

        public static Vector2d Subtract(Vector2d left, Vector2d right) {
            return new Vector2d(left.X - right.X, left.Y - right.Y);
		}

        public static Vector2d Subtract(Vector2d vector, double scalar) {
            return new Vector2d(vector.X - scalar, vector.Y - scalar);
		}

        public static Vector2d Subtract(double scalar, Vector2d vector) {
            return new Vector2d(scalar - vector.X, scalar - vector.Y);
		}

        public static void Subtract(Vector2d left, Vector2d right, ref Vector2d result) {
			result.X = left.X - right.X;
			result.Y = left.Y - right.Y;
		}

        public static void Subtract(Vector2d vector, double scalar, ref Vector2d result) {
			result.X = vector.X - scalar;
			result.Y = vector.Y - scalar;
		}

        public static void Subtract(double scalar, Vector2d vector, ref Vector2d result) {
			result.X = scalar - vector.X;
			result.Y = scalar - vector.Y;
		}

        public static Vector2d Divide(Vector2d left, Vector2d right) {
            return new Vector2d(left.X / right.X, left.Y / right.Y);
		}

        public static Vector2d Divide(Vector2d vector, double scalar) {
            return new Vector2d(vector.X / scalar, vector.Y / scalar);
		}

        public static Vector2d Divide(double scalar, Vector2d vector) {
            return new Vector2d(scalar / vector.X, scalar / vector.Y);
		}

        public static void Divide(Vector2d left, Vector2d right, ref Vector2d result) {
			result.X = left.X / right.X;
			result.Y = left.Y / right.Y;
		}

        public static void Divide(Vector2d vector, double scalar, ref Vector2d result) {
			result.X = vector.X / scalar;
			result.Y = vector.Y / scalar;
		}

        public static void Divide(double scalar, Vector2d vector, ref Vector2d result) {
			result.X = scalar / vector.X;
			result.Y = scalar / vector.Y;
		}

        public static Vector2d Multiply(Vector2d vector, double scalar) {
            return new Vector2d(vector.X * scalar, vector.Y * scalar);
		}

        public static void Multiply(Vector2d vector, double scalar, ref Vector2d result) {
			result.X = vector.X * scalar;
			result.Y = vector.Y * scalar;
		}

        public static Vector2d Multiply(Vector2d left, Vector2d right) {
            return new Vector2d(left.X * right.X, left.Y * right.Y);
        }

        public static void Multiply(ref Vector2d left, Vector2d right) {
            left.X *= right.X;
            left.Y *= right.Y;
        }

        public static double CrossProduct(Vector2d left, Vector2d right) {
            return (left.X * right.Y) - (left.Y * right.X);
        }

        public static double DotProduct(Vector2d left, Vector2d right) {
			return (left.X * right.X) + (left.Y * right.Y);
		}

        public static Vector2d Negate(Vector2d vector) {
            return new Vector2d(-vector.X, -vector.Y);
		}

        public static bool ApproxEqual(Vector2d left, Vector2d right) {
			return ApproxEqual(left, right, Double.Epsilon);
		}

        public static bool ApproxEqual(Vector2d left, Vector2d right, double tolerance) {
			return ((System.Math.Abs(left.X - right.X) <= tolerance) &&
				    (System.Math.Abs(left.Y - right.Y) <= tolerance) );
		}
        
        /// <summary>
        /// Угол между векторами vec1 и vec2 в диапазоне (-π; +π]<para/>
        /// Угол больше нуля, если vec1 повернут по часовой стрелке по отношению к vec2<para/>
        /// и меньше нуля в обратном случае (имеется ввиду наименьший угол на который<para/>
        /// надо повернуть вектор vec2, чтобы он совпал по направлению с вектором vec1).
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <returns></returns>
        public static double GetAngle(Vector2d vec1, Vector2d vec2) {
            return Math.Atan2(CrossProduct(vec2, vec1), DotProduct(vec2, vec1));
        }

        /// <summary>
        /// Точка пересечения двух прямых (не отрезков!) на плоскости в декартовой системе координат<para/>
        /// Если две прямые не пересекаются, то компоненты возвращаемого вектора будут не действительными<para/>
        /// числами, равными ±Infinity или NaN в случае если прямые параллельны оси. Например точка<para/>
        /// (NaN, ±Infinity) паралельна оси Y.
        /// </summary>
        /// <param name="l1_p1">Первая точка, принадлежащей первой прямой</param>
        /// <param name="l1_p2">Вторая точка, принадлежащей первой прямой</param>
        /// <param name="l2_p1">Первая точка, принадлежащей второй прямой</param>
        /// <param name="l2_p2">Вторая точка, принадлежащей второй прямой</param>
        public static Vector2d GetLinesCross(Vector2d l1_p1, Vector2d l1_p2, Vector2d l2_p1, Vector2d l2_p2) {
            // Уравнение прямой в декартовой системе координат: A∙⒳ + B∙⒴ + C = 0
            var l1 = new Vector3d(l1_p1, 1) * new Vector3d(l1_p2, 1); // l₁X∙⒳ + l₁Y∙⒴ + l₁Z = 0
            var l2 = new Vector3d(l2_p1, 1) * new Vector3d(l2_p2, 1); // l₂X∙⒳ + l₂Y∙⒴ + l₂Z = 0
            var lc = l1 * l2;
            return (Vector2d)lc / lc.Z;
        }

		#endregion

        #region Свойства и Методы

        [DisplayName(__CONST.VectorX_Name)]
        [Description(__CONST.VectorX_Desc)]
        public double ValueX {
            get { return X; }
            set { X = value; }
	    }

        [DisplayName(__CONST.VectorY_Name)]
        [Description(__CONST.VectorY_Desc)]
        public double ValueY {
            get { return Y; }
            set { Y = value; }
	    }

        public bool IsReal() {
            return !Double.IsInfinity(X) && !Double.IsNaN(X) &&
                   !Double.IsInfinity(Y) && !Double.IsNaN(Y);
        }

	    public Vector2d Multiply(Vector2d vector) {
	        return Multiply(this, vector);
	    }

        public Vector2d Multiplied(Vector2d vector) {
	        Multiply(ref this, vector);
            return this;
        }

        public double DotProduct(Vector2d vector) {
            return DotProduct(this, vector);
	    }

        public void Normalize() {
			double length = GetLength();
			if (length == 0)
                throw new DivideByZeroException("Невозможно нормализовать вектор нулевой длинны");
			X /= length;
			Y /= length;
		}

	    public Vector2d Normalized() {
	        Normalize();
	        return this;
	    }

		public double GetLength() {
			return System.Math.Sqrt(X * X + Y * Y);
		}

		public double GetLengthSquared() {
			return (X * X + Y * Y);
		}

		public void ClampZero(double tolerance) {
            X = (tolerance > Math.Abs(X)) ? 0 : X;
            Y = (tolerance > Math.Abs(Y)) ? 0 : Y;
		}

		public void ClampZero() {
            X = (Double.Epsilon > Math.Abs(X)) ? 0 : X;
            Y = (Double.Epsilon > Math.Abs(Y)) ? 0 : Y;
		}

        public bool ApproxEqual(Vector2d vector) {
            return ApproxEqual(this, vector);
        }

        public bool ApproxEqual(Vector2d vector, double tolerance) {
            return ApproxEqual(this, vector, tolerance);
        }

        public double[] ToArray() {
	        return new double[2] { X, Y };
	    }

        public float[] ToFloatArray() {
            return ToArray().Select(v => (float)v).ToArray();
	    }

        public decimal[] ToDecimalArray() {
            return ToArray().Select(v => (decimal)v).ToArray();
	    }

        public PointF ToPointF() {
            return new PointF((float)X, (float)Y);
        }

        public Point ToPoint() {
            return new Point((int)X, (int)Y);
        }

        public Point ToPointRound() {
            return new Point((int)Math.Round(X), (int)Math.Round(Y));
        }

        public Point ToPointFloor() {
            return new Point((int)Math.Floor(X), (int)Math.Floor(Y));
        }

        public Point ToPointCeiling() {
            return new Point((int)Math.Ceiling(X), (int)Math.Ceiling(Y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public Vector3d ToDVector3(double z) { unchecked {
	        return new Vector3d( X, Y, z );
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public Vector4d ToDVector4(double z, double w) { unchecked {
	        return new Vector4d( X, Y, z, w );
	    }}
        #endregion

        #region Реализация интерфейса ICloneable

        object ICloneable.Clone() {
            return new Vector2d(this);
        }

        public Vector2d Clone() {
            return new Vector2d(this);
        }
        #endregion

        #region Реализация интерфейса IEquatable<Vector4>

        bool IEquatable<Vector2d>.Equals(Vector2d v) {
            return (X == v.X) && (Y == v.Y);
        }

        public bool Equals(Vector2d v) {
            return (X == v.X) && (Y == v.Y);
        }
        #endregion

        #region Реализация интерфейса IFormattable

        public string ToString(string format) {
            return String.Format("({0:}, {1})", X.ToString(format, null),
                Y.ToString(format, null));
        }

        string IFormattable.ToString(string format, IFormatProvider provider) {
            return ToString(format, provider);
        }

        public string ToString(string format, IFormatProvider provider) {
            return String.Format("({0:}, {1})", X.ToString(format, provider), 
                Y.ToString(format, provider));
        }
        #endregion

        #region Перегрузки

        public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public override bool Equals(object obj) {
            if (obj is Vector3d) {
                Vector3d v = (Vector3d)obj;
				return (X == v.X) && (Y == v.Y);
			}
			if (obj is Vector4d) {
                Vector4d v = (Vector4d)obj;
				return (X == v.X) && (Y == v.Y);
			}
			return false;
		}

		public override string ToString() {
			return string.Format("({0:}, {1})", X, Y);
		}
		#endregion

        #region Операторы

        public static bool operator ==(Vector2d left, Vector2d right) {
            return ((left.X == right.X) && (left.Y == right.Y));
		}

        public static bool operator !=(Vector2d left, Vector2d right) {
            return ((left.X != right.X) || (left.Y != right.Y));
		}

		public static bool operator >(Vector2d left, Vector2d right) {
			return ((left.X > right.X) && (left.Y > right.Y));
		}

        public static bool operator <(Vector2d left, Vector2d right) {
			return ((left.X < right.X) && (left.Y < right.Y));
		}

        public static bool operator >=(Vector2d left, Vector2d right) {
			return ((left.X >= right.X) && (left.Y >= right.Y));
		}

        public static bool operator <=(Vector2d left, Vector2d right) {
			return ((left.X <= right.X) && (left.Y <= right.Y));
		}

        public static Vector2d operator -(Vector2d vector) {
            return Vector2d.Negate(vector);
		}

        public static Vector2d operator +(Vector2d left, Vector2d right) {
            return Vector2d.Add(left, right);
		}

        public static Vector2d operator +(Vector2d vector, double scalar) {
            return Vector2d.Add(vector, scalar);
		}

        public static Vector2d operator +(double scalar, Vector2d vector) {
            return Vector2d.Add(vector, scalar);
		}

        public static Vector2d operator -(Vector2d left, Vector2d right) {
            return Vector2d.Subtract(left, right);
		}

		public static Vector2d operator -(Vector2d vector, double scalar) {
            return Vector2d.Subtract(vector, scalar);
		}

        public static Vector2d operator -(double scalar, Vector2d vector) {
            return Vector2d.Subtract(scalar, vector);
		}

		public static Vector2d operator *(Vector2d vector, double scalar) {
            return Vector2d.Multiply(vector, scalar);
		}

        public static Vector2d operator *(double scalar, Vector2d vector) {
            return Vector2d.Multiply(vector, scalar);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator ^(Vector2d left, Vector2d right) { unchecked {
            return Vector2d.Multiply(right, left);
		}}

        public static Vector2d operator /(Vector2d vector, double scalar) {
            return Vector2d.Divide(vector, scalar);
		}

        public static Vector2d operator /(double scalar, Vector2d vector) {
            return Vector2d.Divide(scalar, vector);
		}

        public double this[int index] {
			get { switch (index) {
					case  0: return X;
					case  1: return Y;
					default: throw new IndexOutOfRangeException();
			}}
			set { switch (index) {
					case  0: X = value; break;
					case  1: Y = value; break;
					default: throw new IndexOutOfRangeException();
			}}
		}

		public static explicit operator double[](Vector2d vector) {
            return new double[] { vector.X, vector.Y };
		}

		public static explicit operator List<double>(Vector2d vector) {
			List<double> list = new List<double>(2);
			list.Add(vector.X);  list.Add(vector.Y);  
			return list;
		}

		public static explicit operator LinkedList<double>(Vector2d vector) {
			LinkedList<double> list = new LinkedList<double>();
			list.AddLast(vector.X);  list.AddLast(vector.Y); 
			return list;
		}

        public static explicit operator PointF(Vector2d vector) {
            return new PointF((float)vector.X, (float)vector.Y);
		}

        public static explicit operator Point(Vector2d vector) {
            return new Point((int)vector.X, (int)vector.Y);
		}

        public static explicit operator Vector2f(Vector2d vector) {
            return new Vector2f((float)vector.X, (float)vector.Y);
		}
        #endregion
    }


    [Serializable]
    [TypeConverter(typeof(ValueTypeTypeConverter<Vector3f>))]
    [StructLayout(LayoutKind.Explicit, Size = __SIZE)]
	public struct Vector3f : ICloneable, IEquatable<Vector3f>, IFormattable
	{
        public const int __SIZE = 12;
		[FieldOffset( 0)]  public float X;
		[FieldOffset( 4)]  public float Y;
		[FieldOffset( 8)]  public float Z;

        #region Конструкторы
        
        public Vector3f(float x, float y, float z) {
			X = x;  Y = y;   Z = z;
		}

		public Vector3f(float[] coordinates) {
			Debug.Assert(coordinates != null && coordinates.Length == 3);
			X = coordinates[0];  Y = coordinates[1];  Z = coordinates[2];
		}

		public Vector3f(List<float> coordinates) {
			Debug.Assert(coordinates != null && coordinates.Count == 3);
			X = coordinates[0];  Y = coordinates[1];  Z = coordinates[2];
		}

        public Vector3f(decimal[] elements) : this(elements.Select(e => (float)e).ToArray()) { }

        public Vector3f(Vector2f vector, float z) {
			X = vector.X;  Y = vector.Y;  Z = z;
		}

		public Vector3f(Vector3f vector) {
			X = vector.X;  Y = vector.Y;  Z = vector.Z;
		}
        #endregion

        #region Константы

        /// <summary>
        /// Получает вектор, три элемента которого равны нулю.
		/// </summary>
        public static readonly Vector3f Zero  = new Vector3f(0f, 0f, 0f);
        /// <summary>
		/// Получает вектор, три элемента которого равны единице.
		/// </summary>
        public static readonly Vector3f One   = new Vector3f(1f, 1f, 1f);
		/// <summary>
		/// Получает вектор (1,0,0).
		/// </summary>
        public static readonly Vector3f UnitX = new Vector3f(1f, 0f, 0f);
		/// <summary>
		/// Получает вектор (0,1,0).
		/// </summary>
        public static readonly Vector3f UnitY = new Vector3f(0f, 1f, 0f);
		/// <summary>
		/// Получает вектор (0,0,1).
		/// </summary>
        public static readonly Vector3f UnitZ = new Vector3f(0f, 0f, 1f);
        #endregion

        #region Статические методы

        public static Vector3f Add(Vector3f left, Vector3f right) {
            return new Vector3f(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}

        public static Vector3f Add(Vector3f vector, float scalar) {
            return new Vector3f(vector.X + scalar, vector.Y + scalar, vector.Z + scalar);
		}

        public static void Add(Vector3f left, Vector3f right, ref Vector3f result) {
			result.X = left.X + right.X;
			result.Y = left.Y + right.Y;
			result.Z = left.Z + right.Z;
		}

        public static void Add(Vector3f vector, float scalar, ref Vector3f result) {
			result.X = vector.X + scalar;
			result.Y = vector.Y + scalar;
			result.Z = vector.Z + scalar;
		}

        public static Vector3f Subtract(Vector3f left, Vector3f right) {
            return new Vector3f(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		}

        public static Vector3f Subtract(Vector3f vector, float scalar) {
            return new Vector3f(vector.X - scalar, vector.Y - scalar, vector.Z - scalar);
		}

        public static Vector3f Subtract(float scalar, Vector3f vector) {
            return new Vector3f(scalar - vector.X, scalar - vector.Y, scalar - vector.Z);
		}

        public static void Subtract(Vector3f left, Vector3f right, ref Vector3f result) {
			result.X = left.X - right.X;
			result.Y = left.Y - right.Y;
			result.Z = left.Z - right.Z;
		}

        public static void Subtract(Vector3f vector, float scalar, ref Vector3f result) {
			result.X = vector.X - scalar;
			result.Y = vector.Y - scalar;
			result.Z = vector.Z - scalar;
		}

        public static void Subtract(float scalar, Vector3f vector, ref Vector3f result) {
			result.X = scalar - vector.X;
			result.Y = scalar - vector.Y;
			result.Z = scalar - vector.Z;
		}

        public static Vector3f Divide(Vector3f left, Vector3f right) {
            return new Vector3f(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
		}

        public static void Divide(ref Vector3f left, Vector3f right) {
            left.X /= right.X;
            left.Y /= right.Y;
            left.Z /= right.Z;
        }

        public static Vector3f Divide(Vector3f vector, float scalar) {
            return new Vector3f(vector.X / scalar, vector.Y / scalar, vector.Z / scalar);
		}

        public static Vector3f Divide(float scalar, Vector3f vector) {
            return new Vector3f(scalar / vector.X, scalar / vector.Y, scalar / vector.Z);
		}

        public static void Divide(Vector3f left, Vector3f right, ref Vector3f result) {
			result.X = left.X / right.X;
			result.Y = left.Y / right.Y;
			result.Z = left.Z / right.Z;
		}

        public static void Divide(Vector3f vector, float scalar, ref Vector3f result) {
			result.X = vector.X / scalar;
			result.Y = vector.Y / scalar;
			result.Z = vector.Z / scalar;
		}

        public static void Divide(float scalar, Vector3f vector, ref Vector3f result) {
			result.X = scalar / vector.X;
			result.Y = scalar / vector.Y;
			result.Z = scalar / vector.Z;
		}

        public static Vector3f Multiply(Vector3f vector, float scalar) {
            return new Vector3f(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
		}

        public static void Multiply(Vector3f vector, float scalar, ref Vector3f result) {
			result.X = vector.X * scalar;
			result.Y = vector.Y * scalar;
			result.Z = vector.Z * scalar;
		}

        public static Vector3f Multiply(Vector3f left, Vector3f right) {
            return new Vector3f(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        public static void Multiply(ref Vector3f left, Vector3f right) {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
        }

        public static float DotProduct(Vector3f left, Vector3f right) {
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
		}

        public static Vector3f CrossProduct(Vector3f left, Vector3f right) {
            return new Vector3f(left.Y * right.Z - left.Z * right.Y,
				                left.Z * right.X - left.X * right.Z,
				                left.X * right.Y - left.Y * right.X);
		}

        public static void CrossProduct(Vector3f left, Vector3f right, ref Vector3f result) {
			result.X = left.Y * right.Z - left.Z * right.Y;
			result.Y = left.Z * right.X - left.X * right.Z;
			result.Z = left.X * right.Y - left.Y * right.X;
		}

        public static Vector4f CrossProduct(Vector4f left, Vector4f right) {
            return new Vector4f(left.Y * right.Z - left.Z * right.Y,    // По сути это тоже вектороное произведение 3х
				                left.Z * right.X - left.X * right.Z,    // мерных векторов, представленных как 4х мерные
				                left.X * right.Y - left.Y * right.X,    // (последнее значение просто игнорируется)
                                0);
		}

        public static void CrossProduct(Vector4f left, Vector4f right, ref Vector4f result) {
			result.X = left.Y * right.Z - left.Z * right.Y;
			result.Y = left.Z * right.X - left.X * right.Z;
			result.Z = left.X * right.Y - left.Y * right.X;
            result.W = 0;
        }

        public static Vector3f Negate(Vector3f vector) {
            return new Vector3f(-vector.X, -vector.Y, -vector.Z);
		}

        /// <summary>
        /// Отражает вектор от плоскости, заданной нормалью.
        /// </summary>
        /// <param name="vector">вектор, входящий в плоскость</param>
        /// <param name="normal">Вектор нормали к плоскости плоскость, направленный наружу.</param>
        /// <returns>Вектор равный по величене vector, но с отраженным направлением</returns>
	    public static Vector3f Reflect(Vector3f vector, Vector3f normal) {                          //  vector    ^
            // Из свойства векторного сложения vector - reflect = удвоенной проекции vector на normal      |     / surface
            // Т.к. вектор normal нормализованный, то dot(vector,normal) = |vector|*cos(vector,normal)     |   /   normal
            // что соответсвует модулю проекции. А произведение модуля проекции на вектор normal даст \\   | /   
            // саму проецию. Таким образом получается reflect = vector - 2*dot(vector,normal) * normal  \\ V-------->    
            Multiply(normal, 2 * DotProduct(vector, normal), ref normal);                           //    \\    reflected
	        Subtract(vector, normal, ref vector);                                                   //      \\  
	        return vector;                                                                          //        \\ surface
	    }

        public static bool ApproxEqual(Vector3f left, Vector3f right) {
			return ApproxEqual(left, right, Single.Epsilon);
		}

        public static bool ApproxEqual(Vector3f left, Vector3f right, float tolerance) {
			return ((System.Math.Abs(left.X - right.X) <= tolerance) &&
				    (System.Math.Abs(left.Y - right.Y) <= tolerance) &&
				    (System.Math.Abs(left.Z - right.Z) <= tolerance) );
		}
		#endregion

        #region Свойства и Методы

        [DisplayName(__CONST.VectorX_Name)]
        [Description(__CONST.VectorX_Desc)]
        public float ValueX {
            get { return X; }
            set { X = value; }
	    }

        [DisplayName(__CONST.VectorY_Name)]
        [Description(__CONST.VectorY_Desc)]
        public float ValueY {
            get { return Y; }
            set { Y = value; }
	    }

        [DisplayName(__CONST.VectorZ_Name)]
        [Description(__CONST.VectorZ_Desc)]
        public float ValueZ {
            get { return Z; }
            set { Z = value; }
	    }

        public bool IsReal() {
            return !Single.IsInfinity(X) && !Single.IsNaN(X) &&
                   !Single.IsInfinity(Y) && !Single.IsNaN(Y) &&
                   !Single.IsInfinity(Z) && !Single.IsNaN(Z);
        }

	    public Vector3f Multiply(Vector3f vector) {
	        return Multiply(this, vector);
	    }

        public Vector3f Multiplied(Vector3f vector) {
	        Multiply(ref this, vector);
            return this;
        }

        public Vector3f Divide(Vector3f vector) {
            return Divide(this, vector);
	    }

        public Vector3f Divided(Vector3f vector) {
            Divide(ref this, vector);
            return this;
        }

        public float DotProduct(Vector3f vector) {
            return DotProduct(this, vector);
	    }

	    /// <summary>
	    /// Отражает данный вектор, входящий в плоскость, заданной нормалью.
	    /// </summary>
	    /// <param name="vector">вектор, входящий в плоскость</param>
	    /// <param name="normal">Вектор нормали к плоскости плоскость, направленный наружу.</param>
	    /// <returns>Вектор равный по величене, но с отраженным направлением</returns>
	    public Vector3f Reflect(Vector3f normal) {
	        return Reflect(this, normal);
	    }

        public void Normalize() {
			float length = GetLength();
			if (length == 0)
                throw new DivideByZeroException("Невозможно нормализовать вектор нулевой длинны");
			X /= length;
			Y /= length;
			Z /= length;
		}

	    public Vector3f Normalized() {
	        Normalize();
	        return this;
	    }

		public float GetLength() {
			return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z);
		}

		public float GetLengthSquared() {
			return (X * X + Y * Y + Z * Z);
		}

		public void ClampZero(float tolerance) {
            X = (tolerance > Math.Abs(X)) ? 0 : X;
            Y = (tolerance > Math.Abs(Y)) ? 0 : Y;
            Z = (tolerance > Math.Abs(Z)) ? 0 : Z;
		}

		public void ClampZero() {
            X = (Double.Epsilon > Math.Abs(X)) ? 0 : X;
            Y = (Double.Epsilon > Math.Abs(Y)) ? 0 : Y;
            Z = (Double.Epsilon > Math.Abs(Z)) ? 0 : Z;
		}

        public bool ApproxEqual(Vector3f vector) {
            return ApproxEqual(this, vector);
        }

        public bool ApproxEqual(Vector3f vector, float tolerance) {
            return ApproxEqual(this, vector, tolerance);
        }

        public float[] ToArray() {
	        return new float[3] { X, Y, Z };
	    }

        public float[] ToFloatArray() {
            return ToArray().Select(v => (float)v).ToArray();
	    }

        public decimal[] ToDecimalArray() {
            return ToArray().Select(v => (decimal)v).ToArray();
	    }

         public Vector4f ToVector4f(float W) { unchecked {
	        return new Vector4f( X, Y, Z, W );
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public Vector4d ToVector4d(double W) { unchecked {
	        return new Vector4d( X, Y, Z, W );
	    }}
        #endregion

        #region Реализация интерфейса ICloneable

        object ICloneable.Clone() {
            return new Vector3f(this);
        }

        public Vector3f Clone() {
            return new Vector3f(this);
        }
        #endregion

        #region Реализация интерфейса IEquatable<Vector4>

        bool IEquatable<Vector3f>.Equals(Vector3f v) {
            return (X == v.X) && (Y == v.Y) && (Z == v.Z);
        }

        public bool Equals(Vector3f v) {
            return (X == v.X) && (Y == v.Y) && (Z == v.Z);
        }
        #endregion

        #region Реализация интерфейса IFormattable

        public string ToString(string format) {
            return String.Format("({0}, {1}, {2})", X.ToString(format, null),
                Y.ToString(format, null), Z.ToString(format, null));
        }

        string IFormattable.ToString(string format, IFormatProvider provider) {
            return ToString(format, provider);
        }

        public string ToString(string format, IFormatProvider provider) {
            return String.Format("({0:}, {1}, {2})", X.ToString(format, provider), 
                Y.ToString(format, provider), Z.ToString(format, provider));
        }
        #endregion

        #region Перегрузки

        public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

		public override bool Equals(object obj) {
			if (obj is Vector4d) {
                Vector4d v = (Vector4d)obj;
				return (X == v.X) && (Y == v.Y) && (Z == v.Z);
			}
			return false;
		}

		public override string ToString() {
			return string.Format("({0:}, {1}, {2})", X, Y, Z);
		}
		#endregion

        #region Операторы

        public static bool operator ==(Vector3f left, Vector3f right) {
			return ValueType.Equals(left, right);
		}

        public static bool operator !=(Vector3f left, Vector3f right) {
			return !ValueType.Equals(left, right);
		}

		public static bool operator >(Vector3f left, Vector3f right) {
			return ((left.X > right.X) && (left.Y > right.Y) && (left.Z > right.Z));
		}

        public static bool operator <(Vector3f left, Vector3f right) {
			return ((left.X < right.X) && (left.Y < right.Y) && (left.Z < right.Z));
		}

        public static bool operator >=(Vector3f left, Vector3f right) {
			return ((left.X >= right.X) && (left.Y >= right.Y) && (left.Z >= right.Z));
		}

        public static bool operator <=(Vector3f left, Vector3f right) {
			return ((left.X <= right.X) && (left.Y <= right.Y) && (left.Z <= right.Z));
		}

        public static Vector3f operator -(Vector3f vector) {
            return Vector3f.Negate(vector);
		}

        public static Vector3f operator +(Vector3f left, Vector3f right) {
            return Vector3f.Add(left, right);
		}

        public static Vector3f operator +(Vector3f vector, float scalar) {
            return Vector3f.Add(vector, scalar);
		}

        public static Vector3f operator +(float scalar, Vector3f vector) {
            return Vector3f.Add(vector, scalar);
		}

        public static Vector3f operator -(Vector3f left, Vector3f right) {
            return Vector3f.Subtract(left, right);
		}

		public static Vector3f operator -(Vector3f vector, float scalar) {
            return Vector3f.Subtract(vector, scalar);
		}

        public static Vector3f operator -(float scalar, Vector3f vector) {
            return Vector3f.Subtract(scalar, vector);
		}

		public static Vector3f operator *(Vector3f vector, float scalar) {
            return Vector3f.Multiply(vector, scalar);
		}

        public static Vector3f operator *(float scalar, Vector3f vector) {
            return Vector3f.Multiply(vector, scalar);
		}

        public static Vector3f operator *(Vector3f left, Vector3f right) {
            return Vector3f.CrossProduct(left, right);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f operator ^(Vector3f left, Vector3f right) { unchecked {
            return Vector3f.Multiply(right, left); ;
		}}

        public static Vector3f operator /(Vector3f vector, float scalar) {
            return Vector3f.Divide(vector, scalar);
		}

        public static Vector3f operator /(float scalar, Vector3f vector) {
            return Vector3f.Divide(scalar, vector);
		}

        public float this[int index] {
			get { switch (index) {
					case  0: return X;
					case  1: return Y;
					case  2: return Z;
					default: throw new IndexOutOfRangeException();
			}}
			set { switch (index) {
					case  0: X = value; break;
					case  1: Y = value; break;
					case  2: Z = value; break;
					default: throw new IndexOutOfRangeException();
			}}
		}

		public static explicit operator float[](Vector3f vector) {
            return new float[] { vector.X, vector.Y, vector.Z };
		}

		public static explicit operator List<float>(Vector3f vector) {
			List<float> list = new List<float>(3);
			list.Add(vector.X);  list.Add(vector.Y);  list.Add(vector.Z);
			return list;
		}

		public static explicit operator LinkedList<float>(Vector3f vector) {
			LinkedList<float> list = new LinkedList<float>();
			list.AddLast(vector.X);  list.AddLast(vector.Y);  list.AddLast(vector.Z);
			return list;
		}

        public static explicit operator Vector2f(Vector3f vector) {
            return new Vector2f(vector.X, vector.Y);
		}

        public static explicit operator Vector2d(Vector3f vector) {
            return new Vector2d((double)vector.X, (double)vector.Y);
		}
        #endregion
    }


	[Serializable]
    [TypeConverter(typeof(ValueTypeTypeConverter<Vector3d>))]
	[StructLayout(LayoutKind.Explicit, Size = __SIZE)]
	public struct Vector3d : ICloneable, IEquatable<Vector3d>, IFormattable
	{
        public const int __SIZE = 24;
		[FieldOffset( 0)]  public double X;
		[FieldOffset( 8)]  public double Y;
		[FieldOffset(16)]  public double Z;

        #region Конструкторы
        
        public Vector3d(double x, double y, double z) {
			X = x;  Y = y;   Z = z;
		}

		public Vector3d(double[] coordinates) {
			Debug.Assert(coordinates != null && coordinates.Length == 3);
			X = coordinates[0];  Y = coordinates[1];  Z = coordinates[2];
		}

		public Vector3d(List<double> coordinates) {
			Debug.Assert(coordinates != null && coordinates.Count == 3);
			X = coordinates[0];  Y = coordinates[1];  Z = coordinates[2];
		}

        public Vector3d(decimal[] elements) : this(elements.Select(e => (double)e).ToArray()) { }

        public Vector3d(Vector2d vector, double z) {
			X = vector.X;  Y = vector.Y;  Z = z;
		}

		public Vector3d(Vector3d vector) {
			X = vector.X;  Y = vector.Y;  Z = vector.Z;
		}
        #endregion

        #region Константы

        /// <summary>
        /// Получает вектор, три элемента которого равны нулю.
		/// </summary>
        public static readonly Vector3d Zero  = new Vector3d(0d, 0d, 0d);
        /// <summary>
		/// Получает вектор, три элемента которого равны единице.
		/// </summary>
        public static readonly Vector3d One   = new Vector3d(1d, 1d, 1d);
		/// <summary>
		/// Получает вектор (1,0,0).
		/// </summary>
        public static readonly Vector3d UnitX = new Vector3d(1d, 0d, 0d);
		/// <summary>
		/// Получает вектор (0,1,0).
		/// </summary>
        public static readonly Vector3d UnitY = new Vector3d(0d, 1d, 0d);
		/// <summary>
		/// Получает вектор (0,0,1).
		/// </summary>
        public static readonly Vector3d UnitZ = new Vector3d(0d, 0d, 1d);
        #endregion

        #region Статические методы

        public static Vector3d Add(Vector3d left, Vector3d right) {
            return new Vector3d(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}

        public static Vector3d Add(Vector3d vector, double scalar) {
            return new Vector3d(vector.X + scalar, vector.Y + scalar, vector.Z + scalar);
		}

        public static void Add(Vector3d left, Vector3d right, ref Vector3d result) {
			result.X = left.X + right.X;
			result.Y = left.Y + right.Y;
			result.Z = left.Z + right.Z;
		}

        public static void Add(Vector3d vector, double scalar, ref Vector3d result) {
			result.X = vector.X + scalar;
			result.Y = vector.Y + scalar;
			result.Z = vector.Z + scalar;
		}

        public static Vector3d Subtract(Vector3d left, Vector3d right) {
            return new Vector3d(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		}

        public static Vector3d Subtract(Vector3d vector, double scalar) {
            return new Vector3d(vector.X - scalar, vector.Y - scalar, vector.Z - scalar);
		}

        public static Vector3d Subtract(double scalar, Vector3d vector) {
            return new Vector3d(scalar - vector.X, scalar - vector.Y, scalar - vector.Z);
		}

        public static void Subtract(Vector3d left, Vector3d right, ref Vector3d result) {
			result.X = left.X - right.X;
			result.Y = left.Y - right.Y;
			result.Z = left.Z - right.Z;
		}

        public static void Subtract(Vector3d vector, double scalar, ref Vector3d result) {
			result.X = vector.X - scalar;
			result.Y = vector.Y - scalar;
			result.Z = vector.Z - scalar;
		}

        public static void Subtract(double scalar, Vector3d vector, ref Vector3d result) {
			result.X = scalar - vector.X;
			result.Y = scalar - vector.Y;
			result.Z = scalar - vector.Z;
		}

        public static Vector3d Divide(Vector3d left, Vector3d right) {
            return new Vector3d(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
		}

        public static Vector3d Divide(Vector3d vector, double scalar) {
            return new Vector3d(vector.X / scalar, vector.Y / scalar, vector.Z / scalar);
		}

        public static Vector3d Divide(double scalar, Vector3d vector) {
            return new Vector3d(scalar / vector.X, scalar / vector.Y, scalar / vector.Z);
		}

        public static void Divide(Vector3d left, Vector3d right, ref Vector3d result) {
			result.X = left.X / right.X;
			result.Y = left.Y / right.Y;
			result.Z = left.Z / right.Z;
		}

        public static void Divide(Vector3d vector, double scalar, ref Vector3d result) {
			result.X = vector.X / scalar;
			result.Y = vector.Y / scalar;
			result.Z = vector.Z / scalar;
		}

        public static void Divide(double scalar, Vector3d vector, ref Vector3d result) {
			result.X = scalar / vector.X;
			result.Y = scalar / vector.Y;
			result.Z = scalar / vector.Z;
		}

        public static Vector3d Multiply(Vector3d vector, double scalar) {
            return new Vector3d(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
		}

        public static void Multiply(Vector3d vector, double scalar, ref Vector3d result) {
			result.X = vector.X * scalar;
			result.Y = vector.Y * scalar;
			result.Z = vector.Z * scalar;
		}

        public static Vector3d Multiply(Vector3d left, Vector3d right) {
            return new Vector3d(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        public static void Multiply(ref Vector3d left, Vector3d right) {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
        }

        public static double DotProduct(Vector3d left, Vector3d right) {
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
		}

        public static Vector3d CrossProduct(Vector3d left, Vector3d right) {
            return new Vector3d(left.Y * right.Z - left.Z * right.Y,
				                left.Z * right.X - left.X * right.Z,
				                left.X * right.Y - left.Y * right.X);
		}

        public static void CrossProduct(Vector3d left, Vector3d right, ref Vector3d result) {
			result.X = left.Y * right.Z - left.Z * right.Y;
			result.Y = left.Z * right.X - left.X * right.Z;
			result.Z = left.X * right.Y - left.Y * right.X;
		}

        public static Vector4d CrossProduct(Vector4d left, Vector4d right) {
            return new Vector4d(left.Y * right.Z - left.Z * right.Y,    // По сути это тоже вектороное произведение 3х
				                left.Z * right.X - left.X * right.Z,    // мерных векторов, представленных как 4х мерные
				                left.X * right.Y - left.Y * right.X,    // (последнее значение просто игнорируется)
                                0);
		}

        public static void CrossProduct(Vector4d left, Vector4d right, ref Vector4d result) {
			result.X = left.Y * right.Z - left.Z * right.Y;
			result.Y = left.Z * right.X - left.X * right.Z;
			result.Z = left.X * right.Y - left.Y * right.X;
            result.W = 0;
        }

        public static Vector3d Negate(Vector3d vector) {
            return new Vector3d(-vector.X, -vector.Y, -vector.Z);
		}

        /// <summary>
        /// Отражает вектор от плоскости, заданной нормалью.
        /// </summary>
        /// <param name="vector">вектор, входящий в плоскость</param>
        /// <param name="normal">Вектор нормали к плоскости плоскость, направленный наружу.</param>
        /// <returns>Вектор равный по величене vector, но с отраженным направлением</returns>
	    public static Vector3d Reflect(Vector3d vector, Vector3d normal) {                          //  vector    ^
            // Из свойства векторного сложения vector - reflect = удвоенной проекции vector на normal      |     / surface
            // Т.к. вектор normal нормализованный, то dot(vector,normal) = |vector|*cos(vector,normal)     |   /   normal
            // что соответсвует модулю проекции. А произведение модуля проекции на вектор normal даст \\   | /   
            // саму проецию. Таким образом получается reflect = vector - 2*dot(vector,normal) * normal  \\ V-------->    
            Multiply(normal, 2 * DotProduct(vector, normal), ref normal);                           //    \\    reflected
	        Subtract(vector, normal, ref vector);                                                   //      \\  
	        return vector;                                                                          //        \\ surface
	    }

        public static bool ApproxEqual(Vector3d left, Vector3d right) {
			return ApproxEqual(left, right, Double.Epsilon);
		}

        public static bool ApproxEqual(Vector3d left, Vector3d right, double tolerance) {
			return ((System.Math.Abs(left.X - right.X) <= tolerance) &&
				    (System.Math.Abs(left.Y - right.Y) <= tolerance) &&
				    (System.Math.Abs(left.Z - right.Z) <= tolerance) );
		}
		#endregion

        #region Свойства и Методы

        [DisplayName(__CONST.VectorX_Name)]
        [Description(__CONST.VectorX_Desc)]
        public double ValueX {
            get { return X; }
            set { X = value; }
	    }

        [DisplayName(__CONST.VectorY_Name)]
        [Description(__CONST.VectorY_Desc)]
        public double ValueY {
            get { return Y; }
            set { Y = value; }
	    }

        [DisplayName(__CONST.VectorZ_Name)]
        [Description(__CONST.VectorZ_Desc)]
        public double ValueZ {
            get { return Z; }
            set { Z = value; }
	    }

        public bool IsReal() {
            return !Double.IsInfinity(X) && !Double.IsNaN(X) &&
                   !Double.IsInfinity(Y) && !Double.IsNaN(Y) &&
                   !Double.IsInfinity(Z) && !Double.IsNaN(Z);
        }

	    public Vector3d Multiply(Vector3d vector) {
	        return Multiply(this, vector);
	    }

        public Vector3d Multiplied(Vector3d vector) {
	        Multiply(ref this, vector);
            return this;
        }

        public double DotProduct(Vector3d vector) {
            return DotProduct(this, vector);
	    }

	    /// <summary>
	    /// Отражает данный вектор, входящий в плоскость, заданной нормалью.
	    /// </summary>
	    /// <param name="vector">вектор, входящий в плоскость</param>
	    /// <param name="normal">Вектор нормали к плоскости плоскость, направленный наружу.</param>
	    /// <returns>Вектор равный по величене, но с отраженным направлением</returns>
	    public Vector3d Reflect(Vector3d normal) {
	        return Reflect(this, normal);
	    }

        public void Normalize() {
			double length = GetLength();
			if (length == 0)
                throw new DivideByZeroException("Невозможно нормализовать вектор нулевой длинны");
			X /= length;
			Y /= length;
			Z /= length;
		}

	    public Vector3d Normalized() {
	        Normalize();
	        return this;
	    }

		public double GetLength() {
			return System.Math.Sqrt(X * X + Y * Y + Z * Z);
		}

		public double GetLengthSquared() {
			return (X * X + Y * Y + Z * Z);
		}

		public void ClampZero(double tolerance) {
            X = (tolerance > Math.Abs(X)) ? 0 : X;
            Y = (tolerance > Math.Abs(Y)) ? 0 : Y;
            Z = (tolerance > Math.Abs(Z)) ? 0 : Z;
		}

		public void ClampZero() {
            X = (Double.Epsilon > Math.Abs(X)) ? 0 : X;
            Y = (Double.Epsilon > Math.Abs(Y)) ? 0 : Y;
            Z = (Double.Epsilon > Math.Abs(Z)) ? 0 : Z;
		}

        public bool ApproxEqual(Vector3d vector) {
            return ApproxEqual(this, vector);
        }

        public bool ApproxEqual(Vector3d vector, double tolerance) {
            return ApproxEqual(this, vector, tolerance);
        }

        public double[] ToArray() {
	        return new double[3] { X, Y, Z };
	    }

        public float[] ToFloatArray() {
            return ToArray().Select(v => (float)v).ToArray();
	    }

        public decimal[] ToDecimalArray() {
            return ToArray().Select(v => (decimal)v).ToArray();
	    }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public Vector4d ToDVector4(double W) { unchecked {
	        return new Vector4d( X, Y, Z, W );
	    }}
        #endregion

        #region Реализация интерфейса ICloneable

        object ICloneable.Clone() {
            return new Vector3d(this);
        }

        public Vector3d Clone() {
            return new Vector3d(this);
        }
        #endregion

        #region Реализация интерфейса IEquatable<Vector4>

        bool IEquatable<Vector3d>.Equals(Vector3d v) {
            return (X == v.X) && (Y == v.Y) && (Z == v.Z);
        }

        public bool Equals(Vector3d v) {
            return (X == v.X) && (Y == v.Y) && (Z == v.Z);
        }
        #endregion

        #region Реализация интерфейса IFormattable

        public string ToString(string format) {
            return String.Format("({0}, {1}, {2})", X.ToString(format, null),
                Y.ToString(format, null), Z.ToString(format, null));
        }

        string IFormattable.ToString(string format, IFormatProvider provider) {
            return ToString(format, provider);
        }

        public string ToString(string format, IFormatProvider provider) {
            return String.Format("({0:}, {1}, {2})", X.ToString(format, provider), 
                Y.ToString(format, provider), Z.ToString(format, provider));
        }
        #endregion

        #region Перегрузки

        public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

		public override bool Equals(object obj) {
			if (obj is Vector4d) {
                Vector4d v = (Vector4d)obj;
				return (X == v.X) && (Y == v.Y) && (Z == v.Z);
			}
			return false;
		}

		public override string ToString() {
			return string.Format("({0:}, {1}, {2})", X, Y, Z);
		}
		#endregion

        #region Операторы

        public static bool operator ==(Vector3d left, Vector3d right) {
			return ValueType.Equals(left, right);
		}

        public static bool operator !=(Vector3d left, Vector3d right) {
			return !ValueType.Equals(left, right);
		}

		public static bool operator >(Vector3d left, Vector3d right) {
			return ((left.X > right.X) && (left.Y > right.Y) && (left.Z > right.Z));
		}

        public static bool operator <(Vector3d left, Vector3d right) {
			return ((left.X < right.X) && (left.Y < right.Y) && (left.Z < right.Z));
		}

        public static bool operator >=(Vector3d left, Vector3d right) {
			return ((left.X >= right.X) && (left.Y >= right.Y) && (left.Z >= right.Z));
		}

        public static bool operator <=(Vector3d left, Vector3d right) {
			return ((left.X <= right.X) && (left.Y <= right.Y) && (left.Z <= right.Z));
		}

        public static Vector3d operator -(Vector3d vector) {
            return Vector3d.Negate(vector);
		}

        public static Vector3d operator +(Vector3d left, Vector3d right) {
            return Vector3d.Add(left, right);
		}

        public static Vector3d operator +(Vector3d vector, double scalar) {
            return Vector3d.Add(vector, scalar);
		}

        public static Vector3d operator +(double scalar, Vector3d vector) {
            return Vector3d.Add(vector, scalar);
		}

        public static Vector3d operator -(Vector3d left, Vector3d right) {
            return Vector3d.Subtract(left, right);
		}

		public static Vector3d operator -(Vector3d vector, double scalar) {
            return Vector3d.Subtract(vector, scalar);
		}

        public static Vector3d operator -(double scalar, Vector3d vector) {
            return Vector3d.Subtract(scalar, vector);
		}

		public static Vector3d operator *(Vector3d vector, double scalar) {
            return Vector3d.Multiply(vector, scalar);
		}

        public static Vector3d operator *(double scalar, Vector3d vector) {
            return Vector3d.Multiply(vector, scalar);
		}

        public static Vector3d operator *(Vector3d left, Vector3d right) {
            return Vector3d.CrossProduct(left, right);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator ^(Vector3d left, Vector3d right) { unchecked {
            return Vector3d.Multiply(right, left);
		}}

        public static Vector3d operator /(Vector3d vector, double scalar) {
            return Vector3d.Divide(vector, scalar);
		}

        public static Vector3d operator /(double scalar, Vector3d vector) {
            return Vector3d.Divide(scalar, vector);
		}

        public double this[int index] {
			get { switch (index) {
					case  0: return X;
					case  1: return Y;
					case  2: return Z;
					default: throw new IndexOutOfRangeException();
			}}
			set { switch (index) {
					case  0: X = value; break;
					case  1: Y = value; break;
					case  2: Z = value; break;
					default: throw new IndexOutOfRangeException();
			}}
		}

		public static explicit operator double[](Vector3d vector) {
            return new double[] { vector.X, vector.Y, vector.Z };
		}

		public static explicit operator List<double>(Vector3d vector) {
			List<double> list = new List<double>(3);
			list.Add(vector.X);  list.Add(vector.Y);  list.Add(vector.Z);
			return list;
		}

		public static explicit operator LinkedList<double>(Vector3d vector) {
			LinkedList<double> list = new LinkedList<double>();
			list.AddLast(vector.X);  list.AddLast(vector.Y);  list.AddLast(vector.Z);
			return list;
		}

        public static explicit operator Vector2d(Vector3d vector) {
            return new Vector2d(vector.X, vector.Y);
		}

        public static explicit operator Vector2f(Vector3d vector) {
            return new Vector2f((float)vector.X, (float)vector.Y);
		}
        #endregion
    }


    [Serializable]
    [TypeConverter(typeof(ValueTypeTypeConverter<Vector4f>))]
    [StructLayout(LayoutKind.Explicit, Size = __SIZE)]
    public struct Vector4f : ICloneable, IEquatable<Vector4f>, IFormattable
	{
        public const int __SIZE = 16;
		[FieldOffset( 0)]  public float X;
        [FieldOffset( 4)]  public float Y;
        [FieldOffset( 8)]  public float Z;
        [FieldOffset(12)]  public float W;

        #region Конструкторы

        public Vector4f(float x, float y, float z, float w) {
			X = x;  Y = y;  Z = z;  W = w;
		}

		public Vector4f(float[] coordinates) {
			Debug.Assert(coordinates != null && coordinates.Length == 4);
			X = coordinates[0];     Y = coordinates[1];
			Z = coordinates[2];     W = coordinates[3];
		}

		public Vector4f(List<float> coordinates) {
			Debug.Assert(coordinates != null && coordinates.Count == 4);
			X = coordinates[0];     Y = coordinates[1];
			Z = coordinates[2];     W = coordinates[3];
		}

        public Vector4f(decimal[] elements) : this(elements.Select(e => (float)e).ToArray()) { }

        public Vector4f(Vector2f vector, float z, float w) {
			X = vector.X;   Y = vector.Y;   Z = z;   W = w;
		}

        public Vector4f(Vector3f vector, float w) {
			X = vector.X;   Y = vector.Y;
			Z = vector.Z;   W = w;
		}

		public Vector4f(Vector4f vector) {
			X = vector.X;   Y = vector.Y;
			Z = vector.Z;   W = vector.W;
		}
        #endregion

        #region Константы

        /// <summary>
		/// Получает вектор, четыре элемента которого равны нулю.
		/// </summary>
        public static readonly Vector4f Zero  = new Vector4f(0f, 0f, 0f, 0f);
        /// <summary>
		/// Получает вектор, четыре элемента которого равны единице.
		/// </summary>
        public static readonly Vector4f One   = new Vector4f(1f, 1f, 1f, 1f);
		/// <summary>
		/// Получает вектор (1,0,0,0).
		/// </summary>
        public static readonly Vector4f UnitX = new Vector4f(1f, 0f, 0f, 0f);
		/// <summary>
		/// Получает вектор (0,1,0,0).
		/// </summary>
        public static readonly Vector4f UnitY = new Vector4f(0f, 1f, 0f, 0f);
		/// <summary>
		/// Получает вектор (0,0,1,0).
		/// </summary>
        public static readonly Vector4f UnitZ = new Vector4f(0f, 0f, 1f, 0f);
		/// <summary>
		/// Получает вектор (0,0,0,1).
		/// </summary>
        public static readonly Vector4f UnitW = new Vector4f(0f, 0f, 0f, 1f);
        #endregion

        #region Статические методы

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f Add(Vector4f left, Vector4f right) { unchecked {
			return new Vector4f(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f Add(Vector4f vector, float scalar) { unchecked {
			return new Vector4f(vector.X + scalar, vector.Y + scalar, vector.Z + scalar, vector.W + scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Add(Vector4f left, Vector4f right, ref Vector4f result) { unchecked {
			result.X = left.X + right.X;
			result.Y = left.Y + right.Y;
			result.Z = left.Z + right.Z;
			result.W = left.W + right.W;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Add(Vector4f vector, float scalar, ref Vector4f result) { unchecked {
			result.X = vector.X + scalar;
			result.Y = vector.Y + scalar;
			result.Z = vector.Z + scalar;
			result.W = vector.W + scalar;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f Subtract(Vector4f left, Vector4f right) { unchecked {
			return new Vector4f(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f Subtract(Vector4f vector, float scalar) { unchecked {
			return new Vector4f(vector.X - scalar, vector.Y - scalar, vector.Z - scalar, vector.W - scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f Subtract(float scalar, Vector4f vector) { unchecked {
			return new Vector4f(scalar - vector.X, scalar - vector.Y, scalar - vector.Z, scalar - vector.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Subtract(Vector4f left, Vector4f right, ref Vector4f result) { unchecked {
			result.X = left.X - right.X;
			result.Y = left.Y - right.Y;
			result.Z = left.Z - right.Z;
			result.W = left.W - right.W;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Subtract(Vector4f vector, float scalar, ref Vector4f result) { unchecked {
			result.X = vector.X - scalar;
			result.Y = vector.Y - scalar;
			result.Z = vector.Z - scalar;
			result.W = vector.W - scalar;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Subtract(float scalar, Vector4f vector, ref Vector4f result) { unchecked {
			result.X = scalar - vector.X;
			result.Y = scalar - vector.Y;
			result.Z = scalar - vector.Z;
			result.W = scalar - vector.W;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f Divide(Vector4f left, Vector4f right) { unchecked {
			return new Vector4f(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f Divide(Vector4f vector, float scalar) { unchecked {
			return new Vector4f(vector.X / scalar, vector.Y / scalar, vector.Z / scalar, vector.W / scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f Divide(float scalar, Vector4f vector) { unchecked {
			return new Vector4f(scalar / vector.X, scalar / vector.Y, scalar / vector.Z, scalar / vector.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Divide(Vector4f left, Vector4f right, ref Vector4f result) { unchecked {
			result.X = left.X / right.X;
			result.Y = left.Y / right.Y;
			result.Z = left.Z / right.Z;
			result.W = left.W / right.W;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Divide(Vector4f vector, float scalar, ref Vector4f result) { unchecked {
			result.X = vector.X / scalar;
			result.Y = vector.Y / scalar;
			result.Z = vector.Z / scalar;
			result.W = vector.W / scalar;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Divide(float scalar, Vector4f vector, ref Vector4f result) { unchecked {
			result.X = scalar / vector.X;
			result.Y = scalar / vector.Y;
			result.Z = scalar / vector.Z;
			result.W = scalar / vector.W;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f Multiply(Vector4f vector, float scalar) { unchecked {
			return new Vector4f(vector.X * scalar, vector.Y * scalar, vector.Z * scalar, vector.W * scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f Multiply(Vector4f left, Vector4f right) { unchecked {
            return new Vector4f(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(ref Vector4f left, Vector4f right) { unchecked {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
            left.W *= right.W;
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Multiply(Vector4f vector, float scalar, ref Vector4f result) { unchecked {
			result.X = vector.X * scalar;
			result.Y = vector.Y * scalar;
			result.Z = vector.Z * scalar;
			result.W = vector.W * scalar;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DotProduct(Vector4f left, Vector4f right) { unchecked {
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f Negate(Vector4f vector) { unchecked {
			return new Vector4f(-vector.X, -vector.Y, -vector.Z, -vector.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ApproxEqual(Vector4f left, Vector4f right) { unchecked {
			return ApproxEqual(left, right, Single.Epsilon);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ApproxEqual(Vector4f left, Vector4f right, float tolerance) { unchecked {
			return ((System.Math.Abs(left.X - right.X) <= tolerance) &&
				    (System.Math.Abs(left.Y - right.Y) <= tolerance) &&
				    (System.Math.Abs(left.Z - right.Z) <= tolerance) &&
				    (System.Math.Abs(left.W - right.W) <= tolerance) );
		}}
		#endregion

        #region Свойства и Методы

        [DisplayName(__CONST.VectorX_Name)]
        [Description(__CONST.VectorX_Desc)]
        public float ValueX {
            get { return X; }
            set { X = value; }
	    }

        [DisplayName(__CONST.VectorY_Name)]
        [Description(__CONST.VectorY_Desc)]
        public float ValueY {
            get { return Y; }
            set { Y = value; }
	    }

        [DisplayName(__CONST.VectorZ_Name)]
        [Description(__CONST.VectorZ_Desc)]
        public float ValueZ {
            get { return Z; }
            set { Z = value; }
	    }

        [DisplayName(__CONST.VectorW_Name)]
        [Description(__CONST.VectorW_Desc)]
        public float ValueW {
            get { return W; }
            set { W = value; }
	    }

        public bool IsReal() {
            return !Single.IsInfinity(X) && !Single.IsNaN(X) &&
                   !Single.IsInfinity(Y) && !Single.IsNaN(Y) &&
                   !Single.IsInfinity(Z) && !Single.IsNaN(Z) &&
                   !Single.IsInfinity(W) && !Single.IsNaN(W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f Multiply(Vector4f vector) { unchecked {
	        return Multiply(this, vector);
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f Multiplied(Vector4f vector) { unchecked {
	        Multiply(ref this, vector);
            return this;
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float DotProduct(Vector4f vector) { unchecked {
            return DotProduct(this, vector);
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() { unchecked {
			float length = GetLength();
			if (length == 0f)
                throw new DivideByZeroException("Невозможно нормализовать вектор нулевой длинны");
			X /= length;
			Y /= length;
			Z /= length;
			W /= length;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f Normalized() { unchecked {
	        Normalize();
	        return this;
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetLength() { unchecked {
			return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetLengthSquared() { unchecked {
			return (X * X + Y * Y + Z * Z + W * W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClampZero(float tolerance) { unchecked {
            X = (tolerance > Math.Abs(X)) ? 0 : X;
            Y = (tolerance > Math.Abs(Y)) ? 0 : Y;
            Z = (tolerance > Math.Abs(Z)) ? 0 : Z;
            W = (tolerance > Math.Abs(W)) ? 0 : W;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClampZero() { unchecked {
            X = (Double.Epsilon > Math.Abs(X)) ? 0 : X;
            Y = (Double.Epsilon > Math.Abs(Y)) ? 0 : Y;
            Z = (Double.Epsilon > Math.Abs(Z)) ? 0 : Z;
            W = (Double.Epsilon > Math.Abs(W)) ? 0 : W;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public float[] ToArray() { unchecked {
	        return new float[4] { X, Y, Z, W };
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float[] ToFloatArray() { unchecked {
            return ToArray().Select(v => (float)v).ToArray();
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal[] ToDecimalArray() { unchecked {
            return ToArray().Select(v => (decimal)v).ToArray();
	    }}
        #endregion

        #region Реализация интерфейса ICloneable

        object ICloneable.Clone() {
            return new Vector4f(this);
        }

        public Vector4f Clone() {
            return new Vector4f(this);
        }
        #endregion

        #region Реализация интерфейса IEquatable<Vector4>

        bool IEquatable<Vector4f>.Equals(Vector4f v) {
            return (X == v.X) && (Y == v.Y) && (Z == v.Z) && (W == v.W);
        }

        public bool Equals(Vector4f v) {
            return (X == v.X) && (Y == v.Y) && (Z == v.Z) && (W == v.W);
        }
        #endregion

        #region Реализация интерфейса IFormattable

        public string ToString(string format) {
            return String.Format("({0:}, {1}, {2}, {3})", X.ToString(format, null),
                Y.ToString(format, null), Z.ToString(format, null), W.ToString(format, null));
        }

        string IFormattable.ToString(string format, IFormatProvider provider) {
            return ToString(format, provider);
        }

        public string ToString(string format, IFormatProvider provider) {
            return String.Format("({0:}, {1}, {2}, {3})", X.ToString(format, provider), 
                Y.ToString(format, provider), Z.ToString(format, provider), W.ToString(format, provider));
        }
        #endregion

        #region Перегрузки

        public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
		}

		public override bool Equals(object obj) {
			if (obj is Vector4f) {
                Vector4f v = (Vector4f)obj;
				return (X == v.X) && (Y == v.Y) && (Z == v.Z) && (W == v.W);
			}
			return false;
		}

		public override string ToString() {
			return string.Format("({0:}, {1}, {2}, {3})", X, Y, Z, W);
		}
		#endregion

        #region Операторы

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4f left, Vector4f right) { unchecked {
			return ValueType.Equals(left, right);
		}}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector4f left, Vector4f right) { unchecked {
			return !ValueType.Equals(left, right);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Vector4f left, Vector4f right) { unchecked {
			return( (left.X > right.X) && (left.Y > right.Y) &&
				    (left.Z > right.Z) && (left.W > right.W));
		}}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(Vector4f left, Vector4f right) { unchecked {
			return( (left.X < right.X) && (left.Y < right.Y) &&
				    (left.Z < right.Z) && (left.W < right.W)); 
        }}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(Vector4f left, Vector4f right) { unchecked {
			return( (left.X >= right.X) && (left.Y >= right.Y) &&
				    (left.Z >= right.Z) && (left.W >= right.W));
		}}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(Vector4f left, Vector4f right) { unchecked {
			return( (left.X <= right.X) && (left.Y <= right.Y) &&
				    (left.Z <= right.Z) && (left.W <= right.W)); 
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f operator -(Vector4f vector) { unchecked {
			return Vector4f.Negate(vector);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f operator +(Vector4f left, Vector4f right) { unchecked {
			return Vector4f.Add(left, right);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f operator +(Vector4f vector, float scalar) { unchecked {
			return Vector4f.Add(vector, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f operator +(float scalar, Vector4f vector) { unchecked {
			return Vector4f.Add(vector, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f operator -(Vector4f left, Vector4f right) { unchecked {
			return Vector4f.Subtract(left, right);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f operator -(Vector4f vector, float scalar) { unchecked {
			return Vector4f.Subtract(vector, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f operator -(float scalar, Vector4f vector) { unchecked {
			return Vector4f.Subtract(scalar, vector);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f operator *(Vector4f vector, float scalar) { unchecked {
			return Vector4f.Multiply(vector, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f operator *(float scalar, Vector4f vector) { unchecked {
			return Vector4f.Multiply(vector, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator *(Vector4f left, Vector4f right) { unchecked {
            return Vector3f.CrossProduct(left, right);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator ^(Vector4f left, Vector4f right) { unchecked {
            return Vector4f.Multiply(right, left); ;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f operator /(Vector4f vector, float scalar) { unchecked {
			return Vector4f.Divide(vector, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4f operator /(float scalar, Vector4f vector) { unchecked {
			return Vector4f.Divide(scalar, vector);
		}}

		public float this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { unchecked { switch (index) {
					case  0: return X;
					case  1: return Y;
					case  2: return Z;
					case  3: return W;
					default: throw new IndexOutOfRangeException();
			}}}
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
			set { unchecked { switch (index) {
					case  0: X = value; break;
					case  1: Y = value; break;
					case  2: Z = value; break;
					case  3: W = value; break;
					default: throw new IndexOutOfRangeException();
			}}}
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float[](Vector4f vector) { unchecked {
            return new float[] { vector.X, vector.Y, vector.Z, vector.W };
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator List<float>(Vector4f vector) { unchecked {
			List<float> list = new List<float>(4);
			list.Add(vector.X);  list.Add(vector.Y);  list.Add(vector.Z);  list.Add(vector.W);
			return list;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator LinkedList<float>(Vector4f vector) { unchecked {
			LinkedList<float> list = new LinkedList<float>();
			list.AddLast(vector.X);  list.AddLast(vector.Y);  list.AddLast(vector.Z);  list.AddLast(vector.W);
			return list;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector3f(Vector4f vector) { unchecked {
            return new Vector3f(vector.X, vector.Y, vector.Z);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector2f(Vector4f vector) {
            return new Vector2f(vector.X, vector.Y);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector3d(Vector4f vector) { unchecked {
            return new Vector3d((double)vector.X, (double)vector.Y, (double)vector.Z);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector2d(Vector4f vector) {
            return new Vector2d((double)vector.X, (double)vector.Y);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector4d(Vector4f vector) {
            return new Vector4d((double)vector.X, (double)vector.Y, (double)vector.Z, (double)vector.W);
		}
        #endregion
	}


	[Serializable]
    [TypeConverter(typeof(ValueTypeTypeConverter<Vector4d>))]
    [StructLayout(LayoutKind.Explicit, Size = __SIZE)]
    public struct Vector4d : ICloneable, IEquatable<Vector4d>, IFormattable
	{
        public const int __SIZE = 32;
		[FieldOffset( 0)]  public double X;
        [FieldOffset( 8)]  public double Y;
        [FieldOffset(16)]  public double Z;
        [FieldOffset(24)]  public double W;

        #region Конструкторы

        public Vector4d(double x, double y, double z, double w) {
			X = x;  Y = y;  Z = z;  W = w;
		}

		public Vector4d(double[] coordinates) {
			Debug.Assert(coordinates != null && coordinates.Length == 4);
			X = coordinates[0];     Y = coordinates[1];
			Z = coordinates[2];     W = coordinates[3];
		}

		public Vector4d(List<double> coordinates) {
			Debug.Assert(coordinates != null && coordinates.Count == 4);
			X = coordinates[0];     Y = coordinates[1];
			Z = coordinates[2];     W = coordinates[3];
		}

        public Vector4d(decimal[] elements) : this(elements.Select(e => (double)e).ToArray()) { }

        public Vector4d(Vector2d vector, double z, double w) {
			X = vector.X;   Y = vector.Y;   Z = z;   W = w;
		}

        public Vector4d(Vector3d vector, double w) {
			X = vector.X;   Y = vector.Y;
			Z = vector.Z;   W = w;
		}

		public Vector4d(Vector4d vector) {
			X = vector.X;   Y = vector.Y;
			Z = vector.Z;   W = vector.W;
		}
        #endregion

        #region Константы

        /// <summary>
		/// Получает вектор, четыре элемента которого равны нулю.
		/// </summary>
        public static readonly Vector4d Zero  = new Vector4d(0d, 0d, 0d, 0d);
        /// <summary>
		/// Получает вектор, четыре элемента которого равны единице.
		/// </summary>
        public static readonly Vector4d One   = new Vector4d(1d, 1d, 1d, 1d);
		/// <summary>
		/// Получает вектор (1,0,0,0).
		/// </summary>
        public static readonly Vector4d UnitX = new Vector4d(1d, 0d, 0d, 0d);
		/// <summary>
		/// Получает вектор (0,1,0,0).
		/// </summary>
        public static readonly Vector4d UnitY = new Vector4d(0d, 1d, 0d, 0d);
		/// <summary>
		/// Получает вектор (0,0,1,0).
		/// </summary>
        public static readonly Vector4d UnitZ = new Vector4d(0d, 0d, 1d, 0d);
		/// <summary>
		/// Получает вектор (0,0,0,1).
		/// </summary>
        public static readonly Vector4d UnitW = new Vector4d(0d, 0d, 0d, 1d);
        #endregion

        #region Статические методы

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Add(Vector4d left, Vector4d right) { unchecked {
			return new Vector4d(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d Add(Vector4d vector, double scalar) { unchecked {
			return new Vector4d(vector.X + scalar, vector.Y + scalar, vector.Z + scalar, vector.W + scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Add(Vector4d left, Vector4d right, ref Vector4d result) { unchecked {
			result.X = left.X + right.X;
			result.Y = left.Y + right.Y;
			result.Z = left.Z + right.Z;
			result.W = left.W + right.W;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Add(Vector4d vector, double scalar, ref Vector4d result) { unchecked {
			result.X = vector.X + scalar;
			result.Y = vector.Y + scalar;
			result.Z = vector.Z + scalar;
			result.W = vector.W + scalar;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d Subtract(Vector4d left, Vector4d right) { unchecked {
			return new Vector4d(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d Subtract(Vector4d vector, double scalar) { unchecked {
			return new Vector4d(vector.X - scalar, vector.Y - scalar, vector.Z - scalar, vector.W - scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d Subtract(double scalar, Vector4d vector) { unchecked {
			return new Vector4d(scalar - vector.X, scalar - vector.Y, scalar - vector.Z, scalar - vector.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Subtract(Vector4d left, Vector4d right, ref Vector4d result) { unchecked {
			result.X = left.X - right.X;
			result.Y = left.Y - right.Y;
			result.Z = left.Z - right.Z;
			result.W = left.W - right.W;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Subtract(Vector4d vector, double scalar, ref Vector4d result) { unchecked {
			result.X = vector.X - scalar;
			result.Y = vector.Y - scalar;
			result.Z = vector.Z - scalar;
			result.W = vector.W - scalar;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Subtract(double scalar, Vector4d vector, ref Vector4d result) { unchecked {
			result.X = scalar - vector.X;
			result.Y = scalar - vector.Y;
			result.Z = scalar - vector.Z;
			result.W = scalar - vector.W;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d Divide(Vector4d left, Vector4d right) { unchecked {
			return new Vector4d(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d Divide(Vector4d vector, double scalar) { unchecked {
			return new Vector4d(vector.X / scalar, vector.Y / scalar, vector.Z / scalar, vector.W / scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d Divide(double scalar, Vector4d vector) { unchecked {
			return new Vector4d(scalar / vector.X, scalar / vector.Y, scalar / vector.Z, scalar / vector.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Divide(Vector4d left, Vector4d right, ref Vector4d result) { unchecked {
			result.X = left.X / right.X;
			result.Y = left.Y / right.Y;
			result.Z = left.Z / right.Z;
			result.W = left.W / right.W;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Divide(Vector4d vector, double scalar, ref Vector4d result) { unchecked {
			result.X = vector.X / scalar;
			result.Y = vector.Y / scalar;
			result.Z = vector.Z / scalar;
			result.W = vector.W / scalar;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Divide(double scalar, Vector4d vector, ref Vector4d result) { unchecked {
			result.X = scalar / vector.X;
			result.Y = scalar / vector.Y;
			result.Z = scalar / vector.Z;
			result.W = scalar / vector.W;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d Multiply(Vector4d vector, double scalar) { unchecked {
			return new Vector4d(vector.X * scalar, vector.Y * scalar, vector.Z * scalar, vector.W * scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Multiply(Vector4d left, Vector4d right) { unchecked {
            return new Vector4d(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(ref Vector4d left, Vector4d right) { unchecked {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
            left.W *= right.W;
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Multiply(Vector4d vector, double scalar, ref Vector4d result) { unchecked {
			result.X = vector.X * scalar;
			result.Y = vector.Y * scalar;
			result.Z = vector.Z * scalar;
			result.W = vector.W * scalar;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double DotProduct(Vector4d left, Vector4d right) { unchecked {
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d Negate(Vector4d vector) { unchecked {
			return new Vector4d(-vector.X, -vector.Y, -vector.Z, -vector.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ApproxEqual(Vector4d left, Vector4d right) { unchecked {
			return ApproxEqual(left, right, Double.Epsilon);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ApproxEqual(Vector4d left, Vector4d right, double tolerance) { unchecked {
			return ((System.Math.Abs(left.X - right.X) <= tolerance) &&
				    (System.Math.Abs(left.Y - right.Y) <= tolerance) &&
				    (System.Math.Abs(left.Z - right.Z) <= tolerance) &&
				    (System.Math.Abs(left.W - right.W) <= tolerance) );
		}}
		#endregion

        #region Свойства и Методы

        [DisplayName(__CONST.VectorX_Name)]
        [Description(__CONST.VectorX_Desc)]
        public double ValueX {
            get { return X; }
            set { X = value; }
	    }

        [DisplayName(__CONST.VectorY_Name)]
        [Description(__CONST.VectorY_Desc)]
        public double ValueY {
            get { return Y; }
            set { Y = value; }
	    }

        [DisplayName(__CONST.VectorZ_Name)]
        [Description(__CONST.VectorZ_Desc)]
        public double ValueZ {
            get { return Z; }
            set { Z = value; }
	    }

        [DisplayName(__CONST.VectorW_Name)]
        [Description(__CONST.VectorW_Desc)]
        public double ValueW {
            get { return W; }
            set { W = value; }
	    }

        public bool IsReal() {
            return !Double.IsInfinity(X) && !Double.IsNaN(X) &&
                   !Double.IsInfinity(Y) && !Double.IsNaN(Y) &&
                   !Double.IsInfinity(Z) && !Double.IsNaN(Z) &&
                   !Double.IsInfinity(W) && !Double.IsNaN(W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Multiply(Vector4d vector) { unchecked {
	        return Multiply(this, vector);
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Multiplied(Vector4d vector) { unchecked {
	        Multiply(ref this, vector);
            return this;
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double DotProduct(Vector4d vector) { unchecked {
            return DotProduct(this, vector);
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() { unchecked {
			double length = GetLength();
			if (length == 0)
                throw new DivideByZeroException("Невозможно нормализовать вектор нулевой длинны");
			X /= length;
			Y /= length;
			Z /= length;
			W /= length;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Normalized() { unchecked {
	        Normalize();
	        return this;
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double GetLength() { unchecked {
			return System.Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double GetLengthSquared() { unchecked {
			return (X * X + Y * Y + Z * Z + W * W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClampZero(double tolerance) { unchecked {
            X = (tolerance > Math.Abs(X)) ? 0 : X;
            Y = (tolerance > Math.Abs(Y)) ? 0 : Y;
            Z = (tolerance > Math.Abs(Z)) ? 0 : Z;
            W = (tolerance > Math.Abs(W)) ? 0 : W;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClampZero() { unchecked {
            X = (Double.Epsilon > Math.Abs(X)) ? 0 : X;
            Y = (Double.Epsilon > Math.Abs(Y)) ? 0 : Y;
            Z = (Double.Epsilon > Math.Abs(Z)) ? 0 : Z;
            W = (Double.Epsilon > Math.Abs(W)) ? 0 : W;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public double[] ToArray() { unchecked {
	        return new double[4] { X, Y, Z, W };
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float[] ToFloatArray() { unchecked {
            return ToArray().Select(v => (float)v).ToArray();
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal[] ToDecimalArray() { unchecked {
            return ToArray().Select(v => (decimal)v).ToArray();
	    }}
        #endregion

        #region Реализация интерфейса ICloneable

        object ICloneable.Clone() {
            return new Vector4d(this);
        }

        public Vector4d Clone() {
            return new Vector4d(this);
        }
        #endregion

        #region Реализация интерфейса IEquatable<Vector4>

        bool IEquatable<Vector4d>.Equals(Vector4d v) {
            return (X == v.X) && (Y == v.Y) && (Z == v.Z) && (W == v.W);
        }

        public bool Equals(Vector4d v) {
            return (X == v.X) && (Y == v.Y) && (Z == v.Z) && (W == v.W);
        }
        #endregion

        #region Реализация интерфейса IFormattable

        public string ToString(string format) {
            return String.Format("({0:}, {1}, {2}, {3})", X.ToString(format, null),
                Y.ToString(format, null), Z.ToString(format, null), W.ToString(format, null));
        }

        string IFormattable.ToString(string format, IFormatProvider provider) {
            return ToString(format, provider);
        }

        public string ToString(string format, IFormatProvider provider) {
            return String.Format("({0:}, {1}, {2}, {3})", X.ToString(format, provider), 
                Y.ToString(format, provider), Z.ToString(format, provider), W.ToString(format, provider));
        }
        #endregion

        #region Перегрузки

        public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
		}

		public override bool Equals(object obj) {
			if (obj is Vector4d) {
                Vector4d v = (Vector4d)obj;
				return (X == v.X) && (Y == v.Y) && (Z == v.Z) && (W == v.W);
			}
			return false;
		}

		public override string ToString() {
			return string.Format("({0:}, {1}, {2}, {3})", X, Y, Z, W);
		}
		#endregion

        #region Операторы

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4d left, Vector4d right) { unchecked {
			return ValueType.Equals(left, right);
		}}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector4d left, Vector4d right) { unchecked {
			return !ValueType.Equals(left, right);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Vector4d left, Vector4d right) { unchecked {
			return( (left.X > right.X) && (left.Y > right.Y) &&
				    (left.Z > right.Z) && (left.W > right.W));
		}}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(Vector4d left, Vector4d right) { unchecked {
			return( (left.X < right.X) && (left.Y < right.Y) &&
				    (left.Z < right.Z) && (left.W < right.W)); 
        }}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(Vector4d left, Vector4d right) { unchecked {
			return( (left.X >= right.X) && (left.Y >= right.Y) &&
				    (left.Z >= right.Z) && (left.W >= right.W));
		}}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(Vector4d left, Vector4d right) { unchecked {
			return( (left.X <= right.X) && (left.Y <= right.Y) &&
				    (left.Z <= right.Z) && (left.W <= right.W)); 
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d operator -(Vector4d vector) { unchecked {
			return Vector4d.Negate(vector);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d operator +(Vector4d left, Vector4d right) { unchecked {
			return Vector4d.Add(left, right);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d operator +(Vector4d vector, double scalar) { unchecked {
			return Vector4d.Add(vector, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d operator +(double scalar, Vector4d vector) { unchecked {
			return Vector4d.Add(vector, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d operator -(Vector4d left, Vector4d right) { unchecked {
			return Vector4d.Subtract(left, right);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d operator -(Vector4d vector, double scalar) { unchecked {
			return Vector4d.Subtract(vector, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d operator -(double scalar, Vector4d vector) { unchecked {
			return Vector4d.Subtract(scalar, vector);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d operator *(Vector4d vector, double scalar) { unchecked {
			return Vector4d.Multiply(vector, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d operator *(double scalar, Vector4d vector) { unchecked {
			return Vector4d.Multiply(vector, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator *(Vector4d left, Vector4d right) { unchecked {
            return Vector3d.CrossProduct(left, right);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator ^(Vector4d left, Vector4d right) { unchecked {
            return Vector4d.Multiply(right, left); ;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d operator /(Vector4d vector, double scalar) { unchecked {
			return Vector4d.Divide(vector, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4d operator /(double scalar, Vector4d vector) { unchecked {
			return Vector4d.Divide(scalar, vector);
		}}

		public double this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { unchecked { switch (index) {
					case  0: return X;
					case  1: return Y;
					case  2: return Z;
					case  3: return W;
					default: throw new IndexOutOfRangeException();
			}}}
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
			set { unchecked { switch (index) {
					case  0: X = value; break;
					case  1: Y = value; break;
					case  2: Z = value; break;
					case  3: W = value; break;
					default: throw new IndexOutOfRangeException();
			}}}
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double[](Vector4d vector) { unchecked {
            return new double[] { vector.X, vector.Y, vector.Z, vector.W };
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator List<double>(Vector4d vector) { unchecked {
			List<double> list = new List<double>(4);
			list.Add(vector.X);  list.Add(vector.Y);  list.Add(vector.Z);  list.Add(vector.W);
			return list;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator LinkedList<double>(Vector4d vector) { unchecked {
			LinkedList<double> list = new LinkedList<double>();
			list.AddLast(vector.X);  list.AddLast(vector.Y);  list.AddLast(vector.Z);  list.AddLast(vector.W);
			return list;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector3d(Vector4d vector) { unchecked {
            return new Vector3d(vector.X, vector.Y, vector.Z);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector2d(Vector4d vector) {
            return new Vector2d(vector.X, vector.Y);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector3f(Vector4d vector) { unchecked {
            return new Vector3f((float)vector.X, (float)vector.Y, (float)vector.Z);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector2f(Vector4d vector) {
            return new Vector2f((float)vector.X, (float)vector.Y);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector4f(Vector4d vector) {
            return new Vector4f((float)vector.X, (float)vector.Y, (float)vector.Z, (float)vector.W);
		}
        #endregion
	}


    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = __SIZE)]
    public struct Matrix3f : ICloneable, IEquatable<Matrix3f>, IFormattable
    {
        public const int __SIZE = 36;
        [FieldOffset( 0)] public float M11;     // ┎─────┬─────┬─────┓
        [FieldOffset( 4)] public float M12;     // ┃ M11 │ M12 │ M13 ┃
        [FieldOffset( 8)] public float M13;     // ┠━━━━━┼━━━━━┼━━━━━┫
        [FieldOffset(12)] public float M21;     // ┃ M21 │ M22 │ M23 ┃
        [FieldOffset(16)] public float M22;     // ┠━━━━━┼━━━━━┼━━━━━┫
        [FieldOffset(20)] public float M23;     // ┃ M31 │ M32 │ M33 ┃
        [FieldOffset(24)] public float M31;     // ┖─────┴─────┴─────┚
        [FieldOffset(28)] public float M32;
        [FieldOffset(32)] public float M33;

        #region Конструкторы

        public Matrix3f( float m11, float m12, float m13,
			             float m21, float m22, float m23,
			             float m31, float m32, float m33 ) {
			M11 = m11; M12 = m12; M13 = m13;
			M21 = m21; M22 = m22; M23 = m23;
			M31 = m31; M32 = m32; M33 = m33;
		}

        public Matrix3f(float[] elements) {
			Debug.Assert(elements != null && elements.Length == 9);
			M11 = elements[ 0]; M12 = elements[ 1]; M13 = elements[ 2];
			M21 = elements[ 3]; M22 = elements[ 4]; M23 = elements[ 5];
			M31 = elements[ 6]; M32 = elements[ 7]; M33 = elements[ 8];
		}

        public Matrix3f(List<float> elements) {
            Debug.Assert(elements != null && elements.Count == 9);
			M11 = elements[ 0]; M12 = elements[ 1]; M13 = elements[ 2];
			M21 = elements[ 3]; M22 = elements[ 4]; M23 = elements[ 5];
			M31 = elements[ 6]; M32 = elements[ 7]; M33 = elements[ 8];
		}

        public Matrix3f(decimal[] elements) : this(elements.Select(e => (float)e).ToArray()) { }

        public Matrix3f(Matrix3f m) {
            M11 = m.M11; M12 = m.M12; M13 = m.M13;
            M21 = m.M21; M22 = m.M22; M23 = m.M23;
            M31 = m.M31; M32 = m.M32; M33 = m.M33;
		}
        #endregion

        #region Константы

        public static readonly Matrix3f Zero = new Matrix3f(0f,0f,0f, 0f,0f,0f, 0f,0f,0f);

        public static readonly Matrix3f Identity = new Matrix3f(1f, 0f, 0f,
                                                                0f, 1f, 0f,
                                                                0f, 0f, 1f );
        #endregion

        #region Статические методы

		public static Matrix3f Add(Matrix3f left, Matrix3f right) {
			return new Matrix3f(
				left.M11 + right.M11, left.M12 + right.M12, left.M13 + right.M13, 
				left.M21 + right.M21, left.M22 + right.M22, left.M23 + right.M23, 
				left.M31 + right.M31, left.M32 + right.M32, left.M33 + right.M33);
		}

		public static Matrix3f Add(Matrix3f matrix, float scalar) {
			return new Matrix3f(
				matrix.M11 + scalar, matrix.M12 + scalar, matrix.M13 + scalar, 
				matrix.M21 + scalar, matrix.M22 + scalar, matrix.M23 + scalar, 
				matrix.M31 + scalar, matrix.M32 + scalar, matrix.M33 + scalar);
		}

		public static void Add(Matrix3f left, Matrix3f right, ref Matrix3f result) {
			result.M11 = left.M11 + right.M11;
			result.M12 = left.M12 + right.M12;
			result.M13 = left.M13 + right.M13;

			result.M21 = left.M21 + right.M21;
			result.M22 = left.M22 + right.M22;
			result.M23 = left.M23 + right.M23;

			result.M31 = left.M31 + right.M31;
			result.M32 = left.M32 + right.M32;
			result.M33 = left.M33 + right.M33;
		}

		public static void Add(Matrix3f matrix, float scalar, ref Matrix3f result) {
			result.M11 = matrix.M11 + scalar;
			result.M12 = matrix.M12 + scalar;
			result.M13 = matrix.M13 + scalar;

			result.M21 = matrix.M21 + scalar;
			result.M22 = matrix.M22 + scalar;
			result.M23 = matrix.M23 + scalar;

			result.M31 = matrix.M31 + scalar;
			result.M32 = matrix.M32 + scalar;
			result.M33 = matrix.M33 + scalar;
		}

		public static Matrix3f Subtract(Matrix3f left, Matrix3f right) {
			return new Matrix3f(
				left.M11 - right.M11, left.M12 - right.M12, left.M13 - right.M13,
				left.M21 - right.M21, left.M22 - right.M22, left.M23 - right.M23,
				left.M31 - right.M31, left.M32 - right.M32, left.M33 - right.M33);
		}

		public static Matrix3f Subtract(Matrix3f matrix, float scalar) {
			return new Matrix3f(
				matrix.M11 - scalar, matrix.M12 - scalar, matrix.M13 - scalar,
				matrix.M21 - scalar, matrix.M22 - scalar, matrix.M23 - scalar,
				matrix.M31 - scalar, matrix.M32 - scalar, matrix.M33 - scalar);
		}

		public static void Subtract(Matrix3f left, Matrix3f right, ref Matrix3f result) {
			result.M11 = left.M11 - right.M11;
			result.M12 = left.M12 - right.M12;
			result.M13 = left.M13 - right.M13;

			result.M21 = left.M21 - right.M21;
			result.M22 = left.M22 - right.M22;
			result.M23 = left.M23 - right.M23;

			result.M31 = left.M31 - right.M31;
			result.M32 = left.M32 - right.M32;
			result.M33 = left.M33 - right.M33;
		}

		public static void Subtract(Matrix3f matrix, float scalar, ref Matrix3f result) {
			result.M11 = matrix.M11 - scalar;
			result.M12 = matrix.M12 - scalar;
			result.M13 = matrix.M13 - scalar;

			result.M21 = matrix.M21 - scalar;
			result.M22 = matrix.M22 - scalar;
			result.M23 = matrix.M23 - scalar;

			result.M31 = matrix.M31 - scalar;
			result.M32 = matrix.M32 - scalar;
			result.M33 = matrix.M33 - scalar;
		}

		public static Matrix3f Multiply(Matrix3f left, Matrix3f right) {
			return new Matrix3f(
				left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31,
				left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32,
				left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33,

				left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31,
				left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32,
				left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33,

				left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31,
				left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32,
				left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33);
		}

		public static void Multiply(Matrix3f left, Matrix3f right, ref Matrix3f result) {
			result.M11 = left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31;
			result.M12 = left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32;
			result.M13 = left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33;

			result.M21 = left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31;
			result.M22 = left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32;
			result.M23 = left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33;

			result.M31 = left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31;
			result.M32 = left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32;
			result.M33 = left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33;
		}

		public static Vector3f Transform(Matrix3f matrix, Vector3f vector) {
			return new Vector3f(
				(matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z),
				(matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z),
				(matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z));
		}
		
		public static void Transform(Matrix3f matrix, Vector3d vector, ref Vector3d result) {
			result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z);
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z);
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z);
		}

        public static Matrix3f Invert(Matrix3f matrix) {
            float invdet = 1 / matrix.GetDeterminant();
            return new Matrix3f() {
                M11 = invdet * (matrix.M22 * matrix.M33 - matrix.M32 * matrix.M23),
                M12 = invdet * (matrix.M13 * matrix.M32 - matrix.M12 * matrix.M33),
                M13 = invdet * (matrix.M12 * matrix.M23 - matrix.M13 * matrix.M22),

                M21 = invdet * (matrix.M23 * matrix.M31 - matrix.M21 * matrix.M33),
                M22 = invdet * (matrix.M11 * matrix.M33 - matrix.M13 * matrix.M31),
                M23 = invdet * (matrix.M21 * matrix.M13 - matrix.M11 * matrix.M23),
            
                M31 = invdet * (matrix.M21 * matrix.M32 - matrix.M31 * matrix.M22),
                M32 = invdet * (matrix.M31 * matrix.M12 - matrix.M11 * matrix.M32),
                M33 = invdet * (matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12)
            };
        }

        public static Matrix4f TransposeInvert(Matrix4f matrix) {
            float det =  matrix.M11 * (matrix.M22 * matrix.M33 - matrix.M23 * matrix.M32)
                        - matrix.M12 * (matrix.M21 * matrix.M33 - matrix.M23 * matrix.M31) 
                        + matrix.M13 * (matrix.M21 * matrix.M32 - matrix.M22 * matrix.M31);
            float invdet = 1 / det;
            return new Matrix4f() {
                M11 = invdet * (matrix.M22 * matrix.M33 - matrix.M32 * matrix.M23),
                M12 = invdet * (matrix.M23 * matrix.M31 - matrix.M21 * matrix.M33),
                M13 = invdet * (matrix.M21 * matrix.M32 - matrix.M31 * matrix.M22),
                M21 = invdet * (matrix.M13 * matrix.M32 - matrix.M12 * matrix.M33),
                M22 = invdet * (matrix.M11 * matrix.M33 - matrix.M13 * matrix.M31),
                M23 = invdet * (matrix.M31 * matrix.M12 - matrix.M11 * matrix.M32),
                M31 = invdet * (matrix.M12 * matrix.M23 - matrix.M13 * matrix.M22),
                M32 = invdet * (matrix.M21 * matrix.M13 - matrix.M11 * matrix.M23),
                M33 = invdet * (matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12),
                M14 = 0, M24 = 0, M34 = 0, M41 = 0, M42 = 0, M43 = 0, M44 = 0
            };
        }

        public static Matrix4f NormalVecTransf(Matrix4f matrix) {
            return new Matrix4f(
                matrix.M33 * matrix.M22 - matrix.M23 * matrix.M32, 
                matrix.M23 * matrix.M31 - matrix.M21 * matrix.M33, 
                matrix.M21 * matrix.M32 - matrix.M31 * matrix.M22, 0,
                matrix.M13 * matrix.M32 - matrix.M33 * matrix.M12,
                matrix.M33 * matrix.M11 - matrix.M13 * matrix.M31,
                matrix.M31 * matrix.M12 - matrix.M11 * matrix.M32, 0,
                matrix.M23 * matrix.M12 - matrix.M13 * matrix.M22,
                matrix.M21 * matrix.M13 - matrix.M23 * matrix.M11,
                matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12, 0,
                0,   0,   0,   0);
        }

		public static Matrix3f Transpose(Matrix3f m) {
			Matrix3f t = new Matrix3f(m);
			t.Transpose();
			return t;
		}

        public static bool ApproxEqual(Matrix3f left, Matrix3f right, float tolerance) {
			return ((System.Math.Abs(left.M11 - right.M11) <= tolerance) &&
                    (System.Math.Abs(left.M12 - right.M12) <= tolerance) &&
				    (System.Math.Abs(left.M13 - right.M13) <= tolerance) &&

                    (System.Math.Abs(left.M21 - right.M21) <= tolerance) &&
                    (System.Math.Abs(left.M22 - right.M22) <= tolerance) &&
				    (System.Math.Abs(left.M23 - right.M23) <= tolerance) &&

                    (System.Math.Abs(left.M31 - right.M31) <= tolerance) &&
                    (System.Math.Abs(left.M32 - right.M32) <= tolerance) &&
				    (System.Math.Abs(left.M33 - right.M33) <= tolerance) );
		}
		#endregion

        #region Свойства и Методы

        public float Trace {
			get { return M11 + M22 + M33; }
		}  

		public float GetDeterminant() {
            return  M11 * M22 * M33 + M12 * M23 * M31 + M13 * M21 * M32 -
                    M13 * M22 * M31 - M11 * M23 * M32 - M12 * M21 * M33;
		}

        public Matrix3f Invert() {
            return Invert(this);
        }

		public void Transpose() {
		    float temp;
		    temp = M12;  M12 = M21;  M21 = temp;
            temp = M13;  M13 = M31;  M31 = temp;
            temp = M23;  M23 = M32;  M32 = temp;
		}

        public float[] ToArray(bool glorder = false) {
            return glorder 
                ? new float[9] { M11, M21, M31, M12, M22, M32, M13, M23, M33 }
                : new float[9] { M11, M12, M13, M21, M22, M23, M31, M32, M33 };
	    }

        public double[] ToDoubleArray(bool glorder = false) {
            return ToArray(glorder).Select(v => (double)v).ToArray();
	    }

        public decimal[] ToDecimalArray(bool glorder = false) {
            return ToArray(glorder).Select(v => (decimal)v).ToArray();
	    }
        #endregion

        #region Реализация интерфейса ICloneable

        object ICloneable.Clone() {
            return new Matrix3f(this);
        }

        public Matrix3f Clone() {
            return new Matrix3f(this);
        }
        #endregion

        #region Реализация интерфейса IEquatable<Vector4>

        bool IEquatable<Matrix3f>.Equals(Matrix3f m) {
            return  (M11 == m.M11) && (M12 == m.M12) && (M13 == m.M13) &&
				    (M21 == m.M21) && (M22 == m.M22) && (M23 == m.M23) &&
				    (M31 == m.M31) && (M32 == m.M32) && (M33 == m.M33);
        }

        public bool Equals(Matrix3f m) {
            return  (M11 == m.M11) && (M12 == m.M12) && (M13 == m.M13) &&
				    (M21 == m.M21) && (M22 == m.M22) && (M23 == m.M23) && 
				    (M31 == m.M31) && (M32 == m.M32) && (M33 == m.M33);
        }
        #endregion

        #region Реализация интерфейса IFormattable

        public string ToString(string format) {
            return string.Format("3x3[{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}]",
                M11.ToString(format, null), M12.ToString(format, null), M13.ToString(format, null),
                M21.ToString(format, null), M22.ToString(format, null), M23.ToString(format, null),
                M31.ToString(format, null), M32.ToString(format, null), M33.ToString(format, null));
        }

        string IFormattable.ToString(string format, IFormatProvider provider) {
            return ToString(format, provider);
        }

        public string ToString(string format, IFormatProvider provider) {
            return string.Format("3x3[{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}]",
                M11.ToString(format, provider), M12.ToString(format, provider), M13.ToString(format, provider),
                M21.ToString(format, provider), M22.ToString(format, provider), M23.ToString(format, provider),
                M31.ToString(format, provider), M32.ToString(format, provider), M33.ToString(format, provider));
        }
        #endregion

        #region Перегрузки

        public override int GetHashCode() {
			return  M11.GetHashCode() ^ M12.GetHashCode() ^ M13.GetHashCode() ^
				    M21.GetHashCode() ^ M22.GetHashCode() ^ M23.GetHashCode() ^
				    M31.GetHashCode() ^ M32.GetHashCode() ^ M33.GetHashCode();
		}

		public override bool Equals(object obj) {
			if (obj is Matrix3f) {
                Matrix3f m = (Matrix3f)obj;
				return  (M11 == m.M11) && (M12 == m.M12) && (M13 == m.M13) &&
					    (M21 == m.M21) && (M22 == m.M22) && (M23 == m.M23) &&
					    (M31 == m.M31) && (M32 == m.M32) && (M33 == m.M33);
			}
			return false;
		}

		public override string ToString() {
			return string.Format("3x3[{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}]",
				M11, M12, M13, M21, M22, M23, M31, M32, M33);
		}
		#endregion

        #region Операторы

        public static bool operator ==(Matrix3f left, Matrix3f right) {
			return ValueType.Equals(left, right);
		}

        public static bool operator !=(Matrix3f left, Matrix3f right) {
			return !ValueType.Equals(left, right);
		}

        public static Matrix3f operator +(Matrix3f left, Matrix3f right) {
            return Matrix3f.Add(left, right); ;
		}

        public static Matrix3f operator +(Matrix3f matrix, float scalar) {
            return Matrix3f.Add(matrix, scalar);
		}

        public static Matrix3f operator +(float scalar, Matrix3f matrix) {
            return Matrix3f.Add(matrix, scalar);
		}

        public static Matrix3f operator -(Matrix3f left, Matrix3f right) {
            return Matrix3f.Subtract(left, right); ;
		}

        public static Matrix3f operator -(Matrix3f matrix, float scalar) {
            return Matrix3f.Subtract(matrix, scalar);
		}

        public static Matrix3f operator *(Matrix3f left, Matrix3f right) {
            return Matrix3f.Multiply(left, right); ;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3f operator ^(Matrix3f left, Matrix3f right) { unchecked {
            return Matrix3f.Multiply(right, left); ;
		}}

        public static Vector3f operator *(Matrix3f matrix, Vector3f vector) {
            return Matrix3f.Transform(matrix, vector);
		}

		public unsafe float this[int index] {
			get {
				if (index < 0 || index >= 9)
					throw new IndexOutOfRangeException("Недопустимый индекс элемента матрицы!");
				fixed (float* f = &M11)
					return *(f + index);
			}
			set {
				if (index < 0 || index >= 9)
                    throw new IndexOutOfRangeException("Недопустимый индекс элемента матрицы!");
				fixed (float* f = &M11)
					*(f + index) = value;
			}
		}

		public float this[int row, int column] {
			get { return this[(row-1) * 3 + (column-1)]; }
			set { this[(row-1) * 3 + (column-1)] = value; }
		}

        #endregion
    }

	
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = __SIZE)]
    public struct Matrix3d : ICloneable, IEquatable<Matrix3d>, IFormattable
    {
        public const int __SIZE = 72;
        [FieldOffset( 0)] public double M11;     // ┎─────┬─────┬─────┓
        [FieldOffset( 8)] public double M12;     // ┃ M11 │ M12 │ M13 ┃
        [FieldOffset(16)] public double M13;     // ┠━━━━━┼━━━━━┼━━━━━┫
        [FieldOffset(24)] public double M21;     // ┃ M21 │ M22 │ M23 ┃
        [FieldOffset(32)] public double M22;     // ┠━━━━━┼━━━━━┼━━━━━┫
        [FieldOffset(40)] public double M23;     // ┃ M31 │ M32 │ M33 ┃
        [FieldOffset(48)] public double M31;     // ┖─────┴─────┴─────┚
        [FieldOffset(56)] public double M32;
        [FieldOffset(64)] public double M33;

        #region Конструкторы

        public Matrix3d( double m11, double m12, double m13,
			             double m21, double m22, double m23,
			             double m31, double m32, double m33 ) {
			M11 = m11; M12 = m12; M13 = m13;
			M21 = m21; M22 = m22; M23 = m23;
			M31 = m31; M32 = m32; M33 = m33;
		}

        public Matrix3d(double[] elements) {
			Debug.Assert(elements != null && elements.Length == 9);
			M11 = elements[ 0]; M12 = elements[ 1]; M13 = elements[ 2];
			M21 = elements[ 3]; M22 = elements[ 4]; M23 = elements[ 5];
			M31 = elements[ 6]; M32 = elements[ 7]; M33 = elements[ 8];
		}

        public Matrix3d(List<double> elements) {
            Debug.Assert(elements != null && elements.Count == 9);
			M11 = elements[ 0]; M12 = elements[ 1]; M13 = elements[ 2];
			M21 = elements[ 3]; M22 = elements[ 4]; M23 = elements[ 5];
			M31 = elements[ 6]; M32 = elements[ 7]; M33 = elements[ 8];
		}

        public Matrix3d(decimal[] elements) : this(elements.Select(e => (double)e).ToArray()) { }

        public Matrix3d(Matrix3d m) {
            M11 = m.M11; M12 = m.M12; M13 = m.M13;
            M21 = m.M21; M22 = m.M22; M23 = m.M23;
            M31 = m.M31; M32 = m.M32; M33 = m.M33;
		}
        #endregion

        #region Константы

        public static readonly Matrix3d Zero = new Matrix3d(0d,0d,0d, 0d,0d,0d, 0d,0d,0d);

        public static readonly Matrix3d Identity = new Matrix3d(1d, 0d, 0d,
                                                                0d, 1d, 0d,
                                                                0d, 0d, 1d );
        #endregion

        #region Статические методы

		public static Matrix3d Add(Matrix3d left, Matrix3d right) {
			return new Matrix3d(
				left.M11 + right.M11, left.M12 + right.M12, left.M13 + right.M13, 
				left.M21 + right.M21, left.M22 + right.M22, left.M23 + right.M23, 
				left.M31 + right.M31, left.M32 + right.M32, left.M33 + right.M33);
		}

		public static Matrix3d Add(Matrix3d matrix, double scalar) {
			return new Matrix3d(
				matrix.M11 + scalar, matrix.M12 + scalar, matrix.M13 + scalar, 
				matrix.M21 + scalar, matrix.M22 + scalar, matrix.M23 + scalar, 
				matrix.M31 + scalar, matrix.M32 + scalar, matrix.M33 + scalar);
		}

		public static void Add(Matrix3d left, Matrix3d right, ref Matrix3d result) {
			result.M11 = left.M11 + right.M11;
			result.M12 = left.M12 + right.M12;
			result.M13 = left.M13 + right.M13;

			result.M21 = left.M21 + right.M21;
			result.M22 = left.M22 + right.M22;
			result.M23 = left.M23 + right.M23;

			result.M31 = left.M31 + right.M31;
			result.M32 = left.M32 + right.M32;
			result.M33 = left.M33 + right.M33;
		}

		public static void Add(Matrix3d matrix, double scalar, ref Matrix3d result) {
			result.M11 = matrix.M11 + scalar;
			result.M12 = matrix.M12 + scalar;
			result.M13 = matrix.M13 + scalar;

			result.M21 = matrix.M21 + scalar;
			result.M22 = matrix.M22 + scalar;
			result.M23 = matrix.M23 + scalar;

			result.M31 = matrix.M31 + scalar;
			result.M32 = matrix.M32 + scalar;
			result.M33 = matrix.M33 + scalar;
		}

		public static Matrix3d Subtract(Matrix3d left, Matrix3d right) {
			return new Matrix3d(
				left.M11 - right.M11, left.M12 - right.M12, left.M13 - right.M13,
				left.M21 - right.M21, left.M22 - right.M22, left.M23 - right.M23,
				left.M31 - right.M31, left.M32 - right.M32, left.M33 - right.M33);
		}

		public static Matrix3d Subtract(Matrix3d matrix, double scalar) {
			return new Matrix3d(
				matrix.M11 - scalar, matrix.M12 - scalar, matrix.M13 - scalar,
				matrix.M21 - scalar, matrix.M22 - scalar, matrix.M23 - scalar,
				matrix.M31 - scalar, matrix.M32 - scalar, matrix.M33 - scalar);
		}

		public static void Subtract(Matrix3d left, Matrix3d right, ref Matrix3d result) {
			result.M11 = left.M11 - right.M11;
			result.M12 = left.M12 - right.M12;
			result.M13 = left.M13 - right.M13;

			result.M21 = left.M21 - right.M21;
			result.M22 = left.M22 - right.M22;
			result.M23 = left.M23 - right.M23;

			result.M31 = left.M31 - right.M31;
			result.M32 = left.M32 - right.M32;
			result.M33 = left.M33 - right.M33;
		}

		public static void Subtract(Matrix3d matrix, double scalar, ref Matrix3d result) {
			result.M11 = matrix.M11 - scalar;
			result.M12 = matrix.M12 - scalar;
			result.M13 = matrix.M13 - scalar;

			result.M21 = matrix.M21 - scalar;
			result.M22 = matrix.M22 - scalar;
			result.M23 = matrix.M23 - scalar;

			result.M31 = matrix.M31 - scalar;
			result.M32 = matrix.M32 - scalar;
			result.M33 = matrix.M33 - scalar;
		}

		public static Matrix3d Multiply(Matrix3d left, Matrix3d right) {
			return new Matrix3d(
				left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31,
				left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32,
				left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33,

				left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31,
				left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32,
				left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33,

				left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31,
				left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32,
				left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33);
		}

		public static void Multiply(Matrix3d left, Matrix3d right, ref Matrix3d result) {
			result.M11 = left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31;
			result.M12 = left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32;
			result.M13 = left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33;

			result.M21 = left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31;
			result.M22 = left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32;
			result.M23 = left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33;

			result.M31 = left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31;
			result.M32 = left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32;
			result.M33 = left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33;
		}

		public static Vector3d Transform(Matrix3d matrix, Vector3d vector) {
			return new Vector3d(
				(matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z),
				(matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z),
				(matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z));
		}
		
		public static void Transform(Matrix3d matrix, Vector3d vector, ref Vector3d result) {
			result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z);
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z);
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z);
		}

        public static Matrix3d Invert(Matrix3d matrix) {
            double invdet = 1 / matrix.GetDeterminant();
            return new Matrix3d() {
                M11 = invdet * (matrix.M22 * matrix.M33 - matrix.M32 * matrix.M23),
                M12 = invdet * (matrix.M13 * matrix.M32 - matrix.M12 * matrix.M33),
                M13 = invdet * (matrix.M12 * matrix.M23 - matrix.M13 * matrix.M22),

                M21 = invdet * (matrix.M23 * matrix.M31 - matrix.M21 * matrix.M33),
                M22 = invdet * (matrix.M11 * matrix.M33 - matrix.M13 * matrix.M31),
                M23 = invdet * (matrix.M21 * matrix.M13 - matrix.M11 * matrix.M23),
            
                M31 = invdet * (matrix.M21 * matrix.M32 - matrix.M31 * matrix.M22),
                M32 = invdet * (matrix.M31 * matrix.M12 - matrix.M11 * matrix.M32),
                M33 = invdet * (matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12)
            };
        }

        public static Matrix4d TransposeInvert(Matrix4d matrix) {
            double det =  matrix.M11 * (matrix.M22 * matrix.M33 - matrix.M23 * matrix.M32)
                        - matrix.M12 * (matrix.M21 * matrix.M33 - matrix.M23 * matrix.M31) 
                        + matrix.M13 * (matrix.M21 * matrix.M32 - matrix.M22 * matrix.M31);
            double invdet = 1 / det;
            return new Matrix4d() {
                M11 = invdet * (matrix.M22 * matrix.M33 - matrix.M32 * matrix.M23),
                M12 = invdet * (matrix.M23 * matrix.M31 - matrix.M21 * matrix.M33),
                M13 = invdet * (matrix.M21 * matrix.M32 - matrix.M31 * matrix.M22),
                M21 = invdet * (matrix.M13 * matrix.M32 - matrix.M12 * matrix.M33),
                M22 = invdet * (matrix.M11 * matrix.M33 - matrix.M13 * matrix.M31),
                M23 = invdet * (matrix.M31 * matrix.M12 - matrix.M11 * matrix.M32),
                M31 = invdet * (matrix.M12 * matrix.M23 - matrix.M13 * matrix.M22),
                M32 = invdet * (matrix.M21 * matrix.M13 - matrix.M11 * matrix.M23),
                M33 = invdet * (matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12),
                M14 = 0, M24 = 0, M34 = 0, M41 = 0, M42 = 0, M43 = 0, M44 = 0
            };
        }

        public static Matrix4d NormalVecTransf(Matrix4d matrix) {
            return new Matrix4d(
                matrix.M33 * matrix.M22 - matrix.M23 * matrix.M32, 
                matrix.M23 * matrix.M31 - matrix.M21 * matrix.M33, 
                matrix.M21 * matrix.M32 - matrix.M31 * matrix.M22, 0,
                matrix.M13 * matrix.M32 - matrix.M33 * matrix.M12,
                matrix.M33 * matrix.M11 - matrix.M13 * matrix.M31,
                matrix.M31 * matrix.M12 - matrix.M11 * matrix.M32, 0,
                matrix.M23 * matrix.M12 - matrix.M13 * matrix.M22,
                matrix.M21 * matrix.M13 - matrix.M23 * matrix.M11,
                matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12, 0,
                0,   0,   0,   0);
        }

		public static Matrix3d Transpose(Matrix3d m) {
			Matrix3d t = new Matrix3d(m);
			t.Transpose();
			return t;
		}

        public static bool ApproxEqual(Matrix3d left, Matrix3d right, double tolerance) {
			return ((System.Math.Abs(left.M11 - right.M11) <= tolerance) &&
                    (System.Math.Abs(left.M12 - right.M12) <= tolerance) &&
				    (System.Math.Abs(left.M13 - right.M13) <= tolerance) &&

                    (System.Math.Abs(left.M21 - right.M21) <= tolerance) &&
                    (System.Math.Abs(left.M22 - right.M22) <= tolerance) &&
				    (System.Math.Abs(left.M23 - right.M23) <= tolerance) &&

                    (System.Math.Abs(left.M31 - right.M31) <= tolerance) &&
                    (System.Math.Abs(left.M32 - right.M32) <= tolerance) &&
				    (System.Math.Abs(left.M33 - right.M33) <= tolerance) );
		}
		#endregion

        #region Свойства и Методы

        public double Trace {
			get { return M11 + M22 + M33; }
		}  

		public double GetDeterminant() {
            return  M11 * M22 * M33 + M12 * M23 * M31 + M13 * M21 * M32 -
                    M13 * M22 * M31 - M11 * M23 * M32 - M12 * M21 * M33;
		}

        public Matrix3d Invert() {
            return Invert(this);
        }

		public void Transpose() {
		    double temp;
		    temp = M12;  M12 = M21;  M21 = temp;
            temp = M13;  M13 = M31;  M31 = temp;
            temp = M23;  M23 = M32;  M32 = temp;
		}

        public double[] ToArray(bool glorder = false) {
            return glorder 
                ? new double[9] { M11, M21, M31, M12, M22, M32, M13, M23, M33 }
                : new double[9] { M11, M12, M13, M21, M22, M23, M31, M32, M33 };
	    }

        public float[] ToFloatArray(bool glorder = false) {
            return ToArray(glorder).Select(v => (float)v).ToArray();
	    }

        public decimal[] ToDecimalArray(bool glorder = false) {
            return ToArray(glorder).Select(v => (decimal)v).ToArray();
	    }
        #endregion

        #region Реализация интерфейса ICloneable

        object ICloneable.Clone() {
            return new Matrix3d(this);
        }

        public Matrix3d Clone() {
            return new Matrix3d(this);
        }
        #endregion

        #region Реализация интерфейса IEquatable<Vector4>

        bool IEquatable<Matrix3d>.Equals(Matrix3d m) {
            return  (M11 == m.M11) && (M12 == m.M12) && (M13 == m.M13) &&
				    (M21 == m.M21) && (M22 == m.M22) && (M23 == m.M23) &&
				    (M31 == m.M31) && (M32 == m.M32) && (M33 == m.M33);
        }

        public bool Equals(Matrix3d m) {
            return  (M11 == m.M11) && (M12 == m.M12) && (M13 == m.M13) &&
				    (M21 == m.M21) && (M22 == m.M22) && (M23 == m.M23) && 
				    (M31 == m.M31) && (M32 == m.M32) && (M33 == m.M33);
        }
        #endregion

        #region Реализация интерфейса IFormattable

        public string ToString(string format) {
            return string.Format("3x3[{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}]",
                M11.ToString(format, null), M12.ToString(format, null), M13.ToString(format, null),
                M21.ToString(format, null), M22.ToString(format, null), M23.ToString(format, null),
                M31.ToString(format, null), M32.ToString(format, null), M33.ToString(format, null));
        }

        string IFormattable.ToString(string format, IFormatProvider provider) {
            return ToString(format, provider);
        }

        public string ToString(string format, IFormatProvider provider) {
            return string.Format("3x3[{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}]",
                M11.ToString(format, provider), M12.ToString(format, provider), M13.ToString(format, provider),
                M21.ToString(format, provider), M22.ToString(format, provider), M23.ToString(format, provider),
                M31.ToString(format, provider), M32.ToString(format, provider), M33.ToString(format, provider));
        }
        #endregion

        #region Перегрузки

        public override int GetHashCode() {
			return  M11.GetHashCode() ^ M12.GetHashCode() ^ M13.GetHashCode() ^
				    M21.GetHashCode() ^ M22.GetHashCode() ^ M23.GetHashCode() ^
				    M31.GetHashCode() ^ M32.GetHashCode() ^ M33.GetHashCode();
		}

		public override bool Equals(object obj) {
			if (obj is Matrix3d) {
                Matrix3d m = (Matrix3d)obj;
				return  (M11 == m.M11) && (M12 == m.M12) && (M13 == m.M13) &&
					    (M21 == m.M21) && (M22 == m.M22) && (M23 == m.M23) &&
					    (M31 == m.M31) && (M32 == m.M32) && (M33 == m.M33);
			}
			return false;
		}

		public override string ToString() {
			return string.Format("3x3[{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}]",
				M11, M12, M13, M21, M22, M23, M31, M32, M33);
		}
		#endregion

        #region Операторы

         public static bool operator ==(Matrix3d left, Matrix3d right) {
			return ValueType.Equals(left, right);
		}

        public static bool operator !=(Matrix3d left, Matrix3d right) {
			return !ValueType.Equals(left, right);
		}

        public static Matrix3d operator +(Matrix3d left, Matrix3d right) {
            return Matrix3d.Add(left, right); ;
		}

        public static Matrix3d operator +(Matrix3d matrix, double scalar) {
            return Matrix3d.Add(matrix, scalar);
		}

        public static Matrix3d operator +(double scalar, Matrix3d matrix) {
            return Matrix3d.Add(matrix, scalar);
		}

        public static Matrix3d operator -(Matrix3d left, Matrix3d right) {
            return Matrix3d.Subtract(left, right); ;
		}

        public static Matrix3d operator -(Matrix3d matrix, double scalar) {
            return Matrix3d.Subtract(matrix, scalar);
		}

        public static Matrix3d operator *(Matrix3d left, Matrix3d right) {
            return Matrix3d.Multiply(left, right); ;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3d operator ^(Matrix3d left, Matrix3d right) { unchecked {
            return Matrix3d.Multiply(right, left); ;
		}}

        public static Vector3d operator *(Matrix3d matrix, Vector3d vector) {
            return Matrix3d.Transform(matrix, vector);
		}

		public unsafe double this[int index] {
			get {
				if (index < 0 || index >= 9)
					throw new IndexOutOfRangeException("Недопустимый индекс элемента матрицы!");
				fixed (double* f = &M11)
					return *(f + index);
			}
			set {
				if (index < 0 || index >= 9)
                    throw new IndexOutOfRangeException("Недопустимый индекс элемента матрицы!");
				fixed (double* f = &M11)
					*(f + index) = value;
			}
		}

		public double this[int row, int column] {
			get { return this[(row-1) * 3 + (column-1)]; }
			set { this[(row-1) * 3 + (column-1)] = value; }
		}

        #endregion
    }


    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = __SIZE)]
    public struct Matrix4f : ICloneable, IEquatable<Matrix4f>, IFormattable
    {
        public const int __SIZE = 64;
        [FieldOffset( 0)] public float M11;     // ┎─────┬─────┬─────┬─────┓
        [FieldOffset( 4)] public float M12;     // ┃ M11 │ M12 │ M13 │ M14 ┃
        [FieldOffset( 8)] public float M13;     // ┠━━━━━┼━━━━━┼━━━━━┼━━━━━┫
        [FieldOffset(12)] public float M14;     // ┃ M21 │ M22 │ M23 │ M24 ┃
        [FieldOffset(16)] public float M21;     // ┠━━━━━┼━━━━━┼━━━━━┼━━━━━┫
        [FieldOffset(20)] public float M22;     // ┃ M31 │ M32 │ M34 │ M34 ┃
        [FieldOffset(24)] public float M23;     // ┠━━━━━┼━━━━━┼━━━━━┼━━━━━┫
        [FieldOffset(28)] public float M24;     // ┃ M41 │ M42 │ M43 │ M44 ┃
        [FieldOffset(32)] public float M31;     // ┖─────┴─────┴─────┴─────┚
        [FieldOffset(36)] public float M32;
        [FieldOffset(40)] public float M33;
        [FieldOffset(44)] public float M34;
        [FieldOffset(48)] public float M41;
        [FieldOffset(52)] public float M42;
        [FieldOffset(56)] public float M43;
        [FieldOffset(60)] public float M44;


        #region Конструкторы

        public Matrix4f( float m11, float m12, float m13, float m14,
			             float m21, float m22, float m23, float m24,
			             float m31, float m32, float m33, float m34,
			             float m41, float m42, float m43, float m44 ) {
			M11 = m11; M12 = m12; M13 = m13; M14 = m14;
			M21 = m21; M22 = m22; M23 = m23; M24 = m24;
			M31 = m31; M32 = m32; M33 = m33; M34 = m34;
			M41 = m41; M42 = m42; M43 = m43; M44 = m44;
		}

        public Matrix4f(float[] elements) {
			Debug.Assert(elements != null && elements.Length == 16);
			M11 = elements[ 0]; M12 = elements[ 1]; M13 = elements[ 2]; M14 = elements[ 3];
			M21 = elements[ 4]; M22 = elements[ 5]; M23 = elements[ 6]; M24 = elements[ 7];
			M31 = elements[ 8]; M32 = elements[ 9]; M33 = elements[10]; M34 = elements[11];
			M41 = elements[12]; M42 = elements[13]; M43 = elements[14]; M44 = elements[15];
		}

        public Matrix4f(List<float> elements) {
            Debug.Assert(elements != null && elements.Count == 16);
			M11 = elements[ 0]; M12 = elements[ 1]; M13 = elements[ 2]; M14 = elements[ 3];
			M21 = elements[ 4]; M22 = elements[ 5]; M23 = elements[ 6]; M24 = elements[ 7];
			M31 = elements[ 8]; M32 = elements[ 9]; M33 = elements[10]; M34 = elements[11];
			M41 = elements[12]; M42 = elements[13]; M43 = elements[14]; M44 = elements[15];
		}

        public Matrix4f(decimal[] elements) : this(elements.Select(e => (float)e).ToArray()) { }

        public Matrix4f(Matrix4f m) {
            M11 = m.M11; M12 = m.M12; M13 = m.M13; M14 = m.M14;
            M21 = m.M21; M22 = m.M22; M23 = m.M23; M24 = m.M24;
            M31 = m.M31; M32 = m.M32; M33 = m.M33; M34 = m.M34;
            M41 = m.M41; M42 = m.M42; M43 = m.M43; M44 = m.M44;
		}
        #endregion

        #region Константы

        public static readonly Matrix4f Zero = new Matrix4f(0f,0f,0f,0f, 0f,0f,0f,0f, 0f,0f,0f,0f, 0f,0f,0f,0f);

        public static readonly Matrix4f Identity = new Matrix4f(1f, 0f, 0f, 0f,
                                                                0f, 1f, 0f, 0f,
                                                                0f, 0f, 1f, 0f,
                                                                0f, 0f, 0f, 1f );
        #endregion

        #region Статические методы

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4f Add(Matrix4f left, Matrix4f right) { unchecked {
            return new Matrix4f(
				left.M11 + right.M11, left.M12 + right.M12, left.M13 + right.M13, left.M14 + right.M14,
				left.M21 + right.M21, left.M22 + right.M22, left.M23 + right.M23, left.M24 + right.M24,
				left.M31 + right.M31, left.M32 + right.M32, left.M33 + right.M33, left.M34 + right.M34,
				left.M41 + right.M41, left.M42 + right.M42, left.M43 + right.M43, left.M44 + right.M44);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4f Add(Matrix4f matrix, float scalar) { unchecked {
            return new Matrix4f(
				matrix.M11 + scalar, matrix.M12 + scalar, matrix.M13 + scalar, matrix.M14 + scalar,
				matrix.M21 + scalar, matrix.M22 + scalar, matrix.M23 + scalar, matrix.M24 + scalar,
				matrix.M31 + scalar, matrix.M32 + scalar, matrix.M33 + scalar, matrix.M34 + scalar,
				matrix.M41 + scalar, matrix.M42 + scalar, matrix.M43 + scalar, matrix.M44 + scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(Matrix4f left, Matrix4f right, ref Matrix4f result) { unchecked {
			result.M11 = left.M11 + right.M11;
			result.M12 = left.M12 + right.M12;
			result.M13 = left.M13 + right.M13;
			result.M14 = left.M14 + right.M14;

			result.M21 = left.M21 + right.M21;
			result.M22 = left.M22 + right.M22;
			result.M23 = left.M23 + right.M23;
			result.M24 = left.M24 + right.M24;

			result.M31 = left.M31 + right.M31;
			result.M32 = left.M32 + right.M32;
			result.M33 = left.M33 + right.M33;
			result.M34 = left.M34 + right.M34;

			result.M41 = left.M41 + right.M41;
			result.M42 = left.M42 + right.M42;
			result.M43 = left.M43 + right.M43;
			result.M44 = left.M44 + right.M44;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(Matrix4f matrix, float scalar, ref Matrix4f result) { unchecked {
			result.M11 = matrix.M11 + scalar;
			result.M12 = matrix.M12 + scalar;
			result.M13 = matrix.M13 + scalar;
			result.M14 = matrix.M14 + scalar;

			result.M21 = matrix.M21 + scalar;
			result.M22 = matrix.M22 + scalar;
			result.M23 = matrix.M23 + scalar;
			result.M24 = matrix.M24 + scalar;

			result.M31 = matrix.M31 + scalar;
			result.M32 = matrix.M32 + scalar;
			result.M33 = matrix.M33 + scalar;
			result.M34 = matrix.M34 + scalar;

			result.M41 = matrix.M41 + scalar;
			result.M42 = matrix.M42 + scalar;
			result.M43 = matrix.M43 + scalar;
			result.M44 = matrix.M44 + scalar;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4f Subtract(Matrix4f left, Matrix4f right) { unchecked {
            return new Matrix4f(
				left.M11 - right.M11, left.M12 - right.M12, left.M13 - right.M13, left.M14 - right.M14,
				left.M21 - right.M21, left.M22 - right.M22, left.M23 - right.M23, left.M24 - right.M24,
				left.M31 - right.M31, left.M32 - right.M32, left.M33 - right.M33, left.M34 - right.M34,
				left.M41 - right.M41, left.M42 - right.M42, left.M43 - right.M43, left.M44 - right.M44);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4f Subtract(Matrix4f matrix, float scalar) { unchecked {
            return new Matrix4f(
				matrix.M11 - scalar, matrix.M12 - scalar, matrix.M13 - scalar, matrix.M14 - scalar,
				matrix.M21 - scalar, matrix.M22 - scalar, matrix.M23 - scalar, matrix.M24 - scalar,
				matrix.M31 - scalar, matrix.M32 - scalar, matrix.M33 - scalar, matrix.M34 - scalar,
				matrix.M41 - scalar, matrix.M42 - scalar, matrix.M43 - scalar, matrix.M44 - scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(Matrix4f left, Matrix4f right, ref Matrix4f result) { unchecked {
			result.M11 = left.M11 - right.M11;
			result.M12 = left.M12 - right.M12;
			result.M13 = left.M13 - right.M13;
			result.M14 = left.M14 - right.M14;

			result.M21 = left.M21 - right.M21;
			result.M22 = left.M22 - right.M22;
			result.M23 = left.M23 - right.M23;
			result.M24 = left.M24 - right.M24;

			result.M31 = left.M31 - right.M31;
			result.M32 = left.M32 - right.M32;
			result.M33 = left.M33 - right.M33;
			result.M34 = left.M34 - right.M34;

			result.M41 = left.M41 - right.M41;
			result.M42 = left.M42 - right.M42;
			result.M43 = left.M43 - right.M43;
			result.M44 = left.M44 - right.M44;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(Matrix4f matrix, float scalar, ref Matrix4f result) { unchecked {
			result.M11 = matrix.M11 - scalar;
			result.M12 = matrix.M12 - scalar;
			result.M13 = matrix.M13 - scalar;
			result.M14 = matrix.M14 - scalar;

			result.M21 = matrix.M21 - scalar;
			result.M22 = matrix.M22 - scalar;
			result.M23 = matrix.M23 - scalar;
			result.M24 = matrix.M24 - scalar;

			result.M31 = matrix.M31 - scalar;
			result.M32 = matrix.M32 - scalar;
			result.M33 = matrix.M33 - scalar;
			result.M34 = matrix.M34 - scalar;

			result.M41 = matrix.M41 - scalar;
			result.M42 = matrix.M42 - scalar;
			result.M43 = matrix.M43 - scalar;
			result.M44 = matrix.M44 - scalar;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4f Multiply(Matrix4f left, Matrix4f right) { unchecked {
            return new Matrix4f(
				left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31 + left.M14 * right.M41,
				left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32 + left.M14 * right.M42,
				left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33 + left.M14 * right.M43,
				left.M11 * right.M14 + left.M12 * right.M24 + left.M13 * right.M34 + left.M14 * right.M44,

				left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31 + left.M24 * right.M41,
				left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32 + left.M24 * right.M42,
				left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33 + left.M24 * right.M43,
				left.M21 * right.M14 + left.M22 * right.M24 + left.M23 * right.M34 + left.M24 * right.M44,

				left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31 + left.M34 * right.M41,
				left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32 + left.M34 * right.M42,
				left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33 + left.M34 * right.M43,
				left.M31 * right.M14 + left.M32 * right.M24 + left.M33 * right.M34 + left.M34 * right.M44,

				left.M41 * right.M11 + left.M42 * right.M21 + left.M43 * right.M31 + left.M44 * right.M41,
				left.M41 * right.M12 + left.M42 * right.M22 + left.M43 * right.M32 + left.M44 * right.M42,
				left.M41 * right.M13 + left.M42 * right.M23 + left.M43 * right.M33 + left.M44 * right.M43,
				left.M41 * right.M14 + left.M42 * right.M24 + left.M43 * right.M34 + left.M44 * right.M44);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(Matrix4f left, Matrix4f right, ref Matrix4f result) { unchecked {
			result.M11 = left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31 + left.M14 * right.M41;
			result.M12 = left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32 + left.M14 * right.M42;
			result.M13 = left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33 + left.M14 * right.M43;
			result.M14 = left.M11 * right.M14 + left.M12 * right.M24 + left.M13 * right.M34 + left.M14 * right.M44;

			result.M21 = left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31 + left.M24 * right.M41;
			result.M22 = left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32 + left.M24 * right.M42;
			result.M23 = left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33 + left.M24 * right.M43;
			result.M24 = left.M21 * right.M14 + left.M22 * right.M24 + left.M23 * right.M34 + left.M24 * right.M44;

			result.M31 = left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31 + left.M34 * right.M41;
			result.M32 = left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32 + left.M34 * right.M42;
			result.M33 = left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33 + left.M34 * right.M43;
			result.M34 = left.M31 * right.M14 + left.M32 * right.M24 + left.M33 * right.M34 + left.M34 * right.M44;

			result.M41 = left.M41 * right.M11 + left.M42 * right.M21 + left.M43 * right.M31 + left.M44 * right.M41;
			result.M42 = left.M41 * right.M12 + left.M42 * right.M22 + left.M43 * right.M32 + left.M44 * right.M42;
			result.M43 = left.M41 * right.M13 + left.M42 * right.M23 + left.M43 * right.M33 + left.M44 * right.M43;
			result.M44 = left.M41 * right.M14 + left.M42 * right.M24 + left.M43 * right.M34 + left.M44 * right.M44;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f Transform(Matrix4f matrix, Vector4f vector) { unchecked {
			return new Vector4f(
				(matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z) + (matrix.M14 * vector.W),
				(matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z) + (matrix.M24 * vector.W),
				(matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z) + (matrix.M34 * vector.W),
				(matrix.M41 * vector.X) + (matrix.M42 * vector.Y) + (matrix.M43 * vector.Z) + (matrix.M44 * vector.W));
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(Matrix4f matrix, Vector4f vector, ref Vector4f result) { unchecked {
			result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z) + (matrix.M14 * vector.W);
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z) + (matrix.M24 * vector.W);
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z) + (matrix.M34 * vector.W);
			result.W = (matrix.M41 * vector.X) + (matrix.M42 * vector.Y) + (matrix.M43 * vector.Z) + (matrix.M44 * vector.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dPoint(Matrix4f matrix, Vector4f vector, ref Vector4f result) { unchecked {
            Debug.Assert(vector.W == 1d, "vector.W != 1");
			result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z) + matrix.M14;
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z) + matrix.M24;
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z) + matrix.M34;
			result.W = (matrix.M41 * vector.X) + (matrix.M42 * vector.Y) + (matrix.M43 * vector.Z) + matrix.M44;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dVect(Matrix4f matrix, Vector4f vector, ref Vector4f result) { unchecked {
            Debug.Assert(vector.W == 0d, "vector.W != 0");
			result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z);
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z);
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z);
			result.W = (matrix.M41 * vector.X) + (matrix.M42 * vector.Y) + (matrix.M43 * vector.Z);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dPoint(Matrix4f matrix, Vector3f vector, ref Vector3f result) { unchecked {
            result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z) + matrix.M14;
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z) + matrix.M24;
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z) + matrix.M34;
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dVect(Matrix4f matrix, Vector3f vector, ref Vector3f result) { unchecked { ;
			result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z);
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z);
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z);
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dPoint(Matrix4f matrix, Vector2f vector, ref Vector3f result) { unchecked {
            result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + matrix.M14;
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + matrix.M24;
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + matrix.M34;
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dVect(Matrix4f matrix, Vector2f vector, ref Vector3f result) { unchecked { ;
			result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y);
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y);
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y);
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dPoint(Matrix4f matrix, Vector2f vector, ref Vector2f result) { unchecked {
            result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + matrix.M14;
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + matrix.M24;
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dVect(Matrix4f matrix, Vector2f vector, ref Vector2f result) { unchecked { ;
			result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y);
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y);
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4f Invert(Matrix4f matrix) { unchecked {
            var a3434 = matrix.M33 * matrix.M44 - matrix.M34 * matrix.M43 ;
            var a2434 = matrix.M32 * matrix.M44 - matrix.M34 * matrix.M42 ;
            var a2334 = matrix.M32 * matrix.M43 - matrix.M33 * matrix.M42 ;
            var a1434 = matrix.M31 * matrix.M44 - matrix.M34 * matrix.M41 ;
            var a1334 = matrix.M31 * matrix.M43 - matrix.M33 * matrix.M41 ;
            var a1234 = matrix.M31 * matrix.M42 - matrix.M32 * matrix.M41 ;
            var a3424 = matrix.M23 * matrix.M44 - matrix.M24 * matrix.M43 ;
            var a2424 = matrix.M22 * matrix.M44 - matrix.M24 * matrix.M42 ;
            var a2324 = matrix.M22 * matrix.M43 - matrix.M23 * matrix.M42 ;
            var a3423 = matrix.M23 * matrix.M34 - matrix.M24 * matrix.M33 ;
            var a2423 = matrix.M22 * matrix.M34 - matrix.M24 * matrix.M32 ;
            var a2323 = matrix.M22 * matrix.M33 - matrix.M23 * matrix.M32 ;
            var a1424 = matrix.M21 * matrix.M44 - matrix.M24 * matrix.M41 ;
            var a1324 = matrix.M21 * matrix.M43 - matrix.M23 * matrix.M41 ;
            var a1423 = matrix.M21 * matrix.M34 - matrix.M24 * matrix.M31 ;
            var a1323 = matrix.M21 * matrix.M33 - matrix.M23 * matrix.M31 ;
            var a1224 = matrix.M21 * matrix.M42 - matrix.M22 * matrix.M41 ;
            var a1223 = matrix.M21 * matrix.M32 - matrix.M22 * matrix.M31 ;
            var det = matrix.M11 * ( matrix.M22 * a3434 - matrix.M23 * a2434 + matrix.M24 * a2334 ) 
                    - matrix.M12 * ( matrix.M21 * a3434 - matrix.M23 * a1434 + matrix.M24 * a1334 ) 
                    + matrix.M13 * ( matrix.M21 * a2434 - matrix.M22 * a1434 + matrix.M24 * a1234 ) 
                    - matrix.M14 * ( matrix.M21 * a2334 - matrix.M22 * a1334 + matrix.M23 * a1234 ) ;
            det = 1 / det;
            return new Matrix4f() {
               M11 = det *   ( matrix.M22 * a3434 - matrix.M23 * a2434 + matrix.M24 * a2334 ),
               M12 = det * - ( matrix.M12 * a3434 - matrix.M13 * a2434 + matrix.M14 * a2334 ),
               M13 = det *   ( matrix.M12 * a3424 - matrix.M13 * a2424 + matrix.M14 * a2324 ),
               M14 = det * - ( matrix.M12 * a3423 - matrix.M13 * a2423 + matrix.M14 * a2323 ),
               M21 = det * - ( matrix.M21 * a3434 - matrix.M23 * a1434 + matrix.M24 * a1334 ),
               M22 = det *   ( matrix.M11 * a3434 - matrix.M13 * a1434 + matrix.M14 * a1334 ),
               M23 = det * - ( matrix.M11 * a3424 - matrix.M13 * a1424 + matrix.M14 * a1324 ),
               M24 = det *   ( matrix.M11 * a3423 - matrix.M13 * a1423 + matrix.M14 * a1323 ),
               M31 = det *   ( matrix.M21 * a2434 - matrix.M22 * a1434 + matrix.M24 * a1234 ),
               M32 = det * - ( matrix.M11 * a2434 - matrix.M12 * a1434 + matrix.M14 * a1234 ),
               M33 = det *   ( matrix.M11 * a2424 - matrix.M12 * a1424 + matrix.M14 * a1224 ),
               M34 = det * - ( matrix.M11 * a2423 - matrix.M12 * a1423 + matrix.M14 * a1223 ),
               M41 = det * - ( matrix.M21 * a2334 - matrix.M22 * a1334 + matrix.M23 * a1234 ),
               M42 = det *   ( matrix.M11 * a2334 - matrix.M12 * a1334 + matrix.M13 * a1234 ),
               M43 = det * - ( matrix.M11 * a2324 - matrix.M12 * a1324 + matrix.M13 * a1224 ),
               M44 = det *   ( matrix.M11 * a2323 - matrix.M12 * a1323 + matrix.M13 * a1223 ),
            };
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4f Transpose(Matrix4f m) { unchecked {
            Matrix4f t = new Matrix4f(m);
			t.Transpose();
			return t;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproxEqual(Matrix4f left, Matrix4f right, float tolerance) { unchecked {
			return ((System.Math.Abs(left.M11 - right.M11) <= tolerance) &&
                    (System.Math.Abs(left.M12 - right.M12) <= tolerance) &&
				    (System.Math.Abs(left.M13 - right.M13) <= tolerance) &&
                    (System.Math.Abs(left.M14 - right.M14) <= tolerance) &&

                    (System.Math.Abs(left.M21 - right.M21) <= tolerance) &&
                    (System.Math.Abs(left.M22 - right.M22) <= tolerance) &&
				    (System.Math.Abs(left.M23 - right.M23) <= tolerance) &&
                    (System.Math.Abs(left.M24 - right.M24) <= tolerance) &&

                    (System.Math.Abs(left.M31 - right.M31) <= tolerance) &&
                    (System.Math.Abs(left.M32 - right.M32) <= tolerance) &&
				    (System.Math.Abs(left.M33 - right.M33) <= tolerance) &&
                    (System.Math.Abs(left.M34 - right.M34) <= tolerance) &&

                    (System.Math.Abs(left.M41 - right.M41) <= tolerance) &&
                    (System.Math.Abs(left.M42 - right.M42) <= tolerance) &&
				    (System.Math.Abs(left.M43 - right.M43) <= tolerance) &&
                    (System.Math.Abs(left.M44 - right.M44) <= tolerance) );
		}}
        #endregion

        #region Свойства и Методы

        public float Trace {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { unchecked { return M11 + M22 + M33 + M44; } }
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetDeterminant() { unchecked {
        //	float det = 0.0f;
        //	for (int col = 0; col < 4; col++) 
        //		if ((col % 2) == 0) det += this[0, col] * Minor(0, col).Determinant();
        //		else                det -= this[0, col] * Minor(0, col).Determinant();
        //	return det;
			return
				M14 * M23 * M32 * M41 - M13 * M24 * M32 * M41 - M14 * M22 * M33 * M41 + M12 * M24 * M33 * M41 +
				M13 * M22 * M34 * M41 - M12 * M23 * M34 * M41 - M14 * M23 * M31 * M42 + M13 * M24 * M31 * M42 +
				M14 * M21 * M33 * M42 - M11 * M24 * M33 * M42 - M13 * M21 * M34 * M42 + M11 * M23 * M34 * M42 +
				M14 * M22 * M31 * M43 - M12 * M24 * M31 * M43 - M14 * M21 * M32 * M43 + M11 * M24 * M32 * M43 +
				M12 * M21 * M34 * M43 - M11 * M22 * M34 * M43 - M13 * M22 * M31 * M44 + M12 * M23 * M31 * M44 +
				M13 * M21 * M32 * M44 - M11 * M23 * M32 * M44 - M12 * M21 * M33 * M44 + M11 * M22 * M33 * M44;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix4f Invert() { unchecked {
            return Invert(this);
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Transpose() { unchecked {
		    float temp;
		    temp = M12;  M12 = M21;  M21 = temp;
            temp = M13;  M13 = M31;  M31 = temp;
            temp = M14;  M14 = M41;  M41 = temp;
            temp = M23;  M23 = M32;  M32 = temp;
            temp = M24;  M24 = M42;  M42 = temp;
            temp = M34;  M34 = M43;  M43 = temp;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float[] ToArray(bool glorder = false) { unchecked {
            return glorder 
            ? new float[16] { M11, M21, M31, M41, M12, M22, M32, M42, M13, M23, M33, M43, M14, M24, M34, M44 } 
            : new float[16] { M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44 };
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double[] ToDoubeArray(bool glorder = false) { unchecked {
            return ToArray(glorder).Select(v => (double)v).ToArray();
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal[] ToDecimalArray(bool glorder = false) { unchecked {
            return ToArray(glorder).Select(v => (decimal)v).ToArray();
	    }}
        #endregion

        #region Реализация интерфейса ICloneable

        object ICloneable.Clone() {
            return new Matrix4f(this);
        }

        public Matrix4f Clone() {
            return new Matrix4f(this);
        }

        #endregion

        #region Реализация интерфейса IEquatable<Vector4>

        bool IEquatable<Matrix4f>.Equals(Matrix4f m) {
            return  (M11 == m.M11) && (M12 == m.M12) && (M13 == m.M13) && (M14 == m.M14) &&
				    (M21 == m.M21) && (M22 == m.M22) && (M23 == m.M23) && (M24 == m.M24) &&
				    (M31 == m.M31) && (M32 == m.M32) && (M33 == m.M33) && (M34 == m.M34) &&
				    (M41 == m.M41) && (M42 == m.M42) && (M43 == m.M43) && (M44 == m.M44);
        }

        public bool Equals(Matrix4f m) {
            return  (M11 == m.M11) && (M12 == m.M12) && (M13 == m.M13) && (M14 == m.M14) &&
				    (M21 == m.M21) && (M22 == m.M22) && (M23 == m.M23) && (M24 == m.M24) &&
				    (M31 == m.M31) && (M32 == m.M32) && (M33 == m.M33) && (M34 == m.M34) &&
				    (M41 == m.M41) && (M42 == m.M42) && (M43 == m.M43) && (M44 == m.M44);
        }
        #endregion

        #region Реализация интерфейса IFormattable

        public string ToString(string format) {
            return string.Format("4x4[{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}]",
                M11.ToString(format, null), M12.ToString(format, null), M13.ToString(format, null), M14.ToString(format, null),
                M21.ToString(format, null), M22.ToString(format, null), M23.ToString(format, null), M24.ToString(format, null),
                M31.ToString(format, null), M32.ToString(format, null), M33.ToString(format, null), M34.ToString(format, null),
                M41.ToString(format, null), M42.ToString(format, null), M43.ToString(format, null), M44.ToString(format, null));
        }

        string IFormattable.ToString(string format, IFormatProvider provider) {
            return ToString(format, provider);
        }

        public string ToString(string format, IFormatProvider provider) {
            return string.Format("4x4[{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}]",
                M11.ToString(format, provider), M12.ToString(format, provider), M13.ToString(format, provider), M14.ToString(format, provider),
                M21.ToString(format, provider), M22.ToString(format, provider), M23.ToString(format, provider), M24.ToString(format, provider),
                M31.ToString(format, provider), M32.ToString(format, provider), M33.ToString(format, provider), M34.ToString(format, provider),
                M41.ToString(format, provider), M42.ToString(format, provider), M43.ToString(format, provider), M44.ToString(format, provider));
        }
        #endregion

        #region Перегрузки

        public override int GetHashCode() {
			return  M11.GetHashCode() ^ M12.GetHashCode() ^ M13.GetHashCode() ^ M14.GetHashCode() ^
				    M21.GetHashCode() ^ M22.GetHashCode() ^ M23.GetHashCode() ^ M24.GetHashCode() ^
				    M31.GetHashCode() ^ M32.GetHashCode() ^ M33.GetHashCode() ^ M34.GetHashCode() ^
				    M41.GetHashCode() ^ M42.GetHashCode() ^ M43.GetHashCode() ^ M44.GetHashCode();
		}

		public override bool Equals(object obj) {
			if (obj is Matrix4f) {
                Matrix4f m = (Matrix4f)obj;
				return  (M11 == m.M11) && (M12 == m.M12) && (M13 == m.M13) && (M14 == m.M14) &&
					    (M21 == m.M21) && (M22 == m.M22) && (M23 == m.M23) && (M24 == m.M24) &&
					    (M31 == m.M31) && (M32 == m.M32) && (M33 == m.M33) && (M34 == m.M34) &&
					    (M41 == m.M41) && (M42 == m.M42) && (M43 == m.M43) && (M44 == m.M44);
			}
			return false;
		}

		public override string ToString() {
			return string.Format("4x4[{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}]",
				M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44);
		}
		#endregion

        #region Операторы

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Matrix4f left, Matrix4f right) { unchecked {
			return ValueType.Equals(left, right);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Matrix4f left, Matrix4f right) { unchecked {
			return !ValueType.Equals(left, right);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4f operator +(Matrix4f left, Matrix4f right) { unchecked {
            return Matrix4f.Add(left, right); ;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4f operator +(Matrix4f matrix, float scalar) { unchecked {
            return Matrix4f.Add(matrix, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4f operator +(float scalar, Matrix4f matrix) { unchecked {
            return Matrix4f.Add(matrix, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4f operator -(Matrix4f left, Matrix4f right) { unchecked {
            return Matrix4f.Subtract(left, right); ;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4f operator -(Matrix4f matrix, float scalar) { unchecked {
            return Matrix4f.Subtract(matrix, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4f operator *(Matrix4f left, Matrix4f right) { unchecked {
            return Matrix4f.Multiply(left, right); ;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4f operator ^(Matrix4f left, Matrix4f right) { unchecked {
            return Matrix4f.Multiply(right, left); ;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator *(Matrix4f matrix, Vector4f vector) { unchecked {
            return Matrix4f.Transform(matrix, vector);
		}}

		public unsafe float this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { unchecked {
				if (index < 0 || index >= 16)
                    throw new IndexOutOfRangeException("Недопустимый индекс элемента матрицы!");
				fixed (float* f = &M11)
					return *(f + index);
			}}
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
			set { unchecked {
				if (index < 0 || index >= 16)
                    throw new IndexOutOfRangeException("Недопустимый индекс элемента матрицы!");
				fixed (float* f = &M11)
					*(f + index) = value;
			}}
		}

		public float this[int row, int column] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { unchecked { return this[(row - 1) * 4 + (column - 1)]; } }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { unchecked { this[(row - 1) * 4 + (column - 1)] = value; } }
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Matrix3f(Matrix4f matrix) { unchecked {
            return new Matrix3f(matrix.M11, matrix.M12, matrix.M13,
                                matrix.M21, matrix.M22, matrix.M23,
                                matrix.M31, matrix.M32, matrix.M33);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Matrix3d(Matrix4f matrix) { unchecked {
            return new Matrix3d((double)matrix.M11, (double)matrix.M12, (double)matrix.M13,
                                (double)matrix.M21, (double)matrix.M22, (double)matrix.M23,
                                (double)matrix.M31, (double)matrix.M32, (double)matrix.M33);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Matrix4d(Matrix4f matrix) { unchecked {
            return new Matrix4d((double)matrix.M11, (double)matrix.M12, (double)matrix.M13, (double)matrix.M14,
                                (double)matrix.M21, (double)matrix.M22, (double)matrix.M23, (double)matrix.M24,
                                (double)matrix.M31, (double)matrix.M32, (double)matrix.M33, (double)matrix.M34,
                                (double)matrix.M41, (double)matrix.M42, (double)matrix.M43, (double)matrix.M44);
		}}
        #endregion
    }


    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = __SIZE)]
    public struct Matrix4d : ICloneable, IEquatable<Matrix4d>, IFormattable
    {
        public const int __SIZE = 128;
        [FieldOffset(  0)] public double M11;     // ┎─────┬─────┬─────┬─────┓
        [FieldOffset(  8)] public double M12;     // ┃ M11 │ M12 │ M13 │ M14 ┃
        [FieldOffset( 16)] public double M13;     // ┠━━━━━┼━━━━━┼━━━━━┼━━━━━┫
        [FieldOffset( 24)] public double M14;     // ┃ M21 │ M22 │ M23 │ M24 ┃
        [FieldOffset( 32)] public double M21;     // ┠━━━━━┼━━━━━┼━━━━━┼━━━━━┫
        [FieldOffset( 40)] public double M22;     // ┃ M31 │ M32 │ M34 │ M34 ┃
        [FieldOffset( 48)] public double M23;     // ┠━━━━━┼━━━━━┼━━━━━┼━━━━━┫
        [FieldOffset( 56)] public double M24;     // ┃ M41 │ M42 │ M43 │ M44 ┃
        [FieldOffset( 64)] public double M31;     // ┖─────┴─────┴─────┴─────┚
        [FieldOffset( 72)] public double M32;
        [FieldOffset( 80)] public double M33;
        [FieldOffset( 88)] public double M34;
        [FieldOffset( 96)] public double M41;
        [FieldOffset(104)] public double M42;
        [FieldOffset(112)] public double M43;
        [FieldOffset(120)] public double M44;

        #region Конструкторы

        public Matrix4d( double m11, double m12, double m13, double m14,
			             double m21, double m22, double m23, double m24,
			             double m31, double m32, double m33, double m34,
			             double m41, double m42, double m43, double m44 ) {
			M11 = m11; M12 = m12; M13 = m13; M14 = m14;
			M21 = m21; M22 = m22; M23 = m23; M24 = m24;
			M31 = m31; M32 = m32; M33 = m33; M34 = m34;
			M41 = m41; M42 = m42; M43 = m43; M44 = m44;
		}

        public Matrix4d(double[] elements) {
			Debug.Assert(elements != null && elements.Length == 16);
			M11 = elements[ 0]; M12 = elements[ 1]; M13 = elements[ 2]; M14 = elements[ 3];
			M21 = elements[ 4]; M22 = elements[ 5]; M23 = elements[ 6]; M24 = elements[ 7];
			M31 = elements[ 8]; M32 = elements[ 9]; M33 = elements[10]; M34 = elements[11];
			M41 = elements[12]; M42 = elements[13]; M43 = elements[14]; M44 = elements[15];
		}

        public Matrix4d(List<double> elements) {
            Debug.Assert(elements != null && elements.Count == 16);
			M11 = elements[ 0]; M12 = elements[ 1]; M13 = elements[ 2]; M14 = elements[ 3];
			M21 = elements[ 4]; M22 = elements[ 5]; M23 = elements[ 6]; M24 = elements[ 7];
			M31 = elements[ 8]; M32 = elements[ 9]; M33 = elements[10]; M34 = elements[11];
			M41 = elements[12]; M42 = elements[13]; M43 = elements[14]; M44 = elements[15];
		}

        public Matrix4d(decimal[] elements) : this(elements.Select(e => (double)e).ToArray()) { }

        public Matrix4d(Matrix4d m) {
            M11 = m.M11; M12 = m.M12; M13 = m.M13; M14 = m.M14;
            M21 = m.M21; M22 = m.M22; M23 = m.M23; M24 = m.M24;
            M31 = m.M31; M32 = m.M32; M33 = m.M33; M34 = m.M34;
            M41 = m.M41; M42 = m.M42; M43 = m.M43; M44 = m.M44;
		}
        #endregion

        #region Константы

        public static readonly Matrix4d Zero = new Matrix4d(0d,0d,0d,0d, 0d,0d,0d,0d, 0d,0d,0d,0d, 0d,0d,0d,0d);

        public static readonly Matrix4d Identity = new Matrix4d(1d, 0d, 0d, 0d,
                                                                0d, 1d, 0d, 0d,
                                                                0d, 0d, 1d, 0d,
                                                                0d, 0d, 0d, 1d );
        #endregion

        #region Статические методы

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4d Add(Matrix4d left, Matrix4d right) { unchecked {
            return new Matrix4d(
				left.M11 + right.M11, left.M12 + right.M12, left.M13 + right.M13, left.M14 + right.M14,
				left.M21 + right.M21, left.M22 + right.M22, left.M23 + right.M23, left.M24 + right.M24,
				left.M31 + right.M31, left.M32 + right.M32, left.M33 + right.M33, left.M34 + right.M34,
				left.M41 + right.M41, left.M42 + right.M42, left.M43 + right.M43, left.M44 + right.M44);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4d Add(Matrix4d matrix, double scalar) { unchecked {
            return new Matrix4d(
				matrix.M11 + scalar, matrix.M12 + scalar, matrix.M13 + scalar, matrix.M14 + scalar,
				matrix.M21 + scalar, matrix.M22 + scalar, matrix.M23 + scalar, matrix.M24 + scalar,
				matrix.M31 + scalar, matrix.M32 + scalar, matrix.M33 + scalar, matrix.M34 + scalar,
				matrix.M41 + scalar, matrix.M42 + scalar, matrix.M43 + scalar, matrix.M44 + scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(Matrix4d left, Matrix4d right, ref Matrix4d result) { unchecked {
			result.M11 = left.M11 + right.M11;
			result.M12 = left.M12 + right.M12;
			result.M13 = left.M13 + right.M13;
			result.M14 = left.M14 + right.M14;

			result.M21 = left.M21 + right.M21;
			result.M22 = left.M22 + right.M22;
			result.M23 = left.M23 + right.M23;
			result.M24 = left.M24 + right.M24;

			result.M31 = left.M31 + right.M31;
			result.M32 = left.M32 + right.M32;
			result.M33 = left.M33 + right.M33;
			result.M34 = left.M34 + right.M34;

			result.M41 = left.M41 + right.M41;
			result.M42 = left.M42 + right.M42;
			result.M43 = left.M43 + right.M43;
			result.M44 = left.M44 + right.M44;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(Matrix4d matrix, double scalar, ref Matrix4d result) { unchecked {
			result.M11 = matrix.M11 + scalar;
			result.M12 = matrix.M12 + scalar;
			result.M13 = matrix.M13 + scalar;
			result.M14 = matrix.M14 + scalar;

			result.M21 = matrix.M21 + scalar;
			result.M22 = matrix.M22 + scalar;
			result.M23 = matrix.M23 + scalar;
			result.M24 = matrix.M24 + scalar;

			result.M31 = matrix.M31 + scalar;
			result.M32 = matrix.M32 + scalar;
			result.M33 = matrix.M33 + scalar;
			result.M34 = matrix.M34 + scalar;

			result.M41 = matrix.M41 + scalar;
			result.M42 = matrix.M42 + scalar;
			result.M43 = matrix.M43 + scalar;
			result.M44 = matrix.M44 + scalar;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4d Subtract(Matrix4d left, Matrix4d right) { unchecked {
            return new Matrix4d(
				left.M11 - right.M11, left.M12 - right.M12, left.M13 - right.M13, left.M14 - right.M14,
				left.M21 - right.M21, left.M22 - right.M22, left.M23 - right.M23, left.M24 - right.M24,
				left.M31 - right.M31, left.M32 - right.M32, left.M33 - right.M33, left.M34 - right.M34,
				left.M41 - right.M41, left.M42 - right.M42, left.M43 - right.M43, left.M44 - right.M44);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4d Subtract(Matrix4d matrix, double scalar) { unchecked {
            return new Matrix4d(
				matrix.M11 - scalar, matrix.M12 - scalar, matrix.M13 - scalar, matrix.M14 - scalar,
				matrix.M21 - scalar, matrix.M22 - scalar, matrix.M23 - scalar, matrix.M24 - scalar,
				matrix.M31 - scalar, matrix.M32 - scalar, matrix.M33 - scalar, matrix.M34 - scalar,
				matrix.M41 - scalar, matrix.M42 - scalar, matrix.M43 - scalar, matrix.M44 - scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(Matrix4d left, Matrix4d right, ref Matrix4d result) { unchecked {
			result.M11 = left.M11 - right.M11;
			result.M12 = left.M12 - right.M12;
			result.M13 = left.M13 - right.M13;
			result.M14 = left.M14 - right.M14;

			result.M21 = left.M21 - right.M21;
			result.M22 = left.M22 - right.M22;
			result.M23 = left.M23 - right.M23;
			result.M24 = left.M24 - right.M24;

			result.M31 = left.M31 - right.M31;
			result.M32 = left.M32 - right.M32;
			result.M33 = left.M33 - right.M33;
			result.M34 = left.M34 - right.M34;

			result.M41 = left.M41 - right.M41;
			result.M42 = left.M42 - right.M42;
			result.M43 = left.M43 - right.M43;
			result.M44 = left.M44 - right.M44;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(Matrix4d matrix, double scalar, ref Matrix4d result) { unchecked {
			result.M11 = matrix.M11 - scalar;
			result.M12 = matrix.M12 - scalar;
			result.M13 = matrix.M13 - scalar;
			result.M14 = matrix.M14 - scalar;

			result.M21 = matrix.M21 - scalar;
			result.M22 = matrix.M22 - scalar;
			result.M23 = matrix.M23 - scalar;
			result.M24 = matrix.M24 - scalar;

			result.M31 = matrix.M31 - scalar;
			result.M32 = matrix.M32 - scalar;
			result.M33 = matrix.M33 - scalar;
			result.M34 = matrix.M34 - scalar;

			result.M41 = matrix.M41 - scalar;
			result.M42 = matrix.M42 - scalar;
			result.M43 = matrix.M43 - scalar;
			result.M44 = matrix.M44 - scalar;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4d Multiply(Matrix4d left, Matrix4d right) { unchecked {
            return new Matrix4d(
				left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31 + left.M14 * right.M41,
				left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32 + left.M14 * right.M42,
				left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33 + left.M14 * right.M43,
				left.M11 * right.M14 + left.M12 * right.M24 + left.M13 * right.M34 + left.M14 * right.M44,

				left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31 + left.M24 * right.M41,
				left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32 + left.M24 * right.M42,
				left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33 + left.M24 * right.M43,
				left.M21 * right.M14 + left.M22 * right.M24 + left.M23 * right.M34 + left.M24 * right.M44,

				left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31 + left.M34 * right.M41,
				left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32 + left.M34 * right.M42,
				left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33 + left.M34 * right.M43,
				left.M31 * right.M14 + left.M32 * right.M24 + left.M33 * right.M34 + left.M34 * right.M44,

				left.M41 * right.M11 + left.M42 * right.M21 + left.M43 * right.M31 + left.M44 * right.M41,
				left.M41 * right.M12 + left.M42 * right.M22 + left.M43 * right.M32 + left.M44 * right.M42,
				left.M41 * right.M13 + left.M42 * right.M23 + left.M43 * right.M33 + left.M44 * right.M43,
				left.M41 * right.M14 + left.M42 * right.M24 + left.M43 * right.M34 + left.M44 * right.M44);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(Matrix4d left, Matrix4d right, ref Matrix4d result) { unchecked {
			result.M11 = left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31 + left.M14 * right.M41;
			result.M12 = left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32 + left.M14 * right.M42;
			result.M13 = left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33 + left.M14 * right.M43;
			result.M14 = left.M11 * right.M14 + left.M12 * right.M24 + left.M13 * right.M34 + left.M14 * right.M44;

			result.M21 = left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31 + left.M24 * right.M41;
			result.M22 = left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32 + left.M24 * right.M42;
			result.M23 = left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33 + left.M24 * right.M43;
			result.M24 = left.M21 * right.M14 + left.M22 * right.M24 + left.M23 * right.M34 + left.M24 * right.M44;

			result.M31 = left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31 + left.M34 * right.M41;
			result.M32 = left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32 + left.M34 * right.M42;
			result.M33 = left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33 + left.M34 * right.M43;
			result.M34 = left.M31 * right.M14 + left.M32 * right.M24 + left.M33 * right.M34 + left.M34 * right.M44;

			result.M41 = left.M41 * right.M11 + left.M42 * right.M21 + left.M43 * right.M31 + left.M44 * right.M41;
			result.M42 = left.M41 * right.M12 + left.M42 * right.M22 + left.M43 * right.M32 + left.M44 * right.M42;
			result.M43 = left.M41 * right.M13 + left.M42 * right.M23 + left.M43 * right.M33 + left.M44 * right.M43;
			result.M44 = left.M41 * right.M14 + left.M42 * right.M24 + left.M43 * right.M34 + left.M44 * right.M44;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Transform(Matrix4d matrix, Vector4d vector) { unchecked {
			return new Vector4d(
				(matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z) + (matrix.M14 * vector.W),
				(matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z) + (matrix.M24 * vector.W),
				(matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z) + (matrix.M34 * vector.W),
				(matrix.M41 * vector.X) + (matrix.M42 * vector.Y) + (matrix.M43 * vector.Z) + (matrix.M44 * vector.W));
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(Matrix4d matrix, Vector4d vector, ref Vector4d result) { unchecked {
			result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z) + (matrix.M14 * vector.W);
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z) + (matrix.M24 * vector.W);
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z) + (matrix.M34 * vector.W);
			result.W = (matrix.M41 * vector.X) + (matrix.M42 * vector.Y) + (matrix.M43 * vector.Z) + (matrix.M44 * vector.W);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dPoint(Matrix4d matrix, Vector4d vector, ref Vector4d result) { unchecked {
            Debug.Assert(vector.W == 1d, "vector.W != 1");
			result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z) + matrix.M14;
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z) + matrix.M24;
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z) + matrix.M34;
			result.W = (matrix.M41 * vector.X) + (matrix.M42 * vector.Y) + (matrix.M43 * vector.Z) + matrix.M44;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dVect(Matrix4d matrix, Vector4d vector, ref Vector4d result) { unchecked {
            Debug.Assert(vector.W == 0d, "vector.W != 0");
			result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z);
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z);
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z);
			result.W = (matrix.M41 * vector.X) + (matrix.M42 * vector.Y) + (matrix.M43 * vector.Z);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dPoint(Matrix4d matrix, Vector3d vector, ref Vector3d result) { unchecked {
            result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z) + matrix.M14;
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z) + matrix.M24;
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z) + matrix.M34;
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dVect(Matrix4d matrix, Vector3d vector, ref Vector3d result) { unchecked { ;
			result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + (matrix.M13 * vector.Z);
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + (matrix.M23 * vector.Z);
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + (matrix.M33 * vector.Z);
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dPoint(Matrix4d matrix, Vector2d vector, ref Vector3d result) { unchecked {
            result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + matrix.M14;
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + matrix.M24;
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y) + matrix.M34;
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dVect(Matrix4d matrix, Vector2d vector, ref Vector3d result) { unchecked { ;
			result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y);
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y);
			result.Z = (matrix.M31 * vector.X) + (matrix.M32 * vector.Y);
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dPoint(Matrix4d matrix, Vector2d vector, ref Vector2d result) { unchecked {
            result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y) + matrix.M14;
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y) + matrix.M24;
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform3dVect(Matrix4d matrix, Vector2d vector, ref Vector2d result) { unchecked { ;
			result.X = (matrix.M11 * vector.X) + (matrix.M12 * vector.Y);
			result.Y = (matrix.M21 * vector.X) + (matrix.M22 * vector.Y);
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4d Invert(Matrix4d matrix) { unchecked {
            var a3434 = matrix.M33 * matrix.M44 - matrix.M34 * matrix.M43 ;
            var a2434 = matrix.M32 * matrix.M44 - matrix.M34 * matrix.M42 ;
            var a2334 = matrix.M32 * matrix.M43 - matrix.M33 * matrix.M42 ;
            var a1434 = matrix.M31 * matrix.M44 - matrix.M34 * matrix.M41 ;
            var a1334 = matrix.M31 * matrix.M43 - matrix.M33 * matrix.M41 ;
            var a1234 = matrix.M31 * matrix.M42 - matrix.M32 * matrix.M41 ;
            var a3424 = matrix.M23 * matrix.M44 - matrix.M24 * matrix.M43 ;
            var a2424 = matrix.M22 * matrix.M44 - matrix.M24 * matrix.M42 ;
            var a2324 = matrix.M22 * matrix.M43 - matrix.M23 * matrix.M42 ;
            var a3423 = matrix.M23 * matrix.M34 - matrix.M24 * matrix.M33 ;
            var a2423 = matrix.M22 * matrix.M34 - matrix.M24 * matrix.M32 ;
            var a2323 = matrix.M22 * matrix.M33 - matrix.M23 * matrix.M32 ;
            var a1424 = matrix.M21 * matrix.M44 - matrix.M24 * matrix.M41 ;
            var a1324 = matrix.M21 * matrix.M43 - matrix.M23 * matrix.M41 ;
            var a1423 = matrix.M21 * matrix.M34 - matrix.M24 * matrix.M31 ;
            var a1323 = matrix.M21 * matrix.M33 - matrix.M23 * matrix.M31 ;
            var a1224 = matrix.M21 * matrix.M42 - matrix.M22 * matrix.M41 ;
            var a1223 = matrix.M21 * matrix.M32 - matrix.M22 * matrix.M31 ;
            var det = matrix.M11 * ( matrix.M22 * a3434 - matrix.M23 * a2434 + matrix.M24 * a2334 ) 
                    - matrix.M12 * ( matrix.M21 * a3434 - matrix.M23 * a1434 + matrix.M24 * a1334 ) 
                    + matrix.M13 * ( matrix.M21 * a2434 - matrix.M22 * a1434 + matrix.M24 * a1234 ) 
                    - matrix.M14 * ( matrix.M21 * a2334 - matrix.M22 * a1334 + matrix.M23 * a1234 ) ;
            det = 1 / det;
            return new Matrix4d() {
               M11 = det *   ( matrix.M22 * a3434 - matrix.M23 * a2434 + matrix.M24 * a2334 ),
               M12 = det * - ( matrix.M12 * a3434 - matrix.M13 * a2434 + matrix.M14 * a2334 ),
               M13 = det *   ( matrix.M12 * a3424 - matrix.M13 * a2424 + matrix.M14 * a2324 ),
               M14 = det * - ( matrix.M12 * a3423 - matrix.M13 * a2423 + matrix.M14 * a2323 ),
               M21 = det * - ( matrix.M21 * a3434 - matrix.M23 * a1434 + matrix.M24 * a1334 ),
               M22 = det *   ( matrix.M11 * a3434 - matrix.M13 * a1434 + matrix.M14 * a1334 ),
               M23 = det * - ( matrix.M11 * a3424 - matrix.M13 * a1424 + matrix.M14 * a1324 ),
               M24 = det *   ( matrix.M11 * a3423 - matrix.M13 * a1423 + matrix.M14 * a1323 ),
               M31 = det *   ( matrix.M21 * a2434 - matrix.M22 * a1434 + matrix.M24 * a1234 ),
               M32 = det * - ( matrix.M11 * a2434 - matrix.M12 * a1434 + matrix.M14 * a1234 ),
               M33 = det *   ( matrix.M11 * a2424 - matrix.M12 * a1424 + matrix.M14 * a1224 ),
               M34 = det * - ( matrix.M11 * a2423 - matrix.M12 * a1423 + matrix.M14 * a1223 ),
               M41 = det * - ( matrix.M21 * a2334 - matrix.M22 * a1334 + matrix.M23 * a1234 ),
               M42 = det *   ( matrix.M11 * a2334 - matrix.M12 * a1334 + matrix.M13 * a1234 ),
               M43 = det * - ( matrix.M11 * a2324 - matrix.M12 * a1324 + matrix.M13 * a1224 ),
               M44 = det *   ( matrix.M11 * a2323 - matrix.M12 * a1323 + matrix.M13 * a1223 ),
            };
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4d Transpose(Matrix4d m) { unchecked {
            Matrix4d t = new Matrix4d(m);
			t.Transpose();
			return t;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproxEqual(Matrix4d left, Matrix4d right, double tolerance) { unchecked {
			return ((System.Math.Abs(left.M11 - right.M11) <= tolerance) &&
                    (System.Math.Abs(left.M12 - right.M12) <= tolerance) &&
				    (System.Math.Abs(left.M13 - right.M13) <= tolerance) &&
                    (System.Math.Abs(left.M14 - right.M14) <= tolerance) &&

                    (System.Math.Abs(left.M21 - right.M21) <= tolerance) &&
                    (System.Math.Abs(left.M22 - right.M22) <= tolerance) &&
				    (System.Math.Abs(left.M23 - right.M23) <= tolerance) &&
                    (System.Math.Abs(left.M24 - right.M24) <= tolerance) &&

                    (System.Math.Abs(left.M31 - right.M31) <= tolerance) &&
                    (System.Math.Abs(left.M32 - right.M32) <= tolerance) &&
				    (System.Math.Abs(left.M33 - right.M33) <= tolerance) &&
                    (System.Math.Abs(left.M34 - right.M34) <= tolerance) &&

                    (System.Math.Abs(left.M41 - right.M41) <= tolerance) &&
                    (System.Math.Abs(left.M42 - right.M42) <= tolerance) &&
				    (System.Math.Abs(left.M43 - right.M43) <= tolerance) &&
                    (System.Math.Abs(left.M44 - right.M44) <= tolerance) );
		}}
        #endregion

        #region Свойства и Методы

        public double Trace {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { unchecked { return M11 + M22 + M33 + M44; } }
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double GetDeterminant() { unchecked {
        //	double det = 0.0f;
        //	for (int col = 0; col < 4; col++) 
        //		if ((col % 2) == 0) det += this[0, col] * Minor(0, col).Determinant();
        //		else                det -= this[0, col] * Minor(0, col).Determinant();
        //	return det;
			return
				M14 * M23 * M32 * M41 - M13 * M24 * M32 * M41 - M14 * M22 * M33 * M41 + M12 * M24 * M33 * M41 +
				M13 * M22 * M34 * M41 - M12 * M23 * M34 * M41 - M14 * M23 * M31 * M42 + M13 * M24 * M31 * M42 +
				M14 * M21 * M33 * M42 - M11 * M24 * M33 * M42 - M13 * M21 * M34 * M42 + M11 * M23 * M34 * M42 +
				M14 * M22 * M31 * M43 - M12 * M24 * M31 * M43 - M14 * M21 * M32 * M43 + M11 * M24 * M32 * M43 +
				M12 * M21 * M34 * M43 - M11 * M22 * M34 * M43 - M13 * M22 * M31 * M44 + M12 * M23 * M31 * M44 +
				M13 * M21 * M32 * M44 - M11 * M23 * M32 * M44 - M12 * M21 * M33 * M44 + M11 * M22 * M33 * M44;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix4d Invert() { unchecked {
            return Invert(this);
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Transpose() { unchecked {
		    double temp;
		    temp = M12;  M12 = M21;  M21 = temp;
            temp = M13;  M13 = M31;  M31 = temp;
            temp = M14;  M14 = M41;  M41 = temp;
            temp = M23;  M23 = M32;  M32 = temp;
            temp = M24;  M24 = M42;  M42 = temp;
            temp = M34;  M34 = M43;  M43 = temp;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double[] ToArray(bool glorder = false) { unchecked {
            return glorder 
            ? new double[16] { M11, M21, M31, M41, M12, M22, M32, M42, M13, M23, M33, M43, M14, M24, M34, M44 } 
            : new double[16] { M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44 };
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float[] ToFloatArray(bool glorder = false) { unchecked {
            return ToArray(glorder).Select(v => (float)v).ToArray();
	    }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal[] ToDecimalArray(bool glorder = false) { unchecked {
            return ToArray(glorder).Select(v => (decimal)v).ToArray();
	    }}
        #endregion

        #region Реализация интерфейса ICloneable

        object ICloneable.Clone() {
            return new Matrix4d(this);
        }

        public Matrix4d Clone() {
            return new Matrix4d(this);
        }

        #endregion

        #region Реализация интерфейса IEquatable<Vector4>

        bool IEquatable<Matrix4d>.Equals(Matrix4d m) {
            return  (M11 == m.M11) && (M12 == m.M12) && (M13 == m.M13) && (M14 == m.M14) &&
				    (M21 == m.M21) && (M22 == m.M22) && (M23 == m.M23) && (M24 == m.M24) &&
				    (M31 == m.M31) && (M32 == m.M32) && (M33 == m.M33) && (M34 == m.M34) &&
				    (M41 == m.M41) && (M42 == m.M42) && (M43 == m.M43) && (M44 == m.M44);
        }

        public bool Equals(Matrix4d m) {
            return  (M11 == m.M11) && (M12 == m.M12) && (M13 == m.M13) && (M14 == m.M14) &&
				    (M21 == m.M21) && (M22 == m.M22) && (M23 == m.M23) && (M24 == m.M24) &&
				    (M31 == m.M31) && (M32 == m.M32) && (M33 == m.M33) && (M34 == m.M34) &&
				    (M41 == m.M41) && (M42 == m.M42) && (M43 == m.M43) && (M44 == m.M44);
        }
        #endregion

        #region Реализация интерфейса IFormattable

        public string ToString(string format) {
            return string.Format("4x4[{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}]",
                M11.ToString(format, null), M12.ToString(format, null), M13.ToString(format, null), M14.ToString(format, null),
                M21.ToString(format, null), M22.ToString(format, null), M23.ToString(format, null), M24.ToString(format, null),
                M31.ToString(format, null), M32.ToString(format, null), M33.ToString(format, null), M34.ToString(format, null),
                M41.ToString(format, null), M42.ToString(format, null), M43.ToString(format, null), M44.ToString(format, null));
        }

        string IFormattable.ToString(string format, IFormatProvider provider) {
            return ToString(format, provider);
        }

        public string ToString(string format, IFormatProvider provider) {
            return string.Format("4x4[{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}]",
                M11.ToString(format, provider), M12.ToString(format, provider), M13.ToString(format, provider), M14.ToString(format, provider),
                M21.ToString(format, provider), M22.ToString(format, provider), M23.ToString(format, provider), M24.ToString(format, provider),
                M31.ToString(format, provider), M32.ToString(format, provider), M33.ToString(format, provider), M34.ToString(format, provider),
                M41.ToString(format, provider), M42.ToString(format, provider), M43.ToString(format, provider), M44.ToString(format, provider));
        }
        #endregion

        #region Перегрузки

        public override int GetHashCode() {
			return  M11.GetHashCode() ^ M12.GetHashCode() ^ M13.GetHashCode() ^ M14.GetHashCode() ^
				    M21.GetHashCode() ^ M22.GetHashCode() ^ M23.GetHashCode() ^ M24.GetHashCode() ^
				    M31.GetHashCode() ^ M32.GetHashCode() ^ M33.GetHashCode() ^ M34.GetHashCode() ^
				    M41.GetHashCode() ^ M42.GetHashCode() ^ M43.GetHashCode() ^ M44.GetHashCode();
		}

		public override bool Equals(object obj) {
			if (obj is Matrix4d) {
                Matrix4d m = (Matrix4d)obj;
				return  (M11 == m.M11) && (M12 == m.M12) && (M13 == m.M13) && (M14 == m.M14) &&
					    (M21 == m.M21) && (M22 == m.M22) && (M23 == m.M23) && (M24 == m.M24) &&
					    (M31 == m.M31) && (M32 == m.M32) && (M33 == m.M33) && (M34 == m.M34) &&
					    (M41 == m.M41) && (M42 == m.M42) && (M43 == m.M43) && (M44 == m.M44);
			}
			return false;
		}

		public override string ToString() {
			return string.Format("4x4[{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}]",
				M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44);
		}
		#endregion

        #region Операторы

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Matrix4d left, Matrix4d right) { unchecked {
			return ValueType.Equals(left, right);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Matrix4d left, Matrix4d right) { unchecked {
			return !ValueType.Equals(left, right);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4d operator +(Matrix4d left, Matrix4d right) { unchecked {
            return Matrix4d.Add(left, right); ;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4d operator +(Matrix4d matrix, double scalar) { unchecked {
            return Matrix4d.Add(matrix, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4d operator +(double scalar, Matrix4d matrix) { unchecked {
            return Matrix4d.Add(matrix, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4d operator -(Matrix4d left, Matrix4d right) { unchecked {
            return Matrix4d.Subtract(left, right); ;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4d operator -(Matrix4d matrix, double scalar) { unchecked {
            return Matrix4d.Subtract(matrix, scalar);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4d operator *(Matrix4d left, Matrix4d right) { unchecked {
            return Matrix4d.Multiply(left, right); ;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4d operator ^(Matrix4d left, Matrix4d right) { unchecked {
            return Matrix4d.Multiply(right, left); ;
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator *(Matrix4d matrix, Vector4d vector) { unchecked {
            return Matrix4d.Transform(matrix, vector);
		}}

		public unsafe double this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { unchecked {
				if (index < 0 || index >= 16)
                    throw new IndexOutOfRangeException("Недопустимый индекс элемента матрицы!");
				fixed (double* f = &M11)
					return *(f + index);
			}}
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
			set { unchecked {
				if (index < 0 || index >= 16)
                    throw new IndexOutOfRangeException("Недопустимый индекс элемента матрицы!");
				fixed (double* f = &M11)
					*(f + index) = value;
			}}
		}

		public double this[int row, int column] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { unchecked { return this[(row - 1) * 4 + (column - 1)]; } }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { unchecked { this[(row - 1) * 4 + (column - 1)] = value; } }
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Matrix3d(Matrix4d matrix) { unchecked {
            return new Matrix3d(matrix.M11, matrix.M12, matrix.M13,
                                matrix.M21, matrix.M22, matrix.M23,
                                matrix.M31, matrix.M32, matrix.M33);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Matrix3f(Matrix4d matrix) { unchecked {
            return new Matrix3f((float)matrix.M11, (float)matrix.M12, (float)matrix.M13,
                                (float)matrix.M21, (float)matrix.M22, (float)matrix.M23,
                                (float)matrix.M31, (float)matrix.M32, (float)matrix.M33);
		}}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Matrix4f(Matrix4d matrix) { unchecked {
            return new Matrix4f((float)matrix.M11, (float)matrix.M12, (float)matrix.M13, (float)matrix.M14,
                                (float)matrix.M21, (float)matrix.M22, (float)matrix.M23, (float)matrix.M24,
                                (float)matrix.M31, (float)matrix.M32, (float)matrix.M33, (float)matrix.M34,
                                (float)matrix.M41, (float)matrix.M42, (float)matrix.M43, (float)matrix.M44);
		}}
        #endregion
    }
}