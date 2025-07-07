using UnityEngine;

public class EnemyBase : CharacterBase
{
    public float shotInterval;  //発射間隔
    protected float shotTimer;  //発射タイマー
    [Header("GameObject")]
    private GameObject objEnemySpowner;//敵スポナーGameObject
    [Header("プレイヤー参照")]
    protected Transform transformPlayer;//プレイヤーの位置
    [Header("弾発射関連")]
    public float minInterval;        //最短値
    public float maxInterval;        //最長値
    public bool isShooting = false;  //攻撃中かどうか
    [Header("ダメージ判定")]
    public bool isDamage; //ダメージを受けたかどうか
    [Header("スクリプト参照")]
    private SpownerController spownerController;
    private EndlessSpowner endlessSpowner;
    public SpownerSetting spownerSetting;
    public EnemyBulletBase enemyBullet;//弾
    public UIEnemy uIEnemy;
    [Header("アニメーション")]
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        if(GameObject.FindWithTag("Spowner") != null)
        {
            //敵スポナーGameObjectを探して取得する
            objEnemySpowner = GameObject.FindWithTag("Spowner");
            spownerSetting = GameObject.Find("SelectStageNum").GetComponent<SpownerSetting>();
            if (spownerSetting.select != 3)
            {
                spownerController = objEnemySpowner.GetComponent<SpownerController>();
            }
            else
            {
                endlessSpowner = objEnemySpowner.GetComponent<EndlessSpowner>();
            }
        }

        if(GameObject.FindWithTag("Player") != null)
        {
            //プレイヤーのtransform情報を取得
            transformPlayer = GameObject.FindWithTag("Player").transform;
        }

        //発射タイミングの初回設定
        shotInterval = Random.Range(minInterval, maxInterval);
        //Animator情報を取得
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(objEnemySpowner != null)
        {
            if (objEnemySpowner == null)
            {
                Destroy();
            }
        }
    }

    //関数"HPManager(体力情報)"
    public override void HPManager(float number)
    {
        base.HPManager(number);

        uIEnemy.SetUIHP(NowHP);

        if (NowHP <= 0)
        {
            if (spownerSetting.select != 3)
                spownerController.currentSpown--;
            else
                endlessSpowner.currentSpown--;
                Destroy();
        }
    }

    //移動関連
    public void Move3D()
    {
        //プレイヤーの方を向く(水平回転のみ)
        Vector3 direction = (transformPlayer.position - transform.position).normalized;
        direction.y = 0;
        transform.forward = direction;

        //プレイヤーを追いかける
        //transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PBullet"))
        {

            if (nowHP > 0.0f)
            {
                animator.SetTrigger("Hit");
            }
            else
            {
                animator.SetTrigger("Die");
            }
        }
    }
}