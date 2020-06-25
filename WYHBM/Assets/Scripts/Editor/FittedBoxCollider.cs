using UnityEditor;
using UnityEngine;

public class FittedBoxCollider
{
    [MenuItem("Tools/Fitted BoxCollider")]
    static void FittedBoxColliderMenu()
    {
        Transform transform = Selection.activeTransform;
        Quaternion rotation = transform.rotation;
        transform.rotation = Quaternion.identity;

        BoxCollider collider = transform.gameObject.GetComponent<BoxCollider>();

        if (collider == null)
        {
            transform.gameObject.AddComponent<BoxCollider>();
            collider = transform.gameObject.GetComponent<BoxCollider>();
        }

        Bounds bounds = new Bounds(transform.position, Vector3.zero);

        ExtendBounds(transform, ref bounds);

        collider.center = bounds.center - transform.position;
        collider.size = bounds.size;

        transform.rotation = rotation;
    }

    [MenuItem("Tools/Fitted BoxCollider", true)]
    static bool ValidateFittedBoxColliderMenu()
    {
        return Selection.activeTransform != null;
    }

    static void ExtendBounds(Transform t, ref Bounds b)
    {
        Renderer rend = t.gameObject.GetComponent<Renderer>();
        
        if (rend != null)
        {
            b.Encapsulate(rend.bounds.min);
            b.Encapsulate(rend.bounds.max);
        }

        foreach (Transform t2 in t)
        {
            ExtendBounds(t2, ref b);
        }
    }
}