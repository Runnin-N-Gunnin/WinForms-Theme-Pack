using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

#region "Themebase"
//------------------
//Creator: aeonhack
//Site: elitevs.net
//Created: 08/02/2011
//Changed: 12/06/2011
//Version: 1.5.4
//------------------

abstract class ThemeContainer154 : ContainerControl
{

    #region " Initialization "

    protected Graphics G;

    protected Bitmap B;
    public ThemeContainer154()
    {
        SetStyle((ControlStyles)139270, true);

        _ImageSize = Size.Empty;
        Font = new Font("Verdana", 8);

        MeasureBitmap = new Bitmap(1, 1);
        MeasureGraphics = Graphics.FromImage(MeasureBitmap);

        DrawRadialPath = new GraphicsPath();

        InvalidateCustimization();
    }

    protected override sealed void OnHandleCreated(EventArgs e)
    {
        if (DoneCreation)
            InitializeMessages();

        InvalidateCustimization();
        ColorHook();

        if (!(_LockWidth == 0))
            Width = _LockWidth;
        if (!(_LockHeight == 0))
            Height = _LockHeight;
        if (!_ControlMode)
            base.Dock = DockStyle.Fill;

        Transparent = _Transparent;
        if (_Transparent && _BackColor)
            BackColor = Color.Transparent;

        base.OnHandleCreated(e);
    }

