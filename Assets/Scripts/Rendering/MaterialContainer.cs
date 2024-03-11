using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialContainer : MonoBehaviour
{
    [SerializeField]
    Material transparent;

    [SerializeField]
    Material opaque;

    public void SetTransparent()
    {
        Renderer renderer = GetComponent<Renderer>();
        Destroy(renderer.material);
        renderer.material = transparent;
    }

    public void SetOpaque()
    {
        Renderer renderer = GetComponent<Renderer>();
        Destroy(renderer.material);
        renderer.material = opaque;
    }

    public void SetTransparency(float transparency)
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetFloat("_transparency", transparency);
    }
}
