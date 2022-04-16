using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private TextAsset _textFile;
    private string[] _messages;
    private int _messageIndex = 0;
    private bool _isActive = false;

    private void Start()
    {
        _text.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isActive == false)
        {
            Debug.Log("1");
            _isActive = true;
            _messageIndex = 0;
            _text.text = _messages[0];
            _text.gameObject.SetActive(true);
        }
    }

    private void Awake()
    {
        _messages = _textFile.text.Split(new string[] { "\r\n\r\n", "\r\r", "\n\n" }, System.StringSplitOptions.None);
    }


    private void Update()
    {
        if (_isActive)
        {
            if (Input.GetButtonDown("Submit"))
            {
                _messageIndex++;
                if (_messages.Length <= _messageIndex)
                {
                    _isActive = false;
                    _text.gameObject.SetActive(false);
                }
                else
                {
                    _text.text = _messages[_messageIndex];
                }
                  
            }
        }
       
    }
}
