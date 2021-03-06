using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;
using Gdk;
using Gtk;
using SharpGL;
using CGPlatform;
using Window = Gtk.Window;
using UI = Gtk.Builder.ObjectAttribute;

namespace cw
{
    unsafe class MainWindow : Gtk.Window
    {
        [UI] private GLArea _GLArea = null;

        /* Shaders */
        private uint vertexShader;
        private uint fragmentShader;
        private uint shaderProgram;

        /* Locations */
        private int cord3fLocation;
        private int col3fLocation;
        private int norm3fLocation;
        private int proj4fLocation;
        private int view4fLocation;
        private int model4fLocation;
        private int useSingleColorLocation;
        private int singleColor3fLocation;
        private int moveToCornerLocation;
        private int ka3fLocation;
        private int kd3fLocation;
        private int ks3fLocation;
        private int light3fLocation;
        private int ia3fLocation;
        private int il3fLocation;
        private int pLocation;

        /* Vertex buffer and array objects */
        private uint vbo = 0;
        private uint vio = 0;
        private uint vao = 0;
        private bool needToBufferData = false;
        private const int OFFSET_POS = 0;
        private const int OFFSET_COL = sizeof(float) * 3;
        private const int OFFSET_NORM = sizeof(float) * (3 + 3);
        private const int VERTEX_SIZE = sizeof(float) * (3 + 3 + 3);
        private int verticesLen = 0;
        private int bufferLen = 0;
        private int polygonCount = 0;
        private int trianglesLen = 0;

        /* OpenGL context */
        private OpenGL gl = null;
        private double width = 0;
        private double height = 0;
        private bool mouseMotionFlag = false;
        private Vector2D pointerPos;
        private const double ROTATION_SPEED = 0.5;

        Vector4D up = new Vector4D(0, 1, 0, 0);
        Vector4D cameraTarget = new Vector4D();
        Vector4D cameraPos = new Vector4D(0, 0, 0.05, 0);
        Vector4D cameraDirection = new Vector4D();
        Vector4D cameraRight = new Vector4D();
        Vector4D cameraUp = new Vector4D();

        Matrix4D projection = new Matrix4D();
        Matrix4D view = new Matrix4D();

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
        private Matrix4D modelFigure = null;

        /* NURBS parameters */
        [UI] private Adjustment _adjustmentP1X = null;
        [UI] private Adjustment _adjustmentP1Y = null;
        [UI] private Adjustment _adjustmentP1W = null;
        [UI] private Adjustment _adjustmentP2X = null;
        [UI] private Adjustment _adjustmentP2Y = null;
        [UI] private Adjustment _adjustmentP2W = null;
        [UI] private Adjustment _adjustmentP3X = null;
        [UI] private Adjustment _adjustmentP3Y = null;
        [UI] private Adjustment _adjustmentP3W = null;
        [UI] private Adjustment _adjustmentP4X = null;
        [UI] private Adjustment _adjustmentP4Y = null;
        [UI] private Adjustment _adjustmentP4W = null;
        [UI] private Adjustment _adjustmentP5X = null;
        [UI] private Adjustment _adjustmentP5Y = null;
        [UI] private Adjustment _adjustmentP5W = null;
        [UI] private Adjustment _adjustmentP6X = null;
        [UI] private Adjustment _adjustmentP6Y = null;
        [UI] private Adjustment _adjustmentP6W = null;
        private List<Vector4D> controlPoints = null;
        private List<double> weights = null;
        private const float POINT_SIZE = 8;

        /* Figure colour and parameters */
        [UI] private Adjustment _adjustmentFigureR = null;
        [UI] private Adjustment _adjustmentFigureG = null;
        [UI] private Adjustment _adjustmentFigureB = null;
        Misc.Colour figureColour = null;

        [UI] private Adjustment _adjustmentFrameR = null;
        [UI] private Adjustment _adjustmentFrameG = null;
        [UI] private Adjustment _adjustmentFrameB = null;
        Misc.Colour frameColour = null;

        [UI] private Adjustment _adjustmentHorizontalN = null;
        [UI] private Adjustment _adjustmentVerticalN = null;
        private int horizontalN = 0;
        private int verticalN = 0;
        private Figure fig = null;

