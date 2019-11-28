using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;


//------------------
//Creator: aeonhack
//Site: elitevs.net
//Created: 9/23/2011
//Changed: 9/23/2011
//Version: 1.0.0
//Theme Base: 1.5.2
//------------------
class PrimeTheme : ThemeContainer152
{

    public PrimeTheme()
    {
        MoveHeight = 32;
        BackColor = Color.White;
        TransparencyKey = Color.Fuchsia;

        SetColor("Sides", 232, 232, 232);
        SetColor("Gradient1", 252, 252, 252);
        SetColor("Gradient2", 242, 242, 242);
        SetColor("TextShade", Color.White);
        SetColor("Text", 80, 80, 80);
        SetColor("Back", Color.White);
        SetColor("Border1", 180, 180, 180);
        SetColor("Border2", Color.White);
        SetColor("Border3", Color.White);
        SetColor("Border4", 150, 150, 150);
    }

    private Color C1;
    private Color C2;
    private Color C3;
    private SolidBrush B1;
    private SolidBrush B2;
    private SolidBrush B3;
    private Pen P1;
    private Pen P2;
    private Pen P3;

    private Pen P4;
    protected override void ColorHook()
    {
        C1 = GetColor("Sides");
        C2 = GetColor("Gradient1");
        C3 = GetColor("Gradient2");

        B1 = new SolidBrush(GetColor("TextShade"));
        B2 = new SolidBrush(GetColor("Text"));
        B3 = new SolidBrush(GetColor("Back"));

        P1 = new Pen(GetColor("Border1"));
        P2 = new Pen(GetColor("Border2"));
        P3 = new Pen(GetColor("Border3"));
        P4 = new Pen(GetColor("Border4"));

        BackColor = B3.Color;
    }


    private Rectangle RT1;
    protected override void PaintHook()
    {
        G.Clear(C1);

        DrawGradient(C2, C3, 0, 0, Width, 15);

        DrawText(B1, HorizontalAlignment.Left, 13, 1);
        DrawText(B2, HorizontalAlignment.Left, 12, 0);

        RT1 = new Rectangle(12, 30, Width - 24, Height - 42);

        G.FillRectangle(B3, RT1);
        DrawBorders(P1, RT1, 1);
        DrawBorders(P2, RT1);

        DrawBorders(P3, 1);
        DrawBorders(P4);

        DrawCorners(TransparencyKey);
    }
}

//------------------
//Creator: aeonhack
//Site: elitevs.net
//Created: 9/23/2011
//Changed: 9/23/2011
//Version: 1.0.0
//Theme Base: 1.5.2
//------------------
class PrimeButton : ThemeControl152
{

    public PrimeButton()
    {
        SetColor("DownGradient1", 215, 215, 215);
        SetColor("DownGradient2", 235, 235, 235);
        SetColor("NoneGradient1", 235, 235, 235);
        SetColor("NoneGradient2", 215, 215, 215);
        SetColor("NoneGradient3", 252, 252, 252);
        SetColor("NoneGradient4", 242, 242, 242);
        SetColor("Glow", 50, Color.White);
        SetColor("TextShade", Color.White);
        SetColor("Text", 80, 80, 80);
        SetColor("Border1", Color.White);
        SetColor("Border2", 180, 180, 180);
    }

    private Color C1;
    private Color C2;
    private Color C3;
    private Color C4;
    private Color C5;
    private Color C6;
    private SolidBrush B1;
    private SolidBrush B2;
    private SolidBrush B3;
    private Pen P1;

    private Pen P2;
    protected override void ColorHook()
    {
        C1 = GetColor("DownGradient1");
        C2 = GetColor("DownGradient2");
        C3 = GetColor("NoneGradient1");
        C4 = GetColor("NoneGradient2");
        C5 = GetColor("NoneGradient3");
        C6 = GetColor("NoneGradient4");

        B1 = new SolidBrush(GetColor("Glow"));
        B2 = new SolidBrush(GetColor("TextShade"));
        B3 = new SolidBrush(GetColor("Text"));

        P1 = new Pen(GetColor("Border1"));
        P2 = new Pen(GetColor("Border2"));
    }


    protected override void PaintHook()
    {
        if (State == MouseState.Down)
        {
            DrawGradient(C1, C2, ClientRectangle, 90);
        }
        else
        {
            DrawGradient(C3, C4, ClientRectangle, 90);
            DrawGradient(C5, C6, 0, 0, Width, Height / 2, 90f);
        }

        if (State == MouseState.Over)
        {
            G.FillRectangle(B1, ClientRectangle);
        }

        if (State == MouseState.Down)
        {
            DrawText(B2, HorizontalAlignment.Center, 2, 2);
            DrawText(B3, HorizontalAlignment.Center, 1, 1);
        }
        else
        {
            DrawText(B2, HorizontalAlignment.Center, 1, 1);
            DrawText(B3, HorizontalAlignment.Center, 0, 0);
        }

        DrawBorders(P1, 1);
        DrawBorders(P2);

        DrawCorners(BackColor);
    }
}