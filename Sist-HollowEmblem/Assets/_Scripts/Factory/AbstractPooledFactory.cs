using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class AbstractPooledFactory<T> where T : IPooledProduct
{
    protected T _product;
    public abstract T CreateProduct(Vector3 position, Quaternion rotation, int direction);
    public AbstractPooledFactory(T productToProduce)
    {
        _product = productToProduce;
    }
}
