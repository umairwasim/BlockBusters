using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Levels/Level")]
public class LevelData : ScriptableObject
{
    public int parScore;
    public float spawnInterval;
    public float minRange;
    public float maxRange;
}
