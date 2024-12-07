using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptorNode : Node, IDescriptor
{
    public override void SetName()
    {
        base.SetName();
        gameObject.name += "_Descriptor";
    }
}