    private bool DoneCreation;
    protected override sealed void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);

        if (Parent == null)
            return;
        _IsParentForm = Parent is Form;

        if (!_ControlMode)
        {
            InitializeMessages();

            if (_IsParentForm)
            {
                ParentForm.FormBorderStyle = _BorderStyle;
                ParentForm.TransparencyKey = _TransparencyKey;

                if (!DesignMode)
                {
                    ParentForm.Shown += FormShown;
                }
            }

            Parent.BackColor = BackColor;
        }

        OnCreation();
        DoneCreation = true;
        InvalidateTimer();
    }

    #endregion

    private void DoAnimation(bool i)
    {
        OnAnimation();
        if (i)
            Invalidate();
    }

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;

        if (_Transparent && _ControlMode)
        {
            PaintHook();
            e.Graphics.DrawImage(B, 0, 0);
        }
        else
        {
            G = e.Graphics;
            PaintHook();
        }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        ThemeShare.RemoveAnimationCallback(DoAnimation);
        base.OnHandleDestroyed(e);
    }

    private bool HasShown;
    private void FormShown(object sender, EventArgs e)
    {
        if (_ControlMode || HasShown)
            return;

        if (_StartPosition == FormStartPosition.CenterParent || _StartPosition == FormStartPosition.CenterScreen)
        {
            Rectangle SB = Screen.PrimaryScreen.Bounds;
            Rectangle CB = ParentForm.Bounds;
            ParentForm.Location = new Point(SB.Width / 2 - CB.Width / 2, SB.Height / 2 - CB.Width / 2);
        }

        HasShown = true;
    }


    #region " Size Handling "

    private Rectangle Frame;
    protected override sealed void OnSizeChanged(EventArgs e)
    {
        if (_Movable && !_ControlMode)
        {
            Frame = new Rectangle(7, 7, Width - 14, _Header - 7);
        }

        InvalidateBitmap();
        Invalidate();

        base.OnSizeChanged(e);
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        if (!(_LockWidth == 0))
            width = _LockWidth;
        if (!(_LockHeight == 0))
            height = _LockHeight;
        base.SetBoundsCore(x, y, width, height, specified);
    }

    #endregion

    #region " State Handling "

    protected MouseState State;
    private void SetState(MouseState current)
    {
        State = current;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized))
        {
            if (_Sizable && !_ControlMode)
                InvalidateMouse();
        }

        base.OnMouseMove(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        if (Enabled)
            SetState(MouseState.None);
        else
            SetState(MouseState.Block);
        base.OnEnabledChanged(e);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        SetState(MouseState.Over);
        base.OnMouseEnter(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        SetState(MouseState.Over);
        base.OnMouseUp(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        SetState(MouseState.None);

        if (GetChildAtPoint(PointToClient(MousePosition)) != null)
        {
            if (_Sizable && !_ControlMode)
            {
                Cursor = Cursors.Default;
                Previous = 0;
            }
        }

        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
            SetState(MouseState.Down);

        if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized || _ControlMode))
        {
            if (_Movable && Frame.Contains(e.Location))
            {
                Capture = false;
                WM_LMBUTTONDOWN = true;
                DefWndProc(ref Messages[0]);
            }
            else if (_Sizable && !(Previous == 0))
            {
                Capture = false;
                WM_LMBUTTONDOWN = true;
                DefWndProc(ref Messages[Previous]);
            }
        }

        base.OnMouseDown(e);
    }

    private bool WM_LMBUTTONDOWN;
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        if (WM_LMBUTTONDOWN && m.Msg == 513)
        {
            WM_LMBUTTONDOWN = false;

            SetState(MouseState.Over);
            if (!_SmartBounds)
                return;

            if (IsParentMdi)
            {
                CorrectBounds(new Rectangle(Point.Empty, Parent.Parent.Size));
            }
            else
            {
                CorrectBounds(Screen.FromControl(Parent).WorkingArea);
            }
        }
    }

    private Point GetIndexPoint;
    private bool B1;
    private bool B2;
    private bool B3;
    private bool B4;
    private int GetIndex()
    {
        GetIndexPoint = PointToClient(MousePosition);
        B1 = GetIndexPoint.X < 7;
        B2 = GetIndexPoint.X > Width - 7;
        B3 = GetIndexPoint.Y < 7;
        B4 = GetIndexPoint.Y > Height - 7;

        if (B1 && B3)
            return 4;
        if (B1 && B4)
            return 7;
        if (B2 && B3)
            return 5;
        if (B2 && B4)
            return 8;
        if (B1)
            return 1;
        if (B2)
            return 2;
        if (B3)
            return 3;
        if (B4)
            return 6;
        return 0;
    }

    private int Current;
    private int Previous;
    private void InvalidateMouse()
    {
        Current = GetIndex();
        if (Current == Previous)
            return;

        Previous = Current;
        switch (Previous)
        {
            case 0:
                Cursor = Cursors.Default;
                break;
            case 1:
            case 2:
                Cursor = Cursors.SizeWE;
                break;
            case 3:
            case 6:
                Cursor = Cursors.SizeNS;
                break;
            case 4:
            case 8:
                Cursor = Cursors.SizeNWSE;
                break;
            case 5:
            case 7:
                Cursor = Cursors.SizeNESW;
                break;
        }
    }

    private Message[] Messages = new Message[9];
    private void InitializeMessages()
    {
        Messages[0] = Message.Create(Parent.Handle, 161, new IntPtr(2), IntPtr.Zero);
        for (int I = 1; I <= 8; I++)
        {
            Messages[I] = Message.Create(Parent.Handle, 161, new IntPtr(I + 9), IntPtr.Zero);
        }
    }

    private void CorrectBounds(Rectangle bounds)
    {
        if (Parent.Width > bounds.Width)
            Parent.Width = bounds.Width;
        if (Parent.Height > bounds.Height)
            Parent.Height = bounds.Height;

        int X = Parent.Location.X;
        int Y = Parent.Location.Y;

        if (X < bounds.X)
            X = bounds.X;
        if (Y < bounds.Y)
            Y = bounds.Y;

        int Width = bounds.X + bounds.Width;
        int Height = bounds.Y + bounds.Height;

        if (X + Parent.Width > Width)
            X = Width - Parent.Width;
        if (Y + Parent.Height > Height)
            Y = Height - Parent.Height;

        Parent.Location = new Point(X, Y);
    }

    #endregion


    #region " Base Properties "

    public override DockStyle Dock
    {
        get { return base.Dock; }
        set
        {
            if (!_ControlMode)
                return;
            base.Dock = value;
        }
    }

    private bool _BackColor;
    [Category("Misc")]
    public override Color BackColor
    {
        get { return base.BackColor; }
        set
        {
            if (value == base.BackColor)
                return;

            if (!IsHandleCreated && _ControlMode && value == Color.Transparent)
            {
                _BackColor = true;
                return;
            }

            base.BackColor = value;
            if (Parent != null)
            {
                if (!_ControlMode)
                    Parent.BackColor = value;
                ColorHook();
            }
        }
    }

    public override Size MinimumSize
    {
        get { return base.MinimumSize; }
        set
        {
            base.MinimumSize = value;
            if (Parent != null)
                Parent.MinimumSize = value;
        }
    }

    public override Size MaximumSize
    {
        get { return base.MaximumSize; }
        set
        {
            base.MaximumSize = value;
            if (Parent != null)
                Parent.MaximumSize = value;
        }
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

    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            Invalidate();
        }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Color ForeColor
    {
        get { return Color.Empty; }
        set { }
    }
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Image BackgroundImage
    {
        get { return null; }
        set { }
    }
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override ImageLayout BackgroundImageLayout
    {
        get { return ImageLayout.None; }
        set { }
    }

    #endregion

    #region " Public Properties "

    private bool _SmartBounds = true;
    public bool SmartBounds
    {
        get { return _SmartBounds; }
        set { _SmartBounds = value; }
    }

    private bool _Movable = true;
    public bool Movable
    {
        get { return _Movable; }
        set { _Movable = value; }
    }

    private bool _Sizable = true;
    public bool Sizable
    {
        get { return _Sizable; }
        set { _Sizable = value; }
    }

    private Color _TransparencyKey;
    public Color TransparencyKey
    {
        get
        {
            if (_IsParentForm && !_ControlMode)
                return ParentForm.TransparencyKey;
            else
                return _TransparencyKey;
        }
        set
        {
            if (value == _TransparencyKey)
                return;
            _TransparencyKey = value;

            if (_IsParentForm && !_ControlMode)
            {
                ParentForm.TransparencyKey = value;
                ColorHook();
            }
        }
    }

    private FormBorderStyle _BorderStyle;
    public FormBorderStyle BorderStyle
    {
        get
        {
            if (_IsParentForm && !_ControlMode)
                return ParentForm.FormBorderStyle;
            else
                return _BorderStyle;
        }
        set
        {
            _BorderStyle = value;

            if (_IsParentForm && !_ControlMode)
            {
                ParentForm.FormBorderStyle = value;

                if (!(value == FormBorderStyle.None))
                {
                    Movable = false;
                    Sizable = false;
                }
            }
        }
    }

    private FormStartPosition _StartPosition;
    public FormStartPosition StartPosition
    {
        get
        {
            if (_IsParentForm && !_ControlMode)
                return ParentForm.StartPosition;
            else
                return _StartPosition;
        }
        set
        {
            _StartPosition = value;

            if (_IsParentForm && !_ControlMode)
            {
                ParentForm.StartPosition = value;
            }
        }
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
            if (value == null)
                _ImageSize = Size.Empty;
            else
                _ImageSize = value.Size;

            _Image = value;
            Invalidate();
        }
    }

    private Dictionary<string, Color> Items = new Dictionary<string, Color>();
    public Bloom[] Colors
    {
        get
        {
            List<Bloom> T = new List<Bloom>();
            Dictionary<string, Color>.Enumerator E = Items.GetEnumerator();

            while (E.MoveNext())
            {
                T.Add(new Bloom(E.Current.Key, E.Current.Value));
            }

            return T.ToArray();
        }
        set
        {
            foreach (Bloom B in value)
            {
                if (Items.ContainsKey(B.Name))
                    Items[B.Name] = B.Value;
            }

            InvalidateCustimization();
            ColorHook();
            Invalidate();
        }
    }

    private string _Customization;
    public string Customization
    {
        get { return _Customization; }
        set
        {
            if (value == _Customization)
                return;

            byte[] Data = null;
            Bloom[] Items = Colors;

            try
            {
                Data = Convert.FromBase64String(value);
                for (int I = 0; I <= Items.Length - 1; I++)
                {
                    Items[I].Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4));
                }
            }
            catch
            {
                return;
            }

            _Customization = value;

            Colors = Items;
            ColorHook();
            Invalidate();
        }
    }

    private bool _Transparent;
    public bool Transparent
    {
        get { return _Transparent; }
        set
        {
            _Transparent = value;
            if (!(IsHandleCreated || _ControlMode))
                return;

            if (!value && !(BackColor.A == 255))
            {
                throw new Exception("Unable to change value to false while a transparent BackColor is in use.");
            }

            SetStyle(ControlStyles.Opaque, !value);
            SetStyle(ControlStyles.SupportsTransparentBackColor, value);

            InvalidateBitmap();
            Invalidate();
        }
    }

    #endregion

    #region " Private Properties "

    private Size _ImageSize;
    protected Size ImageSize
    {
        get { return _ImageSize; }
    }

    private bool _IsParentForm;
    protected bool IsParentForm
    {
        get { return _IsParentForm; }
    }

    protected bool IsParentMdi
    {
        get
        {
            if (Parent == null)
                return false;
            return Parent.Parent != null;
        }
    }

    private int _LockWidth;
    protected int LockWidth
    {
        get { return _LockWidth; }
        set
        {
            _LockWidth = value;
            if (!(LockWidth == 0) && IsHandleCreated)
                Width = LockWidth;
        }
    }

    private int _LockHeight;
    protected int LockHeight
    {
        get { return _LockHeight; }
        set
        {
            _LockHeight = value;
            if (!(LockHeight == 0) && IsHandleCreated)
                Height = LockHeight;
        }
    }

    private int _Header = 24;
    protected int Header
    {
        get { return _Header; }
        set
        {
            _Header = value;

            if (!_ControlMode)
            {
                Frame = new Rectangle(7, 7, Width - 14, value - 7);
                Invalidate();
            }
        }
    }

    private bool _ControlMode;
    protected bool ControlMode
    {
        get { return _ControlMode; }
        set
        {
            _ControlMode = value;

            Transparent = _Transparent;
            if (_Transparent && _BackColor)
                BackColor = Color.Transparent;

            InvalidateBitmap();
            Invalidate();
        }
    }

    private bool _IsAnimated;
    protected bool IsAnimated
    {
        get { return _IsAnimated; }
        set
        {
            _IsAnimated = value;
            InvalidateTimer();
        }
    }

    #endregion


    #region " Property Helpers "

    protected Pen GetPen(string name)
    {
        return new Pen(Items[name]);
    }
    protected Pen GetPen(string name, float width)
    {
        return new Pen(Items[name], width);
    }

    protected SolidBrush GetBrush(string name)
    {
        return new SolidBrush(Items[name]);
    }

    protected Color GetColor(string name)
    {
        return Items[name];
    }

    protected void SetColor(string name, Color value)
    {
        if (Items.ContainsKey(name))
            Items[name] = value;
        else
            Items.Add(name, value);
    }
    protected void SetColor(string name, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(r, g, b));
    }
    protected void SetColor(string name, byte a, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(a, r, g, b));
    }
    protected void SetColor(string name, byte a, Color value)
    {
        SetColor(name, Color.FromArgb(a, value));
    }

    private void InvalidateBitmap()
    {
        if (_Transparent && _ControlMode)
        {
            if (Width == 0 || Height == 0)
                return;
            B = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
            G = Graphics.FromImage(B);
        }
        else
        {
            G = null;
            B = null;
        }
    }

    private void InvalidateCustimization()
    {
        MemoryStream M = new MemoryStream(Items.Count * 4);

        foreach (Bloom B in Colors)
        {
            M.Write(BitConverter.GetBytes(B.Value.ToArgb()), 0, 4);
        }

        M.Close();
        _Customization = Convert.ToBase64String(M.ToArray());
    }

    private void InvalidateTimer()
    {
        if (DesignMode || !DoneCreation)
            return;

        if (_IsAnimated)
        {
            ThemeShare.AddAnimationCallback(DoAnimation);
        }
        else
        {
            ThemeShare.RemoveAnimationCallback(DoAnimation);
        }
    }

    #endregion


    #region " User Hooks "

    protected abstract void ColorHook();
    protected abstract void PaintHook();

    protected virtual void OnCreation()
    {
    }

    protected virtual void OnAnimation()
    {
    }

    #endregion


    #region " Offset "

    private Rectangle OffsetReturnRectangle;
    protected Rectangle Offset(Rectangle r, int amount)
    {
        OffsetReturnRectangle = new Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2));
        return OffsetReturnRectangle;
    }

    private Size OffsetReturnSize;
    protected Size Offset(Size s, int amount)
    {
        OffsetReturnSize = new Size(s.Width + amount, s.Height + amount);
        return OffsetReturnSize;
    }

    private Point OffsetReturnPoint;
    protected Point Offset(Point p, int amount)
    {
        OffsetReturnPoint = new Point(p.X + amount, p.Y + amount);
        return OffsetReturnPoint;
    }

    #endregion

    #region " Center "


    private Point CenterReturn;
    protected Point Center(Rectangle p, Rectangle c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X + c.X, (p.Height / 2 - c.Height / 2) + p.Y + c.Y);
        return CenterReturn;
    }
    protected Point Center(Rectangle p, Size c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X, (p.Height / 2 - c.Height / 2) + p.Y);
        return CenterReturn;
    }

    protected Point Center(Rectangle child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }
    protected Point Center(Size child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }
    protected Point Center(int childWidth, int childHeight)
    {
        return Center(Width, Height, childWidth, childHeight);
    }

    protected Point Center(Size p, Size c)
    {
        return Center(p.Width, p.Height, c.Width, c.Height);
    }

    protected Point Center(int pWidth, int pHeight, int cWidth, int cHeight)
    {
        CenterReturn = new Point(pWidth / 2 - cWidth / 2, pHeight / 2 - cHeight / 2);
        return CenterReturn;
    }

    #endregion

    #region " Measure "

    private Bitmap MeasureBitmap;

    private Graphics MeasureGraphics;
    protected Size Measure()
    {
        lock (MeasureGraphics)
        {
            return MeasureGraphics.MeasureString(Text, Font, Width).ToSize();
        }
    }
    protected Size Measure(string text)
    {
        lock (MeasureGraphics)
        {
            return MeasureGraphics.MeasureString(text, Font, Width).ToSize();
        }
    }

    #endregion


    #region " DrawPixel "


    private SolidBrush DrawPixelBrush;
    protected void DrawPixel(Color c1, int x, int y)
    {
        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
        }
        else
        {
            DrawPixelBrush = new SolidBrush(c1);
            G.FillRectangle(DrawPixelBrush, x, y, 1, 1);
        }
    }

    #endregion

    #region " DrawCorners "


    private SolidBrush DrawCornersBrush;
    protected void DrawCorners(Color c1, int offset)
    {
        DrawCorners(c1, 0, 0, Width, Height, offset);
    }
    protected void DrawCorners(Color c1, Rectangle r1, int offset)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset);
    }
    protected void DrawCorners(Color c1, int x, int y, int width, int height, int offset)
    {
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawCorners(Color c1)
    {
        DrawCorners(c1, 0, 0, Width, Height);
    }
    protected void DrawCorners(Color c1, Rectangle r1)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height);
    }
    protected void DrawCorners(Color c1, int x, int y, int width, int height)
    {
        if (_NoRounding)
            return;

        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
            B.SetPixel(x + (width - 1), y, c1);
            B.SetPixel(x, y + (height - 1), c1);
            B.SetPixel(x + (width - 1), y + (height - 1), c1);
        }
        else
        {
            DrawCornersBrush = new SolidBrush(c1);
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1);
        }
    }

    #endregion

    #region " DrawBorders "

    protected void DrawBorders(Pen p1, int offset)
    {
        DrawBorders(p1, 0, 0, Width, Height, offset);
    }
    protected void DrawBorders(Pen p1, Rectangle r, int offset)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset);
    }
    protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset)
    {
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawBorders(Pen p1)
    {
        DrawBorders(p1, 0, 0, Width, Height);
    }
    protected void DrawBorders(Pen p1, Rectangle r)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height);
    }
    protected void DrawBorders(Pen p1, int x, int y, int width, int height)
    {
        G.DrawRectangle(p1, x, y, width - 1, height - 1);
    }

    #endregion

    #region " DrawText "

    private Point DrawTextPoint;

    private Size DrawTextSize;
    protected void DrawText(Brush b1, HorizontalAlignment a, int x, int y)
    {
        DrawText(b1, Text, a, x, y);
    }
    protected void DrawText(Brush b1, string text, HorizontalAlignment a, int x, int y)
    {
        if (text.Length == 0)
            return;

        DrawTextSize = Measure(text);
        DrawTextPoint = new Point(Width / 2 - DrawTextSize.Width / 2, Header / 2 - DrawTextSize.Height / 2);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y);
                break;
        }
    }

    protected void DrawText(Brush b1, Point p1)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, p1);
    }
    protected void DrawText(Brush b1, int x, int y)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, x, y);
    }

    #endregion

    #region " DrawImage "


    private Point DrawImagePoint;
    protected void DrawImage(HorizontalAlignment a, int x, int y)
    {
        DrawImage(_Image, a, x, y);
    }
    protected void DrawImage(Image image, HorizontalAlignment a, int x, int y)
    {
        if (image == null)
            return;
        DrawImagePoint = new Point(Width / 2 - image.Width / 2, Header / 2 - image.Height / 2);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Center:
                G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Right:
                G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
        }
    }

    protected void DrawImage(Point p1)
    {
        DrawImage(_Image, p1.X, p1.Y);
    }
    protected void DrawImage(int x, int y)
    {
        DrawImage(_Image, x, y);
    }

    protected void DrawImage(Image image, Point p1)
    {
        DrawImage(image, p1.X, p1.Y);
    }
    protected void DrawImage(Image image, int x, int y)
    {
        if (image == null)
            return;
        G.DrawImage(image, x, y, image.Width, image.Height);
    }

    #endregion

    #region " DrawGradient "

    private LinearGradientBrush DrawGradientBrush;

    private Rectangle DrawGradientRectangle;
    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle);
    }
    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(ColorBlend blend, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, 90f);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawGradient(ColorBlend blend, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, angle);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }


    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle);
    }
    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(Color c1, Color c2, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawGradient(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, angle);
        G.FillRectangle(DrawGradientBrush, r);
    }

    #endregion

    #region " DrawRadial "

    private GraphicsPath DrawRadialPath;
    private PathGradientBrush DrawRadialBrush1;
    private LinearGradientBrush DrawRadialBrush2;

    private Rectangle DrawRadialRectangle;
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, width / 2, height / 2);
    }
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, Point center)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, center.X, center.Y);
    }
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, int cx, int cy)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, cx, cy);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r)
    {
        DrawRadial(blend, r, r.Width / 2, r.Height / 2);
    }
    public void DrawRadial(ColorBlend blend, Rectangle r, Point center)
    {
        DrawRadial(blend, r, center.X, center.Y);
    }
    public void DrawRadial(ColorBlend blend, Rectangle r, int cx, int cy)
    {
        DrawRadialPath.Reset();
        DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1);

        DrawRadialBrush1 = new PathGradientBrush(DrawRadialPath);
        DrawRadialBrush1.CenterPoint = new Point(r.X + cx, r.Y + cy);
        DrawRadialBrush1.InterpolationColors = blend;

        if (G.SmoothingMode == SmoothingMode.AntiAlias)
        {
            G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3);
        }
        else
        {
            G.FillEllipse(DrawRadialBrush1, r);
        }
    }


    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawGradientRectangle);
    }
    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawRadial(Color c1, Color c2, Rectangle r)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawRadial(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, angle);
        G.FillEllipse(DrawGradientBrush, r);
    }

    #endregion

    #region " CreateRound "

    private GraphicsPath CreateRoundPath;

    private Rectangle CreateRoundRectangle;
    public GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
    {
        CreateRoundRectangle = new Rectangle(x, y, width, height);
        return CreateRound(CreateRoundRectangle, slope);
    }

    public GraphicsPath CreateRound(Rectangle r, int slope)
    {
        CreateRoundPath = new GraphicsPath(FillMode.Winding);
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
        CreateRoundPath.CloseFigure();
        return CreateRoundPath;
    }

    #endregion

}

