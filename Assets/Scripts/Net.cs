using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    Quaternion initial_rotation;
    bool swinging = false;
    bool tried_catch = false;

    [SerializeField]
    double swing_duration;

    [SerializeField]
    double swing_recovery_time;

    [SerializeField]
    Transform target;

    [SerializeField]
    float catch_radius;

    double swing_start_time;

    void Start()
    {
        initial_rotation = transform.rotation;
    }

    void Update()
    {
        if (!swinging && Input.GetButtonDown("UseNet"))
        {
            swinging = true;
            swing_start_time = Time.timeAsDouble;
        }
        
        if (swinging && Time.timeAsDouble > swing_start_time + swing_duration + swing_recovery_time)
        {
            swinging = false;
        }

        if (swinging)
        {
            float swing_amount = 0.0f;
            if (Time.timeAsDouble < swing_start_time + swing_duration)
            {
                if (!tried_catch)
                {
                    // swinging down
                    swing_amount = (float)(Time.timeAsDouble - swing_start_time) / (float)swing_duration;
                }
                else
                {
                    // holding net on ground after catching
                    swing_amount = 1.0f;
                }
            }
            else
            {
                // swinging up
                swing_amount = 1.0f - (float)(Time.timeAsDouble - swing_start_time - swing_duration) / (float)(swing_recovery_time);

                if (!tried_catch)
                {
                    tried_catch = true;
                    if (TryCatch())
                    {
                        swing_start_time += 1.0f;
                    }
                }
            }

            transform.localRotation = Quaternion.AngleAxis(swing_amount * 90.0f, Vector3.right) * initial_rotation;
        }
        else
        {
            tried_catch = false;
        }
    }

    bool TryCatch()
    {
        Collider[] caught_organisms = Physics.OverlapSphere(target.position, catch_radius, LayerMask.GetMask("Organism"));
        foreach (Collider collider in caught_organisms)
        {
            Organism organism = collider.GetComponent<Organism>();
            if (organism?.GetOrganismType() == OrganismType.Alien)
            {
                Destroy(collider.gameObject);
                return true;
            }
        }

        return false;
    }

    public float SpeedModifier()
    {
        if (Time.timeAsDouble < swing_start_time + swing_duration && tried_catch)
        {
            return 0.0f;
        }
        else if (swinging)
        {
            return 0.5f;
        }
        else
        {
            return 1.0f;
        }
    }
}
