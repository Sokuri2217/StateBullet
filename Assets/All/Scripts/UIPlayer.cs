using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : UIBase
{
    private bool boolUIDamageEffect;

    [Header("GameObject")]
    public GameObject objUIDamageEffect;//ダメージエフェクトUIGameObject
    public GameObject objUIGamePlay;    //ゲーム中UIGameObject
    public GameObject objUIGameClear;   //ゲームクリアUIGameObject
    public GameObject objUIGameOver;    //ゲームオーバーUIGameObject
    public GameObject[] objButton       //ボタンGameObject
        = new GameObject[2];
    [Header("Sprite")]
    public Sprite[] spriteAttributeBullet//属性弾Sprite
        = new Sprite[6];
    public Sprite[] spriteClimate        //気候Sprite
        = new Sprite[6];
    [Header("Image")]
    public Image imageWeather;                 //気候Image
    public Image imageClimate;                 //気候Image
    public Image[] imageBulletGage             //弾ゲージImage
        = new Image[2];                        
    public Image imageAttributeBullet;         //属性弾Image
    public Image imageHPGage;                  //体力ゲージImage
    [Header("Slider")]
    public Slider sliderHP;                    //体力Slider
    [Header("RectTransform")]
    public RectTransform rectTransformGameOver;//ゲームオーバーRectTransform
    [Header("Script")]
    public PlayerController playerController;  //"PlayerController"

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        boolUIDamageEffect = false;

        //GameObjectの設定
        objUIDamageEffect.SetActive(boolUIDamageEffect);
        objUIGameClear.SetActive(false);
        objUIGameOver.SetActive(false);

        for (int i = 0; i < objButton.Length; i++)
        {
            objButton[i].SetActive(false);
        }

        //プレイヤーの体力を取得して体力Sliderに入れる
        sliderHP.minValue = 0;                               //体力Slider最小値
        sliderHP.maxValue = characterBase.structStatus.maxHP;//体力Slider最大値
        SetUIHP(sliderHP.maxValue);

        if(imageClimate.sprite == null)
        {
            imageClimate.enabled = false;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        for (int i = 0; i < imageBulletGage.Length; i++)
        {
            if(i == 0)
            {
                if (PlayerController.shotTimer[i] == 0.0f)
                {
                    imageBulletGage[i].fillAmount = 0;
                }
                else if (PlayerController.shotTimer[i] < playerController.shotInterval[i])
                {
                    imageBulletGage[i].fillAmount = Mathf.Clamp01(PlayerController.shotTimer[i] / playerController.shotInterval[i]);
                }
                else if (PlayerController.shotTimer[i] >= playerController.shotInterval[i])
                {
                    imageBulletGage[i].fillAmount = 1;
                }
            }
            else if(i == 1)
            {
                if (PlayerController.shotTimer[playerController.NowAttributeBullet] == 0.0f)
                {
                    imageBulletGage[i].fillAmount = 0;
                }
                else if (PlayerController.shotTimer[playerController.NowAttributeBullet] < playerController.shotInterval[playerController.NowAttributeBullet])
                {
                    imageBulletGage[i].fillAmount = Mathf.Clamp01(PlayerController.shotTimer[playerController.NowAttributeBullet] / playerController.shotInterval[playerController.NowAttributeBullet]);
                }
                else if (PlayerController.shotTimer[playerController.NowAttributeBullet] >= playerController.shotInterval[playerController.NowAttributeBullet])
                {
                    imageBulletGage[i].fillAmount = 1;
                }
            
            
            }
        }

        if (playerController.isAcid)
        {
            imageHPGage.color = new Color32(255, 0, 255, 255);
        }
        else
        {
            if (sliderHP.value <= sliderHP.maxValue / 4)
            {
                imageHPGage.color = new Color32(255, 0, 0, 255);
            }
            else if (sliderHP.value <= sliderHP.maxValue / 2)
            {
                imageHPGage.color = new Color32(255, 255, 0, 255);
            }
            else
            {
                imageHPGage.color = new Color32(0, 255, 0, 255);
            }
        }
    }

    public void SetUIHP(float hp)
    {
        sliderHP.value = hp;//体力Sliderに現在の体力を入れる

        if(sliderHP.value != sliderHP.maxValue)
        {
            if (boolUIDamageEffect) return;

            boolUIDamageEffect = true;
            objUIDamageEffect.SetActive(boolUIDamageEffect);
            Invoke("SetUIDamageEffect", 2);
        }

        if (hp <= 0)
        {
            objUIGamePlay.SetActive(false);
            InvokeRepeating("SetUIGameOver", 0, 0.05f);
        }
    }

    public void SetUIAttributeBullet(int attribute)
    {
        imageAttributeBullet.sprite = spriteAttributeBullet[attribute - 1];//属性ImageのSpriteに属性Sprite[使っている属性]を入れる

        switch (attribute)
        {
            case 1://火
                imageBulletGage[1].color = new Color32(255, 0, 0, 255);
                break;
            case 2://水
                imageBulletGage[1].color = new Color32(0, 0, 255, 255);
                break;
            case 3://草
                imageBulletGage[1].color = new Color32(0, 255, 0, 255);
                break;
            case 4://風
                imageBulletGage[1].color = new Color32(127, 255, 127, 255);
                break;
            case 5://金属
                imageBulletGage[1].color = new Color32(127, 127, 127, 255);
                break;
            case 6://爆破
                imageBulletGage[1].color = new Color32(255, 127, 0, 255);
                break;
        }
    }

    public void SetUIDamageEffect()
    {
        boolUIDamageEffect = false;
        objUIDamageEffect.SetActive(boolUIDamageEffect);
    }

    public void SetUIWeather(float timer, float interval)
    {
        imageWeather.fillAmount = Mathf.Clamp01(timer / interval);
    }

    public void SetUIClimate(int number)
    {
        imageClimate.sprite = spriteClimate[number];

        if (imageClimate.sprite != null)
        {
            imageClimate.enabled = true;
        }
    }

    public void SetUIGameClear()
    {
        objUIGameClear.SetActive(true);
        objButton[1].SetActive(true);
    }

    public void SetUIGameOver()
    {
        if (rectTransformGameOver.sizeDelta.y >= 1080)
        {
            CancelInvoke("SetUIGameOver");
            rectTransformGameOver.sizeDelta
            = new Vector2(rectTransformGameOver.sizeDelta.x, 1080);
            objUIGameOver.SetActive(true);

            for (int i = 0; i < objButton.Length; i++)
            {
                objButton[i].SetActive(true);
            }
        }
        else
        {
            rectTransformGameOver.sizeDelta
            = new Vector2(rectTransformGameOver.sizeDelta.x, rectTransformGameOver.sizeDelta.y + 13.5f);
        }
    }
}
