using Cairo;
using Gdk;
using Gtk;
using System;
using System.Collections.Generic;
using UI = Gtk.Builder.ObjectAttribute;

namespace lab1
{
    class MainWindow : Gtk.Window
    {
        [UI] private DrawingArea _drawingArea = null;
        [UI] private Button _buttonResetScale = null;
        [UI] private Button _buttonResetRotation = null;
        [UI] private CheckButton _autoscaleButton = null;
        [UI] private Adjustment _adjustmentCoef_a = null;
        [UI] private Adjustment _adjustmentCoef_k = null;
        [UI] private Adjustment _adjustmentCoef_B = null;
        [UI] private Adjustment _adjustmentScaleX = null;
        [UI] private Adjustment _adjustmentScaleY = null;
        [UI] private Adjustment _adjustmentSteps = null;
        [UI] private Adjustment _adjustmentRotation = null;
        [UI] private Adjustment _adjustmentRotationX = null;
        [UI] private Adjustment _adjustmentRotationY = null;

        private const int DIVISION_SCALE_PIXELS = 32;
        private const double SCALE_FACTOR = 1.1;
        private const double MIN_SCALE_FACTOR = 0.001;
        private const double DEFAULT_SCALE_FACTOR = 50.0;
        private const double DIVISION_SCALE_SIZE = 4;
        private const double ROT_POINT_SIZE = 5;

        private int width = 0;
        private int height = 0;
        private bool mouseMotionFlag = false;
        private Vector2D center = new Vector2D();
        private double OXdown = 0;
        private double OXup = 0;
        private double OYleft = 0;
        private double OYRight = 0;
        private Vector2D pointerPos = new Vector2D();
        private Vector2D scale = new Vector2D();
        private Vector2D shift = new Vector2D();
        private double a = 0;
        private double k = 0;
        private double B = 0;
        private List<Vector2D> points = new List<Vector2D>();
        private double theta = 0;
        private Vector2D rotationShift = new Vector2D();
        private double steps = 0;
        private double step = 0;
        private double maxValueX = -Misc.INF;
        private double maxValueY = -Misc.INF;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);
            DeleteEvent += Window_DeleteEvent;
            _buttonResetScale.Clicked += ButtonResetScale_Clicked;
            _buttonResetRotation.Clicked += ButtonResetRotation_Clicked;
            AddValueChangedEventToAdjustments();
            AddEventsToDrawingArea();
            _drawingArea.Drawn += DrawingArea_Draw;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void ButtonResetScale_Clicked(object Sender, EventArgs arg)
        {
            if (_autoscaleButton.Active)
            {
                return;
            }
            shift.X = 0;
            shift.Y = 0;
            _adjustmentScaleX.Value = DEFAULT_SCALE_FACTOR;
            _adjustmentScaleY.Value = DEFAULT_SCALE_FACTOR;
            _drawingArea.QueueDraw();
        }

        private void ButtonResetRotation_Clicked(object Sender, EventArgs arg)
        {
            _adjustmentRotation.Value = 0;
            _adjustmentRotationX.Value = 0;
            _adjustmentRotationY.Value = 0;
            _drawingArea.QueueDraw();
        }

        private void AddValueChangedEventToAdjustments()
        {
            _adjustmentCoef_a.ValueChanged += Adjustment_ValueChangedEvent;
            _adjustmentCoef_k.ValueChanged += Adjustment_ValueChangedEvent;
            _adjustmentCoef_B.ValueChanged += Adjustment_ValueChangedEvent;
            _adjustmentScaleX.ValueChanged += Adjustment_ValueChangedEvent;
            _adjustmentScaleY.ValueChanged += Adjustment_ValueChangedEvent;
            _adjustmentSteps.ValueChanged += Adjustment_ValueChangedEvent;
            _autoscaleButton.Toggled += AutoscaleButton_ToggledEvent;
            _adjustmentRotation.ValueChanged += Adjustment_ValueChangedEvent;
            _adjustmentRotationX.ValueChanged += Adjustment_ValueChangedEvent;
            _adjustmentRotationY.ValueChanged += Adjustment_ValueChangedEvent;
        }

        private void AddEventsToDrawingArea()
        {
            _drawingArea.Events |= EventMask.ScrollMask;
            _drawingArea.ScrollEvent += DrawingArea_ScrollEvent;
            _drawingArea.Events |= EventMask.ButtonPressMask;
            _drawingArea.ButtonPressEvent += DrawingArea_ButtonPressEvent;
            _drawingArea.Events |= EventMask.PointerMotionMask;
            _drawingArea.MotionNotifyEvent += DrawingArea_MotionNotifyEvent;
            _drawingArea.Events |= EventMask.ButtonReleaseMask;
            _drawingArea.ButtonReleaseEvent += DrawingArea_ButtonReleaseEvent;
        }

