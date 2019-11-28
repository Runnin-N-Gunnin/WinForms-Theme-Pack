	

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Drawing.Drawing2D;
     
    /// <summary>
    /// Created by DoggyCollin
    /// HF link: http://www.hackforums.net/member.php?action=profile&uid=1278211
    /// </summary>
     
    internal static class FoxSettings
    {
        public static Pen BorderColor = new Pen(Color.FromArgb(49, 49, 49));
        public static SolidBrush TextColor = new SolidBrush(Color.FromArgb(249, 249, 249));
        public static SolidBrush TextColorDark = new SolidBrush(Color.FromArgb(66, 66, 66));
        public static SolidBrush FillheaderColor = new SolidBrush(Color.FromArgb(237, 237, 237));
        public static SolidBrush FillbodyColor = new SolidBrush(Color.FromArgb(250, 250, 250));
     
        public static SolidBrush UpColor = new SolidBrush(Color.FromArgb(255, 136, 83)); // Orange(255, 136, 83), Green(116, 214, 36), Blue(90, 160, 220), Red(232, 67, 67)
        public static SolidBrush DownColor = new SolidBrush(Color.FromArgb(UpColor.Color.R - 33, UpColor.Color.G - 33, UpColor.Color.B - 33));
     
        public static Font TextFont = new Font("Verdana", 8.0F);
    }
     
    internal class FoxTheme : Control
    {
        /// <summary>
        /// Gets or sets the visibility of the icon copied from the parent control.
        /// </summary>
        public bool ShowIcon { get { return _showIcon; } set { _showIcon = value; Invalidate(); } }
        private bool _showIcon;
     
        private Graphics G;
     
        private Pen borderColor;
        private SolidBrush textColor;
        private SolidBrush fillheaderColor;
        private SolidBrush fillbodyColor;
     
        public FoxTheme()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            borderColor = FoxSettings.BorderColor;
            textColor = FoxSettings.TextColor;
            fillheaderColor = FoxSettings.FillheaderColor;
            fillbodyColor = FoxSettings.FillbodyColor;
     
            //Set defaults
            Font = FoxSettings.TextFont;
            ForeColor = textColor.Color;
        }
     
        protected override void OnHandleCreated(EventArgs e)
        {
            Dock = DockStyle.Fill;
            if (Parent is Form)
            {
                Form tempWith1 = (Form)Parent;
                tempWith1.FormBorderStyle = 0;
                tempWith1.TransparencyKey = Color.Fuchsia;
                tempWith1.BackColor = fillheaderColor.Color;
            }
            base.OnHandleCreated(e);
        }
     
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (new Rectangle(Parent.Location.X, Parent.Location.Y, Width, 22).IntersectsWith(new Rectangle(MousePosition.X, MousePosition.Y, 1, 1)))
            {
                Capture = false;
                Message M = Message.Create(Parent.Handle, 161, new IntPtr(2), IntPtr.Zero);
                DefWndProc(ref M);
            }
            base.OnMouseDown(e);
        }
     
        protected override void OnPaintBackground(PaintEventArgs pevent) {  }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            var B = new Bitmap(Width, Height);
            using (var G = Graphics.FromImage(B))
            {
                G.DrawPath(new Pen(fillheaderColor.Color), RoundedRectangle.Create(0, 0, Width, Height, 4));
                G.FillPath(fillheaderColor, RoundedRectangle.Create(0, 0, Width, Height, 4));
     
                G.DrawString(Text, Font, new SolidBrush(Color.FromArgb(66, 66, 66)), new Point(5, 5));
     
                G.DrawPath(new Pen(fillbodyColor.Color), RoundedRectangle.Create(6, 22, Width - 13, Height - 28, 4));
                G.FillPath(fillbodyColor, RoundedRectangle.Create(6, 22, Width - 13, Height - 28, 4));
     
                G.DrawPath(new Pen(Color.LightGray), RoundedRectangle.Create(0, 0, Width - 1, Height - 1, 4));
     
                G.FillRectangle(new SolidBrush(Color.Fuchsia), new Rectangle(0, 0, 2, 2));
                G.FillRectangle(new SolidBrush(Color.Fuchsia), new Rectangle(Width - 2, 0, 2, 2));
                G.FillRectangle(new SolidBrush(Color.Fuchsia), new Rectangle(Width - 2, Height - 2, 2, 2));
                G.FillRectangle(new SolidBrush(Color.Fuchsia), new Rectangle(0, Height - 2, 2, 2));
     
                B.SetPixel(1, 1, fillheaderColor.Color);
                B.SetPixel(Width - 2, 1, fillheaderColor.Color);
                B.SetPixel(Width - 2, Height - 2, fillheaderColor.Color);
                B.SetPixel(1, Height - 2, fillheaderColor.Color);
     
                e.Graphics.DrawImage(B, 0, 0);
            }
     
            B.Dispose();
        }
    }
     
    internal class FoxButton : Control
    {
        private Graphics G;
        private bool isMouseDown;
     
        private Pen borderColor;
        private SolidBrush textColor;
        private SolidBrush fillColor;
        private SolidBrush downColor;
     
        private StringFormat stringformat;
        private Font textFont;
     
        public FoxButton()
        {
            borderColor = FoxSettings.BorderColor;
            textColor = FoxSettings.TextColor;
            fillColor = FoxSettings.UpColor;
            downColor = FoxSettings.DownColor;
     
            stringformat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            textFont = FoxSettings.TextFont;
     
            //Set defaults
            Font = FoxSettings.TextFont;
            ForeColor = textColor.Color;
        }
     
        protected override void OnMouseDown(MouseEventArgs e)
        {
            isMouseDown = true;
            Invalidate();
        }
     
        protected override void OnMouseUp(MouseEventArgs e)
        {
            isMouseDown = false;
            Invalidate();
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            G = e.Graphics;
            G.Clear(BackColor);
            G.SmoothingMode = SmoothingMode.HighQuality;
     
            SolidBrush currentBrush;
     
            if (isMouseDown)
                currentBrush = downColor;
            else
                currentBrush = fillColor;
     
            LinearGradientBrush linGrBrush = GraphicsHelper.CreateGradient(Width, Height, currentBrush);
     
            G.DrawPath(new Pen(currentBrush), RoundedRectangle.Create(0, 0, Width, Height, 4));
            G.FillPath(linGrBrush, RoundedRectangle.Create(0, 0, Width, Height, 4));
            G.DrawString(Text, textFont, textColor, new Point(Width / 2, Height / 2), stringformat);
        }
    }
     
    internal class FoxSystemButton : Control
    {
        private Graphics G;
        private bool isMouseDown;
     
        private Pen borderColor;
        private SolidBrush textColor;
        private SolidBrush fillColor;
        private SolidBrush downColor;
        public SolidBrush upColor;
     
        private StringFormat stringformat;
        private Font textFont;
     
        public enum BType
        {
            Close,
            Minimize
        }
     
        public BType ButtonType { get; set; }
     
        public FoxSystemButton()
        {
            borderColor = FoxSettings.BorderColor;
            textColor = FoxSettings.TextColor;
            fillColor = FoxSettings.FillheaderColor;
            downColor = FoxSettings.DownColor;
            upColor = FoxSettings.UpColor;
     
            stringformat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            textFont = FoxSettings.TextFont;
     
            //Set defaults
            Font = FoxSettings.TextFont;
            ForeColor = textColor.Color;
            ButtonType = BType.Close;
            Size = new Size(14, 14);
        }
     
        protected override void OnMouseDown(MouseEventArgs e)
        {
            isMouseDown = true;
            Invalidate();
        }
     
        protected override void OnMouseUp(MouseEventArgs e)
        {
            isMouseDown = false;
            Invalidate();
     
            var form = FindForm();
     
            if (this.ClientRectangle.Contains(e.Location))
            {
                if (ButtonType == BType.Close)
                    form.Close();
                else if (ButtonType == BType.Minimize)
                    form.WindowState = FormWindowState.Minimized;
            }
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            G = e.Graphics;
            G.Clear(BackColor);
            G.SmoothingMode = SmoothingMode.HighQuality;
     
            SolidBrush currentBrush;
     
            if (isMouseDown)
                currentBrush = downColor;
            else
                currentBrush = upColor;
     
            G.DrawRectangle(new Pen(fillColor.Color), new Rectangle(0, 0, 14, 14));
            G.FillRectangle(fillColor, new Rectangle(0, 0, 14, 14));
     
            if (ButtonType == BType.Close)
            {
                G.DrawLine(new Pen(currentBrush, 1.6f), 2, 2, 10, 10);
                G.DrawLine(new Pen(currentBrush, 1.6f), 10, 2, 2, 10);
            }
            else if (ButtonType == BType.Minimize)
            {
                G.DrawLine(new Pen(currentBrush, 2f), 2, 10, 10, 10);
            }
        }
     
        public static void DrawRoundedRectangle(Graphics g, Rectangle r, int d, Pen p)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
     
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.X + r.Width - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Y + r.Height - d, d, d, 90, 90);
            gp.AddLine(r.X, r.Y + r.Height - d, r.X, r.Y + d / 2);
     
            g.DrawPath(p, gp);
        }
    }
     
    internal class FoxProgressBar : Control
    {
        /// <summary>
        /// Value in percentage.
        /// </summary>
        public float Value { get { return _value; } set { _value = value; Invalidate(); } }
        private float _value;
     
        /// <summary>
        /// Sets or gets the visibility setting for the percentage value.
        /// </summary>
        public bool ShowPercentage { get; set; }
     
        private Graphics G;
     
        private Pen borderColor;
        private SolidBrush textColor;
        private SolidBrush fillColor;
        private SolidBrush bodyColor;
        private SolidBrush downColor;
        private SolidBrush upColor;
     
        private StringFormat stringformat;
        private Font textFont;
     
        public FoxProgressBar()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
     
            borderColor = FoxSettings.BorderColor;
            textColor = FoxSettings.TextColor;
            fillColor = FoxSettings.FillheaderColor;
            bodyColor = FoxSettings.FillbodyColor;
            downColor = FoxSettings.DownColor;
            upColor = FoxSettings.UpColor;
     
            stringformat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            textFont = FoxSettings.TextFont;
            ShowPercentage = true;
     
            //Set defaults
            Font = FoxSettings.TextFont;
            ForeColor = textColor.Color;
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            G = e.Graphics;
            G.Clear(bodyColor.Color);
            G.SmoothingMode = SmoothingMode.HighQuality;
     
            float percent = ((float)Width / 100) * Value;
     
            LinearGradientBrush linGrBrush = GraphicsHelper.CreateGradient(Width, Height, upColor);
     
            G.FillPath(fillColor, RoundedRectangle.Create(0, 0, Width - 1, Height - 1, 4));
     
            if ((int)percent <= 0)
                G.FillPath(fillColor, RoundedRectangle.Create(0, 1, (int)percent, Height - 2, 4));
            else
                G.FillPath(linGrBrush, RoundedRectangle.Create(0, 1, (int)percent, Height - 2, 4));
     
            G.DrawPath(new Pen(Color.LightGray), RoundedRectangle.Create(0, 0, Width - 1, Height - 1, 4));
     
     
            var myColor = Value > 50 ? textColor.Color : Color.FromArgb(textColor.Color.ToArgb() ^ 0xffffff);
            if (ShowPercentage)
                G.DrawString(string.Format("{0}%", Value), textFont, new SolidBrush(myColor), new Point((Width + 2) / 2, (Height + 2) / 2), stringformat);
        }
    }
     
    internal class FoxTabControl : TabControl
    {
        private Pen borderColor;
        private SolidBrush textColor;
        private SolidBrush fillColor;
        private SolidBrush downColor;
        private SolidBrush upColor;
        private SolidBrush fillbodyColor;
     
        private Font textFont;
     
        public FoxTabControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            DoubleBuffered = true;
            SizeMode = TabSizeMode.Fixed;
     
            this.TabItemWidth = 120;
            this.TabItemHeight = 30;
            this.Alignment = TabAlignment.Left;
            ItemSize = new Size(TabItemHeight, TabItemWidth);
     
            borderColor = FoxSettings.BorderColor;
            textColor = FoxSettings.TextColor;
            fillColor = FoxSettings.FillheaderColor;
            downColor = FoxSettings.DownColor;
            upColor = FoxSettings.UpColor;
            fillbodyColor = FoxSettings.FillbodyColor;
     
            textFont = FoxSettings.TextFont;
     
            //Set defaults
            Font = FoxSettings.TextFont;
            ForeColor = textColor.Color;
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap B = new Bitmap(Width, Height);
     
            if (Alignment == TabAlignment.Top)
                DrawTopTabControl(Graphics.FromImage(B));
            else if (Alignment == TabAlignment.Right)
                DrawRightTabControl(Graphics.FromImage(B));
            else if (Alignment == TabAlignment.Bottom)
                DrawBottomTabControl(Graphics.FromImage(B));
            else if (Alignment == TabAlignment.Left)
                DrawLeftTabControl(Graphics.FromImage(B));
     
            e.Graphics.DrawImage((Image)B.Clone(), 0, 4);
            B.Dispose();
        }
     
        private void DrawTopTabControl(Graphics G)
        {
            G.Clear(fillbodyColor.Color);
     
            for (int i = 0; i <= TabCount - 1; i++)
            {
                Rectangle x2 = new Rectangle(new Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2), new Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1));
     
                G.FillRectangle(textColor, new Rectangle(new Point(x2.Location.X, x2.Location.Y), new Size(GetTabRect(i).Size.Width, GetTabRect(i).Size.Height)));
                if (i == SelectedIndex)
                {
                    G.FillPath(upColor, RoundedRectangle.Create(new Rectangle(new Point(x2.Location.X, GetTabRect(i).Size.Height - 5), new Size(GetTabRect(i).Size.Width, 5)), 2));
                    G.DrawPath(new Pen(upColor.Color), RoundedRectangle.Create(new Rectangle(new Point(x2.Location.X, GetTabRect(i).Size.Height - 5), new Size(GetTabRect(i).Size.Width, 3)), 2));
                }
     
                G.DrawString(TabPages[i].Text, Font, FoxSettings.TextColorDark, new Rectangle(x2.Location.X, x2.Location.Y, x2.Width, x2.Height), new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                });
            }
     
            G.Dispose();
        }
     
        private void DrawRightTabControl(Graphics G)
        {
            G.Clear(fillbodyColor.Color);
     
            for (int i = 0; i <= TabCount - 1; i++)
            {
                Rectangle x2 = new Rectangle(new Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2), new Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1));
     
                G.FillRectangle(textColor, new Rectangle(new Point(x2.Location.X, x2.Location.Y), new Size(GetTabRect(i).Size.Width, GetTabRect(i).Size.Height)));
                if (i == SelectedIndex)
                {
                    G.FillPath(upColor, RoundedRectangle.Create(new Rectangle(new Point(x2.Location.X, x2.Location.Y), new Size(5, GetTabRect(i).Size.Height)), 2));
                    G.DrawPath(new Pen(upColor.Color), RoundedRectangle.Create(new Rectangle(new Point(x2.Location.X, x2.Location.Y), new Size(3, GetTabRect(i).Size.Height)), 2));
                }
     
                G.DrawString(TabPages[i].Text, Font, new SolidBrush(Color.FromArgb(66, 66, 66)), new Rectangle(x2.Location.X + 10, x2.Location.Y, x2.Width, x2.Height), new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near
                });
            }
     
            G.Dispose();
        }
     
        private void DrawBottomTabControl(Graphics G)
        {
            G.Clear(fillbodyColor.Color);
     
            for (int i = 0; i <= TabCount - 1; i++)
            {
                Rectangle x2 = new Rectangle(new Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2), new Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1));
     
                G.FillRectangle(textColor, new Rectangle(new Point(x2.Location.X, x2.Location.Y), new Size(GetTabRect(i).Size.Width, GetTabRect(i).Size.Height)));
                if (i == SelectedIndex)
                {
                    G.FillPath(upColor, RoundedRectangle.Create(new Rectangle(new Point(x2.Location.X, x2.Location.Y - 5), new Size(GetTabRect(i).Size.Width, 5)), 2));
                    G.DrawPath(new Pen(upColor.Color), RoundedRectangle.Create(new Rectangle(new Point(x2.Location.X, x2.Location.Y - 5), new Size(GetTabRect(i).Size.Width, 3)), 2));
                }
     
                G.DrawString(TabPages[i].Text, Font, new SolidBrush(Color.FromArgb(66, 66, 66)), new Rectangle(x2.Location.X, x2.Location.Y, x2.Width, x2.Height), new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                });
            }
     
            G.Dispose();
        }
     
        private void DrawLeftTabControl(Graphics G)
        {  
            G.Clear(fillbodyColor.Color);
     
            for (int i = 0; i <= TabCount - 1; i++)
            {
                Rectangle x2 = new Rectangle(new Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2), new Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1));
     
                G.FillRectangle(textColor, new Rectangle(new Point(x2.Location.X, x2.Location.Y), new Size(GetTabRect(i).Size.Width, GetTabRect(i).Size.Height)));
                if (i == SelectedIndex)
                {
                    G.FillPath(upColor, RoundedRectangle.Create(new Rectangle(new Point(GetTabRect(i).Size.Width - 5, x2.Location.Y), new Size(5, GetTabRect(i).Size.Height)), 2));
                    G.DrawPath(new Pen(upColor.Color), RoundedRectangle.Create(new Rectangle(new Point(GetTabRect(i).Size.Width - 5, x2.Location.Y), new Size(3, GetTabRect(i).Size.Height)), 2));
                }
     
                G.DrawString(TabPages[i].Text, Font, new SolidBrush(Color.FromArgb(66, 66, 66)), new Rectangle(x2.Location.X, x2.Location.Y, x2.Width - 20, x2.Height), new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Far
                });
            }
     
            G.Dispose();
        }
     
        private int tabItemWidth;
        public int TabItemWidth { get { return tabItemWidth; } set { tabItemWidth = value; RefreshItemSize(); } }
     
        private int tabItemHeight;
        public int TabItemHeight { get { return tabItemHeight; } set { tabItemHeight = value; RefreshItemSize(); } }
     
        public void RefreshItemSize()
        {
            if (Alignment == TabAlignment.Top || Alignment == TabAlignment.Bottom)
                ItemSize = new Size(TabItemWidth, TabItemHeight);
            else if (Alignment == TabAlignment.Left || Alignment == TabAlignment.Right)
                ItemSize = new Size(TabItemHeight, TabItemWidth);
     
            Invalidate();
        }
     
        public override Color BackColor
        {
            get
            {
                return fillbodyColor.Color;
            }
            set
            {
                base.BackColor = value;
            }
        }
     
        public TabAlignment TabItemAlignment
        {
            get
            {
                return base.Alignment;
            }
            set
            {
                base.Alignment = value;
                RefreshItemSize();
            }
        }
     
        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.BackColor = fillbodyColor.Color;
            e.Control.Invalidate();
        }
     
    }
     
    internal class FoxCheckBox : Control
    {
        public bool Checked { get; set; }
        public CheckStyleType CheckStyle { get; set; }
     
        private Graphics G;
     
        private Pen borderColor;
        private SolidBrush textColor;
        private SolidBrush fillColor;
        private SolidBrush downColor;
        private SolidBrush upColor;
     
        private StringFormat stringformat;
        private Font textFont;
     
        public FoxCheckBox()
        {
            borderColor = new Pen(Color.LightGray);
            textColor = FoxSettings.TextColorDark;
            fillColor = FoxSettings.FillheaderColor;
            downColor = FoxSettings.DownColor;
            upColor = FoxSettings.UpColor;
     
            stringformat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            textFont = FoxSettings.TextFont;
     
            //Set defaults
            Font = FoxSettings.TextFont;
            ForeColor = textColor.Color;
            Checked = true;
            CheckStyle = CheckStyleType.Square;
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            G = e.Graphics;
     
            switch (CheckStyle)
            {
                case CheckStyleType.Square:
                    G.DrawRectangle(borderColor, new Rectangle(0, 0, 12, 12));
                    G.DrawString(Text, textFont, textColor, 14, 0);
     
                    if (Checked)
                        G.FillRectangle(GraphicsHelper.CreateGradient(2, 9, upColor), new Rectangle(2, 2, 9, 9));
                    break;
                case CheckStyleType.Round:
                    G.DrawEllipse(borderColor, new Rectangle(0, 0, 13, 13));
                    G.DrawString(Text, textFont, textColor, 14, 0);
     
                    if (Checked)
                        G.FillEllipse(GraphicsHelper.CreateGradient(2, 9, upColor), new Rectangle(2, 2, 9, 9));
                    break;
                case CheckStyleType.Checkmark:
                    G.DrawRectangle(borderColor, new Rectangle(0, 0, 12, 12));
                    G.DrawString(Text, textFont, textColor, 14, 0);
     
                    if (Checked)
                    {
                        G.DrawString("\u221A", new Font("serif", 6, FontStyle.Bold), upColor, 3, 3);
                        //G.DrawLine(new Pen(downColor.Color, 2), 3, 5, 7, 9);
                        //G.DrawLine(new Pen(downColor.Color, 2), 6, 9, 9, 3);
                    }
                    break;
                default:
                    break;
            }
        }
     
        protected override void OnMouseClick(MouseEventArgs e)
        {
            Checked = !Checked;
            Invalidate();
        }
     
        public enum CheckStyleType
        {
            Square = 0,
            Round = 1,
            Checkmark = 2
        }
    }
     
    internal class FoxChartControl : Control
    {
        private Pen borderColor;
        private SolidBrush textColor;
        private SolidBrush fillheaderColor;
        private SolidBrush fillbodyColor;
     
        private Color colorStripes;
        private Color colorValues;
        private Color colorText;
     
        public float[] Values { get; set; }
     
        public float? ValueMin { get; set; }
        public float? ValueMax { get; set; }
        public float ValueStripes { get; set; }
     
        public float SizeStripes { get; set; }
        public float SizeValues { get; set; }
     
        public Font FontText { get; set; }
     
        public FoxChartControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            borderColor = FoxSettings.BorderColor;
            textColor = FoxSettings.TextColor;
            fillheaderColor = FoxSettings.FillheaderColor;
            fillbodyColor = FoxSettings.FillbodyColor;
     
            //Set defaults
            Font = FoxSettings.TextFont;
            ForeColor = textColor.Color;
            BackColor = fillbodyColor.Color;
     
            Values = new float[] { };
     
            ValueMax = null;
            ValueMin = null;
            ValueStripes = 10;
     
            SizeStripes = 1;
            SizeValues = 4;
     
            colorStripes = Color.LightGray;
            colorValues = FoxSettings.UpColor.Color;
            colorText = Color.Black;
     
            FontText = FoxSettings.TextFont;
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
     
            float max = ValueMax ?? (Values.Count() > 0 ? Values.Max() : 0);
            float min = ValueMin ?? (Values.Count() > 0 ? Values.Min() : 0);
     
            float averagegeCount = (max - min) / ValueStripes;
            float currentCount = max;
     
            G.SmoothingMode = SmoothingMode.HighQuality;
     
            float startWidthPosition = ValueMax == null
                ? GraphicsHelper.GetTextWidthInPixels(Values.Select(a => a.ToString()).OrderByDescending(a => a.Length).FirstOrDefault(), FontText)
                : GraphicsHelper.GetTextWidthInPixels(ValueMax.ToString(), FontText);
     
            float chartHeight = Height - 17;
            float itemHeight = 10;
     
            G.FillPath(fillheaderColor, RoundedRectangle.Create(0, 0, Width - 1, Height - 1, 4));
     
            for (int i = 0; i <= ValueStripes; i++)
            {
                G.DrawLine(new Pen(ColorStripes, SizeStripes), startWidthPosition, itemHeight, Width, itemHeight);
                G.DrawString((Math.Floor(currentCount)).ToString(), FontText, new SolidBrush(ColorText), 0, itemHeight - 7);
                itemHeight += chartHeight / ValueStripes;
                currentCount -= averagegeCount;
            }
     
            float lastItemHeight = (chartHeight - (chartHeight / (max - min) * (Values.FirstOrDefault() - min))) + 10;
            float lastItemWidth = startWidthPosition;
            float itemWidth = startWidthPosition;
     
            foreach (var value in Values)
            {
                var valueMin = value - min;
     
                var item = chartHeight - (chartHeight / (max - min) * valueMin);
                item = item == chartHeight ? chartHeight + 10 : item + 10;
     
                var currentBrush = FoxSettings.UpColor;
                G.DrawLine(new Pen(currentBrush.Color, SizeValues), lastItemWidth, lastItemHeight, itemWidth, item);
                G.FillEllipse(currentBrush, itemWidth - (SizeValues / 2), item - (SizeValues / 2), SizeValues, SizeValues);
     
                lastItemHeight = item;
                lastItemWidth = itemWidth;
                itemWidth += Width / Values.Count();
            }
        }
     
        public Color ColorStripes
        {
            get
            {
                return this.colorStripes;
            }
            set
            {
                this.colorStripes = value;
            }
        }
     
        public Color ColorValues
        {
            get
            {
                return this.colorValues;
            }
            set
            {
                this.colorValues = value;
            }
        }
     
        public Color ColorText
        {
            get
            {
                return this.colorText;
            }
            set
            {
                this.colorText = value;
            }
        }
    }
     
    internal class FoxLabel : Control
    {
        private SolidBrush textColor;
        private Font textFont;
     
        public FoxLabel()
        {
            textColor = FoxSettings.TextColorDark;
            textFont = FoxSettings.TextFont;
     
            //Set defaults
            Font = FoxSettings.TextFont;
            ForeColor = textColor.Color;
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            G.DrawString(Text, textFont, textColor, 0, 0);
        }
    }
     
     
    public abstract class RoundedRectangle
    {
        public enum RectangleCorners
        {
            None = 0, TopLeft = 1, TopRight = 2, BottomLeft = 4, BottomRight = 8,
            All = TopLeft | TopRight | BottomLeft | BottomRight
        }
     
        public static GraphicsPath Create(int x, int y, int width, int height,
                                          int radius, RectangleCorners corners)
        {
            int xw = x + width;
            int yh = y + height;
            int xwr = xw - radius;
            int yhr = yh - radius;
            int xr = x + radius;
            int yr = y + radius;
            int r2 = radius * 2;
            int xwr2 = xw - r2 - 1;
            int yhr2 = yh - r2 - 1;
     
            GraphicsPath p = new GraphicsPath();
            p.StartFigure();
     
            //Top Left Corner
            if ((RectangleCorners.TopLeft & corners) == RectangleCorners.TopLeft)
            {
                p.AddArc(x, y, r2, r2, 180, 90);
            }
            else
            {
                p.AddLine(x, yr, x, y);
                p.AddLine(x, y, xr, y);
            }
     
            //Top Edge
            p.AddLine(xr, y, xwr, y);
     
            //Top Right Corner
            if ((RectangleCorners.TopRight & corners) == RectangleCorners.TopRight)
            {
                p.AddArc(xwr2, y, r2, r2, 270, 90);
            }
            else
            {
                p.AddLine(xwr, y, xw, y);
                p.AddLine(xw, y, xw, yr);
            }
     
            //Right Edge
            p.AddLine(xw, yr, xw, yhr);
     
            //Bottom Right Corner
            if ((RectangleCorners.BottomRight & corners) == RectangleCorners.BottomRight)
            {
                p.AddArc(xwr2, yhr2, r2, r2, 0, 90);
            }
            else
            {
                p.AddLine(xw, yhr, xw, yh);
                p.AddLine(xw, yh, xwr, yh);
            }
     
            //Bottom Edge
            p.AddLine(xwr, yh, xr, yh);
     
            //Bottom Left Corner
            if ((RectangleCorners.BottomLeft & corners) == RectangleCorners.BottomLeft)
            {
                p.AddArc(x, yhr2, r2, r2, 90, 90);
            }
            else
            {
                p.AddLine(xr, yh, x, yh);
                p.AddLine(x, yh, x, yhr);
            }
     
            //Left Edge
            p.AddLine(x, yhr, x, yr);
     
            p.CloseFigure();
            return p;
        }
     
        public static GraphicsPath Create(Rectangle rect, int radius, RectangleCorners c)
        { return Create(rect.X, rect.Y, rect.Width, rect.Height, radius, c); }
     
        public static GraphicsPath Create(int x, int y, int width, int height, int radius)
        { return Create(x, y, width, height, radius, RectangleCorners.All); }
     
        public static GraphicsPath Create(Rectangle rect, int radius)
        { return Create(rect.X, rect.Y, rect.Width, rect.Height, radius); }
     
        public static GraphicsPath Create(int x, int y, int width, int height)
        { return Create(x, y, width, height, 5); }
     
        public static GraphicsPath Create(Rectangle rect)
        { return Create(rect.X, rect.Y, rect.Width, rect.Height); }
    }
     
    public static class GraphicsHelper
    {
        public static float GetTextWidthInPixels(string text, Font font)
        {
            var size = TextRenderer.MeasureText(text, font);
            return size.Width;
        }
     
        public static LinearGradientBrush CreateGradient(float width, float height, SolidBrush brush)
        {
            var linGrBrush = new LinearGradientBrush(
                new RectangleF(width, height, width, height),
                Color.FromArgb(255, brush.Color.R - 33, brush.Color.G - 33, brush.Color.B - 33),
                brush.Color, 90F);
     
            float[] relativeIntensities = { 0.0f, 0.5f, 1.0f };
            float[] relativePositions = { 0.0f, 0.2f, 1.0f };
     
            Blend blend = new Blend();
            blend.Factors = relativeIntensities;
            blend.Positions = relativePositions;
            linGrBrush.Blend = blend;
     
            return linGrBrush;
        }
    }
     
    // Created by DoggyCollin