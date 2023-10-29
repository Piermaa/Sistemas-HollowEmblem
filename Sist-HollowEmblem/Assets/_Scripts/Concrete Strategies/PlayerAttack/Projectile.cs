using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile, IPooledObject
{
    [SerializeField] private float delayTime;
    [SerializeField] private float delayCooldown;
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _layer;
    //Animator animator;
    private int _direction;
    private Rigidbody2D _rb;
    [SerializeField] private float _forceMultiplier;

    

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

    #endregion

    public void OnObjectSpawn()
    {

    }

    private void Awake()
    {
        //animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        TranslateProjectile();

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
            Rb.velocity = Vector2.zero;
            Rb.AddForce((Rb.gameObject.transform.position - this.gameObject.transform.position).normalized * 5, ForceMode2D.Impulse);
        }
    }

    private void TranslateProjectile()
    {
        int x = Direction == 0 ? 1 : 0;

        Vector2 moveDirection = new Vector2(x, Direction);
        Vector2 movement = moveDirection.normalized * _speed;
        _rb.velocity = movement;
    }
}
