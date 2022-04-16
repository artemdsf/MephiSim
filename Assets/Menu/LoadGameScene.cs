using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour
{
	public float LoadProgress { get; private set; }

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
		operation = SceneManager.LoadSceneAsync(1);

		while (operation.isDone == false)
		{
			LoadProgress = operation.progress;
			yield return null;
		}
	}
}
