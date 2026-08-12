using UnityEngine;

// Put this script on the LITTLE CHEESE prefab, alongside your Pickup script.
// Keep its Collider NORMAL (do NOT tick "Is Trigger") and give it a Rigidbody.

[RequireComponent(typeof(Pickup))]
public class Cheeseandconfeti : MonoBehaviour
{
    [Header("Drag your objects in here")]
    public GameObject pizzaWithSauce;   // the pizza with sauce object (currently active in scene)
    public GameObject finishedPizza;    // the finished pizza object (starts turned off)

    [Header("Animation stuff (optional, leave empty if you don't want one)")]
    public Animator pizzaAnimator;      // Animator on the pizza with sauce
    public string cheeseTriggerName = "Cheese"; // name of the Trigger param in the Animator

    [Header("Confetti")]
    public ParticleSystem confetti;     // drag your confetti Particle System in here

    private bool hasMelted = false;     // stops it happening more than once
    private Pickup pickup;

    private void Start()
    {
        pickup = GetComponent<Pickup>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the thing we hit is the pizza with sauce
        if (collision.gameObject == pizzaWithSauce && !hasMelted)
        {
            hasMelted = true;
            PlayCheeseAnimation();
        }
    }

    private void PlayCheeseAnimation()
    {
        // stop it being picked up again mid animation
        if (pickup != null) pickup.enabled = false;

        // Play the animation you make (optional)
        if (pizzaAnimator != null)
        {
            pizzaAnimator.SetTrigger(cheeseTriggerName);
        }

        // Wait until the animation is done, then swap the object and remove the cheese
        float animLength = GetAnimationLength();
        Invoke(nameof(FinishPizza), animLength);
    }

    private float GetAnimationLength()
    {
        // Tries to grab the length of the current animation clip
        // so the swap happens right as the animation ends
        if (pizzaAnimator != null && pizzaAnimator.runtimeAnimatorController != null)
        {
            AnimatorClipInfo[] clipInfo = pizzaAnimator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
            {
                return clipInfo[0].clip.length;
            }
        }
        return 1f; // fallback time if it can't find the clip length
    }

    private void FinishPizza()
    {
        if (pizzaWithSauce != null) pizzaWithSauce.SetActive(false);
        if (finishedPizza != null) finishedPizza.SetActive(true);

        // pop the confetti
        if (confetti != null) confetti.Play();

        // little cheese piece disappears
        Destroy(this.gameObject);
    }
}