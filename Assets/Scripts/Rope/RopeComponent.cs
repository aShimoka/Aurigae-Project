using UnityEngine;
using Rope;
using System.Collections.Generic;

public class RopeComponent: MonoBehaviour {
    public int Length;
    public End Start;
    public End End;
    public GameObject BonePrefab;

    // public List<Bone> bones;

    public void Awake() {
        // bones = new List<Bone>();
        Joint2D previousJoint = Start.link;
        for (int i = 0; i < Length; i++) {
            GameObject bone = GameObject.Instantiate(BonePrefab);
            bone.name = $"Bone #{i}";
            bone.transform.SetParent(transform);
            bone.transform.position = Vector2.Lerp(End.transform.position, Start.transform.position, (float)i / (Length - 1)) + Vector2.down * 10;

            Joint2D joint = bone.GetComponent<Joint2D>();

            previousJoint.connectedBody = bone.GetComponent<Rigidbody2D>();
            previousJoint = joint;
        }

        previousJoint.connectedBody = End.link.GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate() {
        // foreach (Bone b in bones) { b.Compute(); }
    }
}
