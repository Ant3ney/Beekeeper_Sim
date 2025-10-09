using System;
using Unity.Behavior;

[BlackboardEnum]
public enum MainEnemyState
{
    Attacking,
	MovingIntoTokenRange,
	WaitingForToken,
	ActionStateInitial
}
