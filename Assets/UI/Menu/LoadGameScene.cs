using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour
{
	public float LoadProgress { get; private set; }
	[SerializeField] private int sceneNumber = 1;

	private AsyncOperation operation;

	public void LoadScene()
	{
		StartCoroutine(StartLoad());
	}

	public void AllowSceneActivation(bool canBeActive)
	{
		operation.allowSceneActivation = canBeActive;
	}

	private IEnumerator StartLoad()
	{
		operation = SceneManager.LoadSceneAsync(sceneNumber);

		while (operation.isDone == false)
		{
			LoadProgress = operation.progress;
			yield return null;
		}
	}
}
