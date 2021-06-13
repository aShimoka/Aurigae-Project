
using UnityEngine;

namespace Rope {

    [RequireComponent(typeof(Rigidbody2D))]
    public class Bone : MonoBehaviour
    {
        public HingeJoint2D Joint {
            get {
                return GetComponent<HingeJoint2D>();
            }
        }

        public Joint2D SecondJoint
        {
            get
            {
                var joints = GetComponents<HingeJoint2D>();
                return joints.Length > 1 ? joints[1] : null;
            }
        }

        public Rigidbody2D rigidbody
        {
            get
            {
                return GetComponent<Rigidbody2D>();
            }
        }

        public RopeComponent Rope
        {
            get
            {
                return transform.parent.GetComponent<RopeComponent>();
            }
        }

        public void FixedUpdate()
        {
            if (Joint)
            {
                float magnitude = (rigidbody.position - Joint.connectedBody.position).magnitude;

                if (magnitude > 1.02)
                {
                    Rope.AddFriction(magnitude - 1.02f);
                }
            }
        }
    }
}
