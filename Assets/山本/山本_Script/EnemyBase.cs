using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float maxHP;     //�ő�HP
    public float currentHP; //���݂�HP

    public float moveSpeed;     //�ړ����x
    public float originalSpeed; //��{�ړ����x

    public float stateCount;     //�������Ԃ̌v���p
    public float sustainability; //��Ԉُ�̎�������

    public bool isMold; //�J�r��Ԃ��ǂ���

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        //���݂�HP���ő�l�ɐݒ�
        currentHP = maxHP;
        //moveSpeed�̒l����{�ړ����x�ɐݒ�
        originalSpeed = moveSpeed;
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
    
    //��Ԉُ�̊Ǘ�
    public void StateManager()
    {
        //�J�r���
        if (isMold)
        {
            sustainability = 30.0f;
            InvokeRepeating("MoldDamage", 2.0f, 2.0f);
        }
    }

    //�J�r��Ԃ̏���
    public void MoldDamage()
    {
        currentHP -= maxHP * 0.005f;
    }
}
