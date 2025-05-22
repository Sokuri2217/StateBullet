using Unity.VisualScripting;
using UnityEngine;

public class EnemyBulletBase : BulletBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    public void OnTriggerEnter(Collider other)
    {
        //�v���C���[�ɓ��������Ƃ�
        if (other.gameObject.tag == "Player")
        {
            //�v���C���[�̃X�N���v�g���擾���āAHP�����炷
            character = other.gameObject.GetComponent<TestPlayer>();
            character.currentHP -= currentAttack;
        }
        //BulletMaster�ȊO�̃I�u�W�F�N�g�ɓ��������Ƃ�
        if (other.gameObject.tag != BulletMaster)
        {
            //���g������
            Destroy(gameObject);
        }
    }
}
