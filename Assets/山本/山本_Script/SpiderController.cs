using UnityEngine;
using UnityEngine.AI;

public class SpiderController : EnemyBase
{
    [Header("攻撃関連")]
    public float attackDistancce;  //攻撃可能になる距離
    public bool isAttack;          //攻撃中

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
            // ターゲットとの距離を計算
            float distanceToTarget = Vector3.Distance(transform.position, transformPlayer.position);

            //一定距離まで近づいたら攻撃する
            if (distanceToTarget <= attackDistancce && !isAttack)
            {
                isAttack = true;
                animator.SetFloat("SpiderMove", 0);
                Shot();
            }
            else
            {
                //プレイヤーに向かって進む
                navMeshAgent.isStopped = false;
                navMeshAgent.speed = structStatus.moveSpeed;
                navMeshAgent.SetDestination(transformPlayer.position);

                //プレイヤーの方を向く(水平回転のみ)
                Vector3 direction = (transformPlayer.position - transform.position).normalized;
                direction.y = 0;
                transform.forward = direction;

                animator.SetFloat("SpiderMove", 1);
            }
        }
    }

    public void Shot()
    {
        GameObject bullet = Instantiate(objBullet, objShotPosition.transform.position, this.transform.rotation);
        //発射した弾から"PlayerBulletBase"を取得する
        EnemyBulletBase enemyBulletBase = bullet.GetComponent<EnemyBulletBase>();
        //引数を渡す
        enemyBulletBase.BulletOwner(this.tag);//名前
        animator.SetTrigger("Attack");
        Invoke("AttackPossible", shotInterval);
    }

    //再び攻撃できるようにする
    public void AttackPossible()
    {
        isAttack = false;
        //次回の攻撃タイミングを設定
        shotInterval = Random.Range(minInterval, maxInterval);
    }
}