abstract class ThemeControl154 : Control
{


    #region " Initialization "

    protected Graphics G;

    protected Bitmap B;
    public ThemeControl154()
    {
        SetStyle((ControlStyles)139270, true);

        _ImageSize = Size.Empty;
        Font = new Font("Verdana", 8);

        MeasureBitmap = new Bitmap(1, 1);
        MeasureGraphics = Graphics.FromImage(MeasureBitmap);

        DrawRadialPath = new GraphicsPath();

        InvalidateCustimization();
        //Remove?
    }

    protected override sealed void OnHandleCreated(EventArgs e)
    {
        InvalidateCustimization();
        ColorHook();

        if (!(_LockWidth == 0))
            Width = _LockWidth;
        if (!(_LockHeight == 0))
            Height = _LockHeight;

        Transparent = _Transparent;
        if (_Transparent && _BackColor)
            BackColor = Color.Transparent;

        base.OnHandleCreated(e);
    }

    private bool DoneCreation;
    protected override sealed void OnParentChanged(EventArgs e)
    {
        if (Parent != null)
        {
            OnCreation();
            DoneCreation = true;
            InvalidateTimer();
        }

        base.OnParentChanged(e);
    }

    #endregion

    private void DoAnimation(bool i)
    {
        OnAnimation();
        if (i)
            Invalidate();
    }

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;

