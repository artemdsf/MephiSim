using UnityEngine;
using UnityEngine.UI;

public class TitleAnim : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Text _text;

    private float _color;

    private void Update()
    {
        _color += Time.deltaTime * _speed * 0.01f;

		if (_color > 1)
		{
            _color = 0;
        }

        _text.color = Color.HSVToRGB(_color, 1, 1);
    }
}
