//theme base 1.5.2
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class ModernTheme : Control {
    public ModernTheme() {
        ForeColor = Color.FromArgb(50, 210, 50);
    }

    private int _TitleHeight = 25;
    public int TitleHeight {
        get { return _TitleHeight; }
        set {
            if (value > Height) {
                value = Height;
            }
            if (value < 2) {
                Height = 1;
            }
            _TitleHeight = value;
            Invalidate();
        }
    }
    private HorizontalAlignment _TitleAlign = (HorizontalAlignment)2;

    public HorizontalAlignment TitleAlign {
        get { return _TitleAlign; }
        set {
            _TitleAlign = value;
            Invalidate();
        }
    }

    protected override void OnHandleCreated(System.EventArgs e) {
        Dock = (DockStyle)5;
        if (Parent is Form)
            ((Form)Parent).FormBorderStyle = 0;
        base.OnHandleCreated(e);
    }

    protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e) {
        if (new Rectangle(Parent.Location.X, Parent.Location.Y, Width - 1, _TitleHeight - 1).IntersectsWith(new Rectangle(MousePosition.X, MousePosition.Y, 1, 1))) {
            Capture = false;
            var M = Message.Create(Parent.Handle, 161, new IntPtr(2), new IntPtr(0));
            DefWndProc(ref M);
        }
        base.OnMouseDown(e);
    }

    private Color C1 = Color.FromArgb(74, 74, 74);
    private Color C2 = Color.FromArgb(63, 63, 63);
    private Color C3 = Color.FromArgb(41, 41, 41);
    private Color C4 = Color.FromArgb(27, 27, 27);
    private Color C5 = Color.FromArgb(0, 0, 0, 0);
    private Color C6 = Color.FromArgb(25, 255, 255, 255);

    protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent) { }

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
        using (Bitmap B = new Bitmap(Width, Height)) {
            using (var G = Graphics.FromImage(B)) {
                G.Clear(C3);
                Draw.Gradient(G, C4, C3, 0, 0, Width, _TitleHeight);

                G.DrawString(Text, Font, new SolidBrush(ForeColor), 6, 6);

                G.DrawLine(new Pen(C3), 0, 1, Width, 1);
                Draw.Blend(G, C5, C6, C5, 0.5F, 0, 0, _TitleHeight + 1, Width, 1);
                G.DrawLine(new Pen(C4), 0, _TitleHeight, Width, _TitleHeight);
                G.DrawRectangle(new Pen(C4), 0, 0, Width - 1, Height - 1);
                e.Graphics.DrawImage((Image)B.Clone(), (float)0, (float)0);
            }
        }
    }
}

public class MCheckBox : Control {
    public MCheckBox() {
        ForeColor = Color.FromArgb(50, 210, 50);
    }
    private bool _checked;
    public bool Checked {
        get { return _checked; }
        set {
            _checked = value;
            Invalidate();
        }
    }

    private int State;
    protected override void OnMouseEnter(System.EventArgs e) {
        State = 1;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e) {
        State = 2;
        Invalidate();
        base.OnMouseDown(e);
    }

    protected override void OnMouseLeave(System.EventArgs e) {
        State = 0;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e) {
        State = 1;
        Invalidate();
        base.OnMouseUp(e);
    }

    protected override void OnClick(System.EventArgs e) {
        _checked = !_checked;
    }

    private Color C1 = Color.FromArgb(31, 31, 31);
    private Color C2 = Color.FromArgb(41, 41, 41);
    private Color C3 = Color.FromArgb(51, 51, 51);
    private Color C4 = Color.FromArgb(0, 0, 0, 0);
    private Color C5 = Color.FromArgb(25, 255, 255, 255);

    protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent) { }

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {

        using (Bitmap B = new Bitmap(Width, Height)) {
            using (var G = Graphics.FromImage(B)) {

                G.Clear(Color.FromArgb(41, 41, 41));

                int BWidth = 15, BHeight = 14;

                if (State == 1 && _checked) {
                    G.DrawRectangle(new Pen(C2), 1, 1, BWidth - 3, BHeight);
                    Draw.Gradient(G, Color.FromArgb(0, 120, 0), Color.FromArgb(0, 110, 0), 1, 1, BWidth - 2, BHeight);
                    G.DrawRectangle(new Pen(C1), 0, 0, BWidth - 1, BHeight);
                }
                else if (State == 2 && _checked) {
                    G.DrawRectangle(new Pen(C2), 1, 1, BWidth - 3, BHeight - 3);
                    Draw.Gradient(G, Color.FromArgb(0, 100, 0), Color.FromArgb(0, 90, 0), 1, 1, BWidth - 2, BHeight);
                    G.DrawRectangle(new Pen(C1), 0, 0, BWidth - 1, BHeight);
                }
                else if (State == 1 && !_checked) {
                    G.DrawRectangle(new Pen(C2), 1, 1, BWidth - 3, BHeight - 3);
                    Draw.Gradient(G, Color.FromArgb(120, 0, 0), Color.FromArgb(110, 0, 0), 1, 1, BWidth - 2, BHeight);
                    G.DrawRectangle(new Pen(C1), 0, 0, BWidth - 1, BHeight);
                }
                else if (State == 2 && !_checked) {
                    G.DrawRectangle(new Pen(C2), 1, 1, BWidth - 3, BHeight - 3);
                    Draw.Gradient(G, Color.FromArgb(100, 0, 0), Color.FromArgb(90, 0, 0), 1, 1, BWidth - 2, BHeight);
                    G.DrawRectangle(new Pen(C1), 0, 0, BWidth - 1, BHeight);
                }
                else if (_checked) {
                    G.DrawRectangle(new Pen(C2), 1, 1, BWidth - 3, BHeight - 3);
                    Draw.Gradient(G, Color.FromArgb(0, 110, 0), Color.FromArgb(0, 100, 0), 1, 1, BWidth - 2, BHeight);
                    G.DrawRectangle(new Pen(C1), 0, 0, BWidth - 1, BHeight);
                }
                else if (!_checked) {
                    G.DrawRectangle(new Pen(C2), 1, 1, BWidth - 3, BHeight - 3);
                    Draw.Gradient(G, Color.FromArgb(110, 0, 0), Color.FromArgb(100, 0, 0), 1, 1, BWidth - 2, BHeight);
                    G.DrawRectangle(new Pen(C1), 0, 0, BWidth - 1, BHeight);
                }
                G.DrawString(Text, Font, new SolidBrush(ForeColor), 16, 1);
                e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            }
        }
    }
}

public class MGroupBox : Control {

    public MGroupBox() {
        ForeColor = Color.FromArgb(50, 210, 50);
    }

    private Color C1 = Color.FromArgb(31, 31, 31);
    private Color C2 = Color.FromArgb(41, 41, 41);
    private Color C3 = Color.FromArgb(51, 51, 51);

    protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent) { }

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
        using (Bitmap B = new Bitmap(Width, Height)) {
            using (var G = Graphics.FromImage(B)) {
                
                Draw.Gradient(G, C3, C2, 1, 1, Width - 2, 20);
                G.FillRectangle(new SolidBrush(C2), new Rectangle(1, 21, Width - 1, Height - 1));

                G.DrawString(Text, Font, new SolidBrush(ForeColor), 5 , 4);

                G.DrawRectangle(new Pen(C1), 0, 0, Width - 1, Height - 1);
                G.DrawLine(new Pen(C1), 0, 21, Width, 21);

                e.Graphics.DrawImage((Image)B.Clone(), (float)0, (float)0);
            }
        }
    }
}



public class MButton : Control {
    public MButton() {
        ForeColor = Color.FromArgb(50, 210, 50);
    }

    private int State;
    protected override void OnMouseEnter(System.EventArgs e) {
        State = 1;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e) {
        State = 2;
        Invalidate();
        base.OnMouseDown(e);
    }

    protected override void OnMouseLeave(System.EventArgs e) {
        State = 0;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e) {
        State = 1;
        Invalidate();
        base.OnMouseUp(e);
    }

    private Color C1 = Color.FromArgb(31, 31, 31);
    private Color C2 = Color.FromArgb(41, 41, 41);
    private Color C3 = Color.FromArgb(51, 51, 51);
    private Color C4 = Color.FromArgb(0, 0, 0, 0);
    private Color C5 = Color.FromArgb(25, 255, 255, 255);

    protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent) { }

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
        using (Bitmap B = new Bitmap(Width, Height)) {
            using (var G = Graphics.FromImage(B)) {
                G.DrawRectangle(new Pen(C1), 0, 0, Width - 1, Height - 1);

                if (State == 2)
                    Draw.Gradient(G, C2, C3, 1, 1, Width - 2, Height - 2);
                else
                    Draw.Gradient(G, C3, C2, 1, 1, Width - 2, Height - 2);

                var O = G.MeasureString(Text, Font);
                G.DrawString(Text, Font, new SolidBrush(ForeColor), Width / 2 - O.Width / 2, Height / 2 - O.Height / 2 + 1);
                Draw.Blend(G, C4, C5, C4, 0.5F, 0, 1, 1, Width - 2, 1);
                e.Graphics.DrawImage((Image)B.Clone(), (float)0, (float)0);
            }
        }
    }
}

public class MProgress : Control {

    private bool _percent;
    public bool ShowPercent {
        get { return _percent; }
        set { _percent = value; }
    }

    private int _Maximum = 100;
    public int Maximum {
        get { return _Maximum; }
        set {
            if (value == 0)
                value = 1;
            _Maximum = value;
            Invalidate();
        }
    }

    private int _Value;
    public int Value {
        get { return _Value; }
        set {
            _Value = value;
            Invalidate();
        }
    }

    public MProgress() {
        ForeColor = Color.FromArgb(50, 210, 50);
        Height = 18;
    }

    private Color C1 = Color.FromArgb(31, 31, 31);
    private Color C2 = Color.FromArgb(41, 41, 41);
    private Color C3 = Color.FromArgb(51, 51, 51);
    private Color C4 = Color.FromArgb(0, 0, 0, 0);
    private Color C5 = Color.FromArgb(25, 255, 255, 255);

    protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent) { }

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
        int V = Width * _Value / _Maximum;
        using (Bitmap B = new Bitmap(Width, Height)) {
            using (var G = Graphics.FromImage(B)) {
                Draw.Gradient(G, C2, C3, 1, 1, Width - 2, Height - 2);
                G.DrawRectangle(new Pen(C2), 1, 1, V - 3, Height - 3);
                Draw.Gradient(G, C3, C2, 2, 2, V - 4, Height - 4);
                G.DrawRectangle(new Pen(C1), 0, 0, Width - 1, Height - 1);
                if (_percent) {
                    string _spercent = (100 * _Value / _Maximum).ToString() + "%";
                    var O = G.MeasureString(_spercent, Font);
                    G.DrawString(_spercent, Font, new SolidBrush(ForeColor), new PointF(Width / 2 - O.Width / 2, Height / 2 - O.Height / 2 + 1));
                }
                e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            }
        }
    }
}

class MComboBox : ComboBox {

    private Color C1 = Color.FromArgb(31, 31, 31);
    private Color C2 = Color.FromArgb(41, 41, 41);
    private Color C3 = Color.FromArgb(51, 51, 51);
    private Color C4 = Color.FromArgb(0, 0, 0, 0);
    private Color C5 = Color.FromArgb(25, 255, 255, 255);

    private int X;
    public MComboBox()
        : base() {
        ForeColor = Color.FromArgb(50, 210, 50);
        TextChanged += GhostCombo_TextChanged;
        DropDownClosed += GhostComboBox_DropDownClosed;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        ItemHeight = 20;
        BackColor = Color.FromArgb(30, 30, 30);
        DropDownStyle = ComboBoxStyle.DropDownList;
    }

    protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e) {
        base.OnMouseMove(e);
        X = e.X;
        Invalidate();
    }

    protected override void OnMouseLeave(System.EventArgs e) {
        base.OnMouseLeave(e);
        X = -1;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e) {
        if (!(DropDownStyle == ComboBoxStyle.DropDownList))
            DropDownStyle = ComboBoxStyle.DropDownList;
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);

        G.Clear(Color.FromArgb(50, 50, 50));
        LinearGradientBrush GradientBrush = new LinearGradientBrush(new Rectangle(0, 0, Width, Height / 5 * 2), Color.FromArgb(20, 0, 0, 0), Color.FromArgb(15, Color.White), 90f);

        G.DrawRectangle(new Pen(C1), 0, 0, Width - 1, Height - 1);
        Draw.Gradient(G, C3, C2, 1, 1, Width - 2, Height - 2);

        int S1 = (int)G.MeasureString("...", Font).Height;
        if (SelectedIndex != -1) {
            G.DrawString(Items[SelectedIndex].ToString(), Font, new SolidBrush(ForeColor), 4, Height / 2 - S1 / 2);
        }
        else {
            if ((Items != null) & Items.Count > 0) {
                G.DrawString(Items[0].ToString(), Font, new SolidBrush(ForeColor), 4, Height / 2 - S1 / 2);
            }
            else {
                G.DrawString("...", Font, new SolidBrush(ForeColor), 4, Height / 2 - S1 / 2);
            }
        }

        if (MouseButtons == System.Windows.Forms.MouseButtons.None & X >= Width - 25 ||
            MouseButtons == System.Windows.Forms.MouseButtons.None & X <= Width - 25 & X >= 0) {
            G.FillRectangle(new SolidBrush(Color.FromArgb(7, Color.White)), Width - 25, 1, Width - 25, Height - 3);
            G.FillRectangle(new SolidBrush(Color.FromArgb(7, Color.White)), 2, 1, Width - 27, Height - 3);
        }

        G.FillPolygon(Brushes.Black, Triangle(new Point(Width - 14, Height / 2), new Size(5, 3)));
        G.FillPolygon(Brushes.White, Triangle(new Point(Width - 15, Height / 2 - 1), new Size(5, 3)));

        e.Graphics.DrawImage(B, 0, 0);
        G.Dispose();
        B.Dispose();
    }

    protected override void OnDrawItem(DrawItemEventArgs e) {
        if (e.Index < 0)
            return;
        Rectangle rect = new Rectangle();
        rect.X = e.Bounds.X;
        rect.Y = e.Bounds.Y;
        rect.Width = e.Bounds.Width - 1;
        rect.Height = e.Bounds.Height - 1;

        e.DrawBackground();
        if ((int)e.State == 785 | (int)e.State == 17) {
            Rectangle x2 = new Rectangle(e.Bounds.Location, new Size(e.Bounds.Width, e.Bounds.Height));
            Rectangle x3 = new Rectangle(x2.Location, new Size(x2.Width, (x2.Height / 2) - 1));
            LinearGradientBrush G1 = new LinearGradientBrush(new Point(x2.X, x2.Y), new Point(x2.X, x2.Y + x2.Height), Color.FromArgb(60, 60, 60), Color.FromArgb(50, 50, 50));
            e.Graphics.FillRectangle(G1, x2);
            G1.Dispose();
            e.Graphics.DrawString(" " + Items[e.Index].ToString(), Font, new SolidBrush(ForeColor), e.Bounds.X, e.Bounds.Y + 2);
        }
        else {
            e.Graphics.FillRectangle(new SolidBrush(BackColor), e.Bounds);
            e.Graphics.DrawString(" " + Items[e.Index].ToString(), Font, new SolidBrush(ForeColor), e.Bounds.X, e.Bounds.Y + 2);
        }
        base.OnDrawItem(e);
    }

    public Point[] Triangle(Point Location, Size Size) {
        Point[] ReturnPoints = new Point[4];
        ReturnPoints[0] = Location;
        ReturnPoints[1] = new Point(Location.X + Size.Width, Location.Y);
        ReturnPoints[2] = new Point(Location.X + Size.Width / 2, Location.Y + Size.Height);
        ReturnPoints[3] = Location;

        return ReturnPoints;
    }

    private void GhostComboBox_DropDownClosed(object sender, System.EventArgs e) {
        DropDownStyle = ComboBoxStyle.Simple;
        Application.DoEvents();
        DropDownStyle = ComboBoxStyle.DropDownList;
    }

    private void GhostCombo_TextChanged(object sender, System.EventArgs e) {
        Invalidate();
    }
}

public class Draw {
    public static void Gradient(Graphics g, Color c1, Color c2, int x, int y, int width, int height) {
        Rectangle R = new Rectangle(x, y, width, height);
        using (LinearGradientBrush T = new LinearGradientBrush(R, c1, c2, LinearGradientMode.Vertical)) {
            g.FillRectangle(T, R);
        }
    }
    public static void Blend(Graphics g, Color c1, Color c2, Color c3, float c, int d, int x, int y, int width, int height) {
        ColorBlend V = new ColorBlend(3);
        V.Colors = new Color[] { c1, c2, c3 };
        V.Positions = new float[] { 0F, c, 1F };
        Rectangle R = new Rectangle(x, y, width, height);
        using (LinearGradientBrush T = new LinearGradientBrush(R, c1, c1, (LinearGradientMode)d)) {
            T.InterpolationColors = V;
            g.FillRectangle(T, R);
        }
    }
}