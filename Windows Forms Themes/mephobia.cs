//Credit to ๖ۣۜMephobia for the original theme
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
public enum MouseState : byte
{
    None = 0,
    Over = 1,
    Down = 2,
    Block = 3,
}
public static class Draw
{
    public static GraphicsPath RoundRect(Rectangle rect, int Curve)
    {
        GraphicsPath P = new GraphicsPath();
        int ArcRectWidth = Curve * 2;
        P.AddArc(new Rectangle(rect.X, rect.Y, ArcRectWidth, ArcRectWidth), -180, 90);
        P.AddArc(new Rectangle(rect.Width - ArcRectWidth + rect.X, rect.Y, ArcRectWidth, ArcRectWidth), -90, 90);
        P.AddArc(new Rectangle(rect.Width - ArcRectWidth + rect.X, rect.Height - ArcRectWidth + rect.Y, ArcRectWidth, ArcRectWidth), 0, 90);
        P.AddArc(new Rectangle(rect.X, rect.Height - ArcRectWidth + rect.Y, ArcRectWidth, ArcRectWidth), 90, 90);
        P.AddLine(new Point(rect.X, rect.Height - ArcRectWidth + rect.Y), new Point(rect.X, Curve + rect.Y));
        return P;
    }
    public static GraphicsPath RoundRect(int X, int Y, int Width, int Height, int Curve)
    {
        return RoundRect(new Rectangle(X, Y, Width, Height), Curve);
    }
}
public class PerplexTheme : ContainerControl
{
    public PerplexTheme()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.FromArgb(25, 25, 25);
        DoubleBuffered = true;
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        Rectangle TopLeft = new Rectangle(0, 0, Width - 125, 28);
        Rectangle TopRight = new Rectangle(Width - 82, 0, 81, 28);
        Rectangle Body = new Rectangle(10, 10, Width - 21, Height - 16);
        Rectangle Body2 = new Rectangle(5, 5, Width - 11, Height - 6);
        base.OnPaint(e);
        LinearGradientBrush BodyBrush = new LinearGradientBrush(Body2, Color.FromArgb(25, 25, 25), Color.FromArgb(30, 35, 48), 90);
        LinearGradientBrush BodyBrush2 = new LinearGradientBrush(Body, Color.FromArgb(46, 46, 46), Color.FromArgb(50, 55, 58), 120);
        LinearGradientBrush gloss = new LinearGradientBrush(new Rectangle(0, 0, Width - 128, 28 / 2), Color.FromArgb(240, Color.FromArgb(26, 26, 26)), Color.FromArgb(5, 255, 255, 255), 90);
        LinearGradientBrush gloss2 = new LinearGradientBrush(new Rectangle(Width - 82, 0,Width - 205, 28 / 2), Color.FromArgb(240, Color.FromArgb(26, 26, 26)), Color.FromArgb(5, 255, 255, 255), 90);
        LinearGradientBrush mainbrush = new LinearGradientBrush(TopLeft, Color.FromArgb(26, 26, 26), Color.FromArgb(30, 30, 30), 90);
        LinearGradientBrush mainbrush2 = new LinearGradientBrush(TopRight, Color.FromArgb(26, 26, 26), Color.FromArgb(30, 30, 30), 90);
        Pen P1 = new Pen(Color.FromArgb(174, 195, 30), 2);
        Font drawFont = new Font("Tahoma", 10, FontStyle.Bold);
            
        G.Clear(Color.Fuchsia);
        G.FillPath(BodyBrush, Draw.RoundRect(Body2, 3));
        G.DrawPath(Pens.Black, Draw.RoundRect(Body2, 3));
            
        G.FillPath(BodyBrush2, Draw.RoundRect(Body, 3));
        G.DrawPath(Pens.Black, Draw.RoundRect(Body, 3));

