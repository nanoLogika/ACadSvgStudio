#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

using Svg;
using System.Drawing.Drawing2D;

namespace ACadSvgStudio {

    public partial class SvgViewerUserControl : UserControl {

        private bool _debugEnabled = false;
        private SvgDocument _svgDocument;
        private RectangleF _bounds;
        private bool _needsUpdate = true;
        private int _x = 0;
        private int _y = 0;
        private float _zoom = 1;
        private SizeF _dimensions;
        private bool _mouseDown = false;
        private Point _mousePos;


        public SvgViewerUserControl() {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);

            this.MouseWheel += OnMouseWheel;
        }


        public bool DebugEnabled {
            get {
                return _debugEnabled;
            }
            set {
                _debugEnabled = value;
                Invalidate();
            }
        }


        private int X {
            get {
                return _x;
            }
            set {
                if (value != _x) {
                    _needsUpdate = true;
                }

                _x = value;
            }
        }


        private int Y {
            get {
                return _y;
            }
            set {
                if (value != _y) {
                    _needsUpdate = true;
                }

                _y = value;
            }
        }


        private float Zoom {
            get {
                return _zoom;
            }
            set {
                if (_zoom != value) {
                    updateViewBox();
                    _needsUpdate = true;
                }

                _zoom = value;
            }
        }


        public void LoadSvgContent(string content, bool resetPosition) {
            try {
                _needsUpdate = true;

                _svgDocument = Svg.SvgDocument.FromSvg<SvgDocument>(content);
                _bounds = _svgDocument.Bounds;

                updateViewBox();

                if (resetPosition) {
                    X = 0;
                    Y = 0;

                    Zoom = 1;

                    CenterToFit();
                }
                else {
                    Invalidate();
                }
            }
            catch {

            }
        }


        private bool hasErrors() {
            if (_svgDocument == null) {
                return true;
            }

            return false;
        }


        private void updateViewBox() {
            if (_svgDocument == null) {
                return;
            }

            _svgDocument.ViewBox = new SvgViewBox(_bounds.X, _bounds.Y, _bounds.Width, _bounds.Height);
        }


        private Rectangle getBoundingBox() {
            int width = 0;
            int height = 0;

            if (_svgDocument != null) {
                width = (int)(_bounds.Width * _zoom);
                height = (int)(_bounds.Height * _zoom);
            }

            return new Rectangle(_x, _y, width, height);
        }


        private void draw(Graphics g) {
            Rectangle boundingBox = getBoundingBox();

            if (boundingBox.Width == 0 || boundingBox.Height == 0) {
                _needsUpdate = false;
                return;
            }

            try {
                g.TranslateTransform(_x, _y);

                _svgDocument.Draw(g, boundingBox.Size);

				_needsUpdate = false;
            }
            catch (Exception ex) {

            }
        }


        private void SvgViewerUserControl_Paint(object sender, PaintEventArgs e) {
            e.Graphics.Clear(BackColor);
            e.Graphics.DrawString("SVG Viewer", this.Font, new SolidBrush(Color.Yellow), 0, 0);
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            if (hasErrors()) {
                e.Graphics.FillRectangle(new SolidBrush(Color.IndianRed), 0, 0, Width, Height);
                return;
            }

            if (DebugEnabled) {
                SizeF size = _svgDocument.GetDimensions();

                Color sizeColor = Color.SkyBlue;
                Rectangle boundingBox = getBoundingBox();
                e.Graphics.DrawRectangle(new Pen(sizeColor), boundingBox);


                e.Graphics.DrawLine(new Pen(new HatchBrush(HatchStyle.SmallCheckerBoard, Color.DeepSkyBlue), 1), Width / 2, 0, Width / 2, Height);
				e.Graphics.DrawLine(new Pen(new HatchBrush(HatchStyle.SmallCheckerBoard, Color.DeepSkyBlue), 1), 0, Height / 2, Width, Height / 2);


				List<string> lines = new List<string>();
                lines.Add($"X: {_x}");
                lines.Add($"Y: {_y}");
                lines.Add($"Zoom: {_zoom}");
                lines.Add($"ViewBox: Min X: {_svgDocument.ViewBox.MinX}, Min Y: {_svgDocument.ViewBox.MinY}, Width: {_svgDocument.ViewBox.Width}, Height: {_svgDocument.ViewBox.Height}");
                lines.Add($"Size Width: {size.Width}");
                lines.Add($"Size Height: {size.Height}");
                lines.Add($"User Control Width: {Width}");
                lines.Add($"User Control Height: {Height}");

                for (int y = 0; y < lines.Count; y++) {
                    string line = lines[y];

                    SolidBrush brush;
                    if (line.StartsWith("Size")) {
                        brush = new SolidBrush(sizeColor);
                    }
                    else {
                        brush = new SolidBrush(Color.Yellow);
                    }

                    e.Graphics.DrawString(line, this.Font, brush, 0, (y + 1) * 20);
                }
            }

            draw(e.Graphics);
        }


        private void center() {
            Rectangle boundingBox = getBoundingBox();
            if (boundingBox.Width != 0 && boundingBox.Height != 0) {
                float w1 = (float)Width / 2;
                float h1 = (float)Height / 2;

                float w2 = (float)boundingBox.Width / 2;
                float h2 = (float)boundingBox.Height / 2;

                X = (int)(w1 - w2);
                Y = (int)(h1 - h2);
            }
        }


        public void CenterToFit() {
            if (_svgDocument == null) {
                return;
            }

            float w = _bounds.Width;
            float h = _bounds.Height;

            if (w != 0 && h != 0) {
                Zoom = Math.Min(Width, Height) / Math.Max(w, h);

                // Offset margin 10%
                Zoom -= Zoom * 0.1f;

                center();

                Invalidate();
            }
        }


        private void centerToFitToolStripMenuItem_Click(object sender, EventArgs e) {
            CenterToFit();
        }


        private void SvgViewerUserControl_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && !_mouseDown) {
                _mouseDown = true;
                _mousePos = new Point((_x * -1) + e.X, (_y * -1) + e.Y);
            }
        }


        private void SvgViewerUserControl_MouseMove(object sender, MouseEventArgs e) {
            bool invalidate = _mouseDown || DebugEnabled;

            if (_mouseDown) {
                X = (_mousePos.X - (e.Location.X)) * -1;
                Y = (_mousePos.Y - (e.Location.Y)) * -1;
            }

            if (invalidate) {
				Invalidate();
			}
        }


        private void SvgViewerUserControl_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                _mouseDown = false;
            }
        }


        private void SvgViewerUserControl_Resize(object sender, EventArgs e) {
            Invalidate();
        }


        private void OnMouseWheel(object? sender, MouseEventArgs e) {
            if (_svgDocument == null) {
                return;
            }


            Rectangle prevRect = getBoundingBox();

            float factor = 1.2f;
            if (e.Delta > 0) {
                Zoom *= factor;
            }
            else {
                Rectangle boundingBox = getBoundingBox();
                if (boundingBox.Width > 0 && boundingBox.Height > 0) {
                    Zoom /= factor;
                }
            }
            
            Rectangle newRect = getBoundingBox();
            
            if (prevRect.Width != 0 && prevRect.Height != 0) {
                Size deltaSize = newRect.Size - prevRect.Size;

                X -= (deltaSize.Width / 2);
                Y -= (deltaSize.Height / 2);
            }
            
            Invalidate();
        }
    }
}
