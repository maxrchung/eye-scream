using UnityEngine;
using System.Collections;

public class EndScript : MonoBehaviour
{
    public GameObject gameLogo;
    public GameObject ed;
    public float endSpeed;
    public float blinkSpeed = 10f;
    public float openSpeed;
    public float blinkMinInterval = 0.4f;
    public float blinkMaxInterval = 5;
    bool blinking = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void GameEnd()
    {
        StartCoroutine(EyeOpen());
    }

    IEnumerator EyeOpen()
    {
        yield return new WaitForSeconds(1f);
        while (gameLogo.GetComponent<RectTransform>().sizeDelta.y < 400)
        {
            float unlerp = gameLogo.GetComponent<RectTransform>().sizeDelta.y + openSpeed * Time.deltaTime;
            gameLogo.GetComponent<RectTransform>().sizeDelta =
                new Vector2(gameLogo.GetComponent<RectTransform>().sizeDelta.x,
                unlerp);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(BlinkInterval());
        yield return new WaitForSeconds(2f);
        StartCoroutine(EndSequence());
    }

    IEnumerator BlinkInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(blinkMinInterval,blinkMaxInterval));
            if(!blinking)
                StartCoroutine(gameLogoBlink());
            yield return null;
        }
    }

    IEnumerator gameLogoBlink()
    {
        blinking = true;
        float original = gameLogo.GetComponent<RectTransform>().sizeDelta.y;
        float lerper = 0;
        while (lerper < 1)
        {
            lerper += blinkSpeed * Time.deltaTime;
            float lerped = Mathf.Lerp(gameLogo.GetComponent<RectTransform>().sizeDelta.y, 0, lerper);
            gameLogo.GetComponent<RectTransform>().sizeDelta =
                new Vector2(gameLogo.GetComponent<RectTransform>().sizeDelta.x,
                lerped);
            yield return null;
        }
        lerper = 0;
        while (lerper < 1)
        {
            lerper += blinkSpeed * Time.deltaTime;
            float lerped = Mathf.Lerp(gameLogo.GetComponent<RectTransform>().sizeDelta.y, original, lerper);
            gameLogo.GetComponent<RectTransform>().sizeDelta =
                new Vector2(gameLogo.GetComponent<RectTransform>().sizeDelta.x,
                lerped);
            yield return null;
        }
        blinking = false;
    }
    
    IEnumerator EndSequence()
    {
        float lerper = 0;
        while (lerper < 1)
        {
            lerper += endSpeed * Time.deltaTime;
            float gameLogoTransform = Mathf.Lerp(gameLogo.GetComponent<RectTransform>().anchoredPosition.x, -50f, lerper);
            float gameLogoWidth = Mathf.Lerp(gameLogo.GetComponent<RectTransform>().sizeDelta.x, 1025, lerper);
            float edTransform = Mathf.Lerp(ed.GetComponent<RectTransform>().anchoredPosition.x, 450f, lerper);
            gameLogo.GetComponent<RectTransform>().anchoredPosition = new Vector2(gameLogoTransform,
            gameLogo.GetComponent<RectTransform>().anchoredPosition.y);
            gameLogo.GetComponent<RectTransform>().sizeDelta = new Vector2(gameLogoWidth,
            gameLogo.GetComponent<RectTransform>().sizeDelta.y);
            ed.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(edTransform, ed.GetComponent<RectTransform>().anchoredPosition.y);
            yield return null;
        }
    }
}
