using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTree : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Transform>().rotation = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up) * GetComponent<Transform>().rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
