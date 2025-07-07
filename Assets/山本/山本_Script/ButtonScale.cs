using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("拡大・縮小倍率")]
    public Vector3 upScale;     //拡大
    public Vector3 normalScale; //縮小

    [Header("SE関連")]
    public AudioSource se;       //コンポーネント参照
    public AudioClip selectClip; //ボタンをした時の音
    public AudioClip onClip;     //ボタンの上にカーソルが乗った音

    [Header("外部参照用")]
    public bool isActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        se = GameObject.Find("SE").GetComponent<AudioSource>();

        //初期倍率を縮小率に設定
        normalScale = transform.localScale;
        //縮小率の1.2倍の値を拡大率に設定
        upScale = normalScale * 1.2f;
    }

    //カーソルがボタンに乗ったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        //ボタンを拡大
        //SEを鳴らす
        transform.localScale = upScale;
        se.PlayOneShot(onClip);
        isActive = true;
    }

    //カーソルがボタンに乗っていないとき
    public void OnPointerExit(PointerEventData eventData)
    {
        //ボタンを元のサイズに縮小
        transform.localScale = normalScale;
        isActive = false;
    }
}
