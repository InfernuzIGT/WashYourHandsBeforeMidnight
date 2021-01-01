using UnityEngine;

public class WaypointController : MonoBehaviour
{
    [Header("Positions")]
    public Vector3[] positions;

    [Header("Debug")]
    public bool showDebug;
    public Color colorLine = new Color(1, 1, 1, 0.5f);
    public Color colorSphere = new Color(1, 0, 0, 0.5f);
    [Range(0f, 1f)]
    public float radius = 0.75f;

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (showDebug && positions.Length != 0)
        {
            Gizmos.color = colorSphere;

            for (int i = 0; i < positions.Length; i++)
            {
                Gizmos.DrawSphere(positions[i], radius);
            }
        }
    }

#endif
}