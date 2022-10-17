using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CompCamo;

public class CamoPresets
{
    internal static List<string> GetCamoTags()
    {
        return new List<string>
        {
            "PassiveCamo_Arctic_Low",
            "PassiveCamo_Arctic_Med",
            "PassiveCamo_Arctic_High",
            "PassiveCamo_Desert_Low",
            "PassiveCamo_Desert_Med",
            "PassiveCamo_Desert_High",
            "PassiveCamo_Jungle_Low",
            "PassiveCamo_Jungle_Med",
            "PassiveCamo_Jungle_High",
            "PassiveCamo_Stone_Low",
            "PassiveCamo_Stone_Med",
            "PassiveCamo_Stone_High",
            "PassiveCamo_Woodland_Low",
            "PassiveCamo_Woodland_Med",
            "PassiveCamo_Woodland_High",
            "PassiveCamo_Urban_Low",
            "PassiveCamo_Urban_Med",
            "PassiveCamo_Urban_High"
        };
    }

    internal static List<string> GetColourTags()
    {
        return new List<string>
        {
            "PassiveCamo_Black_Set",
            "PassiveCamo_White_Set",
            "PassiveCamo_Red_Set",
            "PassiveCamo_Orange_Set",
            "PassiveCamo_Yellow_Set",
            "PassiveCamo_Green_Set",
            "PassiveCamo_Blue_Set",
            "PassiveCamo_Cyan_Set",
            "PassiveCamo_Violet_Set",
            "PassiveCamo_Purple_Set",
            "PassiveCamo_Brown_Set",
            "PassiveCamo_Rainbow_Set",
            "PassiveCamo_Light_Set",
            "PassiveCamo_Dark_Set"
        };
    }

