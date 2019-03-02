using UnityEngine;

public static class ResourceLoader
{
    public static GameObject LoadMathItemPrefab(MathItemType type, Color bgColor)
    {
        string prefabName;

        switch (type)
        {
            case MathItemType.Zero:
                prefabName = "0";
                break;
            case MathItemType.One:
                prefabName = "1";
                break;
            case MathItemType.Two:
                prefabName = "2";
                break;
            case MathItemType.Three:
                prefabName = "3";
                break;
            case MathItemType.Four:
                prefabName = "4";
                break;
            case MathItemType.Five:
                prefabName = "5";
                break;
            case MathItemType.Six:
                prefabName = "6";
                break;
            case MathItemType.Seven:
                prefabName = "7";
                break;
            case MathItemType.Eight:
                prefabName = "8";
                break;
            case MathItemType.Nine:
                prefabName = "9";
                break;
            case MathItemType.Plus:
                prefabName = "plus";
                break;
            case MathItemType.Minus:
                prefabName = "minus";
                break;
            case MathItemType.Division:
                prefabName = "division";
                break;
            case MathItemType.Multiply:
                prefabName = "multiply";
                break;
            default:
                Debug.Log("wrong type for instantiating shape");
                return null;
        }

        var prefabFullName = string.Format("Prefabs/MathItems/{0}", prefabName);

        return Resources.Load<GameObject>(prefabName);
    }

    public static GameObject LoadShapePrefab(int shapeNumber)
    {
        var shapePrefix = "Tile";
        var shapePostfix = string.Empty;

        switch (shapeNumber)
        {
            case 1:
                shapePostfix = "J";
                break;
            case 2:
                shapePostfix = "L";
                break;
            case 3:
                shapePostfix = "Long";
                break;
            case 4:
                shapePostfix = "S";
                break;
            case 5:
                shapePostfix = "Square";
                break;
            case 6:
                shapePostfix = "T";
                break;
            case 7:
                shapePostfix = "Z";
                break;
            default:
                Debug.Log("wrong number for instantiating shape");
                return null;
        }

        var shapeFullName = string.Format("Prefabs/{0}_{1}", shapePrefix, shapePostfix);
        return Resources.Load<GameObject>(shapeFullName);
    }
}
