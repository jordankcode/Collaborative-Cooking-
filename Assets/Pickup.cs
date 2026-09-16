using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    bool isHolding = false;
    public bool IsHolding => isHolding; // lets other scripts check if this is being held

    [SerializeField]
    float throwForce = 600f;
    [SerializeField]
    float maxDistance = 3f;
    float distance;

    TempParent tempParent;
    Rigidbody rb;
    PizzaTopping pizzaTopping; // if this is on the actual pizza, we'll find this

    Vector3 objectPos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tempParent = TempParent.Instance;
        pizzaTopping = GetComponent<PizzaTopping>(); // null if this isn't the pizza
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
            Hold();
    }

    private void OnMouseDown()
    {
        //pickup
        if (tempParent != null)
        {
            distance = Vector3.Distance(this.transform.position, tempParent.transform.position);

            if (distance <= maxDistance)
            {
                isHolding = true;
                rb.useGravity = false;
                rb.detectCollisions = true;

                this.transform.SetParent(tempParent.transform);

                // if this object is the pizza (has PizzaTopping), tell PizzaHolder we're carrying it
                if (pizzaTopping != null && PizzaHolder.instance != null)
                {
                    PizzaHolder.instance.PickUpPizza(this.gameObject);
                }
            }
        }
        else
        {
            Debug.Log("Temp Parent item not found in scene!");
        }
    }

    private void OnMouseUp()
    {
        Drop();
    }

    private void OnMousExit()
    {
        Drop();
    }

    private void Hold()
    {
        distance = Vector3.Distance(this.transform.position, tempParent.transform.position);

        if (distance >= maxDistance)
        {
            Drop();
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (Input.GetMouseButtonDown(1))
        {
            rb.AddForce(tempParent.transform.forward * throwForce);
            Drop();
        }
    }

    private void Drop()
    {
        if (isHolding)
        {
            isHolding = false;
            objectPos = this.transform.position;
            this.transform.position = objectPos;
            this.transform.SetParent(null);
            rb.useGravity = true;

            // if this was the pizza, tell PizzaHolder we're not carrying it anymore
            if (pizzaTopping != null && PizzaHolder.instance != null && PizzaHolder.instance.currentPizza == this.gameObject)
            {
                PizzaHolder.instance.DropPizza();
            }
        }

    }

}