using System.Collections;
using UnityEngine;

public class Doeyer : Eyenteractable
{
    public int maxCount = 3;
    public float riseHeight = 3f;      // how far up the door rises
    public float riseSpeed = 2f;       // how fast it rises
    public float riseDuration = 1f;

    private int count = 0;
    private bool isOpened = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Eyenteract(GameObject initiator)
    {
        if (isOpened)
        {
            return;
        }

        count++;

        if (count >= maxCount)
        {
            isOpened = true;
            AudioManager.PlaySFX("door_opening");
            StartCoroutine(MoveDoeyer());
        }
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

    public override void Uneyenteract()
    {
        count--;
    }
}
