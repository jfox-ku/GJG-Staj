using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Changed MonoSingleton into Monobehaviour and singleton
public class InputManager : MonoSingleton<InputManager>
{

    public bool IsInputDisabled;
    public event Action<Vector2> InputEvent;
    public event Action<Vector2> InputDownEvent;
    public event Action<Vector2> InputUpEvent;
    public virtual void OnInput(Vector2 pos)
    {
        InputEvent?.Invoke(pos);
    }

    #region inputDown
    public virtual void OnInputDown(Vector2 pos)
    {
        InputDownEvent?.Invoke(pos);
    }
    #endregion inputDown

    public virtual void OnInputUp(Vector2 pos)
    {
        InputUpEvent?.Invoke(pos);
    }
    
    public bool IsOverUiElement()
    {
#if UNITY_EDITOR
        return EventSystem.current.IsPointerOverGameObject();
#else
        if (Input.touchCount > 0)
        {
            return EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId);
        }
        return false;
#endif
    }
}
