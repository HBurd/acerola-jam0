using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFlock : MonoBehaviour
{
    [SerializeField]
    GameObject bird;

    [SerializeField]
    int flock_size = 5;

    [SerializeField]
    float spawn_radius = 5.0f;

    [SerializeField]
    float hazard_threshold;

    SmallBird[] birds;

    Vector3 center;

    List<GameObject> nearby_hazards = new List<GameObject>();

    [SerializeField]
    List<OrganismType> danger_types;

    [SerializeField]
    bool autospawn = false;

    public void SetBirdCount(int size)
    {
        flock_size = size;
    }

    void Start()
    {
        if (autospawn)
        {
            Spawn();
        }

    }

    public void Spawn()
    {
        birds = new SmallBird[flock_size];
        for (int i = 0; i < flock_size; ++i)
        {
            float delta_x = Random.Range(-spawn_radius, spawn_radius);
            float delta_z = Random.Range(-spawn_radius, spawn_radius);
            Vector3 pos = transform.position + delta_x * Vector3.right + delta_z * Vector3.forward;
            birds[i] = Instantiate(bird, pos, Quaternion.identity).GetComponent<SmallBird>();
            birds[i].SetFlock(this);
        }
    }

    public Vector3 GetCenter()
    {
        return center;
    }

    public float GetSpawnRadius()
    {
        return spawn_radius;
    }

    public List<GameObject> GetHazards()
    {
        return nearby_hazards;
    }

    void Update()
    {
        
    }


    void LateUpdate()
    {
        center = Vector3.zero;
        float min_x = Mathf.Infinity;
        float max_x = -Mathf.Infinity;
        float min_z = Mathf.Infinity;
        float max_z = -Mathf.Infinity;

        for (int i = 0; i < birds.Length; ++i)
        {
            min_x = Mathf.Min(min_x, birds[i].transform.position.x);
            max_x = Mathf.Max(max_x, birds[i].transform.position.x);
            min_z = Mathf.Min(min_z, birds[i].transform.position.z);
            max_z = Mathf.Max(max_z, birds[i].transform.position.z);

            center += birds[i].transform.position;
        }
        //center.x = 0.5f * (max_x + min_x);
        //center.z = 0.5f * (max_z + min_z);
        center /= birds.Length;
        float radius = 0.5f * Mathf.Max(max_x - min_x, max_z - min_z);

        // Find nearby hazards
        Collider[] hazards = Physics.OverlapSphere(center, radius + hazard_threshold, LayerMask.GetMask("Organism"));
        nearby_hazards = new List<GameObject>();
        foreach (Collider hazard in hazards)
        {
            foreach (OrganismType danger_type in danger_types)
            {
                if (hazard.transform.GetComponent<Organism>().GetOrganismType() == danger_type)
                {
                    nearby_hazards.Add(hazard.transform.gameObject);
                }
            }
        }
        //nearby_hazards = Singleton.Get<Animals>().AnimalsInRadius(center, radius + hazard_threshold, OrganismType.Player);
    }

    public float GetHazardThreshold()
    {
        return hazard_threshold;
    }
}
