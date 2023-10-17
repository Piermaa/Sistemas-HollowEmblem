using UnityEngine;

public class BulletFactory : AbstractPooledFactory<Bullet>
{
    public BulletFactory(Bullet productToProduce) : base(productToProduce)
    {
    }
    public override Bullet CreateProduct(Vector3 position, Quaternion rotation, int direction, ScriptableObject stats)
    {
        return (Bullet)_product.Clone( position, rotation, direction, stats);
    }
}
