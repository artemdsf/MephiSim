using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteMenu : MonoBehaviour
{
    [SerializeField] private RectTransform _winningScreen;

    private void Start()
    {
        CharactersManager.Instance.OnBossesDeath.AddListener(OpenWinningScreen);
    }

    void OpenWinningScreen()
    {
        _winningScreen.gameObject.SetActive(true);
    }

}
