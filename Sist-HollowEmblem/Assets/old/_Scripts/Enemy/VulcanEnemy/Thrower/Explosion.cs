using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IPooledObject
{
    float timer;
    [SerializeField] ParticleSystem particles;
  
    public void OnObjectSpawn()
    {
        timer = 1;
        Explode();        
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer<0)
        {
            gameObject.SetActive(false);
        }
    }

    void Explode()
    {
        particles.Play();
    }
}
