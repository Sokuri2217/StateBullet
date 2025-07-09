using UnityEngine;

public class UIStage : MonoBehaviour
{
    [Header("ˆêŽž’âŽ~")]
    public GameObject pausePanel;
    public GameObject playUI;

    public bool isPause;
    private bool isInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PauseGame();
    }

    //ˆêŽž’âŽ~
    public void PauseGame()
    {
        if(!isInput)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) 
            {
                switch(isPause)
                {
                    case true:
                        if(pausePanel.activeSelf)
                        {
                            Time.timeScale = 1;
                            isPause = false;
                            pausePanel.SetActive(false);
                            playUI.SetActive(true);
                        }
                        break;
                    case false:
                        isPause = true;
                        playUI.SetActive(false);
                        pausePanel.SetActive(true);
                        Time.timeScale = 0;
                        break;
                }
                isInput = true;
            }
        }
        else
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                isInput = false;
            }
        }
    }
}
