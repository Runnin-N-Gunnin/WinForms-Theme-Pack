//THIS IS NOT THE FINAL VERSION.

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;

//------------------
//Creator: aeonhack
//Site: elitevs.net
//Created: 9/2/2011
//Changed: 9/2/2011
//Version: 1.0.0
//Theme Base: 1.5.3
//------------------
internal class DroneTheme : ThemeContainer153
{
	public DroneTheme()
	{
		Header = 24;
		TransparencyKey = Color.Fuchsia;
	}

	protected override void ColorHook()
	{

	}

	protected override void PaintHook()
	{
		G.Clear(Color.FromArgb(24, 24, 24));

		DrawGradient(Color.FromArgb(0, 55, 90), Color.FromArgb(0, 70, 128), 11, 8, Width - 22, 17);
		G.FillRectangle(new SolidBrush(Color.FromArgb(0, 55, 90)), 11, 3, Width - 22, 5);

		Pen P = new Pen(Color.FromArgb(13, Color.White));
		G.DrawLine(P, 10, 1, 10, Height);
		G.DrawLine(P, Width - 11, 1, Width - 11, Height);
		G.DrawLine(P, 11, Height - 11, Width - 12, Height - 11);
		G.DrawLine(P, 11, 29, Width - 12, 29);
		G.DrawLine(P, 11, 25, Width - 12, 25);

		G.FillRectangle(new SolidBrush(Color.FromArgb(13, Color.White)), 0, 2, Width, 6);
		G.FillRectangle(new SolidBrush(Color.FromArgb(13, Color.White)), 0, Height - 6, Width, 4);

		G.FillRectangle(new SolidBrush(Color.FromArgb(24, 24, 24)), 11, Height - 6, Width - 22, 4);

		HatchBrush T = new HatchBrush(HatchStyle.Trellis, Color.FromArgb(24, 24, 24), Color.FromArgb(8, 8, 8));
		G.FillRectangle(T, 11, 30, Width - 22, Height - 41);

		DrawText(Brushes.White, HorizontalAlignment.Left, 15, 2);

		DrawBorders(new Pen(Color.FromArgb(58, 58, 58)), 1);
		DrawBorders(Pens.Black);

		P = new Pen(Color.FromArgb(25, Color.White));
		G.DrawLine(P, 11, 3, Width - 12, 3);
		G.DrawLine(P, 12, 2, 12, 7);
		G.DrawLine(P, Width - 13, 2, Width - 13, 7);

		G.DrawLine(Pens.Black, 11, 0, 11, Height);
		G.DrawLine(Pens.Black, Width - 12, 0, Width - 12, Height);

		G.DrawRectangle(Pens.Black, 11, 2, Width - 23, 22);
		G.DrawLine(Pens.Black, 11, Height - 12, Width - 12, Height - 12);
		G.DrawLine(Pens.Black, 11, 30, Width - 12, 30);

		DrawCorners(Color.Fuchsia);
	}

}

//------------------
//Creator: aeonhack
//Site: elitevs.net
//Created: 9/2/2011
//Changed: 9/2/2011
//Version: 1.0.0
//Theme Base: 1.5.3
//------------------
internal class DroneButton : ThemeControl153
{
	protected override void ColorHook()
	{

	}

	protected override void PaintHook()
	{
		DrawBorders(new Pen(Color.FromArgb(32, 32, 32)), 1);
		G.FillRectangle(new SolidBrush(Color.FromArgb(62, 62, 62)), 0, 0, Width, 8);
		DrawBorders(Pens.Black, 2);
		DrawBorders(Pens.Black);

		if (State == MouseState.Over)
		{
			G.FillRectangle(new SolidBrush(Color.FromArgb(0, 55, 90)), 3, 3, Width - 6, Height - 6);
			DrawBorders(new Pen(Color.FromArgb(0, 66, 108)), 3);
		}
		else if (State == MouseState.Down)
		{
			G.FillRectangle(new SolidBrush(Color.FromArgb(0, 44, 72)), 3, 3, Width - 6, Height - 6);
			DrawBorders(new Pen(Color.FromArgb(0, 55, 90)), 3);
		}
		else
		{
			G.FillRectangle(new SolidBrush(Color.FromArgb(24, 24, 24)), 3, 3, Width - 6, Height - 6);
			DrawBorders(new Pen(Color.FromArgb(38, 38, 38)), 3);
		}

		G.FillRectangle(new SolidBrush(Color.FromArgb(13, Color.White)), 3, 3, Width - 6, 8);

		if (State == MouseState.Down)
		{
			DrawText(Brushes.White, HorizontalAlignment.Center, 1, 1);
		}
		else
		{
			DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0);
		}

	}

}

//------------------
//Creator: aeonhack
//Site: elitevs.net
//Created: 9/24/2011
//Changed: 9/24/2011
//Version: 1.0.0
//Theme Base: 1.5.3
//------------------
internal class DroneProgressBar : ThemeControl153
{
	private ColorBlend Blend;


	public DroneProgressBar()
	{
		Blend = new ColorBlend();
		Blend.Colors = new Color[] {Color.FromArgb(0, 55, 90), Color.FromArgb(0, 66, 108), Color.FromArgb(0, 66, 108), Color.FromArgb(0, 55, 90)};
		Blend.Positions = new float[] {0.0F, 0.4F, 0.6F, 1.0F};
	}

