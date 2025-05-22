using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("基礎能力")]
    public float maxHP;         //最大HP
    public float currentHP;     //現在のHP
    public float moveSpeed;     //移動速度
    public float originalSpeed; //基本移動速度

    [Header("落下関連")]
    public Vector3 pos;         //位置取得
    public float maxHeight;     //着地する前の最大高度
    public float safeHeight;    //落下してもダメージにならない高度
    public bool onGround;       //着地判定

    [Header("状態異常関連")]
    public float stateCount;     //持続時間の計測用
    public float sustainability; //状態異常の持続時間
    public bool isState;         //状態異常になっているか

    [Header("スリップダメージの頻度")]
    public float frequency;  //繰り返す間隔
    public float moldFre;    //カビの間隔
    public float corrFre;    //腐食の間隔
    public float burnFre;    //燃焼の間隔
    public float actiFre;    //活性化の間隔
    public bool setDuration; //間隔を設定したかどうか(何度も再設定しないように)

    [Header("状態異常の判別")]
    public bool isMold;      //カビ状態かどうか
    public bool isCorrosion; //腐食状態かどうか
    public bool isBurning;   //燃焼状態かどうか
    public bool isActive;    //活性化状態かどうか

    [Header("割合値")]
    public float mold;      //カビ
    public float corrosion; //腐食
    public float burning;   //燃焼
    public float active;    //活性化

    [Header("割合の初期値")]
    public float firstMold;      //カビ
    public float firstCorrosion; //腐食
    public float firstBurning;   //燃焼
    public float firstActive;    //活性化

    //ダメージ値　火　　水　　風　　　爆破　　　金属　　草　　通常　
    //public float fire, water, wind, explosion, metal, grass, normal;
    //初期値
    //public float firstFire, firstWater, firstWind, firstExplosion, firstMetal, firstGrass, firstNormal;
    [Header("付与中の属性")]
    public bool isFire;  //火
    public bool isWater; //水
    public bool isWind;  //風
    public bool isMetal; //金属
    public bool isGrass; //草

    [Header("属性弾の倍率")]
    public float fire;
    public float water;
    public float wind;
    public float explosion;
    public float metal;
    public float grass;
    public float normal;

    [Header("環境による変化")]
    public float downSpeed; //移動速度低下
    public float burnUp;    //燃焼割合増加
    public float longBurn;  //燃焼持続増加
    public float actiUp;    //活性化割合増加

    [Header("スクリプト参照")]
    public Environment environment;     //環境

    [Header("アニメーション")]
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //現在のHPを最大値に設定
        currentHP = maxHP;
        //moveSpeedの値を基本移動速度に設定
        originalSpeed = moveSpeed;
        //状態異常の持続時間を設定(30秒)
        sustainability = 30.0f;
        //環境情報を取得
        environment = GameObject.Find("VirtualEnvironment").GetComponent<Environment>();
        //Animator情報を取得
        animator = GetComponent<Animator>();
        //割合ダメージの初期化
        firstMold = mold;
        firstCorrosion = corrosion;
        firstBurning = burning;
        firstActive = active;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //オブジェクトの座標取得
        pos = transform.position;

        //落下ダメージ
        FallDamage();

        //環境状態による変化を管理
        Environment();

        //属性の付与及び状態異常付与の管理
        StateEnchant();

        //状態異常の管理
        StateManager();
    }

    //落下ダメージ
    public void FallDamage()
    {
        //最高度を更新し続ける
        if (pos.y > maxHeight)
        {
            maxHeight = pos.y;
        }

        //着地時に金属体じゃない＆一定の高度以上からの落下時にダメージ
        if (!isMetal && maxHeight > safeHeight && onGround)
        {
            Debug.Log("痛っ！");
            currentHP -= (maxHeight - safeHeight);
            maxHeight = 0.0f;
        }
    }

    //環境状態による変化を管理
    public void Environment()
    {
        if (environment.change)
        {
            //豪雨(炎＆燃焼無効・常時水付着)
            if (environment.rain)
            {
                isWater = true;
                isBurning = false;
            }
            //猛暑(カビ無効＆消滅)
            if (environment.hot)
            {
                isMold = false;
            }
            //暴風(燃焼の割合増加・移動速度低下)
            if (environment.storm)
            {
                burning *= burnUp;
                moveSpeed *= downSpeed;
            }
            //高重力(金属体で移動不可)
            if (environment.high_gravity)
            {
                if (isMetal)
                    moveSpeed = 0.0f;
                else
                    moveSpeed = originalSpeed;
            }
            //低重力(風を受けると一定時間浮遊・金属体で無効化)
            if (environment.low_gravity)
            {
                if (!isMetal)
                {

                }
            }
            //豊穣(活性化の割合増加・燃焼の持続延長)
            if (environment.abundant)
            {
                active *= actiUp;
                if (isBurning)
                    frequency += longBurn;
            }
        }
    }

    //属性の付与及び状態異常付与の管理
    public void StateEnchant()
    {
        //状態異常を付与するための条件
        {
            if (isWater && isWind && isGrass)//カビ
                isMold = true;
            else if (isWater && isWind && isMetal)//腐食
                isCorrosion = true;
            else if (isFire && isGrass)//燃焼
                isBurning = true;
            else if (isWater && isGrass)//活性化
                isActive = true;
        }

        //属性付与における相性
        {
            //火と水は共存できず、水が優先される
            if (isWater)
                isFire = false;
        }

        //状態異常になったことを確認するフラグをtrueにする
        if (isMold || isCorrosion || isBurning || isActive)
            isState = true;

        //状態異常になったとき、全属性の付与を解除する
        if (isState)
        {
            isFire = false;
            isWater = false;
            isWind = false;
            isMetal = false;
            isGrass = false;
        }
    }

    //状態異常の時間系の管理
    public void StateManager()
    {
        //なんらかの状態異常になったとき
        if (isState)
        {
            //持続時間の計測を開始
            stateCount += Time.deltaTime;
            //現在のカウントが持続時間以上になったらカビを解除する
            if (stateCount >= sustainability)
                isState = false;
            //間隔の設定
            if (!setDuration)
            {
                //時間の設定
                //カビ
                if (isMold)
                    frequency = moldFre;
                //腐食
                if (isCorrosion)
                    frequency = corrFre;
                //燃焼
                if (isBurning)
                    frequency = burnFre;
                //活性化
                if (isActive)
                    frequency = actiFre;
                //設定を何回もしないようにするため
                setDuration = true;
            }
            //ダメージ間隔の計測
            frequency -= Time.deltaTime;
            //割合処理
            if (frequency <= 0.0f)
            {
                //カビ
                if (isMold)
                    MoldDamage();
                //腐食
                if (isCorrosion)
                    CorrDamage();
                //燃焼
                if (isBurning)
                    BurningDamage();
                //活性化
                if (isActive)
                    ActiveRecovery();
            }
        }
        else
        {
            setDuration = false;
            isMold = false;
            isCorrosion = false;
            isBurning = false;
            isActive = false;
        }
    }

    //カビ状態の処理
    public void MoldDamage()
    {
        currentHP -= maxHP * (mold / 100);
        frequency = moldFre;
    }
    //腐食状態の処理
    public void CorrDamage()
    {
        currentHP -= maxHP * (corrosion / 100);
        frequency = corrFre;
    }
    //燃焼状態の処理
    public void BurningDamage()
    {
        currentHP -= maxHP * (burning / 100);
        //攻撃力を上昇させる処理を書く
    }
    //活性化状態の処理
    public void ActiveRecovery()
    {
        currentHP += maxHP * (active / 100);
        frequency = actiFre;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Ground")
        {
            onGround = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = false;
        }
    }
}
