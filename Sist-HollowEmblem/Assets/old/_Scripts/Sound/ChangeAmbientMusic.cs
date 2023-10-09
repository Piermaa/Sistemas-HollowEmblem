using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAmbientMusic : MonoBehaviour
{
    [SerializeField] GameObject[] bosses;

    public AudioSource audioSource;
    bool mustMute;
    [SerializeField] AudioClip ambientClip;
    [SerializeField] AudioClip spiderBossFightMusic;
    [SerializeField] AudioClip slamBossFightMusic;
    [SerializeField] AudioClip finalBossFightMusic;
    float defaultVolume;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        defaultVolume = audioSource.volume;
    }

    private void Update()
    {
        mustMute = Input.GetKeyDown(KeyCode.K);


        if (mustMute)
        {
            StartCoroutine(Silencing());       
        }
    }


    public void ChangeSong()
    {
        if (ChangeCameraPosition.bossIsActive)
        {
            if (bosses[0] != null)
            {
                if (bosses[0].activeInHierarchy)
                {
                    audioSource.clip = spiderBossFightMusic;
                    audioSource.Play();
                    print("boss1plays");
                }


            }

            if (bosses[1] != null)
            {
                if (bosses[1].activeInHierarchy)
                {
                    audioSource.clip = slamBossFightMusic;
                    audioSource.Play();
                }
            }

            if (bosses[2] != null)
            {
                if (bosses[2].activeInHierarchy)
                {
                    audioSource.clip = finalBossFightMusic;
                    audioSource.Play();
                }
            }
        }

        else
        {
            audioSource.clip = ambientClip;
            audioSource.Play();
        }

        audioSource.volume = defaultVolume;
    }

    public void SetSilence()
    {
        mustMute = true;
    }
    IEnumerator Silencing()
    {
       
            // Colocar el audio del ascensor

            for (float i = audioSource.volume; i >= 0; i -= Time.deltaTime/25)
            {
                
                    audioSource.volume = i;
                

                yield return null;
            }
            audioSource.Stop();
            mustMute = false;
        
    }
}
