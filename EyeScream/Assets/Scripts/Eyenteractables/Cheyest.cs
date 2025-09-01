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

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in locks)
        {
            // As long if a lock is around, we can't use the chest
            if (item.activeSelf)
            {
                return;
            }
        }

        isEyenteractable = true;
    }

    public override void Eyenteract(GameObject initiator)
    {
        Debug.Log("Cheyest ACTIVATED!!!!!!!!!!!!!!!");
    }
}
