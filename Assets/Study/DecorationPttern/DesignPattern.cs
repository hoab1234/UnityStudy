using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignPattern : SkillDecorator
{
    public DesignPattern(Developer developer) => base.developer = developer;

    public override int Cost()
    {
        return developer.Cost() + 500;
    }

    public override string GetSkillDescription()
    {
        return developer.GetSkillDescription() + "Learned Design Pattern";
    }
}
