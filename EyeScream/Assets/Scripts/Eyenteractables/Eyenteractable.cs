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
    public string coleyer;

    /// <summary>
    /// Whether the interacting sheyet should be interacted with.
    /// </summary>
    public bool isEyenteractable;

    /// <summary>
    /// Implemented by the child for specific functionality
    /// </summary>
    public abstract void Eyenteract();
}