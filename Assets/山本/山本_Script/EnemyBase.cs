using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float maxHP;     //�ő�HP
    public float currentHP; //���݂�HP

    public float moveSpeed;     //�ړ����x
    public float originalSpeed; //��{�ړ����x

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //���݂�HP���ő�l�ɐݒ�
        currentHP = maxHP;
        //moveSpeed�̒l����{�ړ����x�ɐݒ�
        originalSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
