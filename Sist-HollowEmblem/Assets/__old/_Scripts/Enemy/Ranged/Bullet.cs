using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour, IProduct, IAirEnemyBullet
{
    #region IProduct Properties
    
    public string ObjectPoolerKey => _bulletStats.ObjectPoolerKey;
    public GameObject MyGameObject => gameObject;
    
    #endregion

    #region IAirEnemyBullet Properties

    public int Damage => _bulletStats.Damage;

    #endregion
    
    #region ClassProperties

    #region Serialized Properties
    [SerializeField] private BulletStats _bulletStats;
    #endregion
    private Rigidbody2D _rigidbody2D;
    private float _despawnTimer;
    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
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
    #endregion
    
    #region IProduct Methods
    public IProduct Clone() => ObjectPooler.Instance.SpawnFromPool(ObjectPoolerKey).GetComponent<IProduct>();
    
    #endregion

    #region IAirEnemyBullet Methods

    public void Reset(Vector2 spawnPosition)
    {
        transform.position = spawnPosition;
    }

    public void Shoot(Vector2 direction)
    {
        _rigidbody2D.velocity = Vector3.zero;
        
        _rigidbody2D.velocity = new Vector2(-direction.x * _bulletStats.Speed,0) ;

        var partscale = GetComponentInChildren<ParticleSystem>().gameObject.transform;
        
        Vector3 theScale = transform.localScale;
        theScale.x = direction.x;
        transform.localScale = theScale;
        
        theScale = partscale.localScale;
        theScale.x = direction.x;
        partscale.localScale = theScale;
    }

    #endregion
   
}
