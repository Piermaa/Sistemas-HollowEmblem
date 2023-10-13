using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerNO : MonoBehaviour
{
    [SerializeField] GameObject victorySprite;
    [SerializeField] GameObject defeatSprite;
    public GameObject abUnlockerPrefab;

    public Animator animator;

    public static GameManagerNO instance = null;

    public static GameManagerNO Instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
          
        }    //DontDestroyOnLoad(gameObject);
        //}

        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    public void Start()
    {
        Resume();
    }

    public void StopTime()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void StartVictory()
    {
        animator.SetTrigger("Victory");
    }

    public void StartVictory(Vector3 position, string AbilityName)
    {
        if(animator!=null)
        animator.SetTrigger("Victory");

        var auO= Instantiate(abUnlockerPrefab,position,Quaternion.identity);
        auO.TryGetComponent<AbilityUnlocker>(out var au);
        au.unlockedAb = AbilityName;
    }

    public void StartDefeat()
    {
        animator.SetTrigger("Defeat");
    }

    public void GameOver()
    {
        
    }
}