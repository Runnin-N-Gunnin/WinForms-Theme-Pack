// theme base 1.5.2
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class TransparentLabel : Label
{
    public TransparentLabel()
    {
        base.SetStyle(ControlStyles.Opaque, true);
        base.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
        this.ForeColor = Color.FromArgb(0x71, 170, 0xba);
    }

    protected override System.Windows.Forms.CreateParams CreateParams
    {
        get
        {
            System.Windows.Forms.CreateParams createParams = base.CreateParams;
            createParams.ExStyle |= 0x20;
            return createParams;
        }
    }
}

public class SkyDarkTop : Control
{
    private Color C1 = Color.FromArgb(0x5e, 0x67, 0x6a);
    private Color C2 = Color.FromArgb(0x98, 0xb6, 0xc0);
    private Color C3 = Color.FromArgb(0x47, 70, 0x45);
    private Color C4 = Color.FromArgb(0x3a, 0x38, 0x36);
    private Color CD = Color.FromArgb(0x56, 0x5e, 0x60);
    private MouseState ms;

    public SkyDarkTop()
    {
        this.DoubleBuffered = true;
        base.Size = new Size(10, 10);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        this.State = MouseState.Down;
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        this.State = MouseState.Over;
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        this.State = MouseState.None;
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        this.State = MouseState.Over;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap image = new Bitmap(base.Width, base.Height);
        Graphics graphics = Graphics.FromImage(image);
        LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, base.Height), this.C3, this.C4);
        graphics.FillRectangle(brush, base.ClientRectangle);
        brush.Dispose();
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        switch (this.State)
        {
            case MouseState.None:
                graphics.DrawEllipse(new Pen(this.C1, 2f), new Rectangle(2, 2, base.Width - 5, base.Height - 5));
                break;

            case MouseState.Over:
                graphics.DrawEllipse(new Pen(this.C2, 2f), new Rectangle(2, 2, base.Width - 5, base.Height - 5));
                break;

            case MouseState.Down:
                graphics.DrawEllipse(new Pen(this.CD, 2f), new Rectangle(2, 2, base.Width - 5, base.Height - 5));
                break;
        }
        graphics.FillEllipse(ConversionFunctions.ToBrush(this.C2), new Rectangle(5, 5, base.Width - 11, base.Height - 11));
        e.Graphics.DrawImage(image, 0, 0);
        graphics.Dispose();
        image.Dispose();
    }

    private MouseState State
    {
        get
        {
            return this.ms;
        }
        set
        {
            this.ms = value;
            base.Invalidate();
        }
    }

    public enum MouseState
    {
        None,
        Over,
        Down
    }
}

public class SkyDarkTabControl : TabControl
{
    private Color C1 = Color.FromArgb(0x3e, 60, 0x3a);
    private Color C2 = Color.FromArgb(80, 0x4e, 0x4c);
    private Color C3 = Color.FromArgb(0x33, 0x31, 0x2f);

    public SkyDarkTabControl()
    {
        base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        this.DoubleBuffered = true;
    }

