using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ZoneTrigger))]
public class SignalizationMaker : MonoBehaviour
{
    [SerializeField] private AudioClip _signalizationClip;

    [SerializeField] private ZoneTrigger _zoneTrigger;

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
        _zoneTrigger.PlayerEntered += IncreaseVolume;
        _zoneTrigger.PlayerLeft += DecreaseVolume;
    }

    private void Start()
    {
        if (TryGetComponent<AudioSource>(out _audioSource) == false)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _audioSource.clip = _signalizationClip;
        _audioSource.loop = true;
        _audioSource.volume = _minVolume;
    }

    private void OnDisable()
    {
        _zoneTrigger.PlayerEntered -= IncreaseVolume;
        _zoneTrigger.PlayerLeft -= DecreaseVolume;
    }

    private IEnumerator ChangeVolumeCoroutine(float duration, float endVolume)
    {
        float currentVolume = _audioSource.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            _audioSource.volume = Mathf.MoveTowards(currentVolume, endVolume, time / duration);
            
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
        StopVolumeCoroutine();

        _audioSource.Play();
        _currentVolumeCoroutine = StartCoroutine(ChangeVolumeCoroutine(_durationToChangeVolume, _maxVolume));
    }

    private void DecreaseVolume()
    {
        StopVolumeCoroutine();

        _currentVolumeCoroutine = StartCoroutine(ChangeVolumeCoroutine(_durationToChangeVolume, _minVolume));
    }

    private void StopVolumeCoroutine()
    {
        if (_currentVolumeCoroutine != null)
        {
            StopCoroutine(_currentVolumeCoroutine);
        }
    }
}