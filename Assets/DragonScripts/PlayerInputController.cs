using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputState
{
    public float horizontalMovement = 0;
    public bool ducking;
    public bool jumpPressed;
    public bool pullPressed;
    public bool throwPressed;
}


public class PlayerInputController : MonoBehaviour{
    private InputState inputState = new InputState();

    public float HorizontalMovement => inputState.horizontalMovement;
    
    public bool JumpPressed() { 
        bool pressed = inputState.jumpPressed;
        inputState.jumpPressed = false;
        return pressed;
    } 
    public bool ThrowPressed() { 
        bool pressed = inputState.throwPressed;
        inputState.throwPressed = false;
        return pressed;
    } 
    
    public bool PullPressed() { 
        bool pressed = inputState.pullPressed;
        inputState.pullPressed = false;
        return pressed;
    } 

    public bool Ducking => inputState.ducking;

    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        inputState.horizontalMovement = inputVec.x;
        inputState.ducking = inputVec.y < -0.3f;
    }

    public void OnJump()
    {
        inputState.jumpPressed = true;
    }

    public void OnPull()
    {
        inputState.pullPressed = true;
    }

    public void OnThrow()
    {
        inputState.throwPressed = true;
    }

    public void ClearInputs() 
    {
        inputState.throwPressed = false;
        inputState.pullPressed = false;
        inputState.jumpPressed = false;
    }
}

