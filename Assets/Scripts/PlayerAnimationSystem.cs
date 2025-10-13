using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationSystem : MonoBehaviour
{
    [Header("Movement Detection")]
    [Tooltip("How fast the player must move before considered 'moving'.")]
    public float movementThreshold = 0.05f;

    [Header("References")]
    [Tooltip("If left empty, the script will automatically find the parent Rigidbody2D.")]
    public Rigidbody2D parentRigidbody;

    [Tooltip("If left empty, the script will automatically find the SpriteRenderer on this GameObject.")]
    public SpriteRenderer spriteRenderer;

    private Animator animator;

    public bool IsMoving { get; private set; }
    public float MoveDirectionX { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();

        // Auto-find Rigidbody2D in parent if not assigned
        if (parentRigidbody == null)
        {
            parentRigidbody = GetComponentInParent<Rigidbody2D>();
        }

        // Auto-find SpriteRenderer on this object if not assigned
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (parentRigidbody == null)
        {
            Debug.LogWarning($"{name} (PlayerAnimationSystem) did not find a parent Rigidbody2D!");
        }
    }

    void Update()
    {


        if (parentRigidbody == null) {
            return;
	    Debug.Log("No parent");
	} else {

	    //Debug.Log("Yes parent");
	}

		    if(parentRigidbody.gameObject.name == "Player") return;
	Debug.Log("Update: " + parentRigidbody.gameObject.name);


        // ✅ Determine if player is moving
        Vector2 velocity = parentRigidbody.linearVelocity;

        IsMoving = velocity.sqrMagnitude > (movementThreshold * movementThreshold);
	Debug.Log("velocity.sqrMagnitude" + velocity.sqrMagnitude);

	if(IsMoving) {
		Debug.Log("Moving: " + parentRigidbody.gameObject.name);
	} else {

		Debug.Log("not Moving: " + parentRigidbody.gameObject.name);
	}

        // ✅ Update Animator parameter
        animator.SetBool("IsMoving", IsMoving);

        // ✅ Get movement direction along X
        MoveDirectionX = velocity.x;

        // ✅ Flip sprite based on direction
        if (spriteRenderer != null && Mathf.Abs(MoveDirectionX) > movementThreshold)
        {
            spriteRenderer.flipX = MoveDirectionX >= 0; // Faces left if moving left
        }
    }

    public bool IsEnemy()
    {
	    if (transform.parent != null)
	    {
		    GameObject parentObj = transform.parent.gameObject;

		    if (parentObj.CompareTag("Enemy"))
		    {
			    return true;
		    }
		    else
		    {
			    return false;
		    }
	    }
	    else
	    {
		    return false;
	    }
    }
}
