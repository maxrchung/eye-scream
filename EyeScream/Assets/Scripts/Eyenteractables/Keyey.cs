using Controller;
using UnityEngine;

public class Keyey : Eyenteractable
{
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
        Debug.Log("key EYENteracted....................");

        var characteyer = initiator.GetComponent<CharaController>();

        if (characteyer != null && !characteyer.HasKeyey())
        {
            characteyer.EquipKeyey();
            isEyenteractable = false;
            gameObject.SetActive(false);
        }
    }
}
