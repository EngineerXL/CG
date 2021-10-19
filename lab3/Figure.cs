using System;
using System.Collections.Generic;

namespace lab3
{
    public class Polygon
    {
        public List<Vector4D> _data;
        public List<Vector4D> _normals;
        public int Count;

        public Polygon()
        {
            Count = 3;
            _data = new List<Vector4D>(Count);
            _normals = new List<Vector4D>(Count);
        }

        public Polygon(Vector4D a, Vector4D b, Vector4D c) : this()
        {
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

        public Vector4D GetCenter()
        {
            return (_data[0] + _data[1] + _data[2]) / Count;
        }

        public Vector4D NormalVector()
        {
            return Vector4D.Cross(_data[1] - _data[0], _data[2] - _data[0]);
        }

        public bool Visible()
        {
            return NormalVector().Z < Misc.EPS;
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
            First = vertex;
            Second = new List<int>();
        }
    }

    public class Figure
    {
        private Vector4D centerLow;
        private Vector4D centerHigh;

        private List< List<Vector4D> > _dataVert;

        public List<VertexPolygonsPair> globalVertices;
        public List<VertexPolygonsPair> _vertices;
        public List<Vector4D> _normals;
        public List<Polygon> _polygons;

        private int _nPhi;
        private int _nTheta;
        private double _dPhi;
        private double _dTheta;
        private double _ro;
        private double _theta;

        public Figure(int nPhi, int nTheta, double ro, double theta)
        {
            _nPhi = nPhi;
            _nTheta = nTheta;
            _ro = ro;
            _theta = Misc.ToRadians(theta);
            _dPhi = Misc.ToRadians(Misc.MAX_DEG / (double)_nPhi);
            _dTheta = _theta / (double)(_nTheta);
            GenVertices();
            GenFigure();
            GenVertexPolygons();
        }

        private void GenVertices()
        {
            centerHigh = new Vector4D(0, 0, _ro, 1);
            centerLow = new Vector4D(0, 0, _ro * Math.Cos(_theta), 1);
            _vertices = new List<VertexPolygonsPair>();
            _vertices.Add(new VertexPolygonsPair(centerLow));
            _vertices.Add(new VertexPolygonsPair(centerHigh));
            _dataVert = new List< List<Vector4D> >();
            double theta = _dTheta;
            for (int i = 0; i < _nTheta; ++i)
            {
                double curZ = _ro * Math.Cos(theta);
                double curXY = _ro * Math.Sin(theta);
                double phi = 0;
                List<Vector4D> curLevelVertices = new List<Vector4D>();
                for (int j = 0; j < _nPhi; ++j)
                {
                    Vector4D curVertex = new Vector4D(curXY * Math.Cos(phi), curXY * Math.Sin(phi), curZ, 1);
                    curLevelVertices.Add(curVertex);
                    _vertices.Add(new VertexPolygonsPair(curVertex));
                    phi = phi + _dPhi;
                }
                _dataVert.Add(curLevelVertices);
                theta = theta + _dTheta;
            }
            globalVertices = new List<VertexPolygonsPair>();
            foreach (VertexPolygonsPair vertex in _vertices)
            {
                Vector4D curVertex = new Vector4D(vertex.First);
                globalVertices.Add(new VertexPolygonsPair(curVertex));
            }
        }

        private void GenFigure()
        {
            _polygons = new List<Polygon>();
            GenLow();
            GenHigh();
            GenSphere();
        }

        private void GenLow()
        {
            for (int j = 0; j < _nPhi; ++j)
            {
                Vector4D a = _dataVert[_dataVert.Count - 1][j];
                Vector4D b = _dataVert[_dataVert.Count - 1][(j + 1) % _nPhi];
                _polygons.Add(new Polygon(centerLow, b, a));
            }
        }

        private void GenSphere()
        {
            for (int i = 0; i < _nTheta - 1; ++i)
            {
                for (int j = 0; j < _nPhi; ++j)
                {
                    Vector4D a = _dataVert[i][j];
                    Vector4D b = _dataVert[i][(j + 1) % _nPhi];
                    Vector4D c = _dataVert[i + 1][(j + 1) % _nPhi];
                    Vector4D d = _dataVert[i + 1][j];
                    _polygons.Add(new Polygon(a, c, b));
                    _polygons.Add(new Polygon(c, a, d));
                }
            }
        }

        private void GenHigh()
        {
            for (int j = 0; j < _nPhi; ++j)
            {
                Vector4D a = _dataVert[0][j];
                Vector4D b = _dataVert[0][(j + 1) % _nPhi];
                _polygons.Add(new Polygon(centerHigh, a, b));
            }
        }

        public void GenVertN()
        {
            List<Vector4D> polygonNormals = new List<Vector4D>();
            foreach (Polygon poly in _polygons)
            {
                polygonNormals.Add(poly.NormalVector());
                for (int i = 0; i < poly.Count; ++i)
                {
                    poly._normals.Add(new Vector4D());
                }
            }
            _normals = new List<Vector4D>();
            foreach (VertexPolygonsPair item in _vertices)
            {
                Vector4D vec = new Vector4D();
                foreach (int polyId in item.Second)
                {
                    vec = vec + _polygons[polyId].NormalVector();
                }
                vec = vec / item.Second.Count;
                _normals.Add(vec);
                foreach (int polyId in item.Second)
                {
                    Polygon poly = _polygons[polyId];
                    for (int i = 0; i < poly.Count; ++i)
                    {
                        if (item.First == poly[i])
                        {
                            poly._normals[i] = vec;
                        }
                    }
                }
            }

            // Console.WriteLine("GenVertN");
            // for (int i = 0; i < _vertices.Count; ++i)
            // {
            //     VertexPolygonsPair curPair = _vertices[i];
            //     curPair.First.N.Assign(new Vector4D());
            // }
            // foreach (Polygon poly in _polygons)
            // {
            //     Vector4D N = poly.NormalVector();
            //     Console.WriteLine("normal");
            //     Console.WriteLine(N.X.ToString() + ", " + N.Y.ToString() + ", " + N.Z.ToString());
            //     for (int i = 0; i < poly._data.Count; ++i)
            //     {
            //         Console.WriteLine("summ");
            //         Console.WriteLine(poly._data[i].N.X.ToString() + ", " + poly._data[i].N.Y.ToString() + ", " + poly._data[i].N.Z.ToString());
            //         poly._data[i].N = poly._data[i].N + N;
            //         Console.WriteLine("summed");
            //         Console.WriteLine(poly._data[i].N.X.ToString() + ", " + poly._data[i].N.Y.ToString() + ", " + poly._data[i].N.Z.ToString());
            //     }
            // }
            // for (int i = 0; i < _vertices.Count; ++i)
            // {
            //     VertexPolygonsPair curPair = _vertices[i];
            //     Console.WriteLine("Vertex:");
            //     Console.WriteLine(curPair.First.vertex.X.ToString() + ", " + curPair.First.vertex.Y.ToString() + ", " + curPair.First.vertex.Z.ToString());
            //     Console.WriteLine(curPair.First.N.X.ToString() + ", " + curPair.First.N.Y.ToString() + ", " + curPair.First.N.Z.ToString());
            //     curPair.First.N = curPair.First.N / (double) curPair.Second.Count;
            //     curPair.First.N.Normalize();
            // }
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
                _vertices[i].First.Assign(globalVertices[i].First * matr);
            }
        }
    }
}