        if (_Transparent)
        {
            PaintHook();
            e.Graphics.DrawImage(B, 0, 0);
        }
        else
        {
            G = e.Graphics;
            PaintHook();
        }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        ThemeShare.RemoveAnimationCallback(DoAnimation);
        base.OnHandleDestroyed(e);
    }

    #region " Size Handling "

    protected override sealed void OnSizeChanged(EventArgs e)
    {
        if (_Transparent)
        {
            InvalidateBitmap();
        }

        Invalidate();
        base.OnSizeChanged(e);
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        if (!(_LockWidth == 0))
            width = _LockWidth;
        if (!(_LockHeight == 0))
            height = _LockHeight;
        base.SetBoundsCore(x, y, width, height, specified);
    }

    #endregion

    #region " State Handling "

    private bool InPosition;
    protected override void OnMouseEnter(EventArgs e)
    {
        InPosition = true;
        SetState(MouseState.Over);
        base.OnMouseEnter(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (InPosition)
            SetState(MouseState.Over);
        base.OnMouseUp(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
            SetState(MouseState.Down);
        base.OnMouseDown(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        InPosition = false;
        SetState(MouseState.None);
        base.OnMouseLeave(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        if (Enabled)
            SetState(MouseState.None);
        else
            SetState(MouseState.Block);
        base.OnEnabledChanged(e);
    }

    protected MouseState State;
    private void SetState(MouseState current)
    {
        State = current;
        Invalidate();
    }

    #endregion


    #region " Base Properties "

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Color ForeColor
    {
        get { return Color.Empty; }
        set { }
    }
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Image BackgroundImage
    {
        get { return null; }
        set { }
    }
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override ImageLayout BackgroundImageLayout
    {
        get { return ImageLayout.None; }
        set { }
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
    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            Invalidate();
        }
    }

    private bool _BackColor;
    [Category("Misc")]
    public override Color BackColor
    {
        get { return base.BackColor; }
        set
        {
            if (!IsHandleCreated && value == Color.Transparent)
            {
                _BackColor = true;
                return;
            }

            base.BackColor = value;
            if (Parent != null)
                ColorHook();
        }
    }

    #endregion

    #region " Public Properties "

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
            if (value == null)
            {
                _ImageSize = Size.Empty;
            }
            else
            {
                _ImageSize = value.Size;
            }

            _Image = value;
            Invalidate();
        }
    }

    private bool _Transparent;
    public bool Transparent
    {
        get { return _Transparent; }
        set
        {
            _Transparent = value;
            if (!IsHandleCreated)
                return;

            if (!value && !(BackColor.A == 255))
            {
                throw new Exception("Unable to change value to false while a transparent BackColor is in use.");
            }

            SetStyle(ControlStyles.Opaque, !value);
            SetStyle(ControlStyles.SupportsTransparentBackColor, value);

            if (value)
                InvalidateBitmap();
            else
                B = null;
            Invalidate();
        }
    }

    private Dictionary<string, Color> Items = new Dictionary<string, Color>();
    public Bloom[] Colors
    {
        get
        {
            List<Bloom> T = new List<Bloom>();
            Dictionary<string, Color>.Enumerator E = Items.GetEnumerator();

            while (E.MoveNext())
            {
                T.Add(new Bloom(E.Current.Key, E.Current.Value));
            }

            return T.ToArray();
        }
        set
        {
            foreach (Bloom B in value)
            {
                if (Items.ContainsKey(B.Name))
                    Items[B.Name] = B.Value;
            }

            InvalidateCustimization();
            ColorHook();
            Invalidate();
        }
    }

    private string _Customization;
    public string Customization
    {
        get { return _Customization; }
        set
        {
            if (value == _Customization)
                return;

            byte[] Data = null;
            Bloom[] Items = Colors;

            try
            {
                Data = Convert.FromBase64String(value);
                for (int I = 0; I <= Items.Length - 1; I++)
                {
                    Items[I].Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4));
                }
            }
            catch
            {
                return;
            }

            _Customization = value;

            Colors = Items;
            ColorHook();
            Invalidate();
        }
    }

    #endregion

    #region " Private Properties "

    private Size _ImageSize;
    protected Size ImageSize
    {
        get { return _ImageSize; }
    }

    private int _LockWidth;
    protected int LockWidth
    {
        get { return _LockWidth; }
        set
        {
            _LockWidth = value;
            if (!(LockWidth == 0) && IsHandleCreated)
                Width = LockWidth;
        }
    }

    private int _LockHeight;
    protected int LockHeight
    {
        get { return _LockHeight; }
        set
        {
            _LockHeight = value;
            if (!(LockHeight == 0) && IsHandleCreated)
                Height = LockHeight;
        }
    }

    private bool _IsAnimated;
    protected bool IsAnimated
    {
        get { return _IsAnimated; }
        set
        {
            _IsAnimated = value;
            InvalidateTimer();
        }
    }

    #endregion


    #region " Property Helpers "

    protected Pen GetPen(string name)
    {
        return new Pen(Items[name]);
    }
    protected Pen GetPen(string name, float width)
    {
        return new Pen(Items[name], width);
    }

    protected SolidBrush GetBrush(string name)
    {
        return new SolidBrush(Items[name]);
    }

    protected Color GetColor(string name)
    {
        return Items[name];
    }

    protected void SetColor(string name, Color value)
    {
        if (Items.ContainsKey(name))
            Items[name] = value;
        else
            Items.Add(name, value);
    }
    protected void SetColor(string name, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(r, g, b));
    }
    protected void SetColor(string name, byte a, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(a, r, g, b));
    }
    protected void SetColor(string name, byte a, Color value)
    {
        SetColor(name, Color.FromArgb(a, value));
    }

    private void InvalidateBitmap()
    {
        if (Width == 0 || Height == 0)
            return;
        B = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
        G = Graphics.FromImage(B);
    }

    private void InvalidateCustimization()
    {
        MemoryStream M = new MemoryStream(Items.Count * 4);

        foreach (Bloom B in Colors)
        {
            M.Write(BitConverter.GetBytes(B.Value.ToArgb()), 0, 4);
        }

        M.Close();
        _Customization = Convert.ToBase64String(M.ToArray());
    }

    private void InvalidateTimer()
    {
        if (DesignMode || !DoneCreation)
            return;

        if (_IsAnimated)
        {
            ThemeShare.AddAnimationCallback(DoAnimation);
        }
        else
        {
            ThemeShare.RemoveAnimationCallback(DoAnimation);
        }
    }
    #endregion


    #region " User Hooks "

    protected abstract void ColorHook();
    protected abstract void PaintHook();

    protected virtual void OnCreation()
    {
    }

    protected virtual void OnAnimation()
    {
    }

    #endregion


    #region " Offset "

    private Rectangle OffsetReturnRectangle;
    protected Rectangle Offset(Rectangle r, int amount)
    {
        OffsetReturnRectangle = new Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2));
        return OffsetReturnRectangle;
    }

    private Size OffsetReturnSize;
    protected Size Offset(Size s, int amount)
    {
        OffsetReturnSize = new Size(s.Width + amount, s.Height + amount);
        return OffsetReturnSize;
    }

    private Point OffsetReturnPoint;
    protected Point Offset(Point p, int amount)
    {
        OffsetReturnPoint = new Point(p.X + amount, p.Y + amount);
        return OffsetReturnPoint;
    }

    #endregion

    #region " Center "


    private Point CenterReturn;
    protected Point Center(Rectangle p, Rectangle c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X + c.X, (p.Height / 2 - c.Height / 2) + p.Y + c.Y);
        return CenterReturn;
    }
    protected Point Center(Rectangle p, Size c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X, (p.Height / 2 - c.Height / 2) + p.Y);
        return CenterReturn;
    }

    protected Point Center(Rectangle child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }
    protected Point Center(Size child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }
    protected Point Center(int childWidth, int childHeight)
    {
        return Center(Width, Height, childWidth, childHeight);
    }

    protected Point Center(Size p, Size c)
    {
        return Center(p.Width, p.Height, c.Width, c.Height);
    }

    protected Point Center(int pWidth, int pHeight, int cWidth, int cHeight)
    {
        CenterReturn = new Point(pWidth / 2 - cWidth / 2, pHeight / 2 - cHeight / 2);
        return CenterReturn;
    }

    #endregion

    #region " Measure "

    private Bitmap MeasureBitmap;
    //TODO: Potential issues during multi-threading.
    private Graphics MeasureGraphics;

    protected Size Measure()
    {
        return MeasureGraphics.MeasureString(Text, Font, Width).ToSize();
    }
    protected Size Measure(string text)
    {
        return MeasureGraphics.MeasureString(text, Font, Width).ToSize();
    }

    #endregion


    #region " DrawPixel "


    private SolidBrush DrawPixelBrush;
    protected void DrawPixel(Color c1, int x, int y)
    {
        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
        }
        else
        {
            DrawPixelBrush = new SolidBrush(c1);
            G.FillRectangle(DrawPixelBrush, x, y, 1, 1);
        }
    }

    #endregion

    #region " DrawCorners "


    private SolidBrush DrawCornersBrush;
    protected void DrawCorners(Color c1, int offset)
    {
        DrawCorners(c1, 0, 0, Width, Height, offset);
    }
    protected void DrawCorners(Color c1, Rectangle r1, int offset)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset);
    }
    protected void DrawCorners(Color c1, int x, int y, int width, int height, int offset)
    {
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawCorners(Color c1)
    {
        DrawCorners(c1, 0, 0, Width, Height);
    }
    protected void DrawCorners(Color c1, Rectangle r1)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height);
    }
    protected void DrawCorners(Color c1, int x, int y, int width, int height)
    {
        if (_NoRounding)
            return;

        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
            B.SetPixel(x + (width - 1), y, c1);
            B.SetPixel(x, y + (height - 1), c1);
            B.SetPixel(x + (width - 1), y + (height - 1), c1);
        }
        else
        {
            DrawCornersBrush = new SolidBrush(c1);
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1);
        }
    }

    #endregion

    #region " DrawBorders "

    protected void DrawBorders(Pen p1, int offset)
    {
        DrawBorders(p1, 0, 0, Width, Height, offset);
    }
    protected void DrawBorders(Pen p1, Rectangle r, int offset)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset);
    }
    protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset)
    {
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawBorders(Pen p1)
    {
        DrawBorders(p1, 0, 0, Width, Height);
    }
    protected void DrawBorders(Pen p1, Rectangle r)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height);
    }
    protected void DrawBorders(Pen p1, int x, int y, int width, int height)
    {
        G.DrawRectangle(p1, x, y, width - 1, height - 1);
    }

    #endregion

    #region " DrawText "

    private Point DrawTextPoint;

    private Size DrawTextSize;
    protected void DrawText(Brush b1, HorizontalAlignment a, int x, int y)
    {
        DrawText(b1, Text, a, x, y);
    }
    protected void DrawText(Brush b1, string text, HorizontalAlignment a, int x, int y)
    {
        if (text.Length == 0)
            return;

        DrawTextSize = Measure(text);
        DrawTextPoint = Center(DrawTextSize);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y);
                break;
        }
    }

    protected void DrawText(Brush b1, Point p1)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, p1);
    }
    protected void DrawText(Brush b1, int x, int y)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, x, y);
    }

    #endregion

    #region " DrawImage "


    private Point DrawImagePoint;
    protected void DrawImage(HorizontalAlignment a, int x, int y)
    {
        DrawImage(_Image, a, x, y);
    }
    protected void DrawImage(Image image, HorizontalAlignment a, int x, int y)
    {
        if (image == null)
            return;
        DrawImagePoint = Center(image.Size);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Center:
                G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Right:
                G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
        }
    }

    protected void DrawImage(Point p1)
    {
        DrawImage(_Image, p1.X, p1.Y);
    }
    protected void DrawImage(int x, int y)
    {
        DrawImage(_Image, x, y);
    }

    protected void DrawImage(Image image, Point p1)
    {
        DrawImage(image, p1.X, p1.Y);
    }
    protected void DrawImage(Image image, int x, int y)
    {
        if (image == null)
            return;
        G.DrawImage(image, x, y, image.Width, image.Height);
    }

    #endregion

    #region " DrawGradient "

    private LinearGradientBrush DrawGradientBrush;

    private Rectangle DrawGradientRectangle;
    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle);
    }
    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(ColorBlend blend, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, 90f);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawGradient(ColorBlend blend, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, angle);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }


    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle);
    }
    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(Color c1, Color c2, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawGradient(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, angle);
        G.FillRectangle(DrawGradientBrush, r);
    }

    #endregion

    #region " DrawRadial "

    private GraphicsPath DrawRadialPath;
    private PathGradientBrush DrawRadialBrush1;
    private LinearGradientBrush DrawRadialBrush2;

    private Rectangle DrawRadialRectangle;
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, width / 2, height / 2);
    }
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, Point center)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, center.X, center.Y);
    }
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, int cx, int cy)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, cx, cy);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r)
    {
        DrawRadial(blend, r, r.Width / 2, r.Height / 2);
    }
    public void DrawRadial(ColorBlend blend, Rectangle r, Point center)
    {
        DrawRadial(blend, r, center.X, center.Y);
    }
    public void DrawRadial(ColorBlend blend, Rectangle r, int cx, int cy)
    {
        DrawRadialPath.Reset();
        DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1);

        DrawRadialBrush1 = new PathGradientBrush(DrawRadialPath);
        DrawRadialBrush1.CenterPoint = new Point(r.X + cx, r.Y + cy);
        DrawRadialBrush1.InterpolationColors = blend;

        if (G.SmoothingMode == SmoothingMode.AntiAlias)
        {
            G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3);
        }
        else
        {
            G.FillEllipse(DrawRadialBrush1, r);
        }
    }


    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawRadialRectangle);
    }
    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawRadialRectangle, angle);
    }

    protected void DrawRadial(Color c1, Color c2, Rectangle r)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillEllipse(DrawRadialBrush2, r);
    }
    protected void DrawRadial(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, angle);
        G.FillEllipse(DrawRadialBrush2, r);
    }

    #endregion

    #region " CreateRound "

    private GraphicsPath CreateRoundPath;

    private Rectangle CreateRoundRectangle;
    public GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
    {
        CreateRoundRectangle = new Rectangle(x, y, width, height);
        return CreateRound(CreateRoundRectangle, slope);
    }

    public GraphicsPath CreateRound(Rectangle r, int slope)
    {
        CreateRoundPath = new GraphicsPath(FillMode.Winding);
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
        CreateRoundPath.CloseFigure();
        return CreateRoundPath;
    }

    #endregion

}

static class ThemeShare
{

    #region " Animation "

