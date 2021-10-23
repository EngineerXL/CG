// Режим совместимости для работы на рабочем столе с глубиной цвета отличной от 32 бит.
// Если используется 32 битный рабочий стол то можно отключить - будет чуть быстрее работать.
#define COMPATIBLE_MODE

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Gtk;
using Gdk;
using Cairo;
// using CGPlatform;
using CairoHelper = Gdk.CairoHelper;

namespace lab3
{
    #region GTK Pixel Buffer Implementation

    public unsafe class CairoSurface : DrawingSurface
    {
        public override int Width {
            get { return _width; }
        }

        public override int Height {
            get { return _height; }
        }
        protected override  Clipper Clip {
            get { return _clipper; }
        }

        public override void* ImageBits {
            get { return _imgptr; }
        }

        private readonly DrawingArea _widget;
        private int _winpos_x1, _winpos_y1, _width, _height;
        private Clipper _clipper;
        private Pixbuf _pixbuf;
        private Context cr;
        private Widget _window;
        private void* _imgptr;

        #if COMPATIBLE_MODE
        private int _prev_width, _prev_height;
        #endif

        public CairoSurface(DrawingArea widget)
        {
            _imgptr = null;
            _widget = widget;
            _clipper = null;
            _prev_width = _prev_height = -1;
            _widget.SizeAllocated += (o, args) => {
                _width  = args.Allocation.Width;
                _height = args.Allocation.Height;
                _clipper = new Clipper(this);
                #if COMPATIBLE_MODE
                if (0 != ((_width ^ _prev_width) | (_height ^ _prev_height))) {
                    _prev_width  = _width;
                    _prev_height = _height;
                    if (_imgptr != null)
                        Marshal.FreeHGlobal((IntPtr)_imgptr);
                    _imgptr = (void*)Marshal.AllocHGlobal((_width * _height << 2) + 1);
                }
                #endif
            };
            _window = _widget;
            while (_window.Parent != null)
                _window = _window.Parent;
        }

        public void BeginUpdate(Context cr)
        {
            if (this.cr != null)
                throw new Exception("BeginUpdate() нельзя вызывать повторно до вызова EndUpdate()");

            var target = (this.cr = cr).GetTarget();
            _widget.TranslateCoordinates(_window, 0, 0, out _winpos_x1, out _winpos_y1);
            _pixbuf = new Pixbuf(target, _winpos_x1, _winpos_y1, _width, _height);
            #if !COMPATIBLE_MODE
            _imgptr = (void*)_pixbuf.Pixels;
            #else
            if (_pixbuf.BitsPerSample != 8)
                throw new NotImplementedException("Unsupported BitsPerSample");
            PixelCast pxcast;
            switch (_pixbuf.NChannels) {
                case 4: pxcast = PixelCast.XRGB8888_XRGB8888; break;
                case 3: pxcast = PixelCast.XRGB0888_XRGB8888; break;
                default: throw new NotImplementedException("Unsupported NChannels");
            }
            BitBlt(pxcast, (void*)_pixbuf.Pixels, _imgptr, _pixbuf.Rowstride,_width << 2, _width, _height);
            #endif
            target.Dispose();
        }

