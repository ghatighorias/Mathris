
using UnityEngine;


public static class InputHandler
{
    public static InputActionMapper GetUserAction()
    {
        return new InputActionMapper()
        {
            softDrop = false,//Input.GetKeyDown(KeyCode.DownArrow) | Input.GetKeyUp(KeyCode.DownArrow),
            hardDrop = Input.GetKeyDown(KeyCode.Space),
            rotate = Input.GetKeyDown(KeyCode.UpArrow),
            moveLeft = Input.GetKeyDown(KeyCode.LeftArrow),
            moveRight = Input.GetKeyDown(KeyCode.RightArrow)
        };
    }
}


public class InputActionMapper
{
    public bool softDrop;
    public bool hardDrop;
    public bool moveLeft;
    public bool moveRight;
    public bool rotate;
}