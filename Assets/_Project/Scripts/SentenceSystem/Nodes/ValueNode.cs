using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueNode : Node, IValue
{
    public override void SetName()
    {
        base.SetName();
        gameObject.name += "_Value";
    }
}
