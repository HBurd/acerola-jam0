using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flutter : MonoBehaviour
{
    Vector3 velocity;

    [SerializeField]
    float min_height;

    [SerializeField]
    float flutter_speed;

    [SerializeField]
    float gravity_factor;

    [SerializeField]
    float max_height;

    [SerializeField]
    float drift_factor;

    //[SerializeField]
    //float avg_flutter_period;

    //float base_flutter_chance;

    void Start()
    {
        // assuming 60fps (probably incorrectly)
        //base_flutter_chance = 1 - Mathf.Pow(1 - 1.0f / avg_flutter_period, 1.0f / 60.0f);
    }

    void Update()
    {
        velocity += Physics.gravity * gravity_factor * Time.deltaTime;

        float flutter_chance = 1.0f - Mathf.Pow(1.0f - Mathf.Clamp((max_height - transform.position.y) / (max_height - min_height), 0.0f, 1.0f), Time.deltaTime);

        if (Random.Range(0.0f, 1.0f) < flutter_chance)
        {
            velocity.y = flutter_speed * Random.Range(0.5f, 1.5f);
        }

        velocity.x = drift_factor * 2.0f * (Mathf.PerlinNoise(transform.position.x * 0.1f + (float)(0.01 * Time.timeAsDouble), transform.position.z * 0.1f) - 0.5f);
        velocity.z = drift_factor * 2.0f * (Mathf.PerlinNoise(transform.position.z * 0.1f, transform.position.x * 0.1f + (float)(0.01 * Time.timeAsDouble)) - 0.5f);

        transform.position += velocity * Time.deltaTime;
    }
}
