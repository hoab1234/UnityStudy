using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Developer
{
    protected string skillDescrition = string.Empty;


    public abstract int Cost();
    public abstract string GetSkillDescription();
}

public abstract class SkillDecorator : Developer
{
    public Developer developer;
}
