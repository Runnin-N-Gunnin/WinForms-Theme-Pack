	

    #region Imports
     
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
     
    #endregion
    #region RoundRect
     
    // [CREDIT][DO NOT REMOVE]
    //
    // This module was written by Aeonhack
    //
    // [CREDIT][DO NOT REMOVE]
     
    static class RoundRectangle
    {
        public static GraphicsPath RoundRect(Rectangle Rectangle, int Curve)
        {
            GraphicsPath GP = new GraphicsPath();
            int EndArcWidth = Curve * 2;
            GP.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, EndArcWidth, EndArcWidth), -180, 90);
            GP.AddArc(new Rectangle(Rectangle.Width - EndArcWidth + Rectangle.X, Rectangle.Y, EndArcWidth, EndArcWidth), -90, 90);
            GP.AddArc(new Rectangle(Rectangle.Width - EndArcWidth + Rectangle.X, Rectangle.Height - EndArcWidth + Rectangle.Y, EndArcWidth, EndArcWidth), 0, 90);
            GP.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - EndArcWidth + Rectangle.Y, EndArcWidth, EndArcWidth), 90, 90);
            GP.AddLine(new Point(Rectangle.X, Rectangle.Height - EndArcWidth + Rectangle.Y), new Point(Rectangle.X, Curve + Rectangle.Y));
            return GP;
        }
     
        public static GraphicsPath RoundRect(int X, int Y, int Width, int Height, int Curve)
        {
            Rectangle Rectangle = new Rectangle(X, Y, Width, Height);
            GraphicsPath GP = new GraphicsPath();
            int EndArcWidth = Curve * 2;
            GP.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, EndArcWidth, EndArcWidth), -180, 90);
            GP.AddArc(new Rectangle(Rectangle.Width - EndArcWidth + Rectangle.X, Rectangle.Y, EndArcWidth, EndArcWidth), -90, 90);
            GP.AddArc(new Rectangle(Rectangle.Width - EndArcWidth + Rectangle.X, Rectangle.Height - EndArcWidth + Rectangle.Y, EndArcWidth, EndArcWidth), 0, 90);
            GP.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - EndArcWidth + Rectangle.Y, EndArcWidth, EndArcWidth), 90, 90);
            GP.AddLine(new Point(Rectangle.X, Rectangle.Height - EndArcWidth + Rectangle.Y), new Point(Rectangle.X, Curve + Rectangle.Y));
            return GP;
        }
    }
     
    #endregion
     
    //|------DO-NOT-REMOVE------|
    //
    // Creator: HazelDev
    // Site   : HazelDev.com
    // Created: 23.Jul.2014
    // Changed: 29.Sep.2014
    // Version: 1.3.0
    //
    //|------DO-NOT-REMOVE------|
     
    #region  Control Renderer
     
    #region  Color Table
     
    public abstract class xColorTable
    {
        public abstract Color TextColor { get; }
        public abstract Color Background { get; }
        public abstract Color SelectionBorder { get; }
        public abstract Color SelectionTopGradient { get; }
        public abstract Color SelectionMidGradient { get; }
        public abstract Color SelectionBottomGradient { get; }
        public abstract Color PressedBackground { get; }
        public abstract Color CheckedBackground { get; }
        public abstract Color CheckedSelectedBackground { get; }
        public abstract Color DropdownBorder { get; }
        public abstract Color Arrow { get; }
        public abstract Color OverflowBackground { get; }
    }
     
    public abstract class ColorTable
    {
        public abstract xColorTable CommonColorTable { get; }
        public abstract Color BackgroundTopGradient { get; }
        public abstract Color BackgroundBottomGradient { get; }
        public abstract Color DroppedDownItemBackground { get; }
        public abstract Color DropdownTopGradient { get; }
        public abstract Color DropdownBottomGradient { get; }
        public abstract Color Separator { get; }
        public abstract Color ImageMargin { get; }
    }
     
    public class MSColorTable : ColorTable
    {
     
        private xColorTable _CommonColorTable;
     
        public MSColorTable()
        {
            _CommonColorTable = new DefaultCColorTable();
        }
     
        public override xColorTable CommonColorTable
        {
            get
            {
                return _CommonColorTable;
            }
        }
     
        public override System.Drawing.Color BackgroundTopGradient
        {
            get
            {
                return Color.FromArgb(246, 246, 246);
            }
        }
     
        public override System.Drawing.Color BackgroundBottomGradient
        {
            get
            {
                return Color.FromArgb(226, 226, 226);
            }
        }
     
        public override System.Drawing.Color DropdownTopGradient
        {
            get
            {
                return Color.FromArgb(246, 246, 246);
            }
        }
     
        public override System.Drawing.Color DropdownBottomGradient
        {
            get
            {
                return Color.FromArgb(246, 246, 246);
            }
        }
     
        public override System.Drawing.Color DroppedDownItemBackground
        {
            get
            {
                return Color.FromArgb(240, 240, 240);
            }
        }
     
        public override System.Drawing.Color Separator
        {
            get
            {
                return Color.FromArgb(190, 195, 203);
            }
        }
     
        public override System.Drawing.Color ImageMargin
        {
            get
            {
                return Color.FromArgb(240, 240, 240);
            }
        }
    }
     
    public class DefaultCColorTable : xColorTable
    {
     
        public override System.Drawing.Color CheckedBackground
        {
            get
            {
                return Color.FromArgb(230, 230, 230);
            }
        }
     
        public override System.Drawing.Color CheckedSelectedBackground
        {
            get
            {
                return Color.FromArgb(230, 230, 230);
            }
        }
     
        public override System.Drawing.Color SelectionBorder
        {
            get
            {
                return Color.FromArgb(180, 180, 180);
            }
        }
     
        public override System.Drawing.Color SelectionTopGradient
        {
            get
            {
                return Color.FromArgb(240, 240, 240);
            }
        }
     
        public override System.Drawing.Color SelectionMidGradient
        {
            get
            {
                return Color.FromArgb(235, 235, 235);
            }
        }
     
        public override System.Drawing.Color SelectionBottomGradient
        {
            get
            {
                return Color.FromArgb(230, 230, 230);
            }
        }
     
        public override System.Drawing.Color PressedBackground
        {
            get
            {
                return Color.FromArgb(232, 232, 232);
            }
        }
     
        public override System.Drawing.Color TextColor
        {
            get
            {
                return Color.FromArgb(80, 80, 80);
            }
        }
     
        public override System.Drawing.Color Background
        {
            get
            {
                return Color.FromArgb(188, 199, 216);
            }
        }
     
        public override System.Drawing.Color DropdownBorder
        {
            get
            {
                return Color.LightGray;
            }
        }
     
        public override System.Drawing.Color Arrow
        {
            get
            {
                return Color.Black;
            }
        }
     
        public override System.Drawing.Color OverflowBackground
        {
            get
            {
                return Color.FromArgb(213, 220, 232);
            }
        }
    }
     
    #endregion
    #region  Renderer
     
    public class ControlRenderer : ToolStripProfessionalRenderer
    {
     
        public ControlRenderer()
            : this(new MSColorTable())
        {
        }
     
        public ControlRenderer(ColorTable ColorTable)
        {
            this.ColorTable = ColorTable;
        }
     
        private ColorTable _ColorTable;
        public new ColorTable ColorTable
        {
            get
            {
                if (_ColorTable == null)
                {
                    _ColorTable = new MSColorTable();
                }
                return _ColorTable;
            }
            set
            {
                _ColorTable = value;
            }
        }
     
        protected override void OnRenderToolStripBackground(System.Windows.Forms.ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBackground(e);
     
            // Menu strip bar gradient
            using (LinearGradientBrush LGB = new LinearGradientBrush(e.AffectedBounds, this.ColorTable.BackgroundTopGradient, this.ColorTable.BackgroundBottomGradient, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(LGB, e.AffectedBounds);
            }
     
        }
     
        protected override void OnRenderToolStripBorder(System.Windows.Forms.ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip.Parent == null)
            {
                // Draw border around the menu drop-down
                Rectangle Rect = new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
                using (Pen P1 = new Pen(this.ColorTable.CommonColorTable.DropdownBorder))
                {
                    e.Graphics.DrawRectangle(P1, Rect);
                }
     
     
                // Fill the gap between menu drop-down and owner item
                using (SolidBrush B1 = new SolidBrush(this.ColorTable.DroppedDownItemBackground))
                {
                    e.Graphics.FillRectangle(B1, e.ConnectedArea);
                }
     
            }
        }
     
        protected override void OnRenderMenuItemBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Enabled)
            {
                if (e.Item.Selected)
                {
                    if (!e.Item.IsOnDropDown)
                    {
                        Rectangle SelRect = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1);
                        RectDrawing.DrawSelection(e.Graphics, this.ColorTable.CommonColorTable, SelRect);
                    }
                    else
                    {
                        Rectangle SelRect = new Rectangle(2, 0, e.Item.Width - 4, e.Item.Height - 1);
                        RectDrawing.DrawSelection(e.Graphics, this.ColorTable.CommonColorTable, SelRect);
                    }
                }
     
                if (((ToolStripMenuItem)e.Item).DropDown.Visible && !e.Item.IsOnDropDown)
                {
                    Rectangle BorderRect = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height);
                    // Fill the background
                    Rectangle BackgroundRect = new Rectangle(1, 1, e.Item.Width - 2, e.Item.Height + 2);
                    using (SolidBrush B1 = new SolidBrush(this.ColorTable.DroppedDownItemBackground))
                    {
                        e.Graphics.FillRectangle(B1, BackgroundRect);
                    }
     
     
                    // Draw border
                    using (Pen P1 = new Pen(this.ColorTable.CommonColorTable.DropdownBorder))
                    {
                        RectDrawing.DrawRoundedRectangle(e.Graphics, P1, System.Convert.ToSingle(BorderRect.X), System.Convert.ToSingle(BorderRect.Y), System.Convert.ToSingle(BorderRect.Width), System.Convert.ToSingle(BorderRect.Height), 2);
                    }
     
                }
                e.Item.ForeColor = this.ColorTable.CommonColorTable.TextColor;
            }
        }
     
        protected override void OnRenderItemText(System.Windows.Forms.ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = this.ColorTable.CommonColorTable.TextColor;
            base.OnRenderItemText(e);
        }
     
        protected override void OnRenderItemCheck(System.Windows.Forms.ToolStripItemImageRenderEventArgs e)
        {
            base.OnRenderItemCheck(e);
     
            Rectangle rect = new Rectangle(3, 1, e.Item.Height - 3, e.Item.Height - 3);
            Color c = default(Color);
     
            if (e.Item.Selected)
            {
                c = this.ColorTable.CommonColorTable.CheckedSelectedBackground;
            }
            else
            {
                c = this.ColorTable.CommonColorTable.CheckedBackground;
            }
     
            using (SolidBrush b = new SolidBrush(c))
            {
                e.Graphics.FillRectangle(b, rect);
            }
     
     
            using (Pen p = new Pen(this.ColorTable.CommonColorTable.SelectionBorder))
            {
                e.Graphics.DrawRectangle(p, rect);
            }
     
     
            e.Graphics.DrawString("ü", new Font("Wingdings", 13, FontStyle.Regular), Brushes.Black, new Point(4, 2));
        }
     
        protected override void OnRenderSeparator(System.Windows.Forms.ToolStripSeparatorRenderEventArgs e)
        {
            base.OnRenderSeparator(e);
            int PT1 = 28;
            int PT2 = System.Convert.ToInt32(e.Item.Width);
            int Y = 3;
            using (Pen P1 = new Pen(this.ColorTable.Separator))
            {
                e.Graphics.DrawLine(P1, PT1, Y, PT2, Y);
            }
     
        }
     
        protected override void OnRenderImageMargin(System.Windows.Forms.ToolStripRenderEventArgs e)
        {
            base.OnRenderImageMargin(e);
     
            Rectangle BackgroundRect = new Rectangle(0, -1, e.ToolStrip.Width, e.ToolStrip.Height + 1);
            using (LinearGradientBrush LGB = new LinearGradientBrush(BackgroundRect,
                    this.ColorTable.DropdownTopGradient,
                    this.ColorTable.DropdownBottomGradient,
                    LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(LGB, BackgroundRect);
            }
     
     
            using (SolidBrush B1 = new SolidBrush(this.ColorTable.ImageMargin))
            {
                e.Graphics.FillRectangle(B1, e.AffectedBounds);
            }
     
        }
     
        protected override void OnRenderButtonBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1);
            bool @checked = System.Convert.ToBoolean(((ToolStripButton)e.Item).Checked);
            bool drawBorder = false;
     
            if (@checked)
            {
                drawBorder = true;
     
                if (e.Item.Selected && !e.Item.Pressed)
                {
                    using (SolidBrush b = new SolidBrush(this.ColorTable.CommonColorTable.CheckedSelectedBackground))
                    {
                        e.Graphics.FillRectangle(b, rect);
                    }
     
                }
                else
                {
                    using (SolidBrush b = new SolidBrush(this.ColorTable.CommonColorTable.CheckedBackground))
                    {
                        e.Graphics.FillRectangle(b, rect);
                    }
     
                }
     
            }
            else
            {
     
                if (e.Item.Pressed)
                {
                    drawBorder = true;
                    using (SolidBrush b = new SolidBrush(this.ColorTable.CommonColorTable.PressedBackground))
                    {
                        e.Graphics.FillRectangle(b, rect);
                    }
     
                }
                else if (e.Item.Selected)
                {
                    drawBorder = true;
                    RectDrawing.DrawSelection(e.Graphics, this.ColorTable.CommonColorTable, rect);
                }
     
            }
     
            if (drawBorder)
            {
                using (Pen p = new Pen(this.ColorTable.CommonColorTable.SelectionBorder))
                {
                    e.Graphics.DrawRectangle(p, rect);
                }
     
            }
        }
     
        protected override void OnRenderDropDownButtonBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1);
            bool drawBorder = false;
     
            if (e.Item.Pressed)
            {
                drawBorder = true;
                using (SolidBrush b = new SolidBrush(this.ColorTable.CommonColorTable.PressedBackground))
                {
                    e.Graphics.FillRectangle(b, rect);
                }
     
            }
            else if (e.Item.Selected)
            {
                drawBorder = true;
                RectDrawing.DrawSelection(e.Graphics, this.ColorTable.CommonColorTable, rect);
            }
     
            if (drawBorder)
            {
                using (Pen p = new Pen(this.ColorTable.CommonColorTable.SelectionBorder))
                {
                    e.Graphics.DrawRectangle(p, rect);
                }
     
            }
        }
     
        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderSplitButtonBackground(e);
            bool drawBorder = false;
            bool drawSeparator = true;
            ToolStripSplitButton item = (ToolStripSplitButton)e.Item;
            checked
            {
                Rectangle btnRect = new Rectangle(0, 0, item.ButtonBounds.Width - 1, item.ButtonBounds.Height - 1);
                Rectangle borderRect = new Rectangle(0, 0, item.Bounds.Width - 1, item.Bounds.Height - 1);
                bool flag = item.DropDownButtonPressed;
                if (flag)
                {
                    drawBorder = true;
                    drawSeparator = false;
                    SolidBrush b = new SolidBrush(this.ColorTable.CommonColorTable.PressedBackground);
                    try
                    {
                        e.Graphics.FillRectangle(b, borderRect);
                    }
                    finally
                    {
                        flag = (b != null);
                        if (flag)
                        {
                            ((IDisposable)b).Dispose();
                        }
                    }
                }
                else
                {
                    flag = item.DropDownButtonSelected;
                    if (flag)
                    {
                        drawBorder = true;
                        RectDrawing.DrawSelection(e.Graphics, this.ColorTable.CommonColorTable, borderRect);
                    }
                }
                flag = item.ButtonPressed;
                if (flag)
                {
                    SolidBrush b2 = new SolidBrush(this.ColorTable.CommonColorTable.PressedBackground);
                    try
                    {
                        e.Graphics.FillRectangle(b2, btnRect);
                    }
                    finally
                    {
                        flag = (b2 != null);
                        if (flag)
                        {
                            ((IDisposable)b2).Dispose();
                        }
                    }
                }
                flag = drawBorder;
                if (flag)
                {
                    Pen p = new Pen(this.ColorTable.CommonColorTable.SelectionBorder);
                    try
                    {
                        e.Graphics.DrawRectangle(p, borderRect);
                        flag = drawSeparator;
                        if (flag)
                        {
                            e.Graphics.DrawRectangle(p, btnRect);
                        }
                    }
                    finally
                    {
                        flag = (p != null);
                        if (flag)
                        {
                            ((IDisposable)p).Dispose();
                        }
                    }
                    this.DrawCustomArrow(e.Graphics, item);
                }
            }
        }
     
     
        private void DrawCustomArrow(Graphics g, ToolStripSplitButton item)
        {
            int dropWidth = System.Convert.ToInt32(item.DropDownButtonBounds.Width - 1);
            int dropHeight = System.Convert.ToInt32(item.DropDownButtonBounds.Height - 1);
            float triangleWidth = dropWidth / 2.0F + 1;
            float triangleLeft = System.Convert.ToSingle(item.DropDownButtonBounds.Left + (dropWidth - triangleWidth) / 2.0F);
            float triangleHeight = triangleWidth / 2.0F;
            float triangleTop = System.Convert.ToSingle(item.DropDownButtonBounds.Top + (dropHeight - triangleHeight) / 2.0F + 1);
            RectangleF arrowRect = new RectangleF(triangleLeft, triangleTop, triangleWidth, triangleHeight);
     
            this.DrawCustomArrow(g, item, Rectangle.Round(arrowRect));
        }
     
        private void DrawCustomArrow(Graphics g, ToolStripItem item, Rectangle rect)
        {
            ToolStripArrowRenderEventArgs arrowEventArgs = new ToolStripArrowRenderEventArgs(g, item, rect, this.ColorTable.CommonColorTable.Arrow, ArrowDirection.Down);
            base.OnRenderArrow(arrowEventArgs);
        }
     
        protected override void OnRenderOverflowButtonBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            Rectangle rect = default(Rectangle);
            Rectangle rectEnd = default(Rectangle);
            rect = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 2);
            rectEnd = new Rectangle(rect.X - 5, rect.Y, rect.Width - 5, rect.Height);
     
            if (e.Item.Pressed)
            {
                using (SolidBrush b = new SolidBrush(this.ColorTable.CommonColorTable.PressedBackground))
                {
                    e.Graphics.FillRectangle(b, rect);
                }
     
            }
            else if (e.Item.Selected)
            {
                RectDrawing.DrawSelection(e.Graphics, this.ColorTable.CommonColorTable, rect);
            }
            else
            {
                using (SolidBrush b = new SolidBrush(this.ColorTable.CommonColorTable.OverflowBackground))
                {
                    e.Graphics.FillRectangle(b, rect);
                }
     
            }
     
            using (Pen P1 = new Pen(this.ColorTable.CommonColorTable.Background))
            {
                RectDrawing.DrawRoundedRectangle(e.Graphics, P1, System.Convert.ToSingle(rectEnd.X), System.Convert.ToSingle(rectEnd.Y), System.Convert.ToSingle(rectEnd.Width), System.Convert.ToSingle(rectEnd.Height), 3);
            }
     
     
            // Icon
            int w = System.Convert.ToInt32(rect.Width - 1);
            int h = System.Convert.ToInt32(rect.Height - 1);
            float triangleWidth = w / 2.0F + 1;
            float triangleLeft = System.Convert.ToSingle(rect.Left + (w - triangleWidth) / 2.0F + 3);
            float triangleHeight = triangleWidth / 2.0F;
            float triangleTop = System.Convert.ToSingle(rect.Top + (h - triangleHeight) / 2.0F + 7);
            RectangleF arrowRect = new RectangleF(triangleLeft, triangleTop, triangleWidth, triangleHeight);
            this.DrawCustomArrow(e.Graphics, e.Item, Rectangle.Round(arrowRect));
     
            using (Pen p = new Pen(this.ColorTable.CommonColorTable.Arrow))
            {
                e.Graphics.DrawLine(p, triangleLeft + 2, triangleTop - 2, triangleLeft + triangleWidth - 2, triangleTop - 2);
            }
     
        }
    }
     
    #endregion
    #region  Drawing
     
    public class RectDrawing
    {
     
        public static void DrawSelection(Graphics G, xColorTable ColorTable, Rectangle Rect)
        {
            Rectangle TopRect = default(Rectangle);
            Rectangle BottomRect = default(Rectangle);
            Rectangle FillRect = new Rectangle(Rect.X + 1, Rect.Y + 1, Rect.Width - 1, Rect.Height - 1);
     
            TopRect = FillRect;
            TopRect.Height -= System.Convert.ToInt32(TopRect.Height / 2);
            BottomRect = new Rectangle(TopRect.X, TopRect.Bottom, TopRect.Width, FillRect.Height - TopRect.Height);
     
            // Top gradient
            using (LinearGradientBrush LGB = new LinearGradientBrush(TopRect, ColorTable.SelectionTopGradient, ColorTable.SelectionMidGradient, LinearGradientMode.Vertical))
            {
                G.FillRectangle(LGB, TopRect);
            }
     
     
            // Bottom
            using (SolidBrush B1 = new SolidBrush(ColorTable.SelectionBottomGradient))
            {
                G.FillRectangle(B1, BottomRect);
            }
     
     
            // Border
            using (Pen P1 = new Pen(ColorTable.SelectionBorder))
            {
                RectDrawing.DrawRoundedRectangle(G, P1, System.Convert.ToSingle(Rect.X), System.Convert.ToSingle(Rect.Y), System.Convert.ToSingle(Rect.Width), System.Convert.ToSingle(Rect.Height), 2);
            }
     
        }
     
        public static void DrawRoundedRectangle(Graphics G, Pen P, float X, float Y, float W, float H, float Rad)
        {
     
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddLine(X + Rad, Y, X + W - (Rad * 2), Y);
                gp.AddArc(X + W - (Rad * 2), Y, Rad * 2, Rad * 2, 270, 90);
                gp.AddLine(X + W, Y + Rad, X + W, Y + H - (Rad * 2));
                gp.AddArc(X + W - (Rad * 2), Y + H - (Rad * 2), Rad * 2, Rad * 2, 0, 90);
                gp.AddLine(X + W - (Rad * 2), Y + H, X + Rad, Y + H);
                gp.AddArc(X, Y + H - (Rad * 2), Rad * 2, Rad * 2, 90, 90);
                gp.AddLine(X, Y + H - (Rad * 2), X, Y + Rad);
                gp.AddArc(X, Y, Rad * 2, Rad * 2, 180, 90);
                gp.CloseFigure();
     
                G.SmoothingMode = SmoothingMode.AntiAlias;
                G.DrawPath(P, gp);
                G.SmoothingMode = SmoothingMode.Default;
            }
     
        }
    }
     
    #endregion
     
    #endregion
    #region ThemeContainer
     
    public class iTalk_ThemeContainer : ContainerControl
    {
     
        #region   Variables
     
        private Point MouseP = new Point(0, 0);
        private bool Cap = false;
        private int MoveHeight;
        private bool _DrawBottomBar = true;
        private string _TextBottom = null;
        const int BorderCurve = 7;
     
        #endregion
        #region Properties
     
        public bool DrawBottomBar
        {
            get { return _DrawBottomBar; }
            set
            {
                _DrawBottomBar = value;
                Invalidate();
            }
        }
     
        public string TextBottom
        {
            get { return _TextBottom; }
            set
            {
                _TextBottom = value;
                Invalidate();
            }
        }
     
        #endregion
        #region EventArgs
     
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left & new Rectangle(0, 0, Width, MoveHeight).Contains(e.Location))
            {
                Cap = true;
                MouseP = e.Location;
            }
        }
     
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Cap = false;
        }
     
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            bool cap = this.Cap;
            if (cap)
            {
                this.Parent.Location = Control.MousePosition - (Size)this.MouseP;
            }
        }
     
        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            this.ParentForm.FindForm().Text = this.Text;
        }
     
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }
     
        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }
     
        #endregion
     
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.ParentForm.FormBorderStyle = FormBorderStyle.None;
            this.ParentForm.TransparencyKey = Color.Fuchsia;
        }
     
        protected override void CreateHandle()
        {
            base.CreateHandle();
        }
     
        public iTalk_ThemeContainer()
            : base()
        {
            SetStyle((ControlStyles)139270, true);
            Dock = DockStyle.Fill;
            MoveHeight = 25;
            Padding = new Padding(3, 28, 3, 28);
            Font = new Font("Segoe UI", 8, FontStyle.Regular);
            ForeColor = Color.FromArgb(142, 142, 142);
            BackColor = Color.FromArgb(246, 246, 246);
            DoubleBuffered = true;
        }
     
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
     
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);
            Rectangle ClientRectangle = new Rectangle(0, 0, Width - 1, Height - 1);
            Color TransparencyKey = this.ParentForm.TransparencyKey;
     
            G.SmoothingMode = SmoothingMode.Default;
            G.Clear(TransparencyKey);
     
            // Draw the container borders
            G.FillPath(new SolidBrush(Color.FromArgb(52, 52, 52)), RoundRectangle.RoundRect(ClientRectangle, BorderCurve));
            // Draw a rectangle in which the controls should be added on
            G.FillPath(new SolidBrush(Color.FromArgb(246, 246, 246)), RoundRectangle.RoundRect(new Rectangle(2, 20, Width - 5, Height - 42), BorderCurve));
     
            // Patch the header with a rectangle that has a curve so its border will remain within container bounds
            G.FillPath(new SolidBrush(Color.FromArgb(52, 52, 52)), RoundRectangle.RoundRect(new Rectangle(2, 2, Width / 2 + 2, 16), BorderCurve));
            G.FillPath(new SolidBrush(Color.FromArgb(52, 52, 52)), RoundRectangle.RoundRect(new Rectangle(Width / 2 - 3, 2, Width / 2, 16), BorderCurve));
            // Fill the header rectangle below the patch
            G.FillRectangle(new SolidBrush(Color.FromArgb(52, 52, 52)), new Rectangle(2, 15, Width - 5, 10));
     
            // Increase the thickness of the container borders
            G.DrawPath(new Pen(Color.FromArgb(52, 52, 52)), RoundRectangle.RoundRect(new Rectangle(2, 2, Width - 5, Height - 5), BorderCurve));
            G.DrawPath(new Pen(Color.FromArgb(52, 52, 52)), RoundRectangle.RoundRect(ClientRectangle, BorderCurve));
     
            // Draw the string from the specified 'Text' property on the header rectangle
            G.DrawString(Text, new Font("Trebuchet MS", 10, FontStyle.Bold), new SolidBrush(Color.FromArgb(221, 221, 221)), new Rectangle(BorderCurve, BorderCurve - 4, Width - 1, 22), new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Near
            });
     
     
            // Draws a rectangle at the bottom of the container
            if (_DrawBottomBar == true)
            {
                G.FillRectangle(new SolidBrush(Color.FromArgb(52, 52, 52)), 0, Height - 25, Width - 3, 22 - 2);
                G.DrawLine(new Pen(Color.FromArgb(52, 52, 52)), 5, Height - 5, Width - 6, Height - 5);
                G.DrawLine(new Pen(Color.FromArgb(52, 52, 52)), 7, Height - 4, Width - 7, Height - 4);
     
                G.DrawString(_TextBottom, new Font("Trebuchet MS", 10, FontStyle.Bold), new SolidBrush(Color.FromArgb(221, 221, 221)), 5, Height - 23);
            }
            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }
     
    #endregion
    #region ControlBox
     
    public class iTalk_ControlBox : Control
    {
     
        #region Enums
     
        public enum MouseState : byte
        {
            None = 0,
            Over = 1,
            Down = 2,
            Block = 3
        }
     
        #endregion
        #region Variables
     
        MouseState State = MouseState.None;
        int i;
        Rectangle CloseRect = new Rectangle(28, 0, 47, 18);
        Rectangle MinimizeRect = new Rectangle(0, 0, 28, 18);
     
        #endregion
        #region EventArgs
     
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (i > 0 & i < 28)
            {
                this.FindForm().WindowState = FormWindowState.Minimized;
            }
            else if (i > 30 & i < 75)
            {
                this.FindForm().Close();
            }
     
            State = MouseState.Down;
        }
     
        protected override void OnMouseEnter(System.EventArgs e)
        {
            base.OnMouseEnter(e);
            State = MouseState.Over;
            Invalidate();
        }
     
        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
            State = MouseState.None;
            Invalidate();
        }
     
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            State = MouseState.Over;
            Invalidate();
        }
     
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            i = e.Location.X;
            Invalidate();
        }
     
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Width = 77;
            Height = 19;
        }
     
        #endregion
     
        public iTalk_ControlBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
            DoubleBuffered = true;
            Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Point location = new Point(checked(this.FindForm().Width - 81), -1);
            this.Location = location;
        }
     
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
     
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);
            GraphicsPath GP_MinimizeRect = new GraphicsPath();
            GraphicsPath GP_CloseRect = new GraphicsPath();
     
            GP_MinimizeRect.AddRectangle(MinimizeRect);
            GP_CloseRect.AddRectangle(CloseRect);
            G.Clear(BackColor);
     
            switch (State)
            {
                case MouseState.None:
                NonePoint:
                    LinearGradientBrush MinimizeGradient = new LinearGradientBrush(MinimizeRect, Color.FromArgb(73, 73, 73), Color.FromArgb(58, 58, 58), 90);
                    G.FillPath(MinimizeGradient, GP_MinimizeRect);
                    G.DrawPath(new Pen(Color.FromArgb(40, 40, 40)), GP_MinimizeRect);
                    G.DrawString("0", new Font("Marlett", 11, FontStyle.Regular), new SolidBrush(Color.FromArgb(221, 221, 221)), MinimizeRect.Width - 22, MinimizeRect.Height - 16);
     
                    LinearGradientBrush CloseGradient = new LinearGradientBrush(CloseRect, Color.FromArgb(73, 73, 73), Color.FromArgb(58, 58, 58), 90);
                    G.FillPath(CloseGradient, GP_CloseRect);
                    G.DrawPath(new Pen(Color.FromArgb(40, 40, 40)), GP_CloseRect);
                    G.DrawString("r", new Font("Marlett", 11, FontStyle.Regular), new SolidBrush(Color.FromArgb(221, 221, 221)), CloseRect.Width - 4, CloseRect.Height - 16);
                    break;
                case MouseState.Over:
                    if (i > 0 & i < 28)
                    {
                        LinearGradientBrush xMinimizeGradient = new LinearGradientBrush(this.MinimizeRect, Color.FromArgb(76, 76, 76, 76), Color.FromArgb(48, 48, 48), 90f);
                        G.FillPath(xMinimizeGradient, GP_MinimizeRect);
                        G.DrawPath(new Pen(Color.FromArgb(40, 40, 40)), GP_MinimizeRect);
                        G.DrawString("0", new Font("Marlett", 11, FontStyle.Regular), new SolidBrush(Color.FromArgb(221, 221, 221)), MinimizeRect.Width - 22, MinimizeRect.Height - 16);
     
                        LinearGradientBrush xCloseGradient = new LinearGradientBrush(CloseRect, Color.FromArgb(73, 73, 73), Color.FromArgb(58, 58, 58), 90);
                        G.FillPath(xCloseGradient, GP_CloseRect);
                        G.DrawPath(new Pen(Color.FromArgb(40, 40, 40)), GP_CloseRect);
                        G.DrawString("r", new Font("Marlett", 11, FontStyle.Regular), new SolidBrush(Color.FromArgb(221, 221, 221)), CloseRect.Width - 4, CloseRect.Height - 16);
                    }
                    else if (i > 30 & i < 75)
                    {
                        LinearGradientBrush xCloseGradient = new LinearGradientBrush(CloseRect, Color.FromArgb(76, 76, 76, 76), Color.FromArgb(48, 48, 48), 90);
                        G.FillPath(xCloseGradient, GP_CloseRect);
                        G.DrawPath(new Pen(Color.FromArgb(40, 40, 40)), GP_CloseRect);
                        G.DrawString("r", new Font("Marlett", 11, FontStyle.Regular), new SolidBrush(Color.FromArgb(221, 221, 221)), CloseRect.Width - 4, CloseRect.Height - 16);
     
                        LinearGradientBrush xMinimizeGradient = new LinearGradientBrush(MinimizeRect, Color.FromArgb(73, 73, 73), Color.FromArgb(58, 58, 58), 90);
                        G.FillPath(xMinimizeGradient, RoundRectangle.RoundRect(MinimizeRect, 1));
                        G.DrawPath(new Pen(Color.FromArgb(40, 40, 40)), GP_MinimizeRect);
                        G.DrawString("0", new Font("Marlett", 11, FontStyle.Regular), new SolidBrush(Color.FromArgb(221, 221, 221)), MinimizeRect.Width - 22, MinimizeRect.Height - 16);
                    }
                    else
                    {
                        goto NonePoint; // Return to [MouseState = None]    
                    }
                    break;
            }
     
            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            G.Dispose();
            GP_CloseRect.Dispose();
            GP_MinimizeRect.Dispose();
            B.Dispose();
        }
    }
     
    #endregion
    #region Button 1
     
    class iTalk_Button_1 : Control
    {
     
        #region Variables
     
        private int MouseState;
        private GraphicsPath Shape;
        private LinearGradientBrush InactiveGB;
        private LinearGradientBrush PressedGB;
        private LinearGradientBrush PressedContourGB;
        private Rectangle R1;
        private Pen P1;
        private Pen P3;
        private Image _Image;
        private Size _ImageSize;
        private StringAlignment _TextAlignment = StringAlignment.Center;
        private Color _TextColor = Color.FromArgb(150, 150, 150);
        private ContentAlignment _ImageAlign = ContentAlignment.MiddleLeft;
     
        #endregion
        #region Image Designer
     
        private static PointF ImageLocation(StringFormat SF, SizeF Area, SizeF ImageArea)
        {
            PointF MyPoint = default(PointF);
            switch (SF.Alignment)
            {
                case StringAlignment.Center:
                    MyPoint.X = Convert.ToSingle((Area.Width - ImageArea.Width) / 2);
                    break;
                case StringAlignment.Near:
                    MyPoint.X = 2;
                    break;
                case StringAlignment.Far:
                    MyPoint.X = Area.Width - ImageArea.Width - 2;
     
                    break;
            }
     
            switch (SF.LineAlignment)
            {
                case StringAlignment.Center:
                    MyPoint.Y = Convert.ToSingle((Area.Height - ImageArea.Height) / 2);
                    break;
                case StringAlignment.Near:
                    MyPoint.Y = 2;
                    break;
                case StringAlignment.Far:
                    MyPoint.Y = Area.Height - ImageArea.Height - 2;
                    break;
            }
            return MyPoint;
        }
     
        private StringFormat GetStringFormat(ContentAlignment _ContentAlignment)
        {
            StringFormat SF = new StringFormat();
            switch (_ContentAlignment)
            {
                case ContentAlignment.MiddleCenter:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleLeft:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleRight:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.TopCenter:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopLeft:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomRight:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Far;
                    break;
            }
            return SF;
        }
     
        #endregion
        #region Properties
     
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
     
        protected Size ImageSize
        {
            get { return _ImageSize; }
        }
     
        public ContentAlignment ImageAlign
        {
            get { return _ImageAlign; }
            set
            {
                _ImageAlign = value;
                Invalidate();
            }
        }
     
        public StringAlignment TextAlignment
        {
            get { return this._TextAlignment; }
            set
            {
                this._TextAlignment = value;
                this.Invalidate();
            }
        }
     
        public override Color ForeColor
        {
            get { return this._TextColor; }
            set
            {
                this._TextColor = value;
                this.Invalidate();
            }
        }
     
        #endregion
        #region EventArgs
     
        protected override void OnMouseUp(MouseEventArgs e)
        {
            MouseState = 0;
            Invalidate();
            base.OnMouseUp(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseState = 1;
            Invalidate();
            base.OnMouseDown(e);
        }
     
        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = 0;
            Invalidate();
            base.OnMouseLeave(e);
        }
     
        protected override void OnTextChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }
     
        #endregion
     
        public iTalk_Button_1()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
     
            BackColor = Color.Transparent;
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 12);
            ForeColor = Color.FromArgb(150, 150, 150);
            Size = new Size(166, 40);
            _TextAlignment = StringAlignment.Center;
            P1 = new Pen(Color.FromArgb(190, 190, 190)); // P1 = Border color
        }
     
        protected override void OnResize(System.EventArgs e)
        {
            if (Width > 0 && Height > 0)
            {
                Shape = new GraphicsPath();
                R1 = new Rectangle(0, 0, Width, Height);
     
                InactiveGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(251, 251, 251), Color.FromArgb(225, 225, 225), 90f);
                PressedGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(235, 235, 235), Color.FromArgb(223, 223, 223), 90f);
                PressedContourGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(167, 167, 167), Color.FromArgb(167, 167, 167), 90f);
     
                P3 = new Pen(PressedContourGB);
            }
     
            var _Shape = Shape;
            _Shape.AddArc(0, 0, 10, 10, 180, 90);
            _Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            _Shape.CloseAllFigures();
     
            Invalidate();
            base.OnResize(e);
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            var _G = e.Graphics;
            _G.SmoothingMode = SmoothingMode.HighQuality;
            PointF ipt = ImageLocation(GetStringFormat(ImageAlign), Size, ImageSize);
     
            switch (MouseState)
            {
                case 0:
                    _G.FillPath(InactiveGB, Shape);
                    _G.DrawPath(P1, Shape);
                    if ((Image == null))
                    {
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        _G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
                case 1:
                    _G.FillPath(PressedGB, Shape);
                    _G.DrawPath(P3, Shape);
     
                    if ((Image == null))
                    {
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        _G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
            }
            base.OnPaint(e);
        }
    }
     
    #endregion
    #region Button 2
     
    class iTalk_Button_2 : Control
    {
     
        #region Variables
     
        private int MouseState;
        private GraphicsPath Shape;
        private LinearGradientBrush InactiveGB;
        private LinearGradientBrush PressedGB;
        private LinearGradientBrush PressedContourGB;
        private Rectangle R1;
        private Pen P1;
        private Pen P3;
        private Image _Image;
        private Size _ImageSize;
        private StringAlignment _TextAlignment = StringAlignment.Center;
        private ContentAlignment _ImageAlign = ContentAlignment.MiddleLeft;
     
        #endregion
        #region Image Designer
     
        private static PointF ImageLocation(StringFormat SF, SizeF Area, SizeF ImageArea)
        {
            PointF MyPoint = default(PointF);
            switch (SF.Alignment)
            {
                case StringAlignment.Center:
                    MyPoint.X = Convert.ToSingle((Area.Width - ImageArea.Width) / 2);
                    break;
                case StringAlignment.Near:
                    MyPoint.X = 2;
                    break;
                case StringAlignment.Far:
                    MyPoint.X = Area.Width - ImageArea.Width - 2;
                    break;
            }
     
            switch (SF.LineAlignment)
            {
                case StringAlignment.Center:
                    MyPoint.Y = Convert.ToSingle((Area.Height - ImageArea.Height) / 2);
                    break;
                case StringAlignment.Near:
                    MyPoint.Y = 2;
                    break;
                case StringAlignment.Far:
                    MyPoint.Y = Area.Height - ImageArea.Height - 2;
                    break;
            }
            return MyPoint;
        }
     
        private StringFormat GetStringFormat(ContentAlignment _ContentAlignment)
        {
            StringFormat SF = new StringFormat();
            switch (_ContentAlignment)
            {
                case ContentAlignment.MiddleCenter:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleLeft:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleRight:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.TopCenter:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopLeft:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomRight:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Far;
                    break;
            }
            return SF;
        }
     
        #endregion
        #region Properties
     
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
     
        public StringAlignment TextAlignment
        {
            get { return this._TextAlignment; }
            set
            {
                this._TextAlignment = value;
                this.Invalidate();
            }
        }
     
        protected Size ImageSize
        {
            get { return _ImageSize; }
        }
     
        public ContentAlignment ImageAlign
        {
            get { return _ImageAlign; }
            set
            {
                _ImageAlign = value;
                Invalidate();
            }
        }
     
        #endregion
        #region EventArgs
     
        protected override void OnMouseUp(MouseEventArgs e)
        {
            MouseState = 0;
            Invalidate();
            base.OnMouseUp(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseState = 1;
            Invalidate();
            base.OnMouseDown(e);
        }
     
        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = 0;
            // [Inactive]
            Invalidate();
            // Update control
            base.OnMouseLeave(e);
        }
     
        protected override void OnTextChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }
     
        #endregion
     
        public iTalk_Button_2()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
     
            BackColor = Color.Transparent;
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 14);
            ForeColor = Color.White;
            Size = new Size(166, 40);
            _TextAlignment = StringAlignment.Center;
            P1 = new Pen(Color.FromArgb(0, 118, 176));
        }
     
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            if (Width > 0 && Height > 0)
            {
                Shape = new GraphicsPath();
                R1 = new Rectangle(0, 0, Width, Height);
     
                InactiveGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(0, 176, 231), Color.FromArgb(0, 152, 224), 90f);
                PressedGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(0, 118, 176), Color.FromArgb(0, 149, 222), 90f);
                PressedContourGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(0, 118, 176), Color.FromArgb(0, 118, 176), 90f);
     
                P3 = new Pen(PressedContourGB);
            }
     
            var _Shape = Shape;
            _Shape.AddArc(0, 0, 10, 10, 180, 90);
            _Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            _Shape.CloseAllFigures();
     
            Invalidate();
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            var _G = e.Graphics;
            _G.SmoothingMode = SmoothingMode.HighQuality;
     
            PointF ipt = ImageLocation(GetStringFormat(ImageAlign), Size, ImageSize);
     
            switch (MouseState)
            {
                case 0:
                    _G.FillPath(InactiveGB, Shape);
                    _G.DrawPath(P1, Shape);
                    if ((Image == null))
                    {
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        _G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
                case 1:
                    _G.FillPath(PressedGB, Shape);
                    _G.DrawPath(P3, Shape);
                    if ((Image == null))
                    {
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        _G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
            }
            base.OnPaint(e);
        }
    }
     
    #endregion
    #region Toggle Button
     
    [DefaultEvent("ToggledChanged")]
    class iTalk_Toggle : Control
    {
     
        #region Designer
     
        //|------DO-NOT-REMOVE------|
        //|---------CREDITS---------|
     
        // Pill class and functions were originally created by Tedd
        // Last edited by Tedd on: 12/20/2013
        // Modified by HazelDev on: 1/4/2014
     
        //|---------CREDITS---------|
        //|------DO-NOT-REMOVE------|
     
        public class PillStyle
        {
            public bool Left;
            public bool Right;
        }
     
        public GraphicsPath Pill(Rectangle Rectangle, PillStyle PillStyle)
        {
            GraphicsPath functionReturnValue = default(GraphicsPath);
            functionReturnValue = new GraphicsPath();
     
            if (PillStyle.Left)
            {
                functionReturnValue.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Height, Rectangle.Height), -270, 180);
            }
            else
            {
                functionReturnValue.AddLine(Rectangle.X, Rectangle.Y + Rectangle.Height, Rectangle.X, Rectangle.Y);
            }
     
            if (PillStyle.Right)
            {
                functionReturnValue.AddArc(new Rectangle(Rectangle.X + Rectangle.Width - Rectangle.Height, Rectangle.Y, Rectangle.Height, Rectangle.Height), -90, 180);
            }
            else
            {
                functionReturnValue.AddLine(Rectangle.X + Rectangle.Width, Rectangle.Y, Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height);
            }
     
            functionReturnValue.CloseAllFigures();
            return functionReturnValue;
        }
     
        public object Pill(int X, int Y, int Width, int Height, PillStyle PillStyle)
        {
            return Pill(new Rectangle(X, Y, Width, Height), PillStyle);
        }
     
        #endregion
        #region Enums
     
        public enum _Type
        {
            YesNo,
            OnOff,
            IO
        }
     
        #endregion
        #region Variables
     
        private Timer AnimationTimer = new Timer { Interval = 1 };
        private int ToggleLocation = 0;
        public event ToggledChangedEventHandler ToggledChanged;
        public delegate void ToggledChangedEventHandler();
        private bool _Toggled;
        private _Type ToggleType;
        private Rectangle Bar;
        private Size cHandle = new Size(15, 20);
     
        #endregion
        #region Properties
     
        public bool Toggled
        {
            get { return _Toggled; }
            set
            {
                _Toggled = value;
                Invalidate();
     
                if (ToggledChanged != null)
                {
                    ToggledChanged();
                }
            }
        }
     
        public _Type Type
        {
            get { return ToggleType; }
            set
            {
                ToggleType = value;
                Invalidate();
            }
        }
     
        #endregion
        #region EventArgs
     
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Width = 41;
            Height = 23;
        }
     
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Toggled = !Toggled;
        }
     
        #endregion
     
        public iTalk_Toggle()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            AnimationTimer.Tick += new EventHandler(AnimationTimer_Tick);
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AnimationTimer.Start();
        }
     
        void AnimationTimer_Tick(object sender, EventArgs e)
        {
            //  Create a slide animation when toggled on/off
            if ((_Toggled == true))
            {
                if ((ToggleLocation < 100))
                {
                    ToggleLocation += 10;
                    this.Invalidate(false);
                }
            }
            else if ((ToggleLocation > 0))
            {
                ToggleLocation -= 10;
                this.Invalidate(false);
            }
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;
            G.Clear(Parent.BackColor);
            checked
            {
                Point point = new Point(0, (int)Math.Round(unchecked((double)this.Height / 2.0 - (double)this.cHandle.Height / 2.0)));
                Point arg_A8_0 = point;
                Point point2 = new Point(0, (int)Math.Round(unchecked((double)this.Height / 2.0 + (double)this.cHandle.Height / 2.0)));
                LinearGradientBrush Gradient = new LinearGradientBrush(arg_A8_0, point2, Color.FromArgb(250, 250, 250), Color.FromArgb(240, 240, 240));
                this.Bar = new Rectangle(8, 10, this.Width - 21, this.Height - 21);
     
                G.SmoothingMode = SmoothingMode.AntiAlias;
                G.FillPath(Gradient, (GraphicsPath)this.Pill(0, (int)Math.Round(unchecked((double)this.Height / 2.0 - (double)this.cHandle.Height / 2.0)), this.Width - 1, this.cHandle.Height - 5, new iTalk_Toggle.PillStyle
                {
                    Left = true,
                    Right = true
                }));
                G.DrawPath(new Pen(Color.FromArgb(177, 177, 176)), (GraphicsPath)this.Pill(0, (int)Math.Round(unchecked((double)this.Height / 2.0 - (double)this.cHandle.Height / 2.0)), this.Width - 1, this.cHandle.Height - 5, new iTalk_Toggle.PillStyle
                {
                    Left = true,
                    Right = true
                }));
                Gradient.Dispose();
                switch (this.ToggleType)
                {
                    case iTalk_Toggle._Type.YesNo:
                        {
                            bool toggled = this.Toggled;
                            if (toggled)
                            {
                                G.DrawString("Yes", new Font("Segoe UI", 7f, FontStyle.Regular), Brushes.Gray, (float)(this.Bar.X + 7), (float)this.Bar.Y, new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                });
                            }
                            else
                            {
                                G.DrawString("No", new Font("Segoe UI", 7f, FontStyle.Regular), Brushes.Gray, (float)(this.Bar.X + 18), (float)this.Bar.Y, new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                });
                            }
                            break;
                        }
                    case iTalk_Toggle._Type.OnOff:
                        {
                            bool toggled = this.Toggled;
                            if (toggled)
                            {
                                G.DrawString("On", new Font("Segoe UI", 7f, FontStyle.Regular), Brushes.Gray, (float)(this.Bar.X + 7), (float)this.Bar.Y, new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                });
                            }
                            else
                            {
                                G.DrawString("Off", new Font("Segoe UI", 7f, FontStyle.Regular), Brushes.Gray, (float)(this.Bar.X + 18), (float)this.Bar.Y, new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                });
                            }
                            break;
                        }
                    case iTalk_Toggle._Type.IO:
                        {
                            bool toggled = this.Toggled;
                            if (toggled)
                            {
                                G.DrawString("I", new Font("Segoe UI", 7f, FontStyle.Regular), Brushes.Gray, (float)(this.Bar.X + 7), (float)this.Bar.Y, new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                });
                            }
                            else
                            {
                                G.DrawString("O", new Font("Segoe UI", 7f, FontStyle.Regular), Brushes.Gray, (float)(this.Bar.X + 18), (float)this.Bar.Y, new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                });
                            }
                            break;
                        }
                }
                G.FillEllipse(new SolidBrush(Color.FromArgb(249, 249, 249)), this.Bar.X + (int)Math.Round(unchecked((double)this.Bar.Width * ((double)this.ToggleLocation / 80.0))) - (int)Math.Round((double)this.cHandle.Width / 2.0), this.Bar.Y + (int)Math.Round((double)this.Bar.Height / 2.0) - (int)Math.Round(unchecked((double)this.cHandle.Height / 2.0 - 1.0)), this.cHandle.Width, this.cHandle.Height - 5);
                G.DrawEllipse(new Pen(Color.FromArgb(177, 177, 176)), this.Bar.X + (int)Math.Round(unchecked((double)this.Bar.Width * ((double)this.ToggleLocation / 80.0) - (double)checked((int)Math.Round((double)this.cHandle.Width / 2.0)))), this.Bar.Y + (int)Math.Round((double)this.Bar.Height / 2.0) - (int)Math.Round(unchecked((double)this.cHandle.Height / 2.0 - 1.0)), this.cHandle.Width, this.cHandle.Height - 5);
            }
        }
    }
    #endregion
    #region Label
     
    class iTalk_Label : Label
    {
     
        public iTalk_Label()
        {
            Font = new Font("Segoe UI", 8);
            ForeColor = Color.FromArgb(142, 142, 142);
            BackColor = Color.Transparent;
        }
    }
     
    #endregion
    #region Link Label
     
    class iTalk_LinkLabel : LinkLabel
    {
     
        public iTalk_LinkLabel()
        {
            Font = new Font("Segoe UI", 8, FontStyle.Regular);
            BackColor = Color.Transparent;
            LinkColor = Color.FromArgb(51, 153, 225);
            ActiveLinkColor = Color.FromArgb(0, 101, 202);
            VisitedLinkColor = Color.FromArgb(0, 101, 202);
            LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
        }
    }
     
    #endregion
    #region Header Label
     
    class iTalk_HeaderLabel : Label
    {
     
        public iTalk_HeaderLabel()
        {
            Font = new Font("Segoe UI", 25, FontStyle.Regular);
            ForeColor = Color.FromArgb(80, 80, 80);
            BackColor = Color.Transparent;
        }
    }
     
    #endregion
    #region Big TextBox
     
    [DefaultEvent("TextChanged")]
    class iTalk_TextBox_Big : Control
    {
        #region Variables
     
        public TextBox iTalkTB = new TextBox();
        private GraphicsPath Shape;
        private int _maxchars = 32767;
        private bool _ReadOnly;
        private bool _Multiline;
        private Image _Image;
        private Size _ImageSize;
        private HorizontalAlignment ALNType;
        private bool isPasswordMasked = false;
        private Pen P1;
        private SolidBrush B1;
     
        #endregion
        #region Properties
     
        public HorizontalAlignment TextAlignment
        {
            get { return ALNType; }
            set
            {
                ALNType = value;
                Invalidate();
            }
        }
        public int MaxLength
        {
            get { return _maxchars; }
            set
            {
                _maxchars = value;
                iTalkTB.MaxLength = MaxLength;
                Invalidate();
            }
        }
     
        public bool UseSystemPasswordChar
        {
            get { return isPasswordMasked; }
            set
            {
                iTalkTB.UseSystemPasswordChar = UseSystemPasswordChar;
                isPasswordMasked = value;
                Invalidate();
            }
        }
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                _ReadOnly = value;
                if (iTalkTB != null)
                {
                    iTalkTB.ReadOnly = value;
                }
            }
        }
        public bool Multiline
        {
            get { return _Multiline; }
            set
            {
                _Multiline = value;
                if (iTalkTB != null)
                {
                    iTalkTB.Multiline = value;
     
                    if (value)
                    {
                        iTalkTB.Height = Height - 23;
                    }
                    else
                    {
                        Height = iTalkTB.Height + 23;
                    }
                }
     
            }
        }
     
        public Image Image
        {
            get
            {
                return _Image;
            }
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
     
                if (Image == null)
                {
                    iTalkTB.Location = new Point(8, 10);
                }
                else
                {
                    iTalkTB.Location = new Point(35, 11);
                }
                Invalidate();
            }
        }
     
        protected Size ImageSize
        {
            get
            {
                return _ImageSize;
            }
        }
     
        #endregion
        #region EventArgs
     
        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            iTalkTB.Text = Text;
            Invalidate();
        }
     
        protected override void OnForeColorChanged(System.EventArgs e)
        {
            base.OnForeColorChanged(e);
            iTalkTB.ForeColor = ForeColor;
            Invalidate();
        }
     
        protected override void OnFontChanged(System.EventArgs e)
        {
            base.OnFontChanged(e);
            iTalkTB.Font = Font;
        }
     
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }
     
        private void _OnKeyDown(object Obj, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                iTalkTB.SelectAll();
                e.SuppressKeyPress = true;
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                iTalkTB.Copy();
                e.SuppressKeyPress = true;
            }
        }
     
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            if (_Multiline)
            {
                iTalkTB.Height = Height - 23;
            }
            else
            {
                Height = iTalkTB.Height + 23;
            }
     
            Shape = new GraphicsPath();
            var _with1 = Shape;
            _with1.AddArc(0, 0, 10, 10, 180, 90);
            _with1.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _with1.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _with1.AddArc(0, Height - 11, 10, 10, 90, 90);
            _with1.CloseAllFigures();
        }
     
        protected override void OnGotFocus(System.EventArgs e)
        {
            base.OnGotFocus(e);
            iTalkTB.Focus();
        }
     
        #endregion
        public void AddTextBox()
        {
            var _TB = iTalkTB;
            _TB.Location = new Point(7, 10);
            _TB.Text = string.Empty;
            _TB.BorderStyle = BorderStyle.None;
            _TB.TextAlign = HorizontalAlignment.Left;
            _TB.Font = new Font("Tahoma", 11);
            _TB.UseSystemPasswordChar = UseSystemPasswordChar;
            _TB.Multiline = false;
            iTalkTB.KeyDown += _OnKeyDown;
        }
     
        public iTalk_TextBox_Big()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
     
        AddTextBox();
        Controls.Add(iTalkTB);
     
        P1 = new Pen(Color.FromArgb(180, 180, 180)); // P1 = Border color
        B1 = new SolidBrush(Color.White); // B1 = Rect Background color
        BackColor = Color.Transparent;
        ForeColor = Color.DimGray;
     
        Text = null;
        Font = new Font("Tahoma", 11);
        Size = new Size(135, 43);
        DoubleBuffered = true;
    }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);
     
            G.SmoothingMode = SmoothingMode.AntiAlias;
     
            if (Image == null)
            {
                iTalkTB.Width = Width - 18;
            }
            else
            {
                iTalkTB.Width = Width - 45;
            }
     
            iTalkTB.TextAlign = TextAlignment;
            iTalkTB.UseSystemPasswordChar = UseSystemPasswordChar;
     
            G.Clear(Color.Transparent);
            G.FillPath(B1, Shape); // Draw background
            G.DrawPath(P1, Shape); // Draw border
     
     
            if (Image != null)
            {
                G.DrawImage(_Image, 5, 8, 24, 24);
                // 24x24 is the perfect size of the image
            }
     
            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }
     
    #endregion
    #region Small TextBox
     
    [DefaultEvent("TextChanged")]
    class iTalk_TextBox_Small : Control
    {
        #region Variables
     
        public TextBox iTalkTB = new TextBox();
        private GraphicsPath Shape;
        private int _maxchars = 32767;
        private bool _ReadOnly;
        private bool _Multiline;
        private HorizontalAlignment ALNType;
        private bool isPasswordMasked = false;
        private Pen P1;
        private SolidBrush B1;
     
        #endregion
        #region Properties
     
        public HorizontalAlignment TextAlignment
        {
            get { return ALNType; }
            set
            {
                ALNType = value;
                Invalidate();
            }
        }
        public int MaxLength
        {
            get { return _maxchars; }
            set
            {
                _maxchars = value;
                iTalkTB.MaxLength = MaxLength;
                Invalidate();
            }
        }
     
        public bool UseSystemPasswordChar
        {
            get { return isPasswordMasked; }
            set
            {
                iTalkTB.UseSystemPasswordChar = UseSystemPasswordChar;
                isPasswordMasked = value;
                Invalidate();
            }
        }
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                _ReadOnly = value;
                if (iTalkTB != null)
                {
                    iTalkTB.ReadOnly = value;
                }
            }
        }
        public bool Multiline
        {
            get { return _Multiline; }
            set
            {
                _Multiline = value;
                if (iTalkTB != null)
                {
                    iTalkTB.Multiline = value;
     
                    if (value)
                    {
                        iTalkTB.Height = Height - 10;
                    }
                    else
                    {
                        Height = iTalkTB.Height + 10;
                    }
                }
            }
        }
     
        #endregion
        #region EventArgs
     
        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            iTalkTB.Text = Text;
            Invalidate();
        }
     
        protected override void OnForeColorChanged(System.EventArgs e)
        {
            base.OnForeColorChanged(e);
            iTalkTB.ForeColor = ForeColor;
            Invalidate();
        }
     
        protected override void OnFontChanged(System.EventArgs e)
        {
            base.OnFontChanged(e);
            iTalkTB.Font = Font;
      }
     
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }
     
        private void _OnKeyDown(object Obj, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                iTalkTB.SelectAll();
                e.SuppressKeyPress = true;
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                iTalkTB.Copy();
                e.SuppressKeyPress = true;
            }
        }
     
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            if (_Multiline)
            {
                iTalkTB.Height = Height - 10;
            }
            else
            {
                Height = iTalkTB.Height + 10;
            }
     
            Shape = new GraphicsPath();
            var _with1 = Shape;
            _with1.AddArc(0, 0, 10, 10, 180, 90);
            _with1.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _with1.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _with1.AddArc(0, Height - 11, 10, 10, 90, 90);
            _with1.CloseAllFigures();
        }
     
        protected override void OnGotFocus(System.EventArgs e)
        {
            base.OnGotFocus(e);
            iTalkTB.Focus();
        }
     
        #endregion
        public void AddTextBox()
        {
            var _TB = iTalkTB;
            _TB.Size = new Size(Width - 10, 33);
            _TB.Location = new Point(7, 5);
            _TB.Text = string.Empty;
            _TB.BorderStyle = BorderStyle.None;
            _TB.TextAlign = HorizontalAlignment.Left;
            _TB.Font = new Font("Tahoma", 11);
            _TB.UseSystemPasswordChar = UseSystemPasswordChar;
            _TB.Multiline = false;
            iTalkTB.KeyDown += _OnKeyDown;
        }
     
        public iTalk_TextBox_Small()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
     
            AddTextBox();
            Controls.Add(iTalkTB);
     
            P1 = new Pen(Color.FromArgb(180, 180, 180)); // P1 = Border color
            B1 = new SolidBrush(Color.White); // B1 = Rect Background color
            BackColor = Color.Transparent;
            ForeColor = Color.DimGray;
     
            Text = null;
            Font = new Font("Tahoma", 11);
            Size = new Size(135, 33);
            DoubleBuffered = true;
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);
     
            G.SmoothingMode = SmoothingMode.AntiAlias;
     
            var _TB = iTalkTB;
            _TB.Width = Width - 10;
            _TB.TextAlign = TextAlignment;
            _TB.UseSystemPasswordChar = UseSystemPasswordChar;
     
            G.Clear(Color.Transparent);
            G.FillPath(B1, Shape); // Draw background
            G.DrawPath(P1, Shape); // Draw border
     
            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            G.Dispose();
            B.Dispose();
        }
     
    }
     
    #endregion
    #region RichTextBox
     
    [DefaultEvent("TextChanged")]
    class iTalk_RichTextBox : Control
    {
     
        #region Variables
     
        public RichTextBox iTalkRTB = new RichTextBox();
        private bool _ReadOnly;
        private bool _WordWrap;
        private bool _AutoWordSelection;
        private GraphicsPath Shape;
     
        #endregion
        #region Properties
     
        public override string Text
        {
            get { return iTalkRTB.Text; }
            set
            {
                iTalkRTB.Text = value;
                Invalidate();
            }
        }
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                _ReadOnly = value;
                if (iTalkRTB != null)
                {
                    iTalkRTB.ReadOnly = value;
                }
            }
        }
        public bool WordWrap
        {
            get { return _WordWrap; }
            set
            {
                _WordWrap = value;
                if (iTalkRTB != null)
                {
                    iTalkRTB.WordWrap = value;
                }
            }
        }
        public bool AutoWordSelection
        {
            get { return _AutoWordSelection; }
            set
            {
                _AutoWordSelection = value;
                if (iTalkRTB != null)
                {
                    iTalkRTB.AutoWordSelection = value;
                }
            }
        }
        #endregion
        #region EventArgs
     
        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            iTalkRTB.Text = Text;
            Invalidate();
        }
     
        protected override void OnForeColorChanged(System.EventArgs e)
        {
            base.OnForeColorChanged(e);
            iTalkRTB.ForeColor = ForeColor;
            Invalidate();
        }
     
        protected override void OnFontChanged(System.EventArgs e)
        {
            base.OnFontChanged(e);
            iTalkRTB.Font = Font;
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }
     
        protected override void OnSizeChanged(System.EventArgs e)
        {
            base.OnSizeChanged(e);
            iTalkRTB.Size = new Size(Width - 13, Height - 11);
        }
     
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
     
            Shape = new GraphicsPath();
            var _with1 = Shape;
            _with1.AddArc(0, 0, 10, 10, 180, 90);
            _with1.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _with1.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _with1.AddArc(0, Height - 11, 10, 10, 90, 90);
            _with1.CloseAllFigures();
        }
     
        #endregion
     
        public void AddRichTextBox()
        {
            var _with2 = iTalkRTB;
            _with2.BackColor = Color.White;
            _with2.Size = new Size(Width - 10, 100);
            _with2.Location = new Point(7, 5);
            _with2.Text = string.Empty;
            _with2.BorderStyle = BorderStyle.None;
            _with2.Font = new Font("Tahoma", 10);
            _with2.Multiline = true;
        }
     
        public iTalk_RichTextBox()
            : base()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
     
            AddRichTextBox();
            Controls.Add(iTalkRTB);
            BackColor = Color.Transparent;
            ForeColor = Color.DimGray;
     
            Text = null;
            Font = new Font("Tahoma", 10);
            Size = new Size(150, 100);
            WordWrap = true;
            AutoWordSelection = false;
            DoubleBuffered = true;
        }
     
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(this.Width, this.Height);
            Graphics G = Graphics.FromImage(B);
            G.SmoothingMode = SmoothingMode.AntiAlias;
            G.Clear(Color.Transparent);
            G.FillPath(Brushes.White, this.Shape);
            G.DrawPath(new Pen(Color.FromArgb(180, 180, 180)), this.Shape);
            G.Dispose();
            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            B.Dispose();
        }
    }
     
    #endregion
    #region  NumericUpDown
     
    public class iTalk_NumericUpDown : Control
    {
     
        #region  Enums
     
        public enum _TextAlignment
        {
            Near,
            Center
        }
     
        #endregion
        #region  Variables
     
        private GraphicsPath Shape;
        private Pen P1;
        private SolidBrush B1;
     
        private long _Value;
        private long _Minimum;
        private long _Maximum;
        private int Xval;
        private int Yval;
        private bool KeyboardNum;
        private _TextAlignment MyStringAlignment;
     
        #endregion
        #region  Properties
     
        public long Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (value <= _Maximum & value >= _Minimum)
                {
                    _Value = value;
                }
                Invalidate();
            }
        }
     
        public long Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {
                if (value < _Maximum)
                {
                    _Minimum = value;
                }
                if (_Value < _Minimum)
                {
                    _Value = Minimum;
                }
                Invalidate();
            }
        }
     
        public long Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                if (value > _Minimum)
                {
                    _Maximum = value;
                }
                if (_Value > _Maximum)
                {
                    _Value = _Maximum;
                }
                Invalidate();
            }
        }
     
        public _TextAlignment TextAlignment
        {
            get
            {
                return MyStringAlignment;
            }
            set
            {
                MyStringAlignment = value;
                Invalidate();
            }
        }
     
        #endregion
        #region  EventArgs
     
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            Height = 28;
            Shape = new GraphicsPath();
            Shape.AddArc(0, 0, 10, 10, 180, 90);
            Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            Shape.CloseAllFigures();
        }
     
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Xval = e.Location.X;
            Yval = e.Location.Y;
            Invalidate();
     
            if (e.X < Width - 24)
            {
                Cursor = Cursors.IBeam;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }
     
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (Xval > this.Width - 23 && Xval < this.Width - 3)
            {
                if (Yval < 15)
                {
                    if ((Value + 1) <= _Maximum)
                    {
                        _Value++;
                    }
                }
                else
                {
                    if ((Value - 1) >= _Minimum)
                    {
                        _Value--;
                    }
                }
            }
            else
            {
                KeyboardNum = !KeyboardNum;
                Focus();
            }
            Invalidate();
        }
     
        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            try
            {
                if (KeyboardNum == true)
                {
                    _Value = long.Parse((_Value).ToString() + e.KeyChar.ToString().ToString());
                }
                if (_Value > _Maximum)
                {
                    _Value = _Maximum;
                }
            }
            catch (Exception)
            {
            }
        }
     
        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Back)
            {
                string TemporaryValue = _Value.ToString();
                TemporaryValue = TemporaryValue.Remove(Convert.ToInt32(TemporaryValue.Length - 1));
                if (TemporaryValue.Length == 0)
                {
                    TemporaryValue = "0";
                }
                _Value = Convert.ToInt32(TemporaryValue);
            }
            Invalidate();
        }
     
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
            {
                if ((Value + 1) <= _Maximum)
                {
                    _Value++;
                }
                Invalidate();
            }
            else
            {
                if ((Value - 1) >= _Minimum)
                {
                    _Value--;
                }
                Invalidate();
            }
        }
     
        #endregion
     
        public iTalk_NumericUpDown()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
     
            P1 = new Pen(Color.FromArgb(180, 180, 180)); // P1 = Border color
            B1 = new SolidBrush(Color.White); // B1 = Rect Background color
            BackColor = Color.Transparent;
            ForeColor = Color.DimGray;
     
            _Minimum = 0;
            _Maximum = 100;
     
            Font = new Font("Tahoma", 11);
            Size = new Size(70, 28);
            MinimumSize = new Size(62, 28);
            DoubleBuffered = true;
        }
     
        public void Increment(int Value)
        {
            this._Value += Value;
            Invalidate();
        }
     
        public void Decrement(int Value)
        {
            this._Value -= Value;
            Invalidate();
        }
     
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {      
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);
     
            G.SmoothingMode = SmoothingMode.AntiAlias;
     
            G.Clear(Color.Transparent); // Set control background color
            G.FillPath(B1, Shape); // Draw background
            G.DrawPath(P1, Shape); // Draw border
     
            LinearGradientBrush ColorGradient = new LinearGradientBrush(new Rectangle(Width - 23, 4, 19, 19), Color.FromArgb(241, 241, 241), Color.FromArgb(241, 241, 241), 90.0F);
            G.FillRectangle(ColorGradient, ColorGradient.Rectangle); // Fills the body of the rectangle
     
            G.DrawRectangle(new Pen(Color.FromArgb(252, 252, 252)), new Rectangle(Width - 22, 5, 17, 17));
            G.DrawRectangle(new Pen(Color.FromArgb(180, 180, 180)), new Rectangle(Width - 23, 4, 19, 19));
     
            G.DrawLine(new Pen(Color.FromArgb(250, 252, 250)), new Point(Width - 22, Height - 16), new Point(Width - 5, Height - 16));
            G.DrawLine(new Pen(Color.FromArgb(180, 180, 180)), new Point(Width - 22, Height - 15), new Point(Width - 5, Height - 15));
            G.DrawLine(new Pen(Color.FromArgb(250, 250, 250)), new Point(Width - 22, Height - 14), new Point(Width - 5, Height - 14));
     
            G.DrawString("+", new Font("Tahoma", 8), Brushes.Gray, Width - 19, Height - 26);
            G.DrawString("-", new Font("Tahoma", 12), Brushes.Gray, Width - 19, Height - 20);
     
            switch (MyStringAlignment)
            {
                case _TextAlignment.Near:
                    G.DrawString(System.Convert.ToString(Value), Font, new SolidBrush(ForeColor), new Rectangle(5, 0, Width - 1, Height - 1), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    break;
                case _TextAlignment.Center:
                    G.DrawString(System.Convert.ToString(Value), Font, new SolidBrush(ForeColor), new Rectangle(0, 0, Width - 1, Height - 1), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    break;
            }
     
            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }
     
    #endregion
    #region Left Chat Bubble
     
    public class iTalk_ChatBubble_L : Control
    {
     
        #region Variables
     
        private GraphicsPath Shape;
        private Color _TextColor = Color.FromArgb(52, 52, 52);
        private Color _BubbleColor = Color.FromArgb(217, 217, 217);
        private bool _DrawBubbleArrow = true;
     
        #endregion
        #region Properties
     
        public override Color ForeColor
        {
            get { return this._TextColor; }
            set
            {
                this._TextColor = value;
                this.Invalidate();
            }
        }
     
        public Color BubbleColor
        {
            get { return this._BubbleColor; }
            set
            {
                this._BubbleColor = value;
                this.Invalidate();
            }
        }
     
        public bool DrawBubbleArrow
        {
            get { return _DrawBubbleArrow; }
            set
            {
                _DrawBubbleArrow = value;
                Invalidate();
            }
        }
     
        #endregion
     
        public iTalk_ChatBubble_L()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            Size = new Size(152, 38);
            BackColor = Color.Transparent;
            ForeColor = Color.FromArgb(52, 52, 52);
            Font = new Font("Segoe UI", 10);
        }
     
        protected override void OnResize(System.EventArgs e)
        {
            Shape = new GraphicsPath();
     
            var _Shape = Shape;
            _Shape.AddArc(9, 0, 10, 10, 180, 90);
            _Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _Shape.AddArc(9, Height - 11, 10, 10, 90, 90);
            _Shape.CloseAllFigures();
     
            Invalidate();
            base.OnResize(e);
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(this.Width, this.Height);
            Graphics G = Graphics.FromImage(B);
            var _G = G;
            _G.SmoothingMode = SmoothingMode.HighQuality;
            _G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _G.Clear(BackColor);
     
                    // Fill the body of the bubble with the specified color
            _G.FillPath(new SolidBrush(_BubbleColor), Shape);
            // Draw the string specified in 'Text' property
            _G.DrawString(Text, Font, new SolidBrush(ForeColor), new Rectangle(15, 4, Width - 17, Height - 5));
           
           // Draw a polygon on the right side of the bubble
            if (_DrawBubbleArrow == true) {
                Point[] p = {
                                new Point(9, Height - 19),
                                new Point(0, Height - 25),
                                new Point(9, Height - 30)
                            };
                _G.FillPolygon(new SolidBrush(_BubbleColor), p);
                _G.DrawPolygon(new Pen(new SolidBrush(_BubbleColor)), p);
            }
                G.Dispose();
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawImageUnscaled(B, 0, 0);
                B.Dispose();
            }
        }
     
    #endregion
    #region Right Chat Bubble
     
    public class iTalk_ChatBubble_R : Control
    {
     
        #region Variables
     
        private GraphicsPath Shape;
        private Color _TextColor = Color.FromArgb(52, 52, 52);
        private Color _BubbleColor = Color.FromArgb(192, 206, 215);
        private bool _DrawBubbleArrow = true;
     
        #endregion
        #region Properties
     
        public override Color ForeColor
        {
            get { return this._TextColor; }
            set
            {
                this._TextColor = value;
                this.Invalidate();
            }
        }
     
        public Color BubbleColor
        {
            get { return this._BubbleColor; }
            set
            {
                this._BubbleColor = value;
                this.Invalidate();
            }
        }
     
        public bool DrawBubbleArrow
        {
            get { return _DrawBubbleArrow; }
            set
            {
                _DrawBubbleArrow = value;
                Invalidate();
            }
        }
     
        #endregion
     
        public iTalk_ChatBubble_R()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            Size = new Size(152, 38);
            BackColor = Color.Transparent;
            ForeColor = Color.FromArgb(52, 52, 52);
            Font = new Font("Segoe UI", 10);
        }
     
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            Shape = new GraphicsPath();
     
            var _with1 = Shape;
            _with1.AddArc(0, 0, 10, 10, 180, 90);
            _with1.AddArc(Width - 18, 0, 10, 10, -90, 90);
            _with1.AddArc(Width - 18, Height - 11, 10, 10, 0, 90);
            _with1.AddArc(0, Height - 11, 10, 10, 90, 90);
            _with1.CloseAllFigures();
     
            Invalidate();
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(this.Width, this.Height);
            Graphics G = Graphics.FromImage(B);
     
            var _G = G;
            _G.SmoothingMode = SmoothingMode.HighQuality;
            _G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _G.Clear(BackColor);
     
                    // Fill the body of the bubble with the specified color
            _G.FillPath(new SolidBrush(_BubbleColor), Shape);
            // Draw the string specified in 'Text' property
            _G.DrawString(Text, Font, new SolidBrush(ForeColor), (new Rectangle(6, 4, Width - 15, Height)));
           
            // Draw a polygon on the right side of the bubble
            if (_DrawBubbleArrow == true)
            {
                Point[] p = {
                new Point(Width - 8, Height - 19),
                new Point(Width, Height - 25),
                new Point(Width - 8, Height - 30)
            };
                _G.FillPolygon(new SolidBrush(_BubbleColor), p);
                _G.DrawPolygon(new Pen(new SolidBrush(_BubbleColor)), p);
            }
     
            G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(B, 0, 0);
            B.Dispose();
        }
    }
     
    #endregion
    #region Separator
     
    public class iTalk_Separator : Control
    {
     
        public iTalk_Separator()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            this.Size = new Size(120, 10);
        }
     
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(184, 183, 188)), 0, 5, Width, 5);
        }
    }
     
    #endregion
    #region Panel
     
    class iTalk_Panel : ContainerControl
    {
     
     
        private GraphicsPath Shape;
        public iTalk_Panel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
     
            BackColor = Color.Transparent;
            this.Size = new Size(187, 117);
            Padding = new Padding(5, 5, 5, 5);
            DoubleBuffered = true;
        }
     
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
     
            Shape = new GraphicsPath();
            var _with1 = Shape;
            _with1.AddArc(0, 0, 10, 10, 180, 90);
            _with1.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _with1.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _with1.AddArc(0, Height - 11, 10, 10, 90, 90);
            _with1.CloseAllFigures();
        }
     
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);
     
            G.SmoothingMode = SmoothingMode.HighQuality;
     
            G.Clear(Color.Transparent);
            G.FillPath(Brushes.White, Shape); // Draw RTB background
            G.DrawPath(new Pen(Color.FromArgb(180, 180, 180)), Shape); // Draw border
           
            G.Dispose();
            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            B.Dispose();
        }
    }
     
    #endregion
    #region GroupBox
     
    public class iTalk_GroupBox : ContainerControl
    {
     
        public iTalk_GroupBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            DoubleBuffered = true;
            this.Size = new Size(212, 104);
            this.MinimumSize = new Size(136, 50);
            this.Padding = new Padding(5, 28, 5, 5);
        }
     
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);
            Rectangle TitleBox = new Rectangle(51, 3, Width - 103, 18);
            Rectangle box = new Rectangle(0, 0, Width - 1, Height - 10);
     
            G.Clear(Color.Transparent);
            G.SmoothingMode = SmoothingMode.HighQuality;
     
            // Draw the body of the GroupBox
            G.FillPath(Brushes.White, RoundRectangle.RoundRect(new Rectangle(1, 12, Width - 3, box.Height - 1), 8));
            // Draw the border of the GroupBox
            G.DrawPath(new Pen(Color.FromArgb(159, 159, 161)), RoundRectangle.RoundRect(new Rectangle(1, 12, Width - 3, Height - 13), 8));
     
            // Draw the background of the title box
            G.FillPath(Brushes.White, RoundRectangle.RoundRect(TitleBox, 1));
            // Draw the border of the title box
            G.DrawPath(new Pen(Color.FromArgb(182, 180, 186)), RoundRectangle.RoundRect(TitleBox, 4));
            // Draw the specified string from 'Text' property inside the title box
            G.DrawString(Text, new Font("Tahoma", 9, FontStyle.Regular), new SolidBrush(Color.FromArgb(53, 53, 53)), TitleBox, new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            });
     
            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }
     
    #endregion
    #region CheckBox
     
    [DefaultEvent("CheckedChanged")]
    class iTalk_CheckBox : Control
    {
     
        #region Variables
     
        private GraphicsPath Shape;
        private LinearGradientBrush GB;
        private Rectangle R1;
        private Rectangle R2;
        private bool _Checked;
        public event CheckedChangedEventHandler CheckedChanged;
        public delegate void CheckedChangedEventHandler(object sender);
     
        #endregion
        #region Properties
     
        public bool Checked
        {
            get { return _Checked; }
            set
            {
                _Checked = value;
                if (CheckedChanged != null)
                {
                    CheckedChanged(this);
                }
                Invalidate();
            }
        }
     
        #endregion
     
        public iTalk_CheckBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
     
            BackColor = Color.Transparent;
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 10);
            Size = new Size(120, 26);
        }
     
        protected override void OnClick(EventArgs e)
        {
            _Checked = !_Checked;
            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }
            Invalidate();
            base.OnClick(e);
        }
     
        protected override void OnTextChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }
     
        protected override void OnResize(System.EventArgs e)
        {
            if (Width > 0 && Height > 0)
            {
                Shape = new GraphicsPath();
     
                R1 = new Rectangle(17, 0, Width, Height + 1);
                R2 = new Rectangle(0, 0, Width, Height);
                GB = new LinearGradientBrush(new Rectangle(0, 0, 25, 25), Color.FromArgb(250, 250, 250), Color.FromArgb(240, 240, 240), 90);
     
                var _Shape = Shape;
                _Shape.AddArc(0, 0, 7, 7, 180, 90);
                _Shape.AddArc(7, 0, 7, 7, -90, 90);
                _Shape.AddArc(7, 7, 7, 7, 0, 90);
                _Shape.AddArc(0, 7, 7, 7, 90, 90);
                _Shape.CloseAllFigures();
                Height = 15;
            }
     
            Invalidate();
            base.OnResize(e);
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
     
            var _G = e.Graphics;
            _G.Clear(Color.FromArgb(246, 246, 246));
            _G.SmoothingMode = SmoothingMode.AntiAlias;
            // Fill the body of the CheckBox
            _G.FillPath(GB, Shape);
            // Draw the border
            _G.DrawPath(new Pen(Color.FromArgb(160, 160, 160)), Shape);
            // Draw the string
            _G.DrawString(Text, Font, new SolidBrush(Color.FromArgb(142, 142, 142)), R1, new StringFormat { LineAlignment = StringAlignment.Center });
     
            if (Checked)
            {
                _G.DrawString("ü", new Font("Wingdings", 14), new SolidBrush(Color.FromArgb(142, 142, 142)), new Rectangle(-2, 1, Width, Height), new StringFormat { LineAlignment = StringAlignment.Center });
            }
            e.Dispose();
        }
    }
     
    #endregion
    #region RadioButton
     
    [DefaultEvent("CheckedChanged")]
    class iTalk_RadioButton : Control
    {
     
        #region Enums
     
        public enum MouseState : byte
        {
            None = 0,
            Over = 1,
            Down = 2,
            Block = 3
        }
     
        #endregion
        #region Variables
     
        private bool _Checked;
        public event CheckedChangedEventHandler CheckedChanged;
        public delegate void CheckedChangedEventHandler(object sender);
     
        #endregion
        #region Properties
     
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
     
        #endregion
        #region EventArgs
     
        protected override void OnTextChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }
     
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 15;
        }
     
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (!_Checked)
                Checked = true;
            base.OnMouseDown(e);
        }
     
        #endregion
     
        public iTalk_RadioButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
            Font = new Font("Segoe UI", 10);
            Width = 132;
        }
     
        private void InvalidateControls()
        {
            if (!IsHandleCreated || !_Checked)
                return;
     
            foreach (Control _Control in Parent.Controls)
            {
                if (!object.ReferenceEquals(_Control, this) && _Control is iTalk_RadioButton)
                {
                    ((iTalk_RadioButton)_Control).Checked = false;
                }
            }
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var _G = e.Graphics;
     
            _G.Clear(Color.FromArgb(246, 246, 246));
            _G.SmoothingMode = SmoothingMode.AntiAlias;
                   
            LinearGradientBrush LGB = new LinearGradientBrush(new Rectangle(new Point(0, 0), new Size(14, 14)), Color.FromArgb(250, 250, 250), Color.FromArgb(240, 240, 240), 90);
            _G.FillEllipse(LGB, new Rectangle(new Point(0, 0), new Size(14, 14)));
     
            GraphicsPath GP = new GraphicsPath();
            GP.AddEllipse(new Rectangle(0, 0, 14, 14));
            _G.SetClip(GP);
            _G.ResetClip();
     
            // Draw ellipse border
            _G.DrawEllipse(new Pen(Color.FromArgb(160, 160, 160)), new Rectangle(new Point(0, 0), new Size(14, 14)));
     
            // Draw an ellipse inside the body
            if (_Checked)
            {
                SolidBrush EllipseColor = new SolidBrush(Color.FromArgb(142, 142, 142));
                _G.FillEllipse(EllipseColor, new Rectangle(new Point(4, 4), new Size(6, 6)));
            }
            // Draw the string specified in 'Text' property
            _G.DrawString(Text, Font, new SolidBrush(Color.FromArgb(142, 142, 142)), 16, 8, new StringFormat { LineAlignment = StringAlignment.Center });
                   
            e.Dispose();
        }
    }
     
    #endregion
    #region Notification Number
     
    class iTalk_NotificationNumber : Control
    {
        #region Variables
     
        private int _Value = 0;
        private int _Maximum = 99;
     
        #endregion
        #region Properties
     
        public int Value
        {
            get
            {
                if (this._Value == 0)
                {
                    return 0;
                }
                return this._Value;
            }
            set
            {
                if (value > this._Maximum)
                {
                    value = this._Maximum;
                }
                this._Value = value;
                this.Invalidate();
            }
        }
     
        public int Maximum
        {
            get
            {
                return this._Maximum;
            }
            set
            {
                if (value < this._Value)
                {
                    this._Value = value;
                }
                this._Maximum = value;
                this.Invalidate();
            }
        }
     
     
     
        #endregion
     
        public iTalk_NotificationNumber()
        {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
     
        Text = null;
        DoubleBuffered = true;
        }
     
        protected override void OnResize(EventArgs e)
        {
        base.OnResize(e);
            Height = 20;
            Width = 20;
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var _G = e.Graphics;
            string myString = _Value.ToString();
            _G.Clear(BackColor);
            _G.SmoothingMode = SmoothingMode.AntiAlias;
            LinearGradientBrush LGB = new LinearGradientBrush(new Rectangle(new Point(0, 0), new Size(18, 20)), Color.FromArgb(197, 69, 68), Color.FromArgb(176, 52, 52), 90f);
     
                    // Fills the body with LGB gradient
            _G.FillEllipse(LGB, new Rectangle(new Point(0, 0), new Size(18, 18)));
            // Draw border
            _G.DrawEllipse(new Pen(Color.FromArgb(205, 70, 66)), new Rectangle(new Point(0, 0), new Size(18, 18)));
            _G.DrawString(myString, new Font("Segoe UI", 8, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 253)), new Rectangle(0, 0, Width - 2, Height), new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            });
            e.Dispose();
        }
     
    }
     
    #endregion
    #region ListView
     
    class iTalk_Listview : ListView
    {
     
        [DllImport("uxtheme", CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string textSubAppName, string textSubIdList);
     
        public iTalk_Listview()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
            HeaderStyle = ColumnHeaderStyle.Nonclickable;
            BorderStyle = System.Windows.Forms.BorderStyle.None;
        }
     
        protected override void OnHandleCreated(EventArgs e)
        {
            iTalk_Listview.SetWindowTheme(this.Handle, "explorer", null);
            base.OnHandleCreated(e);
        }
    }
     
    #endregion
    #region ComboBox
     
    class iTalk_ComboBox : ComboBox
    {
     
        #region Variables
     
        private int _StartIndex = 0;
        private Color _HoverSelectionColor = Color.FromArgb(241, 241, 241);
     
        #endregion
        #region Custom Properties
     
        public int StartIndex
        {
            get { return _StartIndex; }
            set
            {
                _StartIndex = value;
                try
                {
                    base.SelectedIndex = value;
                }
                catch
                {
                }
                Invalidate();
            }
        }
     
        public Color HoverSelectionColor
        {
            get { return _HoverSelectionColor; }
            set
            {
                _HoverSelectionColor = value;
                Invalidate();
            }
        }
     
        #endregion
        #region EventArgs
     
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(_HoverSelectionColor), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            }
     
            if (!(e.Index == -1))
            {
                e.Graphics.DrawString(GetItemText(Items[e.Index]), e.Font, Brushes.DimGray, e.Bounds);
            }
        }
     
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            SuspendLayout();
            Update();
            ResumeLayout();
        }
     
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }
     
        #endregion
     
        public iTalk_ComboBox()
        {
            SetStyle((ControlStyles)139286, true);
            SetStyle(ControlStyles.Selectable, false);
     
            DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
     
            BackColor = Color.FromArgb(246, 246, 246);
            ForeColor = Color.FromArgb(142, 142, 142);
            Size = new Size(135, 26);
            ItemHeight = 20;
            DropDownHeight = 100;
            Font = new Font("Segoe UI", 10, FontStyle.Regular);
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            LinearGradientBrush LGB = default(LinearGradientBrush);
            GraphicsPath GP = default(GraphicsPath);
     
            e.Graphics.Clear(BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
     
            // Create a curvy border
            GP = RoundRectangle.RoundRect(0, 0, Width - 1, Height - 1, 5);
            // Fills the body of the rectangle with a gradient
            LGB = new LinearGradientBrush(ClientRectangle, Color.FromArgb(241, 241, 241), Color.FromArgb(241, 241, 241), 90f);
     
            e.Graphics.SetClip(GP);
            e.Graphics.FillRectangle(LGB, ClientRectangle);
            e.Graphics.ResetClip();
     
            // Draw rectangle border
            e.Graphics.DrawPath(new Pen(Color.FromArgb(204, 204, 204)), GP);
            // Draw string
            e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(142, 142, 142)), new Rectangle(3, 0, Width - 20, Height), new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near
            });
     
            // Draw the dropdown arrow
            e.Graphics.DrawLine(new Pen(Color.FromArgb(160, 160, 160), 2), new Point(Width - 18, 10), new Point(Width - 14, 14));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(160, 160, 160), 2), new Point(Width - 14, 14), new Point(Width - 10, 10));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(160, 160, 160)), new Point(Width - 14, 15), new Point(Width - 14, 14));
                   
            GP.Dispose();
            LGB.Dispose();
        }
    }
     
    #endregion
    #region Circular ProgressBar
     
    public class iTalk_ProgressBar : Control
    {
     
        #region Enums
     
        public enum _ProgressShape
        {
            Round,
            Flat
        }
     
        #endregion
        #region Variables
     
        private long _Value;
        private long _Maximum = 100;
        private Color _ProgressColor1 = Color.FromArgb(92, 92, 92);
        private Color _ProgressColor2 = Color.FromArgb(92, 92, 92);
        private _ProgressShape ProgressShapeVal;
     
        #endregion
        #region Custom Properties
     
        public long Value
        {
            get { return _Value; }
            set
            {
                if (value > _Maximum)
                    value = _Maximum;
                _Value = value;
                Invalidate();
            }
        }
     
        public long Maximum
        {
            get { return _Maximum; }
            set
            {
                if (value < 1)
                    value = 1;
                _Maximum = value;
                Invalidate();
            }
        }
     
        public Color ProgressColor1
        {
            get { return _ProgressColor1; }
            set
            {
                _ProgressColor1 = value;
                Invalidate();
            }
        }
     
        public Color ProgressColor2
        {
            get { return _ProgressColor2; }
            set
            {
                _ProgressColor2 = value;
                Invalidate();
            }
        }
     
        public _ProgressShape ProgressShape
        {
            get { return ProgressShapeVal; }
            set
            {
                ProgressShapeVal = value;
                Invalidate();
            }
        }
     
        #endregion
        #region EventArgs
     
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetStandardSize();
        }
     
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetStandardSize();
        }
     
        protected override void OnPaintBackground(PaintEventArgs p)
        {
            base.OnPaintBackground(p);
        }
     
        #endregion
     
        public iTalk_ProgressBar()
        {
            Size = new Size(130, 130);
            Font = new Font("Segoe UI", 15);
            MinimumSize = new Size(100, 100);
            DoubleBuffered = true;
        }
     
        private void SetStandardSize()
        {
            int _Size = Math.Max(Width, Height);
            Size = new Size(_Size, _Size);
        }
     
        public void Increment(int Val)
        {
            this._Value += Val;
            Invalidate();
        }
     
        public void Decrement(int Val)
        {
            this._Value -= Val;
            Invalidate();
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Bitmap bitmap = new Bitmap(this.Width, this.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.Clear(this.BackColor);
                    using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, this._ProgressColor1, this._ProgressColor2, LinearGradientMode.ForwardDiagonal))
                    {
                        using (Pen pen = new Pen(brush, 14f))
                        {
                            switch (this.ProgressShapeVal)
                            {
                                case _ProgressShape.Round:
                                    pen.StartCap = LineCap.Round;
                                    pen.EndCap = LineCap.Round;
                                    break;
     
                                case _ProgressShape.Flat:
                                    pen.StartCap = LineCap.Flat;
                                    pen.EndCap = LineCap.Flat;
                                    break;
                            }
                            graphics.DrawArc(pen, 0x12, 0x12, (this.Width - 0x23) - 2, (this.Height - 0x23) - 2, -90, (int)Math.Round((double)((360.0 / ((double)this._Maximum)) * this._Value)));
                        }
                    }
                    using (LinearGradientBrush brush2 = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0x34, 0x34, 0x34), Color.FromArgb(0x34, 0x34, 0x34), LinearGradientMode.Vertical))
                    {
                        graphics.FillEllipse(brush2, 0x18, 0x18, (this.Width - 0x30) - 1, (this.Height - 0x30) - 1);
                    }
                    SizeF MS = graphics.MeasureString(Convert.ToString(Convert.ToInt32((100 / _Maximum) * _Value)), Font);
                    graphics.DrawString(Convert.ToString(Convert.ToInt32((100 / _Maximum) * _Value)), Font, Brushes.White, Convert.ToInt32(Width / 2 - MS.Width / 2), Convert.ToInt32(Height / 2 - MS.Height / 2));
                    e.Graphics.DrawImage(bitmap, 0, 0);
                    graphics.Dispose();
                    bitmap.Dispose();
                }
            }
        }
    }
     
    #endregion
    #region Progress Indicator
     
    class iTalk_ProgressIndicator : Control
    {
     
        #region Variables
     
        private readonly SolidBrush BaseColor = new SolidBrush(Color.DarkGray);
        private readonly SolidBrush AnimationColor = new SolidBrush(Color.DimGray);
     
        private readonly Timer AnimationSpeed = new Timer();
        private PointF[] FloatPoint;
        private BufferedGraphics BuffGraphics;
        private int IndicatorIndex;
        private readonly BufferedGraphicsContext GraphicsContext = BufferedGraphicsManager.Current;
     
        #endregion
        #region Custom Properties
     
        public Color P_BaseColor
        {
            get { return BaseColor.Color; }
            set { BaseColor.Color = value; }
        }
     
        public Color P_AnimationColor
        {
            get { return AnimationColor.Color; }
            set { AnimationColor.Color = value; }
        }
     
        public int P_AnimationSpeed
        {
            get { return AnimationSpeed.Interval; }
            set { AnimationSpeed.Interval = value; }
        }
     
        #endregion
        #region EventArgs
     
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetStandardSize();
            UpdateGraphics();
            SetPoints();
        }
     
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            AnimationSpeed.Enabled = this.Enabled;
        }
     
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AnimationSpeed.Tick += AnimationSpeed_Tick;
            AnimationSpeed.Start();
        }
     
        private void AnimationSpeed_Tick(object sender, EventArgs e)
        {
            if (IndicatorIndex.Equals(0))
            {
                IndicatorIndex = FloatPoint.Length - 1;
            }
            else
            {
                IndicatorIndex -= 1;
            }
            this.Invalidate(false);
        }
     
        #endregion
     
        public iTalk_ProgressIndicator()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
     
            Size = new Size(80, 80);
            Text = string.Empty;
            MinimumSize = new Size(80, 80);
            SetPoints();
            AnimationSpeed.Interval = 100;
        }
     
        private void SetStandardSize()
        {
            int _Size = Math.Max(Width, Height);
            Size = new Size(_Size, _Size);
        }
     
        private void SetPoints()
        {
            Stack<PointF> stack = new Stack<PointF>();
            PointF startingFloatPoint = new PointF(((float)this.Width) / 2f, ((float)this.Height) / 2f);
            for (float i = 0f; i < 360f; i += 45f)
            {
                this.SetValue(startingFloatPoint, (int)Math.Round((double)((((double)this.Width) / 2.0) - 15.0)), (double)i);
                PointF endPoint = this.EndPoint;
                endPoint = new PointF(endPoint.X - 7.5f, endPoint.Y - 7.5f);
                stack.Push(endPoint);
            }
            this.FloatPoint = stack.ToArray();
        }
     
        private void UpdateGraphics()
        {
            if ((this.Width > 0) && (this.Height > 0))
            {
                Size size2 = new Size(this.Width + 1, this.Height + 1);
                this.GraphicsContext.MaximumBuffer = size2;
                this.BuffGraphics = this.GraphicsContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
                this.BuffGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.BuffGraphics.Graphics.Clear(this.BackColor);
            int num2 = this.FloatPoint.Length - 1;
            for (int i = 0; i <= num2; i++)
            {
                if (this.IndicatorIndex == i)
                {
                    this.BuffGraphics.Graphics.FillEllipse(this.AnimationColor, this.FloatPoint[i].X, this.FloatPoint[i].Y, 15f, 15f);
                }
                else
                {
                    this.BuffGraphics.Graphics.FillEllipse(this.BaseColor, this.FloatPoint[i].X, this.FloatPoint[i].Y, 15f, 15f);
                }
            }
            this.BuffGraphics.Render(e.Graphics);
        }
     
     
        private double Rise;
        private double Run;
        private PointF _StartingFloatPoint;
     
        private X AssignValues<X>(ref X Run, X Length)
        {
            Run = Length;
            return Length;
        }
     
        private void SetValue(PointF StartingFloatPoint, int Length, double Angle)
        {
            double CircleRadian = Math.PI * Angle / 180.0;
     
            _StartingFloatPoint = StartingFloatPoint;
            Rise = AssignValues(ref Run, Length);
            Rise = Math.Sin(CircleRadian) * Rise;
            Run = Math.Cos(CircleRadian) * Run;
        }
     
        private PointF EndPoint
        {
            get
            {
                float LocationX = Convert.ToSingle(_StartingFloatPoint.Y + Rise);
                float LocationY = Convert.ToSingle(_StartingFloatPoint.X + Run);
     
                return new PointF(LocationY, LocationX);
            }
        }
    }
     
    #endregion
    #region TabControl
     
    class iTalk_TabControl : TabControl
    {
     
        // NOTE: For best quality icons/images on the TabControl; from the associated ImageList, set
        // the image size (24,24) so it can fit in the tab rectangle. However, to ensure a
        // high-quality image drawing, make sure you only add (32,32) images and not (24,24) as
        // determined in the ImageList
     
        // INFO: A free, non-commercial icon list that would fit in perfectly with the TabControl is
        // Wireframe Toolbar Icons by Gentleface. Licensed under Creative Commons Attribution.
        // Check it out from here: http://www.gentleface.com/free_icon_set.html
     
        public iTalk_TabControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);
     
            DoubleBuffered = true;
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(44, 135);
            DrawMode = TabDrawMode.OwnerDrawFixed;
     
            foreach (TabPage Page in this.TabPages)
            {
                Page.BackColor = Color.FromArgb(246, 246, 246);
            }
        }
     
        protected override void CreateHandle()
        {
            base.CreateHandle();
     
            base.DoubleBuffered = true;
            SizeMode = TabSizeMode.Fixed;
            Appearance = TabAppearance.Normal;
            Alignment = TabAlignment.Left;
        }
     
       
     protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control is TabPage)
            {
                IEnumerator enumerator;
                try
                {
                    enumerator = this.Controls.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        TabPage current = (TabPage)enumerator.Current;
                        current = new TabPage();
                    }
                }
                finally
                {
                    e.Control.BackColor = Color.FromArgb(246, 246, 246);
                }
            }
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);
     
            var _Graphics = G;
     
            _Graphics.Clear(Color.FromArgb(246, 246, 246));
            _Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            _Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            _Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
     
            // Draw tab selector background
            _Graphics.FillRectangle(new SolidBrush(Color.FromArgb(54, 57, 64)), new Rectangle(-5, 0, ItemSize.Height + 4, Height));
            // Draw vertical line at the end of the tab selector rectangle
            _Graphics.DrawLine(new Pen(Color.FromArgb(25, 26, 28)), ItemSize.Height - 1, 0, ItemSize.Height - 1, Height);
     
            for (int TabIndex = 0; TabIndex <= TabCount - 1; TabIndex++)
            {
                if (TabIndex == SelectedIndex)
                {
                    Rectangle TabRect = new Rectangle(new Point(GetTabRect(TabIndex).Location.X - 2, GetTabRect(TabIndex).Location.Y - 2), new Size(GetTabRect(TabIndex).Width + 3, GetTabRect(TabIndex).Height - 8));
     
                    // Draw background of the selected tab
                    _Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 36, 38)), TabRect.X, TabRect.Y, TabRect.Width - 4, TabRect.Height + 3);
                    // Draw a tab highlighter on the background of the selected tab
                    Rectangle TabHighlighter = new Rectangle(new Point(GetTabRect(TabIndex).X - 2, GetTabRect(TabIndex).Location.Y - (TabIndex == 0 ? 1 : 1)), new Size(4, GetTabRect(TabIndex).Height - 7));
                    _Graphics.FillRectangle(new SolidBrush(Color.FromArgb(89, 169, 222)), TabHighlighter);
                    // Draw tab text
                    _Graphics.DrawString(TabPages[TabIndex].Text, new Font(Font.FontFamily, Font.Size, FontStyle.Bold), new SolidBrush(Color.FromArgb(254, 255, 255)), new Rectangle(TabRect.Left + 40, TabRect.Top + 12, TabRect.Width - 40, TabRect.Height), new StringFormat { Alignment = StringAlignment.Near });
     
                    if (this.ImageList != null)
                    {
                        int Index = TabPages[TabIndex].ImageIndex;
                        if (!(Index == -1))
                        {
                            _Graphics.DrawImage(ImageList.Images[TabPages[TabIndex].ImageIndex], TabRect.X + 9, TabRect.Y + 6, 24, 24);
                        }
                    }
                }
                else
                {
                    Rectangle TabRect = new Rectangle(new Point(GetTabRect(TabIndex).Location.X - 2, GetTabRect(TabIndex).Location.Y - 2), new Size(GetTabRect(TabIndex).Width + 3, GetTabRect(TabIndex).Height - 8));
                    _Graphics.DrawString(TabPages[TabIndex].Text, new Font(Font.FontFamily, Font.Size, FontStyle.Bold), new SolidBrush(Color.FromArgb(159, 162, 167)), new Rectangle(TabRect.Left + 40, TabRect.Top + 12, TabRect.Width - 40, TabRect.Height), new StringFormat { Alignment = StringAlignment.Near });
     
                    if (this.ImageList != null)
                    {
                        int Index = TabPages[TabIndex].ImageIndex;
                        if (!(Index == -1))
                        {
                            _Graphics.DrawImage(ImageList.Images[TabPages[TabIndex].ImageIndex], TabRect.X + 9, TabRect.Y + 6, 24, 24);
                        }
                    }
     
                }
            }
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }
     
    #endregion
    #region TrackBar
     
    [DefaultEvent("ValueChanged")]
    class iTalk_TrackBar : Control
    {
     
        #region Enums
     
        public enum ValueDivisor
        {
            By1 = 1,
            By10 = 10,
            By100 = 100,
            By1000 = 1000
        }
     
        #endregion
        #region Variables
     
        private GraphicsPath PipeBorder;
        private GraphicsPath TrackBarHandle;
        private Rectangle TrackBarHandleRect;
        private Rectangle ValueRect;
        private LinearGradientBrush VlaueLGB;
        private LinearGradientBrush TrackBarHandleLGB;
        private bool Cap;
     
        private int ValueDrawer;
        private int _Minimum = 0;
        private int _Maximum = 10;
        private int _Value = 0;
        private Color _ValueColour = Color.FromArgb(224, 224, 224);
        private bool _DrawHatch = true;
        private bool _DrawValueString = false;
        private bool _JumpToMouse = false;
        private ValueDivisor DividedValue = ValueDivisor.By1;
     
        #endregion
        #region Custom Properties
     
        public int Minimum
        {
            get { return _Minimum; }
     
            set
            {
                if (value >= _Maximum)
                    value = _Maximum - 10;
                if (_Value < value)
                    _Value = value;
     
                _Minimum = value;
                Invalidate();
            }
        }
     
        public int Maximum
        {
            get { return _Maximum; }
     
            set
            {
                if (value <= _Minimum)
                    value = _Minimum + 10;
                if (_Value > value)
                    _Value = value;
     
                _Maximum = value;
                Invalidate();
            }
        }
     
        public event ValueChangedEventHandler ValueChanged;
        public delegate void ValueChangedEventHandler();
        public int Value
        {
            get { return _Value; }
            set
            {
                if (_Value != value)
                {
                    if (value < _Minimum)
                    {
                        _Value = _Minimum;
                    }
                    else
                    {
                        if (value > _Maximum)
                        {
                            _Value = _Maximum;
                        }
                        else
                        {
                            _Value = value;
                        }
                    }
                    Invalidate();
                    if (ValueChanged != null)
                    {
                        ValueChanged();
                    }
                }
            }
        }
     
        public ValueDivisor ValueDivison
        {
            get
            {
                return this.DividedValue;
            }
            set
            {
                this.DividedValue = value;
                this.Invalidate();
            }
        }
     
        [Browsable(false)]
        public float ValueToSet
        {
            get
            {
                return (float)(((double)this._Value) / ((double)this.DividedValue));
            }
            set
            {
                this.Value = (int)Math.Round((double)(value * ((float)this.DividedValue)));
            }
        }
     
        public Color ValueColour
        {
            get { return _ValueColour; }
            set
            {
                _ValueColour = value;
                Invalidate();
            }
        }
     
        public bool DrawHatch
        {
            get { return _DrawHatch; }
            set
            {
                _DrawHatch = value;
                Invalidate();
            }
        }
     
        public bool DrawValueString
        {
            get { return _DrawValueString; }
            set
            {
                _DrawValueString = value;
                if (_DrawValueString == true)
                {
                    Height = 40;
                }
                else
                {
                    Height = 22;
                }
                Invalidate();
            }
        }
     
        public bool JumpToMouse
        {
            get
            {
                return this._JumpToMouse;
            }
            set
            {
                this._JumpToMouse = value;
            }
        }
     
        #endregion
        #region EventArgs
     
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if ((this.Cap && (e.X > -1)) && (e.X < (this.Width + 1)))
            {
                this.Value = this._Minimum + ((int)Math.Round((double)((this._Maximum - this._Minimum) * (((double)e.X) / ((double)this.Width)))));
            }
        }
     
           protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button == MouseButtons.Left)
        {
            this.ValueDrawer = (int) Math.Round((double) ((((double) (this._Value - this._Minimum)) / ((double) (this._Maximum - this._Minimum))) * (this.Width - 11)));
            this.TrackBarHandleRect = new Rectangle(this.ValueDrawer, 0, 10, 20);
            this.Cap = this.TrackBarHandleRect.Contains(e.Location);
            if (this._JumpToMouse)
            {
                this.Value = this._Minimum + ((int) Math.Round((double) ((this._Maximum - this._Minimum) * (((double) e.X) / ((double) this.Width)))));
            }
        }
    }
     
         protected override void OnMouseUp(MouseEventArgs e)
           {
               base.OnMouseUp(e);
               this.Cap = false;
           }
     
     
        #endregion
     
        public iTalk_TrackBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);
     
            _DrawHatch = true;
            Size = new Size(80, 22);
            MinimumSize = new Size(37, 22);
        }
     
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_DrawValueString == true) {
                Height = 40;
            } else {
                Height = 22;
            }
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;
            HatchBrush Hatch = new HatchBrush(HatchStyle.WideDownwardDiagonal, Color.FromArgb(20, Color.Black), Color.Transparent);
            G.Clear(Parent.BackColor);
            G.SmoothingMode = SmoothingMode.AntiAlias;
            checked
            {
                this.PipeBorder = RoundRectangle.RoundRect(1, 6, this.Width - 3, 8, 3);
                try
                {
                    this.ValueDrawer = (int)Math.Round(unchecked(checked((double)(this._Value - this._Minimum) / (double)(this._Maximum - this._Minimum)) * (double)checked(this.Width - 11)));
                }
                catch (Exception)
                {
                }
                this.TrackBarHandleRect = new Rectangle(this.ValueDrawer, 0, 10, 20);
                G.SetClip(this.PipeBorder);
                this.ValueRect = new Rectangle(1, 7, this.TrackBarHandleRect.X + this.TrackBarHandleRect.Width - 2, 7);
                this.VlaueLGB = new LinearGradientBrush(this.ValueRect, this._ValueColour, this._ValueColour, 90f);
                G.FillRectangle(this.VlaueLGB, this.ValueRect);
     
                if (_DrawHatch == true)
                {
                G.FillRectangle(Hatch, this.ValueRect);
                }
     
                G.ResetClip();
                G.SmoothingMode = SmoothingMode.AntiAlias;
                G.DrawPath(new Pen(Color.FromArgb(180, 180, 180)), this.PipeBorder);
                this.TrackBarHandle = RoundRectangle.RoundRect(this.TrackBarHandleRect, 3);
                this.TrackBarHandleLGB = new LinearGradientBrush(this.ClientRectangle, SystemColors.Control, SystemColors.Control, 90f);
                G.FillPath(this.TrackBarHandleLGB, this.TrackBarHandle);
                G.DrawPath(new Pen(Color.FromArgb(180, 180, 180)), this.TrackBarHandle);
     
                if (_DrawValueString == true)
                {
                    G.DrawString(System.Convert.ToString(ValueToSet), Font, Brushes.Gray, 0, 25);
                }
            }
        }
    }
     
    #endregion
    #region MenuStrip
     
    public class iTalk_MenuStrip : MenuStrip
    {
     
        public iTalk_MenuStrip()
        {
            this.Renderer = new ControlRenderer();
        }
     
        public new ControlRenderer Renderer
        {
            get { return (ControlRenderer)base.Renderer; }
            set { base.Renderer = value; }
        }
     
    }
     
    #endregion
    #region ContextMenuStrip
     
    public class iTalk_ContextMenuStrip : ContextMenuStrip
    {
     
        public iTalk_ContextMenuStrip()
        {
            this.Renderer = new ControlRenderer();
        }
     
        public new ControlRenderer Renderer
        {
            get { return (ControlRenderer)base.Renderer; }
            set { base.Renderer = value; }
        }
    }
     
    #endregion
    #region StatusStrip
     
    public class iTalk_StatusStrip : StatusStrip
    {
     
        public iTalk_StatusStrip()
        {
            this.Renderer = new ControlRenderer();
            SizingGrip = false;
        }
     
        public new ControlRenderer Renderer
        {
            get { return (ControlRenderer)base.Renderer; }
            set { base.Renderer = value; }
        }
    }
     
    #endregion
    #region Info Icon
     
    class iTalk_Icon_Info : Control
    {
        public iTalk_Icon_Info()
        {
            this.ForeColor = Color.DimGray;
            this.BackColor = Color.FromArgb(246, 246, 246);
            this.Size = new Size(33, 33);
            DoubleBuffered = true;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
     
            e.Graphics.FillEllipse(new SolidBrush(Color.Gray), new Rectangle(1, 1, 29, 29));
            e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(246, 246, 246)), new Rectangle(3, 3, 25, 25));
     
            e.Graphics.DrawString("¡", new Font("Segoe UI", 25, FontStyle.Bold), new SolidBrush(Color.Gray), new Rectangle(4, -14, Width, 43), new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near
            });
        }
    }
     
    #endregion
    #region  Tick Icon
     
    class iTalk_Icon_Tick : Control
    {
     
        public iTalk_Icon_Tick()
        {
            this.ForeColor = Color.DimGray;
            this.BackColor = Color.FromArgb(246, 246, 246);
            this.Size = new Size(33, 33);
            DoubleBuffered = true;
        }
     
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
     
            e.Graphics.FillEllipse(new SolidBrush(Color.Gray), new Rectangle(1, 1, 29, 29));
            e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(246, 246, 246)), new Rectangle(3, 3, 25, 25));
     
            e.Graphics.DrawString("ü", new Font("Wingdings", 25, FontStyle.Bold), new SolidBrush(Color.Gray), new Rectangle(0, -3, Width, 43), new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near
            });
        }
    }
     
    #endregion