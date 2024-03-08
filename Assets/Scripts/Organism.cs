using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrganismType
{
    Player,
    SmallBird,
    Snake,
    Butterfly,
    Deer,
    Jaguar,
}

public class Organism : MonoBehaviour
{
    [SerializeField]
    OrganismType organism_type;

    [SerializeField]
    GameObject corpse;

    void Start()
    {
        Singleton.Get<Animals>().RegisterAnimal(gameObject, organism_type);
    }


    public OrganismType GetOrganismType()
    {
        return organism_type;
    }

    public static string GetTypeName(OrganismType type)
    {
        switch (type)
        {
            case OrganismType.Player:
                return "Player";
            case OrganismType.SmallBird:
                return "Small bird";
            case OrganismType.Snake:
                return "Snake";
            case OrganismType.Butterfly:
                return "Butterfly";
            case OrganismType.Deer:
                return "Deer";
            case OrganismType.Jaguar:
                return "Jaguar";
        }

        return "INVALID";
    }

    public GameObject Kill()
    {
        Destroy(gameObject);
        if (corpse)
        {
            return Instantiate(corpse, transform.position, Quaternion.identity);
        }

        return null;
    }

    public string GetName()
    {
        return GetTypeName(organism_type);
    }
}
