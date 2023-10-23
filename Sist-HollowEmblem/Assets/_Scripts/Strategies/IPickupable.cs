using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IPickupable
{
    void OnPickup(Collider2D player);
}

