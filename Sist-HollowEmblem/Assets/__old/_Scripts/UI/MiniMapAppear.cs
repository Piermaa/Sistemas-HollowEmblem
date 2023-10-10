using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapAppear : MonoBehaviour
{
    public GameObject miniMap;
    void Start()
    {
        miniMap.SetActive(true);
    }

}
