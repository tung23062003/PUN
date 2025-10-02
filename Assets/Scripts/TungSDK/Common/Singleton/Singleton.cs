using UnityEngine;

public abstract class Singleton<T> : TickBehaviour where T : Singleton<T>
{
    private static T instance;

    public static bool isInitialized
    {
        get
        {
            return instance != null;
        }
    }

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (FindObjectsOfType<T>().Length > 1)
                {
                    Debug.LogError("[Singleton] Something went really wrong - there should never be more than 1 singleton! Reopening the scene might fix it.");
                    return instance;
                }

                if (instance == null)
                {
                    Debug.LogError("[Singleton] Something went really wrong - specified Singleton does not found!");
                }
            }
            return instance;
        }
    }
}
