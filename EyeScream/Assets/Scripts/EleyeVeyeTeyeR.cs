using System.Collections;
using UnityEngine;

public class EleyeVeyeTeyeR : MonoBehaviour
{
    public float targetScale;
    public float growSpeed;
    public float elevatorPause;

    private float initialScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialScale = transform.localScale.y;

        StartCoroutine(Grow());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Grow()
    {
        while (true)
        {
            while (transform.localScale.y < targetScale)
            {
                float newY = Mathf.MoveTowards(transform.localScale.y, targetScale, growSpeed * Time.deltaTime);


                // Move position up by half of the added height to keep bottom fixed
                float deltaY = (newY - transform.localScale.y) / 2f;
                transform.position += Vector3.up * deltaY;

                // Update scale
                Vector3 scale = transform.localScale;
                scale.y = newY;

                transform.localScale = scale;

                yield return null;
            }

            yield return new WaitForSeconds(elevatorPause);

            // lol i am too stupid
            while (transform.localScale.y > initialScale)
            {
                float newY = Mathf.MoveTowards(transform.localScale.y, initialScale, growSpeed * Time.deltaTime);

                // Move position up by half of the added height to keep bottom fixed
                float deltaY = (newY - transform.localScale.y) / 2f;
                transform.position += Vector3.up * deltaY;

                // Update scale
                Vector3 scale = transform.localScale;
                scale.y = newY;
                transform.localScale = scale;

                yield return null;
            }

            yield return new WaitForSeconds(elevatorPause);
        }

    }
}
