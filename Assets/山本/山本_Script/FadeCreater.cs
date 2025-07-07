using NUnit.Framework.Internal;
using UnityEngine;

public class FadeCreater : MonoBehaviour
{
    [Header("フェード用オブジェクト")]
    public GameObject fade; //色を黒に設定したImage

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //シーン開始時に生成
        GameObject createFade = Instantiate(fade, transform);
    }
}
