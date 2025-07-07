using UnityEngine;
using UnityEngine.AI;

public class CharacterBase : MonoBehaviour
{
    //ステータス構造体
    [System.Serializable]
    public struct StructStatus
    {
        [Header("基礎ステータス")]
        public float maxHP;     //最大HP
        public float moveSpeed; //移動速度
    }

    //座標構造体
    [System.Serializable]
    public struct StructPosition
    {
        [Header("座標関連")]
        public float maxHeight; //着地する前の最大高度
        public float safeHeight;//落下してもダメージにならない高度
    }

    //構造体変数
    public StructStatus structStatus;    //ステータス
    public StructPosition structPosition;//座標

    //現在のステータス
    protected float nowHP;       //現在のHP
    protected float nowMoveSpeed;//現在の移動速度
    //現在の座標
    protected Vector3 position;//座標
    protected bool isGround;   //着地判定
    //重力
    protected float gravityForce = 11;//重力の力

    [Header("付与中の属性(火,水,草,風,金属)")]
    protected bool[] isAttribute = new bool[5];
    [Header("属性の付与時間")]
    public float[] attributeLimit = new float[5];   //属性の付与時間
    protected float[] attributeTimer = new float[5];//属性用タイマー
    [Header("属性弾のダメージ倍率(通常,火,水,草,風,金属,爆破)")]
    public float[] attributeDamage = new float[7];
    [Header("付与中の状態異常(燃焼,活性化,カビ,腐食)")]
    protected bool[] isAbnormal = new bool[4];
    [Header("状態異常の付与時間")]
    public float[] abnormalLimit = new float[4];//状態の付与時間
    protected float abnormalTimer;              //状態異常用タイマー
    [Header("状態異常の頻度(燃焼,活性化,カビ,腐食)")]
    public float[] abnormalFrequency = new float[4];
    protected float frequency;                      //繰り返す間隔
    protected bool setDuration;                     //間隔を設定したかどうか(何度も再設定しないように)
    [Header("状態異常の割合(燃焼,活性化,カビ,腐食)")]
    public float[] abnormalRatio = new float[4];
    [Header("状態異常の割合初期値(燃焼,活性化,カビ,腐食)")]
    protected float[] abnormalDefaultRatio = new float[4];
    [Header("環境による変化")]
    public float[] environmentalChange = new float[4];
    [Header("ゲームオブジェクト")]
    public GameObject objBullet;      //弾オブジェクト
    public GameObject objShotPosition;//発射位置オブジェクト
    [Header("コンポーネント参照")]
    protected Rigidbody setRigidbody;   //"Rigidbody"
    protected NavMeshAgent navMeshAgent;//"NavMeshAgent"
    [Header("スクリプト参照")]
    public Environment environment;//環境
    public UIBase uIBase;          //UI

    //現在の体力プロパティ
    public float NowHP
    {
        get { return nowHP; }
        set { nowHP = value; }
    }

    //付与中の属性プロパティ
    public bool[] IsAttribute
    {
        get { return isAttribute; }
        set { isAttribute = value; }
    }

    //付与中の状態異常プロパティ
    public bool[] IsAbnormal
    {
        get { return isAbnormal; }
        set { isAbnormal = value; }
    }

    //属性用タイマープロパティ
    public float[] AttributeTimer
    {
        get { return attributeTimer; }
    }

    //状態異常用タイマープロパティ
    public float AbnormalTimer
    {
        get { return abnormalTimer; }
    }

    //各属性参照(通常,火,水,草,風,金属,爆破)
    public enum enumAttribute
    {
        NORMAL,
        FIRE,
        WATER,
        GRASS,
        WIND,
        METAL,
        EXPLOSION,
    }

    //各状態異常参照(燃焼,活性化,カビ,腐食)
    public enum enumAbnormal
    {
        BURNING,
        ACTIVE,
        MOLD,
        CORROSION,
    }

    //各状態異常参照(燃焼割合増加,燃焼持続増加,活性化割合増加,移動速度低下)
    public enum enumEnvironment
    {
        BURNING_UP,
        LONG_BURNING,
        ACTIVE_UP,
        SPEED_DOWN,
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //ステータスを初期化
        nowHP = structStatus.maxHP;           //体力
        nowMoveSpeed = structStatus.moveSpeed;//移動速度

        if(GameObject.Find("VirtualEnvironment") != null)
        {
            //環境情報を取得
            environment = GameObject.Find("VirtualEnvironment").GetComponent<Environment>();
        }

