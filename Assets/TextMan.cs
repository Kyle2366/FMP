using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMan : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI difficultyText;
    public GameObject text;
    
  
    void Update()
    {
        Load();
    }
    public void Load()
    {
      difficultyText.text = PlayerPrefs.GetString("difficulty");
    }
   
}
