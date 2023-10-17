using UnityEngine;
public abstract class AbstractFactory<T> where T : IProduct
{
    protected T _product;
    public abstract T CreateProduct();
    
    public AbstractFactory(T productToProduce)
    {
        _product = productToProduce;
    }
}
