using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeniorDeveloper : Developer
{
    public SeniorDeveloper() => this.skillDescrition = "Senior Developer";

    public override int Cost()
    {
        return 500;
    }

    public override string GetSkillDescription()
    {
        return this.skillDescrition;
    }
}
