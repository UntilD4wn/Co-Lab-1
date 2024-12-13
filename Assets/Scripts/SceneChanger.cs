using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Level1");
        Debug.Log("Change Scene Operational");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit Operational");

    }

    public void Restart()
    {
        SceneManager.LoadScene("Level2");
        Debug.Log("Restart Operational");

    }


    public void Menu()
    {
        SceneManager.LoadScene("StartScreen");
        Debug.Log("Menu Button Operational");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Level2");
    }


}
