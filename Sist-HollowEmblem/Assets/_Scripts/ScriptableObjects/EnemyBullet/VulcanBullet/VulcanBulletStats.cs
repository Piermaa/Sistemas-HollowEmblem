using UnityEngine;

[CreateAssetMenu(fileName = "BulletStats", menuName = "Stats/BulletStats/VulcanBulletStats", order = 0)]
public class VulcanBulletStats : ScriptableObject
{
    [SerializeField] private VulcanBulletStatsValues _vulcanBulletStatsValues;
    
    public Vector2 ImpulseDirection => _vulcanBulletStatsValues.ImpulseDirection;
    public float Force=> _vulcanBulletStatsValues.Force;
    public string ObjectPoolerKey => _vulcanBulletStatsValues.ObjectPoolerKey;
    public string ExplosionObjectPoolerKey => _vulcanBulletStatsValues.ExplosionObjectPoolerKey;
}

[System.Serializable]
public struct VulcanBulletStatsValues
{
    public Vector2 ImpulseDirection;
    public float Force;
    public string ObjectPoolerKey;
    public string ExplosionObjectPoolerKey;
}