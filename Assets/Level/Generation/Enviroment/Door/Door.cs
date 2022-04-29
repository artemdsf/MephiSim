using UnityEditor.Animations;
using UnityEngine;

public class Door : MonoBehaviour
{
	private Animator _animator;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Open();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Close();
		}
	}

	private void Open()
	{
		_animator.SetBool("Open", true);
	}

	private void Close()
	{
		_animator.SetBool("Open", false);
	}

	private void AddCollider()
	{
		Collider2D[] collider2D = GetComponents<Collider2D>();
		foreach (var collider in collider2D)
		{
			if (collider.isTrigger == false)
			{
				collider.enabled = true;
			}
		}
	}

	private void RemoveCollider()
	{
		Collider2D[] collider2D = GetComponents<Collider2D>();
		foreach (var collider in collider2D)
		{
			if (collider.isTrigger == false)
			{
				collider.enabled = false;
			}
		}
	}
}