        G.FillPath(mainbrush, Draw.RoundRect(TopLeft, 3));
        G.FillPath(gloss, Draw.RoundRect(TopLeft, 3));
        G.DrawPath(Pens.Black, Draw.RoundRect(TopLeft, 3));

        G.FillPath(mainbrush, Draw.RoundRect(TopRight, 3));
        G.FillPath(gloss2, Draw.RoundRect(TopRight, 3));
        G.DrawPath(Pens.Black, Draw.RoundRect(TopRight, 3));

        G.DrawLine(P1, 14, 9, 14, 22);
        G.DrawLine(P1, 17, 6, 17, 25);
        G.DrawLine(P1, 20, 9, 20, 22);
        G.DrawLine(P1, 11, 12, 11, 19);
        G.DrawLine(P1, 23, 12, 23, 19);
        G.DrawLine(P1, 8, 14, 8, 17);
        G.DrawLine(P1, 26, 14, 26, 17);
        G.DrawString(Text, drawFont, new SolidBrush(Color.WhiteSmoke), new Rectangle(32, 1, Width - 1, 27), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
    private Point MouseP = new Point(0, 0);
    private bool cap = false;
    private int moveheight = 29;
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button == MouseButtons.Left && new Rectangle(0, 0, Width, moveheight).Contains(e.Location))
        {
            cap = true;
            MouseP = e.Location;
        }
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        cap = false;
    }
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (cap)
        {
            Point p = new Point();
            p.X = MousePosition.X - MouseP.X;
            p.Y = MousePosition.Y - MouseP.Y;
            Parent.Location = p;
        }
    }
    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        ParentForm.FormBorderStyle = FormBorderStyle.None;
        ParentForm.TransparencyKey = Color.Fuchsia;
        Dock = DockStyle.Fill;
    }
}
public class PerplexButton : Control
{
    MouseState State = MouseState.None;
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        State = MouseState.Down;
        Invalidate();
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        State = MouseState.Over;
        Invalidate();
    }
    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        State = MouseState.Over;
        Invalidate();
    }
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        State = MouseState.None;
        Invalidate();
    }
    public PerplexButton()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(205, 205, 205);
        DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        Rectangle ClientRectangle = new Rectangle(0, 0, Width - 1, Height - 1);
        base.OnPaint(e);
        G.Clear(BackColor);
        Font drawFont = new Font("Tahoma", 8, FontStyle.Bold);
        G.SmoothingMode = SmoothingMode.HighQuality;
        Rectangle R1 = new Rectangle(0, 0, Width - 125, 35 / 2);
        Rectangle R2 = new Rectangle(5, Height - 10, Width - 11, 5);
        Rectangle R3 = new Rectangle(6, Height - 9, Width - 13, 3);
        Rectangle R4 = new Rectangle(1, 1, Width - 3, Height - 3);
        Rectangle R5 = new Rectangle(1, 0, Width - 1, Height - 1);
        Rectangle R6 = new Rectangle(0, -1, Width - 1, Height - 1);
        LinearGradientBrush lgb = new LinearGradientBrush(ClientRectangle, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90);
        LinearGradientBrush botbar = new LinearGradientBrush(R2, Color.FromArgb(44, 45, 49), Color.FromArgb(45, 46, 50), 90);
        LinearGradientBrush fill = new LinearGradientBrush(R3, Color.FromArgb(174, 195, 30), Color.FromArgb(141, 153, 16), 90);
        LinearGradientBrush gloss = null;
        Pen o = new Pen(Color.FromArgb(50, 50, 50), 1);
        StringFormat format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        if (State == MouseState.Over)
            gloss = new LinearGradientBrush(R1, Color.FromArgb(15, Color.FromArgb(26, 26, 26)), Color.FromArgb(1, 255, 255, 255), 90);
        else if (State == MouseState.Down)
            gloss = new LinearGradientBrush(R1, Color.FromArgb(100, Color.FromArgb(26, 26, 26)), Color.FromArgb(1, 255, 255, 255), 90);
        else
            gloss = new LinearGradientBrush(R1, Color.FromArgb(75, Color.FromArgb(26, 26, 26)), Color.FromArgb(3, 255, 255, 255), 90);

        G.FillPath(lgb, Draw.RoundRect(ClientRectangle, 2));
        G.FillPath(gloss, Draw.RoundRect(ClientRectangle, 2));
        G.FillPath(botbar, Draw.RoundRect(R2, 1));
        G.FillPath(fill, Draw.RoundRect(R3, 1));
        G.DrawPath(o, Draw.RoundRect(ClientRectangle, 2));
        G.DrawPath(Pens.Black, Draw.RoundRect(R4, 2));
        G.DrawString(Text, drawFont, new SolidBrush(Color.FromArgb(5, 5, 5)), R5, format);
        G.DrawString(Text, drawFont, new SolidBrush(Color.FromArgb(205, 205, 205)), R6, format);

        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}
