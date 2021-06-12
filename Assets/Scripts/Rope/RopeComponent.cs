using UnityEngine;
using Rope;
using System.Collections.Generic;
using UnityEngine.Events;

public class RopeComponent: MonoBehaviour {
    public int Length;
    public End Start;
    public End End;
    public GameObject BonePrefab;
    static BreakEvent onBreak = new RopeComponent.BreakEvent();

    public bool broken;

    // public List<Bone> bones;

    public void Awake() {
        broken = false;
        // bones = new List<Bone>();
        Bone previous = Start.link;
        for (int i = 0; i < Length; i++) {
            GameObject bone = GameObject.Instantiate(BonePrefab);
            bone.name = $"Bone #{i}";
            bone.transform.SetParent(transform);
            bone.transform.position = Vector2.Lerp(End.transform.position, Start.transform.position, (float)i / (Length - 1)) + Vector2.down * 10;

            Bone currentBone = bone.GetComponent<Bone>();
            currentBone.AttachParent(previous);
            currentBone.onBreak = onBreak;

            previous = currentBone;
        }

        End.link.AttachParent(previous);
        onBreak.AddListener(BreakRope);

    }

    public class BreakEvent : UnityEvent<Bone, Bone> {
        
    }

    public void BreakRope(Bone firstBone, Bone secondBone){
        if(!broken){
            broken  = true;
            secondBone.DetachParent();
        }
    }

}
