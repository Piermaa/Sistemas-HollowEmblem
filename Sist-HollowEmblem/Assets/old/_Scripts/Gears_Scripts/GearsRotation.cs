using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearsRotation : MonoBehaviour
{
    [SerializeField] GameObject Gear;

    void Update()
    {
        GearRotation();
    }

    public void GearRotation()
    {
        float z = -1.5f;

        Vector3 rotation = new Vector3 (0, 0, z);

        Gear.transform.Rotate(rotation);
    }
}
