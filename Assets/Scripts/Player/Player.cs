using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The main component used to control the player in the world.
/// Requires all sub-components with the RequireComponent attribute.
/// </summary>
[RequireComponent(typeof(PlayerMovement))]
public class Player: MonoBehaviour {
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
    // --- /Attributes ---

    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Called when the component is instantiated in the scene.
            /// </summary>
            public void Awake() {
                
            }
    // --- /Methods ---
}
