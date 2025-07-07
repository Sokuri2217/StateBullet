using UnityEngine;
using System.Collections;

public class GameEnd : ButtonScale
{
    //ゲーム終了
    public void StartEnd()
    {
        StartCoroutine(LoadEnd());
    }

    private IEnumerator LoadEnd()
    {
        //ボタンを押してから1.5秒後に処理実行
        yield return new WaitForSeconds(1.5f);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();//ゲームプレイ終了
#endif

    }
}
