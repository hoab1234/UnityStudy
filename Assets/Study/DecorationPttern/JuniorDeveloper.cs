using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuniorDeveloper : Developer
{
    public JuniorDeveloper() => this.skillDescrition = "Junior Developer";

    public override int Cost()
    {
        return 100;
    }

    public override string GetSkillDescription()
    {
        return this.skillDescrition;
    }
}
