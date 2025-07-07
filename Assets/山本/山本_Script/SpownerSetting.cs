using UnityEngine;

public class SpownerSetting : MonoBehaviour
{
    public int select;
    public static SpownerSetting Instance { get; private set; }

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
    }
}
