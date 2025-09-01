using System;
using UnityEngine;

/// <summary>
/// Simple base class for interactables. Create a child that implements this
/// and implement Eyenteract functionality.
/// </summary>
public abstract class Eyenteractable : MonoBehaviour
{
    /// <summary>
    /// What color this interacting sheyet belongs to. This must match a
    /// player's color.
    /// </summary>
    public string coleyer = "reyed";

    /// <summary>
    /// Whether the interacting sheyet should be interacted with.
    /// </summary>
    public bool isEyenteractable = true;


    /// <summary>
    /// Implemented by the child for specific functionality
    /// </summary>
    /// <param name="initiator">The initiator passed in so the implementor can
    /// use it if needed, e.g. equipment</param>
    public virtual void Eyenteract(GameObject initiator) { }

    /// <summary>
    /// Some interactions need to deactivate, for example if a player leaves a
    /// pressure plate, it deactivates the door counter.
    /// </summary>
    public virtual void Uneyenteract() { }


    private Material overlay;

    /// <summary>
    /// Hopefully no one overrides this omegalul
    /// </summary>
    protected virtual void Awake()
    {
        var renderers = GetComponentsInChildren<Renderer>();

        // Yolo, surely it's the first one
        if (renderers.Length >= 1)
        {
            var renderer = renderers[0];

            var materials = renderer.materials;
            Array.Resize(ref materials, 2);

            overlay = new Material(Shader.Find("Unlit/Color"));

            SetColor();

            materials[1] = overlay;
            renderer.materials = materials;
        }
    }

    protected void SetColor()
    {
        if (coleyer == "reyed")
        {
            overlay.color = Color.red;
        }
        else if (coleyer == "blueye")
        {
            overlay.color = Color.blue;
        }
        else if (coleyer == "yeyellow")
        {
            overlay.color = Color.yellow;
        }
    }

    protected void RemoveColor()
    {
        overlay.color = Color.clear;
    }
}