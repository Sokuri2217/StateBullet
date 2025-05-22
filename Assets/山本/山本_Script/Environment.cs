using UnityEngine;
using UnityEngine.UI;

public class Environment : MonoBehaviour
{
    //環境変化フラグ
    //           雨、 猛暑、暴風、   高重力、     低重力、    豊穣期
    public bool rain, hot, storm, high_gravity, low_gravity, abundant;
    //ゲーム開始カウント
    public float start_time;
    //環境変化までのカウント
    public float change_time;
    //環境変化までの現在のカウント
    public float now_time;
    //環境変化の乱数格納用
    public int random_environment;
    //変更しましたフラグ
    public bool change;


    public Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ////環境フラグを全てfalseにする（最初の環境をフラットにする）
        //FlatEnvironment();

        ////環境変化までの時間を設定
        //now_time = 0;

        //image.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (start_time > 0.0f)
        //{
        //    //ゲーム開始までのカウントダウン
        //    start_time -= Time.deltaTime;
        //}
        //else
        //{
        //    //時間経過で環境を変化させる
        //    ChangeEnvironment();

        //    //時間経過に伴って、UIにも変化を与える
        //    image.fillAmount = now_time / change_time;
        //}
    }

    //時間経過で環境を変化させる
    void ChangeEnvironment()
    {
        now_time += Time.deltaTime;

        if (now_time >= change_time)
        {
            change = true;
            random_environment = Random.Range(0, 100);

            //環境フラグを全てfalseにする（最初の環境をフラットにする）
            FlatEnvironment();

            //格納した値ごとに、変化する環境のフラグをtrueにする
            if (random_environment <= 24)
                rain = true;
            else if (random_environment <= 39)
                hot = true;
            else if (random_environment <= 64)
                storm = true;
            else if (random_environment <= 79)
                high_gravity = true;
            else if (random_environment <= 94)
                low_gravity = true;
            else if (random_environment <= 99)
                abundant = true;

            //カウントをリセットする
            now_time = 0;
            //変更完了
            change = false;
        }
    }

    //環境フラグを全てfalseにする（最初の環境をフラットにする）
    void FlatEnvironment()
    {
        rain = false;
        hot = false;
        storm = false;
        high_gravity = false;
        low_gravity = false;
        abundant = false;
    }
}
