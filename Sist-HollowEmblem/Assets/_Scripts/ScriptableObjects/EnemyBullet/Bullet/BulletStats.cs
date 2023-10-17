using UnityEngine;

[CreateAssetMenu(fileName = "BulletStats", menuName = "Stats/BulletStats/BulletStats", order = 0)]
public class BulletStats : ScriptableObject
{
    [SerializeField] private BulletStatsValues _bulletStatsValues;
    
    public int Damage=>_bulletStatsValues.Damage;
    public float Speed=>_bulletStatsValues.Speed;
    public float TimeLimit=>_bulletStatsValues.TimeLimit;
    public string ObjectPoolerKey=>_bulletStatsValues.ObjectPoolerKey;
}

[System.Serializable]
public struct BulletStatsValues
{
    public int Damage;
    public float Speed;
    public float TimeLimit;
    public string ObjectPoolerKey;
}