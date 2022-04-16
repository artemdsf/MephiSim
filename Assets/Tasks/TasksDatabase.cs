using System.Collections.Generic;
using UnityEngine;

public class Task
{
	public readonly Sprite Question;
	public readonly string Answer;
	public Task(Sprite question, string answer)
	{
		Question = question;
		Answer = answer;
	}
}

public class TasksDatabase : MonoBehaviour
{
	[SerializeField] private TextAsset _text;
	[SerializeField] private string _folderWithTasks;

	private List<Task> _easyTasks = new List<Task>();
	private List<Task> _mediumTasks = new List<Task>();
	private List<Task> _hardTasks = new List<Task>();
	private List<Task>[] _tasksLists;


	private static TasksDatabase _instance;
	public static TasksDatabase Instance => _instance;

	private void Awake()
	{
		if (_instance != null)
		{
			Destroy(this);
		}

		_tasksLists = new List<Task>[] { _easyTasks, _mediumTasks, _hardTasks };
		_instance = this;
		DontDestroyOnLoad(gameObject);
		LoadDatabase();
	}

	private void LoadDatabase()
	{
		string[] strings;
		strings = _text.text.Split(new string[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);

		foreach (string str in strings)
		{
			string[] taskStrings = str.Split(',');
			string questionSpriteName = taskStrings[0];
			string answer = taskStrings[1];

			Sprite sprite = Resources.Load<Sprite>(_folderWithTasks + '/' + questionSpriteName);
			Task task = new Task(sprite, answer);

			int diffculty = int.Parse(questionSpriteName.Split('_')[2]);
			_tasksLists[diffculty - 1].Add(task);
		}
	}

	public Task GetTask()
	{
		if (LevelManager.TasksDiffculty == 0)
		{
			Debug.LogWarning("Attemp to get task when difficulty is set to \"NoTasks\"");
			return null;
		}

		List<Task> tasks = _tasksLists[(int)LevelManager.TasksDiffculty - 1];

		if (tasks == null || tasks.Count == 0)
		{
			Debug.LogWarning($"No tasks for difficulty {LevelManager.TasksDiffculty} was found");
			return null;
		}

		Task task = tasks[Random.Range(0, tasks.Count - 1)];
		tasks.Remove(task);
		return task;
	}
}
