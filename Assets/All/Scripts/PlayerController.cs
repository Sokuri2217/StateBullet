using UnityEngine;

public class PlayerController : CharacterBase
{
    [Header("現在の属性弾")]
    protected int nowAttributeBullet;              //現在の属性弾
    public float []shotInterval = new float[7];    //発射間隔
    public static float[] shotTimer = new float[7];//発射タイマー
    public bool isShooting = false;                //攻撃中かどうか
    [Header("GameObject")]
    private GameObject objEnemySpowner;            //敵スポナーGameObject
    public CameraController cameraController;      //"CameraController"
    public UIPlayer uIPlayer;                      //"uIPlayer"
    [Header("毒ダメージ関連")]
    public bool isAcid;      //毒状態かどうか
    public float acidDamage; //割合ダメージ
    public float acidLimit;  //ダメージが発生するまでの時間
    public float acidTimer;  //計測用

    public AudioSource se;
    public AudioClip shotSe;
    public AudioClip damageSe;

    public UIStage uiStage; 

    //現在の属性弾プロパティ
    public int NowAttributeBullet
    {
        get { return nowAttributeBullet; }
    }

    protected override void Start()
    {
        base.Start();//継承先の関数"Start"を実行する

        //敵スポナーGameObjectを探して取得する
        objEnemySpowner = GameObject.FindWithTag("Spowner");

        //AudioSourceを取得
        se = GameObject.Find("SE").GetComponent<AudioSource>();

        nowAttributeBullet = (int)enumAttribute.FIRE;

        uIPlayer.SetUIAttributeBullet(nowAttributeBullet);

        //発射タイマーリセット
        for (int i = 0; i < shotTimer.Length; i++)
        {
            shotTimer[i] = shotInterval[i];
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();//継承先の関数"Update"を実行する

        if (!uiStage.isPause)  
        {
            if (objEnemySpowner == null)
            {
                if (!uIPlayer.objUIGameClear.activeSelf)
                {
                    uIPlayer.SetUIGameClear();
                    CursorLock();
                }
            }
            else
            {
                //体力が0より上の場合
                if (NowHP > 0)
                {
                    Active();//関数"Active"を実行する
                }
            }
        }
    }

    //関数"HPManager(数値)"
    public override void HPManager(float number)
    {
        base.HPManager(number);
        uIPlayer.SetUIHP(NowHP);

        if (NowHP <= 0) 
        {
            CursorLock();
        }
    }

    //
    public void CursorLock()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    //関数"Active"
    public void Active()
    {
        if (Time.timeScale == 0) 
            Cursor.lockState = CursorLockMode.Locked; // カーソルを画面中央にロックする

        cameraController.MoveCamera();
        // 移動量の計算
        float horizontal = Input.GetAxis("Horizontal");//ADキー
        float vertical = Input.GetAxis("Vertical");    //WSキー
        //カメラの前後左右方向に移動する
        Vector3 move = cameraController.transform.forward * vertical + cameraController.transform.right * horizontal;
        transform.position += move * nowMoveSpeed * Time.deltaTime;
        //マウスホイールのスクロールを取得する
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if(scroll < 0f || scroll > 0f)
        {
            if (scroll < 0f)
            {
                nowAttributeBullet -= 1;

                if (nowAttributeBullet < 1)
                {
                    nowAttributeBullet = 6;
                }
            }
            else if (scroll > 0f)
            {
                nowAttributeBullet += 1;

                if (nowAttributeBullet > 6)
                {
                    nowAttributeBullet = 1;
                }
            }

            uIPlayer.SetUIAttributeBullet(nowAttributeBullet);
        }

        //shotTimerがshotInterval以上になったとき
        if (shotTimer[0] >= shotInterval[0])
        {
            //マウスを左クリック時
            if (Input.GetMouseButtonDown(0))
            {
                Shot(0);//発射
            }
        }
        //shotTimerがshotInterval以上になったとき
        if (shotTimer[nowAttributeBullet] >= shotInterval[nowAttributeBullet])
        {
            //マウスを右クリック時
            if (Input.GetMouseButtonDown(1))
            {
                Shot(nowAttributeBullet);//発射
            }
            //自分に属性を付与
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Grant(nowAttributeBullet);
            }
        }

        //毒状態の処理
        Acid();

        for (int i = 0; i < shotTimer.Length; i++)
        {
            if (shotTimer[i] < shotInterval[i])
            {
                Interval(i);//間隔
            }
        }
    }

    //間隔
    public void Interval(int number)
    {
        //発射までの時間計測
        shotTimer[number] += Time.deltaTime;
    }

    //発射
    public void Shot(int number)
    {
        shotTimer[number] = 0;//引数番号のタイマーをリセット

        //弾を発射位置からカメラ前方に向けて発射
        GameObject bullet = Instantiate(objBullet, objShotPosition.transform.position, Quaternion.LookRotation(Camera.main.transform.forward));
        //発射した弾から"PlayerBulletBase"を取得する
        PlayerBulletBase playerBulletBase = bullet.GetComponent<PlayerBulletBase>();
        //引数を渡す
        playerBulletBase.BulletOwner(this.tag);//名前
        playerBulletBase.SetAttribute(number); //属性
        //発射音を鳴らす
        se.PlayOneShot(shotSe);
    }

    //付与
    public void Grant(int number)
    {
        shotTimer[number] = 0;//引数番号のタイマーをリセット

        if (number >= (int)enumAttribute.FIRE &&
           number <= (int)enumAttribute.METAL)
        {
            isAttribute[number - 1] = true;
        }
        else if(number == (int)enumAttribute.EXPLOSION)
        {
            if(NowHP > 1)
            {
                HPManager((NowHP / 2) * -1);
            }
        }
    }

    //毒状態の処理
    public void Acid()
    {
        //火が付与されたら毒を解除
        if (isAttribute[(int)enumAttribute.FIRE - 1])
            isAcid = false;

        //毒ダメージの処理
        if (isAcid)
        {
            acidTimer += Time.deltaTime;

            if (acidTimer >= acidLimit)
            {
                nowHP -= structStatus.maxHP * (acidDamage / 100);
                acidTimer = 0.0f;
            }
        }
        else
        {
            acidTimer = 0.0f;
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "EBullet") 
        {
            se.PlayOneShot(damageSe);
        }
    }
}
