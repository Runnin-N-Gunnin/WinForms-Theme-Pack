
    #region Themebase
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Drawing.Imaging;
    using System.Text;
     
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
     
        private const int Rate = 10;
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
    #region Positron Theme
     
    //-------------------------------------
    //------PLEASE LEAVE CREDITS HERE------
    //-------------------------------------
    //Creator: PatPositron HFUID=1448959
    //Site: patpositron.com
    //Created: 4/28/13
    //Changed: 6/12/13
    //Version: 1.8
    //-------------------------------------
    //------PLEASE LEAVE CREDITS HERE------
    //-------------------------------------
     
    class PositronTheme : ThemeContainer154
    {
        Color BG, GT, GB;
        Brush TB, Black, H;
        Pen b, IB, PB;
     
        public PositronTheme()
        {
            TransparencyKey = Color.Fuchsia;
            BackColor = Color.FromArgb(225,225,225);
            Font = new Font("Verdana", 8);
            SetColor("BG", Color.FromArgb(208,208,208));
            SetColor("TB", Color.FromArgb(100,100,100));
            SetColor("Black", Color.Black);
            SetColor("Hover", Color.FromArgb(210,210,210));
            SetColor("Top", Color.FromArgb(220, 220, 220));
            SetColor("Bot", Color.FromArgb(200,200,200));
            SetColor("Border", Color.FromArgb(150, 150, 150));
            DoubleBuffered = true;
        }
        protected override void ColorHook()
        {
            BG = GetColor("BG");
            TB = GetBrush("TB");
            Black = GetBrush("Black");
            b = GetPen("Bot");
            GT = GetColor("Top");
            GB = GetColor("Bot");
            H = GetBrush("Hover");
            PB = GetPen("Border");
            IB = GetPen("Bot");
        }
        protected override void PaintHook()
        {
            HatchBrush HBM = new HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(30, Color.White), Color.Transparent);
            G.Clear(BG);
            G.FillRectangle(HBM, new Rectangle(0, 0, Width - 1, Height - 1));
            G.FillRectangle(new SolidBrush(BackColor), new Rectangle(8, 27, Width - 16, Height - 35));
            G.DrawString(Text, Font, TB, new Point(29, 7));
            G.DrawIcon(ParentForm.Icon, new Rectangle(7, 4, 19, 20));
            DrawBorders(PB);
            DrawBorders(IB, 1);
     
        }
    }
     
    class PositronButton : ThemeControl154
    {
        Color TG, BG;
        Brush TC, H;
        Pen B, IB;
        public PositronButton()
        {
            SetColor("TopG", Color.FromArgb(220,220,220));
            SetColor("BottomG", Color.FromArgb(200,200,200));
            SetColor("Text", Color.FromArgb(100,100,100));
            SetColor("Border", Color.FromArgb(150,150,150));
            SetColor("Inside", Color.FromArgb(200,200,200));
            SetColor("Hover", Color.FromArgb(210,210,210));
            Size = new Size(100, 30);
        }
        protected override void ColorHook()
        {
            TG = GetColor("TopG");
            BG = GetColor("BottomG");
            TC = GetBrush("Text");
            B = GetPen("Border");
            IB = GetPen("Inside");
            H = GetBrush("Hover");
        }
        protected override void PaintHook()
        {
            G.Clear(TG);
            switch (State)
            {
                case MouseState.None:
                    LinearGradientBrush LGB1 = new LinearGradientBrush(new Rectangle(0,0,Width-1,Height-1), TG, BG, 90F);
                    G.FillRectangle(LGB1, new Rectangle(2, 2, Width-4, Height-4));
                    break;
                case MouseState.Over:
                    G.FillRectangle(H, new Rectangle(2, 2, Width - 4, Height - 4));
                    break;
                case MouseState.Down:
                    LinearGradientBrush LGB3 = new LinearGradientBrush(new Rectangle(0, 0, Width - 1, Height - 1), BG, TG, 90F);
                    G.FillRectangle(LGB3, new Rectangle(2, 2, Width - 4, Height - 4));
                    break;
            }
            DrawBorders(IB);
            DrawText(TC, HorizontalAlignment.Center, 0, 0);
            G.DrawRectangle(B, new Rectangle(1, 1, Width - 3, Height - 3));
        }
    }
     
    class PositronGroupBox : ThemeContainer154
    {
        Pen PB, IB;
        Brush BT;
        Color BG;
        public PositronGroupBox()
        {
            ControlMode = true;
            SetColor("Border", Color.FromArgb(150,150,150));
            SetColor("Text", Color.FromArgb(100, 100, 100));
            SetColor("BG", Color.FromArgb(208, 208, 208));
            SetColor("Inside", Color.FromArgb(200, 200, 200));
            Size = new Size(160, 80);
        }
        protected override void ColorHook()
        {
            PB = GetPen("Border");
            BT = GetBrush("Text");
            BG = GetColor("BG");
            IB = GetPen("Inside");
        }
        protected override void PaintHook()
        {
            HatchBrush HBM = new HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(30, Color.White), Color.Transparent);
            G.Clear(BG);
            G.FillRectangle(HBM, new Rectangle(0, 0, Width - 1, Height - 1));
            G.FillRectangle(new SolidBrush(Color.FromArgb(220,220,220)), new Rectangle(5, 20, Width - 10, Height - 25));
            DrawBorders(IB);
            DrawBorders(PB, 1);
            G.DrawString(Text, Font, BT, new Point(6, 3));
        }
    }
     
    [DefaultEvent("CheckedChanged")]
    class PositronRadioButton : ThemeControl154
    {
        Brush TB, Inside;
        Pen B, IB;
     
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
     
        public event CheckedChangedEventHandler CheckedChanged;
        public delegate void CheckedChangedEventHandler(object sender);
     
        private void InvalidateControls()
        {
            if (!IsHandleCreated || !_Checked)
                return;
     
            foreach (Control C in Parent.Controls)
            {
                if (!object.ReferenceEquals(C, this) && C is PositronRadioButton)
                {
                    ((PositronRadioButton)C).Checked = false;
                }
            }
        }
     
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (!_Checked)
                Checked = true;
            base.OnMouseDown(e);
        }
     
        public PositronRadioButton()
        {
            LockHeight = 22;
            Width = 140;
            Size = new Size(150, 22);
            SetColor("Text", Color.FromArgb(100,100,100));
            SetColor("Border", Color.FromArgb(175,175,175));
            SetColor("IB", Color.FromArgb(200, 200, 200));
            SetColor("B", Color.FromArgb(150, 150, 150));
        }
     
        protected override void ColorHook()
        {
            TB = GetBrush("Text");
            B = GetPen("B");
            IB = GetPen("IB");
            Inside = GetBrush("Border");
        }
     
        protected override void PaintHook()
        {
            G.Clear(BackColor);
            G.SmoothingMode = SmoothingMode.AntiAlias;
            if (_Checked)
                G.FillEllipse(TB, new Rectangle(new Point(6,6), new Size(6,6)));
            if (State == MouseState.Over)
            {
                if (_Checked) { }
                else
                    G.FillEllipse(Inside, new Rectangle(new Point(5, 5), new Size(8, 8)));
            }
     
            G.DrawEllipse(new Pen(Color.FromArgb(125,125,125)), new Rectangle(new Point(1, 1), new Size(16, 16)));
            G.DrawEllipse(new Pen(Color.FromArgb(200,200,200)), new Rectangle(new Point(0, 0), new Size(18, 18)));
     
            G.DrawString(Text, Font, TB, 19, 2);
        }
    }
     
    [DefaultEvent("CheckedChanged")]
    class PositronCheckBox : ThemeControl154
    {
        Color BG;
        Brush TB, IN;
        Pen IB, B;
     
        private bool _Checked;
        public event CheckedChangedEventHandler CheckedChanged;
        public delegate void CheckedChangedEventHandler(object sender);
     
        public bool Checked
        {
            get { return _Checked; }
            set
            {
                _Checked = value;
                Invalidate();
                if (CheckedChanged != null)
                {
                    CheckedChanged(this);
                }
            }
        }
     
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_Checked == true)
                _Checked = false;
            else
                _Checked = true;
        }
     
        public PositronCheckBox()
        {
            LockHeight = 22;
            SetColor("BG", Color.FromArgb(240, 240, 240));
            SetColor("Texts", Color.FromArgb(100, 100, 100));
            SetColor("Inside", Color.FromArgb(175,175,175));
            SetColor("IB", Color.FromArgb(200,200,200));
            SetColor("B", Color.FromArgb(150,150,150));
            Size = new Size(150, 22);
        }
     
        protected override void ColorHook()
        {
            BG = GetColor("BG");
            TB = GetBrush("Texts");
            IN = GetBrush("Inside");
            IB = GetPen("IB");
            B = GetPen("B");
        }
     
        protected override void PaintHook()
        {
            G.Clear(BackColor);
            G.SmoothingMode = SmoothingMode.AntiAlias;
     
            if (_Checked)
                G.DrawString("a", new Font("Marlett", 12), TB, new Point(-1, 1));
     
            if (State == MouseState.Over)
            {
                G.FillRectangle(IN, new Rectangle(new Point(4,4), new Size(10,10)));
                if (_Checked)
                    G.DrawString("a", new Font("Marlett", 12), TB, new Point(-1, 1));
            }
     
            G.DrawRectangle(B, 2, 2, 14,14);
            G.DrawRectangle(IB, 1,1,16,16);
            G.DrawString(Text, Font, TB, 19, 3);
        }
    }
     
    [DefaultEvent("CheckedChanged")]
    class PositronOnOff : ThemeControl154
    {
        Brush TB;
        Pen b;
     
        private bool _Checked = false;
        public event CheckedChangedEventHandler CheckedChanged;
        public delegate void CheckedChangedEventHandler(object sender);
     
        public bool Checked
        {
            get { return _Checked; }
            set
            {
                _Checked = value;
                Invalidate();
                if (CheckedChanged != null)
                {
                    CheckedChanged(this);
                }
            }
        }
        protected void DrawBorders(Pen p1, Graphics G)
        {
            DrawBorders(p1, 0, 0, Width, Height, G);
        }
        protected void DrawBorders(Pen p1, int offset, Graphics G)
        {
            DrawBorders(p1, 0, 0, Width, Height, offset, G);
        }
        protected void DrawBorders(Pen p1, int x, int y, int width, int height, Graphics G)
        {
            G.DrawRectangle(p1, x, y, width - 1, height - 1);
        }
        protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset, Graphics G)
        {
            DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2), G);
        }
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_Checked == true)
                _Checked = false;
            else
                _Checked = true;
        }
     
        public PositronOnOff()
        {
            LockHeight = 24;
            LockWidth = 62;
            SetColor("Texts", Color.FromArgb(100, 100, 100));
            SetColor("border", Color.FromArgb(125, 125, 125));
        }
     
        protected override void ColorHook()
        {
            TB = GetBrush("Texts");
            b = GetPen("border");
        }
     
        protected override void PaintHook()
        {
            G.Clear(BackColor);
            LinearGradientBrush LGB1 = new LinearGradientBrush(new Rectangle(0,0,Width,Height), Color.FromArgb(120, 120, 120), Color.FromArgb(100, 100, 100), 90);
            HatchBrush HB1 = new HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(10, Color.White), Color.Transparent);
     
            if (_Checked)
            {
                G.FillRectangle(LGB1, new Rectangle(2, 2, (Width / 2) - 2, Height - 4));
                G.FillRectangle(HB1, new Rectangle(2, 2, (Width / 2) - 2, Height - 4));
                G.DrawString("On", Font, TB, new Point(36, 6));
            }
            else if (!_Checked)
            {
                G.FillRectangle(LGB1, new Rectangle((Width / 2) - 1, 2, (Width / 2) - 1, Height - 4));
                G.FillRectangle(HB1, new Rectangle((Width / 2) - 1, 2, (Width / 2) - 1, Height - 4));
                G.DrawString("Off", Font, TB, new Point(5, 6));
            }
            DrawBorders(new Pen(new SolidBrush(Color.FromArgb(200, 200, 200))), G);
            DrawBorders(new Pen(new SolidBrush(Color.FromArgb(150, 150, 150))), 1, G);
     
        }
    }
     
    class PositronLabel : Label
    {
        public PositronLabel()
        {
            ForeColor = Color.FromArgb(100,100,100);
            BackColor = Color.Transparent;
            Font = new Font("Verdana", 8);
        }
    }
     
    class PositronControlBox : ThemeControl154
    {
        Color TG, BG;
        Brush TC, H;
        Pen B, IB;
     
        public PositronControlBox()
        {
            SetColor("TopG", Color.FromArgb(220, 220, 220));
            SetColor("BottomG", Color.FromArgb(200, 200, 200));
            SetColor("Text", Color.FromArgb(100, 100, 100));
            SetColor("Border", Color.FromArgb(150, 150, 150));
            SetColor("Inside", Color.FromArgb(200, 200, 200));
            SetColor("Hover", Color.FromArgb(210, 210, 210));
            LockHeight = 22;
            LockWidth = 22;
        }
        protected override void ColorHook()
        {
            TG = GetColor("TopG");
            BG = GetColor("BottomG");
            TC = GetBrush("Text");
            B = GetPen("Border");
            IB = GetPen("Inside");
            H = GetBrush("Hover");
        }
     
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Application.Exit();
                Environment.Exit(0);
            }
        }
     
        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
        }
     
        protected override void PaintHook()
        {
            switch (State)
            {
                case MouseState.None:
                    LinearGradientBrush LGB1 = new LinearGradientBrush(new Rectangle(0, 0, Width - 1, Height - 1), TG, BG, 90F);
                    G.FillRectangle(LGB1, new Rectangle(0,0,22,22));
                    break;
                case MouseState.Over:
                    G.FillRectangle(H, new Rectangle(0,0,22,22));
                    break;
                case MouseState.Down:
                    LinearGradientBrush LGB3 = new LinearGradientBrush(new Rectangle(0, 0, Width - 1, Height - 1), BG, TG, 90F);
                    G.FillRectangle(LGB3, new Rectangle(0,0,22,22));
                    break;
            }
            DrawBorders(IB);
            G.DrawString("r", new Font("Marlett", 8), TC, 4,6);
        }
    }
     
    [DefaultEvent("TextChanged")]
    class PositronTextBox : ThemeControl154
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
                    Base.Location = new Point(10,6);
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
        public PositronTextBox()
        {
            Base = new TextBox();
     
            Base.Font = new Font("Verdana", 8);
            Base.Text = Text;
            Base.MaxLength = _MaxLength;
            Base.Multiline = _Multiline;
            Base.ReadOnly = _ReadOnly;
            Base.UseSystemPasswordChar = _UseSystemPasswordChar;
            Base.Size = new Size(100, 25);
            Size = new Size(112, 25);
            Base.BorderStyle = BorderStyle.None;
     
            Base.Location = new Point(10,6);
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
     
     
            SetColor("B", Color.FromArgb(210, 210, 210));
            SetColor("Inside", Color.FromArgb(200, 200, 200));
            SetColor("Border", Color.FromArgb(150, 150, 150));
        }
     
        Color b, i, bb;
     
        protected override void ColorHook()
        {
            Base.ForeColor = Color.FromArgb(100, 100, 100);
            Base.BackColor = Color.FromArgb(210, 210, 210);
            b = GetColor("B");
            i = GetColor("Inside");
            bb = GetColor("Border");
        }
     
        protected override void PaintHook()
        {
            G.Clear(b);
            Base.Size = new Size(Width - 10, Height - 10);
            G.FillRectangle(new SolidBrush(b), new Rectangle(1, 1, Width - 2, Height - 2));
            DrawBorders(new Pen(new SolidBrush(bb)), 1);
            DrawBorders(new Pen(new SolidBrush(i)));
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
            Base.Location = new Point(5, 5);
            Base.Width = Width - 10;
     
            if (_Multiline)
            {
                Base.Height = Height - 11;
            }
            base.OnResize(e);
        }
     
    }
     
    class PositronProgressBar : ThemeControl154
    {
        private int _Value;
        public int Value
        {
            get { return _Value; }
            set
            {
                if (value >= Minimum & value <= _Max)
                    _Value = value;
                Invalidate();
            }
        }
     
        private Orientation _Orientation;
        public Orientation Orientation
        {
            get { return _Orientation; }
            set
            {
                _Orientation = value;
                Invalidate();
            }
        }
     
     
        private int _Max = 100;
        public int Maximum
        {
            get { return _Max; }
            set
            {
                if (value > _Min)
                    _Max = value;
                Invalidate();
            }
        }
     
        private int _Min = 0;
        public int Minimum
        {
            get { return _Min; }
            set
            {
                if (value < _Max)
                    _Min = value;
                Invalidate();
            }
        }
     
        private void Increment(int amount)
        {
            Value += amount;
        }
     
        private bool _ShowValue = false;
        [Description("Indicates if the value of the progress bar will be shown in the middle of it.")]
        public bool ShowValue
        {
            get { return _ShowValue; }
            set
            {
                _ShowValue = value;
                Invalidate();
            }
        }
       
        Brush BT;
        Pen IB, PB;
        Color BG, IC;
     
        public PositronProgressBar()
        {
            Transparent = true;
            Value = 50;
            ShowValue = true;
            SetColor("Text", Color.FromArgb(100,100,100));
            SetColor("Inside", Color.FromArgb(200, 200, 200));
            SetColor("Border", Color.FromArgb(150, 150, 150));
            SetColor("BG", Color.FromArgb(210,210,210));
            SetColor("IC", Color.FromArgb(215, 215, 215));
            MinimumSize = new Size(40,14);
            Size = new Size(175,30);
        }
     
        protected override void ColorHook()
        {
            BT = GetBrush("Text");
            IB = GetPen("Inside");
            PB = GetPen("Border");
            BG = GetColor("BG");
            IC = GetColor("IC");
        }
     
        protected override void PaintHook()
        {
            switch(_Orientation)
            {
                case System.Windows.Forms.Orientation.Horizontal:
     
                    int area = Convert.ToInt32((_Value * (Width - 6)) / _Max);
                    G.Clear(BG);
                    LinearGradientBrush LGB1 = new LinearGradientBrush(new Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(220,220,220), Color.FromArgb(200,200,200), 90F);
     
                    if (_Value == _Max)
                    {
                        G.FillRectangle(LGB1, new Rectangle(3,3, Width - 4, Height - 4));
                        DrawBorders(PB, 3);
                    }
                    else if (_Value == _Min)
                    { }
                    else
                    {
                        G.FillRectangle(LGB1, new Rectangle(3, 3, area, Height - 4));
                        G.DrawRectangle(PB, new Rectangle(3, 3, area - 1, Height - 7));
                    }
                    if (_ShowValue)
                    {
                        string val = _Value.ToString();
                        DrawText(BT, val, HorizontalAlignment.Center, 0, 0);
                    }
     
                    break;
     
                case System.Windows.Forms.Orientation.Vertical:
     
                    int area2 = Convert.ToInt32((_Value * (Height - 6)) / _Max);
     
                    G.Clear(BG);
                    LinearGradientBrush LGB2 = new LinearGradientBrush(new Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(220,220,220), Color.FromArgb(200,200,200), 90F);
     
                    if (_Value == _Max)
                    {
                        G.FillRectangle(LGB2, new Rectangle(3,3, Width - 4, Height - 4));
                        DrawBorders(PB, 3);
                    }
                    else if (_Value == _Min)
                    { }
                    else
                    {
                        G.FillRectangle(LGB2, new Rectangle(3, 3, Width - 4, area2));
                        G.DrawRectangle(PB, new Rectangle(3, 3, Width - 7, area2));
                    }
                    if (_ShowValue)
                    {
                        string val = _Value.ToString();
                        DrawText(BT, val, HorizontalAlignment.Center, 0, 0);
                    }
     
     
                    break;
            }
     
            DrawBorders(IB);
            DrawBorders(PB, 1);
        }
    }
     
    class PositronListBox : ListBox
    {
        private bool mShowScroll;
        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!mShowScroll)
                    cp.Style = cp.Style & ~0x200000;
                return cp;
            }
        }
        [Description("Indicates whether the vertical scrollbar appears or not.")]
        public bool ShowScrollbar
        {
            get { return mShowScroll; }
            set
            {
                if (value == mShowScroll)
                    return;
                mShowScroll = value;
                if (Handle != IntPtr.Zero)
                    RecreateHandle();
            }
        }
     
        public PositronListBox()
            {
                    SetStyle(ControlStyles.DoubleBuffer, true);
                    BorderStyle = System.Windows.Forms.BorderStyle.None;
                    DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
                    ItemHeight = 16;
                    ForeColor = Color.Black;
                    BackColor = Color.FromArgb(210,210,210);
                    IntegralHeight = false;
                    Font = new Font("Verdana", 8);
            ScrollAlwaysVisible = false;
            }
        protected void DrawBorders(Pen p1)
        {
            DrawBorders(p1, 0, 0, Width, Height);
        }
        protected void DrawBorders(Pen p1, int offset)
        {
            DrawBorders(p1, 0, 0, Width, Height, offset);
        }
        protected void DrawBorders(Pen p1, int x, int y, int width, int height)
        {
            CreateGraphics().DrawRectangle(p1, x, y, width - 1, height - 1);
        }
        protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset)
        {
            DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
        }
            protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
            {
            if ((e.Index >= 0))
            {
                Rectangle ItemBounds = e.Bounds;
                e.Graphics.FillRectangle(new SolidBrush(BackColor), ItemBounds);
     
                if (((e.State.ToString().IndexOf("Selected,") + 1) > 0))
                {
                    LinearGradientBrush LGB1 = new LinearGradientBrush(ItemBounds, Color.FromArgb(120,120,120), Color.FromArgb(100,100,100), 90);
                    HatchBrush HB1 = new HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(10, Color.White), Color.Transparent);
                    e.Graphics.FillRectangle(LGB1, ItemBounds);
                    e.Graphics.FillRectangle(HB1, ItemBounds);
                    e.Graphics.DrawString(Items[e.Index].ToString(), Font, new SolidBrush(Color.FromArgb(200, 200, 200)), 5, Convert.ToInt32((e.Bounds.Y + ((e.Bounds.Height / 2) - 7))));
                }
                else
                {
                    try
                    {
                        e.Graphics.DrawString(Items[e.Index].ToString(), Font, new SolidBrush(Color.FromArgb(100,100,100)), 5, Convert.ToInt32((e.Bounds.Y + ((e.Bounds.Height / 2) - 7))));
                    }
                    catch { }
                }
            }
            DrawBorders(new Pen(new SolidBrush(Color.FromArgb(200, 200, 200))));
            DrawBorders(new Pen(new SolidBrush(Color.FromArgb(150, 150, 150))), 1);
            base.OnDrawItem(e);
        }
            public void CustomPaint()
            {
                    CreateGraphics().DrawRectangle(new Pen(Color.FromArgb(210,210,210)), new Rectangle(0, 0, Width - 1, Height - 1));
            }
    }
     
    class PositronDivider : ThemeControl154
    {
       
        private Orientation _Orientation;
       
        public Orientation Orientation
        {
            get { return _Orientation; }
            set {
                _Orientation = value;
                if (value == System.Windows.Forms.Orientation.Vertical)
                {
                    LockHeight = 0;
                    LockWidth = 14;
                }
                else
                {
                    LockHeight = 14;
                    LockWidth = 0;
                }
                Invalidate();
            }
        }
       
         public PositronDivider()
         {
            Transparent = true;
            BackColor = Color.Transparent;
            LockHeight = 14;
         }
       
        protected override void ColorHook()
        {
        }
       
        protected override void PaintHook()
        {
            G.Clear(BackColor);
            ColorBlend BL1 = new ColorBlend();
            ColorBlend BL2 = new ColorBlend();
            BL1.Positions = new float[] {
                    0.0F,
                    0.15F,
                    0.85F,
                    1.0F};
            BL2.Positions = new float[] {
                    0.0F,
                    0.15F,
                    0.5F,
                    0.85F,
                    1.0F};
            BL1.Colors = new Color[] {
                    Color.Transparent,
                    Color.LightGray,
                    Color.LightGray,
                    Color.Transparent};
            BL2.Colors = new Color[] {
                    Color.Transparent,
                    Color.FromArgb(144, 144, 144),
                    Color.FromArgb(160, 160, 160),
                    Color.FromArgb(156, 156, 156),
                    Color.Transparent};
            if (_Orientation == System.Windows.Forms.Orientation.Vertical)
            {
                DrawGradient(BL1, 6, 0, 1, Height);
                DrawGradient(BL2, 7, 0, 1, Height);
            }
            else
            {
                DrawGradient(BL1, 0, 6, Width, 1, 0.0F);
                DrawGradient(BL2, 0, 7, Width, 1, 0.0F);
            }
        }
    }
     
    class PositronComboBox : ComboBox
    {
        private int X;
            private int _StartIndex = 0;
            public int StartIndex {
                    get { return _StartIndex; }
                    set {
                            _StartIndex = value;
                            try {
                                    base.SelectedIndex = value;
                            } catch {
                            }
                            Invalidate();
                    }
            }
        protected void DrawBorders(Pen p1, Graphics G)
        {
            DrawBorders(p1, 0, 0, Width, Height, G);
        }
        protected void DrawBorders(Pen p1, int offset, Graphics G)
        {
            DrawBorders(p1, 0, 0, Width, Height, offset, G);
        }
        protected void DrawBorders(Pen p1, int x, int y, int width, int height, Graphics G)
        {
            G.DrawRectangle(p1, x, y, width - 1, height - 1);
        }
        protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset, Graphics G)
        {
            DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2), G);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            X = e.X;
            base.OnMouseMove(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            X = 0;
            base.OnMouseLeave(e);
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                X = 0;
            }
            base.OnMouseClick(e);
        }
     
        private SolidBrush B1, B2, B3;
     
            public PositronComboBox()
            {
            SetStyle((ControlStyles)139286, true);
            SetStyle(ControlStyles.Selectable, false);
            DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
     
            BackColor = Color.FromArgb(225, 225, 225);
                    DropDownStyle = ComboBoxStyle.DropDownList;
     
            Font = new Font("Verdana", 8);
     
            B1 = new SolidBrush(Color.FromArgb(230,230,230));
            B2 = new SolidBrush(Color.FromArgb(210,210,210));
            B3 = new SolidBrush(Color.FromArgb(100, 100, 100));
            }
     
            protected override void OnPaint(PaintEventArgs e)
            {
                    Graphics G = e.Graphics;
            Point[] points = new Point[] {new Point(Width - 15, 9), new Point(Width - 6, 9), new Point(Width - 11, 14)};
            G.Clear(BackColor);
            G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
     
            LinearGradientBrush LGB1 = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(220,220,220), Color.FromArgb(200,200,200), 90F);
     
            G.FillRectangle(LGB1, new Rectangle(0, 0, Width, Height));
     
            G.DrawLine(new Pen(new SolidBrush(Color.FromArgb(150,150,150))), new Point(Width - 21, 2), new Point(Width - 21, Height));
     
            DrawBorders(new Pen(new SolidBrush(Color.FromArgb(200, 200, 200))), G);
            DrawBorders(new Pen(new SolidBrush(Color.FromArgb(150, 150, 150))),1, G);
     
            try { G.DrawString((string)Items[SelectedIndex].ToString(), Font, new SolidBrush(Color.FromArgb(100, 100, 100)), new Point(3, 4)); }
            catch { G.DrawString(" . . . ", Font, new SolidBrush(Color.FromArgb(100,100,100)), new Point(3,4)); }
     
            if (X >= 1)
            {
                LinearGradientBrush LGB3 = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(200,200,200), Color.FromArgb(220,220,220), 90F);
                G.FillRectangle(LGB3, new Rectangle(Width - 20, 2, 18, 17));
                G.FillPolygon(B3, points);
            }
            else
            {
                G.FillPolygon(B3, points);
            }
            }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            LinearGradientBrush LGB1 = new LinearGradientBrush(e.Bounds, Color.FromArgb(120, 120, 120), Color.FromArgb(100, 100, 100), 90);
            HatchBrush HB1 = new HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(10, Color.White), Color.Transparent);
     
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(LGB1, new Rectangle(1, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
                e.Graphics.FillRectangle(HB1, e.Bounds);
                e.Graphics.DrawString(GetItemText(Items[e.Index]), e.Font, new SolidBrush(Color.FromArgb(200,200,200)), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(B2, e.Bounds);
                try { e.Graphics.DrawString(GetItemText(Items[e.Index]), e.Font, new SolidBrush(Color.FromArgb(100, 100, 100)), e.Bounds); }
                catch { }
            }
     
        }
    }
     
    class PositronTabControl : TabControl
    {
        private Brush TB;
        private int i = 0;
     
        protected void DrawBorders(Pen p1, Graphics G)
        {
            DrawBorders(p1, 0, 0, Width, Height, G);
        }
        protected void DrawBorders(Pen p1, int offset, Graphics G)
        {
            DrawBorders(p1, 0, 0, Width, Height, offset, G);
        }
        protected void DrawBorders(Pen p1, int x, int y, int width, int height, Graphics G)
        {
            G.DrawRectangle(p1, x, y, width - 1, height - 1);
        }
        protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset, Graphics G)
        {
            DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2), G);
        }
     
        public PositronTabControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            SizeMode = TabSizeMode.Fixed;
            DoubleBuffered = true;
            ItemSize = new Size(30,120);
            Size = new Size(250, 150);
            TB = new SolidBrush(Color.FromArgb(100, 100, 100));
        }
     
        protected override void CreateHandle()
        {
            base.CreateHandle();
            Alignment = TabAlignment.Left;
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);
            HatchBrush HBS = new HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(6, Color.Black), Color.Transparent);
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.Clear(FindForm().BackColor);
            G.FillRectangle(HBS, new Rectangle(0, 0, Width, Height));
     
            try
            {
                SelectedTab.BackColor = Color.FromArgb(210, 210, 210);
            }
            catch { }
            for (i = 0; i <= TabCount - 1; i++)
            {
                Rectangle TabRect = GetTabRect(i);
                try
                {
                    LinearGradientBrush LGB1 = new LinearGradientBrush(TabRect, Color.FromArgb(190, 190, 190), Color.FromArgb(220, 220, 220), 90F);
                    LinearGradientBrush LGB2 = new LinearGradientBrush(TabRect, Color.FromArgb(220, 220, 220), Color.FromArgb(190, 190, 190), 90F);
     
                    if (i == SelectedIndex)
                    {
                        G.FillRectangle(LGB1, TabRect);
                        G.DrawString(TabPages[i].Text, new Font(Font.FontFamily, Font.Size, FontStyle.Bold), TB, TabRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.FillRectangle(LGB2, TabRect);
                        G.DrawString(TabPages[i].Text, Font, TB, TabRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    G.DrawLine(new Pen(new SolidBrush(Color.FromArgb(150, 150, 150))), new Point(TabRect.X, TabRect.Y), new Point(ItemSize.Height + 1, TabRect.Y));
                    G.DrawLine(new Pen(new SolidBrush(Color.FromArgb(150, 150, 150))), new Point(), new Point());
                }
                catch { }
            }
            DrawBorders(new Pen(new SolidBrush(Color.FromArgb(200, 200, 200))), 1, G);
            DrawBorders(new Pen(new SolidBrush(Color.FromArgb(150, 150, 150))), 2, G);
            G.DrawLine(new Pen(Color.FromArgb(150, 150, 150)), new Point(ItemSize.Height + 2, Height - (Height - 3)), new Point(ItemSize.Height + 2, Height - 3));
     
            e.Graphics.DrawImage(B, 0, 0);
            G.Dispose();
            B.Dispose();
            base.OnPaint(e);
        }
    }
     
    #endregion