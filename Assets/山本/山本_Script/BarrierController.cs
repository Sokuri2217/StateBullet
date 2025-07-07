using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public float hp;
    public Transform corePos;

    public Material[] barrierMaterial;

    public SpownerController spownerController;

    void Start()
    {
        corePos = GameObject.Find("Spowner_Core").GetComponent<Transform>();
        spownerController = GameObject.Find("Spowner_Core").GetComponent<SpownerController>();
        spownerController.currentBarrier++;
    }

    // Update is called once per frame
    void Update()
    {
        //コアの方を向く(水平回転のみ)
        Vector3 direction = (corePos.position - transform.position).normalized;
        direction.y = 0;
        transform.forward = direction;

        //耐久値によって色を変える
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (hp == 1)
        {
            meshRenderer.material = barrierMaterial[2];
        }
        else if (hp <= (hp * 0.5f))  
        {
            meshRenderer.material = barrierMaterial[1];
        }
        else
        {
            meshRenderer.material = barrierMaterial[0];
        }

        if (hp <= 0)
        {
            spownerController.currentBarrier--;
            Destroy(gameObject);
        }
    }
}
