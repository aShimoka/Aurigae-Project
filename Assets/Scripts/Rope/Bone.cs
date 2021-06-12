
using UnityEngine;

namespace Rope {

[RequireComponent(typeof(Rigidbody2D))]
public class Bone : MonoBehaviour {
    public Transform parent;
    public Transform child;
    private Rigidbody2D rigidbody;
    private MeshRenderer meshRenderer;
    public float springiness;
    public float distance;
    public float damp;
    public float pull;
    public float push;

    public void Awake() { rigidbody = GetComponent<Rigidbody2D>(); meshRenderer = GetComponent<MeshRenderer>(); }
    
    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rigidbody.position, parent.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rigidbody.position, child.position);

        Vector2 toParent = (rigidbody.position - (Vector2)parent.position);
        Vector2 toChild = (rigidbody.position - (Vector2)child.position);

        float parentDiscrepency = distance - toParent.magnitude;
        float childDiscrepency = distance - toChild.magnitude;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(rigidbody.position, rigidbody.position + (parentDiscrepency * toParent.normalized));
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(rigidbody.position, rigidbody.position + (childDiscrepency * toChild.normalized));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(rigidbody.position, rigidbody.position + ((parentDiscrepency * toParent.normalized) + (childDiscrepency * toChild.normalized)) / 2);
    }

    public void Compute() {
        // Get the distance to both parent and child.
        Vector2 toParent = (rigidbody.position - (Vector2)parent.position);
        Vector2 toChild = (rigidbody.position - (Vector2)child.position);

        if (toParent.magnitude > distance * 1.15f || toChild.magnitude > distance * 1.15f) {
            meshRenderer.material.color = Color.red;
        } else {
            meshRenderer.material.color = Color.blue;
        }

        float parentDiscrepency = (distance - toParent.magnitude);
        float childDiscrepency = (distance - toChild.magnitude);
        // if (parentDiscrepency > 0) { parentDiscrepency *= push; }
        if (parentDiscrepency < 0) { parentDiscrepency *= pull; }
        // if (childDiscrepency > 0) { childDiscrepency *= push; }
        if (childDiscrepency < 0) { childDiscrepency *= pull; }

        rigidbody.position += (parentDiscrepency * toParent.normalized + childDiscrepency * toChild.normalized) / 2;

        Vector2 direction = (toChild + toParent) / 2;

        // Vector2 velocityTarget = direction + (rigidbody.velocity + Physics2D.gravity * springiness);
        // Vector2 projectOnConnection = Vector3.Project(velocityTarget, direction);
        // rigidbody.velocity = Physics2D.gravity * springiness;//(velocityTarget - projectOnConnection) / (1 + damp * Time.fixedDeltaTime);
    }
}
}
