using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource jump;
    public AudioSource dash;
    public AudioSource attack;
    public AudioSource takeDmg;
    public AudioSource heal;
    public AudioSource doubleJump;
    public AudioSource slam;
    public AudioSource slime;
    public AudioSource inflictdDmg;
    public AudioSource step;
    public AudioSource powerUp;
    public AudioSource die;
    public AudioSource shoot;
    public AudioSource reload;
    public AudioSource pickupable;

    public AudioClip[] steps;
    public void PlaySound(AudioSource sound)
    {
        if (!sound.isPlaying)
        {
            sound.Play();
        }
    }
    public void PlaySound(AudioSource sound, bool canRepeat)
    {
        
         sound.PlayOneShot(sound.clip);
        
    }

    public void PlayStep()
    {
          step.PlayOneShot(steps[Random.Range(0, steps.Length - 1)]);
    }
}
