using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SimpleSaveLoad;
public class SwipeControl : MonoBehaviour
{
    int HALF_SCREEN = Screen.width / 2;
    public int SWIPE_SENSITIVITY { get; set; }

    InputControl inputControl;

    TouchData touchData0 = new TouchData();
    TouchData touchData1 = new TouchData();

    bool isMoveRight = false;
    bool isMoveLeft = false;

    void Awake()
    {
        inputControl = GetComponent<InputControl>();
    }

    int touchCount = 0;
    Touch touch;
    void Update()
    {
        touchCount = Input.touchCount;
        if (touchCount > 0)
        {
            for (int i = 0; i < touchCount; i++)
            {
                touch = Input.GetTouch(i);

                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        TouchBegin(touch);
                        break;
                    case TouchPhase.Moved:
                        if (touch.fingerId == touchData0.FINGER_ID) MoveTouch(touchData0, touch.position.x);
                        if (touch.fingerId == touchData1.FINGER_ID) MoveTouch(touchData1, touch.position.x);
                        break;
                    case TouchPhase.Stationary:
                        if (touch.fingerId == touchData0.FINGER_ID) StationaryTouch(touchData0, touch.position.x);
                        if (touch.fingerId == touchData1.FINGER_ID) StationaryTouch(touchData1, touch.position.x);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (touch.fingerId == touchData0.FINGER_ID) TouchUp(touchData0);
                        if (touch.fingerId == touchData1.FINGER_ID) TouchUp(touchData1);
                        break;
                    default:
                        break;
                }
            }
        }

        if (isMoveRight) inputControl.MoveRight();
        if (isMoveLeft) inputControl.MoveLeft();
    }

    void TouchBegin(Touch touch)
    {
        if (touchData0.IS_EMPTY)
        {
            touchData0.IS_EMPTY = false;
            touchData0.FINGER_ID = touch.fingerId;
            touchData0.IS_MOVE = touch.position.x < HALF_SCREEN;
            touchData0.TOUCH_START_POS_X = touch.position.x;
            if (touchData0.IS_MOVE == false) JumpTouch();
        }
        else if (touchData1.IS_EMPTY)
        {
            touchData1.IS_EMPTY = false;
            touchData1.FINGER_ID = touch.fingerId;
            touchData1.IS_MOVE = touch.position.x < HALF_SCREEN;
            touchData1.TOUCH_START_POS_X = touch.position.x;
            if (touchData1.IS_MOVE == false) JumpTouch();
        }
    }

    void MoveTouch(TouchData touchData, float touchPosX)
    {
        if (touchData.IS_MOVE == false)
            return;

        if ((touchPosX - touchData.TOUCH_START_POS_X) > SWIPE_SENSITIVITY)
        {
            isMoveLeft = false;
            isMoveRight = true;
            touchData.TOUCH_START_POS_X = touchPosX;
        }
        if ((touchData.TOUCH_START_POS_X - touchPosX) > SWIPE_SENSITIVITY)
        {
            isMoveRight = false;
            isMoveLeft = true;
            touchData.TOUCH_START_POS_X = touchPosX;
        }
    }

    void StationaryTouch(TouchData touchData, float touchPosX) // Sensitive mesafe geçilmeden yapılan hareketler için şart.
    {
        if (touchData.IS_MOVE == false)
            return;

        touchData.TOUCH_START_POS_X = touchPosX;
    }

    void TouchUp(TouchData touchData)
    {
        if (touchData.IS_MOVE)
        {
            isMoveRight = false;
            isMoveLeft = false;
            inputControl.MoveStop();
        }

        touchData.IS_EMPTY = true;
        touchData.IS_MOVE = false;
        touchData.FINGER_ID = -1;
    }

    void JumpTouch()
    {
        inputControl.Jump();
    }
}

public class TouchData
{
    public bool IS_EMPTY = true;
    public int FINGER_ID = -1;
    public bool IS_MOVE;
    public float TOUCH_START_POS_X;
}