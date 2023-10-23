using UnityEngine;

public class Pickupable : MonoBehaviour, IPickupable
{
    private const string PLAYER_TAG = "Player";
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER_TAG)&& gameObject.activeInHierarchy)
        {
            OnPickup(other);
        }
    }

    public virtual void OnPickup(Collider2D player)
    {
       gameObject.SetActive(false);
    }
    
}
