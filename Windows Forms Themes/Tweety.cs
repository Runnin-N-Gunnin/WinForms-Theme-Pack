using System;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing;

//.::Tweety Theme::.
//Author:   UnReLaTeDScript
//Credits:  Aeonhack [Themebase]
//Version:  1.0
abstract class Theme : ContainerControl
{

    #region " Initialization "

    protected Graphics G;
    public Theme()
    {
        SetStyle((ControlStyles)139270, true);
    }

    private bool ParentIsForm;
    protected override void OnHandleCreated(EventArgs e)
    {
        Dock = DockStyle.Fill;
        ParentIsForm = Parent is Form;
        if (ParentIsForm)
        {
            if (!(_TransparencyKey == Color.Empty))
                ParentForm.TransparencyKey = _TransparencyKey;
            ParentForm.FormBorderStyle = FormBorderStyle.None;
        }
        base.OnHandleCreated(e);
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            Invalidate();
        }
    }
    #endregion

    #region " Sizing and Movement "

    private bool _Resizable = true;
    public bool Resizable
    {
        get { return _Resizable; }
        set { _Resizable = value; }
    }

    private int _MoveHeight = 24;
    public int MoveHeight
    {
        get { return _MoveHeight; }
        set
        {
            _MoveHeight = value;
            Header = new Rectangle(7, 7, Width - 14, _MoveHeight - 7);
        }
    }

    private IntPtr Flag;
    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (!(e.Button == MouseButtons.Left))
            return;
        if (ParentIsForm)
            if (ParentForm.WindowState == FormWindowState.Maximized)
                return;

        if (Header.Contains(e.Location))
        {
            Flag = new IntPtr(2);
        }
        else if (Current.Position == 0 | !_Resizable)
        {
            return;
        }
        else
        {
            Flag = new IntPtr(Current.Position);
        }

        Capture = false;
        Message m = Message.Create(Parent.Handle, 161, Flag, IntPtr.Zero);
        DefWndProc(ref m);

        base.OnMouseDown(e);
    }

    private struct Pointer
    {
        public readonly Cursor Cursor;
        public readonly byte Position;
        public Pointer(Cursor c, byte p)
        {
            Cursor = c;
            Position = p;
        }
    }

    private bool F1;
    private bool F2;
    private bool F3;
    private bool F4;
    private Point PTC;
    private Pointer GetPointer()
    {
        PTC = PointToClient(MousePosition);
        F1 = PTC.X < 7;
        F2 = PTC.X > Width - 7;
        F3 = PTC.Y < 7;
        F4 = PTC.Y > Height - 7;

        if (F1 & F3)
            return new Pointer(Cursors.SizeNWSE, 13);
        if (F1 & F4)
            return new Pointer(Cursors.SizeNESW, 16);
        if (F2 & F3)
            return new Pointer(Cursors.SizeNESW, 14);
        if (F2 & F4)
            return new Pointer(Cursors.SizeNWSE, 17);
        if (F1)
            return new Pointer(Cursors.SizeWE, 10);
        if (F2)
            return new Pointer(Cursors.SizeWE, 11);
        if (F3)
            return new Pointer(Cursors.SizeNS, 12);
        if (F4)
            return new Pointer(Cursors.SizeNS, 15);
        return new Pointer(Cursors.Default, 0);
    }

    private Pointer Current;
    private Pointer Pending;
    private void SetCurrent()
    {
        Pending = GetPointer();
        if (Current.Position == Pending.Position)
            return;
        Current = GetPointer();
        Cursor = Current.Cursor;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_Resizable)
            SetCurrent();
        base.OnMouseMove(e);
    }

    protected Rectangle Header;
    protected override void OnSizeChanged(EventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;
        Header = new Rectangle(7, 7, Width - 14, _MoveHeight - 7);
        Invalidate();
        base.OnSizeChanged(e);
    }

    #endregion

    #region " Convienence "

    public abstract void PaintHook();
    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;
        G = e.Graphics;
        PaintHook();
    }

    private Color _TransparencyKey;
    public Color TransparencyKey
    {
        get { return _TransparencyKey; }
        set
        {
            _TransparencyKey = value;
            Invalidate();
        }
    }

    private Image _Image;
    public Image Image
    {
        get { return _Image; }
        set
        {
            _Image = value;
            Invalidate();
        }
    }
    public int ImageWidth
    {
        get
        {
            if (_Image == null)
                return 0;
            return _Image.Width;
        }
    }

    private Size _Size;
    private Rectangle _Rectangle;
    private LinearGradientBrush _Gradient;

    private SolidBrush _Brush;
    protected void DrawCorners(Color c, Rectangle rect)
    {
        _Brush = new SolidBrush(c);
        G.FillRectangle(_Brush, rect.X, rect.Y, 1, 1);
        G.FillRectangle(_Brush, rect.X + (rect.Width - 1), rect.Y, 1, 1);
        G.FillRectangle(_Brush, rect.X, rect.Y + (rect.Height - 1), 1, 1);
        G.FillRectangle(_Brush, rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), 1, 1);
    }

    protected void DrawBorders(Pen p1, Pen p2, Rectangle rect)
    {
        G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
        G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3);
    }

    protected void DrawText(HorizontalAlignment a, Color c, int x)
    {
        DrawText(a, c, x, 0);
    }
    protected void DrawText(HorizontalAlignment a, Color c, int x, int y)
    {
        if (string.IsNullOrEmpty(Text))
            return;
        _Size = G.MeasureString(Text, Font).ToSize();
        _Brush = new SolidBrush(c);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawString(Text, Font, _Brush, x, _MoveHeight / 2 - _Size.Height / 2 + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawString(Text, Font, _Brush, Width - _Size.Width - x, _MoveHeight / 2 - _Size.Height / 2 + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawString(Text, Font, _Brush, Width / 2 - _Size.Width / 2 + x, _MoveHeight / 2 - _Size.Height / 2 + y);
                break;
        }
    }

    protected void DrawIcon(HorizontalAlignment a, int x)
    {
        DrawIcon(a, x, 0);
    }
    protected void DrawIcon(HorizontalAlignment a, int x, int y)
    {
        if (_Image == null)
            return;
        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawImage(_Image, x, _MoveHeight / 2 - _Image.Height / 2 + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawImage(_Image, Width - _Image.Width - x, _MoveHeight / 2 - _Image.Height / 2 + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawImage(_Image, Width / 2 - _Image.Width / 2, _MoveHeight / 2 - _Image.Height / 2);
                break;
        }
    }

    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        _Rectangle = new Rectangle(x, y, width, height);
        _Gradient = new LinearGradientBrush(_Rectangle, c1, c2, angle);
        G.FillRectangle(_Gradient, _Rectangle);
    }

    #endregion

}
static class Draw
{
    public static GraphicsPath RoundRect(Rectangle Rectangle, int Curve)
    {
        GraphicsPath P = new GraphicsPath();
        int ArcRectangleWidth = Curve * 2;
        P.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90);
        P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90);
        P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90);
        P.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90);
        P.AddLine(new Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), new Point(Rectangle.X, Curve + Rectangle.Y));
        return P;
    }
    //Public Function RoundRect(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal Curve As Integer) As GraphicsPath
    //    Dim Rectangle As Rectangle = New Rectangle(X, Y, Width, Height)
    //    Dim P As GraphicsPath = New GraphicsPath()
    //    Dim ArcRectangleWidth As Integer = Curve * 2
    //    P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
    //    P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
    //    P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
    //    P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
    //    P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
    //    Return P
    //End Function
}
abstract class ThemeControl : Control
{