        /* Drawing elements */
        [UI] private CheckButton _checkButtonFillPolygons = null;
        [UI] private CheckButton _checkButtonDrawFrame = null;
        [UI] private CheckButton _checkButtonDrawSource = null;
        private const float SOURCE_SIZE = 12;
        [UI] private CheckButton _checkButtonAutoScale = null;
        [UI] private CheckButton _checkButtonIgnoreInvisible = null;
        [UI] private CheckButton _checkButtonPolygonSmooth = null;
        [UI] private CheckButton _checkButtonDrawControlPoints = null;
        [UI] private CheckButton _checkButtonDrawControlLine = null;
        [UI] private CheckButton _checkButtonDrawAxises = null;
        private const float AXIS_SIZE = 0.1f;
        [UI] private RadioButton _radioButtonNoShading = null;
        [UI] private RadioButton _radioButtonSimpleShading = null;

        /* Material parameters */
        [UI] private Adjustment _adjustmentKaR = null;
        [UI] private Adjustment _adjustmentKaG = null;
        [UI] private Adjustment _adjustmentKaB = null;
        private Misc.Colour ka = null;

        [UI] private Adjustment _adjustmentKdR = null;
        [UI] private Adjustment _adjustmentKdG = null;
        [UI] private Adjustment _adjustmentKdB = null;
        private Misc.Colour kd = null;

        [UI] private Adjustment _adjustmentKsR = null;
        [UI] private Adjustment _adjustmentKsG = null;
        [UI] private Adjustment _adjustmentKsB = null;
        private Misc.Colour ks = null;

        [UI] private Adjustment _adjustmentP = null;
        private double p = 0;

        /* Light source parameters */
        [UI] private Adjustment _adjustmentLightX = null;
        [UI] private Adjustment _adjustmentLightY = null;
        [UI] private Adjustment _adjustmentLightZ = null;
        private Vector4D lightSource = new Vector4D();
        private Matrix4D modelLight = new Matrix4D();

        [UI] private Adjustment _adjustmentIaR = null;
        [UI] private Adjustment _adjustmentIaG = null;
        [UI] private Adjustment _adjustmentIaB = null;
        private Misc.Colour ia = null;

        [UI] private Adjustment _adjustmentIlR = null;
        [UI] private Adjustment _adjustmentIlG = null;
        [UI] private Adjustment _adjustmentIlB = null;
        private Misc.Colour il = null;

