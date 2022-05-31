using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float GetMovementInput();
    public abstract bool GetJumpInput();
}
