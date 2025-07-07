using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public float bulletSpeed;      //弾速
    public float currentAttack;    //現在の攻撃力
    public string bulletOwner;     //弾の持ち主
    protected int bulletAttribute; //弾の属性
    public Environment environment;

    [Header("属性弾の火力(通常,火,水,草,風,金属,爆破)")]
    public float[] bulletPower = new float[7];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        environment = GameObject.Find("VirtualEnvironment").GetComponent<Environment>();
    }

    protected virtual void Update()
    {
        this.transform.position += bulletSpeed * transform.forward * Time.deltaTime;//前方向に移動する
    }

    //弾の持主
    public void BulletOwner(string name)
    {
        bulletOwner = name;
    }

    //属性セット
    public void SetAttribute(int number)
    {
        bulletAttribute = number;//引数を弾の属性にセットする
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        //BulletMaster以外のオブジェクトに当たったとき
        if (collision.gameObject.tag != bulletOwner)
        {
            //自身を消去
            Destroy(this.gameObject);
        }
    }
}
