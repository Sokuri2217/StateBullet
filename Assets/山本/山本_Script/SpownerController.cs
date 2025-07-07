using UnityEngine;
using UnityEngine.AI;

public class SpownerController : CharacterBase
{
    public float fleeDistance = 20f;  // 逃げる距離
    public float searchRadius = 25f;  // 逃げる範囲の検索半径

    [Header("スポーン関係")]
    public GameObject[] spownEnemy;
    public float spownLimit;
    public float spownTimer;
    public int maxSpown;
    public int currentSpown;
    public float currentSpownUp;
    public float spownUp;
    public int currentBreak;

    [Header("バリア関係")]
    public GameObject[] spownerBarrier;
    public GameObject[] barrierPos;
    public GameObject barrierObject;
    public float barrierLimit;
    public float barrierTimer;
    public int maxBarrier;
    public int currentBarrier;

    [Header("回転速度")]
    public float rotationSpped;

    [Header("プレイヤー参照")]
    public Transform playerPos;

    private SpownerSetting spownerSetting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        //プレイヤーのtransform情報を取得
        playerPos = GameObject.FindWithTag("Player").transform;
        //スポナーの中身を設定
        spownerSetting = GameObject.Find("SelectStageNum").GetComponent<SpownerSetting>();
        //バリアの初回生成
        for (int i = 0; i < maxBarrier; i++)
        {
            spownerBarrier[i] = Instantiate(barrierObject, barrierPos[i].transform);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //プレイヤーから逃げる処理
        AwayMove();

        //スポーン管理
        SpownerCount();

        //バリア回復
        HealBarrier();

        //Y軸を中心に回転させる
        transform.Rotate(new Vector3(0, rotationSpped, 0));
    }

    //関数"HPManager(体力情報)"
    public override void HPManager(float number)
    {
        base.HPManager(number);

        //uIEnemy.SetUIHP(NowHP);

        if (NowHP <= 0)
        {
            Destroy();
        }
    }

    //プレイヤーから逃げる処理
    public void AwayMove()
    {
        ////プレイヤーから逃げる
        //navMeshAgent.isStopped = false;
        //navMeshAgent.speed = structStatus.moveSpeed;

        //// プレイヤーの反対方向に向かって逃げる位置を計算
        //Vector3 fleeDir = (transform.position - playerPos.position).normalized;
        //Vector3 fleePosition = transform.position + fleeDir * 1f;

        //navMeshAgent.SetDestination(fleePosition);

        navMeshAgent.speed = structStatus.moveSpeed;

        // プレイヤーの方向に反対向きの単位ベクトルを計算
        Vector3 fleeDirection = (transform.position - playerPos.position).normalized;

        // 逃げる目的地をプレイヤーから離れる方向に計算
        Vector3 desiredFleePos = transform.position + fleeDirection * fleeDistance;

        // desiredFleePosがNavMesh上に存在するか確認
        NavMeshHit hit;
        if (NavMesh.SamplePosition(desiredFleePos, out hit, searchRadius, NavMesh.AllAreas))
        {
            // 計算した位置がプレイヤーからさらに遠いか確認
            if ((hit.position - playerPos.position).sqrMagnitude > (transform.position - playerPos.position).sqrMagnitude)
            {
                // プレイヤーからさらに遠い位置が見つかれば、その位置に向かう
                navMeshAgent.SetDestination(hit.position);
            }
            else
            {
                // そうでなければ、最適な逃げ場所を探す
                FindBestEscapePoint();
            }
        }
        else
        {
            // NavMesh上に無理に行けない位置だった場合、最適な逃げ場所を探す
            FindBestEscapePoint();
        }
    }

    //スポーン管理
    public void SpownerCount()
    {
        if (currentSpown < maxSpown) 
        {
            spownTimer += Time.deltaTime;

            if (spownTimer >= (spownLimit * currentSpownUp)) 
            {
                GameObject enemy = Instantiate(spownEnemy[spownerSetting.select], this.transform.position, Quaternion.identity);
                currentSpown++;
                spownTimer = 0.0f;
            }
        }
    }

    //バリア回復
    public void HealBarrier()
    {
        if (currentBarrier == 0)  
        {
            barrierTimer += Time.deltaTime;

            if (barrierTimer >= barrierLimit) 
            {
                for (int i = 0; i < maxBarrier; i++) 
                {
                    spownerBarrier[i] = Instantiate(barrierObject, barrierPos[i].transform);
                }
                barrierTimer = 0.0f;
                currentSpownUp *= spownUp;
            }
        }
    }

    // 逃げるための最適な位置を見つける
    void FindBestEscapePoint()
    {
        Vector3 bestPos = transform.position;  // 初期位置を最適な位置として設定
        float bestDist = 0f;  // 最適位置との距離

        // 周囲をランダムに10回探索して、最もプレイヤーから遠い位置を見つける
        for (int i = 0; i < 10; i++)
        {
            // ランダムな方向に探索範囲を広げる
            Vector3 randomDir = Random.insideUnitSphere * searchRadius;
            randomDir += transform.position;

            // ランダムな位置がNavMesh上に存在するか確認
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDir, out hit, 2f, NavMesh.AllAreas))
            {
                // プレイヤーからの距離を計算
                float dist = (hit.position - playerPos.position).sqrMagnitude;

                // プレイヤーから最も遠い位置を更新
                if (dist > bestDist)
                {
                    bestDist = dist;
                    bestPos = hit.position;
                }
            }
        }

        // 最もプレイヤーから遠い位置を目的地に設定
        navMeshAgent.SetDestination(bestPos);
    }
}