    private static int Frames;
    private static bool Invalidate;

    public static PrecisionTimer ThemeTimer = new PrecisionTimer();
    //1000 / 50 = 20 FPS
    private const int FPS = 50;

    private const int Rate = 50;
    public delegate void AnimationDelegate(bool invalidate);


    private static List<AnimationDelegate> Callbacks = new List<AnimationDelegate>();
    private static void HandleCallbacks(IntPtr state, bool reserve)
    {
        Invalidate = (Frames >= FPS);
        if (Invalidate)
            Frames = 0;

        lock (Callbacks)
        {
            for (int I = 0; I <= Callbacks.Count - 1; I++)
            {
                Callbacks[I].Invoke(Invalidate);
            }
        }

        Frames += Rate;
    }

    private static void InvalidateThemeTimer()
    {
        if (Callbacks.Count == 0)
        {
            ThemeTimer.Delete();
        }
        else
        {
            ThemeTimer.Create(0, Rate, HandleCallbacks);
        }
    }

    public static void AddAnimationCallback(AnimationDelegate callback)
    {
        lock (Callbacks)
        {
            if (Callbacks.Contains(callback))
                return;

            Callbacks.Add(callback);
            InvalidateThemeTimer();
        }
    }

    public static void RemoveAnimationCallback(AnimationDelegate callback)
    {
        lock (Callbacks)
        {
            if (!Callbacks.Contains(callback))
                return;

            Callbacks.Remove(callback);
            InvalidateThemeTimer();
        }
    }

    #endregion

}

enum MouseState : byte
{
    None = 0,
    Over = 1,
    Down = 2,
    Block = 3
}

struct Bloom
{

    public string _Name;
    public string Name
    {
        get { return _Name; }
    }

    private Color _Value;
    public Color Value
    {
        get { return _Value; }
        set { _Value = value; }
    }

    public string ValueHex
    {
        get { return string.Concat("#", _Value.R.ToString("X2", null), _Value.G.ToString("X2", null), _Value.B.ToString("X2", null)); }
        set
        {
            try
            {
                _Value = ColorTranslator.FromHtml(value);
            }
            catch
            {
                return;
            }
        }
    }


    public Bloom(string name, Color value)
    {
        _Name = name;
        _Value = value;
    }
}

//------------------
//Creator: aeonhack
//Site: elitevs.net
//Created: 11/30/2011
//Changed: 11/30/2011
//Version: 1.0.0
//------------------
class PrecisionTimer : IDisposable
{

    private bool _Enabled;
    public bool Enabled
    {
        get { return _Enabled; }
    }

    private IntPtr Handle;

    private TimerDelegate TimerCallback;
    [DllImport("kernel32.dll", EntryPoint = "CreateTimerQueueTimer")]
    private static extern bool CreateTimerQueueTimer(ref IntPtr handle, IntPtr queue, TimerDelegate callback, IntPtr state, uint dueTime, uint period, uint flags);

    [DllImport("kernel32.dll", EntryPoint = "DeleteTimerQueueTimer")]
    private static extern bool DeleteTimerQueueTimer(IntPtr queue, IntPtr handle, IntPtr callback);

    public delegate void TimerDelegate(IntPtr r1, bool r2);

    public void Create(uint dueTime, uint period, TimerDelegate callback)
    {
        if (_Enabled)
            return;

        TimerCallback = callback;
        bool Success = CreateTimerQueueTimer(ref Handle, IntPtr.Zero, TimerCallback, IntPtr.Zero, dueTime, period, 0);

        if (!Success)
            ThrowNewException("CreateTimerQueueTimer");
        _Enabled = Success;
    }

    public void Delete()
    {
        if (!_Enabled)
            return;
        bool Success = DeleteTimerQueueTimer(IntPtr.Zero, Handle, IntPtr.Zero);

        if (!Success && !(Marshal.GetLastWin32Error() == 997))
        {
            ThrowNewException("DeleteTimerQueueTimer");
        }

        _Enabled = !Success;
    }

    private void ThrowNewException(string name)
    {
        throw new Exception(string.Format("{0} failed. Win32Error: {1}", name, Marshal.GetLastWin32Error()));
    }

    public void Dispose()
    {
        Delete();
    }
}

#endregion

//#############################################
//#               ! CREDITS !                 #
//#############################################
//# Themebase 1.5.4: Aeonhack                 #
//# Theme: Mavamaarten                        #
//#############################################
//# Thanks to Tyrant (Reptile) for helping me #
//# with the progressbar animation !          #
//#############################################
//# Converted to C# by: Ethernal Five         #
//# On 04/04/2012                             #
//#############################################

class CrystalClearThemeContainer : ThemeContainer154
{
    Color G1;
    Color G2;
    Color Glow;
    Color BG;
    Color Edge;

    private RoundingType _Rounding;
    public enum RoundingType : int
    {
        TypeOne = 1,
        TypeTwo = 2,
        None = 0
    }

    protected override void ColorHook()
    {
        G1 = GetColor("Gradient 1");
        G2 = GetColor("Gradient 2");
        Glow = GetColor("Glow");
        BG = GetColor("Background");
        Edge = GetColor("Edges");
    }

    public RoundingType Rounding
    {
        get { return _Rounding; }
        set { _Rounding = value; }
    }


    public CrystalClearThemeContainer()
    {
        SetColor("Gradient 1", Color.FromArgb(230, 230, 230));
        SetColor("Gradient 2", Color.FromArgb(210, 210, 210));
        SetColor("Glow", Color.FromArgb(230, 230, 230));
        SetColor("Background", Color.FromArgb(230, 230, 230));
        SetColor("Edges", Color.FromArgb(170, 170, 170));
        TransparencyKey = Color.Fuchsia;
        MinimumSize = new Size(175, 150);
        BackColor = Color.FromArgb(230, 230, 230);
    }


    protected override void PaintHook()
    {
        G.Clear(BG);

        //Draw titlebar gradient
        LinearGradientBrush LB = new LinearGradientBrush(new Rectangle(new Point(1, 1), new Size(Width - 2, 25)), G1, G2, 90f);
        G.FillRectangle(LB, new Rectangle(new Point(1, 1), new Size(Width - 2, 25)));

        //Draw glow
        G.FillRectangle(new SolidBrush(BG), new Rectangle(new Point(1, 1), new Size(Width - 2, 11)));

        //Draw outline + rounded corners
        G.DrawRectangle(new Pen(Edge), new Rectangle(0, 0, Width - 1, Height - 1));
        G.DrawLine(new Pen(Edge), new Point(0, 26), new Point(Width, 26));

        switch (_Rounding)
        {
            case RoundingType.TypeOne:
                //////left upper corner
                DrawPixel(Color.Fuchsia, 0, 0);
                DrawPixel(Color.Fuchsia, 1, 0);
                DrawPixel(Color.Fuchsia, 0, 1);
                DrawPixel(Edge, 1, 1);
                //////right upper corner
                DrawPixel(Color.Fuchsia, Width - 1, 0);
                DrawPixel(Color.Fuchsia, Width - 2, 0);
                DrawPixel(Color.Fuchsia, Width - 1, 1);
                DrawPixel(Edge, Width - 2, 1);
                //////left bottom corner
                DrawPixel(Color.Fuchsia, 0, Height - 1);
                DrawPixel(Color.Fuchsia, 1, Height - 1);
                DrawPixel(Color.Fuchsia, 0, Height - 2);
                DrawPixel(Edge, 1, Height - 2);
                //////right bottom corner
                DrawPixel(Color.Fuchsia, Width - 1, Height - 1);
                DrawPixel(Color.Fuchsia, Width - 2, Height - 1);
                DrawPixel(Color.Fuchsia, Width - 1, Height - 2);
                DrawPixel(Edge, Width - 2, Height - 2);
                break;
            case RoundingType.TypeTwo:
                //////left upper corner
                DrawPixel(Color.Fuchsia, 0, 0);
                DrawPixel(Color.Fuchsia, 1, 0);
                DrawPixel(Color.Fuchsia, 2, 0);
                DrawPixel(Color.Fuchsia, 3, 0);
                DrawPixel(Color.Fuchsia, 0, 1);
                DrawPixel(Color.Fuchsia, 0, 2);
                DrawPixel(Color.Fuchsia, 0, 3);
                DrawPixel(Color.Fuchsia, 1, 1);
                DrawPixel(Edge, 2, 1);
                DrawPixel(Edge, 3, 1);
                DrawPixel(Edge, 1, 2);
                DrawPixel(Edge, 1, 3);
                //////right upper corner
                DrawPixel(Color.Fuchsia, Width - 1, 0);
                DrawPixel(Color.Fuchsia, Width - 2, 0);
                DrawPixel(Color.Fuchsia, Width - 3, 0);
                DrawPixel(Color.Fuchsia, Width - 4, 0);
                DrawPixel(Color.Fuchsia, Width - 1, 1);
                DrawPixel(Color.Fuchsia, Width - 1, 2);
                DrawPixel(Color.Fuchsia, Width - 1, 3);
                DrawPixel(Color.Fuchsia, Width - 2, 1);
                DrawPixel(Edge, Width - 3, 1);
                DrawPixel(Edge, Width - 4, 1);
                DrawPixel(Edge, Width - 2, 2);
                DrawPixel(Edge, Width - 2, 3);
                //////left bottom corner
                DrawPixel(Color.Fuchsia, 0, Height - 1);
                DrawPixel(Color.Fuchsia, 0, Height - 2);
                DrawPixel(Color.Fuchsia, 0, Height - 3);
                DrawPixel(Color.Fuchsia, 0, Height - 4);
                DrawPixel(Color.Fuchsia, 1, Height - 1);
                DrawPixel(Color.Fuchsia, 2, Height - 1);
                DrawPixel(Color.Fuchsia, 3, Height - 1);
                DrawPixel(Color.Fuchsia, 1, Height - 2);
                DrawPixel(Edge, 2, Height - 2);
                DrawPixel(Edge, 3, Height - 2);
                DrawPixel(Edge, 1, Height - 3);
                DrawPixel(Edge, 1, Height - 4);
                //////right bottom corner
                DrawPixel(Color.Fuchsia, Width - 1, Height - 1);
                DrawPixel(Color.Fuchsia, Width - 1, Height - 2);
                DrawPixel(Color.Fuchsia, Width - 1, Height - 3);
                DrawPixel(Color.Fuchsia, Width - 1, Height - 4);
                DrawPixel(Color.Fuchsia, Width - 2, Height - 1);
                DrawPixel(Color.Fuchsia, Width - 3, Height - 1);
                DrawPixel(Color.Fuchsia, Width - 4, Height - 1);
                DrawPixel(Color.Fuchsia, Width - 2, Height - 2);
                DrawPixel(Edge, Width - 3, Height - 2);
                DrawPixel(Edge, Width - 4, Height - 2);
                DrawPixel(Edge, Width - 2, Height - 3);
                DrawPixel(Edge, Width - 2, Height - 4);
                break;
        }



        //Draw title & icon
        G.DrawString(FindForm().Text, new Font("Segoe UI", 9), Brushes.Black, new Point(27, 5));
        G.DrawIcon(FindForm().Icon, new Rectangle(7, 6, 16, 16));
    }
}
class CrystalClearControlBox : ThemeControl154
{
    private int X;
    Color BG;
    Pen Edge;
    Color Icons;

