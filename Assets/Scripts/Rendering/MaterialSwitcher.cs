using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    bool transparent = false;

    [SerializeField]
    float y_threshold = -20.0f;
    [SerializeField]
    float x_threshold = -3.0f;

    bool ShouldBeTransparent(Vector3 position_view)
    {
        return position_view.y <= y_threshold && Mathf.Abs(position_view.x) <= x_threshold;
    }

    void Update()
    {
        Vector3 position_view = Camera.main.worldToCameraMatrix * new Vector4(transform.position.x, transform.position.y, transform.position.z, 1.0f);
        if (ShouldBeTransparent(position_view) && !transparent)
        {
            transparent = true;
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).GetComponent<MaterialContainer>()?.SetTransparent();
            }
        }
        else if (!ShouldBeTransparent(position_view) && transparent)
        {
            transparent = false;
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).GetComponent<MaterialContainer>()?.SetOpaque();
            }
        }

        /*
        Renderer renderer = GetComponent<Renderer>();
        
        if (!ShouldBeTransparent(position_view))
        {
            Destroy(renderer.material);
            renderer.material = opaque_material;
            active_material = opaque_material;
        }
        else if (ShouldBeTransparent(position_view))
        {
            Destroy(renderer.material);
            GetComponent<Renderer>().material = transparent_material;
            active_material = transparent_material;
        }*/
    }
}
