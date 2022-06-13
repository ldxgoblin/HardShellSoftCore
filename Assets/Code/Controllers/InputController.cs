using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float GetHorizontalInput();
    public abstract Vector2 GetHorizontalAndVerticalInput();
    public abstract bool GetJumpInput();
    public abstract bool GetDashInput();
    
}
