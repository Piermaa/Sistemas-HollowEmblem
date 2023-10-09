using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyed : MonoBehaviour
{
    Animator animator;
    public GameObject minimapIcon;
    void Start()
    {
        animator = GetComponent<Animator>();
        minimapIcon.SetActive(true);
    }

    
    public void Destruction()
    {
        animator.SetTrigger("Destroy");
        minimapIcon.SetActive(false);
    }
}
