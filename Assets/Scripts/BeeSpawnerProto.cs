using UnityEngine;

public class BeeSpawnerProto : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject prefab;

    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
if (Input.GetMouseButton(0))
    {
        Vector3 mInput = Input.mousePosition;
        mInput.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mInput);
        mouseWorldPos.z = 0;

        if (prefab != null)
        {
            Instantiate(prefab, mouseWorldPos, Quaternion.identity);
        }
    }
    }
}
