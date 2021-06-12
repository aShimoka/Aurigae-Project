
using UnityEngine;

namespace Rope {

[RequireComponent(typeof(Rigidbody2D))]
public class Bone : MonoBehaviour {
    public Transform parent;
    public Transform child;
    private Rigidbody2D rigidbody;
    private MeshRenderer meshRenderer;
    public float maxDistance;
    public float warnDistance;

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

    }

    public void Compute() {

        // Check for rope break
        /*if(parent != null && onBreak != null){
            Vector2 toParent = (rigidbody.position - (Vector2)parent.position);

            if(toParent.magnitude > maxDistance){
                onBreak.Invoke();
            }
        }*/
    }

    public void FixedUpdate(){
        Compute();
    }

}
}
