using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private AudioClip _clip;

    private AudioSource _audioSource;
    private int _score = 0;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _scoreText.text = _score.ToString();
    }

    public void AddScore(int value)
    {
        _audioSource.PlayOneShot(_clip);

        _score = Mathf.Clamp(_score + value, 0, int.MaxValue);
        _scoreText.text = _score.ToString();

        if (_score > PlayerPrefs.GetInt("Record", 0))
        {
            PlayerPrefs.SetInt("Record", _score);
        }
    }
}