public class PerplexControlBox : Control
{
    MouseState State = MouseState.None;
    Rectangle MinBtn = new Rectangle(0, 0, 20, 20);
    Rectangle MaxBtn = new Rectangle(25, 0, 20, 20);
    int x = 0;

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Location.X > 0 && e.Location.X < 20)
            FindForm().WindowState = FormWindowState.Minimized;
        else if (e.Location.X > 25 && e.Location.X < 45)
            FindForm().Close();
        State = MouseState.Down;
        Invalidate();
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        State = MouseState.Over;
        Invalidate();
    }
    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        State = MouseState.Over;
        Invalidate();
    }
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        State = MouseState.None;
        Invalidate();
    }
    public PerplexControlBox()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(205, 205, 205);
        DoubleBuffered = true;
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        base.OnPaint(e);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;

        LinearGradientBrush mlgb = null;
        Font mf = new Font("Marlett", 9);
        SolidBrush mfb = new SolidBrush(Color.FromArgb(174, 195, 30));
        Pen P1 = new Pen(Color.FromArgb(21, 21, 21), 1);
        Color C1 = Color.FromArgb(66, 67, 70);
        Color C2 = Color.FromArgb(43, 44, 48);
        GraphicsPath GP1 = Draw.RoundRect(MinBtn, 4);
        GraphicsPath GP2 = Draw.RoundRect(MaxBtn, 4);
        switch (State)
        {
            case MouseState.None:
                mlgb = new LinearGradientBrush(MinBtn, C1, C2, 90);
                G.FillPath(mlgb, GP1);
                G.DrawPath(P1, GP1);
                G.DrawString("0", mf, mfb, 4, 4);

                G.FillPath(mlgb, GP2);
                G.DrawPath(P1, GP2);
                G.DrawString("r", mf, mfb, 28, 4);
                break;
            case MouseState.Over:
                if (x > 0 && x < 20)
                {
                    mlgb = new LinearGradientBrush(MinBtn, Color.FromArgb(100, C1), Color.FromArgb(100, C2), 90);
                    G.FillPath(mlgb, GP1);
                    G.DrawPath(P1, GP1);
                    G.DrawString("0", mf, mfb, 4, 4);

                    mlgb = new LinearGradientBrush(MaxBtn, C1, C2, 90);
                    G.FillPath(mlgb, Draw.RoundRect(MaxBtn, 4));
                    G.DrawPath(P1, GP2);
                    G.DrawString("r", mf, mfb, 4, 4);
                }
                else if (x > 25 && x < 45)
                {
                    mlgb = new LinearGradientBrush(MinBtn, C1, C2, 90);
                    G.FillPath(mlgb, GP1);
                    G.DrawPath(P1, GP1);
                    G.DrawString("0", mf, mfb, 4, 4);
                    mlgb = new LinearGradientBrush(MaxBtn, Color.FromArgb(100, C1), Color.FromArgb(100, C2), 90);
                    G.FillPath(mlgb, GP2);
                    G.DrawPath(P1, GP2);
                    G.DrawString("r", mf, mfb, 28, 4);
                }
                else
                {
                    mlgb = new LinearGradientBrush(MinBtn, C1, C2, 90);
                    G.FillPath(mlgb, GP1);
                    G.DrawPath(P1, GP1);
                    G.DrawString("0", mf, mfb, 4, 4);

                    LinearGradientBrush lgb = new LinearGradientBrush(MaxBtn, C1, C2, 90);
                    G.FillPath(lgb, GP2);
                    G.DrawPath(P1, GP2);
                    G.DrawString("r", mf, mfb, 28, 4);
                }
                break;
            case MouseState.Down:
                mlgb = new LinearGradientBrush(MinBtn, C1, C2, 90);
                G.FillPath(mlgb, GP1);
                G.DrawPath(P1, GP1);
                G.DrawString("0", mf, mfb, 4, 4);

                mlgb = new LinearGradientBrush(MaxBtn, C1, C2, 90);
                G.FillPath(mlgb, GP2);
                G.DrawPath(P1, GP2);
                G.DrawString("r", mf, mfb, 28, 4);
                break;
            default:
                mlgb = new LinearGradientBrush(MinBtn, C1, C2, 90);
                G.FillPath(mlgb, GP1);
                G.DrawPath(P1, GP1);
                G.DrawString("0", mf, mfb, 4, 4);

                mlgb = new LinearGradientBrush(MaxBtn, C1, C2, 90);
                G.FillPath(mlgb, GP2);
                G.DrawPath(P1, GP2);
                G.DrawString("r", mf, mfb, 28, 4);
                break;
        }
        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}