    #region " Initialization "

    protected Graphics G;
    protected Bitmap B;
    public ThemeControl()
    {
        SetStyle((ControlStyles)139270, true);
        B = new Bitmap(1, 1);
        G = Graphics.FromImage(B);
    }

    public void AllowTransparent()
    {
        SetStyle(ControlStyles.Opaque, false);
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            Invalidate();
        }
    }
    #endregion

    #region " Mouse Handling "

    protected enum State : byte
    {
        MouseNone = 0,
        MouseOver = 1,
        MouseDown = 2
    }

    protected State MouseState;
    protected override void OnMouseLeave(EventArgs e)
    {
        ChangeMouseState(State.MouseNone);
        base.OnMouseLeave(e);
    }
    protected override void OnMouseEnter(EventArgs e)
    {
        ChangeMouseState(State.MouseOver);
        base.OnMouseEnter(e);
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        ChangeMouseState(State.MouseOver);
        base.OnMouseUp(e);
    }
    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            ChangeMouseState(State.MouseDown);
        base.OnMouseDown(e);
    }

    private void ChangeMouseState(State e)
    {
        MouseState = e;
        Invalidate();
    }

    #endregion

    #region " Convienence "

    public abstract void PaintHook();
    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;
        PaintHook();
        e.Graphics.DrawImage(B, 0, 0);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        if (!(Width == 0) && !(Height == 0))
        {
            B = new Bitmap(Width, Height);
            G = Graphics.FromImage(B);
            Invalidate();
        }
        base.OnSizeChanged(e);
    }

    private bool _NoRounding;
    public bool NoRounding
    {
        get { return _NoRounding; }
        set
        {
            _NoRounding = value;
            Invalidate();
        }
    }

    private Image _Image;
    public Image Image
    {
        get { return _Image; }
        set
        {
            _Image = value;
            Invalidate();
        }
    }
    public int ImageWidth
    {
        get
        {
            if (_Image == null)
                return 0;
            return _Image.Width;
        }
    }
    public int ImageTop
    {
        get
        {
            if (_Image == null)
                return 0;
            return Height / 2 - _Image.Height / 2;
        }
    }

    private Size _Size;
    private Rectangle _Rectangle;
    private LinearGradientBrush _Gradient;

    private SolidBrush _Brush;
    protected void DrawCorners(Color c, Rectangle rect)
    {
        if (_NoRounding)
            return;

        B.SetPixel(rect.X, rect.Y, c);
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y, c);
        B.SetPixel(rect.X, rect.Y + (rect.Height - 1), c);
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), c);
    }

    protected void DrawBorders(Pen p1, Pen p2, Rectangle rect)
    {
        G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
        G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3);
    }

    protected void DrawText(HorizontalAlignment a, Color c, int x)
    {
        DrawText(a, c, x, 0);
    }
    protected void DrawText(HorizontalAlignment a, Color c, int x, int y)
    {
        if (string.IsNullOrEmpty(Text))
            return;
        _Size = G.MeasureString(Text, Font).ToSize();
        _Brush = new SolidBrush(c);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawString(Text, Font, _Brush, x, Height / 2 - _Size.Height / 2 + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawString(Text, Font, _Brush, Width - _Size.Width - x, Height / 2 - _Size.Height / 2 + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawString(Text, Font, _Brush, Width / 2 - _Size.Width / 2 + x, Height / 2 - _Size.Height / 2 + y);
                break;
        }
    }

    protected void DrawIcon(HorizontalAlignment a, int x)
    {
        DrawIcon(a, x, 0);
    }
    protected void DrawIcon(HorizontalAlignment a, int x, int y)
    {
        if (_Image == null)
            return;
        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawImage(_Image, x, Height / 2 - _Image.Height / 2 + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawImage(_Image, Width - _Image.Width - x, Height / 2 - _Image.Height / 2 + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawImage(_Image, Width / 2 - _Image.Width / 2, Height / 2 - _Image.Height / 2);
                break;
        }
    }

    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        _Rectangle = new Rectangle(x, y, width, height);
        _Gradient = new LinearGradientBrush(_Rectangle, c1, c2, angle);
        G.FillRectangle(_Gradient, _Rectangle);
    }
    #endregion

}
abstract class ThemeContainerControl : ContainerControl
{

