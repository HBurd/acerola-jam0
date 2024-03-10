using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCull : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        // This is definitely wrong (up is a direction) but it works??
        Vector3 up = transform.InverseTransformPoint(Vector3.up);
        mesh.bounds = new Bounds(mesh.bounds.center, mesh.bounds.size + up * 15.0f);
        Destroy(this);
    }
}
