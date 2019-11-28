using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
//Creator: Slurms Makenzi
//Thanks Aeon for theme class!
//Date: 7/7/2011
//Site: www.loungeforum.net
//Version: 1.0
//Thanks xZ3ROxPROJ3CTx/Janniek kuenen
//http://www.hackforums.net/showthread.php?tid=1472377
//

//\\\\\\\\\\\\\\\Use Credits Were Credits Are Needed///////////////////

//

#region "Imports"
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Runtime.InteropServices;
#endregion
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
        Message msg = Message.Create(Parent.Handle, 161, Flag, System.IntPtr.Zero);
        DefWndProc(ref msg);

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
public class Draw
{
    public static void Gradient(Graphics g, Color c1, Color c2, int x, int y, int width, int height)
    {
        Rectangle R = new Rectangle(x, y, width, height);
        using (LinearGradientBrush T = new LinearGradientBrush(R, c1, c2, LinearGradientMode.Vertical))
        {
            g.FillRectangle(T, R);
        }
    }
    public static void Gradient(Graphics g, Color c1, Color c2, Rectangle r)
    {
        using (LinearGradientBrush T = new LinearGradientBrush(r, c1, c2, LinearGradientMode.Vertical))
        {
            g.FillRectangle(T, r);
        }
    }
    public static void Blend(Graphics g, Color c1, Color c2, Color c3, float c, int d, int x, int y, int width, int height)
    {
        ColorBlend v = new ColorBlend(3);
        v.Colors = new Color[] {
			c1,
			c2,
			c3
		};
        v.Positions = new float[] {
			0,
			c,
			1
		};
        Rectangle R = new Rectangle(x, y, width, height);
        using (LinearGradientBrush T = new LinearGradientBrush(R, c1, c1, (LinearGradientMode)d))
        {
            T.InterpolationColors = v;
            g.FillRectangle(T, R);
        }
    }
    public static GraphicsPath RoundedRectangle(int x, int y, int width, int height, int cornerwidth, int PenWidth)
    {
        GraphicsPath p = new GraphicsPath();
        p.StartFigure();
        p.AddArc(new Rectangle(x, y, cornerwidth, cornerwidth), 180, 90);
        p.AddLine(cornerwidth, y, width - cornerwidth - PenWidth, y);

        p.AddArc(new Rectangle(width - cornerwidth - PenWidth, y, cornerwidth, cornerwidth), -90, 90);
        p.AddLine(width - PenWidth, cornerwidth, width - PenWidth, height - cornerwidth - PenWidth);

        p.AddArc(new Rectangle(width - cornerwidth - PenWidth, height - cornerwidth - PenWidth, cornerwidth, cornerwidth), 0, 90);
        p.AddLine(width - cornerwidth - PenWidth, height - PenWidth, cornerwidth, height - PenWidth);

        p.AddArc(new Rectangle(x, height - cornerwidth - PenWidth, cornerwidth, cornerwidth), 90, 90);
        p.CloseFigure();

        return p;
    }


    public static void BackGround(int width, int height, Graphics G)
    {
        Color P1 = Color.FromArgb(29, 25, 22);
        Color P2 = Color.FromArgb(35, 31, 28);

        for (int y = 0; y <= height; y += 4)
        {
            for (int x = 0; x <= width; x += 4)
            {
                G.FillRectangle(new SolidBrush(P1), new Rectangle(x, y, 1, 1));
                G.FillRectangle(new SolidBrush(P2), new Rectangle(x, y + 1, 1, 1));
                try
                {
                    G.FillRectangle(new SolidBrush(P1), new Rectangle(x + 2, y + 2, 1, 1));
                    G.FillRectangle(new SolidBrush(P2), new Rectangle(x + 2, y + 3, 1, 1));
                }
                catch
                {
                }
            }
        }
    }
}
class PurityxTheme : Theme
{

    public PurityxTheme()
    {
        Resizable = false;
        BackColor = Color.FromKnownColor(KnownColor.Control);
        MoveHeight = 25;
        TransparencyKey = Color.Fuchsia;
    }

