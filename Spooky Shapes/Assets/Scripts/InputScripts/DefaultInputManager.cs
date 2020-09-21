using UnityEngine;
using UnityEngine.EventSystems;

public class DefaultInputManager : InputManager
{
    private bool _hasCatchedOnInputDown;

    public void Update()
    {
        InputControl();
    }

    private void InputControl()
    {
        if (IsInputDisabled)
        {
            return;
        }
#if !UNITY_EDITOR
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                OnInputUp(GetInputPosition());
            }
            else if (Input.touches[0].phase == TouchPhase.Began)
            {
                OnInputDown(GetInputPosition());
            }
            else
            {
                OnInput(GetInputPosition());
            }
        }
#else
        if (Input.GetMouseButtonUp(0))
        {
            OnInputUp(GetInputPosition());
        }
        else if (Input.GetMouseButtonDown(0))
        {
            OnInputDown(GetInputPosition());
        }
        else if (Input.GetMouseButton(0))
        {
            OnInput(GetInputPosition());
        }
#endif
    }

    private Vector2 GetInputPosition()
    {
#if !UNITY_EDITOR
        if (Input.touchCount > 0)
        {
            return Input.touches[0].position;
        }
        return Vector2.zero;
#else
        return Input.mousePosition;
#endif
    }
    

    public override void OnInput(Vector2 pos)
    {
        if (!_hasCatchedOnInputDown)
        {
            return;
        }

        base.OnInput(pos);
    }

    public override void OnInputDown(Vector2 pos)
    {
        if (IsOverUiElement())
        {
            return;
        }
        _hasCatchedOnInputDown = true;

        base.OnInputDown(pos);
    }

    public override void OnInputUp(Vector2 pos)
    {
        if (!_hasCatchedOnInputDown)
        {
            return;
        }
        _hasCatchedOnInputDown = false;

        base.OnInputUp(pos);
    }

}