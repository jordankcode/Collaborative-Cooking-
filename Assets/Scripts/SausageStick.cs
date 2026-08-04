using UnityEngine;

// Put this script on the SAUSAGE SLICE prefab, alongside your Pickup script.
// Make sure the Pizza object has the tag "Pizza" set on it (Inspector top left tag dropdown).

[RequireComponent(typeof(Pickup))]
public class SausageStick : MonoBehaviour
{
    [Header("How long it needs to sit still on the pizza before it sticks")]
    public float stickTime = 4f;

    private float touchTimer = 0f;
    private bool isTouchingPizza = false;
    private bool hasStuck = false;

    private Transform pizzaTransform;
    private Rigidbody rb;
    private Pickup pickup;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pickup = GetComponent<Pickup>();
    }

    private void Update()
    {
        if (hasStuck) return;

        // only count up the timer if it's touching the pizza AND not currently being held
        if (isTouchingPizza && !pickup.IsHolding)
        {
            touchTimer += Time.deltaTime;

            if (touchTimer >= stickTime)
            {
                StickToPizza();
            }
        }
        else
        {
            // reset the timer if it gets picked back up or lifted off
            touchTimer = 0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pizza"))
        {
            isTouchingPizza = true;
            pizzaTransform = collision.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pizza"))
        {
            isTouchingPizza = false;
            touchTimer = 0f;
        }
    }

    private void StickToPizza()
    {
        hasStuck = true;

        // freeze it in place so it can't fall off
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        // parent it to the pizza so it moves with it if the pizza moves
        this.transform.SetParent(pizzaTransform);

        // stop it being picked up again once it's stuck
        pickup.enabled = false;
    }
}
