using UnityEngine;

public class BulletFactory : AbstractFactory<Bullet>
{
    public BulletFactory(Bullet productToProduce) : base(productToProduce)
    {
    }
    public override Bullet CreateProduct() => (Bullet)_product.Clone();
}
