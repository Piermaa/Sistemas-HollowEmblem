using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlockerPickupable : Pickupable
{
    [SerializeField] private PlayerAbility _abilityToUnlock;

    public override void OnPickup(Collider2D player)
    {
        player.gameObject.GetComponent<Player>().UnlockAbility(_abilityToUnlock);
        base.OnPickup(player);
    }
}
