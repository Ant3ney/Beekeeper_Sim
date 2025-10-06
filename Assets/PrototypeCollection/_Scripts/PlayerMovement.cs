using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed = 5f;
	DumbData dumbData;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	    dumbData = GameObject.Find("EasyData").GetComponent<DumbData>();
	    moveSpeed = dumbData.playerMoveSpeed;
	}

	// Update is called once per frame
	void Update()
	{
		float moveX = Input.GetAxis("Horizontal"); // A (-1)  → D (+1)
		float moveZ = Input.GetAxis("Vertical");   // S (-1)  → W (+1)

		Vector3 move = new Vector3(moveX, moveZ, 0);

		Vector3 CurrentPosition = transform.position;
		CurrentPosition += move * moveSpeed * Time.deltaTime;
		CurrentPosition.z = 0;
		transform.position = CurrentPosition;
	}
}
