using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    GameEvent _MovementInputChanged;
    public void HandleMovementInput(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        _MovementInputChanged.Raise(this, input);
    }
}
