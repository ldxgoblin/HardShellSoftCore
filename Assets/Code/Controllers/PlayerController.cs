using UnityEngine;

[CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
public class PlayerController : InputController
{
    public override float GetMovementInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override bool GetJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }
}
