using Cairo;
using Gdk;
using Gtk;
using System;
using System.Collections.Generic;
using UI = Gtk.Builder.ObjectAttribute;

namespace lab3
{
    class MainWindow : Gtk.Window
    {
        [UI] private DrawingArea _drawingArea = null;
        private double width = 0;
        private double height = 0;
        private Vector2D windowCenter = null;
        private Figure fig = null;
        private List<Misc.Colour> polygonColors = null;
        private List<Misc.Colour> polygonShade = null;
        private bool mouseMotionFlag = false;
        private Vector2D pointerPos = null;

        /* Drawing constants */
        private const double NORMAL_VECTOR_SIZE = 32;
        private const double ROTATION_SPEED = 0.5;
        private const double AXIS_SIZE = 40;
        private const double POINT_SIZE = 4;
        private const double AXIS_POS = 0.9;
        private readonly Misc.Colour DEFAULT_LINE_COLOUR = new Misc.Colour(0, 0, 1);
        private readonly Misc.Colour DEFAULT_NORMAL_COLOR = new Misc.Colour(1, 0, 0);
        private readonly Misc.Colour DEFAULT_LINE_COLOUR_FILL = new Misc.Colour(0, 0, 0);
        private readonly Misc.Colour OX_COLOUR = new Misc.Colour(1, 0, 0);
        private readonly Misc.Colour OY_COLOUR = new Misc.Colour(0, 1, 0);
        private readonly Misc.Colour OZ_COLOUR = new Misc.Colour(0, 0, 1);
        private readonly Cairo.Color GRADIENT_STOP_COLOR = new Cairo.Color(0, 0, 0);

        /* Movement elements */
        [UI] private Adjustment _adjustmentAlpha = null;
        [UI] private Adjustment _adjustmentBeta = null;
        [UI] private Adjustment _adjustmentGamma = null;
        private double alpha = 0;
        private double beta = 0;
        private double gamma = 0;

        [UI] private Adjustment _adjustmentScaleX = null;
        [UI] private Adjustment _adjustmentScaleY = null;
        [UI] private Adjustment _adjustmentScaleZ = null;
        private double scaleX = 0;
        private double scaleY = 0;
        private double scaleZ = 0;

        [UI] private Adjustment _adjustmentShiftX = null;
        [UI] private Adjustment _adjustmentShiftY = null;
        [UI] private Adjustment _adjustmentShiftZ = null;
        private double shiftX = 0;
        private double shiftY = 0;
        private double shiftZ = 0;

        /* Drawing elements */
        [UI] private CheckButton _checkButtonDrawFrame = null;
        [UI] private CheckButton _checkButtonDrawNormalVectors = null;
        [UI] private CheckButton _checkButtonIgnoreInvisible = null;
        [UI] private CheckButton _checkButtonFillPolygons = null;
        [UI] private CheckButton _checkButtonRandomizeColours = null;
        private bool needToGenColors = true;

        /* Projection elements */
        [UI] private RadioButton _radioButtonNoProjection = null;
        [UI] private RadioButton _radioButtonProjX = null;
        [UI] private RadioButton _radioButtonProjY = null;
        [UI] private RadioButton _radioButtonProjZ = null;
        [UI] private RadioButton _radioButtonIsometric = null;
        private readonly double ISOMETRIC_X = Misc.ToRadians(35);
        private readonly double ISOMETRIC_Y = Misc.ToRadians(-45);
        private readonly double ISOMETRIC_Z = Misc.ToRadians(0);
        [UI] private CheckButton _checkButtonAutoScale = null;
        private Matrix4D rescaleMatr = new Matrix4D();

        /* Figure parameters */
        [UI] private Adjustment _adjustmentVerticalN = null;
        [UI] private Adjustment _adjustmentHorizontalN = null;
        [UI] private Adjustment _adjustmentR = null;
        [UI] private Adjustment _adjustmentTheta = null;
        private int verticalN = 0;
        private int horizontalN = 0;
        private double r = 0;
        private double theta = 0;

        /* Shading */
        [UI] private RadioButton _radioButtonNoShading = null;
        [UI] private RadioButton _radioButtonFlat = null;
        [UI] private RadioButton _radioButtonGouraud = null;
        private CairoSurface crSurface = null;

        /* Material parameters */
        [UI] private Adjustment _adjustmentFigureR = null;
        [UI] private Adjustment _adjustmentFigureG = null;
        [UI] private Adjustment _adjustmentFigureB = null;
        Misc.Colour figureColour = null;

