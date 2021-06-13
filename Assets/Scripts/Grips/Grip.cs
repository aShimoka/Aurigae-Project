using UnityEngine;

/// <summary>
/// Simple component used to mark a element that is grippable.
/// </summary>
public class Grip: MonoBehaviour {
    // ---  Attributes ---
        // -- Public Attributes --
            public bool isGrabbed { get; private set; } = false;
    // --- /Attributes ---
    // ---  Methods ---
        // -- Public Methods --
            public virtual void OnGrab() { this.isGrabbed = true; }
            public virtual void OnRelase() { this.isGrabbed = false; }
    // --- /Methods ---
}
