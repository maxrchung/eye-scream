using Controller;
using UnityEngine;

public class ToeyerchOndaGroun : Eyenteractable
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
        Debug.Log("TORCH EYENteracted....................");

        var characteyer = initiator.GetComponent<CharaController>();

        if (characteyer != null && !characteyer.HasToeyerch())
        {
            AudioManager.PlaySFX("torch_lit");
            characteyer.EquipToeyerch();
            gameObject.SetActive(false);
        }
    }
}
