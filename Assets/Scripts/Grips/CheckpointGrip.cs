using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class CheckpointGrip: Grip {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>List of all the players that are being followed by the camera.</summary>
            public CheckpointGrip other;
    // --- /Attributes ---

    // ---  Methods ---
        // -- Public Methods --
            public override void OnGrab() {
                // Store the checkpoint in the users.
                Player[] players = Object.FindObjectsOfType<Player>();
                players[0].SetCheckpoint(this);
                players[1].SetCheckpoint(this.other);
            }
    // --- /Methods ---
}
