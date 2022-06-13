using UnityEngine;

[CreateAssetMenu(fileName = "AIController", menuName = "InputController/AIController")]
public class AIInputSource : InputSource
{
    
    // These are currently only placeholder values as there is no AI System yet
    public override float GetHorizontalInput()
    {
        return 0f;
    }

    public override Vector2 GetHorizontalAndVerticalInput()
    {
        return Vector2.zero;
    }

    public override bool GetJumpInput()
    {
        return false;
    }

    public override bool GetDashInput()
    {
        return false;
    }

    public override bool GetExitInput()
    {
        return false;
    }
}
