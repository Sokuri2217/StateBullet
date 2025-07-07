using UnityEngine;
using UnityEngine.UI;
using System;
public class UIBase : MonoBehaviour
{
    [Header("Sprite")]
    public Sprite[] spriteIsAttribute   //属性Sprite
    = new Sprite[5];
    public Sprite[] spriteAttributeTimer//属性用タイマーSprite
    = new Sprite[5];
    public Sprite[] spriteIsAbnormal    //状態異常Sprite
        = new Sprite[4];
    [Header("Image")]
    public Image[] imageIsAttribute     //付与中の属性image
        = new Image[3];
    public Image[] imageAttributeTimer  //属性用タイマーimage
        = new Image[3];
    public Image imageIsAbnormal;       //状態異常Image
    public Image imageAbnormalTimer;    //状態異常用タイマーimage
    [Header("Script")]
    public CharacterBase characterBase;//"characterBase"

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        imageAbnormalTimer.fillAmount = 0;

        for (int i = 0; i < imageIsAttribute.Length; i++)
        {
            imageIsAttribute[i].enabled = false;
            imageAttributeTimer[i].enabled = false;
        }

        imageIsAbnormal.enabled = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    //属性UI
    public void IsAttribute(int attribute)
    {
        //
        if (characterBase.IsAttribute[attribute])
        {
            for (int i = 0; i < imageAttributeTimer.Length; i++)
            {
                if (imageAttributeTimer[i].sprite != null && imageAttributeTimer[i].sprite == spriteAttributeTimer[attribute])
                {
                    imageAttributeTimer[i].fillAmount = Mathf.Clamp01(characterBase.AttributeTimer[attribute] / characterBase.attributeLimit[attribute]);
                }
            }

            //すでに表示済みなら何もしない
            if (Array.Exists(imageIsAttribute, image => image.sprite == spriteIsAttribute[attribute]))
            {
                return;
            }

            //空いてるとこに表示する
            for (int i = 0; i < imageIsAttribute.Length; i++)
            {
                if (imageIsAttribute[i].sprite == null)
                {
                    imageIsAttribute[i].enabled = true;
                    imageAttributeTimer[i].enabled = true;

                    imageIsAttribute[i].sprite = spriteIsAttribute[attribute];
                    imageAttributeTimer[i].sprite = spriteAttributeTimer[attribute];

                    imageAttributeTimer[i].type = Image.Type.Filled;
                    imageAttributeTimer[i].fillOrigin = 2;

                    break;
                }
            }
        }
        else
        {
            //非表示にする
            for (int i = 0; i < imageIsAttribute.Length; i++)
            {
                if (imageIsAttribute[i].sprite == spriteIsAttribute[attribute])
                {
                    imageIsAttribute[i].sprite = null;
                    imageAttributeTimer[i].sprite = null;

                    imageIsAttribute[i].enabled = false;
                    imageAttributeTimer[i].enabled = false;
                    break;
                }
            }
        }
    }

    //状態異常UI
    public void IsAbnormal(int abnormal)
    {
        imageAbnormalTimer.fillAmount = Mathf.Clamp01(characterBase.AbnormalTimer / characterBase.abnormalLimit[abnormal]);

        if (characterBase.IsAbnormal[abnormal] == true)
        {
            if (imageIsAbnormal.sprite == null)
            {
                imageIsAbnormal.enabled = true;
                imageIsAbnormal.sprite = spriteIsAbnormal[abnormal];
            }
        }
        else if(characterBase.IsAbnormal[abnormal] == false)
        {
            imageIsAbnormal.sprite = null;
            imageIsAbnormal.enabled = false;
        }
    }
}
