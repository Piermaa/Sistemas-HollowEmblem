using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    public Transform playerTargetTransform;
    public PlayerMovement playerMovement;
    public Rigidbody2D rb;
    Vector3 startPos;
    Transform newTarget;
    bool mustAim;
    // Start is called before the first frame update
    void Start()
    {
        playerTargetTransform = GameObject.Find("PlayerTarget").transform;
        vcam = GetComponent<CinemachineVirtualCamera>();
        startPos = playerTargetTransform.position;
        vcam.Follow = playerTargetTransform;
    }

    // Update is called once per frame
    void Update()
    {
        if (mustAim)
        {
            StartCoroutine(Awaiting());
        }
        if(!mustAim && playerTargetTransform.localPosition != Vector3.zero)
        {
            playerTargetTransform.localPosition = Vector3.zero;
        }
    }

    public void ChangeTarget(Transform targetToAim)
    {
        playerMovement.gameObject.TryGetComponent<Animator>(out var anim);
        anim.SetBool("isWalking", false);
        anim.SetTrigger("Idle");
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        playerMovement.enabled = false;
        newTarget = targetToAim;
        mustAim=true;
    }

    private IEnumerator Awaiting()
    {
    
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            playerTargetTransform.position = Vector2.MoveTowards(playerTargetTransform.position, newTarget.position, i*1/20);
            yield return null;
        }
        Debug.Log("Esperando");

        yield return new WaitForSeconds(3);
        mustAim = false;
        playerTargetTransform.localPosition = Vector3.zero;
        playerMovement.enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