    SolidBrush glow;
    int a;
    int b;

    int c;
    protected override void ColorHook()
    {
        Icons = GetColor("Icons");
        BG = GetColor("Background");
        Edge = GetPen("Button edge color");
        glow = GetBrush("Glow");
    }

    public CrystalClearControlBox()
    {
        IsAnimated = true;
        SetColor("Icons", Color.FromArgb(100, 100, 100));
        SetColor("Background", Color.FromArgb(231, 231, 231));
        SetColor("Button edge color", Color.FromArgb(165, 165, 165));
        SetColor("Glow", Color.FromArgb(240, 240, 240));
        this.Size = new Size(84, 18);
        this.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    }

    protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.X;
        Invalidate();
    }

    protected override void OnClick(System.EventArgs e)
    {
        base.OnClick(e);
        if (X <= 22)
        {
            FindForm().WindowState = FormWindowState.Minimized;
        }
        else if (X > 22 & X <= 46)
        {
            if (FindForm().WindowState == FormWindowState.Maximized)
                FindForm().WindowState = FormWindowState.Normal;
            else
                FindForm().WindowState = FormWindowState.Maximized;
        }
        else
        {
            FindForm().Close();
        }
    }

    protected override void OnAnimation()
    {
        base.OnAnimation();
        switch (State)
        {
            case MouseState.Over:
                if (a < 24 & X <= 22)
                {
                    a += 4;
                    if (b > 0)
                    {
                        b -= 4;
                    }
                    if (c > 0)
                    {
                        c -= 4;
                    }
                    if (b < 0)
                        b = 0;
                    if (c < 0)
                        c = 0;
                    Invalidate();
                    Application.DoEvents();
                }

                if (b < 24 & X > 22 & X <= 46)
                {
                    b += 4;
                    if (a > 0)
                    {
                        a -= 4;
                    }
                    if (a < 0)
                        a = 0;
                    if (c > 0)
                    {
                        c -= 4;
                    }
                    if (c < 0)
                        c = 0;
                    Invalidate();
                    Application.DoEvents();
                }

                if (c < 32 & X > 46)
                {
                    c += 4;
                    if (a > 0)
                    {
                        a -= 4;
                    }
                    if (b > 0)
                    {
                        b -= 4;
                    }
                    if (a < 0)
                        a = 0;
                    if (b < 0)
                        b = 0;
                    Invalidate();
                    Application.DoEvents();
                }

                break;
            case MouseState.None:
                if (a > 0)
                {
                    a -= 4;
                }
                if (b > 0)
                {
                    b -= 4;
                }
                if (c > 0)
                {
                    c -= 4;
                }
                if (a < 0)
                    a = 0;
                if (b < 0)
                    b = 0;
                if (c < 0)
                    c = 0;
                Invalidate();
                Application.DoEvents();
                break;
        }
    }

    protected override void PaintHook()
    {
        //Draw outer edge
        G.Clear(BG);

        //Fill buttons
        G.FillRectangle(glow, new Rectangle(0, 0, 21, 8));
        //min button
        G.FillRectangle(glow, new Rectangle(23, 0, 22, 8));
        //max button
        G.FillRectangle(glow, new Rectangle(47, 0, 36, 8));
        //X button

        //Draw button outlines
        G.DrawRectangle(Edge, new Rectangle(0, 0, 21, 17));
        //min button
        G.DrawRectangle(Edge, new Rectangle(23, 0, 22, 17));
        //max button
        G.DrawRectangle(Edge, new Rectangle(47, 0, 36, 17));
        //X button

        //Mouse states
        SolidBrush SB1 = new SolidBrush(Color.FromArgb(a, Color.Cyan));
        SolidBrush SB1_ = new SolidBrush(Color.FromArgb(b, Color.Cyan));
        SolidBrush SB2 = new SolidBrush(Color.FromArgb(c, Color.OrangeRed));
        SolidBrush SB3 = new SolidBrush(Color.FromArgb(20, Color.Black));
        switch (State)
        {
            case MouseState.Down:
                if (X <= 22)
                {
                    a = 0;
                    G.FillRectangle(SB3, new Rectangle(0, 0, 21, 17));
                }
                else if (X > 22 & X <= 46)
                {
                    b = 0;
                    G.FillRectangle(SB3, new Rectangle(23, 0, 22, 17));
                }
                else
                {
                    c = 0;
                    G.FillRectangle(SB3, new Rectangle(47, 0, 36, 17));
                }
                break;
            default:
                if (X <= 22)
                {
                    G.FillRectangle(SB1, new Rectangle(0, 0, 21, 17));
                }
                else if (X > 22 & X <= 46)
                {
                    G.FillRectangle(SB1_, new Rectangle(23, 0, 22, 17));
                }
                else
                {
                    G.FillRectangle(SB2, new Rectangle(47, 0, 36, 17));
                }
                break;
        }

        //Draw icons
        G.DrawString("0", new Font("Marlett", 8.25F), GetBrush("Icons"), new Point(5, 4));
        if (FindForm().WindowState != FormWindowState.Maximized)
            G.DrawString("1", new Font("Marlett", 8), GetBrush("Icons"), new Point(28, 4));
        else
            G.DrawString("2", new Font("Marlett", 8.25F), GetBrush("Icons"), new Point(28, 4));
        G.DrawString("r", new Font("Marlett", 10), GetBrush("Icons"), new Point(56, 3));

        //Round min button corners
        DrawPixel(BG, 0, 0);
        DrawPixel(Edge.Color, 1, 1);
        DrawPixel(Color.FromArgb(210, 210, 210), 0, 17);
        DrawPixel(Edge.Color, 1, 16);

        //Round X button corners
        DrawPixel(BG, Width - 1, 0);
        DrawPixel(Edge.Color, Width - 2, 1);
        DrawPixel(Color.FromArgb(210, 210, 210), Width - 1, 17);
        DrawPixel(Edge.Color, Width - 2, 16);
    }
}
class CrystalClearButton : ThemeControl154
{
    Color G1;
    Color G2;
    Color Glow;
    Color Edge;
    Color TextColor;
    Color Hovercolor;

    int a = 0;
    protected override void ColorHook()
    {
        G1 = GetColor("Gradient 1");
        G2 = GetColor("Gradient 2");
        Glow = GetColor("Glow");
        Edge = GetColor("Edge");
        TextColor = GetColor("Text");
        Hovercolor = GetColor("HoverColor");
    }

    protected override void OnAnimation()
    {
        base.OnAnimation();
        switch (State)
        {
            case MouseState.Over:
                if (a < 40)
                {
                    a += 8;
                    Invalidate();
                    Application.DoEvents();
                }
                break;
            case MouseState.None:
                if (a > 0)
                {
                    a -= 10;
                    if (a < 0)
                        a = 0;
                    Invalidate();
                    Application.DoEvents();
                }
                break;
        }
    }

    protected override void PaintHook()
    {
        G.Clear(G1);
        LinearGradientBrush LGB = new LinearGradientBrush(new Rectangle(new Point(1, 1), new Size(Width - 2, Height - 2)), G1, G2, 90f);
        HatchBrush HB = new HatchBrush(HatchStyle.LightDownwardDiagonal, Color.FromArgb(7, Color.Black), Color.Transparent);
        G.FillRectangle(LGB, new Rectangle(new Point(1, 1), new Size(Width - 2, Height - 2)));
        G.FillRectangle(new SolidBrush(Glow), new Rectangle(new Point(1, 1), new Size(Width - 2, (Height / 2) - 3)));
        G.FillRectangle(HB, new Rectangle(new Point(1, 1), new Size(Width - 2, Height - 2)));

        if (State == MouseState.Over | State == MouseState.None)
        {
            SolidBrush SB = new SolidBrush(Color.FromArgb(a * 2, Color.White));
            G.FillRectangle(SB, new Rectangle(new Point(1, 1), new Size(Width - 2, Height - 2)));
        }
        else if (State == MouseState.Down)
        {
            SolidBrush SB = new SolidBrush(Color.FromArgb(2, Color.Black));
            G.FillRectangle(SB, new Rectangle(new Point(1, 1), new Size(Width - 2, Height - 2)));
        }

        G.DrawRectangle(new Pen(Edge), new Rectangle(new Point(1, 1), new Size(Width - 2, Height - 2)));
        StringFormat sf = new StringFormat();
        sf.LineAlignment = StringAlignment.Center;
        sf.Alignment = StringAlignment.Center;
        G.DrawString(Text, Font, GetBrush("Text"), new RectangleF(2, 2, this.Width - 5, this.Height - 4), sf);
    }

    public CrystalClearButton()
    {
        IsAnimated = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        SetColor("Gradient 1", 230, 230, 230);
        SetColor("Gradient 2", 210, 210, 210);
        SetColor("Glow", 230, 230, 230);
        SetColor("Edge", 170, 170, 170);
        SetColor("Text", Color.Black);
        SetColor("HoverColor", Color.White);
        Size = new Size(145, 25);
    }
}
[DefaultEvent("CheckedChanged")]
class CrystalClearCheckBox : ThemeControl154
{

