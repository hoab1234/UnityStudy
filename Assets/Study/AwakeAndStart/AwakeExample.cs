using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeExample : MonoBehaviour
{
    public GameObject startObject;
    [HideInInspector] public int number;
    
    

    private void Awake()
    {
        number = 1;
        Debug.Log("Awake Example scripts awake is called");
    }

    private void Start()
    {
        Debug.Log("start function is not called when script component is not enable");
        startObject.GetComponent<StartExample>().enabled = true;
    }
}
