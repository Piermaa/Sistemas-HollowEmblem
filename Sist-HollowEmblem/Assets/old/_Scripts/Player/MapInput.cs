using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShowStates { SHOWINGINVENTORY, SHOWINGMAP, HIDING}

public class MapInput : MonoBehaviour
{
    public static MapInput Cortana;

    public GameObject map;
    public GameObject inventory;

    public GameObject miniMap;
    public GameObject secondMiniMapPosition;

    public GameObject cameraNoZoom;
    public GameObject cameraWithZoom;

    GameObject auxiliar;

    private Animator animator;
    public Animator minimapAnimator;

    public Rigidbody2D rb;

    public ShowStates state;

    public bool canInput;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Cortana = this;
    }

    private void Start()
    {
        canInput = true;
        auxiliar = map;
        state = ShowStates.HIDING;
    }

    void Update()
    {
        Show();
        AppearSomething();
    }

    void AppearSomething()
    {   
        
        if (state == ShowStates.HIDING)
        {

            if (Input.GetKeyDown(KeyCode.M) && canInput)
            {
                
                 state = ShowStates.SHOWINGMAP;
                 animator.SetBool("ShowMap", true);
                
            }

            if (Input.GetKeyDown(KeyCode.I) && canInput)
            { 
                 state = ShowStates.SHOWINGINVENTORY;
                 animator.SetBool("ShowMap", true);
            }

            if (Input.GetKeyDown(KeyCode.Tab) && canInput)
            {
                if (auxiliar == map)
                {
                    state = ShowStates.SHOWINGMAP;
                    animator.SetBool("ShowMap", true);
                }

                if (auxiliar == inventory)
                {
                    state = ShowStates.SHOWINGINVENTORY;
                    animator.SetBool("ShowMap", true);
                }
            }
        }

        else if (state == ShowStates.SHOWINGMAP)
        {
            rb.velocity = Vector2.zero;
            
            if ((Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Tab)) && canInput)
            {
                animator.SetBool("ShowMap", false);
                animator.SetTrigger("Disappear");
                HideAuxiliar();
                state = ShowStates.HIDING;
                auxiliar = map;
            }

            else if (Input.GetKeyDown(KeyCode.I) && canInput)
            {
                state = ShowStates.SHOWINGINVENTORY;
            }
        }

        else if (state == ShowStates.SHOWINGINVENTORY)
        {
            rb.velocity = Vector2.zero;

            if ((Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab)) && canInput)
            {
                animator.SetBool("ShowMap", false);
                animator.SetTrigger("Disappear");
                HideAuxiliar();
                state = ShowStates.HIDING;
                auxiliar = inventory;
            }

            else if (Input.GetKeyDown(KeyCode.M) && canInput)
            {
                state = ShowStates.SHOWINGMAP;
            }
        }
    }

    void Show()
    {
        switch (state)
        {
            case ShowStates.SHOWINGMAP:
                map.SetActive(true);
                inventory.SetActive(false);
                ChangeThingsMiniMap();
                auxiliar = map;
                break;

            case ShowStates.SHOWINGINVENTORY:
                map.SetActive(false);
                inventory.SetActive(true);
                ShowNoMap();
                auxiliar = inventory;
                break;

            case ShowStates.HIDING:
                ShowNoMap();
                ShowMiniMap();
                HideAuxiliar();
                break;
        }
    }

    public void PutMap()
    {
        state = ShowStates.SHOWINGMAP;
    }

    public void PutInventory()
    {
        state = ShowStates.SHOWINGINVENTORY;
    }

    public void CanInput()
    {
        canInput = true;
    }

    public void CanNotInput()
    {
        canInput = false;
    }

    public void AppearMinimap()
    {
       miniMap.SetActive(true);
    }

    public void DissappearMiniMap()
    {
        miniMap.SetActive(false);
    }

    public void ChangeThingsMiniMap()
    {
        miniMap.SetActive(false);
        secondMiniMapPosition.SetActive(true);

        cameraNoZoom.SetActive(false);
        cameraWithZoom.SetActive(true);
    }

    public void ShowMiniMap()
    {
        miniMap.SetActive(true);
        secondMiniMapPosition.SetActive(false);

        cameraNoZoom.SetActive(true);
        cameraWithZoom.SetActive(false);
    }

    public void ShowNoMap()
    {
        miniMap.SetActive(false);
        secondMiniMapPosition.SetActive(false);

        cameraNoZoom.SetActive(false);
        cameraWithZoom.SetActive(false);
    }

    void HideAuxiliar()
    {
        auxiliar.SetActive(false);
    }
}