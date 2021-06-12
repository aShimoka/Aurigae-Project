
using UnityEngine;

namespace Rope {

[RequireComponent(typeof(Rigidbody2D))]
public class Bone : MonoBehaviour {
    public Transform parent;
    public Transform child;
    private Rigidbody2D rigidbody;
    private MeshRenderer meshRenderer;
    public float maxDistance;

    public RopeComponent.BreakEvent onBreak;
    
    public void Awake() { rigidbody = GetComponent<Rigidbody2D>(); meshRenderer = GetComponent<MeshRenderer>(); }
    
    public void AttachParent(Bone parent){
        this.parent = parent.transform;
        parent.GetComponent<Joint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        parent.GetComponent<Bone>().AttachChild(this);
    }

    public void AttachChild(Bone child){
        this.child = child.transform;
    }

    public void DetachParent(){
        parent.GetComponent<Joint2D>().connectedBody = null;
        parent.DetachChildren();
        parent = null;
    }

    public void DetachChild(){
        child = null;
    }

    public void OnDrawGizmos() {
        // Gizmos.color = Color.red;
        // Gizmos.DrawLine(rigidbody.position, parent.position);
        // Gizmos.color = Color.blue;
        // Gizmos.DrawLine(rigidbody.position, child.position);

        // Vector2 toParent = (rigidbody.position - (Vector2)parent.position);
        // Vector2 toChild = (rigidbody.position - (Vector2)child.position);

        //float parentDiscrepency = distance - toParent.magnitude;
        //float childDiscrepency = distance - toChild.magnitude;
    }

    public void Compute() {
        // Get the distance to both parent and child.
        Vector2 toChild = (rigidbody.position - (Vector2)child.position);

        // Check for rope break
        if(parent != null){
            Vector2 toParent = (rigidbody.position - (Vector2)parent.position);
        
            if(toParent.magnitude > maxDistance){
                onBreak.Invoke(parent.GetComponent<Bone>(), this);
            }
        }
        if(child != null){
            if(toChild.magnitude > maxDistance){
                onBreak.Invoke(this, child.GetComponent<Bone>());
            }
        }
    }

    public void FixedUpdate(){
        Compute();
    }

}
}
