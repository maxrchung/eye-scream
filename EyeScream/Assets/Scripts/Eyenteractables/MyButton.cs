using UnityEngine;

public class MyButton : Eyenteractable
{
    public Eyenteractable door;

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
        Debug.Log("MyButton eyenteracted!!!");
        AudioManager.PlaySFX("beep");
        door.Eyenteract(initiator);
        isEyenteractable = false;
        RemoveColor();
    }
}
