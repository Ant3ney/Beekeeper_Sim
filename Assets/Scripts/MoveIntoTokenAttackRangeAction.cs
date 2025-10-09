using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveIntoTokenAttackRange", story: "Agent Moves into token attack range", category: "Action", id: "cd9f48bd42e15b13b8277ff937f3c14a")]
public partial class MoveIntoTokenAttackRangeAction : Action
{
	public GameObject owner;
	EnemyCharacter enemyCharacter;
	protected override Status OnStart()
	{
		GameObject owner = this.GameObject;

		EnemyCharacter enemyCharacter = owner.GetComponent<EnemyCharacter>();
		return Status.Running;
	}

	protected override Status OnUpdate()
	{
		if(owner == null) {

			owner = this.GameObject;
				if(owner == null) {
					return Status.Success;
				}
		} 
		enemyCharacter = owner.GetComponent<EnemyCharacter>();
		enemyCharacter.MoveIntoTokenAttackRangeAction();
		return Status.Success;
	}

	protected override void OnEnd()
	{
	}
}

