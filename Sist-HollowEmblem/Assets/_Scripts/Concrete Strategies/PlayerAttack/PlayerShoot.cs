using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class PlayerShoot : MonoBehaviour, IPlayerAttack
{
    #region Class Properties

    #region Serialized Properties

    [SerializeField] private LayerMask _whatIsShooteable;
    [SerializeField] private Transform _attackStartPosition;
    [SerializeField] private Transform[] _attackStartDirections;
    [SerializeField] private int _damage;
    [SerializeField] private bool _isAiming;
    [SerializeField] private ParticleSystem _shootParticleSystem;
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private AudioSource _reloadSound;
    [SerializeField] private Light2D _shootLight;
    [SerializeField] private float _shootIntensity=1.4f;

    #endregion

    #region Private Properties

    private bool _isReloading;
    private int _bulletsRemaining = 10;
    private bool _isUnlocked;
    private int _maxBullets = 10;
    private Animator _animator;
    private PlayerInventory _playerInventory;

    #endregion


    #region IPlayerAttack properties
    public Transform AttackStartPosition => _attackStartPosition;
    
    public float Damage => _damage;

    #endregion

    public bool IsAiming => _isAiming;
    public bool IsReloading => _isReloading;

    #endregion

    #region Monobehaviour Callbacks

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerInventory = GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        ActionsManager.RegisterAction(ItemConstants.USE_AMMO);
        ActionsManager.SubscribeToAction(ItemConstants.USE_AMMO, Reload);
    }

    private void Update()
    {
        if (_shootLight.gameObject.activeInHierarchy)
        {
            _shootLight.intensity -= Time.deltaTime*20;

            if ( _shootLight.intensity<=0)
            {
                _shootLight.gameObject.SetActive(false);
            }
        }
    }

    #endregion
  
    #region IPlayerAttack Methods
    public void Attack(int direction)
    {
        if (_bulletsRemaining > 0 && _isUnlocked)
        {
            _bulletsRemaining--;
            _shootParticleSystem.Play();
            _shootSound.Play();
            _shootLight.gameObject.SetActive(true);
            _shootLight.intensity = _shootIntensity;

            RaycastHit2D hit2D = Physics2D.Raycast(_attackStartPosition.position,
                _attackStartPosition.TransformDirection(Vector2.left), 100, _whatIsShooteable);

            if (hit2D.collider!=null)
            {
                if (hit2D.collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.TakeDamage(_damage);
                }
            }
        }

        UIManager.Instance.GetBulletsUIManager.UpdateBullets(_bulletsRemaining);
    }

    #endregion

    #region Class Methods

    public void Aim(bool isTrue, int direction)
    {
        if (_isUnlocked)
        {
            _attackStartPosition.SetPositionAndRotation(_attackStartDirections[direction].position,
                _attackStartDirections[direction].rotation);
            _isAiming = isTrue;
            _animator.SetBool("Aiming", _isAiming);

            _attackStartPosition.gameObject.SetActive(isTrue);

            Debug.DrawRay(_attackStartPosition.position, _attackStartPosition.right * 100);

            switch (direction)
            {
                case 0:
                    _animator.SetTrigger("AimFront");
                    break;
                case 1:
                    _animator.SetTrigger("AimUp");
                    break;
                case 2:
                    _animator.SetTrigger("AimDown");
                    break;
            }
        }
    }

    /// <summary>
    /// Callen on animation event
    /// </summary>
    public void OnReload()
    {
        int ammoResquested = _maxBullets - _bulletsRemaining;
        _bulletsRemaining += _playerInventory.GetAmmoFromInventory(ammoResquested);
        UIManager.Instance.GetBulletsUIManager.UpdateBullets(_bulletsRemaining);
    }

    public void Reload()
    {
        if (_isUnlocked && _bulletsRemaining < _maxBullets && _playerInventory.SearchAmmo() && !_isReloading)
        {
            _reloadSound.Play();
            _animator.SetTrigger("Reload");
        }
    }

    public void BeginReloadAnimation()
    {
        _isReloading = true;
    }

    public void EndReloadAnimation()
    {
        _isReloading = false;
    }

    public void UnlockShoot()
    {
        _isUnlocked = true;
        UIManager.Instance.GetBulletsUIManager.UnlockGun();
    }

    #endregion
}
