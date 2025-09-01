using Controller;
using UnityEngine;

public class DoeyerActiveyete : Eyenteractable
{
    public Doeyer doeyer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        var characteyer = other.GetComponent<CharaController>();
        if (characteyer != null && characteyer.coleyer == coleyer)
        {
            Debug.Log("Doeyer Activeyete !!!");

            doeyer.IncrementCount();
        }
    }
    void OnTriggerExit(Collider other)
    {
        var characteyer = other.GetComponent<CharaController>();
        if (characteyer != null && characteyer.coleyer == coleyer)
        {
            doeyer.DecrementCount();
        }
    }
}
