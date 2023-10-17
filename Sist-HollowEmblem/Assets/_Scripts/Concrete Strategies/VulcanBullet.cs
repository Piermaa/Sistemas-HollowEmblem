using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class VulcanBullet : MonoBehaviour, IPooledProduct, IVulcanBullet
{
    #region IPooledProduct Properties
    
    public GameObject MyGameObject => gameObject;

    public int Direction
    {
        get=>_direction; 
        set=>_direction=value;
    }

    public ScriptableObject Stats
    {
        get =>_vulcanBulletStats;
    }

    public string ObjectPoolerKey => _vulcanBulletStats.ObjectPoolerKey;

    #endregion

    #region IVulcanBullet Properties

    public Vector2 ImpulseDirection => _vulcanBulletStats.ImpulseDirection;
    public float Force => _vulcanBulletStats.Force;
    public string ExplosionObjectPoolerKey => _vulcanBulletStats.ExplosionObjectPoolerKey;

    #endregion

    #region Class Properties

    [SerializeField] private VulcanBulletStats _vulcanBulletStats; 
    
    private ObjectPooler _objectPooler;
    private Rigidbody2D rb2D;
    private int _direction;
    #endregion

    #region IPooledProduct Methods

    
    public void OnObjectSpawn()
    {
        if (rb2D==null)
        {
            rb2D = GetComponent<Rigidbody2D>();
        }

        Vector3 theScale = transform.localScale;
        theScale.x = _direction;
        transform.localScale=theScale;

        var dir = new Vector2(_vulcanBulletStats.ImpulseDirection.x * _direction,_vulcanBulletStats.ImpulseDirection.y); 
        
        rb2D.AddForce(_vulcanBulletStats.Force * dir,ForceMode2D.Impulse);
    }
    
    public IPooledProduct Clone(Vector3 position, Quaternion rotation, int direction, ScriptableObject stats)
    {
        return ObjectPooler.Instance.SpawnFromPool(_vulcanBulletStats.ObjectPoolerKey, position, rotation, direction, stats)
            .GetComponent<IPooledProduct>();
    }

    #endregion

    #region Monobehaviour Callbacks

    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.CompareTag("Enemy")|| collision.CompareTag("Bomb")))
        {
            _objectPooler.SpawnFromPool(_vulcanBulletStats.ExplosionObjectPoolerKey, transform.position, transform.rotation);
            this.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent<IDamageable>(out var target);
            target.TakeDamage(_vulcanBulletStats.Damage);
        }
    }

    public void SetStats(ScriptableObject stats)
    {
        _vulcanBulletStats=stats as VulcanBulletStats;
    }
    
    #endregion
}
