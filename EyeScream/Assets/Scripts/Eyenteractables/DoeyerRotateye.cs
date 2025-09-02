using System.Collections;
using UnityEngine;

public class DoeyerRotateye : Eyenteractable
{
    public float rotationDuration = 1f; // how long the rotation takes

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
        StartCoroutine(RotateZ90());
    }

    private IEnumerator RotateZ90()
    {
        gameObject.GetComponent<AudioSource>().Play();
        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 0, 90);

        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration);

            // Smooth Lerp
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);

            yield return null;
        }

        // Snap to final rotation to avoid floating point errors
        transform.rotation = endRot;
        gameObject.GetComponent<AudioSource>().Stop();
    }
}
