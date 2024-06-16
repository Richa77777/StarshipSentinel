using TMPro;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int _healthPoints = 0;
    [SerializeField] private TextMeshProUGUI _healthText;

    [SerializeField] private AudioClip _clip;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _healthText.text = _healthPoints.ToString();
    }

    public void SubtractHealth(int value)
    {
        _audioSource.clip = _clip;
        _audioSource.Play();

        _healthPoints = Mathf.Clamp(_healthPoints - value, 0, int.MaxValue);
        _healthText.text = _healthPoints.ToString();

        if (_healthPoints == 0)
        {
            GameController.Instance.Lose();
        }
    }
}