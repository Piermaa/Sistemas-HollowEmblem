using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile, IPlayerBullet
{
    [SerializeField] private float delayTime;
    [SerializeField] private float delayCooldown;
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _layer;
    //Animator animator;
    private int _direction;
    private Rigidbody2D _rb;
    [SerializeField] private float _forceMultiplier;
    [SerializeField] private Rigidbody2D _playerRigidbody;

    #region IProjectile Properties

    public float DelayTime => delayTime;
    public float DelayCooldown => delayCooldown;
    public float Speed => _speed;
    public float ForceMultiplier => _forceMultiplier;
    public LayerMask Layer => _layer;

    public int Direction
    {
        get => _direction;
        set => _direction = value;
    }

    public Rigidbody2D Rb => _rb;

    public int Damage => 1;

    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        delayTime = delayTime > 0 ? delayTime - Time.deltaTime : 0;

        if (delayTime <= 0)
        {
            delayTime = delayCooldown;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.TryGetComponent<IDamageable>(out var damageable))
        { 
            damageable.TakeDamage(1);
            _playerRigidbody.velocity = Vector2.zero;
            _playerRigidbody.AddForce((_playerRigidbody.gameObject.transform.position - this.gameObject.transform.position).normalized * 5, ForceMode2D.Impulse);
            gameObject.SetActive(false);
        }
    }

    public void Attack(Vector3 direction)
    {
        _rb.velocity = direction * Speed;
    }

    public void Reset(Transform spawnPosition, Rigidbody2D playerRb)
    {
        if (_playerRigidbody==null)
        {
            _playerRigidbody = playerRb;
        }

        transform.position = spawnPosition.position;
        transform.rotation = spawnPosition.rotation;
        transform.localScale = -spawnPosition.parent.parent.localScale;
        _rb.velocity = Vector2.zero;
    }
}
