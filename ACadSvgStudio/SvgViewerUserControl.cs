﻿using Svg;
using Svg.Transforms;

namespace ACadSvgStudio {

	public partial class SvgViewerUserControl : UserControl {

		private bool _debugEnabled = false;
		public bool DebugEnabled
		{
			get
			{
				return _debugEnabled;
			}
			set
			{
				_debugEnabled = value;
				Invalidate();
			}
		}

		private Svg.SvgDocument _svgDocument;

		private bool _needsUpdate = true;

		private int _x = 0;
		private int X
		{
			get
			{
				return _x;
			}
			set
			{
				if (value != _x)
				{
					_needsUpdate = true;
				}

				_x = value;
			}
		}

		private int _y = 0;
		private int Y
		{
			get
			{
				return _y;
			}
			set
			{
				if (value != _y)
				{
					_needsUpdate = true;
				}

				_y = value;
			}
		}

		private float _zoom = 1;
		private float Zoom
		{
			get
			{
				return _zoom;
			}
			set
			{
				if (_zoom != value)
				{
					_needsUpdate = true;
				}

				_zoom = value;
			}
		}

		private SizeF _dimensions;

		private bool _posInitialized = false;

		private SvgTransformCollection _svgTransformCollection;

		private bool _mouseDown = false;
		private Point _mousePos;


		public SvgViewerUserControl()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);

			this.MouseWheel += OnMouseWheel;
		}


		public void LoadSvgContent(string content)
		{
			try
			{
				_needsUpdate = true;

				_svgDocument = Svg.SvgDocument.FromSvg<SvgDocument>(content);

				if (!_posInitialized)
				{
					X = 0;
					Y = 0;

					Zoom = 1;

					CenterToFit();

					_posInitialized = true;
				}

				Invalidate();
			}
			catch
			{

			}
		}


		private bool hasErrors()
		{
			if (_svgDocument == null)
			{
				return true;
			}

			return false;
		}


		private SizeF calculateTransforms()
		{
			if (_svgDocument == null)
			{
				_dimensions = new SizeF(0, 0);
				return _dimensions;
			}

			if (_needsUpdate)
			{
				_svgDocument.ViewBox = new SvgViewBox(_svgDocument.Bounds.X, _svgDocument.Bounds.Y, _svgDocument.Bounds.Width, _svgDocument.Bounds.Height);
				_dimensions = _svgDocument.GetDimensions();
			}

			return _dimensions;
		}


		private Rectangle getBoundingBox()
		{
			int width = 0;
			int height = 0;

			if (_svgDocument != null)
			{
				width = (int)(_svgDocument.Bounds.Width * _zoom);
				height = (int)(_svgDocument.Bounds.Height * _zoom);
			}

			return new Rectangle(_x, _y, width, height);
		}


		private void draw(Graphics g)
		{
			SizeF size = calculateTransforms();

			if (size.Width == 0 || size.Height == 0)
			{
				_needsUpdate = false;
				return;
			}

			try
			{
				g.TranslateTransform(_x, _y);
				_svgDocument.Draw(g, new SizeF(_svgDocument.Bounds.Width * _zoom, _svgDocument.Bounds.Height * _zoom));

				_needsUpdate = false;
			}
			catch (Exception ex)
			{

			}
		}


		private void SvgViewerUserControl_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(BackColor);


			e.Graphics.DrawString("SVG Viewer (Experimental)", this.Font, new SolidBrush(Color.Yellow), 0, 0);


			e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

			e.Graphics.DrawRectangle(new Pen(Color.Red), 0, 0, Width - 1, Height - 1);


			if (hasErrors())
			{
				e.Graphics.FillRectangle(new SolidBrush(Color.IndianRed), 0, 0, Width, Height);

				return;
			}


			if (DebugEnabled)
			{
				SizeF size = calculateTransforms();

				Color sizeColor = Color.SkyBlue;
				Rectangle boundingBox = getBoundingBox();
				e.Graphics.DrawRectangle(new Pen(sizeColor), boundingBox);

				List<string> lines = new List<string>();
				lines.Add($"X: {_x}");
				lines.Add($"Y: {_y}");
				lines.Add($"Zoom: {_zoom}");
				lines.Add($"ViewBox: Min X: {_svgDocument.ViewBox.MinX}, Min Y: {_svgDocument.ViewBox.MinY}, Width: {_svgDocument.ViewBox.Width}, Height: {_svgDocument.ViewBox.Height}");
				lines.Add($"Size Width: {size.Width}");
				lines.Add($"Size Height: {size.Height}");
				lines.Add($"User Control Width: {Width}");
				lines.Add($"User Control Height: {Height}");

				for (int y = 0; y < lines.Count; y++)
				{
					string line = lines[y];

					SolidBrush brush;
					if (line.StartsWith("Size"))
					{
						brush = new SolidBrush(sizeColor);
					}
					else
					{
						brush = new SolidBrush(Color.Yellow);
					}

					e.Graphics.DrawString(line, this.Font, brush, 0, (y + 1) * 20);
				}
			}


			draw(e.Graphics);
		}


		private void center()
		{
			Rectangle boundingBox = getBoundingBox();
			if (boundingBox.Width != 0 && boundingBox.Height != 0)
			{
				float w1 = (float)Width / 2;
				float h1 = (float)Height / 2;

				float w2 = (float)boundingBox.Width / 2;
				float h2 = (float)boundingBox.Height / 2;

				X = (int)(w1 - w2);
				Y = (int)(h1 - h2);
			}
		}


		public void CenterToFit()
		{
			if (_svgDocument == null)
			{
				return;
			}


			float w = _svgDocument.Bounds.Width;
			float h = _svgDocument.Bounds.Height;

			if (w != 0 && h != 0)
			{
				if (Width > Height)
				{
					Zoom = Height / Math.Max(w, h);
				}
				else
				{
					Zoom = Width / Math.Max(w, h);
				}

				// Offset
				Zoom /= 1.2f;

				center();

				Invalidate();
			}
		}


		private void centerToFitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CenterToFit();
		}


		private void SvgViewerUserControl_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && !_mouseDown)
			{
				_mouseDown = true;
				_mousePos = new Point((_x * -1) + e.X, (_y * -1) + e.Y);
			}
		}

		private void SvgViewerUserControl_MouseMove(object sender, MouseEventArgs e)
		{
			if (_mouseDown)
			{
				X = (_mousePos.X - (e.Location.X)) * -1;
				Y = (_mousePos.Y - (e.Location.Y)) * -1;

				Invalidate();
			}
		}

		private void SvgViewerUserControl_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_mouseDown = false;
			}
		}

		private void SvgViewerUserControl_Resize(object sender, EventArgs e)
		{
			Invalidate();
		}


		private void OnMouseWheel(object? sender, MouseEventArgs e)
		{
			float zoomFactor = 0.0001f;

			SizeF prevSize = calculateTransforms();

			Zoom += e.Delta * zoomFactor;

			SizeF newSize = calculateTransforms();

			if (prevSize.Width != 0 && prevSize.Height != 0)
			{
				SizeF deltaSize = newSize - prevSize;

				X = (int)(_x - deltaSize.Width);
				Y = (int)(_y - deltaSize.Height);
			}

			Invalidate();
		}
	}
}
