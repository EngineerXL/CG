using Cairo;
using Gdk;
using Gtk;
using System;
using System.Collections.Generic;
using UI = Gtk.Builder.ObjectAttribute;

namespace lab2
{
    class MainWindow : Gtk.Window
    {
        [UI] private DrawingArea _drawingArea = null;
        [UI] private Adjustment _adjustmentAlpha = null;
        [UI] private Adjustment _adjustmentBeta = null;
        [UI] private Adjustment _adjustmentGamma = null;
        [UI] private Adjustment _adjustmentScaleX = null;
        [UI] private Adjustment _adjustmentScaleY = null;
        [UI] private Adjustment _adjustmentScaleZ = null;
        [UI] private Adjustment _adjustmentShiftX = null;
        [UI] private Adjustment _adjustmentShiftY = null;
        [UI] private Adjustment _adjustmentShiftZ = null;
        [UI] private CheckButton _checkButtonDrawNormalVectors = null;
        [UI] private CheckButton _checkButtonFillPolygons = null;
        [UI] private CheckButton _checkButtonRandomizeColours = null;
        [UI] private CheckButton _checkButtonIgnoreInvisible = null;

        [UI] private Adjustment _adjustmentMatrix00 = null;
        [UI] private Adjustment _adjustmentMatrix01 = null;
        [UI] private Adjustment _adjustmentMatrix02 = null;
        [UI] private Adjustment _adjustmentMatrix03 = null;
        [UI] private Adjustment _adjustmentMatrix10 = null;
        [UI] private Adjustment _adjustmentMatrix11 = null;
        [UI] private Adjustment _adjustmentMatrix12 = null;
        [UI] private Adjustment _adjustmentMatrix13 = null;
        [UI] private Adjustment _adjustmentMatrix20 = null;
        [UI] private Adjustment _adjustmentMatrix21 = null;
        [UI] private Adjustment _adjustmentMatrix22 = null;
        [UI] private Adjustment _adjustmentMatrix23 = null;
        [UI] private Adjustment _adjustmentMatrix30 = null;
        [UI] private Adjustment _adjustmentMatrix31 = null;
        [UI] private Adjustment _adjustmentMatrix32 = null;
        [UI] private Adjustment _adjustmentMatrix33 = null;

        private const double NORMAL_VECTOR_SIZE = 20;
        private readonly Cairo.Color DEFAULT_LINE_COLOR = new Cairo.Color(0, 0, 1);
        private readonly Cairo.Color DEFAULT_POLYGON_COLOR = new Cairo.Color(0, 1, 0);
        private readonly Cairo.Color DEFAULT_NORMAL_COLOR = new Cairo.Color(1, 0, 0);
        private readonly Cairo.Color DEFAULT_LINE_COLOR_FILL = new Cairo.Color(0, 0, 0);
        private List<Cairo.Color> polygonColors = null;

        private int width = 0;
        private int height = 0;
        private Vector2D windowCenter = null;
        private double alpha = 0;
        private double beta = 0;
        private double gamma = 0;
        private double scaleX = 0;
        private double scaleY = 0;
        private double scaleZ = 0;
        private double shiftX = 0;
        private double shiftY = 0;
        private double shiftZ = 0;
        private Matrix4D matr = null;
        private double r = 100;
        private double h = 100;
        private OctahedralPrism prism = null;
        private bool drawNormalVectors = false;
        private bool fillPolygons = false;
        private bool randomizeColours = false;
        private bool randomizeToggled = true;
        private bool ignoreInvisible = false;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);
            DeleteEvent += Window_DeleteEvent;
            AddValueChangedEventToAdjustments();
            _drawingArea.Drawn += DrawingArea_DrawnEvent;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void AddValueChangedEventToAdjustments()
        {
            _adjustmentAlpha.ValueChanged += AdjustmentAlpha_ValueChangedEvent;
            _adjustmentBeta.ValueChanged += AdjustmentBeta_ValueChangedEvent;
            _adjustmentGamma.ValueChanged += AdjustmentGamma_ValueChangedEvent;
            _adjustmentScaleX.ValueChanged += Redraw;
            _adjustmentScaleY.ValueChanged += Redraw;
            _adjustmentScaleZ.ValueChanged += Redraw;
            _adjustmentShiftX.ValueChanged += Redraw;
            _adjustmentShiftY.ValueChanged += Redraw;
            _adjustmentShiftZ.ValueChanged += Redraw;
            _checkButtonDrawNormalVectors.Toggled += Redraw;
            _checkButtonFillPolygons.Toggled += Redraw;
            _checkButtonRandomizeColours.Toggled += RandomizeColoursToggledEvent;
            _checkButtonIgnoreInvisible.Toggled += Redraw;
        }

