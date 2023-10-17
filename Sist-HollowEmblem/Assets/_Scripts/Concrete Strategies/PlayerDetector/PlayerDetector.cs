using UnityEngine;

public class PlayerDetector : MonoBehaviour, IPlayerDetector
{
    #region Class Properties

    private const string PLAYER_TAG = "Player";
    protected Transform _playerTransform;

    #endregion

    #region Monobehaviour Callbacks

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            if (_playerTransform == null)
                _playerTransform = other.transform;
            
            OnPlayerDetect();
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            OnPlayerLoose();
        }
    }
    
    #endregion
   
    #region IPlayerDetector Methods

    public virtual void OnPlayerDetect()
    {
        
    }
    public virtual void OnPlayerLoose()
    {
        
    }

    #endregion
  
}