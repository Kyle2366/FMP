using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultKey : MonoBehaviour
{
    [SerializeField] Button KeyBoardDefaultSelection;
    // Start is called before the first frame update
    void Start()
    {
        DefaultSelect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DefaultSelect()
    {
        KeyBoardDefaultSelection.Select();
    }
}
