using UnityEngine;
using Rope;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Rope
{
    public class RopeComponent : MonoBehaviour
    {
        public int Length;
        public End Start;
        public End End;
        public GameObject BonePrefab;
        public static BreakEvent onBreak = new RopeComponent.BreakEvent();

        public bool broken;

        // public List<Bone> bones;

        public void Awake()
        {
            // bones = new List<Bone>();
            Bone previous = Start.link;
            for (int i = 0; i < Length; i++)
            {
                GameObject bone = GameObject.Instantiate(BonePrefab);
                bone.name = $"Bone #{i}";
                bone.transform.SetParent(transform);
                bone.transform.position = Vector2.Lerp(End.transform.position, Start.transform.position, (float)i / (Length - 1));
                bone.transform.localScale = Vector3.one * 0.3f;
                Debug.Log(bone.transform.position);

                Bone currentBone = bone.GetComponent<Bone>();
                currentBone.AttachParent(previous);
                currentBone.onBreak = onBreak;

                previous = currentBone;
            }

            End.link.AttachParent(previous);
            End.link.GetComponents<Joint2D>()[0].connectedBody = previous.GetComponent<Rigidbody2D>();
            onBreak.AddListener(BreakRope);

            broken = false;
        }

        public class BreakEvent : UnityEvent
        {

        }

        public void BreakRope()
        { 
            if (!broken)
            {
                broken = true;
                End.Unbind();
                Destroy(End.link.GetComponents<Joint2D>()[1]);
                End.link = null;

            }
        }

    }
}