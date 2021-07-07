using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace CompCamo
{
    // Token: 0x02000007 RID: 7
    public class CamoPresetColour
    {
        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000045 RID: 69 RVA: 0x00004AD0 File Offset: 0x00002CD0
        public static Color Orange => new Color(0.870588243f, 0.509803951f, 0f, 1f);

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000046 RID: 70 RVA: 0x00004AEB File Offset: 0x00002CEB
        public static Color Purple => new Color(0.5019608f, 0f, 0.3372549f, 1f);

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x06000047 RID: 71 RVA: 0x00004B06 File Offset: 0x00002D06
        public static Color Violet => new Color(1f, 0.41568628f, 0.7607843f, 1f);

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000048 RID: 72 RVA: 0x00004B21 File Offset: 0x00002D21
        public static Color Brown => new Color(0.392156869f, 0.1254902f, 0.0784313753f, 1f);

        // Token: 0x06000049 RID: 73 RVA: 0x00004B3C File Offset: 0x00002D3C
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

        // Token: 0x0600004A RID: 74 RVA: 0x00004BC8 File Offset: 0x00002DC8
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

        // Token: 0x0600004B RID: 75 RVA: 0x00004D44 File Offset: 0x00002F44
        public static float ColourCompare(Color color, Color compare)
        {
            return 0f + Math.Abs(color.r - compare.r) + Math.Abs(color.g - compare.g) + Math.Abs(color.b - compare.b);
        }
    }
}