        private void GenColors()
        {
            randomizeColours = _checkButtonRandomizeColours.Active;
            if (randomizeToggled)
            {
                Random rnd = new Random();
                polygonColors = new List<Cairo.Color>();
                for (int i = 0; i < prism._polygons.Count; ++i) {
                    if (randomizeColours)
                    {
                        polygonColors.Add(new Cairo.Color(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble()));
                    }
                    else
                    {
                        polygonColors.Add(DEFAULT_POLYGON_COLOR);
                    }
                }
            }
            randomizeToggled = false;
        }

        private void AdjustmentAlpha_ValueChangedEvent(object sender, EventArgs args)
        {
            _adjustmentAlpha.Value = Misc.NormalizeAngle(_adjustmentAlpha.Value);
            _drawingArea.QueueDraw();
        }

        private void AdjustmentBeta_ValueChangedEvent(object sender, EventArgs args)
        {
            _adjustmentBeta.Value = Misc.NormalizeAngle(_adjustmentBeta.Value);
            _drawingArea.QueueDraw();
        }

        private void AdjustmentGamma_ValueChangedEvent(object sender, EventArgs args)
        {
            _adjustmentGamma.Value = Misc.NormalizeAngle(_adjustmentGamma.Value);
            _drawingArea.QueueDraw();
        }

        private void Redraw(object sender, EventArgs args)
        {
            _drawingArea.QueueDraw();
        }

        private void RandomizeColoursToggledEvent(object sender, EventArgs args)
        {
            randomizeToggled = true;
            GenColors();
            _drawingArea.QueueDraw();
        }

        private void DrawingArea_DrawnEvent(object sender, DrawnArgs args)
        {
            GetData();
            GenColors();
            GenMatrix();
            Context cr = args.Cr;
            cr.Antialias = Antialias.Subpixel;
            cr.SetSourceRGB(0, 0, 0);
            cr.Paint();
            cr.LineWidth = 1;
            prism.Transform(matr);
            DrawFigure(cr);
            if (drawNormalVectors)
            {
                DrawNormalVectors(cr, prism);
            }
        }

        private void GetData()
        {
            width = _drawingArea.Window.Width;
            height = _drawingArea.Window.Height;
            windowCenter = new Vector2D(width / 2, height / 2);

            alpha = Misc.ToRadians(_adjustmentAlpha.Value);
            beta = Misc.ToRadians(_adjustmentBeta.Value);
            gamma = Misc.ToRadians(_adjustmentGamma.Value);

            shiftX = _adjustmentShiftX.Value;
            shiftY = _adjustmentShiftY.Value;
            shiftZ = _adjustmentShiftZ.Value;

            scaleX = _adjustmentScaleX.Value;
            scaleY = _adjustmentScaleY.Value;
            scaleZ = _adjustmentScaleZ.Value;

            prism = new OctahedralPrism(r, h);

            drawNormalVectors = _checkButtonDrawNormalVectors.Active;
            fillPolygons = _checkButtonFillPolygons.Active;
            ignoreInvisible = _checkButtonIgnoreInvisible.Active;
        }

        private void GenMatrix()
        {
            matr = new Matrix4D();
            matr = matr * Matrix4D.ScaleX(scaleX) * Matrix4D.ScaleY(scaleY) * Matrix4D.ScaleZ(scaleZ);
            matr = matr * Matrix4D.ShiftX(shiftX) * Matrix4D.ShiftY(shiftY) * Matrix4D.ShiftZ(shiftZ);
            matr = matr * Matrix4D.RotX(alpha) * Matrix4D.RotY(beta) * Matrix4D.RotZ(gamma);
            SetMatrixAdjustments();
        }

