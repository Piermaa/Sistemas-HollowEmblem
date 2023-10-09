using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSounds : MonoBehaviour
{

    private AudioSource audioSource;
    [SerializeField] AudioClip[] sounds;
    public float minTime, maxTime;
    public float cooldown;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        CooldownUpdate();
    }

    void CooldownUpdate()
    {
        if (!ChangeCameraPosition.bossIsActive &&!audioSource.isPlaying)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0)
            {
                cooldown = Random.Range(minTime,maxTime);

                audioSource.clip = sounds[Random.Range(0, sounds.Length+1)];
                audioSource.Play();
            }
        }
    }
}
