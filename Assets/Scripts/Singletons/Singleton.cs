using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    static GameObject instance;

    static FieldNotebook field_notebook;

    public static T Get<T>()
    {
        if (!instance)
        {
            instance = GameObject.Find("/Singletons");
        }

        return instance.GetComponent<T>();
    }

    public static FieldNotebook GetFieldNotebook()
    {
        if (!field_notebook)
        {
            field_notebook = GameObject.Find("/Player").GetComponent<FieldNotebook>();
        }

        return field_notebook;
    }
}