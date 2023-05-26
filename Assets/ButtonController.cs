using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private string MainGame = "Level1";
    [SerializeField] private string MainMenu = "Main Menu";

    AudioManager audioManagerScript;
    GameObject amgo;
   

    private void Start()
    {
        amgo = GameObject.Find("AudioManager");

        //create a reference to the AudioManager
        //audioManagerScript = amgo.GetComponent<AudioManager>();

        
    }

    void Update()
    {
        string diff = PlayerPrefs.GetString("difficulty");
        print("difficulty is " + diff);



    }

    public void LoadLevelOne()
    {
        print("Load LEVEL ONE");
        SceneManager.LoadScene(MainGame);
    }
    public void LoadMainMenu()
    {
        print("Load Main Menu");
        SceneManager.LoadScene(MainMenu);
    }

    public void Quit()
    {
       Application.Quit();
    }


    public void SetMusicVolume( float vol )
    {
        print("music vol=" + vol);
        audioManagerScript.SetMusicVolume(vol);
    }

    public void SetSFXVolume(float vol)
    {
        print("sfx vol=" + vol);
        audioManagerScript.SetSFXVolume(vol);
    }

}
