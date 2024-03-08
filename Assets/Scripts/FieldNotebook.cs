using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismInfo
{
    public OrganismType type;
    public bool identified = false;
    public List<ItemType> samples = new List<ItemType>();

    public OrganismInfo(OrganismType type)
    {
        this.type = type;
    }
}

public class FieldNotebook : MonoBehaviour
{
    Dictionary<OrganismType, OrganismInfo> organism_info = new Dictionary<OrganismType, OrganismInfo>();
    public void Discover(OrganismType type)
    {
        EnsureOrganismTracked(type);
        organism_info[type].identified = true;

        UpdateUi?.Invoke(organism_info[type]);
    }

    public void StoreSample(OrganismType organism, ItemType sample)
    {
        EnsureOrganismTracked(organism);
        if (organism_info[organism].samples.Contains(sample))
        {
            return;
        }
        organism_info[organism].samples.Add(sample);

        UpdateUi?.Invoke(organism_info[organism]);
    }

    void EnsureOrganismTracked(OrganismType organism)
    {
        if (!organism_info.ContainsKey(organism))
        {
            organism_info[organism] = new OrganismInfo(organism);
        }
    }

    public delegate void UpdateUiFn(OrganismInfo info);
    public event UpdateUiFn UpdateUi;
}