    #region " Initialization "

    protected Graphics G;
    protected Bitmap B;
    public ThemeContainerControl()
    {
        SetStyle((ControlStyles)139270, true);
        B = new Bitmap(1, 1);
        G = Graphics.FromImage(B);
    }

    public void AllowTransparent()
    {
        SetStyle(ControlStyles.Opaque, false);
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
    }

    #endregion
    #region " Convienence "

    public abstract void PaintHook();
    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;
        PaintHook();
        e.Graphics.DrawImage(B, 0, 0);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        if (!(Width == 0) && !(Height == 0))
        {
            B = new Bitmap(Width, Height);
            G = Graphics.FromImage(B);
            Invalidate();
        }
        base.OnSizeChanged(e);
    }

    private bool _NoRounding;
    public bool NoRounding
    {
        get { return _NoRounding; }
        set
        {
            _NoRounding = value;
            Invalidate();
        }
    }

    private Rectangle _Rectangle;

    private LinearGradientBrush _Gradient;
    protected void DrawCorners(Color c, Rectangle rect)
    {
        if (_NoRounding)
            return;
        B.SetPixel(rect.X, rect.Y, c);
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y, c);
        B.SetPixel(rect.X, rect.Y + (rect.Height - 1), c);
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), c);
    }

    protected void DrawBorders(Pen p1, Pen p2, Rectangle rect)
    {
        G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
        G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3);
    }

    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        _Rectangle = new Rectangle(x, y, width, height);
        _Gradient = new LinearGradientBrush(_Rectangle, c1, c2, angle);
        G.FillRectangle(_Gradient, _Rectangle);
    }
    #endregion

}

