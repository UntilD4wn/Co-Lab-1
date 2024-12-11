using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFunc : MonoBehaviour
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
        SceneManager.LoadScene("Level2");
        Debug.Log("Change Scene Operational");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit Operational");

    }



}
