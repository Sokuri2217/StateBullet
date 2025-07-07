using UnityEngine;

public class DeleteEffect : MonoBehaviour
{
    public float deleteLimit; //íœ‚³‚ê‚é‚Ü‚Å‚ÌŽžŠÔ
    public float deleteTimer; //Œv‘ª—p

    // Update is called once per frame
    void Update()
    {
        deleteTimer += Time.deltaTime;

        //ˆê’èŽžŠÔ‚ªŒo‰ß‚·‚é‚Æíœ
        if (deleteTimer >= deleteLimit) 
        {
            Destroy(gameObject);
        }
    }
}
