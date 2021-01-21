using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Characters/Player", order = 1)]
public class PlayerSO : CharacterSO
{
    [Header("Player")]
    [SerializeField] private int _id = 0;

    public int ID { get { return _id; } }
}