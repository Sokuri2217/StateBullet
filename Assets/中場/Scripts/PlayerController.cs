using UnityEngine;

public class PlayerController : CharacterBase
{
    public CameraController cameraController;//�J�����R���g���[���[

    protected override void Start()
    {
        base.Start();//�p����̊֐�"Start"�����s����
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();//�p����̊֐�"Update"�����s����

        // �ړ��ʂ̌v�Z
        float horizontal = Input.GetAxis("Horizontal");//AD�L�[
        float vertical = Input.GetAxis("Vertical");    //WS�L�[
        //�J�����̑O�㍶�E�����Ɉړ�
        Vector3 move = cameraController.transform.forward * vertical + cameraController.transform.right * horizontal;
        transform.position += move * moveSpeed * Time.deltaTime;
    }
}
