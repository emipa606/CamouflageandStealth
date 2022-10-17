using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace CompCamo;

public class CamoPresetColour
{
    public static Color Orange => new Color(0.870588243f, 0.509803951f, 0f, 1f);

    public static Color Purple => new Color(0.5019608f, 0f, 0.3372549f, 1f);

    public static Color Violet => new Color(1f, 0.41568628f, 0.7607843f, 1f);

    public static Color Brown => new Color(0.392156869f, 0.1254902f, 0.0784313753f, 1f);

    public static List<Color> colourChoices()
    {
        return new List<Color>
        {
            Color.white,
            Color.black,
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.cyan,
            Purple,
            Orange,
            Brown,
            Violet
        };
    }

    public static string GetClosestType(Apparel apparel)
    {
        var result = "White";
        var color = Color.white;
        var drawColor = apparel.DrawColor;
        var list = colourChoices();
        if (list.Count > 0)
        {
            var num = 9f;
            foreach (var color2 in list)
            {
                var num2 = ColourCompare(drawColor, color2);
                if (!(num2 < num))
                {
                    continue;
                }

                color = color2;
                num = num2;
            }
        }

        if (color == Color.white)
        {
            result = "White";
        }
        else if (color == Color.black)
        {
            result = "Black";
        }
        else if (color == Color.red)
        {
            result = "Red";
        }
        else if (color == Orange)
        {
            result = "Orange";
        }
        else if (color == Color.yellow)
        {
            result = "Yellow";
        }
        else if (color == Color.green)
        {
            result = "Green";
        }
        else if (color == Color.blue)
        {
            result = "Blue";
        }
        else if (color == Color.cyan)
        {
            result = "Cyan";
        }
        else if (color == Violet)
        {
            result = "Violet";
        }
        else if (color == Purple)
        {
            result = "Purple";
        }
        else if (color == Brown)
        {
            result = "Brown";
        }

        return result;
    }

    public static float ColourCompare(Color color, Color compare)
    {
        return 0f + Math.Abs(color.r - compare.r) + Math.Abs(color.g - compare.g) + Math.Abs(color.b - compare.b);
    }
}