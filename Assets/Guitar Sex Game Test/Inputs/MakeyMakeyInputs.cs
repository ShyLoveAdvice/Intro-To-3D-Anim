using System;
using UnityEngine;

public enum MMInputs
{
    ButtonTopLeft,
    ButtonTopRight,
    ButtonBottomLeft,
    ButtonBottomRight
}

public class MakeyMakeyInputs : MonoBehaviour
{
    public KeyCode buttonTopLeft;
    public KeyCode buttonTopRight;
    public KeyCode buttonBottomLeft;
    public KeyCode buttonBottomRight;

    private void Update()
    {
        if (Input.GetKey(buttonTopLeft))
        {
            EventBetter.Raise(MMInputs.ButtonTopLeft);
        }

        if (Input.GetKey(buttonTopRight))
        {
            EventBetter.Raise(MMInputs.ButtonTopRight);
        }

        if (Input.GetKey(buttonBottomLeft))
        {
            EventBetter.Raise(MMInputs.ButtonBottomLeft);
        }

        if (Input.GetKey(buttonBottomRight))
        {
            EventBetter.Raise(MMInputs.ButtonBottomRight);
        }
    }
}