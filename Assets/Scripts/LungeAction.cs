using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Lunge", story: "Move close to attack target to attack it. Not just close enough to wait for token. Close enough to mele or start range attack. For range attackers, this might be the same or larger than the token distance.", category: "Action", id: "c6f034f1442490b7dc52e4bf08965e62")]
public partial class LungeAction : Action
{

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

