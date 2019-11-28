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
class FlowTheme : ThemeContainer152
{

    public FlowTheme()
    {
        MoveHeight = 24;
        BackColor = Color.FromArgb(35, 35, 35);
        TransparencyKey = Color.Fuchsia;

        SetColor("Sides", 40, 40, 40);
        SetColor("Gradient", 18, 18, 18);
        SetColor("Text", 0, 132, 255);
        SetColor("Border1", 40, 40, 40);
        SetColor("Border2", 22, 22, 22);
        SetColor("Border3", 65, 65, 65);
        SetColor("Border4", Color.Black);
        SetColor("Hatch1", 39, 39, 39);
        SetColor("Hatch2", 35, 35, 35);
        SetColor("Hatch3", 29, 29, 29);
        SetColor("Hatch4", 26, 26, 26);
        SetColor("Shade1", 50, 7, 7, 7);
        SetColor("Shade2", Color.Transparent);
    }

    private Color C1;
    private Color C2;
    private SolidBrush B1;
    private Pen P1;
    private Pen P2;
    private Pen P3;

    private Pen P4;
    protected override void ColorHook()
    {
        C1 = GetColor("Sides");
        C2 = GetColor("Gradient");

        B1 = new SolidBrush(GetColor("Text"));

        P1 = new Pen(GetColor("Border1"));
        P2 = new Pen(GetColor("Border2"));
        P3 = new Pen(GetColor("Border3"));
        P4 = new Pen(GetColor("Border4"));

        CreateTile();
        CreateShade();

        BackColor = GetColor("Hatch2");
    }


    private Rectangle RT1;
    protected override void PaintHook()
    {
        G.Clear(C1);

        DrawGradient(C2, C1, 0, 0, Width, 24);
        DrawText(B1, HorizontalAlignment.Left, 8, 0);

        RT1 = new Rectangle(8, 24, Width - 16, Height - 32);
        G.FillRectangle(Tile, RT1);

        for (int I = 0; I <= Shade.Length - 1; I++)
        {
            DrawBorders(Shade[I], RT1, I);
        }

        RT1 = new Rectangle(8, 24, Width - 16, Height - 32);
        DrawBorders(P1, RT1, 1);
        DrawBorders(P2, RT1);
        DrawCorners(C1, RT1);

        DrawBorders(P3, 1);
        DrawBorders(P4);

        DrawCorners(TransparencyKey);
    }


    private TextureBrush Tile;
    private byte[] TileData = Convert.FromBase64String("AgIBAQEBAwMBAQEBAAABAQEBAQEBAgIBAQEBAwMBAQEBAAAB");
    private void CreateTile()
    {
        Bitmap TileImage = new Bitmap(6, 6);
        Color[] TileColors = new Color[] {
			GetColor("Hatch1"),
			GetColor("Hatch2"),
			GetColor("Hatch3"),
			GetColor("Hatch4")
		};

        for (int I = 0; I <= 35; I++)
        {
            TileImage.SetPixel(I % 6, I / 6, TileColors[TileData[I]]);
        }

        Tile = new TextureBrush(TileImage);
        TileImage.Dispose();
    }

    private Pen[] Shade;
    private void CreateShade()
    {
        if (Shade != null)
        {
            for (int I = 0; I <= Shade.Length - 1; I++)
            {
                Shade[I].Dispose();
            }
        }

        Bitmap ShadeImage = new Bitmap(1, 20);
        Graphics ShadeGraphics = Graphics.FromImage(ShadeImage);

        Rectangle ShadeBounds = new Rectangle(0, 0, 1, 20);
        LinearGradientBrush Gradient = new LinearGradientBrush(ShadeBounds, GetColor("Shade1"), GetColor("Shade2"), 90f);

        Shade = new Pen[20];
        ShadeGraphics.FillRectangle(Gradient, ShadeBounds);

        for (int I = 0; I <= Shade.Length - 1; I++)
        {
            Shade[I] = new Pen(ShadeImage.GetPixel(0, I));
        }

        Gradient.Dispose();
        ShadeGraphics.Dispose();
        ShadeImage.Dispose();
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
class FlowButton : ThemeControl152
{

    public FlowButton()
    {
        SetColor("DownGradient1", 24, 24, 24);
        SetColor("DownGradient2", 38, 38, 38);
        SetColor("NoneGradient1", 38, 38, 38);
        SetColor("NoneGradient2", 24, 24, 24);
        SetColor("Text", 0, 132, 255);
        SetColor("Border1", 22, 22, 22);
        SetColor("Border2A", 60, 60, 60);
        SetColor("Border2B", 36, 36, 36);
    }

    private Color C1;
    private Color C2;
    private Color C3;
    private Color C4;
    private Color C5;
    private Color C6;
    private SolidBrush B1;
    private LinearGradientBrush B2;
    private Pen P1;

    private Pen P2;
    protected override void ColorHook()
    {
        C1 = GetColor("DownGradient1");
        C2 = GetColor("DownGradient2");
        C3 = GetColor("NoneGradient1");
        C4 = GetColor("NoneGradient2");
        C5 = GetColor("Border2A");
        C6 = GetColor("Border2B");

        B1 = new SolidBrush(GetColor("Text"));

        P1 = new Pen(GetColor("Border1"));
    }

    protected override void PaintHook()
    {
        if (State == MouseState.Down)
        {
            DrawGradient(C1, C2, ClientRectangle, 90f);
        }
        else
        {
            DrawGradient(C3, C4, ClientRectangle, 90f);
        }

        DrawText(B1, HorizontalAlignment.Center, 0, 0);

        B2 = new LinearGradientBrush(ClientRectangle, C5, C6, 90f);
        P2 = new Pen(B2);

        DrawBorders(P1);
        DrawBorders(P2, 1);
    }
}