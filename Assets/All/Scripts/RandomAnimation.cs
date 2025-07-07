using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
    private int nowAnimationNumber;
    private float nowAnimationLength, animationTimer = 0.0f;    //現在のアニメーションの長さ, アニメーションタイマー;           
    private bool isAnimation = true;           
    private string nowAnimationName;                            //現在のアニメーションの名前
    private HumanoidAnimation humanoidAnimation;                //"enum(HumanoidAnimation)"

    private Animator animator = null;                           //"Animator"
    private RuntimeAnimatorController runtimeAnimatorController;//"RuntimeAnimatorController"

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //このオブジェクトのコンポーネントを取得
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

    ////関数"AnimationSet"
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

        AnimationPlay();//関数"AnimationPlay"を実行する
    }

    //関数"AnimationPlay"
    public void AnimationPlay()
    {
        animator.SetInteger("AnimationNumber", nowAnimationNumber);//"animator(Motion)"に"nowAnimation"を設定して再生
    }

    //関数"AnimationWait"
    public void AnimationWait()
    {
        animationTimer += Time.deltaTime;//"animationTimer"に"Time.deltaTime(経過時間)"を足す

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
