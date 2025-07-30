using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StartScreenUi : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _buttons = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _controllers = new List<GameObject>();
    [SerializeField]
    private GameObject _startButton;
    [SerializeField]
    private EventSystem _eventSystem;
    [SerializeField]
    private GameEvent _pressedPlay;

    private int _buttonStartSize;

    private void Start()
    {
        _buttonStartSize = (int)_buttons[0].GetComponent<RectTransform>().sizeDelta.x;
    }
    public void PlayerJoined(Component sender, object obj)
    {
        int index = (int)(obj as int?);
        StartCoroutine(EnlargeButtons(index));
        if (index == 1)
        {
            StartCoroutine(EnablePlayButton());
        }
    }

    public void StartPressed()
    {
        _pressedPlay.Raise(this, EventArgs.Empty);
    }

    private IEnumerator EnlargeButtons(int index)
    {
        float newSize = _buttonStartSize + 25;
        RectTransform button = _buttons[index].GetComponent<RectTransform>();

        float xSize = _buttonStartSize;
        float ySize = _buttonStartSize;

        while (button.sizeDelta.x < newSize)
        {
            xSize += 2 * Time.deltaTime;
            ySize += 2 * Time.deltaTime;
            button.sizeDelta = new Vector2(xSize, ySize);
        }
        yield return null;

        button.sizeDelta = new Vector2(newSize, newSize);
        xSize = newSize;
        ySize = newSize;

        newSize = _buttonStartSize;

        if (index == 0) _controllers[0].GetComponent<Image>().color = Color.cyan;
        else _controllers[1].GetComponent<Image>().color = Color.red;

        yield return new WaitForSeconds(0.2f);

        while (button.sizeDelta.x > newSize)
        {
            xSize -= 2 * Time.deltaTime;
            ySize -= 2 * Time.deltaTime;
            button.sizeDelta = new Vector2(xSize, ySize);
        }
        yield return null;

        button.sizeDelta = new Vector2(newSize, newSize);
        xSize = newSize;
        ySize = newSize;
    }

    private IEnumerator EnablePlayButton()
    {
        yield return new WaitForSeconds(0.5f);
        _startButton.SetActive(true);
        _eventSystem.firstSelectedGameObject = _startButton;
    }
}
