using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _backgrounds = new List<GameObject>();

    [SerializeField] private ObjectPoolBack _backgroundPool;
    [SerializeField] private float _scrollSpeed = 1.0f;
    [SerializeField] private Transform _spawnPoint;

    private List<GameObject> _activeBackgrounds = new List<GameObject>();

    private void Awake()
    {
        _backgroundPool.SetPrefab(_backgrounds[PlayerPrefs.GetInt("Background", 0)]);

        for (int i = 0; i < 3; i++)
        {
            GameObject background = _backgroundPool.GetObject();
            background.transform.position = _spawnPoint.position + new Vector3(0, i * 10.19f, 0);
            _activeBackgrounds.Add(background);
        }
    }

    private void Update()
    {
        MoveBackgrounds();
    }

    private void MoveBackgrounds()
    {
        for (int i = _activeBackgrounds.Count - 1; i >= 0; i--)
        {
            var background = _activeBackgrounds[i];
            background.transform.Translate(Vector3.down * _scrollSpeed * Time.deltaTime);

            if (background.transform.position.y < -12.19f)
            {
                var lastBackground = _activeBackgrounds[_activeBackgrounds.Count - 1];
                background.transform.position = new Vector3(background.transform.position.x, lastBackground.transform.position.y + 10.19f, background.transform.position.z);

                _activeBackgrounds.RemoveAt(i);
                _activeBackgrounds.Add(background);
            }
        }
    }


    public void SetScrollSpeed(float speed)
    {
        _scrollSpeed = speed;
    }
}