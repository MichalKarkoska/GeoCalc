using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GeoCalc.Controls
{
    /// <summary>
    /// Vlastné tlačidlo so zaoblenými rohmi a hover efektom.
    /// </summary>
    public class RoundedButton : Button
    {
        // Farby
        private Color _normalColor = Color.FromArgb(60, 60, 70);
        private Color _hoverColor = Color.FromArgb(80, 80, 95);
        private Color _pressedColor = Color.FromArgb(100, 100, 120);
        private Color _selectedColor = Color.FromArgb(70, 130, 180);
        
        private bool _isHovered = false;
        private bool _isPressed = false;
        private bool _isSelected = false;
        private int _borderRadius = 12;

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
        /// Označuje či je tlačidlo vybrané (aktívne).
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("Či je tlačidlo vybrané (aktívne)")]
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; Invalidate(); }
        }

        /// <summary>
        /// Farba tlačidla v normálnom stave.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("Farba tlačidla v normálnom stave")]
        public Color NormalColor
        {
            get => _normalColor;
            set { _normalColor = value; Invalidate(); }
        }

        /// <summary>
        /// Farba pri výbere (aktívnom stave).
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("Farba tlačidla v aktívnom stave")]
        public Color SelectedColor
        {
            get => _selectedColor;
            set { _selectedColor = value; Invalidate(); }
        }

        public RoundedButton()
        {
            // Nastavenie základných vlastností
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = _normalColor;
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            Cursor = Cursors.Hand;
            Size = new Size(180, 45);
            
            // Dvojité bufferovanie pre plynulé vykresľovanie
            SetStyle(ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.UserPaint | 
                     ControlStyles.DoubleBuffer, true);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            _isPressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            _isPressed = true;
            Invalidate();
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            _isPressed = false;
            Invalidate();
            base.OnMouseUp(mevent);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Určenie farby pozadia
            Color bgColor;
            if (_isSelected)
                bgColor = _selectedColor;
            else if (_isPressed)
                bgColor = _pressedColor;
            else if (_isHovered)
                bgColor = _hoverColor;
            else
                bgColor = _normalColor;

            // Vytvorenie zaobleneho obdĺžnika
            GraphicsPath path = GetRoundedRectPath(new Rectangle(0, 0, Width - 1, Height - 1), _borderRadius);

            // Vyplnenie pozadia
            using (SolidBrush brush = new SolidBrush(bgColor))
            {
                g.FillPath(brush, path);
            }

            // Vykreslenie textu
            TextRenderer.DrawText(g, Text, Font, ClientRectangle, ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            path.Dispose();
        }

        /// <summary>
        /// Vytvorí GraphicsPath pre zaoblený obdĺžnik.
        /// </summary>
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