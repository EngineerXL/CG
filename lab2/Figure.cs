using System;
using System.Collections.Generic;

namespace lab2
{
    public class Polygon
    {
        public List<Vector4D> _data;

        public Polygon()
        {
            _data = new List<Vector4D>(3);
        }

        public Polygon(Vector4D a, Vector4D b, Vector4D c) : base()
        {
            _data = new List<Vector4D>(3);
            _data.Add(a);
            _data.Add(b);
            _data.Add(c);
        }

        public Vector4D this[int i]
        {
            get
            {
                return _data[i];
            }
            set
            {
                _data[i] = value;
            }
        }

        public void TransformPolygon(Matrix4D matr)
        {
            for (int i = 0; i < _data.Count; ++i)
            {
                _data [i] = _data[i] * matr;
            }
        }

        public Vector4D NormalVector()
        {
            return Vector4D.Cross(_data[1] - _data[0], _data[2] - _data[0]);
        }

        public bool Visible()
        {
            return NormalVector().Z < 0;
        }
    }

    public class VertexPolygonsPair
    {
        public Vector4D First;
        public List<int> Second;

        public VertexPolygonsPair()
        {
            First = new Vector4D();
            Second = new List<int>();
        }

        public VertexPolygonsPair(Vector4D vertex)
        {
            First = new Vector4D(vertex);
            Second = new List<int>();
        }
    }

    public class OctahedralPrism
    {
        private Vector4D centerLow;
        private Vector4D centerHigh;
        private List<Vector4D> low;
        private List<Vector4D> high;

        public List<VertexPolygonsPair> _vertices;
        public List<Polygon> _polygons;

        private const double D_PHI = 45;
        private const int POLYGON_COUNT = 32;
        private const int BASE_VERTEX_COUNT = 8;

        public OctahedralPrism(double r, double h)
        {
            low = new List<Vector4D>(BASE_VERTEX_COUNT);
            high = new List<Vector4D>(BASE_VERTEX_COUNT);
            _polygons = new List<Polygon>(POLYGON_COUNT);
            GenVertices(r, h);
            GenFigure();
            GenVertexPolygons();
        }

        private void GenVertices(double r, double h)
        {
            _vertices = new List<VertexPolygonsPair>(2 * BASE_VERTEX_COUNT + 2);
            centerLow = new Vector4D(0, 0, 0, 1);
            centerHigh = new Vector4D(0, 0, h, 1);
            _vertices.Add(new VertexPolygonsPair(centerLow));
            _vertices.Add(new VertexPolygonsPair(centerHigh));
            double phi = 0;
            for (int i = 0; i < BASE_VERTEX_COUNT; ++i)
            {
                Vector4D vertex = new Vector4D(r * Math.Cos(Misc.ToRadians(phi)), r * Math.Sin(Misc.ToRadians(phi)), 0, 0);
                low.Add(vertex + centerLow);
                high.Add(vertex + centerHigh);
                phi = phi + D_PHI;
            }
            for (int i = 0; i < BASE_VERTEX_COUNT; ++i)
            {
                _vertices.Add(new VertexPolygonsPair(low[i]));
                _vertices.Add(new VertexPolygonsPair(high[i]));
            }
        }

        private void GenFigure()
        {
            for (int i = 0; i < BASE_VERTEX_COUNT; ++i)
            {
                Vector4D a = low[i];
                Vector4D b = high[i];
                Vector4D c = high[(i + 1) % BASE_VERTEX_COUNT];
                Vector4D d = low[(i + 1) % BASE_VERTEX_COUNT];
                _polygons.Add(new Polygon(d, c, a));
                _polygons.Add(new Polygon(b, a, c));
                _polygons.Add(new Polygon(centerLow, d, a));
                _polygons.Add(new Polygon(centerHigh, b, c));
            }
        }

        private void GenVertexPolygons()
        {
            for (int i = 0; i < _polygons.Count; ++i)
            {
                foreach (Vector4D vertex in _polygons[i]._data)
                {
                    for (int j = 0; j < _vertices.Count; ++j)
                    {
                        if (_vertices[j].First == vertex)
                        {
                            _vertices[j].Second.Add(i);
                        }
                    }
                }
            }
        }

        public void Transform(Matrix4D matr)
        {
            for (int i = 0; i < _vertices.Count; ++i) {
                _vertices[i].First = _vertices[i].First * matr;
            }
            foreach (Polygon poly in _polygons)
            {
                poly.TransformPolygon(matr);
            }
        }
    }
}
