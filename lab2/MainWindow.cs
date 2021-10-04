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

        /* Drawing elements */
        [UI] private CheckButton _checkButtonDrawFrame = null;
        [UI] private CheckButton _checkButtonDrawNormalVectors = null;
        [UI] private CheckButton _checkButtonIgnoreInvisible = null;
        [UI] private CheckButton _checkButtonFillPolygons = null;
        [UI] private CheckButton _checkButtonRandomizeColours = null;

        /* Projection elements */
        [UI] private RadioButton _radioButtonNoProjection = null;
        [UI] private RadioButton _radioButtonProjX = null;
        [UI] private RadioButton _radioButtonProjY = null;
        [UI] private RadioButton _radioButtonProjZ = null;
        [UI] private RadioButton _radioButtonIsometric = null;

        [UI] private CheckButton _checkButtonAutoScale = null;

        [UI] private Adjustment _adjustmentN = null;
        [UI] private Adjustment _adjustmentR = null;
        [UI] private Adjustment _adjustmentH = null;

        /* Result matrix */
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

        private const double NORMAL_VECTOR_SIZE = 32;
        private const double ROTATION_SPEED = 0.5;
        private const double AXIS_SIZE = 40;
        private const int POINT_SIZE = 4;
        private readonly double ISOMETRIC_X = Misc.ToRadians(-35);
        private readonly double ISOMETRIC_Y = Misc.ToRadians(-45);
        private readonly double ISOMETRIC_Z = Misc.ToRadians(0);
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
        private int n = 8;
        private double r = 0;
        private double h = 0;
        private Prism prism = null;
        private bool drawFrame = true;
        private bool drawNormalVectors = false;
        private bool ignoreInvisible = true;
        private bool needToGenColors = true;
        private bool fillPolygons = false;
        private bool randomizeColours = false;
        private bool autoScale = false;
        private bool mouseMotionFlag = false;
        private Vector2D pointerPos = null;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);
            DeleteEvent += Window_DeleteEvent;
            AddValueChangedEventToAdjustments();
            AddEventsToDrawingArea();
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
            _checkButtonDrawFrame.Toggled += Redraw;
            _checkButtonDrawNormalVectors.Toggled += Redraw;
            _checkButtonIgnoreInvisible.Toggled += Redraw;
            _checkButtonFillPolygons.Toggled += Redraw;
            _checkButtonRandomizeColours.Toggled += RandomizeColoursToggledEvent;
            _radioButtonNoProjection.Toggled += Redraw;
            _radioButtonProjX.Toggled += Redraw;
            _radioButtonProjY.Toggled += Redraw;
            _radioButtonProjZ.Toggled += Redraw;
            _radioButtonIsometric.Toggled += Redraw;
            _checkButtonAutoScale.Toggled += Redraw;
            _adjustmentN.ValueChanged += AdjustmentN_ValueChangedEvent;
            _adjustmentR.ValueChanged += Redraw;
            _adjustmentH.ValueChanged += Redraw;
        }

        private void AddEventsToDrawingArea()
        {
            _drawingArea.Events |= EventMask.ButtonPressMask;
            _drawingArea.ButtonPressEvent += DrawingArea_ButtonPressEvent;
            _drawingArea.Events |= EventMask.PointerMotionMask;
            _drawingArea.MotionNotifyEvent += DrawingArea_MotionNotifyEvent;
            _drawingArea.Events |= EventMask.ButtonReleaseMask;
            _drawingArea.ButtonReleaseEvent += DrawingArea_ButtonReleaseEvent;
        }

        private void DrawingArea_ButtonPressEvent(object sender, ButtonPressEventArgs args)
        {
            mouseMotionFlag = true;
            pointerPos = new Vector2D(args.Event.X, args.Event.Y);
        }

        private void DrawingArea_MotionNotifyEvent(object sender, MotionNotifyEventArgs args)
        {
            if (mouseMotionFlag)
            {
                Vector2D pointerPosNext = new Vector2D(args.Event.X, args.Event.Y);
                Vector2D delta = pointerPosNext - pointerPos;
                _adjustmentAlpha.Value = _adjustmentAlpha.Value + ROTATION_SPEED * delta.Y;
                _adjustmentBeta.Value = _adjustmentBeta.Value + ROTATION_SPEED * delta.X;
                pointerPos = pointerPosNext;
                _drawingArea.QueueDraw();
            }
        }

        private void DrawingArea_ButtonReleaseEvent(object sender, ButtonReleaseEventArgs args)
        {
            mouseMotionFlag = false;
        }

        private void GenColors()
        {
            randomizeColours = _checkButtonRandomizeColours.Active;
            if (needToGenColors)
            {
                polygonColors = new List<Cairo.Color>();
                for (int i = 0; i < prism._polygons.Count; ++i) {
                    if (randomizeColours)
                    {
                        polygonColors.Add(Misc.GenRandColor());
                    }
                    else
                    {
                        polygonColors.Add(DEFAULT_POLYGON_COLOR);
                    }
                }
            }
            needToGenColors = false;
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
            needToGenColors = true;
            GenColors();
            _drawingArea.QueueDraw();
        }

        private void AdjustmentN_ValueChangedEvent(object sender, EventArgs args)
        {
            needToGenColors = true;
            GetData();
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
            if (autoScale)
            {
                Rescale();
            }
            DrawFigure(cr);
            if (drawNormalVectors)
            {
                DrawNormalVectors(cr, prism);
            }
            DrawAxises(cr, 0.9 * (new Vector2D(width, height)));
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

            n = (int)_adjustmentN.Value;
            r = _adjustmentR.Value;
            h = _adjustmentH.Value;
            prism = new Prism(n, r, h);

            drawFrame = _checkButtonDrawFrame.Active;
            drawNormalVectors = _checkButtonDrawNormalVectors.Active;
            ignoreInvisible = _checkButtonIgnoreInvisible.Active;
            fillPolygons = _checkButtonFillPolygons.Active;

            autoScale = _checkButtonAutoScale.Active;
        }

        private void GenMatrix()
        {
            matr = new Matrix4D();
            if (_radioButtonProjX.Active)
            {
                matr = matr * Matrix4D.ProjX();
            }
            if (_radioButtonProjY.Active)
            {
                matr = matr * Matrix4D.ProjY();
            }
            if (_radioButtonProjZ.Active)
            {
                matr = matr * Matrix4D.ProjZ();
            }
            matr = matr * Matrix4D.ScaleX(scaleX) * Matrix4D.ScaleY(scaleY) * Matrix4D.ScaleZ(scaleZ);
            if (_radioButtonIsometric.Active)
            {
                matr = matr * Matrix4D.RotX(ISOMETRIC_X) * Matrix4D.RotY(ISOMETRIC_Y) * Matrix4D.RotZ(ISOMETRIC_Z);
            }
            else
            {
                matr = matr * Matrix4D.ShiftX(shiftX) * Matrix4D.ShiftY(shiftY) * Matrix4D.ShiftZ(shiftZ);
                matr = matr * Matrix4D.RotX(alpha) * Matrix4D.RotY(beta) * Matrix4D.RotZ(gamma);
            }
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

        private void Rescale()
        {
            double maxCord = -Misc.INF;
            foreach (VertexPolygonsPair item in prism._vertices)
            {
                maxCord = Math.Max(maxCord, Math.Abs(item.First.X));
                maxCord = Math.Max(maxCord, Math.Abs(item.First.Y));
                maxCord = Math.Max(maxCord, Math.Abs(item.First.Z));
            }
            double rescaleCoef = Math.Min(width / (2 * maxCord), height / (2 * maxCord));
            Matrix4D rescaleMatr = new Matrix4D();
            rescaleMatr = rescaleMatr * Matrix4D.ScaleX(rescaleCoef) * Matrix4D.ScaleY(rescaleCoef) * Matrix4D.ScaleZ(rescaleCoef);
            prism.Transform(rescaleMatr);
        }

        private void DrawFigure(Context cr)
        {
            for (int i = 0; i < prism._polygons.Count; ++i)
            {
                if (!ignoreInvisible || prism._polygons[i].Visible()) {
                    if (fillPolygons)
                    {
                        FillPolygon(cr, i, polygonColors[i]);
                        if (drawFrame)
                        {
                            DrawPolygon(cr, i, DEFAULT_LINE_COLOR_FILL);
                        }
                    }
                    else if (drawFrame)
                    {
                        DrawPolygon(cr, i, DEFAULT_LINE_COLOR);
                    }
                }
            }
        }

        private void DrawAxises(Context cr, Vector2D shift)
        {
            Matrix4D matr = new Matrix4D();
            if (_radioButtonIsometric.Active)
            {
                matr = matr * Matrix4D.RotX(ISOMETRIC_X) * Matrix4D.RotY(ISOMETRIC_Y) * Matrix4D.RotZ(ISOMETRIC_Z);
            }
            else
            {
                matr = matr * Matrix4D.RotX(alpha) * Matrix4D.RotY(beta) * Matrix4D.RotZ(gamma);
            }
            Vector4D start = new Vector4D();
            Vector4D ox = new Vector4D(1, 0, 0, 0) * matr * AXIS_SIZE;
            Vector4D oy = new Vector4D(0, 1, 0, 0) * matr * AXIS_SIZE;
            Vector4D oz = new Vector4D(0, 0, 1, 0) * matr * AXIS_SIZE;
            Cairo.Color oxColor = new Cairo.Color(1, 0, 0);
            Cairo.Color oyColor = new Cairo.Color(0, 1, 0);
            Cairo.Color ozColor = new Cairo.Color(0, 0, 1);
            DrawVector(cr, shift + start.Proj(), shift + ox.Proj(), oxColor);
            DrawVector(cr, shift + start.Proj(), shift + oy.Proj(), oyColor);
            DrawVector(cr, shift + start.Proj(), shift + oz.Proj(), ozColor);
        }

        private void DrawNormalVectors(Context cr, Prism prism)
        {
            for (int i = 0; i < prism._polygons.Count; ++i)
            {
                if (!ignoreInvisible || prism._polygons[i].Visible()) {
                    cr.SetSourceColor(DEFAULT_NORMAL_COLOR);
                    DrawNormal(cr, prism._polygons[i]);
                }
            }
        }

        private void FillPolygon(Context cr, int id, Cairo.Color col)
        {
            Polygon poly = prism._polygons[id];
            Vector4D vertex = poly[0];
            cr.NewPath();
            cr.LineWidth = 1;
            cr.SetSourceRGB(0, 0, 0);
            cr.MoveTo(vertex.X + windowCenter.X, vertex.Y + windowCenter.Y);
            for (int i = 1; i < poly.Count; ++i)
            {
                vertex = poly[i];
                cr.LineTo(vertex.X + windowCenter.X, vertex.Y + windowCenter.Y);
            }
            cr.ClosePath();
            cr.SetSourceColor(col);
            cr.Fill();
        }

        private void DrawPolygon(Context cr, int id, Cairo.Color col)
        {
            Polygon poly = prism._polygons[id];
            for (int i = 0; i < poly.Count; ++i)
            {
                Vector4D a = poly[i];
                Vector4D b = poly[(i + 1) % poly.Count];
                DrawLine(cr, windowCenter + a.Proj(), windowCenter + b.Proj(), col);
            }
        }

        private void DrawNormal(Context cr, Polygon poly)
        {
            Vector4D polyCenter = (poly[0] + poly[1] + poly[2]) / 3.0;
            Vector4D aN = polyCenter;
            Vector4D N = poly.NormalVector();
            if (N.IsNull())
            {
                return;
            }
            Vector4D bN = aN + NORMAL_VECTOR_SIZE * N / N.Len();
            DrawVector(cr, windowCenter + aN.Proj(), windowCenter + bN.Proj(), DEFAULT_NORMAL_COLOR);
        }

        private static void DrawVector(Context cr, Vector2D point1, Vector2D point2, Cairo.Color col)
        {
            cr.LineWidth = 3;
            DrawLine(cr, (2.0 * point1 + 8.0 * point2) / 10.0, (point1 + 9.0 * point2) / 10.0, col);
            cr.LineWidth = 2;
            DrawLine(cr, (point1 + 9.0 * point2) / 10.0, point2, col);
            cr.LineWidth = 1;
            DrawLine(cr, point1, (2.0 * point1 + 8.0 * point2) / 10.0, col);
        }

        private static void DrawLine(Context cr, Vector2D point1, Vector2D point2, Cairo.Color col)
        {
            cr.SetSourceColor(col);
            cr.MoveTo(point1.X, point1.Y);
            cr.LineTo(point2.X, point2.Y);
            cr.Stroke();
        }
    }
}
