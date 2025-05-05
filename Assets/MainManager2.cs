using UnityEngine;

public class MainManager2 : MonoBehaviour
{
    public static MainManager2 Instance;

    public Transform marker; // This is what we're accessing
    
    public ARColab arColab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}