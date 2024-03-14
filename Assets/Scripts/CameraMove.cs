using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    Transform target;

    [SerializeField]
    float z_offset;

    float y_offset;

    void Start()
    {
        y_offset = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        transform.position = target.position + new Vector3(0.0f, y_offset, z_offset);
    }
}
