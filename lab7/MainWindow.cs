using CGPlatform;
using Gdk;
using Gtk;
using SharpGL;
using System;
using UI = Gtk.Builder.ObjectAttribute;

namespace lab7
{
    unsafe class MainWindow : Gtk.Window
    {
        [UI] private GLArea _GLArea = null;
        private FrameClock _frameClock = null;

        /* Shaders */
        private uint vertexShader;
        private uint fragmentShader;
        private uint shaderProgram;

        /* Locations */
        private int cord3fLocation;
        private int color3fLocation;

        /* Vertex buffer and array objects */
        private uint vbo = 0;
        private uint vio = 0;
        private uint vao = 0;
        private const int OFFSET_POS = 0;
        private const int VERTEX_SIZE = 3 * sizeof(float);
        private int pointsLen = 0;
        private int curveLen = 0;
        private int bufferLen = 0;

        /* OpenGL context */
        private OpenGL gl = null;
        private double width = 0;
        private double height = 0;

        /* Options */
        [UI] CheckButton _checkButtonDrawPoints = null;
        [UI] CheckButton _checkButtonDrawLines = null;

        /* Curve */
        private Curve BSpline = null;
        private int pointId = -1;
        private bool pointChanged = false;
        private const float SOURCE_SIZE = 10;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);
            DeleteEvent += Window_DeleteEvent;
            _GLArea.Realized += GLArea_Realized;
            _GLArea.Render += GLArea_Render;
            _GLArea.Unrealized += GLArea_Unrealized;
            _GLArea.Resize += GLArea_ResizeEvent;
            AddEventsToGLArea();
            BSpline = new Curve();
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
            _frameClock = _GLArea.Context.Window.FrameClock;
            _frameClock.Update += EnqueueRender;
            _frameClock.BeginUpdating();

            /* Generating vertex objects */
            GenBuffers();
            gl.BindVertexArray(vao);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, vbo);
            gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, vio);

            gl.EnableVertexAttribArray((uint)cord3fLocation);
            gl.VertexAttribPointer((uint)cord3fLocation, 3, OpenGL.GL_FLOAT, false, VERTEX_SIZE, (IntPtr)OFFSET_POS);

            SetRenderParametes();
        }

        private void GLArea_Render(object sender, EventArgs args)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.ClearColor(0.25f, 0.25f, 0.25f, 0);

            BufferData();

            if (_checkButtonDrawPoints.Active)
            {
                gl.Uniform3(color3fLocation, 1, new float[3] {1, 0, 0});
                gl.PointSize(SOURCE_SIZE);
                gl.DrawElements(OpenGL.GL_POINTS, pointsLen, OpenGL.GL_UNSIGNED_INT, (IntPtr)0);
            }

            if (_checkButtonDrawLines.Active)
            {
                gl.Uniform3(color3fLocation, 1, new float[3] {1, 1, 1});
                gl.LineWidth(1);
                gl.DrawElements(OpenGL.GL_LINE_STRIP, pointsLen, OpenGL.GL_UNSIGNED_INT, (IntPtr)0);
            }

            gl.Uniform3(color3fLocation, 1, new float[3] {0, 1, 0});
            gl.LineWidth(3);
            gl.DrawElements(OpenGL.GL_LINE_STRIP, curveLen, OpenGL.GL_UNSIGNED_INT, (IntPtr)(pointsLen * sizeof(int)));
        }

        private void BufferData()
        {
            pointsLen = BSpline.CountPoints;
            curveLen = BSpline.CountData;
            bufferLen = (pointsLen + curveLen) * VERTEX_SIZE;
            float[] verticesBuffer = new float[bufferLen];
            fixed (float * ptr = &verticesBuffer[0])
            {
                BSpline.WritePointsData(ptr);
                gl.BufferData(OpenGL.GL_ARRAY_BUFFER, sizeof(float) * (bufferLen), (IntPtr)ptr, OpenGL.GL_DYNAMIC_DRAW);
            }
            int[] indices = new int[bufferLen];
            fixed (int * ptr = &indices[0])
            {
                BSpline.WritePointsIndeces(ptr);
                gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER, sizeof(int) * (bufferLen), (IntPtr)ptr, OpenGL.GL_DYNAMIC_DRAW);
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

        private void CompileShaders()
        {
            int[] success = new int[1];
            System.Text.StringBuilder txt = new System.Text.StringBuilder(512);

            vertexShader = gl.CreateShader(OpenGL.GL_VERTEX_SHADER);
            gl.ShaderSource(vertexShader, HelpUtils.ReadFromRes("SimpleVertex.vert"));
            gl.CompileShader(vertexShader);

            gl.GetShader(vertexShader, OpenGL.GL_COMPILE_STATUS, success);
            if (success[0] == 0)
            {
                gl.GetShaderInfoLog(vertexShader, 512, (IntPtr)0, txt);
                Console.WriteLine("Vertex shader compilation failed!\n" + txt);
            }

            fragmentShader = gl.CreateShader(OpenGL.GL_FRAGMENT_SHADER);
            gl.ShaderSource(fragmentShader, HelpUtils.ReadFromRes("SimpleFragment.frag"));
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
            cord3fLocation  = gl.GetAttribLocation(shaderProgram, "cord3f");
            color3fLocation = gl.GetUniformLocation(shaderProgram, "color3f");
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

        private void SetRenderParametes()
        {
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.DepthFunc(OpenGL.GL_LESS);

            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);

            gl.Enable(OpenGL.GL_POINT_SMOOTH);
            gl.Hint(OpenGL.GL_POINT_SMOOTH_HINT, OpenGL.GL_NICEST);

            gl.Enable(OpenGL.GL_LINE_SMOOTH);
            gl.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);
        }

        private void EnqueueRender(object sender, EventArgs args)
        {
            _GLArea.QueueRender();
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

        private Vector2D GetMousePos(double mouseX, double mouseY)
        {
            return new Vector2D(2 * mouseX / width - 1, 2 * (height - mouseY) / height - 1);
        }

        private void GLArea_ButtonPressEvent(object sender, ButtonPressEventArgs args)
        {
            if (_checkButtonDrawPoints.Active)
            {
                Vector2D mouse = GetMousePos(args.Event.X, args.Event.Y);
                pointId = BSpline.FindPoint(mouse);
                if (pointId == -1)
                {
                    BSpline.AddPoint(mouse);
                }
            }
        }

        private void GLArea_MotionNotifyEvent(object sender, MotionNotifyEventArgs args)
        {
            Vector2D mouse2D = GetMousePos(args.Event.X, args.Event.Y);
            Vector4D mouseMove = new Vector4D(mouse2D.X, mouse2D.Y, 0, 0);
            if (pointId != -1)
            {
                BSpline.Points[pointId] = mouseMove;
                BSpline.CalcCurve();
                pointChanged = true;
            }
        }

        private void GLArea_ButtonReleaseEvent(object sender, ButtonReleaseEventArgs args)
        {
            if (pointId != -1)
            {
                if (!pointChanged)
                {
                    BSpline.RemovePoint(pointId);
                }
                pointChanged = false;
                pointId = -1;
            }
        }

        private void GLArea_ResizeEvent(object sender, EventArgs args)
        {
            width = _GLArea.Allocation.Width;
            height = _GLArea.Allocation.Height;
        }
    }
}
