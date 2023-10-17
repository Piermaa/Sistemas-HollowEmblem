using UnityEngine;
public class VulcanBulletFactory : AbstractPooledFactory<VulcanBullet>
{
    public VulcanBulletFactory(VulcanBullet productToProduce) : base(productToProduce)
    {
    }
    public override VulcanBullet CreateProduct(Vector3 position, Quaternion rotation, int direction)
    {
        return (VulcanBullet)_product.Clone(position, rotation, direction);
    }
}
