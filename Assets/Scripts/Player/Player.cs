using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
/// <summary>
/// Inspector class used to edit the <see cref="Player"/> component.
/// </summary>
[CustomEditor(typeof(Player))]
public class PlayerInspector: Editor {
    // ---  Attributes ---
        // -- Private Attributes --
            /// <summary> Is set if the grips foldout is visible. </summary>
            private bool _showGrips = false;
    // --- /Attributes ---

    // ---  Methods ---
        // -- Unity Events --
            /// <summary> Draws the inspector GUI. </summary>
            public override void OnInspectorGUI() {
                // Draw the default inspector.
                DrawDefaultInspector();

                // Show a button to select the user.
                if (GUILayout.Button("Select")) {
                    Object.FindObjectOfType<PlayerCamera>().Follow(this.target as Player);
                }
            }
    // --- /Methods ---
}
#endif

/// <summary>
/// The main component used to control the player in the world.
/// Requires all sub-components with the RequireComponent attribute.
/// </summary>
[RequireComponent(typeof(PlayerMovement))]
public class Player: MonoBehaviour {
    // ---  Attributes ---
        // -- Serialized Attributes --
            public Player other;
            public bool isDefaultPlayer;
            public bool allowSwap { get => !this.movement.falling; }

        // -- Public Attributes --
            [System.NonSerialized]
            public new PlayerCamera camera;
            [System.NonSerialized]
            public PlayerMovement movement;

            [System.NonSerialized]
            public PlayerInput playerInput;
    // --- /Attributes ---

    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Called when the component is instantiated in the scene.
            /// </summary>
            public void Awake() {
                this.camera = Object.FindObjectOfType<PlayerCamera>();
                this.playerInput = this.GetComponent<PlayerInput>();
                this.movement = this.GetComponent<PlayerMovement>();
                this.playerInput.enabled = this.isDefaultPlayer;
            }

            public void OnSwap() { 
                if (this.allowSwap) {
                    this.camera.Follow(this.other); this.playerInput.enabled = false; this.other.playerInput.enabled = true; 
                }
            }
    // --- /Methods ---
}
