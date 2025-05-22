using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("��b�\��")]
    public float maxHP;         //�ő�HP
    public float currentHP;     //���݂�HP
    public float moveSpeed;     //�ړ����x
    public float originalSpeed; //��{�ړ����x

    [Header("�����֘A")]
    public Vector3 pos;         //�ʒu�擾
    public float maxHeight;     //���n����O�̍ő卂�x
    public float safeHeight;    //�������Ă��_���[�W�ɂȂ�Ȃ����x
    public bool onGround;       //���n����

    [Header("��Ԉُ�֘A")]
    public float stateCount;     //�������Ԃ̌v���p
    public float sustainability; //��Ԉُ�̎�������
    public bool isState;         //��Ԉُ�ɂȂ��Ă��邩

    [Header("�X���b�v�_���[�W�̕p�x")]
    public float frequency;  //�J��Ԃ��Ԋu
    public float moldFre;    //�J�r�̊Ԋu
    public float corrFre;    //���H�̊Ԋu
    public float burnFre;    //�R�Ă̊Ԋu
    public float actiFre;    //�������̊Ԋu
    public bool setDuration; //�Ԋu��ݒ肵�����ǂ���(���x���Đݒ肵�Ȃ��悤��)

    [Header("��Ԉُ�̔���")]
    public bool isMold;      //�J�r��Ԃ��ǂ���
    public bool isCorrosion; //���H��Ԃ��ǂ���
    public bool isBurning;   //�R�ď�Ԃ��ǂ���
    public bool isActive;    //��������Ԃ��ǂ���

    [Header("�����l")]
    public float mold;      //�J�r
    public float corrosion; //���H
    public float burning;   //�R��
    public float active;    //������

    [Header("�����̏����l")]
    public float firstMold;      //�J�r
    public float firstCorrosion; //���H
    public float firstBurning;   //�R��
    public float firstActive;    //������

    //�_���[�W�l�@�΁@�@���@�@���@�@�@���j�@�@�@�����@�@���@�@�ʏ�@
    //public float fire, water, wind, explosion, metal, grass, normal;
    //�����l
    //public float firstFire, firstWater, firstWind, firstExplosion, firstMetal, firstGrass, firstNormal;
    [Header("�t�^���̑���")]
    public bool isFire;  //��
    public bool isWater; //��
    public bool isWind;  //��
    public bool isMetal; //����
    public bool isGrass; //��

    [Header("�����e�̔{��")]
    public float fire;
    public float water;
    public float wind;
    public float explosion;
    public float metal;
    public float grass;
    public float normal;

    [Header("���ɂ��ω�")]
    public float downSpeed; //�ړ����x�ቺ
    public float burnUp;    //�R�Ċ�������
    public float longBurn;  //�R�Ď�������
    public float actiUp;    //��������������

    [Header("�X�N���v�g�Q��")]
    public Environment environment;     //��

    [Header("�A�j���[�V����")]
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //���݂�HP���ő�l�ɐݒ�
        currentHP = maxHP;
        //moveSpeed�̒l����{�ړ����x�ɐݒ�
        originalSpeed = moveSpeed;
        //��Ԉُ�̎������Ԃ�ݒ�(30�b)
        sustainability = 30.0f;
        //�������擾
        environment = GameObject.Find("VirtualEnvironment").GetComponent<Environment>();
        //Animator�����擾
        animator = GetComponent<Animator>();
        //�����_���[�W�̏�����
        firstMold = mold;
        firstCorrosion = corrosion;
        firstBurning = burning;
        firstActive = active;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //�I�u�W�F�N�g�̍��W�擾
        pos = transform.position;

        //�����_���[�W
        FallDamage();

        //����Ԃɂ��ω����Ǘ�
        Environment();

        //�����̕t�^�y�я�Ԉُ�t�^�̊Ǘ�
        StateEnchant();

        //��Ԉُ�̊Ǘ�
        StateManager();
    }

    //�����_���[�W
    public void FallDamage()
    {
        //�ō��x���X�V��������
        if (pos.y > maxHeight)
        {
            maxHeight = pos.y;
        }

        //���n���ɋ����̂���Ȃ������̍��x�ȏォ��̗������Ƀ_���[�W
        if (!isMetal && maxHeight > safeHeight && onGround)
        {
            Debug.Log("�ɂ��I");
            currentHP -= (maxHeight - safeHeight);
            maxHeight = 0.0f;
        }
    }

    //����Ԃɂ��ω����Ǘ�
    public void Environment()
    {
        if (environment.change)
        {
            //���J(�����R�Ė����E�펞���t��)
            if (environment.rain)
            {
                isWater = true;
                isBurning = false;
            }
            //�ҏ�(�J�r����������)
            if (environment.hot)
            {
                isMold = false;
            }
            //�\��(�R�Ă̊��������E�ړ����x�ቺ)
            if (environment.storm)
            {
                burning *= burnUp;
                moveSpeed *= downSpeed;
            }
            //���d��(�����̂ňړ��s��)
            if (environment.high_gravity)
            {
                if (isMetal)
                    moveSpeed = 0.0f;
                else
                    moveSpeed = originalSpeed;
            }
            //��d��(�����󂯂�ƈ�莞�ԕ��V�E�����̂Ŗ�����)
            if (environment.low_gravity)
            {
                if (!isMetal)
                {

                }
            }
            //�L��(�������̊��������E�R�Ă̎�������)
            if (environment.abundant)
            {
                active *= actiUp;
                if (isBurning)
                    frequency += longBurn;
            }
        }
    }

    //�����̕t�^�y�я�Ԉُ�t�^�̊Ǘ�
    public void StateEnchant()
    {
        //��Ԉُ��t�^���邽�߂̏���
        {
            if (isWater && isWind && isGrass)//�J�r
                isMold = true;
            else if (isWater && isWind && isMetal)//���H
                isCorrosion = true;
            else if (isFire && isGrass)//�R��
                isBurning = true;
            else if (isWater && isGrass)//������
                isActive = true;
        }

        //�����t�^�ɂ����鑊��
        {
            //�΂Ɛ��͋����ł����A�����D�悳���
            if (isWater)
                isFire = false;
        }

        //��Ԉُ�ɂȂ������Ƃ��m�F����t���O��true�ɂ���
        if (isMold || isCorrosion || isBurning || isActive)
            isState = true;

        //��Ԉُ�ɂȂ����Ƃ��A�S�����̕t�^����������
        if (isState)
        {
            isFire = false;
            isWater = false;
            isWind = false;
            isMetal = false;
            isGrass = false;
        }
    }

    //��Ԉُ�̎��Ԍn�̊Ǘ�
    public void StateManager()
    {
        //�Ȃ�炩�̏�Ԉُ�ɂȂ����Ƃ�
        if (isState)
        {
            //�������Ԃ̌v�����J�n
            stateCount += Time.deltaTime;
            //���݂̃J�E���g���������Ԉȏ�ɂȂ�����J�r����������
            if (stateCount >= sustainability)
                isState = false;
            //�Ԋu�̐ݒ�
            if (!setDuration)
            {
                //���Ԃ̐ݒ�
                //�J�r
                if (isMold)
                    frequency = moldFre;
                //���H
                if (isCorrosion)
                    frequency = corrFre;
                //�R��
                if (isBurning)
                    frequency = burnFre;
                //������
                if (isActive)
                    frequency = actiFre;
                //�ݒ����������Ȃ��悤�ɂ��邽��
                setDuration = true;
            }
            //�_���[�W�Ԋu�̌v��
            frequency -= Time.deltaTime;
            //��������
            if (frequency <= 0.0f)
            {
                //�J�r
                if (isMold)
                    MoldDamage();
                //���H
                if (isCorrosion)
                    CorrDamage();
                //�R��
                if (isBurning)
                    BurningDamage();
                //������
                if (isActive)
                    ActiveRecovery();
            }
        }
        else
        {
            setDuration = false;
            isMold = false;
            isCorrosion = false;
            isBurning = false;
            isActive = false;
        }
    }

    //�J�r��Ԃ̏���
    public void MoldDamage()
    {
        currentHP -= maxHP * (mold / 100);
        frequency = moldFre;
    }
    //���H��Ԃ̏���
    public void CorrDamage()
    {
        currentHP -= maxHP * (corrosion / 100);
        frequency = corrFre;
    }
    //�R�ď�Ԃ̏���
    public void BurningDamage()
    {
        currentHP -= maxHP * (burning / 100);
        //�U���͂��㏸�����鏈��������
    }
    //��������Ԃ̏���
    public void ActiveRecovery()
    {
        currentHP += maxHP * (active / 100);
        frequency = actiFre;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Ground")
        {
            onGround = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = false;
        }
    }
}
