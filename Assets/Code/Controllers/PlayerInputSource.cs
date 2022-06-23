using UnityEngine;

[CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
public class PlayerInputSource : InputSource
{
    public override float GetHorizontalInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override Vector2 GetHorizontalAndVerticalInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public override bool GetJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public override bool GetDashInput()
    {
        return Input.GetButtonDown("Dash");
    }

    public override bool GetExitInput()
    {
        return Input.GetButtonDown("Cancel");
    }

    public override bool GetFireInput()
    {
        return Input.GetButtonDown("Fire1");
    }
}
