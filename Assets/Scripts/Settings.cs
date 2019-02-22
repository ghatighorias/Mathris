using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {
    public float fallDelay = 1F;
    public bool allowAutomaticDrop = true;
    public Vector2 shapeSpawnPosition = Vector2.zero;


    void Start()
    {
        SpawnRandomShape();
    }

    GameObject SpawnRandomShape()
    {
        var randomShapeNumber = Random.Range(1, 7);
        var shapePrefix = "Tile";
        var shapePostfix = string.Empty;

        switch (randomShapeNumber)
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
        }

        var shapeFullName = string.Format("Prefabs/{0}_{1}", shapePrefix, shapePostfix);

        return (GameObject)Instantiate(Resources.Load<GameObject>(shapeFullName), shapeSpawnPosition, Quaternion.identity);
    }
}
