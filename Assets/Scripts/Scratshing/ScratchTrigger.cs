using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScratchTrigger : MonoBehaviour
{
    [SerializeField]
    private GameEvent _scratchedSpot;

    private void OnTriggerStay(Collider other)
    {
        if (!Mouse.current.leftButton.isPressed) return;
        if (other.tag != "Mouse") return;

        _scratchedSpot.Raise(this, EventArgs.Empty);

        Destroy(gameObject);
    }
}
