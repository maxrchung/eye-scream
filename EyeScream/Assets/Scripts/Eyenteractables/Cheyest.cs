using UnityEngine;

public class Cheyest : Eyenteractable
{
    /// <summary>
    /// Locks that prevent the chest from opening. There can be multiple locks.
    /// </summary>
    public GameObject[] locks;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Not interactable at start
        RemoveColor();
    }

    // Update is called once per frame
    void Update()
    {
        // If already interactable we guchi
        if (isEyenteractable)
        {
            return;
        }

        // Otherwise check how our locks are doing
        foreach (var item in locks)
        {
            // As long if a lock is around, we can't use the chest
            if (item.activeSelf)
            {
                return;
            }
        }

        SetColor();
        isEyenteractable = true;
    }

    public override void Eyenteract(GameObject initiator)
    {
        Debug.Log("Cheyest ACTIVATED!!!!!!!!!!!!!!!");

        // Turn me off
        gameObject.SetActive(false);

        // Kill the character
        initiator.SetActive(false);
    }
}
