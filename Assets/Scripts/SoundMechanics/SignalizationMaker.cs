using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(ZoneTrigger))]
public class SignalizationMaker : MonoBehaviour
{
    [SerializeField] private AudioClip _signalizationClip;

    [SerializeField]
    [Range(0f, 1f)] private float _minVolume = 0f;

    [SerializeField]
    [Range(0f, 1f)] private float _maxVolume = 1f;

    [SerializeField]
    [Range(0f, 1f)] private float _deltaVolume = 0.1f;

    [SerializeField] private float _durationToChangeVolume = 5f;

    private AudioSource _audioSource;
    private Coroutine _currentVolumeCoroutine;

    private void OnEnable()
    {
        ZoneTrigger.PlayerEntered += IncreaseVolume;
        ZoneTrigger.PlayerLeft += DecreaseVolume;
    }

    private void Start()
    {
        if (!TryGetComponent(out _audioSource))
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _audioSource.clip = _signalizationClip;
        _audioSource.loop = true;
        _audioSource.volume = _minVolume;
    }

    private void OnDisable()
    {
        ZoneTrigger.PlayerEntered -= IncreaseVolume;
        ZoneTrigger.PlayerLeft -= DecreaseVolume;
    }

    private IEnumerator ChangeVolumeCoroutine(float duration, float endVolume)
    {
        float currentVolume = _audioSource.volume;
        float time = 0f;

        while (time < duration)
        {
            _audioSource.volume = Mathf.MoveTowards(currentVolume, endVolume, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        _audioSource.volume = endVolume;

        if(_audioSource.volume == 0 && _audioSource.isPlaying == true)
        {
            _audioSource.Stop();
        }
    }

    private void IncreaseVolume()
    {
        if (_currentVolumeCoroutine != null)
        {
            StopCoroutine(_currentVolumeCoroutine);
        }

        _audioSource.Play();
        _currentVolumeCoroutine = StartCoroutine(ChangeVolumeCoroutine(_durationToChangeVolume, _maxVolume));
    }

    private void DecreaseVolume()
    {
        if (_currentVolumeCoroutine != null)
        {
            StopCoroutine(_currentVolumeCoroutine);
        }

        _currentVolumeCoroutine = StartCoroutine(ChangeVolumeCoroutine(_durationToChangeVolume, _minVolume));
    }
}