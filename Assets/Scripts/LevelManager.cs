using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    public enum TasksDiffcultyEnum
    {
        NoTasks,
        Easy,
        Medium,
        Hard
    };

    public delegate void TasksDifficultyStateChanged(TasksDiffcultyEnum tasksDiffculty);
    public static event TasksDifficultyStateChanged TaskDifficultyChanged;
    
    private static TasksDiffcultyEnum _tasksDiffculty = TasksDiffcultyEnum.NoTasks;

    public static TasksDiffcultyEnum TasksDiffculty
    {
        get { return _tasksDiffculty; }
        set
        {
            TaskDifficultyChanged(value);
            _tasksDiffculty = value;
        }
    }
}
