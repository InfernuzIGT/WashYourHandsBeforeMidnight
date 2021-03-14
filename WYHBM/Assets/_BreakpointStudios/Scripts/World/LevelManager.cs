using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Manager")]
    [SerializeField] private SpawnPoint[] _spawnPoints;
    [Space]
    [SerializeField] private CombatArea[] _combatAreas;

    public SpawnPoint GetSpawnPoint(int ID)
    {
        return _spawnPoints[ID];
    }
    
    public Vector3 GetSpawnPointPosition(int ID)
    {
        return _spawnPoints[ID].transform.position;
    }

    public CombatArea GetCombatArea()
    {
        return _combatAreas[Random.Range(0, _combatAreas.Length)];
    }
}