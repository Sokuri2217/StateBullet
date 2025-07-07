using UnityEngine;

public class PlayerBulletBase : BulletBase
{
    public GameObject[] bulletEffect;

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
        //
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyBase enemyBase = collision.gameObject.GetComponent<EnemyBase>();

            currentAttack = bulletPower[bulletAttribute] * enemyBase.attributeDamage[bulletAttribute];

            Instantiate(bulletEffect[bulletAttribute], this.transform.position, Quaternion.identity);
            enemyBase.HPManager(currentAttack);

            if (bulletAttribute >= (int)CharacterBase.enumAttribute.FIRE &&
                bulletAttribute <= (int)CharacterBase.enumAttribute.METAL)
            {
                enemyBase.IsAttribute[bulletAttribute - 1] = true;
            }
        }

        if (collision.gameObject.tag == "Spowner")
        {
            SpownerController spownerController = collision.gameObject.GetComponent<SpownerController>();

            currentAttack = bulletPower[bulletAttribute] * spownerController.attributeDamage[bulletAttribute];

            Instantiate(bulletEffect[bulletAttribute], this.transform.position, Quaternion.identity);
            spownerController.HPManager(currentAttack * -1);

            if (bulletAttribute >= (int)CharacterBase.enumAttribute.FIRE &&
                bulletAttribute <= (int)CharacterBase.enumAttribute.METAL)
            {
                spownerController.IsAttribute[bulletAttribute - 1] = true;
            }
        }

        if(collision.gameObject.tag == "Barrier")
        {
            BarrierController barrierController = collision.gameObject.GetComponent<BarrierController>();
            barrierController.hp--;
        }
    }
}