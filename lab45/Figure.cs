using System;
using SharpGL;
using System.Collections.Generic;

namespace lab4
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

        public List<Vertex> Vertices;
        public List<Polygon> Polygons;

        private int ParamPhi;
        private int ParamTheta;
        private double R;
        private double MaxTheta;
        private double DeltaPhi;
        private double DeltaTheta;

        public Misc.Colour FigureColour;

        public Figure(int paramPhi, int paramTheta, double paramR, double maxTheta, Misc.Colour col)
        {
            ParamPhi = paramPhi;
            ParamTheta = paramTheta - 1;
            R = paramR;
            MaxTheta = Misc.ToRadians(maxTheta);
            DeltaPhi = Misc.ToRadians(Misc.MAX_DEG / (double)(ParamPhi));
            DeltaTheta = MaxTheta / (double)(ParamTheta);
            FigureColour = col;
            GenVertices();
            GenFigure();
            GenVertexNormals();
        }

        private void GenVertices()
        {
            CenterLow = new Vertex(0, 0, R * Math.Cos(MaxTheta), FigureColour);
            CenterHigh = new Vertex(0, 0, R, FigureColour);
            Vertices = new List<Vertex>();
            Vertices.Add(CenterLow);
            Vertices.Add(CenterHigh);
            LayerVertices = new List< List<Vertex> >();
            double stepTheta = DeltaTheta;
            for (int i = 0; i < ParamTheta; ++i)
            {
                double curZ = R * Math.Cos(stepTheta);
                double curXY = R * Math.Sin(stepTheta);
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
                stepTheta = stepTheta + DeltaTheta;
            }
            for (int i = 0; i < Vertices.Count; ++i)
            {
                Vertices[i].Id = i;
            }
        }

        private void GenFigure()
        {
            Polygons = new List<Polygon>();
            GenLow();
            GenHigh();
            GenSphere();
        }

        private void GenLow()
        {
            int lastLayerInd = LayerVertices.Count - 1;
            for (int j = 0; j < ParamPhi; ++j)
            {
                Vertex a = LayerVertices[lastLayerInd][j];
                Vertex b = LayerVertices[lastLayerInd][(j + 1) % ParamPhi];
                Polygons.Add(new Polygon(CenterLow, b, a));
            }
        }

        private void GenSphere()
        {
            for (int i = 0; i < ParamTheta - 1; ++i)
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

        private void GenHigh()
        {
            for (int j = 0; j < ParamPhi; ++j)
            {
                Vertex a = LayerVertices[0][j];
                Vertex b = LayerVertices[0][(j + 1) % ParamPhi];
                Polygons.Add(new Polygon(CenterHigh, a, b));
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
