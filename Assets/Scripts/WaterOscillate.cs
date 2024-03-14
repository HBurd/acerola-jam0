using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOscillate : MonoBehaviour
{
    Vector3 initial_pos;

    [SerializeField]
    double period;

    [SerializeField]
    float magnitude;

    void Start()
    {
        initial_pos = transform.position;
    }

    void Update()
    {
        double t = (Time.timeAsDouble % period) * 2.0 * Mathf.PI / period;
        transform.position = initial_pos + Mathf.Sin((float)t) * magnitude * Vector3.up;
    }
}
