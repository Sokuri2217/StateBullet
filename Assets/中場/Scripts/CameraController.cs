using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed;   //��]���x�l
    public Vector2 minMaxRotation;//�ŏ���l��]�l
    private Vector2 nowRotation;  //���݂̉�]�l

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //��]�l���擾����
        Vector2 angles = transform.eulerAngles;
        nowRotation = angles;
    }

    // Update is called once per frame
    void Update()
    {
        //�}�E�X�̈ړ��ʎ擾
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //�J������]�̍X�V
        nowRotation.x += mouseX * rotationSpeed;
        nowRotation.y -= mouseY * rotationSpeed;
        nowRotation.y = Mathf.Clamp(nowRotation.y, minMaxRotation.x, minMaxRotation.y);
        // ��]�𔽉f
        transform.rotation = Quaternion.Euler(nowRotation.y, nowRotation.x, 0.0f);
    }
}
