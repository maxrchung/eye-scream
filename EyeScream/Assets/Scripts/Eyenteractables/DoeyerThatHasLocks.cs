using System.Collections;
using UnityEngine;

public class DoeyerThatHasLocks : MonoBehaviour
{
    /// <summary>
    /// Locks that prevent the chest from opening. There can be multiple locks.
    /// </summary>
    public GameObject[] locks;

    public float riseHeight = 3f;      // how far up the door rises
    public float riseSpeed = 2f;       // how fast it rises
    public float riseDuration = 1f;


    private bool isDone = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isDone)
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

        isDone = true;
        StartCoroutine(MoveDoeyer());
    }

    IEnumerator MoveDoeyer()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * riseHeight;

        float t = 0f;
        while (t < riseDuration)
        {
            t += Time.deltaTime * riseSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
    }
}
