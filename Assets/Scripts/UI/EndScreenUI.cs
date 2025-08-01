using System;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField]
    private GameEvent _backToLobbyScreen;
    public void BackToLobbyScreen()
    {
        _backToLobbyScreen.Raise(this, EventArgs.Empty);
    }
}
