using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerAbility", menuName = "Abilities/Dash", order = 0)]
public class Dash : PlayerAbility
{
    #region Class Properties

    #region Serialized Properties

    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashTime;

    #endregion
    
    private float _dashTimeTimer = 0;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    
    #endregion
    
    #region IPlayerAbility Method Overrides

    public override void Init(GameObject playerGameObject, AudioSource audioSource)
    {
        base.Init(playerGameObject, audioSource);
        _rigidbody2D = _playerGameObject.GetComponent<Rigidbody2D>();
        _animator = _playerGameObject.GetComponent<Animator>();
    }

    public override void Use()
    {
        if (CanBeUsed())
        {
            _coolDownTimer = _cooldown;
            
            _rigidbody2D.velocity = Vector2.zero;
            AbilityAudioSource.PlayOneShot(_abilitySound);
      
            _animator.SetBool("Jump", false);
            _animator.SetTrigger("Dash");
            
            Vector3 theScale = _playerGameObject.transform.localScale;
            _rigidbody2D.AddForce(new Vector2(-(theScale.x / (Mathf.Abs(theScale.x))), 0) * _dashSpeed, ForceMode2D.Impulse);
            
            _dashTimeTimer = _dashTime;
            _playerGameObject.layer = 9;
        }    
    }

    public override void AbilityUpdate()
    {
        base.AbilityUpdate();
  
        
        if (_dashTimeTimer <= 0)
        {
            _playerGameObject.layer = 3;
        }
        else
        {
            _dashTimeTimer -= Time.deltaTime;
            
            if (_dashTimeTimer < 0)
            {
                _dashTimeTimer = 0;
            }

            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0); // evita que el jugador se mueva en Y al dashear
        }
    }

    #endregion
}
