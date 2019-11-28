using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

/*-------------------------*/
//Theme:          EasyColour
//Creator:        Euras
//Version:        1.0
//Created:        02/08/14
//Website:        eurashd.com
/*-------------------------*/

namespace EasyColour
{
    class EC_Theme : ContainerControl
    {
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        protected override sealed void OnHandleCreated(EventArgs e)
        {
            base.Dock = DockStyle.Fill;
            ForeColor = Color.White;
            base.OnHandleCreated(e);
        }

        private bool _IsParentForm;
        protected bool IsParentForm
        {
            get { return _IsParentForm; }
        }

        private bool _Transparent;
        public bool Transparent
        {
            get { return _Transparent; }
            set
            {
                _Transparent = value;
                if (!(IsHandleCreated))
                    return;

                SetStyle(ControlStyles.Opaque, !value);
                SetStyle(ControlStyles.SupportsTransparentBackColor, value);
                Invalidate();
            }
        }

        protected override sealed void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent == null)
                return;
            _IsParentForm = Parent is Form;

            if (_IsParentForm)
            {
                ParentForm.FormBorderStyle = _BorderStyle;
            }
            Parent.BackColor = BackColor;
        }

        private FormBorderStyle _BorderStyle;
        public FormBorderStyle BorderStyle
        {
            get
            {
                if (_IsParentForm)
                    return ParentForm.FormBorderStyle;
                else
                    return _BorderStyle;
            }
            set
            {
                _BorderStyle = value;
                if (_IsParentForm)
                {
                    ParentForm.FormBorderStyle = value;
                }
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Parent.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
            base.OnMouseDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            g.FillRectangle(new SolidBrush(Color.FromArgb(75, 0, 0, 0)), new Rectangle(-1, -1, Width + 1, 25));
            Image icon = Properties.Resources.icon;
            Size iSize = new Size(16, 16);
            g.DrawImage(icon, 5, 5, 16, 16);
            g.DrawString(Text, Font, new SolidBrush(ForeColor), new Point(25, 5), StringFormat.GenericDefault);

            g.Dispose();
        }
    }

    class EC_Button : Control
    {
        bool isHover;
        string _ButtonText;
        Image _Image;
        Size _ImageSize;

        // Initilize Button
        public EC_Button()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;
        }

        // Mouse Hover Events
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHover = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isHover = false;
            Invalidate();
        }

        // Button Text Property
        [Category("Appearance")]
        public override string Text
        {
            get { return _ButtonText; }
            set { _ButtonText = value; }
        }

        // Button Image Property
        [Category("Appearance")]
        public Image Image
        {
            get { return _Image; }
            set { _Image = value; }
        }

        // Image Size Property
        [Category("Appearance")]
        public Size ImageSize
        {
            get { return _ImageSize; }
            set { _ImageSize = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            SizeF s = g.MeasureString(_ButtonText, Font);
            int x = ClientSize.Width;
            int y = ClientSize.Height;

            // Top / Bottom Fills
            g.FillRectangle(new SolidBrush(Color.FromArgb(120, 0, 0, 0)), new Rectangle(0, y / 2, x - 1, y - 1));
            g.FillRectangle(new SolidBrush(Color.FromArgb(80, 0, 0, 0)), new Rectangle(0, 0, x - 1, y / 2));

            if (Text == null)
            {
                _ButtonText = Name;
            }

            // Draw Button Text
            g.DrawString(_ButtonText, Font, new SolidBrush(Color.White), (x - s.Width) / 2, (y - s.Height) / 2);

            // Apply Button Image
            if (_Image != null)
            {
                int imgX = _ImageSize.Width;
                int imgY = _ImageSize.Height;

                if (_ImageSize != null)
                {
                    g.DrawImage(_Image, 5, (y - imgY) / 2, imgX, imgY);
                }
                else
                {
                    g.DrawImage(_Image, 5, (y - imgY) / 2, 16, 16);
                }
            }

            // Button Hover Overlay
            if (isHover)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(25, 0, 0, 0)), new Rectangle(0, 0, x, y));
            }

            g.Dispose();
        }
    }

    class EC_CheckBox : Control
    {
        bool _Checked;

        // Initialize Checkbox
        public EC_CheckBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Size = new Size(20, 20);
            MaximumSize = Size;
            MinimumSize = Size;
            Cursor = Cursors.Hand;
        }

        // "Check" Events
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (_Checked)
            {
                _Checked = false;
            }
            else if (!_Checked)
            {
                _Checked = true;
            }
            Invalidate();
            base.OnMouseClick(e);
        }

        public bool Checked
        {
            get { return _Checked; }
            set { _Checked = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            int x = ClientSize.Width;
            int y = ClientSize.Height;

            // Fill Outer / Inner Box
            g.FillEllipse(new SolidBrush(Color.FromArgb(25, 0, 0, 0)), new Rectangle(0, 0, x - 1, y - 1));
            g.FillEllipse(new SolidBrush(Color.FromArgb(75, 255, 255, 255)), new Rectangle(3, 3, x - 7, y - 7));

            // Apply Check Overlay
            if (_Checked)
            {
                g.FillEllipse(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), new RectangleF(5, 5, (x / 2) - 1, (y / 2) - 1));
            }

            g.Dispose();
        }
    }

    class EC_Panel : Panel
    {
        public string _PanelText;

        // Text To Display On Panel
        [Category("Appearance")]
        public string PanelText
        {
            get { return _PanelText; }
            set { _PanelText = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            int x = ClientSize.Width;
            int y = ClientSize.Height;

            // Top Bar Fill
            g.FillRectangle(new SolidBrush(Color.FromArgb(25, 0, 0, 0)), new Rectangle(0, 0, x, 25));

            if (PanelText == null)
            {
                _PanelText = Name;
            }

            // Main Fill & Draw Text
            g.FillRectangle(new SolidBrush(Color.FromArgb(50, 0, 0, 0)), new Rectangle(0, 0, x, y));
            g.DrawString(_PanelText, Font, new SolidBrush(Color.White), new Point(5, 5));

            g.Dispose();
        }
    }

    class EC_Tabs : TabControl
    {
        // Initialize
        public EC_Tabs()
        {
            DrawMode = TabDrawMode.OwnerDrawFixed;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            Dock = DockStyle.Bottom;
            SizeMode = TabSizeMode.Normal;
            ItemSize = new Size(100, 20);
        }

        // Tabs Back Color Property
        private Color m_Backcolor = Color.Empty;
        [Browsable(true)]
        public override Color BackColor
        {
            get
            {
                if (m_Backcolor.Equals(Color.Empty))
                {
                    if (Parent == null)
                        return Control.DefaultBackColor;
                    else
                        return Parent.BackColor;
                }
                return m_Backcolor;
            }
            set
            {
                if (m_Backcolor.Equals(value)) return;
                m_Backcolor = value;
                Invalidate();
                base.OnBackColorChanged(EventArgs.Empty);
            }
        }

        public bool ShouldSerializeBackColor()
        {
            return !m_Backcolor.Equals(Color.Empty);
        }

        public override void ResetBackColor()
        {
            m_Backcolor = Color.Empty;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(BackColor);

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Apply Colouring To Each Tab Page
            for (int i = 0; i <= TabPages.Count - 1; i++)
            {
                Rectangle rect = GetTabRect(i);
                Rectangle r = new Rectangle(GetTabRect(i).X + 10, GetTabRect(i).Y + 3, 100, 20);
                if (SelectedTab == TabPages[i])
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(75, 0, 0, 0)), rect);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(25, 0, 0, 0)), rect);
                }

                // Draw Tab Page Text
                g.DrawString(TabPages[i].Text, Font, new SolidBrush(Color.White), (Rectangle)r);
                TabPages[i].UseVisualStyleBackColor = false;
            }
        }
    }

    class EC_Notification : UserControl
    {
        string _Text;

        // Initialize
        public EC_Notification()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Size = new Size(200, 25);
        }

        // Notification Property
        [Category("Appearance")]
        public string Notification
        {
            get { return _Text; }
            set { _Text = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            SizeF s = g.MeasureString(_Text, Font);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            int x = ClientSize.Width;
            int y = ClientSize.Height;

            // Notification Shadow
            g.FillRectangle(new SolidBrush(Color.FromArgb(35, Color.Black)), new Rectangle(5, 5, x, y));
            // Notification Fill
            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, x - 5, y - 5));
            // Notification Text
            g.DrawString(_Text, Font, new SolidBrush(Color.Black), new PointF(5, ((y - 5) - s.Height) / 2));
        }
    }

    class EC_ProgressBar : Control
    {
        int max = 100;
        int min = 0;
        int val = 0;

        // Initialize
        public EC_ProgressBar()
        {
            base.Size = new Size(150, 15);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.FromArgb(25, 0, 0, 0);
            ForeColor = Color.FromArgb(200, 255, 255, 255);
        }

        // Progress Bar Minimum Value
        public int Min
        {
            get { return min; }
            set
            {
                if (value < 0)
                {
                    min = 0;
                }
                if (value > max)
                {
                    min = value;
                    min = value;
                }
                if (value < min)
                {
                    value = min;
                }
                Invalidate();
            }
        }

        // Progress Bar Maximum Value
        public int Max
        {
            get { return max; }
            set
            {
                if (value < min)
                {
                    min = value;
                }
                max = value;
                if (value > max)
                {
                    value = max;
                }
                Invalidate();
            }
        }

        // Progress Bar Current Value
        public int Value
        {
            get { return val; }
            set
            {
                int oldValue = val;
                if (value < min)
                {
                    val = min;
                }
                else if (value > max)
                {
                    val = max;
                }
                else
                {
                    val = value;
                }

                float percent;

                Rectangle newValueRect = new Rectangle(ClientRectangle.X + 2, ClientRectangle.Y + 2, 
                    ClientRectangle.Width - 4, ClientRectangle.Height - 4);
                Rectangle oldValueRect = new Rectangle(ClientRectangle.X + 2, ClientRectangle.Y + 2, 
                    ClientRectangle.Width - 4, ClientRectangle.Height - 4);

                percent = (float)(val - min) / (float)(max - min);
                newValueRect.Width = (int)((float)newValueRect.Width * percent);

                percent = (float)(oldValue - min) / (float)(max - min);
                oldValueRect.Width = (int)((float)oldValueRect.Width * percent);

                Rectangle updateRect = new Rectangle();

                if (newValueRect.Width > oldValueRect.Width)
                {
                    updateRect.X = oldValueRect.Size.Width;
                    updateRect.Width = newValueRect.Width - oldValueRect.Width;
                }
                else
                {
                    updateRect.X = newValueRect.Size.Width;
                    updateRect.Width = oldValueRect.Width - newValueRect.Width;
                }

                updateRect.Height = Height;
                Invalidate(updateRect);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            Graphics g = pevent.Graphics;
            // Progress Bar Background Colour
            g.FillRectangle(new SolidBrush(BackColor), new Rectangle(0, 0, Width, Height));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            SolidBrush brush = new SolidBrush(ForeColor);
            float percent = (float)(val - min) / (float)(max - min);
            Rectangle rect = new Rectangle(2, 2, Width - 4, Height - 4);
            rect.Width = (int)((float)rect.Width * percent);
            
            // Progress Bar ForeColour
            g.FillRectangle(brush, rect);
        }
    }

    class EC_TextBox : RichTextBox
    {
        // Initialize
        public EC_TextBox()
        {
            BorderStyle = BorderStyle.None;
            Multiline = false;
            Size = new Size(Size.Width, 20);
            MaximumSize = new Size(int.MaxValue, Size.Height);
            MinimumSize = Size;
        }

        // PREVENT FLICKERING
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
        }

        private const int WM_PAINT = 15;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PAINT)
            {
                Invalidate();
                base.WndProc(ref m);
                using (Graphics g = Graphics.FromHwnd(Handle))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}