using UnityEngine;

namespace Rope {
[RequireComponent(typeof(Rigidbody2D))]
public class End : MonoBehaviour {
    public Transform parent;
    public Transform child;
    public Bone link;

    /// <summary>
    /// Binds the end of the rope to a given transform.
    /// </summary>
    public void Bind(Transform to) {
        // Store the parent.
        parent = to;
    }
    public void Unbind() {
        parent = null;
    }

    public void FixedUpdate() {
        if (parent) {
            // Snap to the parent.
            transform.position = parent.position;
        }
    }
}
}