using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public Vector2 InputVelocity
    {
        get;
        private set;
    }
    
    public void OnMovementChanged(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        float horizontalInput = context.ReadValue<float>();
        InputVelocity = new Vector2(horizontalInput, 0);
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        InputVelocity += new Vector2(0, 1);
    }

    public void ResetJump() => InputVelocity = new Vector2(InputVelocity.x, 0);
}
