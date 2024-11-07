using Svg;

namespace ACadSvgStudio {

	public partial class SvgViewerUserControl : UserControl {

		private Svg.SvgDocument _svgDocument;

		private int _x = 0;
		private int _y = 0;

		private bool _mouseDown = false;
		private Point _mousePos;


		public SvgViewerUserControl()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
		}


		public void LoadSvgContent(string content)
		{
			try
			{
				_svgDocument = Svg.SvgDocument.FromSvg<SvgDocument>(content);

				_x = 0;
				_y = 0;

				using (Bitmap bitmap = getBitmap())
				{
					_x = 200;
					_y = (Height / 2) - (bitmap.Height / 2);
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
			_svgDocument.ViewBox = new SvgViewBox(-500, -500, 1500, 1500);

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


		private void centerToFitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (Bitmap bitmap = getBitmap())
			{
				_x = (Width / 2) - (bitmap.Width / 2);
				_y = (Height / 2) - (bitmap.Height / 2);
			}

			Invalidate();
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
	}
}
