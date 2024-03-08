using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrganismInfoUi : MonoBehaviour
{
    [SerializeField]
    OrganismType type;

    [SerializeField]
    TMP_Text text;

    [SerializeField]
    GameObject samples;

    [SerializeField]
    GameObject sample_ui;

    void Start()
    {
        Singleton.GetFieldNotebook().UpdateUi += OnDiscover;
        gameObject.SetActive(false);
    }

    void OnDiscover(OrganismInfo info)
    {
        if (info.type == type)
        {
            gameObject.SetActive(true);
            if (info.identified)
            {
                text.text = Organism.GetTypeName(type);
            }
            else
            {
                text.text = "Undiscovered";
            }

            UpdateSamples(info.samples);
        }
    }

    void UpdateSamples(List<ItemType> samples)
    {
        for (int i = 0; i < this.samples.transform.childCount; ++i)
        {
            Destroy(this.samples.transform.GetChild(i).gameObject);
        }

        foreach (ItemType item in samples)
        {
            TMP_Text text = Instantiate(sample_ui, this.samples.transform).GetComponent<TMP_Text>();
            text.text = Item.GetName(item);
        }
    }
}