        private void SetMatrixAdjustments()
        {
            _adjustmentMatrix00.Value = matr[0, 0];
            _adjustmentMatrix01.Value = matr[0, 1];
            _adjustmentMatrix02.Value = matr[0, 2];
            _adjustmentMatrix03.Value = matr[0, 3];

            _adjustmentMatrix10.Value = matr[1, 0];
            _adjustmentMatrix11.Value = matr[1, 1];
            _adjustmentMatrix12.Value = matr[1, 2];
            _adjustmentMatrix13.Value = matr[1, 3];

            _adjustmentMatrix20.Value = matr[2, 0];
            _adjustmentMatrix21.Value = matr[2, 1];
            _adjustmentMatrix22.Value = matr[2, 2];
            _adjustmentMatrix23.Value = matr[2, 3];

            _adjustmentMatrix30.Value = matr[3, 0];
            _adjustmentMatrix31.Value = matr[3, 1];
            _adjustmentMatrix32.Value = matr[3, 2];
            _adjustmentMatrix33.Value = matr[3, 3];
        }

        private void DrawFigure(Context cr)
        {
            for (int i = 0; i < prism._polygons.Count; ++i)
            {
                if (!ignoreInvisible || prism._polygons[i].Visible()) {
                    if (fillPolygons)
                    {
                        DrawAndFillPolygon(cr, i);
                    }
                    else
                    {
                        DrawPolygon(cr, i);
                    }
                }
            }
        }

        private void DrawNormalVectors(Context cr, OctahedralPrism prism)
        {
            for (int i = 0; i < prism._polygons.Count; ++i)
            {
                if (!ignoreInvisible || prism._polygons[i].Visible()) {
                    cr.SetSourceColor(DEFAULT_NORMAL_COLOR);
                    DrawNormal(cr, prism._polygons[i], windowCenter);
                }
            }
        }

        private void DrawAndFillPolygon(Context cr, int id)
        {
            Polygon poly = prism._polygons[id];
            Vector4D a = poly._data[0];
            cr.NewPath();
            cr.MoveTo(a.X + windowCenter.X, a.Y + windowCenter.Y);
            for (int i = 1; i < poly._data.Count; ++i)
            {
                a = poly._data[i];
                cr.LineTo(a.X + windowCenter.X, a.Y + windowCenter.Y);
            }
            cr.ClosePath();
            cr.Save();
            cr.SetSourceColor(polygonColors[id]);
            cr.FillPreserve();
            cr.Restore();
            cr.SetSourceColor(DEFAULT_LINE_COLOR_FILL);
            cr.Stroke();

        }

        private void DrawPolygon(Context cr, int id)
        {
            cr.SetSourceColor(DEFAULT_LINE_COLOR);
            Polygon poly = prism._polygons[id];
            for (int i = 0; i < poly._data.Count; ++i)
            {
                Vector4D a = poly._data[i];
                Vector4D b = poly._data[(i + 1) % poly._data.Count];
                Vector2D projA = new Vector2D(a.X, a.Y);
                Vector2D projB = new Vector2D(b.X, b.Y);
                DrawLine(cr, windowCenter + projA, windowCenter + projB, true);
            }
        }

        private static void DrawNormal(Context cr, Polygon poly, Vector2D shift)
        {
            Vector4D polyCenter = (poly._data[0] + poly._data[1] + poly._data[2]) / 3.0;

            Vector4D a = polyCenter;
            Vector4D b = polyCenter + NORMAL_VECTOR_SIZE * poly.NormalVector() / poly.NormalVector().Len();

            Vector2D projA = new Vector2D(a.X, a.Y);
            Vector2D projB = new Vector2D(b.X, b.Y);
            DrawLine(cr, shift + projA, shift + projB, true);
        }

        private static void DrawLine(Context cr, Vector2D point1, Vector2D point2, bool stroke)
        {
            cr.MoveTo(point1.X, point1.Y);
            cr.LineTo(point2.X, point2.Y);
            if (stroke)
            {
                cr.Stroke();
            }
        }
    }
}
