using UnityEngine;

public class PlayerAnimProto : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite regularSprite;
    public Sprite attackSprite;

    private SpriteRenderer spriteRenderer;
        	DumbData dumbData;

    void Start()
    {
	    dumbData = GameObject.Find("EasyData").GetComponent<DumbData>();
        // Get the SpriteRenderer component on this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

	regularSprite = dumbData.playerSprite;
	attackSprite = dumbData.playerAttackSprite;

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (spriteRenderer == null) return;

        // If left mouse button is held down
        if (Input.GetMouseButton(0))
        {
            spriteRenderer.sprite = attackSprite;
        }
        else
        {
            spriteRenderer.sprite = regularSprite;
        }
    }
}
