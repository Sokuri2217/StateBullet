using UnityEngine;
using UnityEngine.UI;

public class UISpowner : UIBase
{
    [Header("バリア")]
    public Image basicBarrier;      //バリアが一枚以上あるとき
    public Image breakBarrier;      //バリアが全て壊れているとき
    public Image currentBarrierNum; //バリアの残り枚数
    public Sprite[] barrierNum;     //数字の画像
    public bool isBreak;            //バリア全壊フラグ

    [Header("Spowner参照")]
    public SpownerController spownerController;

    private Image imageHP;  //体力"Image"

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        imageHP = GameObject.Find("UI_BARRIER_HP_Front").GetComponent<Image>();

        isBreak = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        BarrierUI();

        //残りHPを体力ゲージに反映
        imageHP.fillAmount = Mathf.Clamp01(spownerController.NowHP / characterBase.structStatus.maxHP);
    }

    public void BarrierUI()
    {
        for (int i = 0; i < spownerController.maxBarrier; i++) 
        {
            if (spownerController.currentBarrier == i) 
            currentBarrierNum.sprite = barrierNum[i];
        }

        if (spownerController.currentBarrier == 0 && !isBreak) 
        {
            basicBarrier.fillAmount = 0.0f;
            isBreak = true;
        }

        if (isBreak)
        {
            //バリア回復時間を可視化
            imageHP.fillAmount = Mathf.Clamp01(spownerController.barrierTimer / spownerController.barrierLimit);
            if(spownerController.barrierTimer >=spownerController.barrierLimit)
            {
                isBreak = false;
            }
        }
    }
}
