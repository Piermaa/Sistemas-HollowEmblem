using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    
    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;  //crea un tipo de arreglo que requiere una llave (string) para acceder a cierto elemento (cola de gameobjects)

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); //anade al diccionario la cola de objetos

            for (int i=0; i<pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);   //instancio objetos, los meto en la cola y los desactivo, desp los uso tpandolos y activandolos
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Rigidbody2D rb, Vector3 scale)
    {

        if (!poolDictionary.ContainsKey(tag))
        {
            print("Tag" + tag + "does not exists");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        //saca de la cola al objeto a despawnear
        
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);

        objectToSpawn.transform.localScale = scale;
        var pAttack = objectToSpawn.GetComponent<PlayerAttack>();
        //Debug.Log("Has");
        //if (pAttack.Rigidbody2d == null)
        //{
        //    pAttack.Rigidbody2d = rb;
        //}
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();     //busca que haya una interface en el objeto a spawnear

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn(); // si tiene el tipo IPooledObject se llamara el metodo OnObjectSpawn() al spawnear el obj  //accede a la interfaz, busca la implementacion del metodo y lo executa
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    
    
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            print("Tag" + tag + "does not exists");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        //saca de la cola al objeto a despawnear
        
        objectToSpawn.SetActive(true);
        
        objectToSpawn.transform.SetPositionAndRotation(position, Quaternion.Euler( new Vector3(rotation.x,0,0)));
        
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();     //busca que haya una interface en el objeto a spawnear
        
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn(); // si tiene el tipo IPooledObject se llamara el metodo OnObjectSpawn() al spawnear el obj  //accede a la interfaz, busca la implementacion del metodo y lo executa
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, int direction, ScriptableObject stats)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            print("Tag" + tag + "does not exists");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        //saca de la cola al objeto a despawnear
        
        objectToSpawn.SetActive(true);
        
        objectToSpawn.transform.SetPositionAndRotation(position, Quaternion.Euler( new Vector3(rotation.x,0,0)));
        
        IPooledProduct pooledObj = objectToSpawn.GetComponent<IPooledProduct>();     //busca que haya una interface en el objeto a spawnear
        
        if (pooledObj != null)
        {
            pooledObj.SetStats(stats);
            pooledObj.Direction = direction;
            pooledObj.OnObjectSpawn(); // si tiene el tipo IPooledObject se llamara el metodo OnObjectSpawn() al spawnear el obj  //accede a la interfaz, busca la implementacion del metodo y lo executa
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    
    
    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            print("Tag"+ tag +"does not exists");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        //saca de la cola al objeto a despawnear
        
        objectToSpawn.SetActive(true);
        //objectToSpawn.transform.position = position;
        //objectToSpawn.transform.rotation= rotation;

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();     //busca que haya una interface en el objeto a spawnear

        if (pooledObj!= null)
        {
            pooledObj.OnObjectSpawn(); // si tiene el tipo IPooledObject se llamara el metodo OnObjectSpawn() al spawnear el obj  //accede a la interfaz, busca la implementacion del metodo y lo executa
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}

