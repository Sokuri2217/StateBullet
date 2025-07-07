using UnityEngine;

public class SelectButton : ButtonScale
{
    [Header("コンポーネント参照")]
    public HomeManager homeManager; //ホーム画面

    //次へ
    public void NextStage()
    {
        if (homeManager.selectStage < (homeManager.maxStage - 1)) 
        {
            //次のステージ
            homeManager.selectStage++;
        }
        else
        {
            //選択をループさせる
            homeManager.selectStage = 0;
        }

        //ボタンを押したときにSEを鳴らす
        se.PlayOneShot(selectClip);
    }

    //前へ
    public void BackStage()
    {
        if (homeManager.selectStage > 0)
        {
            //前のステージ
            homeManager.selectStage--;
        }
        else
        {
            //選択をループさせる
            homeManager.selectStage = (homeManager.maxStage - 1);
        }

        //ボタンを押したときにSEを鳴らす
        se.PlayOneShot(selectClip);
    }
}
