using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour, IPooledProduct
{
    #region IPooledProduct Properties

    public string ObjectPoolerKey => _bulletStats.ObjectPoolerKey;
    public GameObject MyGameObject => gameObject;

    public int Direction
    {
        get => _direction;
        set => _direction = value;
    }

    #endregion

    [SerializeField] private BulletStats _bulletStats;
    
    private Rigidbody2D _rigidbody2D;

    private float _despawnTimer;
    private int _direction;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void OnObjectSpawn()
    {
        _rigidbody2D.velocity = Vector3.zero;
        
        _rigidbody2D.velocity = new Vector2(-_direction * _bulletStats.Speed,0) ;

        var partscale = GetComponentInChildren<ParticleSystem>().gameObject.transform;
        
        Vector3 theScale = transform.localScale;
        theScale.x = _direction;
        transform.localScale = theScale;
        
        theScale = partscale.localScale;
        theScale.x = _direction;
        partscale.localScale = theScale;
    }

    public IPooledProduct Clone(Vector3 position, Quaternion rotation, int direction)
    {
        var product = ObjectPooler.Instance.SpawnFromPool(ObjectPoolerKey,position, rotation, direction)
            .GetComponent<IPooledProduct>();
        
        return product;
    }

    private void Update()
    {
        _despawnTimer += Time.deltaTime;

        if (_despawnTimer>_bulletStats.TimeLimit)
        {
           gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(_bulletStats.Damage);
        }
        gameObject.SetActive(false);
    }
    
}
