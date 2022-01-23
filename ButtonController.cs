using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public PanelButtons panelButtons;
    public ButtonType buttonType;

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case ButtonType.LEFT:
                panelButtons.MoveLeft();
                break;
            case ButtonType.RIGHT:
                panelButtons.MoveRight();
                break;
            case ButtonType.JUMP:
                panelButtons.Jump();
                break;
            default:
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case ButtonType.LEFT:
            case ButtonType.RIGHT:
                panelButtons.MoveStop();
                break;
            default:
                break;
        }
    }
}

public enum ButtonType{
    LEFT,
    RIGHT,
    JUMP
}
