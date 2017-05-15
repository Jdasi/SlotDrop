using UnityEngine;

public class PersistantObjects : MonoBehaviour
{
   
    void Awake()
    {
        if (initialized)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
            initialized = true;
        }
    }

    static bool initialized { get; set; }
}