class TxtBox : ThemeControl
{
    #region "lol"
    TextBox txtbox = new TextBox();
    private bool _passmask = false;
    public bool UseSystemPasswordChar
    {
        get { return _passmask; }
        set
        {
            txtbox.UseSystemPasswordChar = UseSystemPasswordChar;
            _passmask = value;
            Invalidate();
        }
    }
    private int _maxchars = 32767;
    public int MaxLength
    {
        get { return _maxchars; }
        set
        {
            _maxchars = value;
            txtbox.MaxLength = MaxLength;
            Invalidate();
        }
    }
    private HorizontalAlignment _align;
    public HorizontalAlignment TextAlignment
    {
        get { return _align; }
        set
        {
            _align = value;
            Invalidate();
        }
    }

    protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
    {
    }
    protected override void OnTextChanged(System.EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }
    protected override void OnBackColorChanged(System.EventArgs e)
    {
        base.OnBackColorChanged(e);
        txtbox.BackColor = BackColor;
        Invalidate();
    }
    protected override void OnForeColorChanged(System.EventArgs e)
    {
        base.OnForeColorChanged(e);
        txtbox.ForeColor = ForeColor;
        Invalidate();
    }
    protected override void OnFontChanged(System.EventArgs e)
    {
        base.OnFontChanged(e);
        txtbox.Font = Font;
    }
    protected override void OnGotFocus(System.EventArgs e)
    {
        base.OnGotFocus(e);
        txtbox.Focus();
    }
    public void  // ERROR: Handles clauses are not supported in C#
TextChngTxtBox()
    {
        Text = txtbox.Text;
    }
    public void  // ERROR: Handles clauses are not supported in C#
TextChng()
    {
        txtbox.Text = Text;
    }

    #endregion

    protected override void WndProc(ref Message m)
    {
        switch (m.Msg)
        {
            case 15:
                Invalidate();
                base.WndProc(ref m);
                this.PaintHook();
                break; // TODO: might not be correct. Was : Exit Select
            default:
                base.WndProc(ref m);
                break; // TODO: might not be correct. Was : Exit Select
        }
    }

