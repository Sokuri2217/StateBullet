using UnityEngine;
using UnityEngine.UI;

public class CanvasUI : MonoBehaviour
{
    public Slider sliderHp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�v���C���[�̗̑͒l���擾���đ̗̓X���C�_�[�ɓ����
        sliderHp.minValue = 0;//�ŏ��l
        sliderHp.maxValue = 1;//�ő�l
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
