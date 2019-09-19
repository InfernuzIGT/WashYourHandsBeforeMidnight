using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (!_instance)
                Debug.LogWarningFormat("Accessing {0} before its Awake phase", typeof(T).Name);

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null || FindObjectsOfType<T>().Length > 1)
        {
            Debug.LogWarningFormat("Please make sure there is only one {0} in the scene.", typeof(T).Name);
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}