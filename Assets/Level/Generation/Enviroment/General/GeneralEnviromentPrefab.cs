using UnityEngine;

public class Board
{ 
    public Board(GameObject gameObject, GeneralEnviromentInfo info)
	{
        gameObject.GetComponent<SpriteRenderer>().sprite = info.Sprite;
	}
}

public class GeneralEnviromentPrefab : MonoBehaviour
{
    [SerializeField] private GeneralEnviromentContainer _container;
    [SerializeField] private bool _randomSprite = false;
    [Min(0)]
    [SerializeField] private int _boardNum = -1;

    private void OnValidate()
    {
		if (_randomSprite)
		{
            new Board(gameObject, _container.GetItem(_container.Items));
        }
		else
		{
            new Board(gameObject, _container.GetItem(_container.Items, _boardNum));
        }
    }    
}
