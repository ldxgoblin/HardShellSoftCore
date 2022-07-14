using UnityEngine;

public abstract class InputSource : ScriptableObject
{
    public abstract float GetHorizontalInput();
    public abstract Vector2 GetHorizontalAndVerticalInput();
    public abstract bool GetJumpInput();
    public abstract bool GetBoosterInput();
    public abstract bool GetDashInput();
    public abstract bool GetExitInput();
    public abstract bool GetFireInput();
}