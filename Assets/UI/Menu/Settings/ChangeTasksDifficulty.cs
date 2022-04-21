using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTasksDifficulty : MonoBehaviour
{
    private Button _button;
    private Image _image;
    private Sprite _normalSprite;

    [SerializeField] private LevelManager.TasksDiffcultyEnum _setDifficulty;

    private void Start()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
   
        _normalSprite = _image.sprite;

        _button.onClick.AddListener(OnClick);
        LevelManager.TaskDifficultyChanged += DifficultyChanged;

        DifficultyChanged(LevelManager.TasksDiffculty);
    }

    private void OnClick()
    {
        LevelManager.TasksDiffculty = _setDifficulty;
        Debug.Log(LevelManager.TasksDiffculty);
    }

    private void DifficultyChanged(LevelManager.TasksDiffcultyEnum tasksDiffculty)
    {
        StartCoroutine(DifficultyChangedEnumerator());
    }

    IEnumerator DifficultyChangedEnumerator()
    {
        yield return new WaitForEndOfFrame();

        if (LevelManager.TasksDiffculty == _setDifficulty)
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
    