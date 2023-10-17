using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class VulcanBulletExplosion : MonoBehaviour, IPooledObject
{
    [FormerlySerializedAs("particles")]  [SerializeField] private ParticleSystem _particles;
    private float _timer;

    public void OnObjectSpawn()
    {
        _timer = 1;
        Explode();        
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer<0)
        {
            gameObject.SetActive(false);
        }
    }

    private void Explode()
    {
        _particles.Play();
    }
}
