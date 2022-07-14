using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private InputSource inputSource;
    public InputSource InputSource => inputSource;

    public bool IsInputActive()
    {
        return inputSource != null;
    }

    private void SetInputSource(InputSource source)
    {
        inputSource = source;
    }

    private void ClearInputSource()
    {
        inputSource = null;
    }

    public void SwapInputSource(InputHandler origin)
    {
        SetInputSource(origin.InputSource);
        origin.ClearInputSource();
    }
}