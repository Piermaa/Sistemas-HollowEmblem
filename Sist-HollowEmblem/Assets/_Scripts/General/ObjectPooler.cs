using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject SpawnFromPool(string tag, Vector3 position,Quaternion rotation)
    {

        if (!poolDictionary.ContainsKey(tag))
        {
            print("Tag"+ tag +"does not exists");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        //saca de la cola al objeto a despawnear


        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);
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
        if (pAttack.playerRigidBody == null)
        {
            pAttack.playerRigidBody = rb;
        }
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();     //busca que haya una interface en el objeto a spawnear

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn(); // si tiene el tipo IPooledObject se llamara el metodo OnObjectSpawn() al spawnear el obj  //accede a la interfaz, busca la implementacion del metodo y lo executa
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Vector3 direction)
    {

        if (!poolDictionary.ContainsKey(tag))
        {
            print("Tag" + tag + "does not exists");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        //saca de la cola al objeto a despawnear


        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = Quaternion.Euler( new Vector3(rotation.x,0,0));

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();     //busca que haya una interface en el objeto a spawnear
        objectToSpawn.TryGetComponent<Bullet>(out var bullet);

        if (bullet!=null)
        {
            bullet.moveDirection = direction;
        }
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn(); // si tiene el tipo IPooledObject se llamara el metodo OnObjectSpawn() al spawnear el obj  //accede a la interfaz, busca la implementacion del metodo y lo executa
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
















    //}

    //[System.Serializable]
    //public class ObjectPoolItem
    //{

    //	public GameObject objectToPool;
    //	public int amountToPool;
    //	public bool shouldExpand = true;

    //	public ObjectPoolItem(GameObject obj, int amt, bool exp = true)
    //	{
    //		objectToPool = obj;
    //		amountToPool = Mathf.Max(amt, 2);
    //		shouldExpand = exp;
    //	}
    //}

    //public class ObjectPooler : MonoBehaviour
    //{
    //	public static ObjectPooler SharedInstance;
    //	public List<ObjectPoolItem> itemsToPool;


    //	public List<List<GameObject>> pooledObjectsList;
    //	public List<GameObject> pooledObjects;
    //	private List<int> positions;

    //	void Awake()
    //	{

    //		SharedInstance = this;

    //		pooledObjectsList = new List<List<GameObject>>();
    //		pooledObjects = new List<GameObject>();
    //		positions = new List<int>();


    //		for (int i = 0; i < itemsToPool.Count; i++)
    //		{
    //			ObjectPoolItemToPooledObject(i);
    //		}

    //	}


    //	public GameObject GetPooledObject(int index)
    //	{

    //		int curSize = pooledObjectsList[index].Count;
    //		for (int i = positions[index] + 1; i < positions[index] + pooledObjectsList[index].Count; i++)
    //		{

    //			if (!pooledObjectsList[index][i % curSize].activeInHierarchy)
    //			{
    //				positions[index] = i % curSize;
    //				return pooledObjectsList[index][i % curSize];
    //			}
    //		}

    //		if (itemsToPool[index].shouldExpand)
    //		{

    //			GameObject obj = (GameObject)Instantiate(itemsToPool[index].objectToPool);
    //			obj.SetActive(false);
    //			obj.transform.parent = this.transform;
    //			pooledObjectsList[index].Add(obj);
    //			return obj;

    //		}
    //		return null;
    //	}

    //	public List<GameObject> GetAllPooledObjects(int index)
    //	{
    //		return pooledObjectsList[index];
    //	}


    //	public int AddObject(GameObject GO, int amt = 3, bool exp = true)
    //	{
    //		ObjectPoolItem item = new ObjectPoolItem(GO, amt, exp);
    //		int currLen = itemsToPool.Count;
    //		itemsToPool.Add(item);
    //		ObjectPoolItemToPooledObject(currLen);
    //		return currLen;
    //	}


    //	void ObjectPoolItemToPooledObject(int index)
    //	{
    //		ObjectPoolItem item = itemsToPool[index];

    //		pooledObjects = new List<GameObject>();
    //		for (int i = 0; i < item.amountToPool; i++)
    //		{
    //			GameObject obj = (GameObject)Instantiate(item.objectToPool);
    //			obj.SetActive(false);
    //			obj.transform.parent = this.transform;
    //			pooledObjects.Add(obj);
    //		}
    //		pooledObjectsList.Add(pooledObjects);
    //		positions.Add(0);

    //	}
    
}

