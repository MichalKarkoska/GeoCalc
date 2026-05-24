using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GeoCalc.Controls
{
    /// <summary>
    /// Panel so zaoblenými rohmi pre moderný vzhľad.
    /// </summary>
    public class RoundedPanel : Panel
    {
        private int _borderRadius = 15;
        private Color _borderColor = Color.FromArgb(70, 70, 80);
        private int _borderWidth = 1;

        /// <summary>
        /// Polomer zaoblenia rohov.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("Polomer zaoblenia rohov")]
        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; Invalidate(); }
        }

        /// <summary>
        /// Farba okraja.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("Farba okraja panelu")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        /// <summary>
        /// Šírka okraja.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("Šírka okraja v pixeloch")]
        public int BorderWidth
        {
            get => _borderWidth;
            set { _borderWidth = value; Invalidate(); }
        }

        public RoundedPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            GraphicsPath path = GetRoundedRectPath(rect, _borderRadius);

            // Vyplnenie pozadia
            using (SolidBrush brush = new SolidBrush(BackColor))
            {
                g.FillPath(brush, path);
            }

            // Vykreslenie okraja
            if (_borderWidth > 0)
            {
                using (Pen pen = new Pen(_borderColor, _borderWidth))
                {
                    g.DrawPath(pen, path);
                }
            }

            path.Dispose();
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
