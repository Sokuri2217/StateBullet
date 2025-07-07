using UnityEngine;

public class SESetting : MonoBehaviour
{
    public static SESetting Instance { get; private set; }

    //シングルトン(全てのシーンで一つだけ存在させる)
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
