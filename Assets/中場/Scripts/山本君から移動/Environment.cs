using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.UI;

public class Environment : MonoBehaviour
{
    [Header("気候(猛暑,豪雨,豊穣,暴風,低重力,高重力)")]
    public bool[] climate
        = new bool[6];
    //ゲーム開始カウント
    public float start_time;
    //環境変化までのカウント
    public float change_time;
    //環境変化までの現在のカウント
    public float now_time;
    //環境変化の乱数格納用
    public int random_environment;
    public bool change;

    public Image image;

    public UIPlayer uIPlayer;//"uIPlayer"

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //環境フラグを全てfalseにする（最初の環境をフラットにする）
        FlatEnvironment();

        //環境変化までの時間を設定
        now_time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeEnvironment();
    }

    //時間経過で環境を変化させる
    void ChangeEnvironment()
    {
        now_time += Time.deltaTime;
        uIPlayer.SetUIWeather(now_time, change_time);

        if (now_time >= change_time)
        {
            random_environment = Random.Range(0, 100);

            //環境フラグを全てfalseにする（最初の環境をフラットにする）
            FlatEnvironment();

            //格納した値ごとに、変化する環境のフラグをtrueにする
            if (random_environment >= 0 && random_environment <= 14)
            {
                climate[0] = true;
                uIPlayer.SetUIClimate(0);
                Debug.Log("猛暑");
            }
            else if (random_environment >= 15 && random_environment <= 39)
            {
                climate[1] = true;
                uIPlayer.SetUIClimate(1);
                Debug.Log("豪雨");
            }
            else if (random_environment >= 40 && random_environment <= 44)
            {
                climate[2] = true;
                uIPlayer.SetUIClimate(2);
                Debug.Log("豊穣");
            }
            else if (random_environment >= 45 && random_environment <= 69)
            {
                climate[3] = true;
                uIPlayer.SetUIClimate(3);
                Debug.Log("暴風");
            }
            else if (random_environment >= 70 && random_environment <= 84)
            {
                climate[4] = true;
                uIPlayer.SetUIClimate(4);
                Debug.Log("低重力");
            }
            else if (random_environment >= 85 && random_environment <= 99)
            {
                climate[5] = true;
                uIPlayer.SetUIClimate(5);
                Debug.Log("高重力");
            }

            //カウントをリセットする
            now_time = 0;
        }
    }

    //環境フラグを全てfalseにする（最初の環境をフラットにする）
    void FlatEnvironment()
    {
        for (int i = 0; i < climate.Length; i++)
        {
            climate[i] = false;
        }
    }
}
