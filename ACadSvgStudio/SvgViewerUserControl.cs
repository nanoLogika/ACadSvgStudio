using Svg;
using Svg.Transforms;

namespace ACadSvgStudio {

	public partial class SvgViewerUserControl : UserControl {

		private Svg.SvgDocument _svgDocument;

		private int _x = 0;
		private int _y = 0;
		private float _zoom = 1;

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
				_svgDocument = Svg.SvgDocument.FromSvg<SvgDocument>(content);
				if (_svgDocument.Transforms == null)
				{
					_svgDocument.Transforms = new SvgTransformCollection();
					_svgDocument.Transforms.Add(new SvgScale(1, 1));
				}

				_svgTransformCollection = (SvgTransformCollection)_svgDocument.Transforms.Clone();

				_x = 0;
				_y = 0;

				_zoom = 1;

				using (Bitmap bitmap = getBitmap())
				{
					if (bitmap == null)
					{
						return;
					}

					centerToFit();
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


		private Bitmap getBitmap()
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

			Bitmap bitmap = _svgDocument.Draw();
			return bitmap;
		}


		private void SvgViewerUserControl_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawString("SVG Viewer (Experimental)", this.Font, new SolidBrush(Color.Yellow), 0, 0);


			e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

			e.Graphics.DrawRectangle(new Pen(Color.Red), 0, 0, Width - 1, Height - 1);


			if (hasErrors())
			{
				e.Graphics.FillRectangle(new SolidBrush(Color.IndianRed), 0, 0, Width, Height);

				return;
			}


			using (Bitmap bitmap = getBitmap())
			{
				if (bitmap != null)
				{
					e.Graphics.DrawImage(bitmap, _x, _y);
				}
			}
		}


		private void centerToFit()
		{
			using (Bitmap bitmap = getBitmap())
			{
				if (bitmap == null)
				{
					return;
				}

				_x = (Width / 2) - (bitmap.Width / 2);
				_y = (Height / 2) - (bitmap.Height / 2);
			}

			Invalidate();
		}


		private void centerToFitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			centerToFit();
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
				_x = (_mousePos.X - (e.Location.X)) * -1;
				_y = (_mousePos.Y - (e.Location.Y)) * -1;

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

			_zoom += e.Delta * zoomFactor;

			Invalidate();
		}
	}
}
