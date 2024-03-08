using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prowl : MonoBehaviour
{
    [SerializeField]
    float detect_radius;    // search for targets in this radius

    [SerializeField]
    float approach_radius;  // approach target up to this distance

    [SerializeField]
    float escape_radius;    // Lose target lock

    [SerializeField]
    float attack_radius;    // attack when target enters this radius

    [SerializeField]
    float jump_radius;      // jump at target in this radius

    [SerializeField]
    float capture_radius;   // capture target in this radius when attacking

    [SerializeField]
    float danger_radius;    // distance to stay away from danger

    [SerializeField]
    OrganismType[] prey;    // in order of preference

    [SerializeField]
    OrganismType[] danger;

    [SerializeField]
    float approach_speed;

    [SerializeField]
    float approach_angle;

    [SerializeField]
    float attack_speed;

    [SerializeField]
    float jump_speed_vertical;

    [SerializeField]
    float friction;

    [SerializeField]
    double eating_duration;

    [SerializeField]
    double reevaluate_timeout;

    [SerializeField]
    float flee_speed;

    [SerializeField]
    float flee_duration;

    [SerializeField]
    double wander_move_duration;

    double flee_stop_time;
    double wander_move_stop_time;
    double next_wander_time;

    Vector3 wander_dir;

    double eating_time;
    double reevaluate_time;

    Vector3 velocity;

    Quaternion approach_rotation;

    Transform target;

    GameObject caught_prey;

    CharacterController controller;

    bool attacking = false;

    Vector3 flee_dir = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detect_radius, LayerMask.GetMask("Organism"));

        int best_target_type = 1000;

        int num_threats = 0;
        Vector3 avg_threat_pos = Vector3.zero;

        foreach (Collider c in colliders)
        {
            Organism organism = c.GetComponent<Organism>();
            if (!organism)
            {
                continue;
            }

            // Evaluate threats in area
            foreach (OrganismType threat in danger)
            {
                if (threat == organism.GetOrganismType() && (c.transform.position - transform.position).magnitude < danger_radius)
                {
                    avg_threat_pos += c.transform.position;
                    ++num_threats;
                    break;
                }
            }

            // Only look for a new target if we don't already have one
            if (!target)
            {
                for (int i = 0; i < prey.Length; ++i)
                {
                    if (prey[i] == organism.GetOrganismType())
                    {
                        if (i < best_target_type)
                        {
                            best_target_type = i;
                            target = c.transform;
                            reevaluate_time = Time.timeAsDouble + reevaluate_timeout;

                            float approach_angle = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * this.approach_angle;
                            approach_rotation = Quaternion.AngleAxis(approach_angle, Vector3.up);
                        }
                        break;
                    }
                }
            }
        }


        if (num_threats > 0 && !IsFleeing())
        {
            avg_threat_pos /= num_threats;
            Flee(avg_threat_pos);
        }
    }

    void Update()
    {
        if (controller.isGrounded)
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
        else
        {
            velocity += Physics.gravity * Time.deltaTime;
        }

        if (!target)
        {
            attacking = false;
        }

        FindTarget();

        if (Time.timeAsDouble < flee_stop_time)
        {
            velocity = flee_dir * flee_speed;
        }
        else if (caught_prey)
        {
            if (Time.timeAsDouble > eating_time)
            {
                DropPrey();
            }
        }
        else if (target)
        {
            Vector3 delta = (target.position - transform.position);
            delta.y = 0.0f;
            float distance = delta.magnitude;
            Vector3 direction = delta.normalized;

            if (attacking)
            {
                float y_velocity = velocity.y;
                if (distance < jump_radius && controller.isGrounded)
                {
                    y_velocity = jump_speed_vertical;
                }

                velocity = attack_speed * direction + Vector3.up * y_velocity;

                if (distance < capture_radius)
                {
                    attacking = false;
                    caught_prey = target.GetComponent<Organism>().Kill();
                    caught_prey.transform.SetParent(transform);
                    eating_time = Time.timeAsDouble + eating_duration;
                    target = null;
                }
            }
            else if (distance > escape_radius || Time.timeAsDouble > reevaluate_time)
            {
                // lose lock
                target = null;
                velocity = Vector3.zero;
            }
            else if (distance < attack_radius)
            {
                attacking = true;
            }
            else if (controller.isGrounded && distance > approach_radius)
            {
                // approach target
                velocity = approach_speed * (approach_rotation * direction);
            }
        }
        else if (controller.isGrounded)
        {
            // Wander around randomly
            if (Time.timeAsDouble > next_wander_time)
            {
                wander_move_stop_time = Time.timeAsDouble + wander_move_duration;
                next_wander_time = Time.timeAsDouble + wander_move_duration + Random.Range(0.5f, 3.0f);

                float random_angle = Random.Range(0.0f, 2.0f * Mathf.PI);
                float cos_angle = Mathf.Cos(random_angle);
                float sin_angle = Mathf.Sin(random_angle);
                wander_dir = Vector3.right * cos_angle + Vector3.forward * sin_angle;
            }

            if (Time.timeAsDouble < wander_move_stop_time)
            {
                velocity = wander_dir * approach_speed;
            }
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void Flee(Vector3 threat_pos)
    {
        if (attacking)
        {
            return;
        }
        flee_dir = transform.position - threat_pos;
        flee_dir.y = 0.0f;
        flee_dir.Normalize();
        flee_stop_time = Time.timeAsDouble + flee_duration;
        target = null;
        DropPrey();
    }

    bool IsFleeing()
    {
        return Time.timeAsDouble < flee_stop_time;
    }

    void DropPrey()
    {
        if (!caught_prey)
        {
            return;
        }
        caught_prey.transform.SetParent(null);
        caught_prey = null;
    }
}
