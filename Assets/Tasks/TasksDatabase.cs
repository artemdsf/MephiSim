using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public readonly Sprite TaskSprite;
    public readonly string Answer;
    public Task(Sprite taskSprite, string answer)
    {
        TaskSprite = taskSprite;
        Answer = answer;
    }
}

public class TasksDatabase : MonoBehaviour
{
    [SerializeField] private TextAsset _text;
    [SerializeField] string _folderWithTasks;

    private List<Task> _easyTasks;
    private List<Task> _mediumTasks;
    private List<Task> _hardTasks;

    List<Task>[] _tasksLists;


    private static TasksDatabase _instance;

    public static TasksDatabase Instance { 
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
         if (_instance != null)
        {
            Destroy(this);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        LoadDatabase();
        LevelManager.NextLevel();
    }

    public void LoadDatabase()
    {
        _easyTasks = new List<Task>();
        _mediumTasks = new List<Task>();
        _hardTasks = new List<Task>();

        _tasksLists = new List<Task>[] { _easyTasks, _mediumTasks, _hardTasks };

        string[] strings;
        strings = _text.text.Split(new string[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
        
        foreach(string str in strings)
        {
            string[] questionSpriteAndAnswer = str.Split(',');
            string taskSpriteName = questionSpriteAndAnswer[0];
            string answer = questionSpriteAndAnswer[1]; 

            string[] taskProperties = taskSpriteName.Split('_');
            int level = int.Parse(taskProperties[1]);

            if (level == LevelManager.Level)
            {
                Sprite sprite = Resources.Load<Sprite>(_folderWithTasks + '/' + taskSpriteName);
                Task task = new Task(sprite, answer);

                int diffculty = int.Parse(taskProperties[2]);
                _tasksLists[diffculty - 1].Add(task);
            }
        }
        Debug.Log("Tasks are loaded", this);
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
