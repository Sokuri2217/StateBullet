using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public float currentAttack;  //���݂̍U����
    public string BulletMaster;//�e�̎�����̃^�O�ۑ�

    public CharacterBase character;
    public Environment environment;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        environment = GameObject.Find("VirtualEnvironment").GetComponent<Environment>();
    }
}
