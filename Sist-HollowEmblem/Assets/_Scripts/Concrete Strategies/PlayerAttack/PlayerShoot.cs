using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class PlayerShoot : MonoBehaviour, IPlayerAttack
{
    [SerializeField] private Transform _attackStartPosition;
    [SerializeField] private Transform[] _attackStartDirections;
    [SerializeField] private int _damage;
    [SerializeField] private bool _isAiming;
    [SerializeField] private ParticleSystem _shootParticleSystem;
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private AudioSource _reloadSound;
    [SerializeField] private Light2D _shootLight;
    [SerializeField] private float _shootIntensity=1.4f;
    private bool _isReloading;
    [SerializeField]private int _bulletsRemaining = 10;
    private int _maxBullets = 10;
    private Animator _animator;
    private PlayerInventory _playerInventory;

    #region IPlayerAttack properties

    public PlayerMovementController PlayerMovementController => throw new System.NotImplementedException();

    public GameObject Projectile => throw new System.NotImplementedException();

    public Rigidbody2D Rigidbody2d => throw new System.NotImplementedException();

    public Transform AttackStartPosition => _attackStartPosition;

    public float Speed => throw new System.NotImplementedException();

    public float Damage => _damage;

    #endregion

    public bool IsAiming => _isAiming;
    public bool IsReloading => _isReloading;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerInventory = GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        ActionsManager.RegisterAction(ItemConstants.USE_AMMO);
        ActionsManager.SubscribeToAction(ItemConstants.USE_AMMO, Reload);
        if (isActiveAndEnabled)
        {
            UIManager.Instance.GetBulletsUIManager.UnlockGun();
            UIManager.Instance.GetBulletsUIManager.UpdateBullets(_bulletsRemaining);
        }
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

    public void Attack(int direction)
    {
        if (_bulletsRemaining > 0)
        {
            _bulletsRemaining--;
            _shootParticleSystem.Play();
            _shootSound.Play();
            _shootLight.gameObject.SetActive(true);
            _shootLight.intensity = _shootIntensity;
            
            RaycastHit2D hit2D = Physics2D.Raycast(_attackStartPosition.position,
                _attackStartPosition.right, 100);

            if (hit2D == null)
            {
                return;
            }

            if (hit2D.transform.CompareTag("Enemy") && hit2D.transform.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_damage);
            }
        }
        UIManager.Instance.GetBulletsUIManager.UpdateBullets(_bulletsRemaining);

    }

    public void Aim(bool isTrue, int direction)
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
        if (_bulletsRemaining < _maxBullets && _playerInventory.SearchAmmo())
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
}
