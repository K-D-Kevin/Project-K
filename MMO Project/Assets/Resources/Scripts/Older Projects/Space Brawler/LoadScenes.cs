using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour {

    private bool ShortcutsActive = false;
    private void Update()
    {
        if (ShortcutsActive)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                LeaveGame();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                LoadTitle();
            }
        }

    }

    public void SetShortCuts(bool Active = true)
    {
        ShortcutsActive = Active;
    }
    public void LoadMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadCalibration()
    {
        SceneManager.LoadScene("Ship Calibration");
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void LoadAcknowledgements() // Credits
    {
        SceneManager.LoadScene("Acknowledge");
    }

    public void LoadTest()
    {
        Debug.Log("Loading Test");
        SceneManager.LoadScene("TestGeneric");
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void LoadControls()
    {
        SceneManager.LoadScene("Controls");
    }
}
