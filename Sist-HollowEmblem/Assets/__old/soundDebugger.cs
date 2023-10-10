using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundDebugger : MonoBehaviour
{
    public List<AudioSource> audioSources = new List<AudioSource>();
    // Start is called before the first frame update
    void Start()
    {
        var ass = FindObjectsOfType<AudioSource>();

        foreach (AudioSource aas in ass)
        {
            if(aas.playOnAwake)
            {
                audioSources.Add(aas);
            }
        }
    }

 
}
