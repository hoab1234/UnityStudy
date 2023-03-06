using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithms : SkillDecorator
{
    public Algorithms(Developer developer) => base.developer = developer;

    public override int Cost()
    {
        return developer.Cost() + 1000;
    }

    public override string GetSkillDescription()
    {
        return developer.GetSkillDescription() + "Algorithms update";
    }
}
