using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = integerPosition(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 integerPosition(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x*10)/10, Mathf.Round(position.y*10)/10, Mathf.Round(position.z*10)/10);
    }
}
