using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiToggle : MonoBehaviour
{
    bool ui_enabled = false;

    [SerializeField]
    GameObject target;

    void Start()
    {
        target.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("UiToggle"))
        {
            ui_enabled = !ui_enabled;
            target.SetActive(ui_enabled);
        }
    }
}