        private void DrawingArea_Draw(object sender, DrawnArgs args)
        {
            Context ct = args.Cr;
            ct.Antialias = Antialias.Subpixel;
            /* Silver */
            ct.SetSourceRGB(0.75, 0.75, 0.75);
            ct.Paint();
            ct.LineWidth = 1;
            UpdateScreen(ct);
        }

        private void Adjustment_ValueChangedEvent(object sender, EventArgs args)
        {
            _drawingArea.QueueDraw();
        }

        private void AutoscaleButton_ToggledEvent(object sender, EventArgs args)
        {
            if (_autoscaleButton.Active)
            {
                shift.X = 0;
                shift.Y = 0;
            }
            _drawingArea.QueueDraw();
        }

        private void DrawingArea_ScrollEvent(object sender, ScrollEventArgs a)
        {
            if (_autoscaleButton.Active)
            {
                return;
            }
            if (a.Event.Direction == ScrollDirection.Up && scale.X < _adjustmentScaleX.Upper && scale.Y < _adjustmentScaleY.Upper)
            {
                scale = scale * SCALE_FACTOR;
            }
            else if (a.Event.Direction == ScrollDirection.Down)
            {
                scale = scale * (1 / SCALE_FACTOR);
            }
            _adjustmentScaleX.Value = Math.Max(MIN_SCALE_FACTOR, scale.X);
            _adjustmentScaleY.Value = Math.Max(MIN_SCALE_FACTOR, scale.Y);
            _drawingArea.QueueDraw();
        }

        private void DrawingArea_ButtonPressEvent(object sender, ButtonPressEventArgs a)
        {
            if (_autoscaleButton.Active)
            {
                return;
            }
            mouseMotionFlag = true;
            pointerPos.X = a.Event.X;
            pointerPos.Y = a.Event.Y;
        }

        private void DrawingArea_MotionNotifyEvent(object sender, MotionNotifyEventArgs a)
        {
            if (mouseMotionFlag)
            {
                Vector2D pointerPosNext = new Vector2D(a.Event.X, a.Event.Y);
                shift = shift + pointerPosNext - pointerPos;
                pointerPos = pointerPosNext;
                _drawingArea.QueueDraw();
            }
        }

        private void DrawingArea_ButtonReleaseEvent(object sender, ButtonReleaseEventArgs a)
        {
            mouseMotionFlag = false;
        }

        private void UpdateScreen(Context ct)
        {
            GetData();
            points = Misc.GenFunctionValues(a, k, B, step);
            RotateAndRescale();
            center = new Vector2D(width / 2.0, height / 2.0);
            center = center + shift;
            for (int i = 0; i < points.Count; ++i)
            {
                points[i].X = scale.X * points[i].X;
                points[i].Y = scale.Y * points[i].Y;
                points[i] = points[i] + center;
                points[i].Y = 2 * center.Y - points[i].Y;
            }
            DrawAxices(ct);
            DrawScale(ct);
            DrawScale(ct);
            DrawPlot(ct);
            DrawRotationPoint(ct);
        }

        private void GetData()
        {
            /* Drawing area size */
            width = _drawingArea.Window.Width;
            height = _drawingArea.Window.Height;

            /* Coefficients */
            a = _adjustmentCoef_a.Value;
            k = _adjustmentCoef_k.Value;
            B = _adjustmentCoef_B.Value;

            /* Scale values */
            scale.X = _adjustmentScaleX.Value;
            scale.Y = _adjustmentScaleY.Value;

            /* Rotation values */
            theta = Misc.ToRadians(_adjustmentRotation.Value);
            rotationShift.X = _adjustmentRotationX.Value;
            rotationShift.Y = _adjustmentRotationY.Value;

            /* Aproximation values */
            steps = _adjustmentSteps.Value;
            step = B / steps;
        }

        private void RotateAndRescale()
        {
            maxValueX = -Misc.INF;
            maxValueY = -Misc.INF;
            foreach (Vector2D point in points)
            {
                point.Rotate(theta, rotationShift);
                maxValueX = Math.Max(maxValueX, Math.Abs(point.X));
                maxValueY = Math.Max(maxValueY, Math.Abs(point.Y));
            }
            if (_autoscaleButton.Active)
            {
                Rescale();
            }
        }

        private void Rescale()
        {
            double minScale = Math.Min(height / (2 * maxValueY), width / (2 * maxValueX));
            _adjustmentScaleX.Value = minScale;
            _adjustmentScaleY.Value = minScale;
            scale.X = minScale;
            scale.Y = minScale;
        }

