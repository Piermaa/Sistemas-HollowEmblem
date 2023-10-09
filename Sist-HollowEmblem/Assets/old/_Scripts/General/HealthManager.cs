using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] HealthController playerHealth;

    [Header("CommonToAllHealthControllers")]
    public Material takingDamageMaterial;
    public GameObject takingDamageParticlesPrefab;
    private void Start()
    {
        var healths =FindObjectsOfType<HealthController>();

        foreach(HealthController h in healths)
        {
            h.takingDamageMaterial = h.takingDamageMaterial==null ? takingDamageMaterial: h.takingDamageMaterial; //solo cambia si es nulo ;)

            if (h.takeDamageParticles==null)
            {
                var part=Instantiate(takingDamageParticlesPrefab, h.gameObject.transform);
                h.takeDamageParticles = part.GetComponentInChildren<ParticleSystem>();
            }
        }
    }
    IEnumerator BecameInvinsible()
    {
        playerHealth.inmune = true;
        yield return new WaitForSecondsRealtime(0.9f);
        playerHealth.inmune = false;
        yield break;
    }

    public void SetInmunity()
    {
        StartCoroutine(BecameInvinsible());
    }
}
