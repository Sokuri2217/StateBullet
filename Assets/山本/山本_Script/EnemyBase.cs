using UnityEngine;

public class EnemyBase : CharacterBase
{ 
    [Header("�U���֘A")]
    public float shotInterval;        //�e�̔��ˊԊu
    public float minInterval;         //�Ԋu�̍ŒZ�l
    public float maxInterval;         //�Ԋu�̍Œ��l
    public float shotTimer;           //���˂܂ł̌v���p
    public float bulletSpeed;         //�e��
    public GameObject[] bulletPrefab; //�e��Prefab�����邽�߂̕ϐ�
    public float bulletPosY;          //�e�𐶐�����Ƃ��̈ʒu�����p

    [Header("�_���[�W�t���O")]
    public bool isDamage; //�_���[�W���󂯂����ǂ���

    [Header("�X�N���v�g�Q��")]
    public EnemyBulletBase enemyBullet; //�G�̒e

    //�f�o�b�O�p
    public bool test;
    public bool up;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //���ˊԊu�̏���ݒ�
        shotInterval = Random.Range(minInterval, maxInterval);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //�̗͂̊Ǘ�
        HPManager();

        //�U���̊Ԋu
        AttackInterval();

        //�f�o�b�O�p
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (up)
                {
                    if (!test)
                    {
                        Time.timeScale = 0;
                        test = true;
                        up = false;
                    }
                    else
                    {
                        Time.timeScale = 1;
                        test = false;
                        up = false;
                    }
                }
            }
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                up = true;
            }
        }  
    }

    //HP�̊Ǘ�
    public void HPManager()
    {
        //�̗͂�0�����ɂȂ�Ȃ��悤�ɂ��A
        //0�ȉ��ɂȂ������������
        if (currentHP <= 0.0f) 
        {
            currentHP = 0.0f;

            Destroy(gameObject);
        }
        //�̗͂��ő�l������Ȃ��悤�ɂ���
        if (currentHP >= maxHP) 
        {
            currentHP = maxHP;
        }
    }

    //�U���̊Ԋu
    public void AttackInterval()
    {
        //���˂܂ł̎��Ԃ��v��
        shotTimer += Time.deltaTime;

        //shotTimer��shotInterval�̒l�������ɂȂ�����
        if(shotTimer >= shotInterval)
        {
            //�e�𔭎�
            Shot();
            //���ˊԊu���Đݒ�
            shotInterval = Random.Range(minInterval, maxInterval);
            //�^�C�}�[��0�ɖ߂�
            shotTimer = 0;
        }
    }

    //���ˏ���
    void Shot()
    {
        Vector3 bulletPos = this.transform.position;
        bulletPos.y += bulletPosY;
        GameObject bullet = Instantiate(bulletPrefab[0], bulletPos, Quaternion.identity); //�e�𐶐�
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(this.transform.forward * bulletSpeed); //�L�����N�^�[�������Ă�������ɒe�ɗ͂�������
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PBullet"))
        {
            //�_���[�W���󂯂�
            isDamage = true;
        }
    }
}
