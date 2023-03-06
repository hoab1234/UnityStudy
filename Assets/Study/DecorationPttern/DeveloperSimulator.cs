using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperSimulator : MonoBehaviour
{
    void Start()
    {
        Developer dev = new JuniorDeveloper();
        Debug.Log(dev.GetSkillDescription());
        Debug.Log(dev.Cost());

        dev = new Algorithms(dev);
        Debug.Log(dev.GetSkillDescription());
        Debug.Log(dev.Cost());

        Developer dev2 = new SeniorDeveloper();
        Debug.Log(dev2.GetSkillDescription());
        Debug.Log(dev2.Cost());

        dev2 = new Algorithms(dev2);
        dev2 = new DesignPattern(dev2);
        Debug.Log(dev2.GetSkillDescription());
        Debug.Log(dev2.Cost());
    }

}