    public CrystalClearCheckBox()
    {
        LockHeight = 17;
        SetColor("Text", Color.Black);
        SetColor("Gradient 1", 230, 230, 230);
        SetColor("Gradient 2", 210, 210, 210);
        SetColor("Glow", 230, 230, 230);
        SetColor("Edges", 170, 170, 170);
        SetColor("Backcolor", BackColor);
        Width = 160;
    }

    private int X;
    private Color TextColor;
    private Color G1;
    private Color G2;
    private Color Glow;
    private Color Edge;

    private Color BG;
    protected override void ColorHook()
    {
        TextColor = GetColor("Text");
        G1 = GetColor("Gradient 1");
        G2 = GetColor("Gradient 2");
        Glow = GetColor("Glow");
        Edge = GetColor("Edges");
        BG = GetColor("Backcolor");
    }

    protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.Location.X;
        Invalidate();
    }

    protected override void PaintHook()
    {
        G.Clear(BG);
        if (_Checked)
        {
            LinearGradientBrush LGB = new LinearGradientBrush(new Rectangle(new Point(0, 0), new Size(14, 14)), G1, G2, 90f);
            G.FillRectangle(LGB, new Rectangle(new Point(0, 0), new Size(14, 14)));
            G.FillRectangle(new SolidBrush(Glow), new Rectangle(new Point(0, 0), new Size(14, 7)));
        }
        else
        {
            LinearGradientBrush LGB = new LinearGradientBrush(new Rectangle(new Point(0, 0), new Size(14, 16)), G1, G2, 90f);
            G.FillRectangle(LGB, new Rectangle(new Point(0, 0), new Size(14, 14)));
            G.FillRectangle(new SolidBrush(Glow), new Rectangle(new Point(0, 0), new Size(14, 7)));
        }

        if (State == MouseState.Over & X < 15)
        {
            SolidBrush SB = new SolidBrush(Color.FromArgb(70, Color.White));
            G.FillRectangle(SB, new Rectangle(new Point(0, 0), new Size(14, 14)));
        }
        else if (State == MouseState.Down & X < 15)
        {
            SolidBrush SB = new SolidBrush(Color.FromArgb(10, Color.Black));
            G.FillRectangle(SB, new Rectangle(new Point(0, 0), new Size(14, 14)));
        }

        HatchBrush HB = new HatchBrush(HatchStyle.LightDownwardDiagonal, Color.FromArgb(7, Color.Black), Color.Transparent);
        G.FillRectangle(HB, new Rectangle(new Point(0, 0), new Size(14, 14)));
        G.DrawRectangle(new Pen(Edge), new Rectangle(new Point(0, 0), new Size(14, 14)));

        if (_Checked)
            G.DrawString("a", new Font("Marlett", 12), Brushes.Black, new Point(-3, -1));
        DrawText(new SolidBrush(TextColor), HorizontalAlignment.Left, 19, -1);
    }

    private bool _Checked;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            Invalidate();
        }
    }

    protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
    {
        _Checked = !_Checked;
        if (CheckedChanged != null)
        {
            CheckedChanged(this);
        }
        base.OnMouseDown(e);
    }

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);

}
[DefaultEvent("CheckedChanged")]
class CrystalClearRadioButton : ThemeControl154
{

    public CrystalClearRadioButton()
    {
        LockHeight = 17;
        SetColor("Text", Color.Black);
        SetColor("Gradient 1", 230, 230, 230);
        SetColor("Gradient 2", 210, 210, 210);
        SetColor("Glow", 230, 230, 230);
        SetColor("Edges", 170, 170, 170);
        SetColor("Backcolor", BackColor);
        SetColor("Bullet", 40, 40, 40);
        Width = 180;
    }

    private int X;
    private Color TextColor;
    private Color G1;
    private Color G2;
    private Color Glow;
    private Color Edge;

    private Color BG;
    protected override void ColorHook()
    {
        TextColor = GetColor("Text");
        G1 = GetColor("Gradient 1");
        G2 = GetColor("Gradient 2");
        Glow = GetColor("Glow");
        Edge = GetColor("Edges");
        BG = GetColor("Backcolor");
    }

    protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.Location.X;
        Invalidate();
    }

    protected override void PaintHook()
    {
        G.Clear(BG);
        G.SmoothingMode = SmoothingMode.HighQuality;
        if (_Checked)
        {
            LinearGradientBrush LGB = new LinearGradientBrush(new Rectangle(new Point(0, 0), new Size(14, 14)), G1, G2, 90f);
            G.FillEllipse(LGB, new Rectangle(new Point(0, 0), new Size(14, 14)));
            G.FillEllipse(new SolidBrush(Glow), new Rectangle(new Point(0, 0), new Size(14, 7)));
        }
        else
        {
            LinearGradientBrush LGB = new LinearGradientBrush(new Rectangle(new Point(0, 0), new Size(14, 16)), G1, G2, 90f);
            G.FillEllipse(LGB, new Rectangle(new Point(0, 0), new Size(14, 14)));
            G.FillEllipse(new SolidBrush(Glow), new Rectangle(new Point(0, 0), new Size(14, 7)));
        }

        if (State == MouseState.Over & X < 15)
        {
            SolidBrush SB = new SolidBrush(Color.FromArgb(70, Color.White));
            G.FillEllipse(SB, new Rectangle(new Point(0, 0), new Size(14, 14)));
        }
        else if (State == MouseState.Down & X < 15)
        {
            SolidBrush SB = new SolidBrush(Color.FromArgb(10, Color.Black));
            G.FillEllipse(SB, new Rectangle(new Point(0, 0), new Size(14, 14)));
        }

        HatchBrush HB = new HatchBrush(HatchStyle.LightDownwardDiagonal, Color.FromArgb(7, Color.Black), Color.Transparent);
        G.FillEllipse(HB, new Rectangle(new Point(0, 0), new Size(14, 14)));
        G.DrawEllipse(new Pen(Edge), new Rectangle(new Point(0, 0), new Size(14, 14)));

        if (_Checked)
            G.FillEllipse(GetBrush("Bullet"), new Rectangle(new Point(4, 4), new Size(6, 6)));
        DrawText(new SolidBrush(TextColor), HorizontalAlignment.Left, 19, -1);
    }

    private int _Field = 16;
    public int Field
    {
        get { return _Field; }
        set
        {
            if (value < 4)
                return;
            _Field = value;
            LockHeight = value;
            Invalidate();
        }
    }

    private bool _Checked;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            InvalidateControls();
            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }
            Invalidate();
        }
    }

    protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
    {
        if (!_Checked)
            Checked = true;
        base.OnMouseDown(e);
    }

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);

    protected override void OnCreation()
    {
        InvalidateControls();
    }

    private void InvalidateControls()
    {
        if (!IsHandleCreated || !_Checked)
            return;

        foreach (Control C in Parent.Controls)
        {
            if (!object.ReferenceEquals(C, this) && C is CrystalClearRadioButton)
            {
                ((CrystalClearRadioButton)C).Checked = false;
            }
        }
    }

}
class CrystalClearProgressBar : ThemeControl154
{

    Color G1;
    Color G2;
    Color Glow;
    Color Edge;

    int GlowPosition;
    private int _Minimum;
    public int Minimum
    {
        get { return _Minimum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Minimum = value;
            if (value > _Value)
                _Value = value;
            if (value > _Maximum)
                _Maximum = value;
            Invalidate();
        }
    }

    private int _Maximum = 100;
    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Maximum = value;
            if (value < _Value)
                _Value = value;
            if (value < _Minimum)
                _Minimum = value;
            Invalidate();
        }
    }

    public bool Animated
    {
        get { return IsAnimated; }
        set
        {
            IsAnimated = value;
            Invalidate();
        }
    }

    private int _Value;
    public int Value
    {
        get { return _Value; }
        set
        {
            if (value > _Maximum || value < _Minimum)
            {
                throw new Exception("Property value is not valid.");
            }

            _Value = value;
            Invalidate();
        }
    }

    private void Increment(int amount)
    {
        Value += amount;
    }

    public CrystalClearProgressBar()
    {
        SetColor("Gradient 1", 230, 230, 230);
        SetColor("Gradient 2", 210, 210, 210);
        SetColor("Glow", 230, 230, 230);
        SetColor("Edge", 170, 170, 170);
        IsAnimated = true;

    }

    protected override void ColorHook()
    {
        G1 = GetColor("Gradient 1");
        G2 = GetColor("Gradient 2");
        Glow = GetColor("Glow");
        Edge = GetColor("Edge");
    }

    protected override void OnAnimation()
    {
        if (GlowPosition == 0)
        {
            GlowPosition = 7;
        }
        else
        {
            GlowPosition -= 1;
        }
    }

    protected override void PaintHook()
    {
        G.Clear(G1);
        LinearGradientBrush LGB = new LinearGradientBrush(new Rectangle(new Point(1, 1), new Size(Width - 2, Height - 2)), G1, G2, 90f);
        G.FillRectangle(LGB, new Rectangle(new Point(1, 1), new Size((Width / Maximum) * Value - 1, Height - 2)));
        G.FillRectangle(new SolidBrush(Glow), new Rectangle(new Point(1, 1), new Size((Width / Maximum) * Value - 1, (Height / 2) - 3)));

        G.RenderingOrigin = new Point(GlowPosition, 0);
        HatchBrush HB = new HatchBrush(HatchStyle.ForwardDiagonal, Color.FromArgb(20, Color.Black), Color.Transparent);
        G.FillRectangle(HB, new Rectangle(new Point(1, 2), new Size((Width / Maximum) * Value - 1, Height - 3)));

        G.DrawLine(new Pen(Edge), new Point((Width / Maximum) * Value - 1, 1), new Point((Width / Maximum) * Value - 1, Height - 1));
        G.DrawRectangle(new Pen(Edge), new Rectangle(new Point(1, 1), new Size(Width - 2, Height - 2)));
    }

}
class CrystalClearListbox : ListBox
{

