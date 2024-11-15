using Svg;
using Svg.Transforms;

namespace ACadSvgStudio {

	public partial class SvgViewerUserControl : UserControl {

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
				if (_svgDocument.Transforms == null)
				{
					_svgDocument.Transforms = new SvgTransformCollection();
					_svgDocument.Transforms.Add(new SvgScale(1, 1));
				}

				_svgTransformCollection = (SvgTransformCollection)_svgDocument.Transforms.Clone();

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
				_svgDocument.ViewBox = new SvgViewBox(_svgDocument.Bounds.X, _svgDocument.Bounds.Y, _svgDocument.Bounds.Width * _zoom, _svgDocument.Bounds.Height * _zoom);
				if (_svgTransformCollection != null)
				{
					_svgDocument.Transforms = (SvgTransformCollection)_svgTransformCollection.Clone();

					if (_svgDocument.Transforms != null)
					{
						foreach (SvgTransform transform in _svgDocument.Transforms)
						{
							if (transform is SvgScale scale)
							{
								scale.X = scale.X * _zoom;
								scale.Y = scale.Y * _zoom;
							}
						}
					}
				}

				_dimensions = _svgDocument.GetDimensions();
			}

			return _dimensions;
		}


		private void draw(Graphics g)
		{
			SizeF size = calculateTransforms();

			if (size.Width == 0 || size.Height == 0)
			{
				_needsUpdate = false;
				return;
			}

			g.TranslateTransform(_x, _y);
			_svgDocument.Draw(g);
			g.TranslateTransform(-_x, -_y);

			_needsUpdate = false;
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


			draw(e.Graphics);
		}


		public void CenterToFit()
		{
			bool sizeCalculated = false;
			bool isLargerThanControl = false;

			SizeF bitmapSize = new SizeF(0, 0);

			do {
				bitmapSize = calculateTransforms();

				if (!sizeCalculated)
				{
					isLargerThanControl = bitmapSize.Width > Width || bitmapSize.Height > Height;
					sizeCalculated = true;
				}

				X = (int)((Width / 2) - (bitmapSize.Width / 2));
				Y = (int)((Height / 2) - (bitmapSize.Height / 2));


				if (isLargerThanControl && (bitmapSize.Width > Width || bitmapSize.Height > Height)) {
					Zoom -= 0.1f;
				}

				if (!isLargerThanControl && (bitmapSize.Width < Width || bitmapSize.Height < Height))
				{
					Zoom += 0.1f;
				}
			}
			while ((bitmapSize.Width != 0 && bitmapSize.Height != 0) && ((isLargerThanControl && (bitmapSize.Width > Width || bitmapSize.Height > Height))
				|| (!isLargerThanControl && (bitmapSize.Width < Width || bitmapSize.Height < Height))));

			Invalidate();
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
