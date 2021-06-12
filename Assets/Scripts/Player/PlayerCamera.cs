using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

/// <summary>
/// Simple component used to follow a game object in the scene.
/// </summary>
[RequireComponent(typeof(Camera))]
class PlayerCamera: MonoBehaviour {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>List of all the players that are being followed by the camera.</summary>
            public Player[] players = new Player[]{};
        // -- Public Attributes --
    // --- /Attributes ---

    // ---  Methods ---
        // -- Unity Events --
            /// <summary>Unity event fired on every frame. Updates the location of the camera.</summary>
            public void Update() {
                // If the players list is empty, disable the component.
                if (this.players.Length <= 0) {
                    Debug.LogWarning("There are no players to follow !");
                    this.enabled = false;
                    return;
                }

                // Get the average location of all the players.
                Vector2 sum = Vector2.zero; 
                foreach (Player player in this.players) { sum += (Vector2)player.transform.position; }
                this.transform.position = new Vector3{ x = sum.x / this.players.Length, y = sum.y / this.players.Length, z = this.transform.position.z };
            }

        // -- Public Methods --
            /// <summary>
            /// Starts following the provided list of players.
            /// </summary>
            /// <param name="players">The list of players to follow.</param>
            public void Follow(params Player[] players) { this.players = players; this.enabled = true; }
    // --- /Methods ---
}
