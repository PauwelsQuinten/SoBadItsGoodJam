using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoardUI : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _uiElements;
    [SerializeField]
    private TextMeshPro _scorePlayer1, _scorePlayer2;

    public void EnableUI(bool enable)
    {
        foreach (var uiElement in _uiElements) 
        { 
            uiElement.SetActive(enable);
        }
    }

    public void SetScore(int score01, int score02)
    {
        _scorePlayer1.text = score01.ToString();
        _scorePlayer2.text = score02.ToString();
    }
}