	protected override void OnCreation()
	{
		if (!DesignMode)
		{
			System.Threading.Thread T = new System.Threading.Thread(MoveGlow);
			T.IsBackground = true;
			T.Start();
		}
	}

	private float GlowPosition = -1.0F;
	private void MoveGlow()
	{
		while (true)
		{
			GlowPosition += 0.01F;
			if (GlowPosition >= 1.0F)
			{
				GlowPosition = -1.0F;
			}
			Invalidate();
			System.Threading.Thread.Sleep(25);
		}
	}

	private int _Value;
	public int Value
	{
		get
		{
			return _Value;
		}
		set
		{
			if (value > _Maximum)
			{
				value = _Maximum;
			}
			if (value < 0)
			{
				value = 0;
			}

			_Value = value;
			Invalidate();
		}
	}

	private int _Maximum = 100;
	public int Maximum
	{
		get
		{
			return _Maximum;
		}
		set
		{
			if (value < 1)
			{
				value = 1;
			}
			if (_Value > value)
			{
				_Value = value;
			}

			_Maximum = value;
			Invalidate();
		}
	}

	public void Increment(int amount)
	{
		Value += amount;
	}

	protected override void ColorHook()
	{

	}

	private int Progress;
	protected override void PaintHook()
	{
		DrawBorders(new Pen(Color.FromArgb(32, 32, 32)), 1);
		G.FillRectangle(new SolidBrush(Color.FromArgb(50, 50, 50)), 0, 0, Width, 8);

		DrawGradient(Color.FromArgb(8, 8, 8), Color.FromArgb(23, 23, 23), 2, 2, Width - 4, Height - 4, 90.0F);

		Progress = Convert.ToInt32((_Value / (double)_Maximum) * Width);

		if (!(Progress == 0))
		{
			G.SetClip(new Rectangle(3, 3, Progress - 6, Height - 6));
			G.FillRectangle(new SolidBrush(Color.FromArgb(0, 55, 90)), 0, 0, Progress, Height);

			DrawGradient(Blend, Convert.ToInt32(GlowPosition * Progress), 0, Progress, Height, 0.0F);
			DrawBorders(new Pen(Color.FromArgb(15, Color.White)), 3, 3, Progress - 6, Height - 6);

			G.FillRectangle(new SolidBrush(Color.FromArgb(13, Color.White)), 3, 3, Width - 6, 5);

			G.ResetClip();
		}

		DrawBorders(Pens.Black, 2);
		DrawBorders(Pens.Black);
	}
}

//------------------
//Creator: aeonhack
//Site: elitevs.net
//Created: 10/25/2011
//Changed: 10/25/2011
//Version: 1.0.0
//Theme Base: 1.5.3
//------------------
internal class DroneGroupBox : ThemeContainer153
{
	public DroneGroupBox()
	{
		ControlMode = true;
		Header = 26;
	}

	protected override void ColorHook()
	{

	}

	protected override void PaintHook()
	{
		G.Clear(Color.FromArgb(24, 24, 24));

		DrawGradient(Color.FromArgb(0, 55, 90), Color.FromArgb(0, 70, 128), 5, 5, Width - 10, 26);
		G.DrawLine(new Pen(Color.FromArgb(20, Color.White)), 7, 7, Width - 8, 7);

		DrawBorders(Pens.Black, 5, 5, Width - 10, 26, 1);
		DrawBorders(new Pen(Color.FromArgb(36, 36, 36)), 5, 5, Width - 10, 26);

		//???
		DrawBorders(new Pen(Color.FromArgb(8, 8, 8)), 5, 34, Width - 10, Height - 39, 1);
		DrawBorders(new Pen(Color.FromArgb(36, 36, 36)), 5, 34, Width - 10, Height - 39);

		DrawBorders(new Pen(Color.FromArgb(36, 36, 36)), 1);
		DrawBorders(Pens.Black);

		G.DrawLine(new Pen(Color.FromArgb(48, 48, 48)), 1, 1, Width - 2, 1);

		DrawText(Brushes.White, HorizontalAlignment.Left, 9, 5);
	}
}

//------------------
//Creator: aeonhack
//Site: elitevs.net
//Created: 10/25/2011
//Changed: 10/25/2011
//Version: 1.0.0
//Theme Base: 1.5.3
//------------------
internal class DroneSeperator : ThemeControl153
{
	private Orientation _Orientation;
	public Orientation Orientation
	{
		get
		{
			return _Orientation;
		}
		set
		{
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

	public DroneSeperator()
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
		BL1.Positions = new float[] {0.0F, 0.15F, 0.85F, 1.0F};
		BL2.Positions = new float[] {0.0F, 0.15F, 0.5F, 0.85F, 1.0F};

		BL1.Colors = new Color[] {Color.Transparent, Color.Black, Color.Black, Color.Transparent};
		BL2.Colors = new Color[] {Color.Transparent, Color.FromArgb(35, 35, 35), Color.FromArgb(45, 45, 45), Color.FromArgb(35, 35, 35), Color.Transparent};

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