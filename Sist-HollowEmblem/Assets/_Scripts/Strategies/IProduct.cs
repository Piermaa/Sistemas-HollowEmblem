using UnityEngine;

//todo strat pooledproduct!!!!!!!!!!!!!!!!!!!!!!!!!
public interface IProduct
{
    string ObjectPoolerKey { get; }
    IProduct Clone();
    GameObject MyGameObject { get; }
}

public interface IPooledProduct : IPooledObject
{
    string ObjectPoolerKey { get; }
    GameObject MyGameObject { get; }
    int Direction { get; set; }
    IPooledProduct Clone(Vector3 position, Quaternion rotation, int direction);
}