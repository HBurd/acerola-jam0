using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController controller;
    FieldNotebook notebook;

    [SerializeField]
    float speed;

    [SerializeField]
    float pickup_radius;

    [SerializeField]
    float ground_offset;

    [SerializeField]
    Net net;

    [SerializeField]
    double spawn_interval;

    double next_spawn_time;

    [SerializeField]
    float spawn_radius;

    [SerializeField]
    int carrying_capacity;

    [SerializeField]
    float alien_chance = 0.01f;

    [SerializeField]
    GameObject alien;

    [SerializeField]
    GameObject bird_flock_spawner;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        notebook = GetComponent<FieldNotebook>();
    }

    void Update()
    {
        Vector3 movement = Input.GetAxisRaw("Horizontal") * Vector3.right + Input.GetAxisRaw("Vertical") * Vector3.forward;
        movement.Normalize();

        if (movement.magnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }

        Vector3 delta = net.SpeedModifier() * speed * movement * Time.deltaTime;

        controller.Move(delta);

        // snap to ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Environment")))
        {
            transform.position += (hit.distance - ground_offset) * Vector3.down;
        }


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit_info;
            if (Physics.Raycast(ray, out hit_info, Mathf.Infinity, LayerMask.GetMask("Organism", "Machine")))
            {
                Organism organism = hit_info.transform.GetComponent<Organism>();
                Machine machine = hit_info.transform.GetComponent<Machine>();
                if (organism != null && organism.gameObject != gameObject)
                {
                    notebook.Discover(organism.GetOrganismType());
                }
                else if (machine != null)
                {
                    machine.OpenDialog();
                }
            }
        }

        Collider[] pickups = Physics.OverlapSphere(transform.position, pickup_radius, LayerMask.GetMask("Pickup"));

        foreach (Collider collider in pickups)
        {
            Item pickup = collider.GetComponent<Item>();
            if (pickup)
            {
                notebook.StoreSample(pickup.GetSourceOrganism(), pickup.GetItemType());
                Destroy(pickup.gameObject);
            }
        }



        if (Time.timeAsDouble > next_spawn_time)
        {
            next_spawn_time = Time.timeAsDouble + spawn_interval;

            int nearby = 0;

            Collider[] nearby_organisms = Physics.OverlapSphere(transform.position, 2.0f * spawn_radius, LayerMask.GetMask("Organism"));
            foreach (Collider collider in nearby_organisms)
            {
                if (collider.GetComponent<Organism>())
                {
                    nearby += 1;
                }
            }

            if (nearby <= carrying_capacity)
            {
                SpawnOrganisms(carrying_capacity - nearby);
            }
        }
    }


    void SpawnOrganisms(int count)
    {
        int birds_to_spawn = (int)(count * Mathf.Clamp(Random.Range(1.0f - alien_chance - 0.1f, 1.0f - alien_chance + 0.1f), 0.0f, 1.0f));
        int aliens_to_spawn = count - birds_to_spawn;

        Debug.Log(count + ", " + birds_to_spawn + ", " + aliens_to_spawn);

        int flocks_to_spawn = birds_to_spawn > 0 ? Random.Range(1, 3) : 0;

        for (int i = 0; i < aliens_to_spawn; ++i)
        {
            float random_angle = Random.Range(0.0f, 2.0f * Mathf.PI);
            Vector3 random_dir = Vector3.right * Mathf.Cos(random_angle) + Vector3.forward * Mathf.Sin(random_angle);
            Vector3 spawn_pos = transform.position + random_dir * Random.Range(spawn_radius, 2.0f * spawn_radius);
            if (!Physics.Raycast(spawn_pos, Vector3.down, transform.position.y * 0.9f, LayerMask.GetMask("Environment")))
            {
                continue;
            }
            Instantiate(alien, spawn_pos, Quaternion.identity);
        }

        for (int i = 0; i < flocks_to_spawn - 1; ++i)
        {
            int birds_in_flock = (int)(birds_to_spawn * Random.Range(0.25f * birds_to_spawn, 0.75f * birds_to_spawn));
            birds_in_flock = Mathf.Min(birds_in_flock, birds_to_spawn);
            birds_to_spawn -= birds_in_flock;


            float random_angle = Random.Range(0.0f, 2.0f * Mathf.PI);
            Vector3 random_dir = Vector3.right * Mathf.Cos(random_angle) + Vector3.forward * Mathf.Sin(random_angle);
            Vector3 spawn_pos = transform.position + random_dir * Random.Range(spawn_radius, 2.0f * spawn_radius);
            if (!Physics.Raycast(spawn_pos, Vector3.down, transform.position.y * 0.9f, LayerMask.GetMask("Environment")))
            {
                continue;
            }
            BirdFlock flock = Instantiate(bird_flock_spawner, spawn_pos, Quaternion.identity).GetComponent<BirdFlock>();
            flock.SetBirdCount(birds_in_flock);
            flock.Spawn();
        }

        {
            float random_angle = Random.Range(0.0f, 2.0f * Mathf.PI);
            Vector3 random_dir = Vector3.right * Mathf.Cos(random_angle) + Vector3.forward * Mathf.Sin(random_angle);
            Vector3 spawn_pos = transform.position + random_dir * Random.Range(spawn_radius, 2.0f * spawn_radius);
            if (!Physics.Raycast(spawn_pos, Vector3.down, transform.position.y * 0.9f, LayerMask.GetMask("Environment")))
            {
                return;
            }
            BirdFlock flock = Instantiate(bird_flock_spawner, spawn_pos, Quaternion.identity).GetComponent<BirdFlock>();
            flock.SetBirdCount(birds_to_spawn);
            flock.Spawn();
        }
    }
}
