using UnityEngine;

public class timeResult : MonoBehaviour
{
    public float bestTime;
    public float currentTime;

    public static timeResult Instance { get; private set; }

    private void Awake()
    {
        // すでにインスタンスが存在する場合は削除
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // シーンをまたいでオブジェクトを保持

        if (currentTime < bestTime) 
        {
            bestTime = currentTime;
            currentTime = 0;
        }
    }
}
