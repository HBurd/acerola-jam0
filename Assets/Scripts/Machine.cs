using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    [SerializeField]
    GameObject dialog;
    public void OpenDialog()
    {
        dialog.SetActive(true);
    }
}