    public CrystalClearListbox()
    {
        SetStyle(ControlStyles.DoubleBuffer, true);
        Font = new Font("Microsoft Sans Serif", 9);
        BorderStyle = System.Windows.Forms.BorderStyle.None;
        DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        ItemHeight = 21;
        ForeColor = Color.Black;
        BackColor = Color.FromArgb(230, 230, 230);
        IntegralHeight = false;
    }

    protected override void WndProc(ref System.Windows.Forms.Message m)
    {
        base.WndProc(ref m);
        if (m.Msg == 15)
            CustomPaint();
    }

    private Image _Image;
    public Image ItemImage
    {
        get { return _Image; }
        set { _Image = value; }
    }

    protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
    {
        try
        {
            if (e.Index < 0)
                return;
            e.DrawBackground();
            Rectangle rect = new Rectangle(new Point(e.Bounds.Left, e.Bounds.Top + 2), new Size(Bounds.Width, 16));
            e.DrawFocusRectangle();
            if (Strings.InStr(e.State.ToString(), "Selected,") > 0)
            {
                Rectangle x2 = e.Bounds;
                Rectangle x3 = new Rectangle(x2.Location, new Size(x2.Width, (x2.Height / 2)));
                LinearGradientBrush G1 = new LinearGradientBrush(new Point(x2.X, x2.Y), new Point(x2.X, x2.Y + x2.Height), Color.FromArgb(230, 230, 230), Color.FromArgb(210, 210, 210));
                HatchBrush H = new HatchBrush(HatchStyle.LightDownwardDiagonal, Color.FromArgb(10, Color.Black), Color.Transparent);
                e.Graphics.FillRectangle(G1, x2);
                G1.Dispose();
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(230, 230, 230)), x3);
                e.Graphics.FillRectangle(H, x2);
                G1.Dispose();
                e.Graphics.DrawString(" " + Items[e.Index].ToString(), Font, Brushes.Black, 5, e.Bounds.Y + (e.Bounds.Height / 2) - 9);
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(170, 170, 170)), new Rectangle(new Point(x2.Location.X, x2.Location.Y), new Size(x2.Width, x2.Height)));
            }
            else
            {
                Rectangle x2 = e.Bounds;
                e.Graphics.DrawString(" " + Items[e.Index].ToString(), Font, Brushes.Black, 5, e.Bounds.Y + (e.Bounds.Height / 2) - 9);
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(215, 215, 215)), new Rectangle(new Point(x2.Location.X, x2.Location.Y), new Size(x2.Width, x2.Height)));
            }
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(170, 170, 170)), new Rectangle(0, 0, Width - 1, Height - 1));
            base.OnDrawItem(e);
        }
        catch (Exception ex)
        {
        }
    }

    public void CustomPaint()
    {
        CreateGraphics().DrawRectangle(new Pen(Color.FromArgb(170, 170, 170)), new Rectangle(0, 0, Width - 1, Height - 1));
    }
}
class CrystalClearTabControl : TabControl
{

    private Color _BG;
    public Color Backcolor
    {
        get { return _BG; }
        set { _BG = value; }
    }

    public CrystalClearTabControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        DoubleBuffered = true;
        Backcolor = Color.FromArgb(230, 230, 230);
    }
    protected override void CreateHandle()
    {
        base.CreateHandle();
        Alignment = TabAlignment.Top;
    }

    public Pen ToPen(Color color)
    {
        return new Pen(color);
    }

    public Brush ToBrush(Color color)
    {
        return new SolidBrush(color);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        try
        {
            SelectedTab.BackColor = Backcolor;
        }
        catch
        {
        }
        G.Clear(Backcolor);
        G.DrawRectangle(new Pen(Color.FromArgb(170, 170, 170)), new Rectangle(0, 21, Width - 1, Height - 22));
        for (int i = 0; i <= TabCount - 1; i++)
        {
            if (i == SelectedIndex)
            {
                Rectangle x2 = new Rectangle(GetTabRect(i).X - 2, GetTabRect(i).Y, GetTabRect(i).Width, GetTabRect(i).Height - 2);
                Rectangle x3 = new Rectangle(GetTabRect(i).X - 2, GetTabRect(i).Y, GetTabRect(i).Width, GetTabRect(i).Height - 1);
                Rectangle x4 = new Rectangle(GetTabRect(i).X - 2, GetTabRect(i).Y, GetTabRect(i).Width, GetTabRect(i).Height);
                LinearGradientBrush G1 = new LinearGradientBrush(x3, Color.FromArgb(10, 0, 0, 0), Color.FromArgb(230, 230, 230), 90f);
                HatchBrush HB = new HatchBrush(HatchStyle.LightDownwardDiagonal, Color.FromArgb(10, Color.Black), Color.Transparent);

                G.FillRectangle(HB, x3);
                HB.Dispose();
                G.FillRectangle(G1, x3);
                G1.Dispose();
                G.DrawLine(new Pen(Color.FromArgb(170, 170, 170)), x2.Location, new Point(x2.Location.X, x2.Location.Y + x2.Height));
                G.DrawLine(new Pen(Color.FromArgb(170, 170, 170)), new Point(x2.Location.X + x2.Width, x2.Location.Y), new Point(x2.Location.X + x2.Width, x2.Location.Y + x2.Height));
                G.DrawLine(new Pen(Color.FromArgb(170, 170, 170)), new Point(x2.Location.X, x2.Location.Y), new Point(x2.Location.X + x2.Width, x2.Location.Y));
                G.DrawString(TabPages[i].Text, Font, new SolidBrush(Color.Black), x4, new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                });
            }
            else
            {
                Rectangle x2 = new Rectangle(GetTabRect(i).X - 2, GetTabRect(i).Y + 3, GetTabRect(i).Width, GetTabRect(i).Height - 5);
                LinearGradientBrush G1 = new LinearGradientBrush(x2, Color.FromArgb(215, 215, 215), Color.FromArgb(230, 230, 230), -90f);
                G.FillRectangle(G1, x2);
                G1.Dispose();
                G.DrawRectangle(new Pen(Color.FromArgb(170, 170, 170)), x2);
                G.DrawString(TabPages[i].Text, Font, new SolidBrush(Color.Black), x2, new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                });
            }
        }

        e.Graphics.DrawImage(B, 0, 0);
        G.Dispose();
        B.Dispose();
    }
}
[DefaultEvent("TextChanged")]
class CrystalClearTextBox : ThemeControl154
{

    private HorizontalAlignment _TextAlign = HorizontalAlignment.Left;
    public HorizontalAlignment TextAlign
    {
        get { return _TextAlign; }
        set
        {
            _TextAlign = value;
            if (Base != null)
            {
                Base.TextAlign = value;
            }
        }
    }
    private int _MaxLength = 32767;
    public int MaxLength
    {
        get { return _MaxLength; }
        set
        {
            _MaxLength = value;
            if (Base != null)
            {
                Base.MaxLength = value;
            }
        }
    }
    private bool _ReadOnly;
    public bool ReadOnly
    {
        get { return _ReadOnly; }
        set
        {
            _ReadOnly = value;
            if (Base != null)
            {
                Base.ReadOnly = value;
            }
        }
    }
    private bool _UseSystemPasswordChar;
    public bool UseSystemPasswordChar
    {
        get { return _UseSystemPasswordChar; }
        set
        {
            _UseSystemPasswordChar = value;
            if (Base != null)
            {
                Base.UseSystemPasswordChar = value;
            }
        }
    }
    private bool _Multiline;
    public bool Multiline
    {
        get { return _Multiline; }
        set
        {
            _Multiline = value;
            if (Base != null)
            {
                Base.Multiline = value;

                if (value)
                {
                    LockHeight = 0;
                    Base.Height = Height - 11;
                }
                else
                {
                    LockHeight = Base.Height + 11;
                }
            }
        }
    }
    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            if (Base != null)
            {
                Base.Text = value;
            }
        }
    }
    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            if (Base != null)
            {
                Base.Font = value;
                Base.Location = new Point(3, 5);
                Base.Width = Width - 6;

                if (!_Multiline)
                {
                    LockHeight = Base.Height + 11;
                }
            }
        }
    }

    protected override void OnCreation()
    {
        if (!Controls.Contains(Base))
        {
            Controls.Add(Base);
        }
    }

    private TextBox Base;
    public CrystalClearTextBox()
    {
        Base = new TextBox();

        Base.Font = Font;
        Base.Text = Text;
        Base.MaxLength = _MaxLength;
        Base.Multiline = _Multiline;
        Base.ReadOnly = _ReadOnly;
        Base.UseSystemPasswordChar = _UseSystemPasswordChar;

        Base.BorderStyle = BorderStyle.None;

        Base.Location = new Point(4, 4);
        Base.Width = Width - 10;

        if (_Multiline)
        {
            Base.Height = Height - 11;
        }
        else
        {
            LockHeight = Base.Height + 11;
        }

        Base.TextChanged += OnBaseTextChanged;
        Base.KeyDown += OnBaseKeyDown;


        SetColor("Text", Color.Black);
        SetColor("Backcolor", BackColor);
        SetColor("Border", 170, 170, 170);
    }

    private Color BG;

    private Pen P1;
    protected override void ColorHook()
    {
        BG = GetColor("Backcolor");

        P1 = GetPen("Border");

        Base.ForeColor = GetColor("Text");
        Base.BackColor = GetColor("Backcolor");
    }

    protected override void PaintHook()
    {
        G.Clear(BG);
        DrawBorders(P1);
    }
    private void OnBaseTextChanged(object s, EventArgs e)
    {
        Text = Base.Text;
    }
    private void OnBaseKeyDown(object s, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.A)
        {
            Base.SelectAll();
            e.SuppressKeyPress = true;
        }
    }
    protected override void OnResize(EventArgs e)
    {
        Base.Location = new Point(4, 5);
        Base.Width = Width - 8;

        if (_Multiline)
        {
            Base.Height = Height - 5;
        }


        base.OnResize(e);
    }

}