    public override void PaintHook()
    {
        G.Clear(BackColor);
        // Clear the form first

        //DrawGradient(Color.FromArgb(64, 64, 64), Color.FromArgb(32, 32, 32), 0, 0, Width, Height, 90S)   ' Form Gradient
        G.Clear(Color.FromArgb(60, 60, 60));
        DrawGradient(Color.FromArgb(45, 40, 45), Color.FromArgb(32, 32, 32), 0, 0, Width, 25, 90);
        // Form Top Bar

        G.DrawLine(Pens.Black, 0, 25, Width, 25);
        // Top Line
        //G.DrawLine(Pens.Black, 0, Height - 25, Width, Height - 25)   ' Bottom Line

        DrawCorners(Color.Fuchsia, ClientRectangle);
        // Then draw some clean corners
        DrawBorders(Pens.Black, Pens.DimGray, ClientRectangle);
        // Then we draw our form borders

        DrawText(HorizontalAlignment.Left, Color.Red, 7, 1);
        // Finally, we draw our text
    }
}
// Theme Code
class PurityxButton : ThemeControl
{

    public override void PaintHook()
    {
        switch (MouseState)
        {
            case State.MouseNone:
                G.Clear(Color.Red);
                DrawGradient(Color.FromArgb(62, 62, 62), Color.FromArgb(38, 38, 38), 0, 0, Width, Height, 90);
                break;
            case State.MouseOver:
                G.Clear(Color.Red);
                DrawGradient(Color.FromArgb(62, 62, 62), Color.FromArgb(38, 38, 38), 0, 0, Width, Height, 90);
                break;
            case State.MouseDown:
                G.Clear(Color.DarkRed);
                DrawGradient(Color.FromArgb(38, 38, 38), Color.FromArgb(62, 62, 62), 0, 0, Width, Height, 90);
                break;
        }

        DrawBorders(Pens.Black, Pens.DimGray, ClientRectangle);
        // Form Border
        DrawCorners(Color.Black, ClientRectangle);
        // Clean Corners
        DrawText(HorizontalAlignment.Center, Color.Red, 0);
    }
}
// Button Code
public class PurityxLabel : Label
{
    public PurityxLabel()
    {
        Font = new Font("Arial", 8);
        ForeColor = Color.Red;
        BackColor = Color.Transparent;
    }
}
// Label Code
public class PurityxSeperator : Control
{

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        using (Bitmap b = new Bitmap(Width, Height))
        {
            using (Graphics g = Graphics.FromImage(b))
            {
                g.Clear(BackColor);

                Color P1 = Color.FromArgb(255, 255, 255);
                Color P2 = Color.FromArgb(255, 255, 255);
                g.FillRectangle(new SolidBrush(Color.FromArgb(60, 60, 60)), new Rectangle(0, 0, Width, Height));


                Rectangle GRec = new Rectangle(0, Height / 2, Width / 5, 2);
                using (LinearGradientBrush GBrush = new LinearGradientBrush(GRec, Color.Transparent, P2, LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(GBrush, GRec);
                }
                g.DrawLine(new Pen(P2, 2), new Point(GRec.Width, GRec.Y + 1), new Point(Width - GRec.Width + 1, GRec.Y + 1));

                GRec = new Rectangle(Width - (Width / 5), Height / 2, Width / 5, 2);
                using (LinearGradientBrush GBrush = new LinearGradientBrush(GRec, P2, Color.Transparent, LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(GBrush, GRec);
                }
                e.Graphics.DrawImage(b, 0, 0);
            }
        }
        base.OnPaint(e);
    }
}
// Seperator Code
class PurityxGroupbox : Panel
{
    Color Bg = Color.FromArgb(62, 62, 62);
    Color PC2 = Color.FromArgb(204, 204, 204);
    Color FC = Color.FromArgb(204, 204, 204);
    Pen p;
    SolidBrush sb;
    public PurityxGroupbox()
    {
        BackColor = Bg;
    }
    string _t = "";
    public string Header
    {
        get { return _t; }
        set
        {
            _t = value;
            Invalidate();
        }
    }
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        using (Bitmap b = new Bitmap(Width, Height))
        {
            using (Graphics g = Graphics.FromImage(b))
            {
                p = new Pen(PC2);
                sb = new SolidBrush(Bg);
                SizeF M = g.MeasureString(_t, Font);
                GraphicsPath Outline = Draw.RoundedRectangle(0, (int)M.Height / 2, Width - 1, Height - 1, 10, 1);
                g.Clear(BackColor);
                g.FillRectangle(sb, new Rectangle(0, (int)M.Height / 2, Width - 1, Height - 1));
                g.DrawPath(p, Outline);
                g.FillRectangle(sb, new Rectangle(10, (int)(M.Height / 2) - 2, (int)M.Width + 10, (int)M.Height));
                sb = new SolidBrush(FC);
                g.DrawString(base.Text, base.Font, Brushes.White, 7, 1);
                e.Graphics.DrawImage(b, 0, 0);
            }
        }
        base.OnPaint(e);
    }
}
// GroupBox Code