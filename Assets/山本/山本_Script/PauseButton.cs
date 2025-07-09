using UnityEngine;

public class PauseButton : ButtonScale
{
    public GameObject panel;
    public GameObject backPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
        backPanel.SetActive(false);
    }

    public void ClosePanel()
    {
        backPanel.SetActive(true);
        panel.SetActive(false);
    }
}
