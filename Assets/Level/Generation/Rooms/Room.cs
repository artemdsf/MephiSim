using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public enum RoomType
{
	LibRoom,
	BossRoom,
	StartRoom,
	EnemyRoom,
	Hallway
}

public class Room
{
	public Room(Vector3Int position, RoomType roomType, int distance)
	{
		Position = position;
		DistanceFromStart = distance;
		RoomType = roomType;

		LevelInfo.Map.Add(this);
	}

	public Light2D Light;

	public List<GameObject> Content = new List<GameObject>();
	
	public Vector3 CenterPos => LevelInfo.GetChunkCenter(Position);

	public Vector3Int Position;

	public RoomType RoomType;

	public bool IsLast = true;

	public bool IsVisited = false;

	public int DistanceFromStart { get; }

	public void ActivateRoom()
	{
		FindObject(Tag.Enemies)?.SetActive(true);

		if (FindObject(Tag.Lights) != null)
		{
			Light2D lightInstance = Object.Instantiate(Light.gameObject, CenterPos, Quaternion.identity, FindObject(Tag.Lights).transform).GetComponent<Light2D>();
			lightInstance.transform.localScale = new Vector3(LevelInfo.ChunkSize.x + 0.7f, LevelInfo.ChunkSize.y + 3f, 1);
		}

		IsVisited = true;
	}

	private GameObject FindObject(Tag tag)
	{
		return Content.Find(item => item.tag == GameManager.TagsDictionary[tag]);
	}
}