        public void EndUpdate()
        {
            if (cr == null)
                throw new Exception("EndUpdate() может быть вызван только после BeginUpdate()");

            PixelCast pxcast;
            switch (_pixbuf.NChannels) {
                case 4: pxcast = PixelCast.XRGB8888_XRGB8888; break;
                case 3: pxcast = PixelCast.XRGB8888_XRGB0888; break;
                default: throw new NotImplementedException("Unsupported NChannels");
            }
            BitBlt(pxcast, _imgptr, (void*)_pixbuf.Pixels, _width << 2, _pixbuf.Rowstride, _width, _height);

            CairoHelper.SetSourcePixbuf(cr, _pixbuf, 0, 0);
            cr.Paint();
            cr = null;
            _pixbuf.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetRgb(Misc.Colour color)
        { unchecked {     // color = []{ r, g, b } = [] { color[0], color[1], color[2] }
            int col, argb = (int)0xFF000000;
            for (int r, i = 3; 0 != i--;) {
                col = (int) (color[i] * 255);
                r = 0xFF + ((col - 0xFF) & ((col - 0xFF) >> 31));
                col = -((-r) & ((-r) >> 31));
                argb |= col << (i << 3); // abgr  ??! Cairo использует BGR или это Motorola ??!
              //argb |= col << (16 - (i << 3)); // argb
            }
            return argb;
        } }

        private enum PixelCast {
            XRGB8888_XRGB8888,
            XRGB8888_XRGB0888,
            XRGB0888_XRGB8888
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void BitBlt(PixelCast pxcast, void* src, void* dst, int src_stride, int dst_stride, int width, int heigh)
        { unchecked {
            var psrc = (byte*)src;
            var pdst = (byte*)dst;

            int src_pxlen, dst_pxlen;
            switch ((int)pxcast) {
                case (int)PixelCast.XRGB8888_XRGB8888: src_pxlen =    dst_pxlen = 4; break;
                case (int)PixelCast.XRGB8888_XRGB0888: src_pxlen = 4; dst_pxlen = 3; break;
                case (int)PixelCast.XRGB0888_XRGB8888: src_pxlen = 3; dst_pxlen = 4; break;
                default: throw new NotImplementedException("Unsupported PixelCast");
            }

            int src_nextline = src_stride - width * src_pxlen;
            int dst_nextline = dst_stride - width * dst_pxlen;
            switch ((int) pxcast) {
                case (int)PixelCast.XRGB8888_XRGB8888:
                case (int)PixelCast.XRGB8888_XRGB0888:
                    for (; 0 != heigh--; psrc += src_nextline, pdst += dst_nextline)
                    for (int x = width; 0 != x--; psrc += src_pxlen, pdst += dst_pxlen)
                        *(int*)pdst = *(int*)psrc;
                    return;
                case (int)PixelCast.XRGB0888_XRGB8888:
                    for (; 0 != heigh--; psrc += src_nextline, pdst += dst_nextline)
                    for (int x = width; 0 != x--; psrc += src_pxlen, pdst += dst_pxlen)
                        *(int*)pdst = (int)0xFF000000 | *(int*)psrc;
                    return;
                default: throw new NotImplementedException("Unsupported PixelCast");
            }
        }}

        public void DrawTriangle(Misc.Colour color, Vector2D p1, Vector2D p2, Vector2D p3)
        {
            DrawTriangle(this, GetRgb(color), p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y);
        }

        public void DrawTriangle(Misc.Colour color1, Vector2D p1, Misc.Colour color2, Vector2D p2, Misc.Colour color3, Vector2D p3)
        {
            DrawTriangle(this, GetRgb(color1), p1.X, p1.Y, GetRgb(color2), p2.X, p2.Y, GetRgb(color3), p3.X, p3.Y);
        }

        public void DrawLine(Misc.Colour color, Vector2D p1, Vector2D p2)
        { unchecked {
            double x1 = p1.X, y1 = p1.Y, x2 = p2.X, y2 = p2.Y;
            if (!Clip2D(Clip, ref x1, ref y1, ref x2, ref y2))
                return;
            DrawLineHQ((int*)ImageBits, Width, GetRgb(color), x1, y1, x2, y2);
        }}

        public void DrawLineLQ(Misc.Colour color, Vector2D p1, Vector2D p2)
        { unchecked {
            double x1 = p1.X, y1 = p1.Y, x2 = p2.X, y2 = p2.Y;
            if (!Clip2D(Clip, ref x1, ref y1, ref x2, ref y2))
                return;
            DrawLineLQ((int*)ImageBits, Width, GetRgb(color),
                (int)(x1+0.5), (int)(y1+0.5), (int)(x2+0.5), (int)(y2+0.5));
        }}
    }

    #endregion

    public abstract unsafe class DrawingSurface
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
        protected abstract Clipper Clip { get; }
        public abstract void* ImageBits { get; }

        #region Триангуляция

        protected static unsafe void DrawTriangle(CairoSurface surface, int color, double x1, double y1, double x2, double y2, double x3, double y3)
        { unchecked {
            if (surface.Clip.PointsInBounds(x1, y1, x2, y2, x3, y3)) {
                DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x1, y1, x2, y2, x3, y3);
                return;
            }

            // NOTE: Отсутсвует реализация частных случаев триангуляции, когда две или три грани полностью отсекаются

            // Соотношение старых и новых координат после отсечения:
            //
            //                 (_x1,_y1)
            //                     O 1
            //                    / \
            //           (x0,y0) /   \ (x1,y1)
            //                --+-----+--
            //               \ /       \ /
            //        (x2,y2) +         + (x3,y3)
            //               / \       / \
            //            2 /   \     /   \ 3
            //   (_x2,_y2) O ----+---+---- O (_x3,_y3)
            //             (x4,y4)\ /(x5,y5)
            int trimed;
            double _x1 = x1, _y1 = y1, _x2 = x2, _y2 = y2, _x3 = x3, _y3 = y3;
            double  x0 = x1,  y0 = y1,  x4 = x2,  y4 = y2,  x5 = x3,  y5 = y3;
            if (!Clip2D(surface.Clip, ref x0, ref y0, ref x2, ref y2)) {
                if (!Clip2D(surface.Clip, ref x1, ref y1, ref x3, ref y3)) {
                    if (!Clip2D(surface.Clip, ref x4, ref y4, ref x5, ref y5)) {
                        return; // все грани не определены
                    } else {
                        return; // грани 1-2 и 1-3 не определены
                    }
                } else if (!Clip2D(surface.Clip, ref x4, ref y4, ref x5, ref y5)) {
                    return; // грани 1-2 и 2-3 не определены
                } else {
                    x0 = x1;  y0 = y1;  x2 = x4;  y2 = y4;
                    trimed = ((x3 == _x3 && y3 == _y3 && x5 == _x3 && y5 == _y3) ? 11 : 15);
                }
            } else if (!Clip2D(surface.Clip, ref x1, ref y1, ref x3, ref y3)) {
                if (!Clip2D(surface.Clip, ref x4, ref y4, ref x5, ref y5)) {
                    return; // грани 1-3 и 2-3 не определены
                } else {
                    x1 = x0;  y1 = y0;  x3 = x5;  y3 = y5;
                    trimed = ((x2 == _x2 && y2 == _y2 && x4 == _x2 && y4 == _y2) ? 13 : 15);
                }
            } else if (!Clip2D(surface.Clip, ref x4, ref y4, ref x5, ref y5)) {
                x4 = x2;  y4 = y2;  x5 = x3;  y5 = y3;
                trimed = ((x0 == _x1 && y0 == _y1 && x1 == _x1 && y1 == _y1) ? 14 : 15);
            } else
                trimed = ((x0 == _x1 && y0 == _y1 && x1 == _x1 && y1 == _y1) ? 0 : 1)
                       | ((x2 == _x2 && y2 == _y2 && x4 == _x2 && y4 == _y2) ? 0 : 2)
                       | ((x3 == _x3 && y3 == _y3 && x5 == _x3 && y5 == _y3) ? 0 : 4);

            switch (trimed) {
                case 11: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x3, y3, x1, y1, x4, y4);
                         return;
                case 13: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x2, y2, x0, y0, x5, y5);
                         return;
                case 14: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x1, y1, x2, y2, x3, y3);
                         return;
                case  1: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x2, y2, x3, y3, x0, y0);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x3, y3, x0, y0, x1, y1);
                         return;
                case  2: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x1, y1, x3, y3, x4, y4);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x1, y1, x4, y4, x2, y2);
                         return;
                case  4: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x1, y1, x2, y2, x3, y3);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x2, y2, x3, y3, x5, y5);
                         return;
                case  3: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x3, y3, x1, y1, x0, y0);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x3, y3, x0, y0, x2, y2);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x3, y3, x2, y2, x4, y4);
                         return;
                case  5: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x2, y2, x0, y0, x1, y1);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x2, y2, x1, y1, x3, y3);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x2, y2, x3, y3, x5, y5);
                        return;
                case  6: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x1, y1, x2, y2, x4, y4);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x1, y1, x4, y4, x5, y5);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x1, y1, x5, y5, x3, y3);
                         return;
                case 15:
                case  7: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x0, y0, x1, y1, x3, y3);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x0, y0, x2, y2, x4, y4);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x0, y0, x3, y3, x5, y5);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color, x0, y0, x4, y4, x5, y5);
                         return;
                default: throw new NotImplementedException();
            }
        }}

        protected static unsafe void DrawTriangle(CairoSurface surface, int color1, double x1, double y1, int color2, double x2, double y2, int color3, double x3, double y3)
        { unchecked {
            if (surface.Clip.PointsInBounds(x1, y1, x2, y2, x3, y3)) {
                DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color1, x1, y1, color2, x2, y2, color3, x3, y3);
                return;
            }

            // NOTE: Отсутсвует реализация частных случаев триангуляции, когда две или три грани полностью отсекаются

            // Соотношение старых и новых координат после отсечения:
            //
            //                 (_x1,_y1)
            //                     O 1
            //                    / \
            //           (x0,y0) /   \ (x1,y1)
            //                --+-----+--
            //               \ /       \ /
            //        (x2,y2) +         + (x3,y3)
            //               / \       / \
            //            2 /   \     /   \ 3
            //   (_x2,_y2) O ----+---+---- O (_x3,_y3)
            //             (x4,y4)\ /(x5,y5)
            int trimed;
            double _x1 = x1, _y1 = y1, _x2 = x2, _y2 = y2, _x3 = x3, _y3 = y3;
            double  x0 = x1,  y0 = y1,  x4 = x2,  y4 = y2,  x5 = x3,  y5 = y3;
            if (!Clip2D(surface.Clip, ref x0, ref y0, ref x2, ref y2)) {
                if (!Clip2D(surface.Clip, ref x1, ref y1, ref x3, ref y3)) {
                    if (!Clip2D(surface.Clip, ref x4, ref y4, ref x5, ref y5)) {
                        return; // все грани не определены
                    } else {
                        return; // грани 1-2 и 1-3 не определены
                    }
                } else if (!Clip2D(surface.Clip, ref x4, ref y4, ref x5, ref y5)) {
                    return; // грани 1-2 и 2-3 не определены
                } else {
                    x0 = x1;  y0 = y1;  x2 = x4;  y2 = y4;
                    trimed = ((x3 == _x3 && y3 == _y3 && x5 == _x3 && y5 == _y3) ? 11 : 15);
                }
            } else if (!Clip2D(surface.Clip, ref x1, ref y1, ref x3, ref y3)) {
                if (!Clip2D(surface.Clip, ref x4, ref y4, ref x5, ref y5)) {
                    return; // грани 1-3 и 2-3 не определены
                } else {
                    x1 = x0;  y1 = y0;  x3 = x5;  y3 = y5;
                    trimed = ((x2 == _x2 && y2 == _y2 && x4 == _x2 && y4 == _y2) ? 13 : 15);
                }
            } else if (!Clip2D(surface.Clip, ref x4, ref y4, ref x5, ref y5)) {
                x4 = x2;  y4 = y2;  x5 = x3;  y5 = y3;
                trimed = ((x0 == _x1 && y0 == _y1 && x1 == _x1 && y1 == _y1) ? 14 : 15);
            } else
                trimed = ((x0 == _x1 && y0 == _y1 && x1 == _x1 && y1 == _y1) ? 0 : 1)
                       | ((x2 == _x2 && y2 == _y2 && x4 == _x2 && y4 == _y2) ? 0 : 2)
                       | ((x3 == _x3 && y3 == _y3 && x5 == _x3 && y5 == _y3) ? 0 : 4);

            int c0, c1, c2, c3, c4, c5;
            FColor _c1 = color1, _c2 = color2, _c3 = color3;
            if ((trimed & 3) != 0) {
                var cd = (_c2-_c1) / (float)Math.Sqrt((_x2-_x1)*(_x2-_x1) + (_y2-_y1)*(_y2-_y1));
                c0 = _c1 + cd * (float)Math.Sqrt((x0 - _x1)*(x0 - _x1) + (y0 - _y1)*(y0 - _y1));
                c2 = _c1 + cd * (float)Math.Sqrt((x2 - _x1)*(x2 - _x1) + (y2 - _y1)*(y2 - _y1));
            } else { c0 = color1; c2 = color2; }
            if ((trimed & 5) != 0) {
                var cd = (_c3-_c1) / (float)Math.Sqrt((_x3-_x1)*(_x3-_x1) + (_y3-_y1)*(_y3-_y1));
                c1 = _c1 + cd * (float)Math.Sqrt((x1 - _x1)*(x1 - _x1) + (y1 - _y1)*(y1 - _y1));
                c3 = _c1 + cd * (float)Math.Sqrt((x3 - _x1)*(x3 - _x1) + (y3 - _y1)*(y3 - _y1));
            } else { c1 = color1; c3 = color3; }
            if ((trimed & 6) != 0) {
                var cd = (_c3 - _c2) / (float)Math.Sqrt((_x3-_x2)*(_x3-_x2) + (_y3-_y2)*(_y3-_y2));
                c4 = _c2 + cd * (float)Math.Sqrt((x4 - _x2)*(x4 - _x2) + (y4 - _y2)*(y4 - _y2));
                c5 = _c2 + cd * (float)Math.Sqrt((x5 - _x2)*(x5 - _x2) + (y5 - _y2)*(y5 - _y2));
            } else { c4 = color2; c5 = color3; }

            switch (trimed) {
                case 11: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color3, x3, y3, c1, x1, y1, c4, x4, y4);
                         return;
                case 13: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color2, x2, y2, c0, x0, y0, c5, x5, y5);
                         return;
                case 14: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color1, x1, y1, c2, x2, y2, c3, x3, y3);
                         return;
                case  1: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color2, x2, y2, color3, x3, y3, c0, x0, y0);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color3, x3, y3, c0, x0, y0, c1, x1, y1);
                         return;
                case  2: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color1, x1, y1, color3, x3, y3, c4, x4, y4);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color1, x1, y1, c4, x4, y4, c2, x2, y2);
                         return;
                case  4: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color1, x1, y1, color2, x2, y2, c3, x3, y3);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color2, x2, y2, c3, x3, y3, c5, x5, y5);
                         return;
                case  3: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color3, x3, y3, c1, x1, y1, c0, x0, y0);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color3, x3, y3, c0, x0, y0, c2, x2, y2);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color3, x3, y3, c2, x2, y2, c4, x4, y4);
                         return;
                case  5: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color2, x2, y2, c0, x0, y0, c1, x1, y1);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color2, x2, y2, c1, x1, y1, c3, x3, y3);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color2, x2, y2, c3, x3, y3, c5, x5, y5);
                        return;
                case  6: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color1, x1, y1, c2, x2, y2, c4, x4, y4);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color1, x1, y1, c4, x4, y4, c5, x5, y5);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, color1, x1, y1, c5, x5, y5, c3, x3, y3);
                         return;
                case 15:
                case  7: DrawTriangleLQ((int*)surface.ImageBits, surface.Width, c0, x0, y0, c1, x1, y1, c3, x3, y3);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, c0, x0, y0, c2, x2, y2, c4, x4, y4);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, c0, x0, y0, c3, x3, y3, c5, x5, y5);
                         DrawTriangleLQ((int*)surface.ImageBits, surface.Width, c0, x0, y0, c4, x4, y4, c5, x5, y5);
                         return;
                default: throw new NotImplementedException();
            }
        }}

        #endregion

        protected class Clipper {
            public readonly int X1;
            public readonly int Y1;
            public readonly int X2;
            public readonly int Y2;
            public Clipper(CairoSurface surface) {
                X1 = Y1 = 0;
                X2 = surface.Width  - 1;
                Y2 = surface.Height - 1;
            }

            public bool InBounds(double px, double py)
            { unchecked {
                return px >= X1 && py >= Y1 && px <= X2 && py <= Y2;
            }}

            internal bool PointsInBounds(double px1, double py1, double px2, double py2, double px3, double py3)
            { unchecked {
                return px1 >= X1 && px2 >= X1 && px3 >= X1 && py1 >= Y1 && py2 >= Y1 && py3 >= Y1
                    && px1 <= X2 && px2 <= X2 && px3 <= X2 && py1 <= Y2 && py2 <= Y2 && py3 <= Y2;
            }}

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool Clipt(double denom, double num, ref double tE, ref double tL)
            { unchecked {
                double t;
                if (denom > 0.0) {
                    t = num / denom;
                    if (t > tL)
                        return false;
                    if (t > tE)
                        tE = t;
                    return true;
                }
                if (denom < 0.0) {
                    t = num / denom;
                    if (t < tE)
                        return false;
                    if (t < tL)
                        tL = t;
                    return true;
                }
                return (num <= 0);
            }}

            internal bool ClipLine(Clipper clip, ref double x1, ref double y1, ref double x2, ref double y2)
            { unchecked {
                var dx = x2 - x1;
                var dy = y2 - y1;
                if (dx == 0.0 && dy == 0.0 && x1 <= clip.X2 && x1 >= clip.X1
                                            && y1 <= clip.Y2 && y1 >= clip.Y1)
                    return true;
                var te = 0.0;
                var tl = 1.0;
                if (Clipt(dx, clip.X1 - x1, ref te, ref tl)) {
                    if (Clipt(-dx, x1 - clip.X2, ref te, ref tl)) {
                        if (Clipt(dy, clip.Y1 - y1, ref te, ref tl)) {
                            if (Clipt(-dy, y1 - clip.Y2, ref te, ref tl)) {
                                if (tl < 1.0) {
                                    x2 = x1 + tl * dx;
                                    y2 = y1 + tl * dy;
                                }
                                if (te > 0) {
                                    x1 = x1 + te * dx;
                                    y1 = y1 + te * dy;
                                }
                                return true;
                            }
                        }
                    }
                }
                return false;
            }}

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool Clipt(double denom, double num, ref double tE, ref double tL)
        { unchecked {
            double t;
            if (denom > 0.0) {
                t = num / denom;
                if (t > tL)
                    return false;
                if (t > tE)
                    tE = t;
                return true;
            }
            if (denom < 0.0) {
                t = num / denom;
                if (t < tE)
                    return false;
                if (t < tL)
                    tL = t;
                return true;
            }
            return (num <= 0);
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool Clip2D(Clipper clip, ref double x1, ref double y1, ref double x2, ref double y2)
        { unchecked {
            var dx = x2 - x1;
            var dy = y2 - y1;
            if (dx == 0.0 && dy == 0.0 && x1 <= clip.X2 && x1 >= clip.X1
                                       && y1 <= clip.Y2 && y1 >= clip.Y1)
                return true;
            var te = 0.0;
            var tl = 1.0;
            if (Clipt(dx, clip.X1 - x1, ref te, ref tl)) {
                if (Clipt(-dx, x1 - clip.X2, ref te, ref tl)) {
                    if (Clipt(dy, clip.Y1 - y1, ref te, ref tl)) {
                        if (Clipt(-dy, y1 - clip.Y2, ref te, ref tl)) {
                            if (tl < 1.0) {
                                x2 = x1 + tl * dx;
                                y2 = y1 + tl * dy;
                            }
                            if (te > 0) {
                                x1 = x1 + te * dx;
                                y1 = y1 + te * dy;
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }}

        protected static bool ClipRect(Clipper clip, ref int sx, ref int sy, ref int sw, ref int sh)
        {
            if (sx < clip.X1) {
                if ((sw -= clip.X1 - sx) <= 0)
                    return false;
                sx = clip.X1;
            } else {
                if (sx > clip.X2)
                    return false;
            }
            sw += sx;
            if (sw > clip.X2)
                sw = clip.X2 - sx;
            else
                sw -= sx;

            if (sy < clip.Y1) {
                if ((sh -= clip.Y1 - sy) <= 0)
                    return false;
                sy = clip.Y1;
            } else {
                if (sy > clip.Y2)
                    return false;
            }
            sh += sy;
            if (sh > clip.Y2)
                sh = clip.Y2 - sy;
            else
                sh -= sy;

            return sw != 0 && sh != 0;


            sx = (sx < clip.X1) ? clip.X1 : sx;
            sy = (sy < clip.X1) ? clip.Y1 : sy;
            sw = sx + sw; sw = (sw > clip.X2) ? (clip.X2 - sx) : sw - sx;
            sh = sy + sh; sh = (sh > clip.Y2) ? (clip.Y2 - sy) : sh - sy;

            return (++sw > 0 && ++sh > 0);
        }

        #region Рисование линии без сглаживания (реализация алгоритма Брезенхэма)

        protected static unsafe void DrawLineLQ(int* imgptr, int stride, int color, int x1, int y1, int x2, int y2)
        { unchecked {
            if (y1 > y2) {
		        int _t = y1;  y1 = y2;  y2 = _t;
                    _t = x1;  x1 = x2;  x2 = _t;
	        }
            imgptr += y1 * stride + x1;

            int ydelta, xdelta, xadvance;
            ydelta = y2 - y1;
            if ((xdelta = x2 - x1) < 0) {
                xadvance = -1;
                xdelta = -xdelta;
            } else if (xdelta == 0) {
                while (ydelta-- >= 0) {
                    *imgptr = color;
                    imgptr += stride;
                } return;
            } else
                xadvance = 1;

            if (ydelta == 0) {
                while (xdelta-- >= 0) {
                    *imgptr = color;
                    imgptr += xadvance;
                } return;
            }

            int pstep, lstep, hpart, lpart;
            if (xdelta > ydelta) {
                hpart = xdelta;   lpart = ydelta;
                pstep = xadvance; lstep = stride;
            } else if (xdelta != ydelta) {
                hpart = ydelta;   lpart = xdelta;
                pstep = stride;   lstep = xadvance;
            } else {
                while (xdelta-- >= 0) {
                    *imgptr = color;
                    imgptr += stride + xadvance;
                } return;
            }
            int adjInc, adjDec, erTerm, runStep, runFull, runInit, runLast;
            runStep = hpart / lpart;
            erTerm  = hpart - lpart * runStep;
            adjInc  = erTerm << 1;
            adjDec  = lpart << 1;
            erTerm -= adjDec;
            runInit = (runStep >> 1) + 1;
            runLast = runInit;
            if ((adjInc == 0) && ((runStep & 0x01) == 0))
                runInit--;
            if ((runStep & 0x01) != 0)
                erTerm += lpart;

            while (--runInit >= 0) {
                *imgptr = color;
                imgptr += pstep;
            } imgptr += lstep;

            while (--lpart > 0)  {
                runFull = runStep;
                if ((erTerm += adjInc) > 0) {
                    runFull++;
                    erTerm -= adjDec;
                }
                while (--runFull >= 0) {
                    *imgptr = color;
                    imgptr += pstep;
                } imgptr += lstep;
            }
            while (--runLast >= 0) {
                *imgptr = color;
                imgptr += pstep;
            }
        }}

        #endregion

        #region Рисование линии со сглаживанием (реализация алгоритма Ву Сяолиня)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static unsafe void plot(int* imgptr, int color, double c)
        { unchecked {
            int sa = (int)(c * 255);
            sa = sa - (sa & (sa >> 31)) - 0xFF;
            sa = 0xFF + (sa & (sa >> 31));
            int rb = ((0xFF - sa)*(*imgptr & 0x00FF00FF) + sa*(color & 0x00FF00FF)) >> 8;
            int ag = ((*imgptr & 0x0000FF00) + sa*(((color & 0x0000FF00) - (*imgptr & 0x0000FF00)) >> 8));
            *imgptr = (int)0xFF000000 | (ag & 0x0000FF00) | (rb & 0x00FF00FF);
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static unsafe void DrawLineHQ(int* imgptr, int stride, int color, double x1, double y1, double x2, double y2)
        { unchecked {
            double xEnd1, yEnd1, xGap1, yGap1, xEnd2, yEnd2, xGap2, yGap2;
            int xstep, ystep, run, prev, cur;
            int* lprun, lptp1, lptp2;
            bool steep;

            if ((steep = Math.Abs(y2-y1) > Math.Abs(x2-x1))) {
                xGap1=x1; x1=y1; y1=xGap1;
                xGap1=x2; x2=y2; y2=xGap1;
            }
            if (x1>x2) {
                xGap1=x1; x1=x2; x2=xGap1;
                xGap1=y1; y1=y2; y2=xGap1;
            }

            double gradient = (y2-y1)/(x2-x1);
            xGap1 = (0.5-x1 + (xEnd1=(int)(x1+0.5)));
            yEnd1 = y1+gradient*(xEnd1-x1);
            yGap1 = xGap1 * ((yEnd1<0) ? (1.0-(yEnd1-(int)(yEnd1-0.9999999999))) : (yEnd1-(int)yEnd1));
            xGap1 -= yGap1;

            double intery = yEnd1+gradient;
            xGap2 = (0.5+x2 - (xEnd2=(int)(x2+0.5)));
            yEnd2 = y2 + gradient * (xEnd2 - x2);
            yGap2 = xGap2 * ((yEnd2<0) ? (1.0-(yEnd2-(int)(yEnd2-0.9999999999))) : (yEnd2-(int)yEnd2));
            xGap2 -= yGap2;

            run = (ystep=(int)xEnd2) - (xstep=(int)xEnd1);
            if (steep) {
                lptp1 = imgptr + stride*xstep + (int)yEnd1;
                lptp2 = imgptr + stride*ystep + (int)yEnd2;
                lprun = imgptr + stride*xstep+(prev=(int)intery);
                xstep = 1; ystep = stride;
            } else {
                lptp1 = imgptr + stride*(int)yEnd1 + xstep;
                lptp2 = imgptr + stride*(int)yEnd2 + ystep;
                lprun = imgptr + stride*(prev=(int)intery)+xstep;
                ystep = 1; xstep = stride;
            }

            plot(lptp1, color, xGap1);
            plot(lptp1+xstep, color, yGap1);
            plot(lptp2, color, xGap2);
            plot(lptp2+xstep, color, yGap2);

            while (--run > 0) {
                cur = (-prev + (prev = (int)intery));
                lprun += (-(cur & 1)) & ((xstep^(cur>>=1)) + (cur&1));
                xGap1 = (intery<0) ? (1.0-(intery-(int)(intery-0.9999999999))) : (intery-(int)intery);
                intery += gradient;
                plot(lprun+= ystep, color, 1.0 - xGap1);
                plot(lprun + xstep, color, xGap1);
            }
        }}

        #endregion

        #region Рисование треугольников

        protected class FColor
        {
	        public float R, G, B, A;

            public FColor(float r = 1.0f, float g = 1.0f, float b = 1.0f, float a = 1.0f) {
                R = r; G = g; B = b; A = a;
            }

            public FColor(byte r = 255, byte g = 255, byte b = 255, byte a = 255) {
                R = r / 255f; G = g / 255f; B = b / 255f; A = a / 255f;
            }

            public FColor(int argb) {
                A = ((argb >> 24) & 0xFF) / 255f;
                R = ((argb >> 16) & 0xFF) / 255f;
                G = ((argb >>  8) & 0xFF) / 255f;
                B = (argb & 0xFF) / 255f;
            }

            public FColor(FColor color) {
                R = color.R; G = color.G; B = color.B; A = color.A;
            }

            public FColor(System.Drawing.Color color) : this(color.ToArgb()) {}

		    public int ToInt32()
            {
	            int r = (int)(R * 255f);
	            int g = (int)(G * 255f);
	            int b = (int)(B * 255f);
	            int a = (int)(A * 255f);
	            return (a << 24) | (r << 16) | (g << 8) | b;
            }

            public override string ToString()
            {
                return String.Format("{0}, {1}, {2} ({3:X6})", R, G, B, ToInt32() & 0xFFFFFF);
            }

            public System.Drawing.Color ToColor() {
                return System.Drawing.Color.FromArgb(ToInt32());
            }

            public void Add(FColor value) {
                R += value.R;  G += value.G;  B += value.B;  A += value.A;
            }

            public void Sub(FColor value) {
                R -= value.R;  G -= value.G;  B -= value.B;  A -= value.A;
            }

            public void Mul(float value) {
                R *= value;  G *= value;  B *= value;  A *= value;
            }

            public void Div(float value) {
                value = 1 / value;
                R *= value;  G *= value;  B *= value;  A *= value;
            }

            public static FColor operator +(FColor left, FColor right) {
                return new FColor(left.R + right.R, left.G + right.G, left.B + right.B, left.A + right.A);
		    }

            public static FColor operator -(FColor left, FColor right) {
                return new FColor(left.R - right.R, left.G - right.G, left.B - right.B, left.A - right.A);
		    }

            public static FColor operator *(FColor left, float right) {
                return new FColor(left.R * right, left.G * right, left.B * right);
		    }

            public static FColor operator *(float left, FColor right) {
                return new FColor(right.R * left, right.G * left, right.B * left);
		    }

            public static FColor operator *(FColor left, FColor right) {
                return new FColor(right.R * left.R, right.G * left.G, right.B * left.B);
		    }

            public static FColor operator /(FColor left, float right) {
                return new FColor(left.R / right, left.G / right, left.B / right, left.A / right);
		    }

            public static FColor operator /(float left, FColor right) {
                return new FColor(right.R / left, right.G / left, right.B / left, right.A / left);
		    }

            public static implicit operator FColor(int value) {
                return new FColor(value);
            }

            public static implicit operator int(FColor value) {
                return value.ToInt32();
            }

            public static FColor FromRYB(float r, float y, float b)
            {
                  // RYB → RGB
                var f000 = new FColor(1f, 1f, 1f);
                var f001 = new FColor(0.163f, 0.373f, 0.6f);
                var f010 = new FColor(1f, 1f, 0f);
                var f011 = new FColor(0f, 0.66f, 0.2f);
                var f100 = new FColor(1f, 0f, 0f);
                var f101 = new FColor(0.5f, 0.5f, 0f);
                var f110 = new FColor(1f, 0.5f, 0f);
                var f111 = new FColor(0.2f, 0.094f, 0.0f);
                return f000 * ((1f-r)*(1f-y)*(1f-b))
                     + f001 * ((1f-r)*(1f-y)*(b))
                     + f010 * ((1f-r)*(y)*(1f-b))
                     + f100 * ((r)*(1f-y)*(1f-b))
                     + f011 * ((1f-r)*(y)*(b))
                     + f101 * ((r)*(1f-y)*(b))
                     + f110 * ((r)*(y)*(1f-b))
                     + f111 * ((r)*(y)*(b)) ;
            }

            public static FColor FromHSV(float h, float s, float v)
            {
                if (s == 0)
                    return new FColor(v, v, v);
                h = (h == 360) ? 0 : (h/60f);
                int i = (int)Math.Truncate(h);
		        float f = h - i;
		        float p = v * (1f - s);
		        float q = v * (1f - (s * f));
		        float t = v * (1f - (s * (1f - f)));
                switch (i) {
                    case  0: return new FColor(v, t, p);
			        case  1: return new FColor(q, v, p);
			        case  2: return new FColor(p, v, t);
			        case  3: return new FColor(p, q, v);
			        case  4: return new FColor(t, p, v);
			        default: return new FColor(v, p, q);
                }
            }

            public static void ToHSV(FColor rgb, out float h, out float s, out float v)
            {
                float delta, min;
	            h = 0;
	            min = Math.Min(Math.Min(rgb.R, rgb.G), rgb.B);
	            v   = Math.Max(Math.Max(rgb.R, rgb.G), rgb.B);
	            delta = v - min;

                s = (v == 0.0) ? 0 : (delta / v);
	            if (s == 0)
		            h = 0f;
	            else {
		            if (rgb.R == v)
			            h = (rgb.G - rgb.B) / delta;
		            else if (rgb.G == v)
			            h = 2 + (rgb.B - rgb.R) / delta;
		            else if (rgb.B == v)
			            h = 4 + (rgb.R - rgb.G) / delta;

		            h *= 60;
		            if (h < 0.0)
			            h = h + 360;
	            }
                if (h > 360 || h < 0 || s < 0 || s > 1 || v < 0 || v > 1)
                    throw new Exception();
            }

            public void Mul(float s, float v)
            {
                float _h, _s, _v;
                ToHSV(this, out _h, out _s, out _v);
                var color = FromHSV(_h, Math.Min(_s * s, 1f), Math.Min(_v * v, 1f));
                R = color.R; G = color.G; B = color.B;
            }

        }



        [StructLayout(LayoutKind.Explicit, Size = 12)]
        protected struct SColor {
            [FieldOffset(0)] public float R;
	        [FieldOffset(4)] public float G;
            [FieldOffset(8)] public float B;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static SColor operator +(SColor left, SColor right)
            { unchecked {
                return new SColor() { R = left.R + right.R,
                                      G = left.G + right.G,
                                      B = left.B + right.B };
		    }}

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static SColor operator -(SColor left, SColor right)
            { unchecked {
                return new SColor() { R = left.R - right.R,
                                      G = left.G - right.G,
                                      B = left.B - right.B };
		    }}

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static SColor operator *(SColor left, float right)
            { unchecked {
                return new SColor() { R = left.R * right,
                                      G = left.G * right,
                                      B = left.B * right };
		    }}

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static SColor operator *(float left, SColor right)
            { unchecked {
                return new SColor() { R = right.R * left,
                                      G = right.G * left,
                                      B = right.B * left };
		    }}

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator SColor(int value)
            { unchecked {
                return new SColor() { R = ((value >> 16) & 0xFF) / 255f,
                                      G = ((value >>  8) & 0xFF) / 255f,
                                      B = (value & 0xFF) / 255f };
            }}

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator int(SColor value)
            { unchecked {
                int r = (int)(value.R * 255f);
                int g = (int)(value.G * 255f);
                int b = (int)(value.B * 255f);
                return (unchecked((int)0xFF000000)) | (r << 16) | (g << 8) | b;
            }}
        }

        [StructLayout(LayoutKind.Explicit, Size = 48)]
        protected struct SPoint {
            [FieldOffset( 0)] public SColor Color;
		    [FieldOffset(16)] public int X;
            [FieldOffset(20)] public int Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static unsafe void DrawTriangleLQ(int* imgptr, int stride, int color,
                                double x1, double y1, double x2, double y2, double x3, double y3)
        { unchecked {
            var point = stackalloc SPoint[3];
            //02 34 51
            // 1 2 3  +  +  +
            // 1 3 2  +  +  -
            // 2 1 3  -        +  +
            // 2 3 1  -        +  -
            // 3 1 2  +  -
            // 3 2 1  -        -
            if (y1 < y2) {
                if (y1 < y3) {
                    point[0].X = (int)x1;  point[0].Y = (int)y1;
                    if (y2 < y3) {
                        point[1].X = (int)x2;  point[1].Y = (int)y2;
                        point[2].X = (int)x3;  point[2].Y = (int)y3;
                    } else {
                        point[1].X = (int)x3;  point[1].Y = (int)y3;
                        point[2].X = (int)x2;  point[2].Y = (int)y2;
                    }
                } else {
                    point[0].X = (int)x3;  point[0].Y = (int)y3;
                    point[1].X = (int)x1;  point[1].Y = (int)y1;
                    point[2].X = (int)x2;  point[2].Y = (int)y2;
                }
            } else if (y2 < y3) {
                point[0].X = (int)x2;  point[0].Y = (int)y2;
                if (y1 < y3) {
                    point[1].X = (int)x1;  point[1].Y = (int)y1;
                    point[2].X = (int)x3;  point[2].Y = (int)y3;
                } else {
                    point[1].X = (int)x3;  point[1].Y = (int)y3;
                    point[2].X = (int)x1;  point[2].Y = (int)y1;
                }
            } else {
                point[0].X = (int)x3; point[0].Y = (int)y3;
                point[1].X = (int)x2; point[1].Y = (int)y2;
                point[2].X = (int)x1; point[2].Y = (int)y1;
            }

            float e1ydiff = (float)(point[2].Y - point[0].Y);
            if (e1ydiff == 0.0f) return;
            float factorStep1 = 1.0f / e1ydiff;
            float e1xdiff = (float)(point[2].X - point[0].X);

            int p = 1; do {
                float e2ydiff = (float)(point[p + 1].Y - point[p].Y);
                if (e2ydiff == 0.0f) continue;
                float e2xdiff = (float)(point[p + 1].X - point[p].X);
                float factorStep2 = 1.0f / e2ydiff;
                float factor1 = (float)(point[p].Y - point[0].Y) * factorStep1;
                float factor2 = 0.0f;

                var psrc = imgptr + point[p].Y * stride;
                var hlen = point[p+1].Y - point[p].Y;
                do {
                    int sX1 = point[0].X + (int)(e1xdiff * factor1);
                    int sX2 = point[p].X + (int)(e2xdiff * factor2);
                    int xdiff = sX2 - sX1;
                    if (xdiff > 0) {
                        psrc += sX1;
                        do *(psrc++) = color;
                        while (--xdiff != 0);
                        psrc -= sX2;
                    } else if (xdiff < 0) {
                        psrc += sX2;
                        do *(psrc++) = color;
                        while (++xdiff != 0);
                        psrc -= sX1;
                    }
                    psrc += stride;
		            factor1 += factorStep1;
		            factor2 += factorStep2;
	            } while (--hlen > 0);
            } while (--p == 0);
        }}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static unsafe void DrawTriangleLQ(int* imgptr, int stride,
                                                int color1, double x1, double y1,
                                                int color2, double x2, double y2,
                                                int color3, double x3, double y3)
        { unchecked {
            var point = stackalloc SPoint[3];
            //02 34 51
            // 1 2 3  +  +  +
            // 1 3 2  +  +  -
            // 2 1 3  -        +  +
            // 2 3 1  -        +  -
            // 3 1 2  +  -
            // 3 2 1  -        -
            if (y1 < y2) {
                if (y1 < y3) {
                    point[0].Color = color1;  point[0].X = (int)x1;  point[0].Y = (int)y1;
                    if (y2 < y3) {
                        point[1].Color = color2;  point[1].X = (int)x2;  point[1].Y = (int)y2;
                        point[2].Color = color3;  point[2].X = (int)x3;  point[2].Y = (int)y3;
                    } else {
                        point[1].Color = color3;  point[1].X = (int)x3;  point[1].Y = (int)y3;
                        point[2].Color = color2;  point[2].X = (int)x2;  point[2].Y = (int)y2;
                    }
                } else {
                    point[0].Color = color3;  point[0].X = (int)x3;  point[0].Y = (int)y3;
                    point[1].Color = color1;  point[1].X = (int)x1;  point[1].Y = (int)y1;
                    point[2].Color = color2;  point[2].X = (int)x2;  point[2].Y = (int)y2;
                }
            } else if (y2 < y3) {
                point[0].Color = color2;  point[0].X = (int)x2;  point[0].Y = (int)y2;
                if (y1 < y3) {
                    point[1].Color = color1;  point[1].X = (int)x1;  point[1].Y = (int)y1;
                    point[2].Color = color3;  point[2].X = (int)x3;  point[2].Y = (int)y3;
                } else {
                    point[1].Color = color3;  point[1].X = (int)x3;  point[1].Y = (int)y3;
                    point[2].Color = color1;  point[2].X = (int)x1;  point[2].Y = (int)y1;
                }
            } else {
                point[0].Color = color3; point[0].X = (int)x3; point[0].Y = (int)y3;
                point[1].Color = color2; point[1].X = (int)x2; point[1].Y = (int)y2;
                point[2].Color = color1; point[2].X = (int)x1; point[2].Y = (int)y1;
            }

            float e1ydiff = (float)(point[2].Y - point[0].Y);
            if (e1ydiff == 0.0f) return;
            float e1xdiff = (float)(point[2].X - point[0].X);
            SColor e1colordiff = (point[2].Color - point[0].Color);

            int p = 1; do {
                float e2ydiff = (float)(point[p + 1].Y - point[p].Y);
                if (e2ydiff == 0.0f) continue;
                float e2xdiff = (float)(point[p + 1].X - point[p].X);
                SColor e2colordiff = (point[p + 1].Color - point[p].Color);
                float factor1 = (float)(point[p].Y - point[0].Y) / e1ydiff;
                float factorStep1 = 1.0f / e1ydiff;
                float factor2 = 0.0f;
                float factorStep2 = 1.0f / e2ydiff;

                var psrc = imgptr + point[p].Y * stride;
                for (int y = point[p].Y; y < point[p+1].Y; y++) {
                    int sX1 = point[0].X + (int)(e1xdiff * factor1);
                    int sX2 = point[p].X + (int)(e2xdiff * factor2);

                    SColor colordiff, sColor1;
                    int xdiff = sX2 - sX1;
                    if (xdiff != 0) {
                        if (xdiff <= 0) {
                            sX1 = sX2; sX2 = sX1 + (xdiff = -xdiff);
                            sColor1 = point[p].Color + (e2colordiff * factor2);
                            colordiff = point[0].Color + (e1colordiff*factor1) - sColor1;
                        } else {
                            sColor1 = point[0].Color + (e1colordiff * factor1);
                            colordiff = point[p].Color + (e2colordiff * factor2) - sColor1;
                        }
                        float factor = 0.0f;
                        float factorStep = 1.0f / (float)xdiff;

	                    psrc += sX1;
	                    do {
                            *psrc = (sColor1 + (colordiff * factor));
                            ++psrc;
                            factor += factorStep;
                        } while (--xdiff != 0);
                        psrc -= sX2;
                    }

                    psrc += stride;

		            factor1 += factorStep1;
		            factor2 += factorStep2;
	            }
            } while (--p == 0);
        }}

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static unsafe void FillRectangle(
            int* imgptr, int stride,
            int color, int sx, int sy, int width, int height)
        { unchecked {
            var srsLine = imgptr + sy * stride + sx;
            Parallel.For(0, height, y => {
                var srsData = srsLine + y * stride;
                for (int x = 0; x < width; ++x) {
                    *srsData = color;
                    ++srsData;
                }
            });

        }}

    }
}
