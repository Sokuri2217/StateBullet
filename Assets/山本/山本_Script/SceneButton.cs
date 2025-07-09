using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneButton : ButtonScale
{
    [Header("フェード用のImage（黒画像）")]
    public Image fadeImage;

    [Header("遷移先のシーン名")]
    public string sceneName;

    [Header("フェード時間（秒）")]
    public float fadeDuration;

    [Header("BGM参照")]
    public AudioSource bgm;

    public void GameStart()
    {
        Time.timeScale = 1;
        //ボタンを押したときにSEを鳴らす
        se.PlayOneShot(selectClip);
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        //フェードアウトするためのImageを取得
        fadeImage = GameObject.Find("Fade(Clone)").GetComponent<Image>();

        // フェードアウト
        float time = 0f;
        Color c = fadeImage.color;

        //徐々に黒くしていく
        while (time < fadeDuration)
        {
            c.a = Mathf.Lerp(0f, 1f, time / fadeDuration);
            fadeImage.color = c;
            time += Time.deltaTime;
            bgm.volume = Mathf.Lerp(bgm.volume, 0f, time / fadeDuration);
            yield return null;
        }

        // 完全に黒くする
        c.a = 1f;
        fadeImage.color = c;

        //スポナーの状態
        if(sceneName== "EndlessStage")
        {
            SpownerSetting spownerSetting = GameObject.Find("SelectStageNum").GetComponent<SpownerSetting>();
            spownerSetting.select = 3;
        }

            //シーン移動
            SceneManager.LoadScene(sceneName);
    }
}
