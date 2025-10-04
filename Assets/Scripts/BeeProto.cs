using UnityEngine;

public class BeeProto : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float Timer = 2;
    float current = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       current += 1 * Time.deltaTime;

       if(current > Timer)
       {
		Destroy(gameObject);
       }


    }
}
