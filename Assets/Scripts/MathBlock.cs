using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathBlock : MonoBehaviour {
    public MathItemType Type;
    public Sprite mathSprite;
    public Color color = Color.white;

    public static MathBlock InstantiateRandomMathBlock(MathItemType type, Color bgColor, Transform parent)
    {
        return Instantiate(type, bgColor, parent);
    }

    public static MathBlock Instantiate(MathItemType type, Color bgColor, Transform parent)
    {
        var loadedResource = ResourceLoader.LoadMathItemPrefab(type, bgColor);

        if (!loadedResource)
            return null;

        var mathBlock = Instantiate(loadedResource, parent)?.GetComponent<MathBlock>();

        if (!mathBlock)
            return null;

        mathBlock.GetComponent<SpriteRenderer>().color = bgColor;
        mathBlock.Type = type;

        return mathBlock;
    }

    public static string GetMathEquation(List<MathBlock> mathBlocks)
    {
        string equasion = string.Empty;

        foreach (var mathBlock in mathBlocks)
        {
            equasion += mathBlock.GetStringValue();
        }

        return equasion;
    }

    public string GetStringValue()
    {
        switch (Type)
        {
            case MathItemType.Zero:
                return "0";
            case MathItemType.One:
                return "1";
            case MathItemType.Two:
                return "2";
            case MathItemType.Three:
                return "3";
            case MathItemType.Four:
                return "4";
            case MathItemType.Five:
                return "5";
            case MathItemType.Six:
                return "6";
            case MathItemType.Seven:
                return "7";
            case MathItemType.Eight:
                return "8";
            case MathItemType.Nine:
                return "9";
            case MathItemType.Plus:
                return "+";
            case MathItemType.Minus:
                return "-";
            case MathItemType.Division:
                return "/";
            case MathItemType.Multiply:
                return "*";
            default:
                return string.Empty;
        }
    }

    /*
     * randomize numbers and operators differently
     * map the mathitemtype enum to prefab names
     */

}
