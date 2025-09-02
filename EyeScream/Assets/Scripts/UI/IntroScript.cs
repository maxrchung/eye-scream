using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    public float blinkSpeed;
    public float spinSpeed;
    public float endSpeed;
    public float slideSpeed;
    public float blinkMinInterval;
    public float blinkMaxInterval;
    public GameObject instructions;
    public GameObject title;
    public GameObject instruction_roulette;
    public GameObject ed;
    public GameObject continue_flash;
    public float openSpeed = 150f;
    PlayerInput menuActions;
    InputActionMap actionMap;
    InputAction next;
    InputAction prev;
    public bool input_locked = false;
    bool blinking = false;
    bool menu = false;
    int page = 0;
    bool started = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuActions = gameObject.GetComponent<PlayerInput>();
        actionMap = menuActions.currentActionMap;
        next = actionMap.FindAction("next");
        prev = actionMap.FindAction("prev");
        next.performed += MenuNextAction;
        prev.performed += MenuPreviousAction;
        //StartCoroutine(EndSequence());
        StartCoroutine(EyeOpen());
        //StartCoroutine(BlinkInterval());
        //StartCoroutine(EntryDelay());
    }

    void MenuNextAction(InputAction.CallbackContext obj)
    {
        if (!input_locked && started)
        {
            if (!menu)
            {
                StartCoroutine(MoveToInstructions());
                menu = true;
            }
            else if (page < 4)
            {
                StartCoroutine(TurnInstructionsForward());
            }
            else
            {
                SceneManager.LoadScene("Room 1");
            }
        }
    }

    void MenuPreviousAction(InputAction.CallbackContext obj)
    {
        if (!input_locked && menu && page > 0 && started)
            StartCoroutine(TurnInstructionsBackward());
    }

    IEnumerator ContinueFlash()
    {
        started = true;
        while (true)
        {
            continue_flash.SetActive(!continue_flash.activeSelf);
            yield return new WaitForSeconds(1.25f);
        }

    }


    IEnumerator TurnInstructionsBackward()
    {
        page -= 1;
        input_locked = true;
        float rotation = 0;
        while (rotation > -15)
        {
            instruction_roulette.transform.Rotate(0, 0, Time.deltaTime * -spinSpeed);
            rotation -= Time.deltaTime * spinSpeed;
            yield return null;
        }
        input_locked = false;
    }

    IEnumerator TurnInstructionsForward()
    {
        page += 1;
        input_locked = true;
        float rotation = 0;
        while (rotation < 15)
        {
            instruction_roulette.transform.Rotate(0, 0, Time.deltaTime * spinSpeed);
            rotation += Time.deltaTime * spinSpeed;
            yield return null;
        }
        input_locked = false;
    }


    IEnumerator MoveToInstructions()
    {
        input_locked = true;
        float unlerp = instructions.GetComponent<RectTransform>().anchoredPosition.y;
        while (instructions.GetComponent<RectTransform>().anchoredPosition.y < 0)
        {
            unlerp += slideSpeed * Time.deltaTime;
            instructions.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(instructions.GetComponent<RectTransform>().anchoredPosition.x, unlerp);
            yield return null;
        }
        input_locked = false;
    }

    IEnumerator EyeOpen()
    {
        yield return new WaitForSeconds(1f);
        while (title.GetComponent<RectTransform>().sizeDelta.y < 400)
        {
            float unlerp = title.GetComponent<RectTransform>().sizeDelta.y + openSpeed * Time.deltaTime;
            title.GetComponent<RectTransform>().sizeDelta =
                new Vector2(title.GetComponent<RectTransform>().sizeDelta.x,
                unlerp);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(BlinkInterval());
        yield return new WaitForSeconds(5f);
        StartCoroutine(ContinueFlash());
    }

    IEnumerator BlinkInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(blinkMinInterval, blinkMaxInterval));
            if (!blinking)
                StartCoroutine(TitleBlink());
            yield return null;
        }
    }

    IEnumerator TitleBlink()
    {
        blinking = true;
        float original = title.GetComponent<RectTransform>().sizeDelta.y;
        float lerper = 0;
        while (lerper < 1)
        {
            lerper += blinkSpeed * Time.deltaTime;
            float lerped = Mathf.Lerp(title.GetComponent<RectTransform>().sizeDelta.y, 0, lerper);
            title.GetComponent<RectTransform>().sizeDelta =
                new Vector2(title.GetComponent<RectTransform>().sizeDelta.x,
                lerped);
            yield return null;
        }
        lerper = 0;
        while (lerper < 1)
        {
            lerper += blinkSpeed * Time.deltaTime;
            float lerped = Mathf.Lerp(title.GetComponent<RectTransform>().sizeDelta.y, original, lerper);
            title.GetComponent<RectTransform>().sizeDelta =
                new Vector2(title.GetComponent<RectTransform>().sizeDelta.x,
                lerped);
            yield return null;
        }
        blinking = false;
    }
}
