using UnityEngine;

public class PlayerBulletBase : BulletBase
{
    [Header("使用中の属性弾")]
    public bool[] useElement;  //使用属性弾
    [Header("各属性弾の火力")]
    public float fire;
    public float water;
    public float wind;
    public float explosion;
    public float metal;
    public float grass;
    public float normal;

    public enum  Element
    {
        FIRE,
        WATER,
        WIND,
        EXPLOSION,
        METAL,
        GRASS,
        NORMAL
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //初期弾を通常弾に設定
        currentAttack = (int)Element.NORMAL;
        useElement[(int)Element.NORMAL] = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        //プレイヤーに当たったとき
        if (other.gameObject.tag == "Enemy")
        {
            //プレイヤーのスクリプトを取得して、HPを減らす
            character = other.gameObject.GetComponent<CharacterBase>();
            if (useElement[(int)Element.FIRE])
            {
                currentAttack = fire * character.fire;
                character.isFire = true;
            }
            if (useElement[(int)Element.WATER])
            {
                currentAttack = water * character.water;
                character.isWater = true;
            }
            if (useElement[(int)Element.WIND])
            {
                currentAttack = wind * character.wind;
                character.isWind = true;
            }
            if (useElement[(int)Element.EXPLOSION])
            {
                currentAttack = explosion * character.explosion;
            }
            if (useElement[(int)Element.METAL])
            {
                currentAttack = metal * character.metal;
                character.isMetal = true;
            }
            if (useElement[(int)Element.GRASS])
            {
                currentAttack = grass * character.grass;
                character.isGrass = true;
            }
            if (useElement[(int)Element.NORMAL])
            {
                currentAttack = normal * character.normal;
            }

            character.currentHP -= currentAttack;
        }
        //BulletMaster以外のオブジェクトに当たったとき
        if (other.gameObject.tag != BulletMaster)
        {
            //自身を消去
            Destroy(gameObject);
        }
    }
}
