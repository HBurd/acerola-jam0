using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scuttle : MonoBehaviour
{
    CharacterController controller;

    Vector3 velocity;

    [SerializeField]
    float danger_radius;

    [SerializeField]
    OrganismType[] danger_organisms;

    [SerializeField]
    float flee_speed = 5.0f;

    [SerializeField]
    double flee_duration;

    [SerializeField]
    float jump_speed = 0.0f;

    double flee_end_time;

    bool flee = false;

    Vector3 flee_dir;

    [SerializeField]
    float average_movement_time;

    double next_movement_time;

    [SerializeField]
    float movement_speed;

    [SerializeField]
    float friction;

    [SerializeField]
    bool destroy_after_flee = false;

    [SerializeField]
    float subsequent_move_chance = 0.0f;

    [SerializeField]
    float subsequent_move_time = 0.0f;

    [SerializeField]
    float flee_wiggle;

    [SerializeField]
    float flee_wiggle_period;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    void Update()
    {
        if (!controller.isGrounded)
        {
            velocity += Physics.gravity * Time.deltaTime;
        }
        else if (!flee)
        {
            velocity.y = 0.0f;
            Vector3 friction = -this.friction * velocity.normalized * Time.deltaTime;
            if (friction.magnitude < velocity.magnitude)
            {
                velocity += friction;
            }
            else
            {
                velocity = Vector3.zero;
            }
        }

        if (flee)
        {
            float y_velocity = velocity.y;

            double t = 2.0 * Mathf.PI * (Time.timeAsDouble % flee_wiggle_period) / flee_wiggle_period;
            float sin_t = Mathf.Sin((float)t);

            Vector3 ortho = new Vector3(-flee_dir.z, flee_dir.y, flee_dir.x);

            velocity = flee_dir * flee_speed + ortho * flee_wiggle * flee_speed * sin_t + Vector3.up * y_velocity;


            if (Time.timeAsDouble > flee_end_time)
            {
                if (destroy_after_flee)
                {
                    velocity.y = -0.5f * flee_speed;
                    if (Time.timeAsDouble > flee_end_time + 1.0f)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    flee = false;
                    velocity = Vector3.zero;
                }
            }
        }
        else if (controller.isGrounded)
        {
            Collider[] danger = Physics.OverlapSphere(transform.position, danger_radius, LayerMask.GetMask("Organism"));

            foreach (Collider collider in danger)
            {
                Organism organism = collider.GetComponent<Organism>();
                foreach (OrganismType danger_type in danger_organisms)
                {
                    if (danger_type == organism.GetOrganismType())
                    {
                        Flee(organism.transform.position);
                        break;
                    }
                }
            }

            if (Time.timeAsDouble > next_movement_time && !flee)
            {
                if (Random.Range(0.0f, 1.0f) < subsequent_move_chance)
                {
                    next_movement_time = Time.timeAsDouble + subsequent_move_time;
                }
                else
                {
                    next_movement_time = Time.timeAsDouble + Random.Range(average_movement_time * 0.5f, average_movement_time * 1.5f);
                }

                float random_angle = Random.Range(0.0f, 2.0f * Mathf.PI);
                float cos_angle = Mathf.Cos(random_angle);
                float sin_angle = Mathf.Sin(random_angle);

                Vector3 movement_dir = Vector3.right * cos_angle + Vector3.forward * sin_angle;
                velocity += movement_speed * movement_dir + jump_speed * Vector3.up;
            }
        }

        if (!flee || Time.timeAsDouble < flee_end_time)
        {
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            // We want to clip into ground after fleeing
            transform.position += velocity * Time.deltaTime;
        }
    }

    void Flee(Vector3 danger_pos)
    {
        flee_dir = transform.position - danger_pos;
        flee_dir.Normalize();
        flee = true;

        flee_end_time = Time.timeAsDouble + flee_duration;
    }
}
