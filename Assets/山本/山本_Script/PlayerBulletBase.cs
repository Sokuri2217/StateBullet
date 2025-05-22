using UnityEngine;

public class PlayerBulletBase : BulletBase
{
    [Header("�g�p���̑����e")]
    public bool[] useElement;  //�g�p�����e
    [Header("�e�����e�̉Η�")]
    public float fire;
    public float water;
    public float wind;
    public float explosion;
    public float metal;
    public float grass;
    public float normal;

    public enum  Element
    {
        FIRE,
        WATER,
        WIND,
        EXPLOSION,
        METAL,
        GRASS,
        NORMAL
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //�����e��ʏ�e�ɐݒ�
        currentAttack = (int)Element.NORMAL;
        useElement[(int)Element.NORMAL] = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        //�v���C���[�ɓ��������Ƃ�
        if (other.gameObject.tag == "Enemy")
        {
            //�v���C���[�̃X�N���v�g���擾���āAHP�����炷
            character = other.gameObject.GetComponent<CharacterBase>();
            if (useElement[(int)Element.FIRE])
            {
                currentAttack = fire * character.fire;
                character.isFire = true;
            }
            if (useElement[(int)Element.WATER])
            {
                currentAttack = water * character.water;
                character.isWater = true;
            }
            if (useElement[(int)Element.WIND])
            {
                currentAttack = wind * character.wind;
                character.isWind = true;
            }
            if (useElement[(int)Element.EXPLOSION])
            {
                currentAttack = explosion * character.explosion;
            }
            if (useElement[(int)Element.METAL])
            {
                currentAttack = metal * character.metal;
                character.isMetal = true;
            }
            if (useElement[(int)Element.GRASS])
            {
                currentAttack = grass * character.grass;
                character.isGrass = true;
            }
            if (useElement[(int)Element.NORMAL])
            {
                currentAttack = normal * character.normal;
            }

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
