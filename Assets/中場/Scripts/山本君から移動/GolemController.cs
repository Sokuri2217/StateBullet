using UnityEngine;

public class GolemController : EnemyBase
{
    public float shotDistance;  //遠距離攻撃できる距離
    public float fightDistance; //近距離攻撃できる距離
    public int attack;          //攻撃方法判別

    public GameObject fightCollider; //近距離攻撃の当たり判定

    public enum AttackMode
    {
        FIGHT,
        SHOT,
    }

    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //protected override void Start()
    //{
    //    base.Start();
    //}

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //移動処理
        MoveGolem();
    }

    void MoveGolem()
    {
        if (transformPlayer != null)
        {
            // ターゲットとの距離を計算
            float distanceToTarget = Vector3.Distance(transform.position, transformPlayer.position);

            //NavMeshAgentによる追跡
            if (navMeshAgent != null)
            {
                //プレイヤーが一定距離より離れていると追跡
                if (distanceToTarget > shotDistance ||
                    nowHP <= structStatus.maxHP / 2)
                {
                    //追いかける
                    navMeshAgent.isStopped = false;
                    navMeshAgent.speed = structStatus.moveSpeed;
                    navMeshAgent.SetDestination(transformPlayer.position);

                    //プレイヤーが一定距離より近くにいると近距離攻撃する
                    if (distanceToTarget < fightDistance)
                    {
                        navMeshAgent.isStopped = true;
                        navMeshAgent.speed = 0;
                        navMeshAgent.ResetPath();

                        //間隔
                        Interval((int)AttackMode.FIGHT);
                    }
                }
                //遠距離攻撃可能な距離のとき
                else
                {
                    navMeshAgent.isStopped = true;
                    navMeshAgent.speed = 0;
                    navMeshAgent.ResetPath();

                    //間隔
                    Interval((int)AttackMode.SHOT);
                }

                if (navMeshAgent.speed > 0.0f)
                {
                    animator.SetFloat("GolemMove", 1);
                }
                else
                {
                    animator.SetFloat("GolemMove", 0);
                }
            }

            //プレイヤーの方を向く(水平回転のみ)
            Vector3 direction = (transformPlayer.position - transform.position).normalized;
            direction.y = 0;
            transform.forward = direction;
        }
    }

    //間隔
    public void Interval(int attackNum)
    {
        //発射までの時間計測
        shotTimer += Time.deltaTime;

        //shotTimerがshotInterval以上になったとき
        if (shotTimer >= shotInterval)
        {
            //タイマーをリセット
            shotTimer = 0;
            switch(attackNum)
            {
                case (int)AttackMode.SHOT:
                    //攻撃手段を遠距離に設定
                    attack = (int)AttackMode.SHOT;
                    //攻撃アニメーションを再生
                    animator.SetTrigger("Shot");
                    //弾を発射
                    break;
                case (int)AttackMode.FIGHT:
                    //攻撃手段を近距離に設定
                    attack = (int)AttackMode.FIGHT;
                    //攻撃アニメーションを再生
                    animator.SetTrigger("Fight");
                    //弾を発射
                    Invoke("Fight", 0.4f);
                    break;
            }
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

    public void Fight()
    {
        GameObject fight = Instantiate(fightCollider, objShotPosition.transform.position, this.transform.rotation);
        //
        GolemAttack golemAttack = fight.GetComponent<GolemAttack>();
        //引数を渡す
        golemAttack.BulletOwner(this.tag);//名前
        //次回の攻撃タイミングを設定
        shotInterval = Random.Range(minInterval, maxInterval);
    }
}
