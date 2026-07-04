using UnityEngine;

public class PersistentSceneObjects : MonoBehaviour
{
    public static PersistentSceneObjects Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetPersistentSceneObjects()
    {
        Destroy(gameObject);
    }
}
