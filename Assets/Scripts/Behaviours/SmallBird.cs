using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBird : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    double jump_interval = 0.1;

    double next_event_time = 0.0f;

    [SerializeField]
    double jump_chance = 0.2f;

    [SerializeField]
    double jump_cooldown = 0.5;

    bool fly = false;

    Vector3 fly_direction;

    BirdFlock flock;

    [SerializeField]
    float fly_time = 0.5f;

    [SerializeField]
    GameObject feather;

    [SerializeField]
    float feather_drop_chance;

    [SerializeField]
    GameObject poop;

    [SerializeField]
    float poop_drop_chance;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Time.timeAsDouble > next_event_time)
        {
            double event_delta = jump_interval;
            if (!fly)
            {
                bool should_jump = Random.Range(0.0f, 1.0f) < jump_chance;
                
                if (should_jump)
                {
                    Vector3 jump_dir;
                    if ((transform.position - flock.GetCenter()).magnitude > 1.3f * flock.GetSpawnRadius())
                    {
                        // too far out, just fly back
                        Fly();
                        jump_dir = Vector3.zero;
                    }
                    else if ((transform.position - flock.GetCenter()).magnitude > flock.GetSpawnRadius())
                    {
                        // Outside of flock radius so jump towards it
                        Vector3 pos = transform.position;
                        pos.y = 0.0f;
                        jump_dir = flock.GetCenter() - pos;
                        jump_dir.Normalize();

                        // jump faster
                        event_delta = 0.2f * jump_interval;
                    }
                    else
                    {
                        // We're in the flock raidus so just jump randomly
                        float random_angle = Random.Range(0.0f, 2.0f * Mathf.PI);
                        float cos_angle = Mathf.Cos(random_angle);
                        float sin_angle = Mathf.Sin(random_angle);

                        jump_dir = Vector3.right * cos_angle + Vector3.forward * sin_angle;

                        // Wait before next jump
                        event_delta = jump_cooldown;
                    }
                    

                    rb.AddForce(Vector3.up + jump_dir, ForceMode.Impulse);
                }
            }
            else
            {
                fly = false;
            }

            next_event_time = Time.timeAsDouble + event_delta;
        }

        if (!fly)
        {
            List<GameObject> hazards = flock.GetHazards();
            foreach (GameObject hazard in hazards)
            {
                if ((hazard.transform.position - transform.position).magnitude < flock.GetHazardThreshold())
                {
                    Fly();
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (fly)
        {
            // Add upward force
            rb.AddForce(10.0f * fly_direction * (5.0f - rb.velocity.y));

            // Add spinny force
            double t = 2.0 * Mathf.PI * (Time.timeAsDouble % 0.7) / 0.7;
            float sin_t = Mathf.Sin((float)t);
            float cos_t = Mathf.Cos((float)t);

            rb.AddForce(10.0f * (Vector3.right * cos_t + Vector3.forward * sin_t));
        }
    }

    public void Fly()
    {
        fly = true;

        //float random_angle = Random.Range(0.0f, 2.0f * Mathf.PI);
        //float cos_angle = Mathf.Cos(random_angle);
        //float sin_angle = Mathf.Sin(random_angle);

        Vector3 direction = flock.GetCenter() - transform.position; // Vector3.right * cos_angle + Vector3.forward * sin_angle;
        direction.y = 0.0f;
        direction.Normalize();

        fly_direction = 2.0f * Vector3.up + direction;
        fly_direction.Normalize();

        // Wake up object if sleeping (not sure if necessary?)
        rb.AddForce(Vector3.up * 0.1f, ForceMode.Impulse);

        float drop_test = Random.Range(0.0f, 1.0f);
        if (drop_test < feather_drop_chance)
        {
            Instantiate(feather, transform.position, Quaternion.identity);
        }
        else if (drop_test < poop_drop_chance)
        {
            Instantiate(poop, transform.position, Quaternion.identity);
        }

        next_event_time = Time.timeAsDouble + fly_time;
    }

    public void SetFlock(BirdFlock flock)
    {
        this.flock = flock;
    }
}