public class PerplexGroupBox : ContainerControl
{
    public PerplexGroupBox()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.Transparent;
        DoubleBuffered = true;
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        Rectangle Body = new Rectangle(4, 25, Width - 9, Height - 30);
        Rectangle Body2 = new Rectangle(0, 0, Width - 1, Height - 1);
        base.OnPaint(e);
        G.Clear(Color.Transparent);
        G.SmoothingMode = SmoothingMode.HighQuality;
        G.CompositingQuality = CompositingQuality.HighQuality;

        Pen P1 = new Pen(Color.Black);
        LinearGradientBrush BodyBrush = new LinearGradientBrush(Body2, Color.FromArgb(26, 26, 26), Color.FromArgb(30, 30, 30), 90);
        LinearGradientBrush BodyBrush2 = new LinearGradientBrush(Body, Color.FromArgb(46, 46, 46), Color.FromArgb(50, 55, 58), 120);
        Font drawFont = new Font("Tahoma", 9, FontStyle.Bold);
        G.FillPath(BodyBrush, Draw.RoundRect(Body2, 3));
        G.DrawPath(P1, Draw.RoundRect(Body2, 3));

        G.FillPath(BodyBrush2, Draw.RoundRect(Body, 3));
        G.DrawPath(P1, Draw.RoundRect(Body, 3));

        G.DrawString(Text, drawFont, new SolidBrush(Color.WhiteSmoke), 67, 14, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}
public class PerplexProgressBar : Control
{
    private int _Maximum = 100;

    public int Maximum {
        get { return _Maximum; }
        set {
            _Maximum = value;
            Invalidate();
        }
    }
    private int _Value = 0;
    public int Value
    {
        get
        {
            if (_Value == 0)
                return 0;
            else return _Value;
        }
        set
        {
            _Value = value;
            if (_Value > _Maximum)
                _Value = _Maximum;
            Invalidate();
        }
    }
    private bool _ShowPercentage = false;
    public bool ShowPercentage {
        get { return _ShowPercentage; }
        set {
            _ShowPercentage = value;
            Invalidate();
        }
    }

    public PerplexProgressBar() : base()
    {
        DoubleBuffered = true;
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);

        G.SmoothingMode = SmoothingMode.HighQuality;

