using UnityEngine;

public class ActiveExplanation : MonoBehaviour
{
    public GameObject explanation;
    public ButtonScale buttonScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        explanation.SetActive(false);
        buttonScale=GetComponent<ButtonScale>();
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonScale.isActive)
        {
            explanation.SetActive(true);
        }
        else
        {
            explanation.SetActive(false);
        }
    }
}
