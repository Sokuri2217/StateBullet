using UnityEngine;
using UnityEngine.AI;

public class FishController : EnemyBase
{
    [Header("距離制御")]
    public float minDistance = 3f;     // 近すぎたら離れる距離
    public float maxDistance = 8f;     // 遠すぎたら近づく距離

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (transformPlayer != null)
        {
            //敵の移動
            Interval();
            MoveFish();
        }
    }

    //敵の移動
    void MoveFish()
    {
        //ターゲットとの距離を計算
        float distanceToTarget = Vector3.Distance(transform.position, transformPlayer.position);

        //NavMeshAgentによる追跡
        if (navMeshAgent.enabled != false)
        {
            if (distanceToTarget > maxDistance)
            {
                // プレイヤーから離れすぎている → 近づく

                navMeshAgent.isStopped = false;
                navMeshAgent.speed = structStatus.moveSpeed;
                navMeshAgent.SetDestination(transformPlayer.position);

                //プレイヤーの方を向く(水平回転のみ)
                Vector3 direction = (transformPlayer.position - transform.position).normalized;
                direction.y = 0;
                transform.forward = direction;

                //間隔
                Interval();
            }
            else if (distanceToTarget < minDistance)
            {
                // プレイヤーに近すぎる → 反対方向に逃げる
                navMeshAgent.isStopped = false;
                navMeshAgent.speed = structStatus.moveSpeed;

                // プレイヤーの反対方向に向かって逃げる位置を計算
                Vector3 fleeDir = (transform.position - transformPlayer.position).normalized;
                Vector3 fleePosition = transform.position + fleeDir * 5f; // 逃げる距離は任意（例：5m）

                //プレイヤーに背中を向ける(水平回転のみ)
                Vector3 direction = (transform.position - transformPlayer.position).normalized;
                direction.y = 0;
                transform.forward = direction;

                navMeshAgent.SetDestination(fleePosition);
            }
            else
            {
                // ちょうど良い距離 → 止まる
                navMeshAgent.isStopped = true;
                navMeshAgent.speed = 0;
                navMeshAgent.ResetPath();

                //プレイヤーの方を向く(水平回転のみ)
                Vector3 direction = (transformPlayer.position - transform.position).normalized;
                direction.y = 0;
                transform.forward = direction;

                //間隔
                Interval();
            }

            if (navMeshAgent.speed > 0.0f)
            {
                animator.SetFloat("FishMove", 1);
            }
            else
            {
                animator.SetFloat("FishMove", 0);
            }
        }
    }

    //間隔
    public void Interval()
    {
        //発射までの時間計測
        shotTimer += Time.deltaTime;

        //shotTimerがshotInterval以上になったとき
        if (shotTimer >= shotInterval)
        {
            //タイマーをリセット
            shotTimer = 0;
            //攻撃アニメーションを再生
            animator.SetTrigger("Attack");
            //弾を発射
            Invoke("Shot", 0.575f);
        }
    }

    //発射
    public void Shot()
    {
        GameObject bullet = Instantiate(objBullet, objShotPosition.transform.position, this.transform.rotation);
        //発射した弾から"PlayerBulletBase"を取得する
        EnemyBulletBase enemyBulletBase = bullet.GetComponent<EnemyBulletBase>();
        //引数を渡す
        enemyBulletBase.BulletOwner(this.tag);//名前
        //次回の攻撃タイミングを設定
        shotInterval = Random.Range(minInterval, maxInterval);
    }
}