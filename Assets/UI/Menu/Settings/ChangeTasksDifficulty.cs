using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTasksDifficulty : MonoBehaviour
{
    private Button _button;
    private Image _image;
    private Sprite _normalSprite;

    [SerializeField] private GameManager.TasksDiffcultyEnum _setDifficulty;

    private void Start()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
   
        _normalSprite = _image.sprite;

        _button.onClick.AddListener(OnClick);
        GameManager.TaskDifficultyChanged += DifficultyChanged;

        DifficultyChanged(GameManager.TasksDiffculty);
    }

    private void OnClick()
    {
        GameManager.TasksDiffculty = _setDifficulty;
        Debug.Log(GameManager.TasksDiffculty);
    }

    private void DifficultyChanged(GameManager.TasksDiffcultyEnum tasksDiffculty)
    {
        StartCoroutine(DifficultyChangedEnumerator());
    }

    IEnumerator DifficultyChangedEnumerator()
    {
        yield return new WaitForEndOfFrame();

        if (GameManager.TasksDiffculty == _setDifficulty)
        {
            _image.sprite = _button.spriteState.pressedSprite;
            _button.interactable = false;
        }
        else
        {
            _image.sprite = _normalSprite;
            _button.interactable = true;
        }
    }
}
    