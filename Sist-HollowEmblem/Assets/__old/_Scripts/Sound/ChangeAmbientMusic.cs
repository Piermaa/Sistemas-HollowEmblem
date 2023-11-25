using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAmbientMusic : MonoBehaviour
{
    [SerializeField] GameObject[] bosses;

    private AudioSource _audioSource;
    bool mustMute;
    [SerializeField] AudioClip ambientClip;
    float defaultVolume;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        defaultVolume = _audioSource.volume;
    }

    private void Update()
    {
        mustMute = Input.GetKeyDown(KeyCode.K);


        if (mustMute)
        {
            StartCoroutine(Silencing());
        }
    }


    public void SetAmbienceMusic()
    {
        _audioSource.clip = ambientClip;
        _audioSource.volume = defaultVolume;
    }

    public void SetSilence()
    {
        mustMute = true;
    }

    IEnumerator Silencing()
    {
        for (float i = _audioSource.volume; i >= 0; i -= Time.deltaTime / 25)
        {

            _audioSource.volume = i;


            yield return null;
        }

        _audioSource.Stop();
        mustMute = false;

    }
}
