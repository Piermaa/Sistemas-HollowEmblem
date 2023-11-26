using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class VulcanBullet : MonoBehaviour, IProduct, IVulcanBullet
{
    #region IProduct Properties
    
    public GameObject MyGameObject => gameObject;

    public string ObjectPoolerKey => _vulcanBulletStats.ObjectPoolerKey;

    #endregion

    #region IVulcanBullet Properties
    public int Damage => _vulcanBulletStats.Damage;

    public string ExplosionObjectPoolerKey => _vulcanBulletStats.ExplosionObjectPoolerKey;
    #endregion

    #region Class Properties

    [SerializeField] private VulcanBulletStats _vulcanBulletStats;
    private ObjectPooler _objectPooler;
    private Rigidbody2D rb2D;
    #endregion
    
    #region Monobehaviour Callbacks

    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent<IDamageable>(out var target);
            target.TakeDamage(_vulcanBulletStats.Damage);
        }
        if (!collision.CompareTag("Enemy") && !collision.CompareTag("Bomb"))
        {
            _objectPooler.SpawnFromPool(_vulcanBulletStats.ExplosionObjectPoolerKey, transform.position, transform.rotation);
            this.gameObject.SetActive(false);
        }
    }

    public void SetStats(ScriptableObject stats)
    {
        _vulcanBulletStats=stats as VulcanBulletStats;
    }
    
    #endregion
    
    #region IVulcanBullet Methods
    public void Reset(Vector2 spawnPos)
    {
        transform.position = spawnPos;
    }
    public void Shoot(Vector2 direction, float force)
    {
        if (rb2D==null)
        {
            rb2D = GetComponent<Rigidbody2D>();
        }
        
        Vector3 theScale = transform.localScale;
        theScale.x = direction.x > 0? 1:-1;
        transform.localScale=theScale;
        
        rb2D.AddForce(force * direction, ForceMode2D.Impulse);
    }
    #endregion
    
    #region IProduct Methods
    public IProduct Clone() => ObjectPooler.Instance.SpawnFromPool(_vulcanBulletStats.ObjectPoolerKey).GetComponent<IProduct>();
    
    #endregion
}