    protected override void CreateHandle()
    {
        base.CreateHandle();
        base.Alignment = TabAlignment.Top;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap image = new Bitmap(base.Width, base.Height);
        Graphics graphics = Graphics.FromImage(image);
        try
        {
            base.SelectedTab.BackColor = this.C1;
        }
        catch
        {
        }
        graphics.Clear(base.Parent.BackColor);
        for (int i = 0; i <= (base.TabCount - 1); i++)
        {
            if (i == base.SelectedIndex)
            {
                continue;
            }
            Rectangle rect = new Rectangle(base.GetTabRect(i).X, base.GetTabRect(i).Y + 3, base.GetTabRect(i).Width + 2, base.GetTabRect(i).Height);
            LinearGradientBrush brush = new LinearGradientBrush(new Point(rect.X, rect.Y), new Point(rect.X, rect.Y + rect.Height), Color.FromArgb(60, 0x3b, 0x3a), Color.FromArgb(0x45, 0x45, 70));
            graphics.FillRectangle(brush, rect);
            brush.Dispose();
            graphics.DrawRectangle(ConversionFunctions.ToPen(this.C3), rect);
            graphics.DrawRectangle(ConversionFunctions.ToPen(this.C2), new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height));
            string s = string.Empty;
            switch (i)
            {
                case 0:
                    s = "Main";
                    break;

                case 1:
                    s = "Extra";
                    break;

                case 2:
                    s = "Scan";
                    break;

                case 3:
                    s = "Info";
                    break;
            }
            StringFormat format = new StringFormat {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            graphics.DrawString(s, this.Font, ConversionFunctions.ToBrush(130, 0xb0, 190), rect, format);
        }
        graphics.FillRectangle(ConversionFunctions.ToBrush(this.C1), 0, base.ItemSize.Height, base.Width, base.Height);
        graphics.DrawRectangle(ConversionFunctions.ToPen(this.C2), 0, base.ItemSize.Height, base.Width - 1, (base.Height - base.ItemSize.Height) - 1);
        graphics.DrawRectangle(ConversionFunctions.ToPen(this.C3), 1, base.ItemSize.Height + 1, base.Width - 3, (base.Height - base.ItemSize.Height) - 3);
        if (base.SelectedIndex != -1)
        {
            string str2 = string.Empty;
            if (base.SelectedIndex == 0)
            {
                str2 = "Main";
            }
            else if (base.SelectedIndex == 1)
            {
                str2 = "Extra";
            }
            else if (base.SelectedIndex == 2)
            {
                str2 = "Scan";
            }
            else if (base.SelectedIndex == 3)
            {
                str2 = "Info";
            }
            Rectangle layoutRectangle = new Rectangle(base.GetTabRect(base.SelectedIndex).X - 2, base.GetTabRect(base.SelectedIndex).Y, base.GetTabRect(base.SelectedIndex).Width + 3, base.GetTabRect(base.SelectedIndex).Height);
            graphics.FillRectangle(ConversionFunctions.ToBrush(this.C1), new Rectangle(layoutRectangle.X + 2, layoutRectangle.Y + 2, layoutRectangle.Width - 2, layoutRectangle.Height));
            graphics.DrawLine(ConversionFunctions.ToPen(this.C2), new Point(layoutRectangle.X, (layoutRectangle.Y + layoutRectangle.Height) - 2), new Point(layoutRectangle.X, layoutRectangle.Y));
            graphics.DrawLine(ConversionFunctions.ToPen(this.C2), new Point(layoutRectangle.X, layoutRectangle.Y), new Point(layoutRectangle.X + layoutRectangle.Width, layoutRectangle.Y));
            graphics.DrawLine(ConversionFunctions.ToPen(this.C2), new Point(layoutRectangle.X + layoutRectangle.Width, layoutRectangle.Y), new Point(layoutRectangle.X + layoutRectangle.Width, (layoutRectangle.Y + layoutRectangle.Height) - 2));
            graphics.DrawLine(ConversionFunctions.ToPen(this.C3), new Point(layoutRectangle.X + 1, (layoutRectangle.Y + layoutRectangle.Height) - 1), new Point(layoutRectangle.X + 1, layoutRectangle.Y + 1));
            graphics.DrawLine(ConversionFunctions.ToPen(this.C3), new Point(layoutRectangle.X + 1, layoutRectangle.Y + 1), new Point((layoutRectangle.X + layoutRectangle.Width) - 1, layoutRectangle.Y + 1));
            graphics.DrawLine(ConversionFunctions.ToPen(this.C3), new Point((layoutRectangle.X + layoutRectangle.Width) - 1, layoutRectangle.Y + 1), new Point((layoutRectangle.X + layoutRectangle.Width) - 1, (layoutRectangle.Y + layoutRectangle.Height) - 1));
            StringFormat format2 = new StringFormat {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            graphics.DrawString(str2, this.Font, ConversionFunctions.ToBrush(130, 0xb0, 190), layoutRectangle, format2);
        }
        e.Graphics.DrawImage(image, 0, 0);
        graphics.Dispose();
        image.Dispose();
    }
}

public class SkyDarkSeperator : Control
{
    private Alignment al;
    private Color C1 = Color.FromArgb(0x33, 0x31, 0x2f);
    private Color C2 = Color.FromArgb(90, 0x5b, 90);

    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap image = new Bitmap(base.Width, base.Height);
        Graphics graphics = Graphics.FromImage(image);
        switch (this.Align)
        {
            case Alignment.Vertical:
                graphics.DrawLine(ConversionFunctions.ToPen(this.C1), new Point(base.Width / 2, 0), new Point(base.Width / 2, base.Height));
                graphics.DrawLine(ConversionFunctions.ToPen(this.C2), new Point((base.Width / 2) + 1, 0), new Point((base.Width / 2) + 1, base.Height));
                break;

            case Alignment.Horizontal:
                graphics.DrawLine(ConversionFunctions.ToPen(this.C1), new Point(0, base.Height / 2), new Point(base.Width, base.Height / 2));
                graphics.DrawLine(ConversionFunctions.ToPen(this.C2), new Point(0, (base.Height / 2) + 1), new Point(base.Width, (base.Height / 2) + 1));
                break;
        }
        e.Graphics.DrawImage(image, 0, 0);
        graphics.Dispose();
        image.Dispose();
    }

