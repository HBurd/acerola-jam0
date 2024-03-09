using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCull : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.bounds = new Bounds(mesh.bounds.center, 10000.0f * Vector3.one);
    }
}