        [UI] private Adjustment _adjustmentKaR = null;
        [UI] private Adjustment _adjustmentKaG = null;
        [UI] private Adjustment _adjustmentKaB = null;
        private Misc.Colour ka = null;

        [UI] private Adjustment _adjustmentKdR = null;
        [UI] private Adjustment _adjustmentKdG = null;
        [UI] private Adjustment _adjustmentKdB = null;
        private Misc.Colour kd = null;
        private const double SHADING_COEF_K = 0.005;

        [UI] private Adjustment _adjustmentKsR = null;
        [UI] private Adjustment _adjustmentKsG = null;
        [UI] private Adjustment _adjustmentKsB = null;
        private Misc.Colour ks = null;
        private readonly Vector4D INF_VEC = new Vector4D(0, 0, -Misc.INF, 0);
        private const double SHADING_COEF_P = 4;

        /* Light source parameters */
        [UI] private Adjustment _adjustmentLightX = null;
        [UI] private Adjustment _adjustmentLightY = null;
        [UI] private Adjustment _adjustmentLightZ = null;
        private Vector4D lightSource = new Vector4D();

        [UI] private Adjustment _adjustmentIaR = null;
        [UI] private Adjustment _adjustmentIaG = null;
        [UI] private Adjustment _adjustmentIaB = null;
        private Misc.Colour ia = null;

        [UI] private Adjustment _adjustmentIlR = null;
        [UI] private Adjustment _adjustmentIlG = null;
        [UI] private Adjustment _adjustmentIlB = null;
        private Misc.Colour il = null;

