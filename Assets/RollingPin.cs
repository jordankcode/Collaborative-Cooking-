using UnityEngine;

// Put this script on the ROLLING PIN object.
// Make sure the rolling pin has a Collider with "Is Trigger" ticked ON.
// Make sure the dough has a Collider too (does NOT need to be a trigger).

public class RollingPinFlatten : MonoBehaviour
{
    [Header("Drag your dough object in here")]
    public GameObject doughRound;      // the round dough object (with Animator on it)
    public GameObject doughFlat;       // the flat dough object (starts turned off)

    [Header("Animation stuff")]
    public Animator doughAnimator;     // Animator on the round dough
    public string flattenTriggerName = "Flatten"; // name of the Trigger param in the Animator

    private bool hasFlattened = false; // stops it from happening more than once

    private void OnTriggerEnter(Collider other)
    {
        // Check if the thing we hit is the dough
        if (other.gameObject == doughRound && !hasFlattened)
        {
            hasFlattened = true;
            PlayFlattenAnimation();
        }
    }

    private void PlayFlattenAnimation()
    {
        // Play the animation you make
        if (doughAnimator != null)
        {
            doughAnimator.SetTrigger(flattenTriggerName);
        }

        // Wait until the animation is done, then swap the object
        float animLength = GetAnimationLength();
        Invoke(nameof(SwapToFlatDough), animLength);
    }

    private float GetAnimationLength()
    {
        // Tries to grab the length of the current animation clip
        // so the swap happens right as the animation ends
        if (doughAnimator != null && doughAnimator.runtimeAnimatorController != null)
        {
            AnimatorClipInfo[] clipInfo = doughAnimator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
            {
                return clipInfo[0].clip.length;
            }
        }
        return 1f; // fallback time if it can't find the clip length
    }

    private void SwapToFlatDough()
    {
        if (doughRound != null) doughRound.SetActive(false);
        if (doughFlat != null) doughFlat.SetActive(true);
    }
}