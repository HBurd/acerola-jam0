using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    bool transparent = false;

    [SerializeField]
    float y_threshold = 0.0f;
    [SerializeField]
    float x_threshold = 4.0f;

    bool ShouldBeTransparent(Vector3 position_view)
    {
        return position_view.y <= y_threshold && Mathf.Abs(position_view.x) <= x_threshold;
    }

    float GetTransparency(Vector3 position_view)
    {
        float y_scale = Mathf.Clamp(-position_view.y, 0.0f, 10.0f) / 10.0f;
        float x_scale = Mathf.Abs(position_view.x) / x_threshold;
        return Mathf.Lerp(1.0f - y_scale, 1.0f, x_scale);
    }

    void Update()
    {
        Vector3 position_view = Camera.main.worldToCameraMatrix * new Vector4(transform.position.x, transform.position.y, transform.position.z, 1.0f);
        if (ShouldBeTransparent(position_view))
        {
            if (!transparent)
            {
                transparent = true;
                for (int i = 0; i < transform.childCount; ++i)
                {
                    transform.GetChild(i).GetComponent<MaterialContainer>()?.SetTransparent();
                }
            }
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).GetComponent<MaterialContainer>()?.SetTransparency(GetTransparency(position_view));
            }
        }
        else
        {
            if (transparent)
            {
                transparent = false;
                for (int i = 0; i < transform.childCount; ++i)
                {
                    transform.GetChild(i).GetComponent<MaterialContainer>()?.SetOpaque();
                }
            }
        }
    }
}
