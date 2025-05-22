using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public float currentAttack;  //Œ»İ‚ÌUŒ‚—Í
    public string BulletMaster;//’e‚Ì‚¿å‚Ìƒ^ƒO•Û‘¶

    public CharacterBase character;
    public Environment environment;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        environment = GameObject.Find("VirtualEnvironment").GetComponent<Environment>();
    }
}
