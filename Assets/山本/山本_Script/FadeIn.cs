using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    [Header("フェード用のImage（黒画像）")]
    public Image fadeImage; //コンポーネント参照

    [Header("フェード時間（秒）")]
    public float fadeDuration; //フェードに何秒かけるか

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //フェードイン開始
        StartCoroutine(StartFadeIn());
    }

    private IEnumerator StartFadeIn()
    {
        //ImageコンポーネントのColor情報を取得
        Color c = fadeImage.color;
        //計測用変数
        float time = 0.0f;

        // 最初は黒
        c.a = 1.0f;
        fadeImage.color = c;

        // fadeImageを透明にする（フェードイン）
        while (time < fadeDuration)
        {
            c.a = Mathf.Lerp(1.0f, 0.0f, time / fadeDuration);
            fadeImage.color = c;
            time += Time.deltaTime;
            yield return null;
        }

        //完全に透明にする
        c.a = 0.0f;
        fadeImage.color = c;
    }
}