        private void DrawAxices(Context ct)
        {
            ct.SetSourceRGB(0, 0, 0);
            DrawLine(ct, new Vector2D(0, center.Y), new Vector2D(width, center.Y));
            DrawLine(ct, new Vector2D(center.X, 0), new Vector2D(center.X, height));
        }

        private void DrawScale(Context ct)
        {
            OXdown = center.Y + DIVISION_SCALE_SIZE;
            OXup = center.Y - DIVISION_SCALE_SIZE;
            OYleft = center.X - DIVISION_SCALE_SIZE;
            OYRight = center.X + DIVISION_SCALE_SIZE;
            DrawScaleOX(ct);
            DrawScaleOY(ct);
        }

        private void DrawScaleOX(Context ct)
        {
            double scaleDiv = 1e-3;
            for (int degree = -3; degree <= 5; ++degree) {
                if (scale.X * scaleDiv > DIVISION_SCALE_PIXELS)
                {
                    for (int i = 1; center.X + i * scale.X * scaleDiv < width; ++i)
                    {
                        DrawLine(ct, new Vector2D(center.X + i * scale.X * scaleDiv, OXdown), new Vector2D(center.X + i * scale.X * scaleDiv, OXup));
                        PrintText(ct, new Vector2D(center.X + i * scale.X * scaleDiv - 10, OXdown + 10), Misc.NumToString(i, degree));
                    }
                    for (int i = 1; center.X - i * scale.X * scaleDiv > 0; ++i)
                    {
                        DrawLine(ct, new Vector2D(center.X - i * scale.X * scaleDiv, OXdown), new Vector2D(center.X - i * scale.X * scaleDiv, OXup));
                        PrintText(ct, new Vector2D(center.X - i * scale.X * scaleDiv - 10, OXdown + 10), Misc.NumToString(-i, degree));
                    }
                    break;
                }
                scaleDiv = scaleDiv * 10;
            }
        }

        private void DrawScaleOY(Context ct)
        {
            double scaleDiv = 1e-3;
            for (int degree = -3; degree <= 5; ++degree) {
                if (scale.Y * scaleDiv > DIVISION_SCALE_PIXELS)
                {
                    for (int i = 1; center.Y + i * scale.Y * scaleDiv < height; ++i)
                    {
                        DrawLine(ct, new Vector2D(OYleft, center.Y + i * scale.Y * scaleDiv), new Vector2D(OYRight, center.Y + i * scale.Y * scaleDiv));
                        PrintText(ct, new Vector2D(OYRight + 10, center.Y + i * scale.Y * scaleDiv + 5), Misc.NumToString(-i, degree));
                    }
                    for (int i = 1; center.Y - i * scale.Y * scaleDiv > 0; ++i)
                    {
                        DrawLine(ct, new Vector2D(OYleft, center.Y - i * scale.Y * scaleDiv), new Vector2D(OYRight, center.Y - i * scale.Y * scaleDiv));
                        PrintText(ct, new Vector2D(OYRight + 10, center.Y - i * scale.Y * scaleDiv + 5), Misc.NumToString(i, degree));
                    }
                    break;
                }
                scaleDiv = scaleDiv * 10;
            }
        }

        private void DrawPlot(Context ct)
        {
            ct.SetSourceRGB(0, 0, 1);
            for (int i = 1; i < points.Count; ++i)
            {
                DrawLine(ct, points[i], points[i - 1]);
            }
        }

        private void DrawRotationPoint(Context ct)
        {
            Vector2D rotationShiftScaled = new Vector2D(rotationShift.X, rotationShift.Y);
            rotationShiftScaled.X = scale.X * rotationShiftScaled.X;
            rotationShiftScaled.Y = -scale.Y * rotationShiftScaled.Y;
            Vector2D rectCenter = center + rotationShiftScaled;
            ct.SetSourceRGB(1, 0, 0);
            ct.Rectangle(rectCenter.X - ROT_POINT_SIZE / 2, rectCenter.Y - ROT_POINT_SIZE / 2, ROT_POINT_SIZE, ROT_POINT_SIZE);
            ct.Fill();
            ct.Stroke();
        }

        private static void DrawLine(Context context, Vector2D point1, Vector2D point2)
        {
            context.MoveTo(point1.X, point1.Y);
            context.LineTo(point2.X, point2.Y);
            context.Stroke();
        }

        private static void PrintText(Context context, Vector2D pos, String text)
        {
            context.MoveTo(pos.X, pos.Y);
            context.ShowText(text);
            context.Stroke();
        }
    }
}
