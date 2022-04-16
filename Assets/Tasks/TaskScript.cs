using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TaskScript : MonoBehaviour
{
    //Test
    [SerializeField] private Image _question;
    [SerializeField] private Text _input;

    private string _currentAnswer = "";
    private Task _newTask;

    private string _numbers = "0123456789.";

    private bool _answered = false;

    private void Start()
    {
        NewTask();   
    }

    private void Update()
    {
        _currentAnswer = GetInput(_currentAnswer);

        _input.text = _currentAnswer;

        CheckAnswer();
    }

    private void OnEnable()
    { 
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }


    private string GetInput(string answer)
    { 
        for (int i = 0; i < _numbers.Length; i++)
        {
            string current = _numbers[i].ToString();
            if (Input.GetKeyDown(current))
            {
                answer += current;
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace) && answer.Length > 0)
        {
            answer = answer.Remove(answer.Length - 1, 1);
        }

        return answer;
    }

    public bool IsAnswerGivenAndCorrect()
    {
        return _answered;
    }

    public void NewTask()
    {
        _newTask = TasksDatabase.Instance.GetTask();
        _question.sprite = _newTask.TaskSprite;
    }

    private void CheckAnswer()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !_answered)
        {
            Debug.Log(_newTask.Answer);
            if (_currentAnswer == _newTask.Answer)
            {
                Debug.Log("Correct");
                _answered = true;
            }
            else
            {
                Debug.Log("Incorrect");
            }
        }

    }
}