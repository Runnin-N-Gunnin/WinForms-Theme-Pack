	
// aevion theme convertered to c#
    using System;
    using System.Linq;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Drawing.Drawing2D;
     
    namespace Aevion_r2
    {
     
            public static class Helper
            {
                    public struct Fonts
                    {
                            public static Font Primary = new Font("Segoe UI", 9);
                            public static Font PrimaryBold = new Font("Segoe UI", 9, FontStyle.Bold);
                    }
     
                    public struct Colors
                    {
                            public static Color Foreground = Color.White;
                            public static Color Background = Color.FromArgb(48, 57, 65);
                    }
     
     
                    public enum MouseState : byte
                    {
                            None = 0,
                            Hover = 1,
                            Down = 2
                    }
     
                    public enum Direction : byte
                    {
                            Up = 0,
                            Down = 1,
                            Left = 3,
                            Right = 4
                    }
     
                    public static void RoundRect(Graphics g, Int32 x, Int32 y, Int32 Width, Int32 Height, Int32 Curve, Color c)
                    {
                            if (Curve <= 0) throw new ArgumentException("Curve must be Greater than 0.", "Curve");
     
                            var p = new Pen(c);
     
                            var BaseRect = new RectangleF(x, y, Width, Height);
                            var ArcRect = new RectangleF(BaseRect.Location, new SizeF(Curve, Curve));
     
                            g.DrawArc(p, ArcRect, 180, 90);
                            g.DrawLine(p, x + (Curve / 2), y, y + Width - (Curve / 2), y);
     
                            ArcRect.X = BaseRect.Right - Curve;
                            g.DrawArc(p, ArcRect, 270, 90);
                            g.DrawLine(p, x + Width, y + (Curve / 2), x + Width, y + Height - (Curve / 2));
     
                            ArcRect.Y = BaseRect.Bottom - Curve;
                            g.DrawArc(p, ArcRect, 0, 90);
                            g.DrawLine(p, x + (Curve / 2), y + Height, x + Width - (Curve / 2), y + Height);
     
                            ArcRect.X = BaseRect.Left;
                            g.DrawArc(p, ArcRect, 90, 30);
                            g.DrawLine(p, x, y + (Curve / 2), x, y + Height - (Curve / 2));
     
                            p.Dispose();
                    }
     
                    public static void DrawTriangle(Graphics g, Rectangle r, Direction d, Color c)
                    {
                            var w = r.Width/2;
                            var y = r.Height/2;
     
                            Point p0 = Point.Empty, p1 = Point.Empty, p2 = Point.Empty;
     
                            switch (d)
                            {
                                    case Direction.Up:
                                            p0 = new Point(r.Left + w, r.Top);
                                            p1 = new Point(r.Left, r.Bottom);
                                            p2 = new Point(r.Right, r.Bottom);
                                            break;
     
                                    case Direction.Down:
                                            p0 = new Point(r.Left + w, r.Bottom);
                                            p1 = new Point(r.Left, r.Top);
                                            p2 = new Point(r.Right, r.Top);
                                            break;
     
                                    case Direction.Left:
                                            p0 = new Point(r.Left, r.Top + y);
                                            p1 = new Point(r.Right, r.Top);
                                            p2 = new Point(r.Right, r.Bottom);
                                            break;
     
                                    case Direction.Right:
                                            p0 = new Point(r.Right, r.Top + y);
                                            p1 = new Point(r.Left, r.Bottom);
                                            p2 = new Point(r.Left, r.Top);
                                            break;
                            }
     
                            var s = new SolidBrush(c);
                            g.FillPolygon(s, new [] {p0, p1, p2});
     
                            MultiDispose(s);
                    }
     
                    public static void CenterString(Graphics g, String Text, Font Font, Color c, Rectangle rect, Boolean Shadow = false, Int32 yOffset = 1)
                    {
                            CenterString(g, Text, Font, new SolidBrush(c), rect, Shadow, yOffset);
                    }
     
                    public static void CenterString(Graphics g, String Text, Font Font, Brush b, Rectangle rect, Boolean Shadow = false, Int32 yOffset = 1)
                    {
                            var TextSize = g.MeasureString(Text, Font);
                            var x = rect.X + (rect.Width / 2) - (TextSize.Width / 2);
                            var y = rect.Y + (rect.Height / 2) - (TextSize.Height / 2) + yOffset;
     
                            if (Shadow)
                                    g.DrawString(Text, Font, Brushes.Black, x + 1, y + 1);
                            g.DrawString(Text, Font, b, x, y);
     
                            b.Dispose();
                    }
     
                    public static Single ValueToPercentage(Int32 Value, Int32 Maximum, Int32 Minimum)
                    {
                            return (Single)(Value - Minimum) / (Maximum - Minimum);
                    }
     
                    public static void MultiDispose(params IDisposable[] Disposables)
                    {
                            foreach (var Disposable in Disposables.Where(Disposable => Disposable != null))
                            {
                                    Disposable.Dispose();
                            }
                    }
            }
     
            sealed class AevionForm : Control
            {
                    public AevionForm()
                    {
                            DoubleBuffered = true;
                            Font = Helper.Fonts.Primary;
                            ForeColor = Helper.Colors.Foreground;
                            BackColor = Helper.Colors.Background;
                            Dock = DockStyle.Fill;
                    }
     
                    protected override void OnPaint(PaintEventArgs e)
                    {
                            base.OnPaint(e);
                            var g = e.Graphics;
                            g.SmoothingMode=SmoothingMode.HighQuality;
                            g.Clear(Helper.Colors.Background);
                    }
     
            }
     
            sealed class AevionButton : Button
            {
                    private Helper.MouseState State = Helper.MouseState.None;
                    private LinearGradientBrush l1, l2, l3 = null;
     
                    public enum Style
                    {
                            Default,
                            Green,
                            Red
                    }
     
                    private Style _ButtonStyle = Style.Default;
                    private Image _Icon;
                    private Boolean _ShowIcon = false;
     
                    public Style ButtonStyle
                    {
                            get { return _ButtonStyle; }
                            set { _ButtonStyle = value; Invalidate(); }
                    }
     
                    public Image Icon
                    {
                            get { return _Icon; }
                            set { _Icon = value; Invalidate(); }
                    }
     
                    public Boolean ShowIcon
                    {
                            get { return _ShowIcon; }
                            set { _ShowIcon = value; Invalidate(); }
                    }
     
                    public override string Text
                    {
                            get { return base.Text; }
                            set { base.Text = value; Invalidate(); }
                    }
     
                    public override Font Font
                    {
                            get { return base.Font; }
                            set { base.Font = value; Invalidate(); }
                    }
     
                    public AevionButton()
                    {
                            DoubleBuffered = true;
                            Font = Helper.Fonts.PrimaryBold;
                            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
                    }
     
                    protected override void OnMouseDown(MouseEventArgs mevent)
                    {
                            base.OnMouseDown(mevent);
                            State = Helper.MouseState.Down;
                            Invalidate();
                    }
     
                    protected override void OnMouseUp(MouseEventArgs mevent)
                    {
                            base.OnMouseUp(mevent);
                            State = Helper.MouseState.None;
                            Invalidate();
                    }
     
                    protected override void OnMouseEnter(EventArgs e)
                    {
                            base.OnMouseEnter(e);
                            State = Helper.MouseState.Hover;
                            Invalidate();
                    }
     
                    protected override void OnMouseLeave(EventArgs e)
                    {
                            base.OnMouseLeave(e);
                            State = Helper.MouseState.None;
                            Invalidate();
                    }
     
                    protected override void OnPaint(PaintEventArgs e)
                    {
                            base.OnPaint(e);
                            var g = e.Graphics;
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.Clear(Helper.Colors.Background);
     
                            switch (ButtonStyle)
                            {
                                    case Style.Default:
                                            l1 = new LinearGradientBrush(new Rectangle(0, 0, Width, Height + 35), Color.FromArgb(88, 105, 123), Color.Black, LinearGradientMode.Vertical);
                                            l2 = new LinearGradientBrush(new Rectangle(0, 0, Width, Height + 35), Color.FromArgb(108, 125, 143), Color.Black, LinearGradientMode.Vertical);
                                            l3 = new LinearGradientBrush(new Rectangle(0, 0, Width, Height + 35), Color.FromArgb(98, 115, 133), Color.Black, LinearGradientMode.Vertical);
                                            break;
                                    case Style.Red:
                                            l1 = new LinearGradientBrush(new Rectangle(0, 0, Width, Height + 35), Color.FromArgb(173, 83, 74), Color.Black, LinearGradientMode.Vertical);
                                            l2 = new LinearGradientBrush(new Rectangle(0, 0, Width, Height + 35), Color.FromArgb(193, 103, 94), Color.Black, LinearGradientMode.Vertical);
                                            l3 = new LinearGradientBrush(new Rectangle(0, 0, Width, Height + 35), Color.FromArgb(183, 93, 84), Color.Black, LinearGradientMode.Vertical);
                                            break;
                                    case Style.Green:
                                            l1 = new LinearGradientBrush(new Rectangle(0, 0, Width, Height + 35), Color.FromArgb(127, 177, 80), Color.Black, LinearGradientMode.Vertical);
                                            l2 = new LinearGradientBrush(new Rectangle(0, 0, Width, Height + 35), Color.FromArgb(157, 197, 100), Color.Black, LinearGradientMode.Vertical);
                                            l3 = new LinearGradientBrush(new Rectangle(0, 0, Width, Height + 35), Color.FromArgb(147, 187, 90), Color.Black, LinearGradientMode.Vertical);
                                            break;
                            }
     
                            switch (State)
                            {
                                    case Helper.MouseState.Down:
                                            g.FillRectangle(l1, new Rectangle(1, 1, Width - 2, Height - 2));
                                            break;
                                    case Helper.MouseState.Hover:
                                            g.FillRectangle(l2, new Rectangle(1, 1, Width - 2, Height - 2));
                                            break;
                                    case Helper.MouseState.None:
                                            g.FillRectangle(l3, new Rectangle(1, 1, Width - 2, Height - 2));
                                            break;
                            }
     
                            Helper.RoundRect(g, 0, 0, Width - 1, Height - 1, 3, Color.FromArgb(38, 38, 38));
                           
                            if (ShowIcon)
                                    g.DrawImage(Icon, new Point(Width / 8, Height / 2 - 8));
     
                            Helper.CenterString(g, Text, Font, Helper.Colors.Foreground, new Rectangle(0, 0, Width, Height));
                           
                    }
            }
     
            sealed class AevionRadioButton : Control
            {
                    public event EventHandler CheckedChanged = delegate { };
     
                    private Boolean _Checked;
     
                    public Boolean Checked
                    {
                            get { return _Checked; }
                            set {
                                    _Checked = value;
                                    Invalidate();
                                    CheckedChanged(this, new EventArgs());
                            }
                    }
     
                    public override string Text
                    {
                            get { return base.Text; }
                            set { base.Text = value; Invalidate(); }
                    }
     
                    public override Font Font
                    {
                            get { return base.Font; }
                            set { base.Font = value; Invalidate(); }
                    }
     
                    public AevionRadioButton()
                    {
                            DoubleBuffered = true;
                            Size = new Size(Width, 16);
                            Font = Helper.Fonts.Primary;
                            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
                    }
     
                    protected override void OnResize(EventArgs e)
                    {
                            base.OnResize(e);
                            Size = new Size(Width, 16);
                    }
     
                    protected override void OnMouseUp(MouseEventArgs e)
                    {
                            base.OnMouseUp(e);
                            Checked = !Checked;
                    }
     
                    protected override void OnPaint(PaintEventArgs e)
                    {
                            base.OnPaint(e);
                            var g = e.Graphics;
                            var p = new Pen(Color.FromArgb(35, 35, 40));
                            var s = new SolidBrush(Color.FromArgb(220, 220, 255));
     
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.Clear(Helper.Colors.Background);
     
                            var l = new LinearGradientBrush(new Rectangle(0, 0, Width, Height + 35), Color.FromArgb(98, 115, 133), Color.Black, LinearGradientMode.Vertical);
     
                            g.FillEllipse(l, new Rectangle(1, 1, 14, 14));
                            g.DrawEllipse(p, new Rectangle(5, 5, 5, 5));
     
                            if (Checked)
                                    g.FillEllipse(s, new Rectangle(5, 5, 5, 5));
     
                            s.Color = Helper.Colors.Foreground;
                            g.DrawString(Text, Font, s, new PointF(20, 0));
     
                            Helper.MultiDispose(s, p, l);
     
                    }
            }
     
            sealed class AevionCheckBox : Control
            {
                    public event EventHandler CheckedChanged = delegate { };
                   
                    private Boolean _Checked;
     
                    public Boolean Checked
                    {
                            get { return _Checked; }
                            set {
                                    _Checked = value;
                                    Invalidate();
                                    CheckedChanged(this, new EventArgs());
                            }
                    }
                   
                    public override string Text
                    {
                            get { return base.Text; }
                            set { base.Text = value; Invalidate(); }
                    }
     
                    public override Font Font
                    {
                            get { return base.Font; }
                            set { base.Font = value; Invalidate(); }
                    }
     
                    public AevionCheckBox()
                    {
                            DoubleBuffered = true;
                            Font = Helper.Fonts.Primary;
                            Size = new Size(Width, 16);
                    }
     
                    protected override void OnResize(EventArgs e)
                    {
                            base.OnResize(e);
                            Size = new Size(Width, 16);
                    }
     
                    protected override void OnMouseUp(MouseEventArgs e)
                    {
                            base.OnMouseUp(e);
                            Checked = !Checked;
                    }
     
                    protected override void OnPaint(PaintEventArgs e)
                    {
                            base.OnPaint(e);
                            var g = e.Graphics;
                            var s = new SolidBrush(Helper.Colors.Foreground);
                            var l = new LinearGradientBrush(new Rectangle(0, 0, Width, Height + 35), Color.FromArgb(98, 115, 133), Color.Black, LinearGradientMode.Vertical);
     
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.Clear(Helper.Colors.Background);
     
                            g.FillRectangle(l, new Rectangle(1, 1, 13, 13));
                            Helper.RoundRect(g, 0, 0, 14, 14, 3, Color.FromArgb(35, 35, 40));
     
                            if (Checked)
                                    Helper.CenterString(g, "b", new Font("Marlett", 10), Helper.Colors.Foreground, new Rectangle(2, 1, 13, 13));
     
                            g.DrawString(Text, Font, s, new Point(20, -1));
                           
                            Helper.MultiDispose(s, l);
     
                    }
            }
     
            sealed class AevionProgressBar : Control
            {
                    private Int32 _Maximum = 100;
                    private Int32 _Minimum, _Value = 0;
                    private Boolean _ShowText = true;
     
                    public Int32 Maximum
                    {
                            get { return _Maximum; }
                            set
                            {
                                    if (value > Int32.MaxValue)
                                            throw new OverflowException();
                                    if (value < _Minimum)
                                            _Minimum = value - 1;
                                    if (_Value > _Maximum)
                                            _Value = value;
                                    _Maximum = value;
                                    Invalidate();
                            }
                    }
     
                    public Int32 Minimum
                    {
                            get { return _Minimum; }
                            set
                            {
                                    if (value < 0)
                                            throw new ArgumentOutOfRangeException("Minimum", "Value cannot go below 0.");
                                    if (value < _Minimum)
                                            _Value = value;
                                    if (value > _Maximum)
                                            _Maximum = value + 1;
                                    _Minimum = value;
                                    Invalidate();
                            }
                    }
     
                    public Int32 Value
                    {
                            get { return _Value; }
                            set
                            {
                                    if (value > _Maximum)
                                            _Value = _Maximum;
                                    else if (value < _Minimum)
                                            _Value = _Minimum;
                                    else _Value = value;
                                    Invalidate();
                            }
                    }
     
                    public Boolean ShowText
                    {
                            get { return _ShowText; }
                            set { _ShowText = value; Invalidate(); }
                    }
     
                    public override string Text
                    {
                            get { return base.Text; }
                            set { base.Text = value; Invalidate(); }
                    }
     
                    public override Font Font
                    {
                            get { return base.Font; }
                            set { base.Font = value; Invalidate(); }
                    }
     
                    public AevionProgressBar()
                    {
                            DoubleBuffered = true;
                            Font = Helper.Fonts.Primary;
                    }
     
                    protected override void OnPaint(PaintEventArgs e)
                    {
                            base.OnPaint(e);
                            var g = e.Graphics;
                            var l = new LinearGradientBrush(new Point(0, 0), new Point(Width + Value + 50, Height), Color.FromArgb(98, 115, 133), Color.Black);
     
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.Clear(Helper.Colors.Background);
     
                            g.FillRectangle(l, new Rectangle(0, 0, (int)(Helper.ValueToPercentage(Value, Maximum, Minimum) * Width), Height));
     
                            Helper.RoundRect(g, 0, 0, Width - 1, Height - 1, 3, Color.FromArgb(38, 38, 38));
     
                            if (ShowText)
                                    Helper.CenterString(g, Text, Font, Helper.Colors.Foreground, new Rectangle(0, 0, Width, Height));
     
                            Helper.MultiDispose(l);
     
                    }
     
            }
     
            sealed class AevionNotice : TextBox
            {
     
                    public AevionNotice()
                    {
                            DoubleBuffered = true;
                            Font = Helper.Fonts.Primary;
                            Enabled = false;
                            Multiline = true;
                            BorderStyle = BorderStyle.None;
                            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
                    }
     
                    protected override void OnPaint(PaintEventArgs e)
                    {
                            base.OnPaint(e);
                            var g = e.Graphics;
                            var s = new SolidBrush(Color.FromArgb(38, 38, 38));
     
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.Clear(Helper.Colors.Background);
     
                            g.FillRectangle(s, new Rectangle(1, 1, Width - 2, Height - 2));
                            Helper.RoundRect(g, 0, 0, Width - 1, Height - 1, 3, Color.FromArgb(35, 35, 40));
     
                            s.Color = Helper.Colors.Foreground;
                            g.DrawString(Text, Font, s, new Point(12, 8));
     
                    }
            }
     
            sealed class AevionLabel : Label
            {
                    public AevionLabel()
                    {
                            DoubleBuffered = true;
                            Font = Helper.Fonts.Primary;
                            ForeColor = Helper.Colors.Foreground;
                            BackColor = Helper.Colors.Background;
                    }
            }
     
    }