    public TxtBox()
        : base()
    {

        Controls.Add(txtbox);
        {
            txtbox.Multiline = false;
            txtbox.BackColor = Color.FromArgb(0, 0, 0);
            txtbox.ForeColor = ForeColor;
            txtbox.Text = string.Empty;
            txtbox.TextAlign = HorizontalAlignment.Center;
            txtbox.BorderStyle = BorderStyle.None;
            txtbox.Location = new Point(5, 8);
            txtbox.Font = new Font("Arial", 8.25f, FontStyle.Bold);
            txtbox.Size = new Size(Width - 8, Height - 11);
            txtbox.UseSystemPasswordChar = UseSystemPasswordChar;
        }

        Text = "";

        DoubleBuffered = true;
    }

    public override void PaintHook()
    {
        this.BackColor = Color.White;
        G.Clear(Parent.BackColor);
        Pen p = new Pen(Color.FromArgb(204, 204, 204), 1);
        Pen o = new Pen(Color.FromArgb(249, 249, 249), 8);
        G.FillPath(Brushes.White, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 2));
        G.DrawPath(o, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 2));
        G.DrawPath(p, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 2));
        Height = txtbox.Height + 16;
        Font drawFont = new Font("Tahoma", 9, FontStyle.Regular);
        {
            txtbox.Width = Width - 12;
            txtbox.ForeColor = Color.FromArgb(72, 72, 72);
            txtbox.Font = drawFont;
            txtbox.TextAlign = TextAlignment;
            txtbox.UseSystemPasswordChar = UseSystemPasswordChar;
        }
        DrawCorners(Parent.BackColor, ClientRectangle);
    }
}

class PanelBox : ThemeContainerControl
{
    public PanelBox()
    {
        AllowTransparent();
    }
    public override void PaintHook()
    {
        this.Font = new Font("Tahoma", 10);
        this.ForeColor = Color.FromArgb(40, 40, 40);
        G.SmoothingMode = SmoothingMode.AntiAlias;
        G.FillRectangle(new SolidBrush(Color.FromArgb(235, 235, 235)), new Rectangle(2, 0, Width, Height));
        G.FillRectangle(new SolidBrush(Color.FromArgb(249, 249, 249)), new Rectangle(1, 0, Width - 3, Height - 4));
        G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 2, Height - 3);
    }
}
class GroupDropBox : ThemeContainerControl
{
    private bool _Checked;
    private int X;
    private int y;
    private Size _OpenedSize;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            Invalidate();
        }
    }
    public Size OpenSize
    {
        get { return _OpenedSize; }
        set
        {
            _OpenedSize = value;
            Invalidate();
        }
    }
    public GroupDropBox()
    {
        
        AllowTransparent();
        Size = new Size(90, 30);
        MinimumSize = new Size(5, 30);
        _Checked = true;
        this.Resize += new EventHandler(GroupDropBox_Resize);
        this.MouseDown += new MouseEventHandler(GroupDropBox_MouseDown);
    }
    public override void PaintHook()
    {
        this.Font = new Font("Tahoma", 10);
        this.ForeColor = Color.FromArgb(40, 40, 40);
        if (_Checked == true)
        {
            G.SmoothingMode = SmoothingMode.AntiAlias;
            G.Clear(Color.FromArgb(245, 245, 245));
            G.FillRectangle(new SolidBrush(Color.FromArgb(231, 231, 231)), new Rectangle(0, 0, Width, 30));
            G.DrawLine(new Pen(Color.FromArgb(233, 238, 240)), 1, 1, Width - 2, 1);
            G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, Height - 1);
            G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, 30);
            this.Size = _OpenedSize;
            G.DrawString("t", new Font("Marlett", 12), new SolidBrush(this.ForeColor), Width - 25, 5);
        }
        else
        {
            G.SmoothingMode = SmoothingMode.AntiAlias;
            G.Clear(Color.FromArgb(245, 245, 245));
            G.FillRectangle(new SolidBrush(Color.FromArgb(231, 231, 231)), new Rectangle(0, 0, Width, 30));
            G.DrawLine(new Pen(Color.FromArgb(231, 236, 238)), 1, 1, Width - 2, 1);
            G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, Height - 1);
            G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, 30);
            this.Size = new Size(Width, 30);
            G.DrawString("u", new Font("Marlett", 12), new SolidBrush(this.ForeColor), Width - 25, 5);
        }
        G.DrawString(Text, Font, new SolidBrush(this.ForeColor), 7, 6);
    }

    private void GroupDropBox_Resize(object sender, System.EventArgs e)
    {
        if (_Checked == true)
        {
            _OpenedSize = this.Size;
        }
    }


    protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.X;
        y = e.Y;
        Invalidate();
    }


    private void GroupDropBox_MouseDown(object sender, MouseEventArgs e)
    {

        if (X >= Width - 22)
        {
            if (y <= 30)
            {
                switch (Checked)
                {
                    case true:
                        Checked = false;
                        break;
                    case false:
                        Checked = true;
                        break;
                }
            }
        }
    }
}
class GroupPanelBox : ThemeContainerControl
{
    public GroupPanelBox()
    {
        AllowTransparent();
    }
    public override void PaintHook()
    {
        this.Font = new Font("Tahoma", 10);
        this.ForeColor = Color.FromArgb(40, 40, 40);
        G.SmoothingMode = SmoothingMode.AntiAlias;
        G.Clear(Color.FromArgb(245, 245, 245));
        G.FillRectangle(new SolidBrush(Color.FromArgb(231, 231, 231)), new Rectangle(0, 0, Width, 30));
        G.DrawLine(new Pen(Color.FromArgb(233, 238, 240)), 1, 1, Width - 2, 1);
        G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, Height - 1);
        G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, 30);
        G.DrawString(Text, Font, new SolidBrush(this.ForeColor), 7, 6);
    }
}

