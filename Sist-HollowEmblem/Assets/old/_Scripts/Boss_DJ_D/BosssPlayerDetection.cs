using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BosssPlayerDetection : MonoBehaviour
{
    [Tooltip("Left 0 ,Middle 1,Right 2")]
    public int position; 

    [SerializeField]DashBossController boss;
    private void Start()
    {
   
    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch(position)
            {
                case 0:
                    boss.playerPos = DashBossController.PlayerPos.Left;
                    break;
                case 1:
                    boss.playerPos = DashBossController.PlayerPos.Middle;
                    break;
                case 2:
                    boss.playerPos = DashBossController.PlayerPos.Right;
                    break;
            }
        }
    }
}
