using UnityEngine;

public class GolemAttack : EnemyBulletBase
{
    [Header("当たり判定の持続")]
    public float activeLimit; //判定の有効時間
    public float activeTimer; //計測用

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        activeTimer += Time.deltaTime;

        //時間経過で削除
        if (activeTimer >= activeLimit)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);

        //プレイヤーに当たったとき
        if (collision.gameObject.tag == "Player")
        {
            //プレイヤーの体力を減らしたうえで自身を削除
            PlayerController playerController = collision.GetComponent<PlayerController>();
            Destroy(gameObject);
        }
    }
}
