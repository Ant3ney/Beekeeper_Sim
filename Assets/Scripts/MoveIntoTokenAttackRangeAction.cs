using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveIntoTokenAttackRange", story: "Agent Moves into token attack range", category: "Action", id: "cd9f48bd42e15b13b8277ff937f3c14a")]
public partial class MoveIntoTokenAttackRangeAction : Action
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

