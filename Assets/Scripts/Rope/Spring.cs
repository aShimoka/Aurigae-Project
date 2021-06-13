using UnityEngine;

// Wrap the code in the rope namespace.
namespace Rope {
///
///
///
[RequireComponent(typeof(Rigidbody2D))]
public class Spring: MonoBehaviour {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary> Rest distance of the spring. </summary>
            [Header("Parameters")]
            [Tooltip("Rest distance of the spring.")]
            public float distance;
            public float maxDistance;

            /// <summary> Force applied through the spring when its ends are further than the expected distance. </summary>
            [Tooltip("Force applied to the previous and next bodies when the spring is stretched.")]
            public float pullForce;

            /// <summary> Force applied through the spring when its ends are closer than the expected distance. </summary>
            [Tooltip("Force applied to the previous and next bodies when the spring is compressed.")]
            public float pushForce;

            /// <summary> Give in the spring system. </summary>
            [Tooltip("Give of the spring (aka. allowed discrepency delta).")]
            public float give;

        // -- Public Attributes --
            // [System.NonSerialized]
            public Rigidbody2D next;

            [System.NonSerialized]
            public Rigidbody2D current;

            [System.NonSerialized]
            public Vector2 lastApplied;
    // --- /Attributes ---

    // ---  Methods ---
        // -- Unity Events --
            public void Awake() { this.current = this.GetComponent<Rigidbody2D>(); }

            public void FixedUpdate() {
                if (this.next == null) { return; }

                // Get the distance between the elements.
                Vector2 previousToNext = this.current.position - this.next.position;
                float distance = previousToNext.magnitude;

                // Check the discrepency of the two elements.
                float discrepency = this.distance - distance;

                // If the spring is too compressed or too stretched.
                if (discrepency > this.maxDistance || discrepency < -this.maxDistance) {
                    // Snap the next element to the current location.
                    this.next.position = -previousToNext.normalized * this.maxDistance;
                } else if (discrepency > this.give || discrepency < -this.give) {
                    // Apply a force to both links.
                    Vector2 force = previousToNext.normalized * (discrepency > 0 ? this.pushForce : this.pullForce) * -discrepency;
                    this.next.AddForce(force); this.current.AddForce(-force);
                    this.lastApplied = force;
                }
            }

            public void OnDrawGizmos() {
                if (this.next == null) { return; }
                // Render the spring force.
                Gizmos.color = Color.red;
                Gizmos.DrawLine(this.next.position, this.next.position + this.lastApplied);
                Gizmos.DrawLine(this.current.position, this.current.position - this.lastApplied);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.current.position, this.next.position);
            }
    // --- /Methods ---
}
}
