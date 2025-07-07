using UnityEngine;

public class EndlessSpowner : MonoBehaviour
{
    [Header("スポーン関係")]
    public GameObject[] spownEnemy; //敵の種類
    public float spownLimit;        //スポーンする間隔
    public int maxSpown;            //一度に存在出来る敵数
    public int currentSpown;        //現在の敵の数
    public float currentSpownUp;    //現在のスポナーの加速率
    public float spownUp;           //上昇率
    public float spownSmallUp;      //上昇率（小）
    public int spownNum;            //一度にスポーンする敵の種類
    public int addEnemy;            //一度にスポーンする敵の総数

    [Header("スポナー用タイマー")]
    public float spownTimer; //スポーンさせる
    public float addTimer1;  //敵の種類を増やす
    public float addTimer2;  //スポーン頻度を増やす
    public float addTimer3;  //スポーンする敵数を増やす

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //スポナー強化
        PaceUp();

        //スポーン管理
        SpownerCount();
    }

    //スポナー強化
    public void PaceUp()
    {
        //計測
        addTimer1 += Time.deltaTime;
        addTimer2 += Time.deltaTime;
        addTimer3 += Time.deltaTime;

        //一分ごとに敵の種類を増やす
        if (addTimer1 >= 60)  
        {
            //敵の種類数以上の値にならないようにする
            if (spownNum < 2)
            {
                spownNum++;
            }
            //敵の種類が増えなくなると、代わりに出現頻度をあげる
            else
            {
                currentSpownUp *= spownSmallUp;
            }
            //タイマーリセット
            addTimer1 = 0;
        }
        //30秒ごとに出現頻度をあげる
        if(addTimer2 >= 30)
        {
            //出現頻度増加
            currentSpownUp *= spownUp;
            //タイマーリセット
            addTimer2 = 0;
        }
        //二分ごとに一度に出現する敵数を増やす
        if (addTimer3 >= 120)   
        {
            //敵数増加
            addEnemy++;
            //タイマーリセット
            addTimer3 = 0;
        }

        //スポーン頻度の上限（最速1秒）
        if (spownLimit < 1.0f)
        {
            spownLimit = 1.0f;
        }
    }

    //スポーン管理
    public void SpownerCount()
    {
        //現時点での敵の総数が一定未満ならスポーンさせる
        if (currentSpown < maxSpown)
        {
            //一定時間経過でスポーン
            spownTimer += Time.deltaTime;

            if (spownTimer >= (spownLimit * currentSpownUp)) 
            {
                //一度にスポーンする敵数
                for (int j = 0; j <= addEnemy; j++) 
                {
                    //スポーンする敵の種類
                    for (int i = 0; i <= spownNum; i++)
                    {
                        //敵のスポーン上限を超えないようにする
                        if (currentSpown >= maxSpown)
                        {
                            break;
                        }
                        GameObject enemy = Instantiate(spownEnemy[i], this.transform.position, Quaternion.identity);
                        currentSpown++;
                    }
                }
                //タイマーリセット
                spownTimer = 0.0f;
            }
        }
    }
}
