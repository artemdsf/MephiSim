using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TasksManager : MonoBehaviour
{
	[SerializeField] private GameObject _background;
    [SerializeField] private Image _question;
    [SerializeField] private Text _input;

    private string _currentAnswer = "";
    private Task _newTask;

    private string _numbers = "-0.123456789";

	public static TasksManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(this);
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);

		Deactivate();
	}

	public void NewTask()
    {
		Activate();
		_newTask = TasksDatabase.Instance.GetTask();

		Debug.Log(_newTask.Answer);

		_question.sprite = _newTask.TaskSprite;
		StartCoroutine(GiveTask());
	}

	private IEnumerator GiveTask()
	{
		GameManager.IsPaused = true;
		while (true)
		{
			_currentAnswer = GetInput(_currentAnswer);

			_input.text = _currentAnswer;

			if (CorrectAnswer())
			{
				break;
			}

			yield return null;
		}

		Deactivate();
		GameManager.IsPaused = false;

		yield return null;
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

    private bool CorrectAnswer()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_currentAnswer == _newTask.Answer)
            {
				return true;
            }
            else
            {
				return false;
            }
        }
		return false;
	}

	private void Activate()
	{
		_background.SetActive(true);
		_currentAnswer = "";
		Time.timeScale = 0;
	}

	private void Deactivate()
	{
		_background.SetActive(false);
		_currentAnswer = "";
		Time.timeScale = 1;
	}
}