        double val = (double)_Value / _Maximum;
        int intValue = Convert.ToInt32(val * Width);
        G.Clear(BackColor);
        Color C1 = Color.FromArgb(174, 195, 30);
        Color C2 = Color.FromArgb(141, 153, 16); 
        Rectangle R1 = new Rectangle(0, 0, Width - 1, Height - 1);
        Rectangle R2 = new Rectangle(0, 0, intValue - 1, Height - 1);
        Rectangle R3 = new Rectangle(0, 0, intValue - 1, Height - 2);
        GraphicsPath GP1 = Draw.RoundRect(R1, 1);
        GraphicsPath GP2 = Draw.RoundRect(R2, 2);
        GraphicsPath GP3 = Draw.RoundRect(R3, 1);
        LinearGradientBrush gB = new LinearGradientBrush(R1, Color.FromArgb(26, 26, 26), Color.FromArgb(30, 30, 30), 90);
        LinearGradientBrush g1 = new LinearGradientBrush(new Rectangle(2, 2, intValue - 1, Height - 2), C1, C2, 90);
        HatchBrush h1 = new HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(50, C1), Color.FromArgb(25, C2));
        Pen P1 = new Pen(Color.Black);

        G.FillPath(gB, GP1);
        G.FillPath(g1, GP3);
        G.FillPath(h1, GP3);
        G.DrawPath(P1, GP1);
        G.DrawPath(new Pen(Color.FromArgb(150, 97, 94, 90)), GP2);
        G.DrawPath(P1, GP2);

