using System.Collections.Generic;

public enum Tag
{
	Player,
	Enviroment,
	Enemies,
    Enemy,
	Lights,
    EnemyBottom,
    PlayerBottom
}

public static class GameManager
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
    
    private static TasksDiffcultyEnum _tasksDiffculty = TasksDiffcultyEnum.Easy;

    public static TasksDiffcultyEnum TasksDiffculty
    {
        get { return _tasksDiffculty; }
        set
        {
            TaskDifficultyChanged(value);
            _tasksDiffculty = value;
        }
    }
	public static int Level { get; private set; } = 1;

	public static bool IsPaused;

	public static void NextLevel()
    {
        Level++;
        TasksDatabase.Instance.LoadDatabase();
    }

	public static Dictionary<Tag, string> TagsDictionary = new Dictionary<Tag, string>
	{
		{ Tag.Player, "Player" },
		{ Tag.Enemies, "Enemies" },
        { Tag.Enemy, "Teacher" },
        { Tag.Enviroment, "Enviroment" },
		{ Tag.Lights, "Lights" },
        { Tag.EnemyBottom, "TeacherBottom" },
        { Tag.PlayerBottom, "PlayerBottom" }
    };
}
