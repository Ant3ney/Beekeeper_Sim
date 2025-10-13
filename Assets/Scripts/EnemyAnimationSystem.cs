using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationSystem : MonoBehaviour
{
    [Header("Movement Detection")]
    [Tooltip("How far the enemy must move before considered 'moving'.")]
    public float movementThreshold = 0.05f;

    [Header("References")]
    [Tooltip("If left empty, the script will automatically find the SpriteRenderer on this GameObject.")]
    public SpriteRenderer spriteRenderer;

    private Animator animator;
    private Vector3 lastPosition;
    private Vector3 velocity;

    public bool IsMoving { get; private set; }
    public float MoveDirectionX { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();

        // Auto-find SpriteRenderer if not assigned
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        lastPosition = transform.position;
    }

    void Update()
    {
        // ✅ Compute velocity manually since NavMeshAgents don't use Rigidbody2D
        Vector3 currentPosition = transform.position;
        velocity = (currentPosition - lastPosition) / Time.deltaTime;
        lastPosition = currentPosition;

        // ✅ Check if enemy is moving
        IsMoving = velocity.sqrMagnitude > (movementThreshold * movementThreshold);

        // ✅ Update Animator parameter
        animator.SetBool("IsMoving", IsMoving);

        // ✅ Get movement direction along X
        MoveDirectionX = velocity.x;

        // ✅ Flip sprite based on direction
        if (spriteRenderer != null && Mathf.Abs(MoveDirectionX) > movementThreshold)
        {
            spriteRenderer.flipX = MoveDirectionX < 0; // Faces left if moving left
        }

        // Debugging (optional)
        // Debug.Log($"{name} | IsMoving: {IsMoving} | Velocity: {velocity}");
    }
}
