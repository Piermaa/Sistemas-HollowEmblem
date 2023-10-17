using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class VulcanBullet : MonoBehaviour, IPooledObject, IProduct, IVulcanBullet
{
    #region IProduct Properties
    
    public GameObject MyGameObject => gameObject;
    public string ObjectPoolerKey => _vulcanBulletStats.ObjectPoolerKey;

    #endregion

    #region IVulcanBullet Properties

    public Vector2 ImpulseDirection => _vulcanBulletStats.ImpulseDirection;
    public float Force => _vulcanBulletStats.Force;
    public string ExplosionObjectPoolerKey => _vulcanBulletStats.ExplosionObjectPoolerKey;

    #endregion

    [SerializeField] private VulcanBulletStats _vulcanBulletStats; 
    
    private ObjectPooler _objectPooler;
    private Rigidbody2D rb2D;
    
    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
        rb2D = GetComponent<Rigidbody2D>();
    }
    public void OnObjectSpawn()
    {
        if (rb2D==null)
        {
            rb2D = GetComponent<Rigidbody2D>();
        }

        Vector3 theScale = transform.localScale;
        theScale.x = _vulcanBulletStats.ImpulseDirection.x > 0 ? 1 : -1;
        transform.localScale=theScale;
        
        
        rb2D.AddForce(_vulcanBulletStats.ImpulseDirection * _vulcanBulletStats.Force ,ForceMode2D.Impulse);
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
                target.TakeDamage(1);
        }
    }

    //todo reemplazar este clone por el otro, como el pooler siempre
    public IProduct Clone()
    {
        //### Debug ###
            //Debug.Log($"Bullet: {name}");
            //Debug.Log($"Has {_vulcanBulletStats.ObjectPoolerKey}");
        //### ---- ###
        
        return ObjectPooler.Instance.SpawnFromPool(_vulcanBulletStats.ObjectPoolerKey).GetComponent<IProduct>();
    }
}
