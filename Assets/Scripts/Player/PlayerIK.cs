using UnityEngine;
using System;

/// <summary>
/// Component in charge of the movements of the player in the 2D XY plane.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerIK: MonoBehaviour {
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
            /// <summary> Reference to the <see cref="Animator"/> component on this object. </summary>
            public Animator animator { get; private set; }

        // -- Private Attributes --
            private Transform _leftHand;
            private Transform _rightHand;
    // --- /Attributes ---

    // ---  Methods ---
        // -- Unity Events --
            public void Awake() { this.animator = this.GetComponent<Animator>(); }
            public void OnAnimatorIK() {

                // Set the target of the right hand.
                if (this._rightHand != null) {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, this._rightHand.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, this._rightHand.rotation);
                }
                // Set the target of the left hand.
                if (this._leftHand != null) {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, this._leftHand.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, this._leftHand.rotation);
                }
            }
        // -- Public Methods --
            public void Grip(Grip[] grips) {
                // Store the grip's transforms.
                if (grips.Length >= 1) { this._leftHand  = grips[0].transform; } else { this._leftHand  = null; }
                if (grips.Length >= 2) { this._rightHand = grips[1].transform; } else { this._rightHand = null; }
            }
        // -- Private Methods --
    // --- /Methods ---
}
