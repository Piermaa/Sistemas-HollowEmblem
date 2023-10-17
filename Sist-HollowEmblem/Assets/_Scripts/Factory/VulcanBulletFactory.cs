public class VulcanBulletFactory : AbstractFactory<VulcanBullet>
{
    public VulcanBulletFactory(VulcanBullet productToProduce) : base(productToProduce)
    {
    }
    public override VulcanBullet CreateProduct()
    {
        return (VulcanBullet)_product.Clone();
    }
}
