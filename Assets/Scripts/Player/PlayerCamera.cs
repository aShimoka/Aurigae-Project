using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

/// <summary>
/// Simple component used to follow a game object in the scene.
/// </summary>
[RequireComponent(typeof(Camera))]
public class PlayerCamera: MonoBehaviour {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>List of all the players that are being followed by the camera.</summary>
            public Player[] players = new Player[]{};
            public float transitionTime;

        // -- Public Attributes --
            /// <summary> Stores the previous location of the camera. Used to smooth the change between players. </summary>
            [System.NonSerialized]
            public Vector2 previousLocation;

            [System.NonSerialized]
            public float ease;

            public Vector3 average { get { 
                Vector2 sum = Vector2.zero; 
                foreach (Player player in this.players) { sum += (Vector2)player.transform.position; } 
                sum /= this.players.Length; 
                return new Vector3 { x = sum.x, y = sum.y, z = this.transform.position.z }; 
            }}
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

                // Check if the ease flag is set.
                if (this.ease > 0.001f) {
                    // Interpolate the position between the vectors.
                    this.transform.position = Vector3.Lerp(this.previousLocation, this.average, Mathf.SmoothStep(1, 0, this.ease));
                    this.ease -= Time.unscaledDeltaTime / this.transitionTime;
                } else {
                    Time.timeScale = 1;
                    this.transform.position = this.average;
                }
            }

        // -- Public Methods --
            /// <summary>
            /// Starts following the provided list of players.
            /// </summary>
            /// <param name="players">The list of players to follow.</param>
            public void Follow(params Player[] players) { Time.timeScale = 0; this.players = players; this.enabled = true; this.ease = 1f; this.previousLocation = this.transform.position; }
    // --- /Methods ---
}