    public Alignment Align
    {
        get
        {
            return this.al;
        }
        set
        {
            this.al = value;
            base.Invalidate();
        }
    }

    public enum Alignment
    {
        Vertical,
        Horizontal
    }
}

public class SkyDarkRadio : Control
{
    private Color C1 = Color.FromArgb(0x23, 0x23, 0x23);
    private Color C2 = Color.Transparent;
    private Color C3 = Color.Transparent;
    private Color C4 = Color.Transparent;
    private bool chk;
    private MouseState ms;

    public SkyDarkRadio()
    {
        base.Size = new Size(base.Width, 13);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (!base.Enabled)
        {
            this.State = MouseState.Down;
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (base.Enabled)
        {
            this.State = MouseState.None;
            if (!this.Checked)
            {
                this.Checked = true;
            }
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap image = new Bitmap(base.Width, base.Height);
        Graphics graphics = Graphics.FromImage(image);
        graphics.Clear(base.Parent.BackColor);
        switch (base.Enabled)
        {
            case false:
                this.C2 = Color.FromArgb(70, 70, 70);
                this.C3 = Color.FromArgb(0x36, 0x36, 0x33);
                this.C4 = Color.FromArgb(0x59, 0x58, 0x58);
                break;

            case true:
                switch (this.State)
                {
                    case MouseState.None:
                        this.C2 = Color.FromArgb(70, 70, 70);
                        this.C3 = Color.FromArgb(0x36, 0x36, 0x33);
                        this.C4 = Color.FromArgb(0x98, 0xb6, 0xc0);
                        break;

                    case MouseState.Down:
                        this.C2 = Color.FromArgb(0x36, 0x36, 0x33);
                        this.C3 = Color.FromArgb(70, 70, 70);
                        this.C4 = Color.FromArgb(0x70, 0x8e, 0x98);
                        break;
                }
                break;
        }
        Rectangle rect = new Rectangle(0, 0, base.Height - 1, base.Height - 1);
        LinearGradientBrush brush = new LinearGradientBrush(new Point(rect.X + (rect.Width / 2), rect.Y), new Point(rect.X + (rect.Width / 2), rect.Y + rect.Height), this.C2, this.C3);
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.FillEllipse(brush, rect);
        graphics.DrawEllipse(new Pen(ConversionFunctions.ToBrush(this.C1)), rect);
        if (this.Checked)
        {
            graphics.FillEllipse(ConversionFunctions.ToBrush(this.C4), new Rectangle(rect.X + (rect.Width / 4), rect.Y + (rect.Height / 4), rect.Width / 2, rect.Height / 2));
        }
        StringFormat format = new StringFormat {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Near
        };
        graphics.DrawString(base.Text, this.Font, ConversionFunctions.ToBrush(this.C4), new Rectangle((rect.X + rect.Width) + 5, 0, ((base.Width - rect.X) - rect.Width) - 5, base.Height), format);
        e.Graphics.DrawImage(image, 0, 0);
        graphics.Dispose();
        image.Dispose();
    }

    public bool Checked
    {
        get
        {
            return this.chk;
        }
        set
        {
            this.chk = value;
            base.Invalidate();
            try
            {
                if (value)
                {
                    foreach (object obj2 in base.Parent.Controls)
                    {
                        if ((obj2 is SkyDarkRadio) && (!object.ReferenceEquals(obj2, this) & ((SkyDarkRadio) obj2).Enabled))
                        {
                            ((SkyDarkRadio) obj2).Checked = false;
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }

    public MouseState State
    {
        get
        {
            return this.ms;
        }
        set
        {
            this.ms = value;
            base.Invalidate();
        }
    }

    public enum MouseState
    {
        None,
        Down
    }
}
public class SkyDarkProgress : Control
{
    private Color C1 = Color.FromArgb(0x33, 0x31, 0x2f);
    private Color C2 = Color.FromArgb(0x51, 0x4d, 0x4d);
    private Color C3 = Color.FromArgb(0x40, 60, 0x3b);
    private Color C4 = Color.FromArgb(70, 0x47, 70);
    private Color C5 = Color.FromArgb(0x3e, 0x3b, 0x3a);
    private int max = 100;
    private int val;

    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap image = new Bitmap(base.Width, base.Height);
        Graphics graphics = Graphics.FromImage(image);
        Rectangle rect = new Rectangle(3, 3, Convert.ToInt32((int) ((base.Width - 7) * this.val / this.max)), base.Height - 7);
        graphics.Clear(this.C5);
        LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, base.Height), this.C3, this.C4);
        graphics.FillRectangle(brush, rect);
        brush.Dispose();
        graphics.DrawRectangle(ConversionFunctions.ToPen(this.C2), rect);
        graphics.DrawRectangle(ConversionFunctions.ToPen(this.C1), 0, 0, base.Width - 1, base.Height - 1);
        graphics.DrawRectangle(ConversionFunctions.ToPen(this.C2), 1, 1, base.Width - 3, base.Height - 3);
        e.Graphics.DrawImage(image, 0, 0);
        graphics.Dispose();
        image.Dispose();
    }

    public int Maximum
    {
        get
        {
            return this.max;
        }
        set
        {
            if (value < 1)
            {
                this.max = 1;
            }
            else
            {
                this.max = value;
            }
            if (value < this.val)
            {
                this.val = this.max;
            }
            base.Invalidate();
        }
    }

    public int Value
    {
        get
        {
            return this.val;
        }
        set
        {
            if (value > this.max)
            {
                this.val = this.max;
            }
            else if (value < 0)
            {
                this.val = 0;
            }
            else
            {
                this.val = value;
            }
            base.Invalidate();
        }
    }
}

public class SkyDarkForm2 : ContainerControl
{
    private Color C1 = Color.FromArgb(0x3e, 60, 0x3a);
    private Color C2 = Color.FromArgb(0x51, 0x4f, 0x4d);
    private Color C3 = Color.FromArgb(0x47, 70, 0x45);
    private Color C4 = Color.FromArgb(0x3a, 0x38, 0x36);
    private bool Locked;
    private Point Locked1;
    private Rectangle T1;
    private SkyDarkTop withEventsField_Maxim;

    public SkyDarkForm2()
    {
        this.Dock = DockStyle.Fill;
        base.SendToBack();
        this.BackColor = Color.FromArgb(0x3e, 60, 0x3a);
    }

    private void Maxim_Click(object sender, EventArgs e)
    {
        base.ParentForm.WindowState = FormWindowState.Minimized;
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.ParentForm.FormBorderStyle = FormBorderStyle.None;
        SkyDarkTop top = new SkyDarkTop {
            Location = new Point(base.Width - 0x16, 3),
            Size = new Size(0, 0),
            Parent = this
        };
        this.Maxim = top;
        base.Controls.Add(this.Maxim);
        this.Maxim.Show();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        this.T1 = new Rectangle(1, 1, base.Width - 3, 0x12);
        Bitmap image = new Bitmap(base.Width, base.Height);
        Graphics graphics = Graphics.FromImage(image);
        graphics.Clear(this.C1);
        LinearGradientBrush brush = new LinearGradientBrush(new Point(this.T1.X, this.T1.Y), new Point(this.T1.X, this.T1.Y + this.T1.Height), this.C3, this.C4);
        graphics.FillRectangle(brush, this.T1);
        graphics.DrawRectangle(ConversionFunctions.ToPen(this.C2), this.T1);
        graphics.DrawRectangle(ConversionFunctions.ToPen(this.C2), new Rectangle(this.T1.X, (this.T1.Y + this.T1.Height) + 2, this.T1.Width, ((base.Height - this.T1.Y) - this.T1.Height) - 4));
        brush.Dispose();
        StringFormat format = new StringFormat {
            LineAlignment = StringAlignment.Center
        };
        graphics.DrawString(base.Text, this.Font, ConversionFunctions.ToBrush(0x71, 170, 0xba), new Rectangle(new Point(this.T1.X + 4, this.T1.Y), new Size(this.T1.Width - 40, this.T1.Height)), format);
        e.Graphics.DrawImage(image, 0, 0);
        graphics.Dispose();
        image.Dispose();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        this.T1 = new Rectangle(1, 1, base.Width - 3, 0x12);
    }

    public SkyDarkTop Maxim
    {
        get
        {
            return this.withEventsField_Maxim;
        }
        set
        {
            if (this.withEventsField_Maxim != null)
            {
                this.withEventsField_Maxim.Click -= new EventHandler(this.Maxim_Click);
            }
            this.withEventsField_Maxim = value;
            if (this.withEventsField_Maxim != null)
            {
                this.withEventsField_Maxim.Click += new EventHandler(this.Maxim_Click);
            }
        }
    }
}

public class SkyDarkForm : ContainerControl
{
    private Color C1 = Color.FromArgb(0x3e, 60, 0x3a);
    private Color C2 = Color.FromArgb(0x51, 0x4f, 0x4d);
    private Color C3 = Color.FromArgb(0x47, 70, 0x45);
    private Color C4 = Color.FromArgb(0x3a, 0x38, 0x36);
    private bool Locked;
    private Point Locked1;
    private Rectangle T1;
    private SkyDarkTop withEventsField_Exim;
    private SkyDarkTop withEventsField_Maxim;

    public SkyDarkForm()
    {
        this.Dock = DockStyle.Fill;
        base.SendToBack();
        this.BackColor = Color.FromArgb(0x3e, 60, 0x3a);
    }

    private void Exim_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void Maxim_Click(object sender, EventArgs e)
    {
        base.ParentForm.WindowState = FormWindowState.Minimized;
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.ParentForm.FormBorderStyle = FormBorderStyle.None;
        SkyDarkTop top = new SkyDarkTop {
            Location = new Point(base.Width - 0x29, 3),
            Size = new Size(14, 14),
            Parent = this
        };
        this.Maxim = top;
        SkyDarkTop top2 = new SkyDarkTop {
            Location = new Point(base.Width - 0x16, 3),
            Size = new Size(14, 14),
            Parent = this
        };
        this.Exim = top2;
        base.Controls.Add(this.Maxim);
        base.Controls.Add(this.Exim);
        this.Maxim.Show();
        this.Exim.Show();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        this.T1 = new Rectangle(1, 1, base.Width - 3, 0x12);
        Bitmap image = new Bitmap(base.Width, base.Height);
        Graphics graphics = Graphics.FromImage(image);
        graphics.Clear(this.C1);
        LinearGradientBrush brush = new LinearGradientBrush(new Point(this.T1.X, this.T1.Y), new Point(this.T1.X, this.T1.Y + this.T1.Height), this.C3, this.C4);
        graphics.FillRectangle(brush, this.T1);
        graphics.DrawRectangle(ConversionFunctions.ToPen(this.C2), this.T1);
        graphics.DrawRectangle(ConversionFunctions.ToPen(this.C2), new Rectangle(this.T1.X, (this.T1.Y + this.T1.Height) + 2, this.T1.Width, ((base.Height - this.T1.Y) - this.T1.Height) - 4));
        brush.Dispose();
        StringFormat format = new StringFormat {
            LineAlignment = StringAlignment.Center
        };
        graphics.DrawString(base.Text, this.Font, ConversionFunctions.ToBrush(0x71, 170, 0xba), new Rectangle(new Point(this.T1.X + 4, this.T1.Y), new Size(this.T1.Width - 40, this.T1.Height)), format);
        e.Graphics.DrawImage(image, 0, 0);
        graphics.Dispose();
        image.Dispose();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        this.T1 = new Rectangle(1, 1, base.Width - 3, 0x12);
    }

    public SkyDarkTop Exim
    {
        get
        {
            return this.withEventsField_Exim;
        }
        set
        {
            if (this.withEventsField_Exim != null)
            {
                this.withEventsField_Exim.Click -= new EventHandler(this.Exim_Click);
            }
            this.withEventsField_Exim = value;
            if (this.withEventsField_Exim != null)
            {
                this.withEventsField_Exim.Click += new EventHandler(this.Exim_Click);
            }
        }
    }

    public SkyDarkTop Maxim
    {
        get
        {
            return this.withEventsField_Maxim;
        }
        set
        {
            if (this.withEventsField_Maxim != null)
            {
                this.withEventsField_Maxim.Click -= new EventHandler(this.Maxim_Click);
            }
            this.withEventsField_Maxim = value;
            if (this.withEventsField_Maxim != null)
            {
                this.withEventsField_Maxim.Click += new EventHandler(this.Maxim_Click);
            }
        }
    }
}
public class SkyDarkCheck : Control
{
    private Color C1 = Color.FromArgb(0x33, 0x31, 0x2f);
    private Color C2 = Color.FromArgb(80, 0x4d, 0x4d);
    private Color C3 = Color.FromArgb(70, 0x45, 0x44);
    private Color C4 = Color.FromArgb(0x40, 60, 0x3b);
    private Color C5 = Color.Transparent;
    private bool chk;
    private MouseState ms;

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (base.Enabled)
        {
            this.State = MouseState.Down;
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (base.Enabled)
        {
            this.State = MouseState.None;
            this.Checked = !this.Checked;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap image = new Bitmap(base.Width - 5, base.Height - 5);
        Graphics graphics = Graphics.FromImage(image);
        graphics.Clear(base.Parent.BackColor);
        this.C3 = Color.FromArgb(70, 0x45, 0x44);
        this.C4 = Color.FromArgb(0x40, 60, 0x3b);
        switch (base.Enabled)
        {
            case false:
                this.C5 = Color.FromArgb(0x58, 0x58, 0x58);
                break;

            case true:
                switch (this.State)
                {
                    case MouseState.None:
                        this.C5 = Color.FromArgb(0x97, 0xb5, 190);
                        break;

                    case MouseState.Down:
                        this.C5 = Color.FromArgb(0x79, 0x97, 160);
                        this.C3 = Color.FromArgb(0x40, 60, 0x3b);
                        this.C4 = Color.FromArgb(70, 0x45, 0x44);
                        break;
                }
                break;
        }
        Rectangle rect = new Rectangle(0, 0, base.Height - 6, base.Height - 6);
        LinearGradientBrush brush = new LinearGradientBrush(new Point(rect.X, rect.Y), new Point(rect.X, rect.Y + rect.Height), this.C3, this.C4);
        graphics.FillRectangle(brush, rect);
        brush.Dispose();
        graphics.DrawRectangle(ConversionFunctions.ToPen(this.C1), rect);
        graphics.DrawRectangle(ConversionFunctions.ToPen(this.C2), new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2));
        Rectangle rectangle2 = new Rectangle(rect.X + (rect.Width / 4), rect.Y + (rect.Height / 4), rect.Width / 2, rect.Height / 2);
        Point[] pointArray = new Point[] { new Point(rectangle2.X, rectangle2.Y + (rectangle2.Height / 2)), new Point(rectangle2.X + (rectangle2.Width / 2), rectangle2.Y + rectangle2.Height), new Point(rectangle2.X + rectangle2.Width, rectangle2.Y) };
        if (this.Checked)
        {
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            Pen pen = new Pen(ConversionFunctions.ToBrush(this.C5), 2f);
            for (int i = 0; i <= (pointArray.Length - 2); i++)
            {
                graphics.DrawLine(pen, pointArray[i], pointArray[i + 1]);
            }
        }
        StringFormat format = new StringFormat {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Near
        };
        graphics.DrawString(base.Text, this.Font, ConversionFunctions.ToBrush(this.C5), new Rectangle((rect.X + rect.Width) + 5, 0, ((base.Width - rect.X) - rect.Width) - 5, base.Height), format);
        e.Graphics.DrawImage(image, 0, 0);
        graphics.Dispose();
        image.Dispose();
    }

    public bool Checked
    {
        get
        {
            return this.chk;
        }
        set
        {
            this.chk = value;
            base.Invalidate();
        }
    }

    public MouseState State
    {
        get
        {
            return this.ms;
        }
        set
        {
            this.ms = value;
            base.Invalidate();
        }
    }

    public enum MouseState
    {
        None,
        Down
    }
}
public class SkyDarkButton : Control
{
    private Color C1 = Color.FromArgb(0x33, 0x31, 0x2f);
    private Color C2 = Color.FromArgb(90, 0x5b, 90);
    private Color C3 = Color.FromArgb(70, 0x47, 70);
    private Color C4 = Color.FromArgb(0x3e, 0x3d, 0x3a);
    private MouseState ms;

    public SkyDarkButton()
    {
        this.DoubleBuffered = true;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        this.State = MouseState.Down;
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        this.State = MouseState.Over;
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        this.State = MouseState.None;
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        this.State = MouseState.Over;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap image = new Bitmap(base.Width, base.Height);
        Graphics graphics = Graphics.FromImage(image);
        LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, base.Height), this.C3, this.C4);
        graphics.FillRectangle(brush, 0, 0, base.Width, base.Height);
        brush.Dispose();
        if (base.Enabled)
        {
            switch (this.State)
            {
                case MouseState.Over:
                    graphics.FillRectangle(new SolidBrush(Color.FromArgb(20, Color.White)), new Rectangle(0, 0, base.Width, base.Height));
                    break;

                case MouseState.Down:
                    graphics.FillRectangle(new SolidBrush(Color.FromArgb(10, Color.Black)), new Rectangle(0, 0, base.Width, base.Height));
                    break;
            }
        }
        StringFormat format = new StringFormat {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center
        };
        switch (base.Enabled)
        {
            case false:
                graphics.DrawString(this.Text, this.Font, Brushes.Gray, new Rectangle(0, 0, base.Width - 1, base.Height - 1), format);
                break;

            case true:
                graphics.DrawString(this.Text, this.Font, ConversionFunctions.ToBrush(0x71, 170, 0xba), new Rectangle(0, 0, base.Width - 1, base.Height - 1), format);
                break;
        }
        format.Dispose();
        graphics.DrawRectangle(ConversionFunctions.ToPen(this.C1), 0, 0, base.Width - 1, base.Height - 1);
        graphics.DrawRectangle(ConversionFunctions.ToPen(this.C2), 1, 1, base.Width - 3, base.Height - 3);
        e.Graphics.DrawImage(image, 0, 0);
        graphics.Dispose();
        image.Dispose();
    }

    private MouseState State
    {
        get
        {
            return this.ms;
        }
        set
        {
            this.ms = value;
            base.Invalidate();
        }
    }

    public override string Text
    {
        get
        {
            return base.Text;
        }
        set
        {
            base.Text = value;
            base.Invalidate();
        }
    }

    public enum MouseState
    {
        None,
        Over,
        Down
    }
}
internal static class ConversionFunctions
{
    public static Brush ToBrush(Color Color)
    {
        return new SolidBrush(Color);
    }

    public static Brush ToBrush(Pen Pen)
    {
        return new SolidBrush(Pen.Color);
    }

    public static Brush ToBrush(int A, Color C)
    {
        return new SolidBrush(Color.FromArgb(A, C));
    }

    public static Brush ToBrush(int R, int G, int B)
    {
        return new SolidBrush(Color.FromArgb(R, G, B));
    }

    public static Brush ToBrush(int A, int R, int G, int B)
    {
        return new SolidBrush(Color.FromArgb(A, R, G, B));
    }

    public static Pen ToPen(Color Color)
    {
        return new Pen(new SolidBrush(Color));
    }

    public static Pen ToPen(SolidBrush Brush)
    {
        return new Pen(Brush);
    }

    public static Pen ToPen(int A, Color C)
    {
        return new Pen(new SolidBrush(Color.FromArgb(A, C)));
    }

    public static Pen ToPen(int R, int G, int B)
    {
        return new Pen(new SolidBrush(Color.FromArgb(R, G, B)));
    }

    public static Pen ToPen(int A, int R, int G, int B)
    {
        return new Pen(new SolidBrush(Color.FromArgb(A, R, G, B)));
    }
}