using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAbility", menuName = "Abilities/Slam", order = 1)]
public class Slam : PlayerAbility
{
    #region Class Properties

    #region Serialized Properties

    [SerializeField] private float _slamForce = 5;
    [SerializeField] private float _slamTimer;

    #endregion

    private float _slamTimeTimer = 0;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    #endregion

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
            _animator.SetTrigger("Slam");
            _rigidbody2D.AddForce(Vector2.down * _slamForce, ForceMode2D.Impulse);
            AbilityAudioSource.PlayOneShot(_abilitySound);
            //willDestroy = true;
            _slamTimeTimer = _slamTimer;
        }
    }

    public override void AbilityUpdate()
    {
        base.AbilityUpdate();

        _slamTimeTimer -= Time.deltaTime;

        if (_slamTimeTimer < 0)
        {
            _slamTimeTimer = 0;
        }

        _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y); // evita que el jugador se mueva en x al slamear
    }
}
