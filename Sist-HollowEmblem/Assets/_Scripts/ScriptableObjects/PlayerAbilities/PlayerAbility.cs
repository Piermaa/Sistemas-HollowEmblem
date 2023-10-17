using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class PlayerAbility : ScriptableObject, IPlayerAbility
{
    #region IPlayerAbility Properties

    public KeyCode AbilityKey => _abilityKey;
    public GameObject PlayerGameObject => _playerGameObject;
    public AudioSource AbilityAudioSource => _audioSource;
    public AudioClip AbilitySound => _abilitySound;
    public float CooldownTimer => _coolDownTimer;
    public float Cooldown => _cooldown;

    #endregion

    #region Class Properties

    #region Serialized Properties

    [SerializeField] protected float _cooldown;
    [SerializeField] protected AudioClip _abilitySound;
    [SerializeField] protected KeyCode _abilityKey;

    #endregion
   
    protected float _coolDownTimer;
    protected AudioSource _audioSource;
    protected GameObject _playerGameObject;

    #endregion

    #region IPlayerAbility Methods

    public virtual void Use()
    {
    }

    public virtual void AbilityUpdate()
    {
        _coolDownTimer = _coolDownTimer > 0 ? _coolDownTimer - Time.deltaTime : 0;
    }

    public virtual void Init(GameObject playerGameObject, AudioSource audioSource)
    {
        _playerGameObject = playerGameObject;
        _audioSource = audioSource;
    }

    public virtual bool CanBeUsed()
    {
        return _coolDownTimer <= 0;
    }

    #endregion
}