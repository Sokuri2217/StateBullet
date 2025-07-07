using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed;              //回転速度値
    public Vector2 minMaxRotation;           //最小大値回転値
    private Vector2 nowRotation;             //現在の回転値
    public PlayerController playerController;//"PlayerController"

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //回転値を取得する
        Vector2 angles = transform.eulerAngles;
        nowRotation = angles;
    }

    public void MoveCamera()
    {
        //マウスの移動量取得
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        //カメラ回転の更新
        nowRotation.x += mouseX * rotationSpeed;
        nowRotation.y -= mouseY * rotationSpeed;
        nowRotation.y = Mathf.Clamp(nowRotation.y, minMaxRotation.x, minMaxRotation.y);
        // 回転を反映
        transform.rotation = Quaternion.Euler(nowRotation.y, nowRotation.x, 0.0f);
    }
}
