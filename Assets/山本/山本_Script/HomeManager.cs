using UnityEngine;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    [Header("選択中のステージ")]
    public int selectStage;  //ステージ数
    public int maxStage;     //最大値

    public GameObject[] objEnemy      //敵オブジェクト
        = new GameObject[3];
    public GameObject[] compatibility //属性の相性表
        = new GameObject[3];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期選択を0に設定
        selectStage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //背景変更
        ChangeSprit();
    }

    //背景変更
    public void ChangeSprit()
    {
        //選択中のステージ番号に対応した背景画像を表示させる
        for (int i = 0; i < maxStage; i++) 
        {
            if(objEnemy[i] != null)
            {
                if (i == selectStage)
                {
                    objEnemy[i].SetActive(true);
                    compatibility[i].SetActive(true);
                }
                else
                {
                    objEnemy[i].SetActive(false);
                    compatibility[i].SetActive(false);
                }
            }
        }

        //スポナーを管理するために値を保存
        SpownerSetting spownerSetting = GameObject.Find("SelectStageNum").GetComponent<SpownerSetting>();
        spownerSetting.select = selectStage;
    }
}