    public static float GetCamoPresetEff(Apparel apparel, string type)
    {
        var num = 0f;
        var compGearCamo = apparel.TryGetComp<CompGearCamo>();
        if (compGearCamo != null)
        {
            switch (type)
            {
                case "Arctic":
                    num = compGearCamo.Props.ArcticCamoEff;
                    break;
                case "Desert":
                    num = compGearCamo.Props.DesertCamoEff;
                    break;
                case "Jungle":
                    num = compGearCamo.Props.JungleCamoEff;
                    break;
                case "Stone":
                    num = compGearCamo.Props.StoneCamoEff;
                    break;
                case "Woodland":
                    num = compGearCamo.Props.WoodlandCamoEff;
                    break;
                case "Urban":
                    num = compGearCamo.Props.UrbanCamoEff;
                    break;
            }
        }
        else
        {
            var def = apparel.def;
            List<string> list;
            if (def == null)
            {
                list = null;
            }
            else
            {
                var apparel2 = def.apparel;
                list = apparel2?.tags;
            }

            var list2 = list;
            if (list2 is not { Count: > 0 })
            {
                return num;
            }

            foreach (var text in list2)
            {
                if (!text.StartsWith("PassiveCamo") || !GetCamoTags().Contains(text) &&
                    !GetColourTags().Contains(text) &&
                    !text.StartsWith("PassiveCamo_Multi") &&
                    !text.StartsWith("PassiveCamo_Colour"))
                {
                    continue;
                }

                var text2 = GetTagValue(text, 1);
                if (!CamoGearUtility.CamoTypes().Contains(text2) && text2 != "Multi")
                {
                    if (text2 == "Colour")
                    {
                        text2 = CamoPresetColour.GetClosestType(apparel);
                    }

                    switch (text2)
                    {
                        case "Black":
                            switch (type)
                            {
                                case "Arctic":
                                    num = 0.12f;
                                    break;
                                case "Desert":
                                    num = 0.25f;
                                    break;
                                case "Jungle":
                                    num = 0.3f;
                                    break;
                                case "Stone":
                                    num = 0.27f;
                                    break;
                                case "Woodland":
                                    num = 0.29f;
                                    break;
                                case "Urban":
                                    num = 0.3f;
                                    break;
                            }

                            break;
                        case "White":
                            switch (type)
                            {
                                case "Arctic":
                                    num = 0.65f;
                                    break;
                                case "Desert":
                                case "Jungle":
                                    num = 0.22f;
                                    break;
                                case "Stone":
                                    num = 0.3f;
                                    break;
                                case "Woodland":
                                    num = 0.22f;
                                    break;
                                case "Urban":
                                    num = 0.3f;
                                    break;
                            }

                            break;
                        case "Red":
                            switch (type)
                            {
                                case "Arctic":
                                    num = 0.05f;
                                    break;
                                case "Desert":
                                    num = 0.32f;
                                    break;
                                case "Jungle":
                                    num = 0.25f;
                                    break;
                                case "Stone":
                                    num = 0.15f;
                                    break;
                                case "Woodland":
                                    num = 0.25f;
                                    break;
                                case "Urban":
                                    num = 0.15f;
                                    break;
                            }

                            break;
                        case "Orange":
                            switch (type)
                            {
                                case "Arctic":
                                    num = 0.07f;
                                    break;
                                case "Desert":
                                    num = 0.55f;
                                    break;
                                case "Jungle":
                                    num = 0.25f;
                                    break;
                                case "Stone":
                                    num = 0.15f;
                                    break;
                                case "Woodland":
                                    num = 0.24f;
                                    break;
                                case "Urban":
                                    num = 0.17f;
                                    break;
                            }

                            break;
                        case "Yellow":
                            switch (type)
                            {
                                case "Arctic":
                                    num = 0.12f;
                                    break;
                                case "Desert":
                                    num = 0.6f;
                                    break;
                                case "Jungle":
                                    num = 0.29f;
                                    break;
                                case "Stone":
                                    num = 0.12f;
                                    break;
                                case "Woodland":
                                    num = 0.27f;
                                    break;
                                case "Urban":
                                    num = 0.18f;
                                    break;
                            }

                            break;
                        case "Green":
                            switch (type)
                            {
                                case "Arctic":
                                    num = 0.1f;
                                    break;
                                case "Desert":
                                    num = 0.37f;
                                    break;
                                case "Jungle":
                                    num = 0.58f;
                                    break;
                                case "Stone":
                                    num = 0.15f;
                                    break;
                                case "Woodland":
                                    num = 0.55f;
                                    break;
                                case "Urban":
                                    num = 0.19f;
                                    break;
                            }

                            break;
                        case "Blue":
                            switch (type)
                            {
                                case "Arctic":
                                    num = 0.1f;
                                    break;
                                case "Desert":
                                    num = 0.22f;
                                    break;
                                case "Jungle":
                                    num = 0.37f;
                                    break;
                                case "Stone":
                                    num = 0.25f;
                                    break;
                                case "Woodland":
                                    num = 0.35f;
                                    break;
                                case "Urban":
                                    num = 0.25f;
                                    break;
                            }

                            break;
                        case "Cyan":
                            switch (type)
                            {
                                case "Arctic":
                                    num = 0.19f;
                                    break;
                                case "Desert":
                                    num = 0.12f;
                                    break;
                                case "Jungle":
                                    num = 0.24f;
                                    break;
                                case "Stone":
                                    num = 0.25f;
                                    break;
                                case "Woodland":
                                    num = 0.24f;
                                    break;
                                case "Urban":
                                    num = 0.23f;
                                    break;
                            }

                            break;
                        case "Violet":
                            switch (type)
                            {
                                case "Arctic":
                                    num = 0.09f;
                                    break;
                                case "Desert":
                                    num = 0.26f;
                                    break;
                                case "Jungle":
                                    num = 0.21f;
                                    break;
                                case "Stone":
                                case "Woodland":
                                case "Urban":
                                    num = 0.19f;
                                    break;
                            }

                            break;
                        case "Purple":
                            switch (type)
                            {
                                case "Arctic":
                                    num = 0.07f;
                                    break;
                                case "Desert":
                                    num = 0.22f;
                                    break;
                                case "Jungle":
                                    num = 0.24f;
                                    break;
                                case "Stone":
                                case "Woodland":
                                    num = 0.22f;
                                    break;
                                case "Urban":
                                    num = 0.21f;
                                    break;
                            }

                            break;
                        case "Brown":
                            switch (type)
                            {
                                case "Arctic":
                                    num = 0.09f;
                                    break;
                                case "Desert":
                                    num = 0.42f;
                                    break;
                                case "Jungle":
                                    num = 0.4f;
                                    break;
                                case "Stone":
                                    num = 0.42f;
                                    break;
                                case "Woodland":
                                    num = 0.41f;
                                    break;
                                case "Urban":
                                    num = 0.23f;
                                    break;
                            }

                            break;
                        case "Dark":
                            switch (type)
                            {
                                case "Arctic":
                                    num = 0.11f;
                                    break;
                                case "Desert":
                                    num = 0.26f;
                                    break;
                                case "Jungle":
                                    num = 0.31f;
                                    break;
                                case "Stone":
                                    num = 0.28f;
                                    break;
                                case "Woodland":
                                    num = 0.3f;
                                    break;
                                case "Urban":
                                    num = 0.31f;
                                    break;
                            }

                            break;
                        case "Light":
                            switch (type)
                            {
                                case "Arctic":
                                    num = 0.4f;
                                    break;
                                case "Desert":
                                    num = 0.3f;
                                    break;
                                case "Jungle":
                                case "Stone":
                                case "Woodland":
                                    num = 0.2f;
                                    break;
                                case "Urban":
                                    num = 0.35f;
                                    break;
                            }

                            break;
                        case "Rainbow":
                            switch (type)
                            {
                                case "Arctic":
                                case "Desert":
                                case "Jungle":
                                case "Stone":
                                case "Woodland":
                                    num = 0.25f;
                                    break;
                                case "Urban":
                                    num = 0.3f;
                                    break;
                            }

                            break;
                    }

                    return num;
                }

                if (text2 == null)
                {
                    continue;
                }

                switch (text2)
                {
                    case "Arctic":
                        switch (type)
                        {
                            case "Arctic":
                                num = 0.54f;
                                break;
                            case "Desert":
                            case "Jungle":
                                num = 0.22f;
                                break;
                            case "Stone":
                                num = 0.39f;
                                break;
                            case "Woodland":
                                num = 0.22f;
                                break;
                            case "Urban":
                                num = 0.32f;
                                break;
                        }

                        break;
                    case "Desert":
                        switch (type)
                        {
                            case "Arctic":
                                num = 0.22f;
                                break;
                            case "Desert":
                                num = 0.54f;
                                break;
                            case "Jungle":
                                num = 0.39f;
                                break;
                            case "Stone":
                                num = 0.22f;
                                break;
                            case "Woodland":
                                num = 0.39f;
                                break;
                            case "Urban":
                                num = 0.32f;
                                break;
                        }

                        break;
                    case "Jungle":
                        switch (type)
                        {
                            case "Arctic":
                                num = 0.22f;
                                break;
                            case "Desert":
                                num = 0.39f;
                                break;
                            case "Jungle":
                                num = 0.54f;
                                break;
                            case "Stone":
                                num = 0.22f;
                                break;
                            case "Woodland":
                                num = 0.48f;
                                break;
                            case "Urban":
                                num = 0.32f;
                                break;
                        }

                        break;
                    case "Stone":
                        switch (type)
                        {
                            case "Arctic":
                                num = 0.39f;
                                break;
                            case "Desert":
                            case "Jungle":
                                num = 0.22f;
                                break;
                            case "Stone":
                                num = 0.54f;
                                break;
                            case "Woodland":
                                num = 0.22f;
                                break;
                            case "Urban":
                                num = 0.37f;
                                break;
                        }

                        break;
                    case "Woodland":
                        switch (type)
                        {
                            case "Arctic":
                                num = 0.22f;
                                break;
                            case "Desert":
                                num = 0.39f;
                                break;
                            case "Jungle":
                                num = 0.48f;
                                break;
                            case "Stone":
                                num = 0.22f;
                                break;
                            case "Woodland":
                                num = 0.54f;
                                break;
                            case "Urban":
                                num = 0.32f;
                                break;
                        }

                        break;
                    case "Urban":
                        switch (type)
                        {
                            case "Arctic":
                            case "Desert":
                            case "Jungle":
                                num = 0.32f;
                                break;
                            case "Stone":
                                num = 0.37f;
                                break;
                            case "Woodland":
                                num = 0.32f;
                                break;
                            case "Urban":
                                num = 0.54f;
                                break;
                        }

                        break;
                    case "Multi":
                        switch (type)
                        {
                            case "Arctic":
                                num = 0.32f;
                                break;
                            case "Desert":
                                num = 0.49f;
                                break;
                            case "Jungle":
                                num = 0.47f;
                                break;
                            case "Stone":
                                num = 0.37f;
                                break;
                            case "Woodland":
                                num = 0.49f;
                                break;
                            case "Urban":
                                num = 0.35f;
                                break;
                        }

                        break;
                }

                var tagValue = GetTagValue(text, 2);
                if (tagValue != null)
                {
                    switch (tagValue)
                    {
                        case "Med":
                            num *= 1.2f;
                            break;
                        case "High":
                            num *= 1.33f;
                            break;
                    }
                }

                var techLevel = apparel.def.techLevel;
                if (techLevel != TechLevel.Spacer)
                {
                    if (techLevel == TechLevel.Ultra)
                    {
                        num *= 1.1f;
                    }
                }
                else
                {
                    num *= 1.05f;
                }

                return num;
            }

            return num;
        }

        return num;
    }

    internal static string GetTagValue(string valuesStr, int position)
    {
        char[] separator =
        {
            '_'
        };
        var array = valuesStr.Split(separator);
        return array[position];
    }
}