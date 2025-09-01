using System.Collections;
using UnityEngine;

public class Leyeveyer : Eyenteractable
{
    public float rotateDuration = 1f;

    private float direction = 1;

    public Eyenteractable eyenteractable;

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
        StartCoroutine(Rotate(initiator));
    }

    private IEnumerator Rotate(GameObject initiator)
    {
        Debug.Log("Lever activeyeted !!!");

        isEyenteractable = false;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(90f * direction, 0f, 0f);

        float elapsed = 0f;
        while (elapsed < rotateDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / rotateDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Snap to final rotation to avoid precision errors
        transform.rotation = endRotation;

        if (direction > 1)
        {
            eyenteractable.Eyenteract(initiator);
        }
        else
        {
            eyenteractable.Uneyenteract();
        }

        direction = -direction;
        isEyenteractable = true;
    }
}
