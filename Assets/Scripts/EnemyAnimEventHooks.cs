using UnityEngine;

public class EnemyAnimEventHooks : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnAttackObject() {
		Transform parentTransform = transform.parent;
		 GameObject parentObject = parentTransform.gameObject;
		EnemyCharacter enemy = parentObject.GetComponent<EnemyCharacter>();
		enemy.SpawnAttackObject();
    }

    public void AttackFinished() {
	    Transform parentTransform = transform.parent;
	    GameObject parentObject = parentTransform.gameObject;
	    EnemyCharacter enemy = parentObject.GetComponent<EnemyCharacter>();

	    enemy.FinishedEngagement();
    }
}
