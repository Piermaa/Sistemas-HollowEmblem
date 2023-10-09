using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulcanEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    ObjectPooler objectPooler;
    public string[] ammunition;
    public float fireRate = 10;
    float fireCooldown;
    public Transform spawnPos;
    Quaternion invertedRotation;
    Quaternion rotation;

    Animator animator;
    [SerializeField] AudioSource shootsound;
    [SerializeField] ParticleSystem[] shootParticles;
    private void Start()
    {
        animator = GetComponent<Animator>();
        objectPooler = ObjectPooler.Instance;
        fireCooldown = fireRate;

        rotation = Quaternion.Euler(new Vector3(0,0,0));
        invertedRotation = Quaternion.Euler(new Vector3(0, 0, -1));
    }
    IEnumerator Shot()
    {
      
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(1f);
        shootsound.Play();
        shootParticles[0].Play();
        objectPooler.SpawnFromPool(ammunition[1], spawnPos.position, rotation); //Far Shots
        objectPooler.SpawnFromPool(ammunition[1], spawnPos.position, invertedRotation);
        yield return new WaitForSeconds(0.2f);
        shootsound.Play();
        shootParticles[1].Play();
        objectPooler.SpawnFromPool(ammunition[0], spawnPos.position, rotation); //Near shots
        objectPooler.SpawnFromPool(ammunition[0], spawnPos.position, invertedRotation);
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown < 0)
        {
            fireCooldown = fireRate;
            StartCoroutine(Shot());
        }
    }
}