        //コンポーネントを取得
        setRigidbody = this.GetComponent<Rigidbody>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        //属性の制限時間を設定
        {
            for (int i = 0; i < attributeLimit.Length; i++)
            {
                attributeTimer[i] = attributeLimit[i];
            }
        }

        //割合ダメージの初期化
        {
            for (int i = 0; i < abnormalRatio.Length; i++)
            {
                abnormalDefaultRatio[i] = abnormalRatio[i];
            }
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (environment != null)
        {
            //オブジェクトの座標取得
            position = transform.position;

            //落下ダメージ
            FallDamage();

            //重力変化とそれに伴う移動の管理
            GravityMove();

            if (isAbnormal[(int)enumAbnormal.BURNING] == false && isAbnormal[(int)enumAbnormal.ACTIVE] == false &&
                isAbnormal[(int)enumAbnormal.MOLD] == false && isAbnormal[(int)enumAbnormal.CORROSION] == false)
            {
                //属性の付与管理
                AttributeManager();
                //状態異常の付与管理
                AbnormalManager();
            }

            //状態異常の管理
            StateManager();
        }
    }

    //関数"HPManager(体力情報)"
    public virtual void HPManager(float number)
    {
        NowHP += number;//現在の体力から引数"number(数値)"を引く
    }

    //関数"Destroy(破壊)"
    protected virtual void Destroy()
    {
        Destroy(this.gameObject);//このオブジェクトを破壊する
    }

    //落下ダメージ
    public void FallDamage()
    {
        //最高度を更新し続ける
        if (position.y > structPosition.maxHeight)
        {
            structPosition.maxHeight = position.y;
        }

        //着地時に金属体じゃない＆一定の高度以上からの落下時にダメージ
        if (!isAttribute[(int)enumAttribute.METAL - 1] && structPosition.maxHeight > structPosition.safeHeight && isGround)
        {
            nowHP -= (structPosition.maxHeight - structPosition.safeHeight);
            structPosition.maxHeight = 0.0f;
        }
    }

    //重力変化とそれに伴う移動の管理
    public void GravityMove()
    {
        //高重力時に金属体だと移動不可
        if (environment.climate[5] && isAttribute[(int)enumAttribute.METAL - 1])
            nowMoveSpeed = 0;
        else
            nowMoveSpeed = structStatus.moveSpeed;

        //低重力時に風属性が付与され金属体ではないとき、上昇し続ける
        if (environment.climate[4] && isAttribute[(int)enumAttribute.WIND - 1]&& !isAttribute[(int)enumAttribute.METAL - 1]) 
        {
            GravityForce();
        }
        else
        {
            //Y座標が0未満の場合
            if(this.transform.position.y < 0)
            {
                setRigidbody.isKinematic = true;
                navMeshAgent.enabled = true;
                setRigidbody.linearVelocity = Vector3.zero;
            }
        }
    }

    //属性の付与管理
    public void AttributeManager()
    {
        for (int i = 0; i < isAttribute.Length; i++)
        {
            //時間経過に伴う属性のオンオフ
            if (isAttribute[i] == true)
            {
                //火と水は共存できず、水が優先される
                if (isAttribute[i] == isAttribute[(int)enumAttribute.WATER - 1])
                {
                    isAttribute[(int)enumAttribute.FIRE - 1] = false;
                }

                attributeTimer[i] -= Time.deltaTime;

                if (attributeTimer[i] <= 0.0f)
                {
                    isAttribute[i] = false;
                    attributeTimer[i] = 0.0f;
                }

                uIBase.IsAttribute(i);
            }
            else
            {
                attributeTimer[i] = attributeLimit[i];
            }
        }
    }

