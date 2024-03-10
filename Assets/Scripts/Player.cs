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

        Vector3 delta = speed * movement * Time.deltaTime;


        /*
        bool cancel_movement = false;

        // detect water
        if (Physics.Raycast(transform.position + delta, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Water")))
        {
            float rotation = 90.0f;
            Vector3 sample1 = Quaternion.AngleAxis(rotation, Vector3.up) * delta;
            Vector3 sample2 = Quaternion.AngleAxis(-rotation, Vector3.up) * delta;

            float rotation_delta = 0.0f;
            if (Physics.Raycast(transform.position + sample1, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Water")))
            {

            }
            else if (Physics.Raycast(transform.position + sample2, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Water")))
            {
                
            }
            else
            {
                cancel_movement = true;
            }


            while (Physics.Raycast(transform.position + delta, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Water")))
            {

            }
        }
        */

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
    }
}