        /* Presets */
        [UI] private FileChooserButton _fileChooserButtonOpenPreset = null;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);
            DeleteEvent += Window_DeleteEvent;
            _GLArea.Realized += GLArea_Realized;
            _GLArea.Resize += GLArea_ResizeEvent;
            _fileChooserButtonOpenPreset.FileSet += PresetChoosenEvent;
            AddEventsToGLArea();
            AddValueChangedEventToMovement();
            AddValueChangedEventToNURBSParams();
            AddValueChangedEventToDrawingColours();
            AddValueChangedEventToFigureParams();
            AddToggledEventToDrawingParameters();
            AddValueChangedEventToMaterialParams();
            AddValueChangedEventToLightSource();
        }

        private void PresetChoosenEvent(object sender, EventArgs args)
        {
            ReadPreset(_fileChooserButtonOpenPreset.Filename);
        }

        private void ReadPreset(string filename)
        {
            string[] lines = System.IO.File.ReadAllLines(filename);
            float[] values = new float[3 * 6];
            for (int i = 0; i < 3 * 6; ++i)
            {
                values[i] = float.Parse(lines[i], CultureInfo.InvariantCulture);
            }
            _adjustmentP1X.Value = values[0 + 0];
            _adjustmentP1Y.Value = values[0 + 1];
            _adjustmentP1W.Value = values[0 + 2];

            _adjustmentP2X.Value = values[3 + 0];
            _adjustmentP2Y.Value = values[3 + 1];
            _adjustmentP2W.Value = values[3 + 2];

            _adjustmentP3X.Value = values[6 + 0];
            _adjustmentP3Y.Value = values[6 + 1];
            _adjustmentP3W.Value = values[6 + 2];

            _adjustmentP4X.Value = values[9 + 0];
            _adjustmentP4Y.Value = values[9 + 1];
            _adjustmentP4W.Value = values[9 + 2];

            _adjustmentP5X.Value = values[12 + 0];
            _adjustmentP5Y.Value = values[12 + 1];
            _adjustmentP5W.Value = values[12 + 2];

            _adjustmentP6X.Value = values[15 + 0];
            _adjustmentP6Y.Value = values[15 + 1];
            _adjustmentP6W.Value = values[15 + 2];
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs args)
        {
            Application.Quit();
        }

        private void GLArea_Realized(object sender, EventArgs args)
        {
            gl = new OpenGL();
            _GLArea.MakeCurrent();
            CompileShaders();
            GetLayoutLocations();
            gl.UseProgram(shaderProgram);

            /* Generating vertex objects */
            GenBuffers();
            gl.BindVertexArray(vao);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, vbo);
            gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, vio);
            needToBufferData = true;

            gl.EnableVertexAttribArray((uint)cord3fLocation);
            gl.VertexAttribPointer((uint)cord3fLocation, 3, OpenGL.GL_FLOAT, false, VERTEX_SIZE, (IntPtr)OFFSET_POS);
            gl.EnableVertexAttribArray((uint)col3fLocation);
            gl.VertexAttribPointer((uint)col3fLocation, 3, OpenGL.GL_FLOAT, false, VERTEX_SIZE, (IntPtr)OFFSET_COL);
            gl.EnableVertexAttribArray((uint)norm3fLocation);
            gl.VertexAttribPointer((uint)norm3fLocation, 3, OpenGL.GL_FLOAT, false, VERTEX_SIZE, (IntPtr)OFFSET_NORM);

            SetRenderParametes();
            _GLArea.Render += GLArea_Render;
            _GLArea.Unrealized += GLArea_Unrealized;
        }

        private void GLArea_Render(object sender, EventArgs args)
        {
            GetAllData();
            if (needToBufferData)
            {
                BufferFigureData();
                needToBufferData = false;
            }
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            if (_checkButtonIgnoreInvisible.Active)
            {
                gl.Enable(OpenGL.GL_DEPTH_TEST);
            }
            else
            {
                gl.Disable(OpenGL.GL_DEPTH_TEST);
            }
            if (_checkButtonPolygonSmooth.Active)
            {
                gl.Enable(OpenGL.GL_POLYGON_SMOOTH);
                gl.Hint(OpenGL.GL_POLYGON_SMOOTH_HINT, OpenGL.GL_NICEST);
            }
            else
            {
                gl.Disable(OpenGL.GL_POLYGON_SMOOTH);
            }
            if (_checkButtonFillPolygons.Active)
            {
                if (_radioButtonSimpleShading.Active)
                {
                    gl.Uniform1(useSingleColorLocation, 0);
                }
                else
                {
                    gl.Uniform1(useSingleColorLocation, 1);
                }
                gl.Uniform3(singleColor3fLocation, 1, figureColour.ToFloatArray());
                gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                gl.CullFace(OpenGL.GL_BACK);
                gl.DrawElements(OpenGL.GL_TRIANGLES, trianglesLen, OpenGL.GL_UNSIGNED_INT, (IntPtr)0);
            }
            gl.Uniform1(useSingleColorLocation, 1);
            if (_checkButtonDrawFrame.Active)
            {
                gl.LineWidth(1);
                gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
                gl.Uniform3(singleColor3fLocation, 1, frameColour.ToFloatArray());
                if (!_checkButtonIgnoreInvisible.Active)
                {
                    gl.CullFace(OpenGL.GL_FRONT);
                    gl.DrawElements(OpenGL.GL_TRIANGLES, trianglesLen, OpenGL.GL_UNSIGNED_INT, (IntPtr)0);
                }
                gl.CullFace(OpenGL.GL_BACK);
                gl.DrawElements(OpenGL.GL_TRIANGLES, trianglesLen, OpenGL.GL_UNSIGNED_INT, (IntPtr)0);
            }
            if (_checkButtonDrawControlLine.Active)
            {
                gl.Uniform3(singleColor3fLocation, 1, new float[3] {0, 1, 0});
                gl.LineWidth(2);
                gl.DrawElements(OpenGL.GL_LINE_STRIP, 6, OpenGL.GL_UNSIGNED_INT, (IntPtr)((trianglesLen + 0) * sizeof(int)));
            }
            if (_checkButtonDrawControlPoints.Active)
            {
                gl.Uniform3(singleColor3fLocation, 1, new float[3] {1, 0, 0});
                gl.PointSize(POINT_SIZE);
                gl.DrawElements(OpenGL.GL_POINTS, 6, OpenGL.GL_UNSIGNED_INT, (IntPtr)((trianglesLen + 0) * sizeof(int)));
            }
            if (_checkButtonDrawSource.Active)
            {
                gl.UniformMatrix4(model4fLocation, 1, true, modelLight.ToFloatArray());
                gl.Uniform3(singleColor3fLocation, 1, il.ToFloatArray());
                gl.PointSize(SOURCE_SIZE);
                gl.DrawElements(OpenGL.GL_POINTS, 1, OpenGL.GL_UNSIGNED_INT, (IntPtr)((trianglesLen + 6) * sizeof(int)));
            }

            if (_checkButtonDrawAxises.Active)
            {
                gl.UniformMatrix4(model4fLocation, 1, true, (new Matrix4D()).ToFloatArray());
                gl.Uniform1(moveToCornerLocation, 1);
                gl.LineWidth(3);
                gl.Uniform3(singleColor3fLocation, 1, new float[3] {1, 0, 0});
                gl.DrawElements(OpenGL.GL_LINES, 2, OpenGL.GL_UNSIGNED_INT, (IntPtr)((trianglesLen + 7 + 0) * sizeof(int)));
                gl.Uniform3(singleColor3fLocation, 1, new float[3] {0, 1, 0});
                gl.DrawElements(OpenGL.GL_LINES, 2, OpenGL.GL_UNSIGNED_INT, (IntPtr)((trianglesLen + 7 + 2) * sizeof(int)));
                gl.Uniform3(singleColor3fLocation, 1, new float[3] {0, 0, 1});
                gl.DrawElements(OpenGL.GL_LINES, 2, OpenGL.GL_UNSIGNED_INT, (IntPtr)((trianglesLen + 7 + 4) * sizeof(int)));
            }
        }

        private void GLArea_Unrealized(object sender, EventArgs args)
        {
            /* Deleting buffers */
            gl.BindVertexArray(0);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, 0);
            gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, 0);
            gl.DeleteBuffers(1,      new uint[1] {vbo});
            gl.DeleteBuffers(1,      new uint[1] {vio});
            gl.DeleteVertexArrays(1, new uint[1] {vao});
            /* Deleting shaders */
            gl.UseProgram(0);
            gl.DeleteProgram(shaderProgram);
            gl.DeleteShader(vertexShader);
            gl.DeleteShader(fragmentShader);
        }

        private void GLArea_ResizeEvent(object sender, EventArgs args)
        {
            width = _GLArea.Allocation.Width;
            height = _GLArea.Allocation.Height;
            _GLArea.QueueRender();
        }

        private void CompileShaders()
        {
            int[] success = new int[1];
            System.Text.StringBuilder txt = new System.Text.StringBuilder(512);

            vertexShader = gl.CreateShader(OpenGL.GL_VERTEX_SHADER);
            gl.ShaderSource(vertexShader, HelpUtils.ReadFromRes("Phong.vert"));
            gl.CompileShader(vertexShader);

            gl.GetShader(vertexShader, OpenGL.GL_COMPILE_STATUS, success);
            if (success[0] == 0)
            {
                gl.GetShaderInfoLog(vertexShader, 512, (IntPtr)0, txt);
                Console.WriteLine("Vertex shader compilation failed!\n" + txt);
            }

            fragmentShader = gl.CreateShader(OpenGL.GL_FRAGMENT_SHADER);
            gl.ShaderSource(fragmentShader, HelpUtils.ReadFromRes("Phong.frag"));
            gl.CompileShader(fragmentShader);

            gl.GetShader(fragmentShader, OpenGL.GL_COMPILE_STATUS, success);
            if (success[0] == 0)
            {
                gl.GetShaderInfoLog(fragmentShader, 512, (IntPtr)0, txt);
                Console.WriteLine("Fragment shader compilation failed!\n" + txt);
            }

            shaderProgram = gl.CreateProgram();
            gl.AttachShader(shaderProgram, vertexShader);
            gl.AttachShader(shaderProgram, fragmentShader);
            gl.LinkProgram(shaderProgram);

            gl.GetProgram(vertexShader, OpenGL.GL_LINK_STATUS, success);
            if (success[0] == 0)
            {
                gl.GetProgramInfoLog(vertexShader, 512, (IntPtr)0, txt);
                Console.WriteLine("Shader program linking failed!\n" + txt);
            }
        }

        private void GetLayoutLocations()
        {
            cord3fLocation         = gl.GetAttribLocation(shaderProgram, "cord3f");
            col3fLocation          = gl.GetAttribLocation(shaderProgram, "col3f");
            norm3fLocation         = gl.GetAttribLocation(shaderProgram, "norm3f");

            proj4fLocation         = gl.GetUniformLocation(shaderProgram, "proj4f");
            view4fLocation         = gl.GetUniformLocation(shaderProgram, "view4f");
            model4fLocation        = gl.GetUniformLocation(shaderProgram, "model4f");
            useSingleColorLocation = gl.GetUniformLocation(shaderProgram, "useSingleColor");
            singleColor3fLocation  = gl.GetUniformLocation(shaderProgram, "singleColor3f");
            moveToCornerLocation   = gl.GetUniformLocation(shaderProgram, "moveToCorner");
            ka3fLocation           = gl.GetUniformLocation(shaderProgram, "ka3f");
            kd3fLocation           = gl.GetUniformLocation(shaderProgram, "kd3f");
            ks3fLocation           = gl.GetUniformLocation(shaderProgram, "ks3f");
            light3fLocation        = gl.GetUniformLocation(shaderProgram, "light3f");
            ia3fLocation           = gl.GetUniformLocation(shaderProgram, "ia3f");
            il3fLocation           = gl.GetUniformLocation(shaderProgram, "il3f");
            pLocation              = gl.GetUniformLocation(shaderProgram, "p");
        }

        private void GenBuffers()
        {
            fixed (uint * ptr = &vbo)
            {
                gl.GenBuffers(1, (IntPtr)ptr);
            }
            fixed (uint * ptr = &vio)
            {
                gl.GenBuffers(1, (IntPtr)ptr);
            }
            fixed (uint * ptr = &vao)
            {
                gl.GenVertexArrays(1, (IntPtr)ptr);
            }
        }

        private void WriteControlPoints(float * ptr, int offset)
        {
            int i = 0;
            foreach (Vector4D p in controlPoints)
            {
                ptr[offset + i + 0] = (1 + (float)p.X) / 2;
                ptr[offset + i + 1] = 0;
                ptr[offset + i + 2] = (float)p.Y;
                i = i + 9;
            }
        }

        private void WriteAxises(float * ptr, int offset)
        {
            int i = 0;
            for (int j = 0; j < 4; ++j)
            {
                switch (j)
                {
                    case 1:
                        ptr[offset + i + 0] += AXIS_SIZE;
                        break;
                    case 2:
                        ptr[offset + i + 1] += AXIS_SIZE;
                        break;
                    case 3:
                        ptr[offset + i + 2] += AXIS_SIZE;
                        break;
                }
                i = i + 9;
            }
        }

        private void WriteAxisesIndices(int * ptr, int offsetInd, int offsetVertex)
        {
            ptr[offsetInd + 0] = offsetVertex + 0;
            ptr[offsetInd + 1] = offsetVertex + 1;
            ptr[offsetInd + 2] = offsetVertex + 0;
            ptr[offsetInd + 3] = offsetVertex + 2;
            ptr[offsetInd + 4] = offsetVertex + 0;
            ptr[offsetInd + 5] = offsetVertex + 3;
        }

        private void BufferFigureData()
        {
            fig = new Figure(horizontalN, verticalN, controlPoints, weights, new Misc.Colour(0, 0, 0));
            verticesLen = fig.Vertices.Count;
            bufferLen = verticesLen * VERTEX_SIZE;
            float[] verticesBuffer = new float[bufferLen + 11 * VERTEX_SIZE];
            fixed (float * ptr = &verticesBuffer[0])
            {
                fig.WriteVertexData(ptr);
                WriteControlPoints(ptr, verticesLen * 9);
                WriteAxises(ptr, (verticesLen + 7) * 9);
                gl.BufferData(OpenGL.GL_ARRAY_BUFFER, sizeof(float) * (bufferLen + 11 * VERTEX_SIZE), (IntPtr)ptr, OpenGL.GL_DYNAMIC_DRAW);
            }
            polygonCount = fig.Polygons.Count;
            trianglesLen = 3 * polygonCount;
            int[] indices = new int[trianglesLen + 13];
            fixed (int * ptr = &indices[0])
            {
                fig.WriteIndeces(ptr);
                for (int i = 0; i < 7; ++i)
                {
                    ptr[trianglesLen + i] = verticesLen + i;
                }
                WriteAxisesIndices(ptr, trianglesLen + 7, verticesLen + 7);
                gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER, sizeof(int) * (trianglesLen + 13), (IntPtr)ptr, OpenGL.GL_DYNAMIC_DRAW);
            }
        }

        private void SetRenderParametes()
        {
            gl.FrontFace(OpenGL.GL_CW);

            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.DepthFunc(OpenGL.GL_LESS);

            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);

            gl.Enable(OpenGL.GL_POINT_SMOOTH);
            gl.Hint(OpenGL.GL_POINT_SMOOTH_HINT, OpenGL.GL_NICEST);

            gl.Enable(OpenGL.GL_LINE_SMOOTH);
            gl.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);

            gl.Enable(OpenGL.GL_POLYGON_SMOOTH);
            gl.Hint(OpenGL.GL_POLYGON_SMOOTH_HINT, OpenGL.GL_NICEST);

            gl.ClearColor(0, 0, 0, 0);
        }

        private void AddEventsToGLArea()
        {
            _GLArea.Events |= EventMask.ButtonPressMask;
            _GLArea.ButtonPressEvent += GLArea_ButtonPressEvent;
            _GLArea.Events |= EventMask.PointerMotionMask;
            _GLArea.MotionNotifyEvent += GLArea_MotionNotifyEvent;
            _GLArea.Events |= EventMask.ButtonReleaseMask;
            _GLArea.ButtonReleaseEvent += GLArea_ButtonReleaseEvent;
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

        private void AddValueChangedEventToNURBSParams()
        {
            _adjustmentP1X.ValueChanged += GenerateFigureAndDraw;
            _adjustmentP1Y.ValueChanged += GenerateFigureAndDraw;
            _adjustmentP1W.ValueChanged += GenerateFigureAndDraw;

            _adjustmentP2X.ValueChanged += GenerateFigureAndDraw;
            _adjustmentP2Y.ValueChanged += GenerateFigureAndDraw;
            _adjustmentP2W.ValueChanged += GenerateFigureAndDraw;

            _adjustmentP3X.ValueChanged += GenerateFigureAndDraw;
            _adjustmentP3Y.ValueChanged += GenerateFigureAndDraw;
            _adjustmentP3W.ValueChanged += GenerateFigureAndDraw;

            _adjustmentP4X.ValueChanged += GenerateFigureAndDraw;
            _adjustmentP4Y.ValueChanged += GenerateFigureAndDraw;
            _adjustmentP4W.ValueChanged += GenerateFigureAndDraw;

            _adjustmentP5X.ValueChanged += GenerateFigureAndDraw;
            _adjustmentP5Y.ValueChanged += GenerateFigureAndDraw;
            _adjustmentP5W.ValueChanged += GenerateFigureAndDraw;

            _adjustmentP6X.ValueChanged += GenerateFigureAndDraw;
            _adjustmentP6Y.ValueChanged += GenerateFigureAndDraw;
            _adjustmentP6W.ValueChanged += GenerateFigureAndDraw;
        }

        private void AddValueChangedEventToDrawingColours()
        {
            _adjustmentFigureR.ValueChanged += Redraw;
            _adjustmentFigureG.ValueChanged += Redraw;
            _adjustmentFigureB.ValueChanged += Redraw;

            _adjustmentFrameR.ValueChanged += Redraw;
            _adjustmentFrameG.ValueChanged += Redraw;
            _adjustmentFrameB.ValueChanged += Redraw;
        }

        private void AddValueChangedEventToFigureParams()
        {
            _adjustmentHorizontalN.ValueChanged += GenerateFigureAndDraw;
            _adjustmentVerticalN.ValueChanged += GenerateFigureAndDraw;
        }

        private void AddToggledEventToDrawingParameters()
        {
            _checkButtonFillPolygons.Toggled += Redraw;
            _checkButtonDrawFrame.Toggled += Redraw;
            _checkButtonDrawSource.Toggled += Redraw;
            _checkButtonAutoScale.Toggled += Redraw;
            _checkButtonIgnoreInvisible.Toggled += Redraw;
            _checkButtonPolygonSmooth.Toggled += Redraw;
            _checkButtonDrawControlPoints.Toggled += Redraw;
            _checkButtonDrawControlLine.Toggled += Redraw;
            _checkButtonDrawAxises.Toggled += Redraw;
            _radioButtonNoShading.Toggled += Redraw;
            _radioButtonSimpleShading.Toggled += Redraw;
        }

        private void AddValueChangedEventToMaterialParams()
        {
            _adjustmentKaR.ValueChanged += Redraw;
            _adjustmentKaG.ValueChanged += Redraw;
            _adjustmentKaB.ValueChanged += Redraw;
            _adjustmentKdR.ValueChanged += Redraw;
            _adjustmentKdG.ValueChanged += Redraw;
            _adjustmentKdB.ValueChanged += Redraw;
            _adjustmentKsR.ValueChanged += Redraw;
            _adjustmentKsG.ValueChanged += Redraw;
            _adjustmentKsB.ValueChanged += Redraw;
            _adjustmentP.ValueChanged += Redraw;
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
        }

        private void GenMatrix()
        {
            Matrix4D translation = Matrix4D.Shift(shiftX, shiftY, shiftZ);
            Matrix4D rotation = Matrix4D.RotX(alpha) * Matrix4D.RotY(beta) * Matrix4D.RotZ(gamma);
            Matrix4D scale = Matrix4D.Scale(scaleX, scaleY, scaleZ);
            modelFigure = translation * rotation * scale;
            modelLight = Matrix4D.Shift(lightSource.X, lightSource.Y, lightSource.Z);
            projection = new Matrix4D();
            if (_checkButtonAutoScale.Active)
            {
                projection[0, 0] = Math.Min(height / width, 1);
                projection[1, 1] = Math.Min(width / height, 1);
            }
            gl.UniformMatrix4(proj4fLocation, 1, true, projection.ToFloatArray());
            gl.UniformMatrix4(view4fLocation, 1, true, view.ToFloatArray());
            gl.UniformMatrix4(model4fLocation, 1, true, modelFigure.ToFloatArray());
        }

        private void GetAllData()
        {
            GetMovementData();
            GetNURBSData();
            GetFigureData();
            GetMaterialData();
            GetLightSourceData();
            GenMatrix();

            gl.Uniform3(ka3fLocation, 1, ka.ToFloatArray());
            gl.Uniform3(kd3fLocation, 1, kd.ToFloatArray());
            gl.Uniform3(ks3fLocation, 1, ks.ToFloatArray());
            gl.Uniform1(pLocation, (float)p);

            Vector4D l = new Vector4D(0, 0, 0, 1);
            l = projection * view * modelLight * l;
            gl.Uniform3(light3fLocation, 1, l.ToFloatArray());
            gl.Uniform3(ia3fLocation, 1, ia.ToFloatArray());
            gl.Uniform3(il3fLocation, 1, il.ToFloatArray());
            gl.Uniform1(moveToCornerLocation, 0);
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

        private void GetNURBSData()
        {
            controlPoints = new List<Vector4D>(6);
            controlPoints.Add(new Vector4D(_adjustmentP1X.Value, _adjustmentP1Y.Value, 0, 0));
            controlPoints.Add(new Vector4D(_adjustmentP2X.Value, _adjustmentP2Y.Value, 0, 0));
            controlPoints.Add(new Vector4D(_adjustmentP3X.Value, _adjustmentP3Y.Value, 0, 0));
            controlPoints.Add(new Vector4D(_adjustmentP4X.Value, _adjustmentP4Y.Value, 0, 0));
            controlPoints.Add(new Vector4D(_adjustmentP5X.Value, _adjustmentP5Y.Value, 0, 0));
            controlPoints.Add(new Vector4D(_adjustmentP6X.Value, _adjustmentP6Y.Value, 0, 0));
            weights = new List<double>(6);
            weights.Add(_adjustmentP1W.Value);
            weights.Add(_adjustmentP2W.Value);
            weights.Add(_adjustmentP3W.Value);
            weights.Add(_adjustmentP4W.Value);
            weights.Add(_adjustmentP5W.Value);
            weights.Add(_adjustmentP6W.Value);
        }

        private void GetFigureData()
        {
            figureColour = new Misc.Colour(_adjustmentFigureR.Value, _adjustmentFigureG.Value, _adjustmentFigureB.Value) / Misc.MAX_RGB;
            frameColour = new Misc.Colour(_adjustmentFrameR.Value, _adjustmentFrameG.Value, _adjustmentFrameB.Value) / Misc.MAX_RGB;
            horizontalN = (int)_adjustmentHorizontalN.Value;
            verticalN = (int)_adjustmentVerticalN.Value;
        }

        private void GetMaterialData()
        {
            ka = new Misc.Colour(_adjustmentKaR.Value, _adjustmentKaG.Value, _adjustmentKaB.Value);
            kd = new Misc.Colour(_adjustmentKdR.Value, _adjustmentKdG.Value, _adjustmentKdB.Value);
            ks = new Misc.Colour(_adjustmentKsR.Value, _adjustmentKsG.Value, _adjustmentKsB.Value);
            p = _adjustmentP.Value;
        }

        private void GetLightSourceData()
        {
            lightSource = new Vector4D(_adjustmentLightX.Value, _adjustmentLightY.Value, _adjustmentLightZ.Value, 1);
            ia = new Misc.Colour(_adjustmentIaR.Value, _adjustmentIaG.Value, _adjustmentIaB.Value);
            il = new Misc.Colour(_adjustmentIlR.Value, _adjustmentIlG.Value, _adjustmentIlB.Value);
        }

        private void AdjustmentAlpha_ValueChangedEvent(object sender, EventArgs args)
        {
            _adjustmentAlpha.Value = Misc.NormalizeAngle(_adjustmentAlpha.Value);
            _GLArea.QueueRender();
        }

        private void AdjustmentBeta_ValueChangedEvent(object sender, EventArgs args)
        {
            _adjustmentBeta.Value = Misc.NormalizeAngle(_adjustmentBeta.Value);
            _GLArea.QueueRender();
        }

        private void AdjustmentGamma_ValueChangedEvent(object sender, EventArgs args)
        {
            _adjustmentGamma.Value = Misc.NormalizeAngle(_adjustmentGamma.Value);
            _GLArea.QueueRender();
        }

        private void Redraw(object sender, EventArgs args)
        {
            _GLArea.QueueRender();
        }

        private void GenerateFigureAndDraw(object sender, EventArgs args)
        {
            needToBufferData = true;
            _GLArea.QueueRender();
        }

        private void GLArea_ButtonPressEvent(object sender, ButtonPressEventArgs args)
        {
            mouseMotionFlag = true;
            pointerPos = new Vector2D(args.Event.X, args.Event.Y);
        }

        private void GLArea_MotionNotifyEvent(object sender, MotionNotifyEventArgs args)
        {
            if (mouseMotionFlag)
            {
                Vector2D pointerPosNext = new Vector2D(args.Event.X, args.Event.Y);
                Vector2D delta = ROTATION_SPEED * (pointerPosNext - pointerPos);
                double rotX = Misc.ToRadians(delta.Y);
                double rotY = Misc.ToRadians(delta.X);
                Matrix4D rot = Matrix4D.RotFromAxisAngle(cameraUp, rotY) * Matrix4D.RotFromAxisAngle(cameraRight, rotX);
                UpdateCameraRotation(rot);
                pointerPos = pointerPosNext;
                _GLArea.QueueRender();
            }
        }

        private void GLArea_ButtonReleaseEvent(object sender, ButtonReleaseEventArgs args)
        {
            mouseMotionFlag = false;
        }

        private void UpdateCameraRotation(Matrix4D rot)
        {
            cameraPos = rot * cameraPos;
            cameraDirection = cameraPos - cameraTarget;
            up = rot * up;
            cameraRight = Vector4D.Cross(up, cameraDirection);
            cameraUp = Vector4D.Cross(cameraDirection, cameraRight);
            cameraDirection.Normalize();
            cameraRight.Normalize();
            cameraUp.Normalize();

            Matrix4D res = new Matrix4D();
            res[0, 0] = cameraRight.X;
            res[0, 1] = cameraRight.Y;
            res[0, 2] = cameraRight.Z;
            res[1, 0] = cameraUp.X;
            res[1, 1] = cameraUp.Y;
            res[1, 2] = cameraUp.Z;
            res[2, 0] = cameraDirection.X;
            res[2, 1] = cameraDirection.Y;
            res[2, 2] = cameraDirection.Z;

            Matrix4D invCameraPos = new Matrix4D();
            invCameraPos[0, 3] = -cameraPos.X;
            invCameraPos[1, 3] = -cameraPos.Y;
            invCameraPos[2, 3] = -cameraPos.Z;
            view = res * invCameraPos;
        }
    }
}
