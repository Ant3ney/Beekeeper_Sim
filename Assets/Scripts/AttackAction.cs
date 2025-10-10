using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "Attacks player", category: "Action", id: "eb2218f883b0767703d7f01275971fb1")]
public partial class AttackAction : Action
{
	EnemyCharacter enemyCharacter;
	public GameObject owner;
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
		enemyCharacter.Attacking();
		return Status.Success;
	}

	protected override void OnEnd()
	{
	}
}