        [UI] private CheckButton _checkButtonDrawSource = null;
        private const double SOURCE_SIZE = 5;
        [UI] private CheckButton _checkButtonDrawRays = null;
        private const double RAY_VECTOR_SIZE = 50;
        private readonly Misc.Colour DEFAULT_LIGHT_RAY_COLOUR = new Misc.Colour(1, 1, 0);
        [UI] private CheckButton _checkButtonAutoScaleLight = null;

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
        private Matrix4D matr = null;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);
            DeleteEvent += Window_DeleteEvent;
            AddValueChangedEventToMovement();
            AddToggledEventToDrawingAndProjection();
            AddValueChangedEventToFigureParams();
            AddEventsToDrawingArea();
            AddToggledEventToShading();
            AddValueChangedEventToMaterialParams();
            AddValueChangedEventToLightSource();
            crSurface = new CairoSurface(_drawingArea);
            _drawingArea.Drawn += DrawingArea_DrawnEvent;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void AddValueChangedEventToMovement()
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
        }

        private void AddToggledEventToDrawingAndProjection()
        {
            _checkButtonDrawFrame.Toggled += Redraw;
            _checkButtonDrawNormalVectors.Toggled += Redraw;
            _checkButtonIgnoreInvisible.Toggled += Redraw;
            _checkButtonFillPolygons.Toggled += Redraw;
            _checkButtonRandomizeColours.Toggled += RecolourAndDraw;

            _radioButtonNoProjection.Toggled += Redraw;
            _radioButtonProjX.Toggled += Redraw;
            _radioButtonProjY.Toggled += Redraw;
            _radioButtonProjZ.Toggled += Redraw;
            _radioButtonIsometric.Toggled += Redraw;
            _checkButtonAutoScale.Toggled += Redraw;
        }

        private void AddValueChangedEventToFigureParams()
        {
            _adjustmentHorizontalN.ValueChanged += GenerateFigureAndDraw;
            _adjustmentVerticalN.ValueChanged += GenerateFigureAndDraw;
            _adjustmentR.ValueChanged += GenerateFigureAndDraw;
            _adjustmentTheta.ValueChanged += GenerateFigureAndDraw;
        }

        private void AddToggledEventToShading()
        {
            _radioButtonNoShading.Toggled += Redraw;
            _radioButtonFlat.Toggled += Redraw;
            _radioButtonGouraud.Toggled += Redraw;
        }

        private void AddValueChangedEventToMaterialParams()
        {
            _adjustmentFigureR.ValueChanged += RecolourAndDraw;
            _adjustmentFigureG.ValueChanged += RecolourAndDraw;
            _adjustmentFigureB.ValueChanged += RecolourAndDraw;

            _adjustmentKaR.ValueChanged += Redraw;
            _adjustmentKaG.ValueChanged += Redraw;
            _adjustmentKaB.ValueChanged += Redraw;
            _adjustmentKdR.ValueChanged += Redraw;
            _adjustmentKdG.ValueChanged += Redraw;
            _adjustmentKdB.ValueChanged += Redraw;
            _adjustmentKsR.ValueChanged += Redraw;
            _adjustmentKsG.ValueChanged += Redraw;
            _adjustmentKsB.ValueChanged += Redraw;
        }

        private void AddValueChangedEventToLightSource()
        {
            _adjustmentLightX.ValueChanged += Redraw;
            _adjustmentLightY.ValueChanged += Redraw;
            _adjustmentLightZ.ValueChanged += Redraw;

            _adjustmentIaR.ValueChanged += Redraw;
            _adjustmentIaG.ValueChanged += Redraw;
            _adjustmentIaB.ValueChanged += Redraw;
            _adjustmentIlR.ValueChanged += Redraw;
            _adjustmentIlG.ValueChanged += Redraw;
            _adjustmentIlB.ValueChanged += Redraw;

            _checkButtonDrawSource.Toggled += Redraw;
            _checkButtonDrawRays.Toggled += Redraw;
            _checkButtonAutoScaleLight.Toggled += Redraw;

        }

        private void GetWindowData()
        {
            width = _drawingArea.Window.Width;
            height = _drawingArea.Window.Height;
            windowCenter = new Vector2D(width / 2, height / 2);
        }

        private void GetMovementData()
        {
            alpha = Misc.ToRadians(_adjustmentAlpha.Value);
            beta = Misc.ToRadians(_adjustmentBeta.Value);
            gamma = Misc.ToRadians(_adjustmentGamma.Value);

            shiftX = _adjustmentShiftX.Value;
            shiftY = _adjustmentShiftY.Value;
            shiftZ = _adjustmentShiftZ.Value;

            scaleX = _adjustmentScaleX.Value;
            scaleY = _adjustmentScaleY.Value;
            scaleZ = _adjustmentScaleZ.Value;
        }

        private void GetFigureData()
        {
            horizontalN = (int)_adjustmentHorizontalN.Value;
            verticalN = (int)_adjustmentVerticalN.Value;
            r = _adjustmentR.Value;
            theta = _adjustmentTheta.Value;
        }

        private void GetMaterialData()
        {
            figureColour = new Misc.Colour(_adjustmentFigureR.Value, _adjustmentFigureG.Value, _adjustmentFigureB.Value) / Misc.MAX_RGB;
            ka = new Misc.Colour(_adjustmentKaR.Value, _adjustmentKaG.Value, _adjustmentKaB.Value);
            kd = new Misc.Colour(_adjustmentKdR.Value, _adjustmentKdG.Value, _adjustmentKdB.Value);
            ks = new Misc.Colour(_adjustmentKsR.Value, _adjustmentKsG.Value, _adjustmentKsB.Value);
        }

        private void GetLightSourceData()
        {
            lightSource = new Vector4D(_adjustmentLightX.Value, _adjustmentLightY.Value, _adjustmentLightZ.Value, 0);
            ia = new Misc.Colour(_adjustmentIaR.Value, _adjustmentIaG.Value, _adjustmentIaB.Value);
            il = new Misc.Colour(_adjustmentIlR.Value, _adjustmentIlG.Value, _adjustmentIlB.Value);
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

        private void RecolourAndDraw(object sender, EventArgs args)
        {
            needToGenColors = true;
            _drawingArea.QueueDraw();
        }

        private void GenerateFigureAndDraw(object sender, EventArgs args)
        {
            GenFigure();
            needToGenColors = true;
            GenColors();
            GenShade();
            _drawingArea.QueueDraw();
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
                Vector2D delta = ROTATION_SPEED * (pointerPosNext - pointerPos);
                _adjustmentAlpha.Value = _adjustmentAlpha.Value - delta.Y;
                _adjustmentBeta.Value = _adjustmentBeta.Value + delta.X;
                pointerPos = pointerPosNext;
                _drawingArea.QueueDraw();
            }
        }

        private void DrawingArea_ButtonReleaseEvent(object sender, ButtonReleaseEventArgs args)
        {
            mouseMotionFlag = false;
        }

        private void DrawingArea_DrawnEvent(object sender, DrawnArgs args)
        {

            GetWindowData();
            Context cr = args.Cr;
            cr.Antialias = Antialias.Subpixel;
            cr.SetSourceRGB(0, 0, 0);
            cr.Paint();
            cr.LineWidth = 1;
            if (fig == null)
            {
                GenFigure();
                needToGenColors = true;
            }
            GenMatrix();
            fig.Transform(matr);
            if (_checkButtonAutoScale.Active)
            {
                Rescale();
            }
            GenColors();
            GenShade();
            DrawFigure(cr);
            if (_checkButtonDrawNormalVectors.Active)
            {
                DrawNormalVectors(cr);
            }
            if (_checkButtonDrawRays.Active)
            {
                DrawRays(cr);
            }
            if (_checkButtonDrawSource.Active)
            {
                DrawSource(cr);
            }
            DrawAxises(cr, AXIS_POS * (new Vector2D(width, height)));
        }

        private void GenFigure()
        {
            GetFigureData();
            fig = new Figure(horizontalN, verticalN, r, theta);
        }

        private void GenColors()
        {
            GetMaterialData();
            bool randColours = _checkButtonRandomizeColours.Active;
            if (needToGenColors)
            {
                polygonColors = new List<Misc.Colour>();
                for (int i = 0; i < fig._polygons.Count; ++i) {
                    if (randColours)
                    {
                        polygonColors.Add(Misc.Colour.GenRandColor());
                    }
                    else
                    {
                        polygonColors.Add(figureColour);
                    }
                }
                needToGenColors = false;
            }
        }

        private void GenMatrix()
        {
            GetMovementData();
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
            foreach (VertexPolygonsPair item in fig._vertices)
            {
                maxCord = Math.Max(maxCord, Math.Abs(item.First.X));
                maxCord = Math.Max(maxCord, Math.Abs(item.First.Y));
                maxCord = Math.Max(maxCord, Math.Abs(item.First.Z));
            }
            if (_checkButtonAutoScaleLight.Active && _checkButtonDrawSource.Active)
            {
                maxCord = Math.Max(maxCord, Math.Abs(lightSource.X - SOURCE_SIZE));
                maxCord = Math.Max(maxCord, Math.Abs(lightSource.Y - SOURCE_SIZE));
                maxCord = Math.Max(maxCord, Math.Abs(lightSource.X + SOURCE_SIZE));
                maxCord = Math.Max(maxCord, Math.Abs(lightSource.Y + SOURCE_SIZE));
                maxCord = Math.Max(maxCord, Math.Abs(lightSource.Z));
            }
            double rescaleCoef = Math.Min(width / (2 * maxCord), height / (2 * maxCord));
            rescaleMatr = new Matrix4D();
            rescaleMatr = rescaleMatr * Matrix4D.ScaleX(rescaleCoef) * Matrix4D.ScaleY(rescaleCoef) * Matrix4D.ScaleZ(rescaleCoef);
            fig.Transform(matr * rescaleMatr);
        }

        private void GenShade()
        {
            GetMaterialData();
            GetLightSourceData();
            polygonShade = new List<Misc.Colour>();
            if (_radioButtonNoShading.Active)
            {
                foreach (Misc.Colour col in polygonColors)
                {
                    polygonShade.Add(col);
                }
                return;
            }
            if (_radioButtonFlat.Active)
            {
                for (int i = 0; i < polygonColors.Count; ++i)
                {
                    Vector4D N = fig._polygons[i].NormalVector();
                    Vector4D curPolyCenter = fig._polygons[i].GetCenter();
                    polygonShade.Add(GenShadePoint(polygonColors[i], curPolyCenter, N));
                }
            }
        }

        private Vector4D GetVectorL(Vector4D p)
        {
            return lightSource - p;
        }

        private Vector4D GetVectorR(Vector4D p, Vector4D n, Vector4D l)
        {
            return 2 * n * Vector4D.Dot(n, l) - l;
        }

        private Vector4D GetVectorS(Vector4D p)
        {
            return INF_VEC - p;
        }

        private Misc.Colour GenShadePoint(Misc.Colour col, Vector4D point, Vector4D N)
        {
            N.Normalize();
            Vector4D L = GetVectorL(point);
            double dist = 0.001 * L.Len();
            L.Normalize();
            Vector4D R = GetVectorR(point, N, L);
            R.Normalize();
            Vector4D S = GetVectorS(point);
            S.Normalize();
            Misc.Colour ambientIntensity = ka * ia;
            Misc.Colour diffuseIntensity = kd * il * Vector4D.Dot(L, N);
            Misc.Colour specularIntensity = ks * il * Math.Pow(Vector4D.Dot(R, S), SHADING_COEF_P);
            if (Vector4D.Dot(L, N) < Misc.EPS)
            {
                diffuseIntensity = new Misc.Colour();
                specularIntensity = new Misc.Colour();
            }
            if (Vector4D.Dot(R, S) < Misc.EPS)
            {
                specularIntensity = new Misc.Colour();
            }
            ambientIntensity.Clamp();
            diffuseIntensity.Clamp();
            specularIntensity.Clamp();
            Misc.Colour res = col * (ambientIntensity + (diffuseIntensity + specularIntensity) / (dist + SHADING_COEF_K));
            res.Clamp();
            return res;
        }

        private void DrawFigure(Context cr)
        {
            if (_checkButtonFillPolygons.Active && _radioButtonGouraud.Active)
            {
                fig.GenVertN();
                crSurface.BeginUpdate(cr);
                for (int i = 0; i < fig._polygons.Count; ++i)
                {
                    if (!_checkButtonIgnoreInvisible.Active || fig._polygons[i].Visible())
                    {
                        GouraudPolygon(cr, i);
                    }
                }
                crSurface.EndUpdate();
            }
            if (_checkButtonFillPolygons.Active && !_radioButtonGouraud.Active)
            {
                for (int i = 0; i < fig._polygons.Count; ++i)
                {
                    if (!_checkButtonIgnoreInvisible.Active || fig._polygons[i].Visible())
                    {
                        FillPolygon(cr, fig._polygons[i], polygonShade[i]);
                    }
                }
            }
            if (_checkButtonDrawFrame.Active)
            {
                for (int i = 0; i < fig._polygons.Count; ++i)
                {
                    if (!_checkButtonIgnoreInvisible.Active || fig._polygons[i].Visible())
                    {
                        if (_checkButtonFillPolygons.Active)
                        {
                            DrawPolygon(cr, i, DEFAULT_LINE_COLOUR_FILL);
                        }
                        else
                        {
                            DrawPolygon(cr, i, DEFAULT_LINE_COLOUR);
                        }
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
            DrawVector(cr, shift + start.Proj(), shift + ox.Proj(), OX_COLOUR);
            DrawVector(cr, shift + start.Proj(), shift + oy.Proj(), OY_COLOUR);
            DrawVector(cr, shift + start.Proj(), shift + oz.Proj(), OZ_COLOUR);
        }

        private void DrawNormalVectors(Context cr)
        {
            foreach (Polygon poly in fig._polygons)
            {
                if (!_checkButtonIgnoreInvisible.Active || poly.Visible()) {
                    DrawNormal(cr, poly);
                }
            }
        }

        private void DrawSource(Context cr)
        {
            Vector4D lightSourceScaled = lightSource * rescaleMatr;
            Vector4D a = new Vector4D(lightSourceScaled);
            a.X = a.X - SOURCE_SIZE;
            a.Y = a.Y - SOURCE_SIZE;

            Vector4D b = new Vector4D(lightSourceScaled);
            b.X = b.X - SOURCE_SIZE;
            b.Y = b.Y + SOURCE_SIZE;

            Vector4D c = new Vector4D(lightSourceScaled);
            c.X = c.X + SOURCE_SIZE;
            c.Y = c.Y + SOURCE_SIZE;

            Vector4D d = new Vector4D(lightSourceScaled);
            d.X = d.X + SOURCE_SIZE;
            d.Y = d.Y - SOURCE_SIZE;

            Polygon low = new Polygon(a, b, c);
            Polygon high = new Polygon(c, d, a);
            FillPolygon(cr, low, il);
            FillPolygon(cr, high, il);
        }

        private void GouraudPolygon(Context cr, int id)
        {
            List<Vector4D> polyVertices = new List<Vector4D>();
            List<Misc.Colour> vertColours = new List<Misc.Colour>();
            Polygon poly = fig._polygons[id];
            for (int i = 0; i < poly.Count; ++i)
            {
                polyVertices.Add(poly[i]);
                vertColours.Add(GenShadePoint(polygonColors[id], poly[i], poly._normals[i]));
            }
            if (Vector4D.ApproxEqual(poly[0], poly[1]) || Vector4D.ApproxEqual(poly[2], poly[1]) || Vector4D.ApproxEqual(poly[0], poly[2]))
            {
                return;
            }
            if (Misc.Colour.ApproxEqual(vertColours[0], vertColours[1]) && Misc.Colour.ApproxEqual(vertColours[0], vertColours[1]) ) {
                crSurface.DrawTriangle(vertColours[0], windowCenter + polyVertices[0].Proj(), windowCenter + polyVertices[1].Proj(), windowCenter + polyVertices[2].Proj());
                return;
            }
            crSurface.DrawTriangle(vertColours[0], windowCenter + polyVertices[0].Proj(), vertColours[1], windowCenter + polyVertices[1].Proj(), vertColours[2], windowCenter + polyVertices[2].Proj());
        }

        private void FillPolygon(Context cr, Polygon poly, Misc.Colour col)
        {
            Vector4D vertex = poly[0];
            cr.MoveTo(vertex.X + windowCenter.X, vertex.Y + windowCenter.Y);
            for (int i = 1; i < poly.Count; ++i)
            {
                vertex = poly[i];
                cr.LineTo(vertex.X + windowCenter.X, vertex.Y + windowCenter.Y);
            }
            cr.ClosePath();
            cr.SetSourceColor(col.ToCairo());
            cr.StrokePreserve();
            cr.Fill();
        }

        private void DrawPolygon(Context cr, int id, Misc.Colour col)
        {
            Polygon poly = fig._polygons[id];
            for (int i = 0; i < poly.Count; ++i)
            {
                Vector4D a = poly[i];
                Vector4D b = poly[(i + 1) % poly.Count];
                DrawLine(cr, windowCenter + a.Proj(), windowCenter + b.Proj(), col);
            }
        }

        private void DrawNormal(Context cr, Polygon poly)
        {
            Vector4D polyCenter = poly.GetCenter();
            Vector4D aN = polyCenter;
            Vector4D N = poly.NormalVector();
            if (N.IsNull())
            {
                return;
            }
            N.Normalize();
            Vector4D bN = aN + NORMAL_VECTOR_SIZE * N;
            DrawVector(cr, windowCenter + aN.Proj(), windowCenter + bN.Proj(), DEFAULT_NORMAL_COLOR);
        }

        private void DrawRays(Context cr)
        {
            foreach (Polygon poly in fig._polygons)
            {
                if (!_checkButtonIgnoreInvisible.Active || poly.Visible()) {
                    DrawRayVectors(cr, poly);
                }
            }
        }

        private void DrawRayVectors(Context cr, Polygon poly)
        {
            Vector4D aN = poly.GetCenter();
            Vector4D N = poly.NormalVector();
            if (N.IsNull())
            {
                return;
            }
            N.Normalize();
            Vector4D L = GetVectorL(aN);
            L.Normalize();
            if (Vector4D.Dot(L, N) > -Misc.EPS)
            {
                Vector4D lN = aN + RAY_VECTOR_SIZE * L;
                DrawVector(cr, windowCenter + lN.Proj(), windowCenter + aN.Proj(), DEFAULT_LIGHT_RAY_COLOUR);

                Vector4D R = GetVectorR(aN, N, L);
                R.Normalize();
                Vector4D rN = aN + RAY_VECTOR_SIZE * R;
                DrawVector(cr, windowCenter + aN.Proj(), windowCenter + rN.Proj(), DEFAULT_LIGHT_RAY_COLOUR);
            }
        }

        private static void DrawVector(Context cr, Vector2D point1, Vector2D point2, Misc.Colour col)
        {
            cr.LineWidth = 3;
            DrawLine(cr, (2.0 * point1 + 8.0 * point2) / 10.0, (point1 + 9.0 * point2) / 10.0, col);
            cr.LineWidth = 2;
            DrawLine(cr, (point1 + 9.0 * point2) / 10.0, point2, col);
            cr.LineWidth = 1;
            DrawLine(cr, point1, (2.0 * point1 + 8.0 * point2) / 10.0, col);
        }

        private static void DrawLine(Context cr, Vector2D point1, Vector2D point2, Misc.Colour col)
        {
            cr.SetSourceColor(col.ToCairo());
            cr.MoveTo(point1.X, point1.Y);
            cr.LineTo(point2.X, point2.Y);
            cr.Stroke();
        }
    }
}
