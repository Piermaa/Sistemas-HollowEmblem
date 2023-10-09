using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class RangedIa : MonoBehaviour
{

    Transform player;
    public bool playerToLeft;
    [SerializeField] Transform aimTransform;

    void Start()
    {
 
        player = FindObjectOfType<CharacterController2D>().transform;
    }

    void Update()
    {
        if (GetPlayerPosition() < 0)
        {
            playerToLeft = true;
        }
        else
        {
            playerToLeft = false;
        }

        if (playerToLeft)
        {
            Flip();
            aimTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else
        {
            Flip();
            aimTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
       
    }

    float GetPlayerPosition()
    {
        float diff = Mathf.Abs(player.transform.position.x) - Mathf.Abs(transform.position.x);
        if(diff>0)
        {
            return 1;
        }
        return -1;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x = GetPlayerPosition();
        transform.localScale = theScale;
    }
}
