using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
    private int nowAnimationNumber;
    private float nowAnimationLength, animationTimer = 0.0f;    //���݂̃A�j���[�V�����̒���, �A�j���[�V�����^�C�}�[;           
    private bool isAnimation = true;           
    private string nowAnimationName;                            //���݂̃A�j���[�V�����̖��O
    private HumanoidAnimation humanoidAnimation;                //"enum(HumanoidAnimation)"

    private Animator animator = null;                           //"Animator"
    private RuntimeAnimatorController runtimeAnimatorController;//"RuntimeAnimatorController"

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //���̃I�u�W�F�N�g�̃R���|�[�l���g���擾
        animator = this.GetComponent<Animator>();
        runtimeAnimatorController = animator.runtimeAnimatorController;
        AnimationSet();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimation == false)
        {
            isAnimation = true;
            AnimationSet();
        }
        else if (isAnimation == true)
        {
            AnimationWait();
        }
    }

    ////�֐�"AnimationSet"
    public void AnimationSet()
    {
        nowAnimationNumber = (int)Random.Range((int)HumanoidAnimation.AngryPoint, (int)HumanoidAnimation.Yelling + 1);
        humanoidAnimation = (HumanoidAnimation)nowAnimationNumber;
        nowAnimationName = humanoidAnimation.ToString();

        foreach (AnimationClip clip in runtimeAnimatorController.animationClips)
        {
            if (clip.name == nowAnimationName)
            {
                nowAnimationLength = clip.length;
            }
        }

        AnimationPlay();//�֐�"AnimationPlay"�����s����
    }

    //�֐�"AnimationPlay"
    public void AnimationPlay()
    {
        animator.SetInteger("AnimationNumber", nowAnimationNumber);//"animator(Motion)"��"nowAnimation"��ݒ肵�čĐ�
    }

    //�֐�"AnimationWait"
    public void AnimationWait()
    {
        animationTimer += Time.deltaTime;//"animationTimer"��"Time.deltaTime(�o�ߎ���)"�𑫂�

        if (animationTimer >= nowAnimationLength)
        {
            animationTimer = 0.0f;
            isAnimation = false; 
        }
    }

    public enum HumanoidAnimation
    {
        AngryPoint = 1,
        Cheering = 2,
        Clapping = 3,
        Yelling = 4
    }
}
