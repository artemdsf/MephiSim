using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct Task_t
{
    public string question;
    public int answer;
}

public class TaskScript : MonoBehaviour
{
    //Test
    [SerializeField] private string _testQuestion;
    [SerializeField] private int _testAnswer;

    [SerializeField] private Text _question;
    [SerializeField] private Text _input;

    [SerializeField] private Spawner _spawner;

    private string _currentAnswer = "";
    private Task_t _newTask;

    private string _numbers = "0123456789";

    private void Start()
    {
        _newTask = GetTask();

        _question.text = _newTask.question;
    }

    private void Update()
    {
        _currentAnswer = GetInput(_currentAnswer);

        _input.text = _currentAnswer;

        CheckAnswer();
    }

    private Task_t GetTask()
    {
        Task_t task = new Task_t();

        task.question = _testQuestion;
        task.answer = _testAnswer;

        return task;
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

    private void CheckAnswer()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_currentAnswer == _newTask.answer.ToString())
            {
                Debug.Log("Correct");
                gameObject.SetActive(false);
                _spawner.Spawn();
            }
            else
            {
                Debug.Log("Incorrect");
            }
        }
    }
}