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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        notebook = GetComponent<FieldNotebook>();
    }

    void Update()
    {
        Vector3 movement = Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward;
        controller.SimpleMove(speed * movement);


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
    }
}
