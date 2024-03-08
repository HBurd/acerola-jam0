using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Feather,
    Dung,
}

public class Item : MonoBehaviour
{
    [SerializeField]
    ItemType type;

    [SerializeField]
    OrganismType source_organism;

    public ItemType GetItemType()
    {
        return type;
    }

    public OrganismType GetSourceOrganism()
    {
        return source_organism;
    }


    public static string GetName(ItemType type)
    {
        switch (type)
        {
            case ItemType.Feather:
                return "Feather";
            case ItemType.Dung:
                return "Dung";
            default:
                return "INVALID";
        }
    }
}
