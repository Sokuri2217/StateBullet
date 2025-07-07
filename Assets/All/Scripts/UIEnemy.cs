using UnityEngine;
using UnityEngine.UI;

public class UIEnemy : UIBase
{
    [Header("体力の表示時間")]
    public float hPActiveLimit;
    private float hPActiveTimer;
    [Header("GameObject")]
    public GameObject objHP;//体力"GameObject"

    private Image imageHP;  //体力"Image"

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //体力オブジェクトの子オブジェクト"UI_HP_Front"を探して"Image"コンポーネントを取得する
        Transform childObj = objHP.transform.Find("UI_HP_Front");
        imageHP = childObj.GetComponent<Image>();

        objHP.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(objHP.activeSelf)
        {
            hPActiveTimer += Time.deltaTime;

            if(hPActiveTimer >= hPActiveLimit)
            {
                objHP.SetActive(false);
                hPActiveTimer = 0;
            }
        }
    }

    public void SetUIHP(float hp)
    {
        if(objHP.activeSelf)
        {
            hPActiveTimer = 0;
        }
        else
        {
            objHP.SetActive(true);
        }
        
        imageHP.fillAmount = Mathf.Clamp01(hp / characterBase.structStatus.maxHP);
    }
}
