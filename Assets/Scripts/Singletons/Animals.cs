using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animals : MonoBehaviour
{
    Dictionary<OrganismType, List<GameObject>> animals = new Dictionary<OrganismType, List<GameObject>>();

    public void RegisterAnimal(GameObject instance, OrganismType type)
    {
        if (!animals.ContainsKey(type))
        {
            animals[type] = new List<GameObject>();
        }
        animals[type].Add(instance);
    }

    public List<GameObject> AnimalsInRadius(Vector3 pos, float radius, params OrganismType[] types)
    {
        List<GameObject> result = new List<GameObject>();
        foreach (OrganismType type in types)
        {
            foreach (GameObject animal in animals[type])
            {
                if ((animal.transform.position - pos).magnitude < radius)
                {
                    result.Add(animal);
                }
            }
        }

        return result;
    }
}