        if (_ShowPercentage) 
            G.DrawString(Convert.ToString(string.Concat(Value, "%")), Font, Brushes.White, R1, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}
[DefaultEvent("CheckedChanged")]
public class PerplexCheckBox : Control
{
    MouseState State = MouseState.None;
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        State = MouseState.Down;
        Invalidate();
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        State = MouseState.Over;
        Invalidate();
    }
    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        State = MouseState.Over;
        Invalidate();
    }
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        State = MouseState.None;
        Invalidate();
    }
    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        Height = 16;
    }
    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }
    protected override void OnClick(EventArgs e)
    {
        _Checked = !_Checked;
        if (CheckedChanged != null)
            CheckedChanged(this, EventArgs.Empty);
        base.OnClick(e);
    }
    private bool _Checked = false;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            if (CheckedChanged != null)
                CheckedChanged(this, EventArgs.Empty);
            Invalidate();
        }
    }
    public PerplexCheckBox()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
        ForeColor = Color.Black;
        Size = new Size(145, 16);
        DoubleBuffered = true;
    }
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        G.SmoothingMode = SmoothingMode.HighQuality;
        G.CompositingQuality = CompositingQuality.HighQuality;
        Rectangle checkBoxRectangle = new Rectangle(0, 0, Height - 1, Height - 1);
        LinearGradientBrush bodyGrad = new LinearGradientBrush(checkBoxRectangle, Color.FromArgb(174, 195, 30), Color.FromArgb(141, 153, 16), 90);
        SolidBrush nb = new SolidBrush(Color.FromArgb(205, 205, 205));
        StringFormat format = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
        Font drawFont = new Font("Tahoma", 9, FontStyle.Bold);
        G.Clear(BackColor);
        G.FillRectangle(bodyGrad, bodyGrad.Rectangle);
        G.DrawRectangle(new Pen(Color.Black), checkBoxRectangle);
        G.DrawString(Text, drawFont, Brushes.Black, new Point(17, 9), format);
        G.DrawString(Text, drawFont, nb, new Point(16, 8), format);

        if (_Checked)
        {
            Rectangle chkPoly = new Rectangle(checkBoxRectangle.X + checkBoxRectangle.Width / 4, checkBoxRectangle.Y + checkBoxRectangle.Height / 4, checkBoxRectangle.Width / 2, checkBoxRectangle.Height / 2);
            Point[] p = new Point[] {new Point(chkPoly.X, chkPoly.Y + chkPoly.Height /2), 
                        new Point(chkPoly.X + chkPoly.Width / 2, chkPoly.Y + chkPoly.Height), 
                        new Point(chkPoly.X + chkPoly.Width, chkPoly.Y)};
            Pen P1 = new Pen(Color.FromArgb(12, 12, 12), 2);
            for (int i = 0; i <= p.Length - 2; i++)
                G.DrawLine(P1, p[i], p[i + 1]);
        }
        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }

    public event EventHandler CheckedChanged;
}
[DefaultEvent("CheckedChanged")]
public class PerplexRadioButton : Control
{
    MouseState State = MouseState.None;
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        State = MouseState.Down;
        Invalidate();
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        State = MouseState.Over;
        Invalidate();
    }
    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        State = MouseState.Over;
        Invalidate();
    }
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        State = MouseState.None;
        Invalidate();
    }
    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        Height = 16;
    }
    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }
    protected override void OnClick(EventArgs e)
    {
        Checked = !Checked;
        base.OnClick(e);
    }
    private bool _Checked = false;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            InvalidateControls();
            if (CheckedChanged != null)
                CheckedChanged(this, EventArgs.Empty);
            Invalidate();
        }
    }
    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        InvalidateControls();
    }
    private void InvalidateControls()
    {
        if (!IsHandleCreated || !_Checked) return;
        foreach (Control C in Parent.Controls)
            if (C is PerplexRadioButton && C != this)
                ((PerplexRadioButton)C).Checked = false;
    }
    public PerplexRadioButton()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
        ForeColor = Color.Black;
        Size = new Size(150, 16);
        DoubleBuffered = true;
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        G.Clear(BackColor);
        Rectangle radioBtnRectangle = new Rectangle(0, 0, Height - 1, Height - 1);
        Rectangle R1 = new Rectangle(4, 4, Height - 9, Height - 9);
        StringFormat format = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
        LinearGradientBrush bgGrad = new LinearGradientBrush(radioBtnRectangle, Color.FromArgb(174, 195, 30), Color.FromArgb(141, 153, 16), 90);
        Color C1 = Color.FromArgb(250, 15, 15, 15);
        SolidBrush nb = new SolidBrush(Color.FromArgb(205, 205, 205));
        G.SmoothingMode = SmoothingMode.HighQuality;
        G.CompositingQuality = CompositingQuality.HighQuality;
        Font drawFont = new Font("Tahoma", 10, FontStyle.Bold);

        G.FillEllipse(bgGrad, radioBtnRectangle);
        G.DrawEllipse(new Pen(Color.Black), radioBtnRectangle);

        if (Checked)
        {
            LinearGradientBrush chkGrad = new LinearGradientBrush(R1, C1, C1, 90);
            G.FillEllipse(chkGrad, R1);
        }

        G.DrawString(Text, drawFont, Brushes.Black, new Point(17, 2), format);
        G.DrawString(Text, drawFont, nb, new Point(16, 1), format);

        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
    public event EventHandler CheckedChanged;
}
public class PerplexLabel : Control
{
    public PerplexLabel()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(205, 205, 205);
        DoubleBuffered = true;
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        Rectangle ClientRectangle = new Rectangle(0, 0, Width - 1, Height - 1);
        base.OnPaint(e);
        G.Clear(BackColor);
        Font drawFont = new Font("Tahoma", 9, FontStyle.Bold);
        StringFormat format = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center};
        G.CompositingQuality = CompositingQuality.HighQuality;
        G.SmoothingMode = SmoothingMode.HighQuality;
        G.DrawString(Text, drawFont, new SolidBrush(Color.FromArgb(5, 5, 5)), new Rectangle(1, 0, Width - 1, Height - 1), format);
        G.DrawString(Text, drawFont, new SolidBrush(Color.FromArgb(205, 205, 205)), new Rectangle(0, -1, Width - 1, Height - 1), format);
        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}