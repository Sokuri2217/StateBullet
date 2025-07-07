using Unity.VisualScripting;
using UnityEngine;

public class EnemyBulletBase : BulletBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);

        //プレイヤーに当たったとき
        if (collision.gameObject.tag == "Player")
        {
            //プレイヤーのスクリプトを取得して、HPを減らす
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.HPManager(currentAttack * -1);
        }
    }
}