    //状態異常の付与管理
    public void AbnormalManager()
    {
        //状態異常を付与するための条件
        {
            //風属性派生(水 + 風)
            if (isAttribute[(int)enumAttribute.WATER - 1] && isAttribute[(int)enumAttribute.WIND - 1])
            {
                //カビ(+ 草)
                if (isAttribute[(int)enumAttribute.GRASS - 1])
                    isAbnormal[(int)enumAbnormal.MOLD] = true;
                //腐食(+ 金属)
                else if (isAttribute[(int)enumAttribute.METAL - 1])
                    isAbnormal[(int)enumAbnormal.CORROSION] = true;
            }
            //その他属性派生
            else
            {
                //燃焼(火 + 草)
                if (isAttribute[(int)enumAttribute.FIRE - 1] && isAttribute[(int)enumAttribute.GRASS - 1])
                    isAbnormal[(int)enumAbnormal.BURNING] = true;
                //活性化(水 + 草)
                else if (isAttribute[(int)enumAttribute.WATER - 1] && isAttribute[(int)enumAttribute.GRASS - 1])
                    isAbnormal[(int)enumAbnormal.ACTIVE] = true;
            }

            for (int i = 0; i < isAbnormal.Length; i++)
            {
                if (isAbnormal[i] == true)
                {
                    uIBase.IsAbnormal(i);
                }
            }
        }

        //気候変化による変化
        {
            //猛暑：草とカビを無効化
            if (environment.climate[0])
            {
                isAttribute[(int)enumAttribute.GRASS - 1] = false;
                isAbnormal[(int)enumAbnormal.MOLD] = false;
            }
            //豪雨：火と燃焼を無効化・常時水を付与
            if (environment.climate[1])
            {
                isAttribute[(int)enumAttribute.FIRE - 1] = false;
                isAttribute[(int)enumAttribute.WATER - 1] = true;
                isAbnormal[(int)enumAbnormal.BURNING] = false;
            }
            //豊穣：燃焼の効果時間延長
            if (environment.climate[2] && isAbnormal[(int)enumAbnormal.BURNING])
                abnormalLimit[(int)enumAbnormal.BURNING] = abnormalLimit[(int)enumAbnormal.BURNING] + 10;
            else
                abnormalLimit[(int)enumAbnormal.BURNING] = abnormalLimit[(int)enumAbnormal.BURNING];
        }
    }

    //状態異常の時間系の管理
    public void StateManager()
    {
        for (int i = 0; i < isAbnormal.Length; i++)
        {
            if (isAbnormal[i])
            {
                //
                if (abnormalTimer == 0)
                {
                    abnormalTimer = abnormalLimit[i];
                }
                //全属性の付与を解除する
                for (int j = 0; j < isAttribute.Length; j++)
                {
                    isAttribute[j] = false;
                    uIBase.IsAttribute(j);
                }

                //持続時間の計測を開始
                abnormalTimer -= Time.deltaTime;

                //時間切れになるとかかっている状態異常を解除する
                if (abnormalTimer <= 0.0f)
                {
                    isAbnormal[i] = false;
                    uIBase.IsAbnormal(i);

                    //スリップ間隔を設定できるようにし、
                    //状態異常を全て解除し、
                    //効果時間をリセット
                    setDuration = false;
                    abnormalTimer = 0;
                }
                else
                {
                    uIBase.IsAbnormal(i);
                }
                    
                //間隔の設定
                if (!setDuration)
                {
                    for (int k = 0; k < abnormalFrequency.Length;k++)
                    {
                        //時間の設定
                        if (isAbnormal[k])
                        {
                            frequency = abnormalFrequency[k];
                        }
                    }

                    //設定を何回もしないようにするため
                    setDuration = true;
                }
                //ダメージ間隔の計測
                frequency -= Time.deltaTime;

                //割合処理
                if (frequency <= 0.0f)
                {
                    Abnormal();
                }

                break;
            }
        }
    }

    //状態異常
    public void Abnormal()
    {
        for (int i = 0; i < isAbnormal.Length; i++)
        {
            if (isAbnormal[(int)enumAbnormal.BURNING])
            {
                //暴風時は、ダメージ倍率を上昇
                if (environment.climate[3])
                    HPManager((structStatus.maxHP * ((abnormalRatio[i] / 100) * environmentalChange[i])) * -1);
                else
                    HPManager((structStatus.maxHP * (abnormalRatio[i] / 100)) * -1);
            }
            else if(isAbnormal[(int)enumAbnormal.ACTIVE])
            {
                //豊穣時は、回復量を上昇
                if (environment.climate[2])
                    HPManager(structStatus.maxHP * ((abnormalRatio[i] / 100) * environmentalChange[i]));
                else
                    HPManager(structStatus.maxHP * (abnormalRatio[i] / 100));
            }
            else
            {
                HPManager((structStatus.maxHP * (abnormalRatio[i] / 100)) * -1);
            }

            frequency = abnormalFrequency[i];
        }
    }

    public void GravityForce()
    {
        navMeshAgent.enabled = false;                                                        //NavMeshAgentを無効化
        setRigidbody.isKinematic = false;                                                    //RigidbodyのisKinematicを無効化
        setRigidbody.AddForce(Vector3.up * gravityForce * Time.deltaTime, ForceMode.Impulse);//上昇する
    }

    //着地判定
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            navMeshAgent.updatePosition = true;
        }
    }

    //離陸判定
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = false;
        }
    }
}