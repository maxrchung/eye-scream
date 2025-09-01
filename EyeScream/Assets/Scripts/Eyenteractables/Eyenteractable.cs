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
}