using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffSpriteMiniMap : MonoBehaviour
{
    public BoxCollider2D col;

    public GameObject miniMapIcon;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        TurnOff();
    }

    public void TurnOff()
    {
        if (col.enabled == false)
        {
            miniMapIcon.SetActive(false);
        }

        else
        {
            miniMapIcon.SetActive(true);
        }
    }
}