class ButtonGreen : ThemeControl
{
    public override void PaintHook()
    {
        this.Font = new Font("Arial", 10);
        G.Clear(this.BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                Pen p1 = new Pen(Color.FromArgb(120, 159, 22), 1);
                LinearGradientBrush x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(157, 209, 57), Color.FromArgb(130, 181, 18), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(190, 232, 109)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0);
                break;
            case State.MouseDown:
                Pen p2 = new Pen(Color.FromArgb(120, 159, 22), 1);
                LinearGradientBrush x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(125, 171, 25), Color.FromArgb(142, 192, 40), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(142, 172, 30)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1);
                break;
            case State.MouseOver:
                Pen p3 = new Pen(Color.FromArgb(120, 159, 22), 1);
                LinearGradientBrush x3 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(165, 220, 59), Color.FromArgb(137, 191, 18), LinearGradientMode.Vertical);
                G.FillPath(x3, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p3, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(190, 232, 109)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1);
                break;
        }
        this.Cursor = Cursors.Hand;
    }
}
class ButtonBlue : ThemeControl
{
    public override void PaintHook()
    {
        this.Font = new Font("Arial", 10);
        G.Clear(this.BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                Pen p = new Pen(Color.FromArgb(34, 112, 171), 1);
                LinearGradientBrush x = new LinearGradientBrush(ClientRectangle, Color.FromArgb(51, 159, 231), Color.FromArgb(33, 128, 206), LinearGradientMode.Vertical);
                G.FillPath(x, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(131, 197, 241)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0);
                break;
            case State.MouseDown:
                Pen p1 = new Pen(Color.FromArgb(34, 112, 171), 1);
                LinearGradientBrush x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(37, 124, 196), Color.FromArgb(53, 153, 219), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));

                DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1);
                break;
            case State.MouseOver:
                Pen p2 = new Pen(Color.FromArgb(34, 112, 171), 1);
                LinearGradientBrush x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(54, 167, 243), Color.FromArgb(35, 165, 217), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(131, 197, 241)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1);
                break;
        }
        this.Cursor = Cursors.Hand;
    }
}