using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamBossController : MonoBehaviour
{
    HealthController health;
    Animator animator;

    int maxHealth;

    [SerializeField]float slamCoolDown;
    float slamTimer;

    public Transform[] positionsTransforms;
    Vector3[] positions= {Vector3.zero, Vector3.zero, Vector3.zero };
    Vector3 newPos = Vector2.zero;

    const string RIGHT="Right";
    const string LEFT="Left";
    const string MIDDLE = "Middle";
    public enum BossPosition
    {
        Left,
        Middle,
        Right
    }
    public BossPosition bossPosition;
    private void Start()
    {
        for (int i=0;i<3 ;i++)
        {
            positions[i] = positionsTransforms[i].position;
        }
        //positions[0] = positionsTransforms[0].position;
        //positions[1] = positionsTransforms[1].position;
        //positions[2] = positionsTransforms[2].position;

        animator = GetComponent<Animator>();
        health = GetComponent<HealthController>();
        maxHealth = health.healthPoints;
    }

    private void Update()
    {
        slamTimer -= Time.deltaTime;
       
        if(slamTimer<0)
        {
            newPos = positions[Slam(DecideSlam())];
            slamTimer = slamCoolDown;
        }
        // Vector2.MoveTowards(transform.position, new Vector2(moveSpots[spotsIndex].transform.position.x, transform.position.y), speed * Time.deltaTime);
        if (newPos != Vector3.zero)
        {
            transform.position = Vector2.MoveTowards(transform.position, newPos, 2 * Time.deltaTime);

        }

        if (Vector2.Distance(transform.position, new Vector2(newPos.x, transform.position.x)) < 0.1f)
        {
            animator.SetTrigger("Slam");
        }

    }

    string DecideSlam()
    {
  
        string finalDirection=null;

        do
        {
            int dir = Random.Range(1, 4);
            switch (dir)
            {
                case 1:
                    finalDirection = LEFT;
                    break;
                case 2:
                    finalDirection = MIDDLE;
                    break;
                case 3:
                    finalDirection = RIGHT;
                    break;
            }
        } while (bossPosition.ToString() == finalDirection);
        print(finalDirection);
        return finalDirection;
    }

    int Slam(string direction)
    {
        //animator.SetTrigger("Slam");
        switch (bossPosition)
        {
            case BossPosition.Left:
                if (direction == RIGHT)
                {
                    return 2;
                    
                }
                else
                {
                    return 1;
                }
            
            case BossPosition.Middle:
                if (direction == RIGHT)
                {
                    return 2;
                }
                else
                {
                    return 0;
                }
             

            case BossPosition.Right:
                if (direction == LEFT)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
             
        }
        return 0;
        
    }

    void Move(int dir)
    {
        transform.Translate(Vector3.MoveTowards(transform.position, positions[dir], 10));
    }
}
