using System;
using SharpGL;
using System.Collections.Generic;

namespace cw
{
    public class Vertex
    {
        public Vector4D Data;
        public Vector4D Normal;
        public Misc.Colour Col;
        public int Id;

        public Vertex()
        {
            Data = new Vector4D();
            Normal = new Vector4D();
            Col = new Misc.Colour();
            Id = 0;
        }

        public Vertex(double x, double y, double z, Misc.Colour col)
        {
            Data = new Vector4D(x, y, z, 0);
            Normal = new Vector4D();
            Col = new Misc.Colour(col);
            Id = 0;
        }

        public Vertex(double x, double y, double z, double w, Misc.Colour col)
        {
            Data = new Vector4D(x, y, z, w);
            Normal = new Vector4D();
            Col = new Misc.Colour(col);
            Id = 0;
        }
    }

    public class Polygon
    {
        public List<Vertex> Data;
        public int Count;

        public Polygon()
        {
            Count = 0;
            Data = new List<Vertex>();
        }

        public Polygon(Vertex a, Vertex b, Vertex c) : this()
        {
            Count = 3;
            Data.Add(a);
            Data.Add(b);
            Data.Add(c);
        }

        public Vertex this[int i]
        {
            get
            {
                return Data[i];
            }
            set
            {
                Data[i] = value;
            }
        }

        public Vector4D GetCenter()
        {
            Vector4D res = new Vector4D();
            foreach (Vertex vert in Data)
            {
                res = res + vert.Data;
            }
            return res / Count;
        }

        public Vector4D NormalVector()
        {
            Vector4D res = Vector4D.Cross(Data[1].Data - Data[0].Data, Data[2].Data - Data[0].Data);
            res.Normalize();
            return res;
        }

        public bool Visible()
        {
            return NormalVector().Z < Misc.EPS;
        }
    }

    public class Figure
    {
        private Vertex CenterLow;
        private Vertex CenterHigh;
        private List< List<Vertex> > LayerVertices;

        public List<Vector4D> ControlPoints;
        public List<double> Weights;

        public List<Vertex> Vertices;
        public List<Polygon> Polygons;

        private int ParamPhi;
        private int ParamCurve;
        private double DeltaPhi;

        public Misc.Colour FigureColour;

        public Figure(int paramPhi, int paramCurve, List<Vector4D> controlPoints, List<double> weights, Misc.Colour col)
        {
            ParamPhi = paramPhi;
            ParamCurve = paramCurve - 1;
            DeltaPhi = Misc.ToRadians(Misc.MAX_DEG / (double)(ParamPhi));
            ControlPoints = controlPoints;
            Weights = weights;
            FigureColour = col;
            GenVertices();
            GenFigure();
            GenVertexNormals();
        }

        private void GenVertices()
        {
            Vertices = new List<Vertex>();
            Curve nurbs = new Curve(ParamCurve, ControlPoints, Weights);
            LayerVertices = new List< List<Vertex> >();
            for (int i = 0; i < nurbs.CountData; ++i)
            {
                double curZ = nurbs.Data[i].Y;
                double curXY = (1 + nurbs.Data[i].X) / 2;
                double stepPhi = 0;
                List<Vertex> curLayerVertices = new List<Vertex>();
                for (int j = 0; j < ParamPhi; ++j)
                {
                    Vertex curVertex = new Vertex(curXY * Math.Cos(stepPhi), curXY * Math.Sin(stepPhi), curZ, FigureColour);
                    curLayerVertices.Add(curVertex);
                    Vertices.Add(curVertex);
                    stepPhi = stepPhi + DeltaPhi;
                }
                LayerVertices.Add(curLayerVertices);
            }
            CenterLow = new Vertex(0, 0, Vertices[Vertices.Count - 1].Data.Z, FigureColour);
            CenterHigh = new Vertex(0, 0, Vertices[0].Data.Z, FigureColour);
            Vertices.Add(CenterLow);
            Vertices.Add(CenterHigh);
            for (int i = 0; i < Vertices.Count; ++i)
            {
                Vertices[i].Id = i;
            }
        }

        private void GenFigure()
        {
            Polygons = new List<Polygon>();
            GenCenter();
        }

        private void GenCenter()
        {
            for (int i = 0; i < LayerVertices.Count - 1; ++i)
            {
                for (int j = 0; j < ParamPhi; ++j)
                {
                    Vertex a = LayerVertices[i][j];
                    Vertex b = LayerVertices[i][(j + 1) % ParamPhi];
                    Vertex c = LayerVertices[i + 1][(j + 1) % ParamPhi];
                    Vertex d = LayerVertices[i + 1][j];
                    Polygons.Add(new Polygon(a, c, b));
                    Polygons.Add(new Polygon(c, a, d));
                }
            }
        }

        public void GenVertexNormals()
        {
            foreach (Vertex vert in Vertices)
            {
                vert.Normal = new Vector4D();
                vert.Normal.W = 0;
            }
            foreach (Polygon poly in Polygons)
            {
                Vector4D polyN = poly.NormalVector();
                polyN.Normalize();
                foreach (Vertex vert in poly.Data)
                {
                    vert.Normal = vert.Normal + polyN;
                    vert.Normal.W += 1;
                }
            }
            foreach (Vertex vert in Vertices)
            {
                vert.Normal.X = vert.Normal.X / vert.Normal.W;
                vert.Normal.Y = vert.Normal.Y / vert.Normal.W;
                vert.Normal.Z = vert.Normal.Z / vert.Normal.W;
                vert.Normal.W = 0;
                vert.Normal.Normalize();
            }
        }

        unsafe public void WriteVertexData(float* ptr)
        {
            int i = 0;
            foreach (Vertex vert in Vertices)
            {
                ptr[i + 0] = (float)vert.Data.X;
                ptr[i + 1] = (float)vert.Data.Y;
                ptr[i + 2] = (float)vert.Data.Z;
                ptr[i + 3] = (float)vert.Col.r;
                ptr[i + 4] = (float)vert.Col.g;
                ptr[i + 5] = (float)vert.Col.b;
                ptr[i + 6] = (float)vert.Normal.X;
                ptr[i + 7] = (float)vert.Normal.Y;
                ptr[i + 8] = (float)vert.Normal.Z;
                i = i + 9;
            }
        }

        unsafe public void WriteIndeces(int* ptr)
        {
            int i = 0;
            foreach (Polygon poly in Polygons)
            {
                ptr[i + 0] = poly[0].Id;
                ptr[i + 1] = poly[1].Id;
                ptr[i + 2] = poly[2].Id;
                i = i + 3;
            }
        }
    }
}
