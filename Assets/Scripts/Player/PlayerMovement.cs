using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
/// <summary>
/// Inspector class used to edit the <see cref="PlayerMovement"/> component.
/// </summary>
[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementInspector: Editor {
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

                // Draw all the closest grips.
                if (this._showGrips = EditorGUILayout.Foldout(this._showGrips, "Test")) {
                    PlayerMovement target = (PlayerMovement)this.target;
                    if (target.closestGrips.Item1 != null) {
                        EditorGUILayout.Vector2Field("First Closest", target.closestGrips.Item1.transform.position);
                    }
                    if (target.closestGrips.Item2 != null) {
                        EditorGUILayout.Vector2Field("Second Closest", target.closestGrips.Item2.transform.position);
                    }
                    if (target.closestGrips.Item3 != null) {
                        EditorGUILayout.Vector2Field("Third Closest", target.closestGrips.Item3.transform.position);
                    }
                    if (target.closestGrips.Item4 != null) {
                        EditorGUILayout.Vector2Field("Fourth Closest", target.closestGrips.Item4.transform.position);
                    }
                }
            }
    // --- /Methods ---
}
#endif

/// <summary>
/// Component in charge of the movements of the player in the 2D XY plane.
/// TODO: Implement the more complex grip system.
/// </summary>
[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D), typeof(GripParser))]
class PlayerMovement: MonoBehaviour {
    // ---  Attributes ---
    // -- Serialized Attributes --
    /// <summary>Movement speed of the player element.</summary>
    [Tooltip("Speed of movement of the player element.")]
            public float speed;

            /// <summary>Maximum distance to a grip.</summary>
            [Tooltip("Maximum distance to a given grip.")]
            public float reach;

            public bool falling;
            public float deadzone;

            public PlayerIK playerIkHandler;

        // -- Public Attributes --
            /// <summary> Rigidbody of the player object. </summary>
            public new Rigidbody2D rigidbody { get; private set; }

            /// <summary> Parser of the level's grip elements. </summary>
            public GripParser parser { get; private set; }

            /// <summary> List of all the grips that are closest to the instance. </summary>
            public System.Tuple<Grip, Grip, Grip, Grip> closestGrips { get; private set; } = new System.Tuple<Grip, Grip, Grip, Grip>(null, null, null, null);

        // -- Private Attributes --
            /// <summary> Direction of the last input made by the user. </summary>
            private Vector2 _lastInput;

            private Joint2D[] _joints;
    // --- /Attributes ---

    // ---  Methods ---
    // -- Unity Events --
    /// <summary>
    /// Called when the component is instantiated in the scene.
    /// </summary>
    [ExecuteInEditMode]
            public void Awake() {
                // Query the rigidbody component.
                this.rigidbody = this.GetComponent<Rigidbody2D>();

                // Get the grip parser method.
                this.parser = this.GetComponent<GripParser>();
                // Use the "reach" of the character.
                this.parser.chunkSize = this.reach;
                this.StartCoroutine(this._AfterEndOfFrame());
            }

            private IEnumerator _AfterEndOfFrame() {
                    yield return new WaitForEndOfFrame();

                    // Generate the grip chunk grid.
                    this.parser.Generate();

                    /*SpringJoint2D[] joints = new SpringJoint2D[] {
                        this.gameObject.AddComponent<SpringJoint2D>(),
                        this.gameObject.AddComponent<SpringJoint2D>(),
                        this.gameObject.AddComponent<SpringJoint2D>(),
                        this.gameObject.AddComponent<SpringJoint2D>(),
                    };
                    joints[0].autoConfigureDistance = false; joints[0].distance = 0;
                    joints[1].autoConfigureDistance = false; joints[1].distance = 0;
                    joints[2].autoConfigureDistance = false; joints[2].distance = 0;
                    joints[3].autoConfigureDistance = false; joints[3].distance = 0;
                    this._joints = joints;*/
            }

            /// <summary>
            /// Event fired by the <see cref="PlayerInput"/> component when the user changes movement direction.
            /// </summary>
            /// <param name="input">Parameters of the provided movement direction.</param>
            public void OnMove(InputValue input) {
                // If the component is disabled, do nothing.
                if (!this.enabled || this.falling) { return; }

                // Get the direction of the movement.
                this._lastInput = input.Get<Vector2>();
                // Ensure that the gravity is disabled.
                this.rigidbody.gravityScale = 0f;

                // If the input is near zero.
                if (this._lastInput.magnitude < this.deadzone) {
                    // Fix the player on the screen.
                    this.rigidbody.bodyType = RigidbodyType2D.Static;
                } else {
                    // Allow the player to move on the screen.
                    this.rigidbody.bodyType = RigidbodyType2D.Dynamic;
                }
            }

            public void OnDrop(){
                if (falling)
                {
                    if (closestGrips.Item1 != null)
                    {
                        falling = false;
                        rigidbody.gravityScale = 0;
                rigidbody.bodyType = RigidbodyType2D.Static;
                    }
                }
                else {
                    falling = true;
                    rigidbody.bodyType = RigidbodyType2D.Dynamic;
                    rigidbody.gravityScale = 1;
                }
            }

            public void OnBreak()
            {
                Rope.RopeComponent.onBreak.Invoke();
            }

            /// <summary>
            /// Called on a fixed timer for physics simulations.
            /// </summary>
            public void FixedUpdate() {
                if (rigidbody.gravityScale < 0.001f)
                {
                    // Check if the body is dynamic.
                    if (this.rigidbody.bodyType == RigidbodyType2D.Dynamic)
                    {
                        if(this.rigidbody.velocity != Vector2.zero)
                        {
                            this.rigidbody.velocity = Vector2.zero;
                        }
                        // Cap the movement speed of the element.
                        this.rigidbody.velocity = this._lastInput * this.speed;

                        // Update the closest elements.
                        this._UpdateClosestGrips(this.rigidbody.position + this.rigidbody.velocity * Time.fixedDeltaTime);
                        /*this._joints[0].enabled = this.closestGrips.Item1 != null;
                        this._joints[0].connectedBody = this.closestGrips.Item1?.GetComponent<Rigidbody2D>();
                        this._joints[1].enabled = this.closestGrips.Item2 != null;
                        this._joints[1].connectedBody = this.closestGrips.Item2?.GetComponent<Rigidbody2D>();
                        this._joints[2].enabled = this.closestGrips.Item3 != null;
                        this._joints[2].connectedBody = this.closestGrips.Item3?.GetComponent<Rigidbody2D>();
                        this._joints[3].enabled = this.closestGrips.Item4 != null;
                        this._joints[3].connectedBody = this.closestGrips.Item4?.GetComponent<Rigidbody2D>();*/

                        // If there is no grip nearby stop any movement.
                        if (this.closestGrips.Item1 == null){ this.rigidbody.velocity = Vector2.zero; this.rigidbody.bodyType = RigidbodyType2D.Static;  }
                    }
                }
                else
                {
                    this._UpdateClosestGrips(this.rigidbody.position + this.rigidbody.velocity * Time.fixedDeltaTime);
                }

                this.playerIkHandler.Grip(this.closestGrips);
            }

            /// <summary> Draws the gizmos of the component. </summary>
            public void OnDrawGizmos() {
                // Check if the rigidbody was loaded.
                if (this.rigidbody != null) {
                    // Show the direction of the player.
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(this.transform.position, this.rigidbody.velocity);
                }

                // Draw a line to all the closest grips.

                Gizmos.color = Color.magenta;
                if (this.closestGrips.Item1 != null) Gizmos.DrawLine(this.transform.position, this.closestGrips.Item1.transform.position);
                if (this.closestGrips.Item2 != null) Gizmos.DrawLine(this.transform.position, this.closestGrips.Item2.transform.position);
                if (this.closestGrips.Item3 != null) Gizmos.DrawLine(this.transform.position, this.closestGrips.Item3.transform.position);
                if (this.closestGrips.Item4 != null) Gizmos.DrawLine(this.transform.position, this.closestGrips.Item4.transform.position);
            }
        
        // -- Private Methods --
            private void _UpdateClosestGrips(Vector2 position) {
                // Clear the closest tuple.
                this.closestGrips = new System.Tuple<Grip, Grip, Grip, Grip>(null, null, null, null);

                // Get the current chunk location.
                System.Tuple<int, int> chunk = new System.Tuple<int, int>(
                    Mathf.FloorToInt(position.x / this.parser.chunkSize), 
                    Mathf.FloorToInt(position.y / this.parser.chunkSize)
                );

                // Loop through the nearest chunks.
                for (int i = chunk.Item1 - 1; i <= chunk.Item1 + 1; i++)
                for (int j = chunk.Item2 - 1; j <= chunk.Item2 + 1; j++) {
                    System.Tuple<int, int> chunkKey = new System.Tuple<int, int>(i, j);
                    // Check if the chunk exists.
                    if (!this.parser.gripChunks.ContainsKey(chunkKey)) { continue; }
                    // Get the chunk's grips.
                    IEnumerable<Grip> chunkGrips = this.parser.gripChunks[chunkKey];

                    // Loop through the grips.
                    foreach (Grip grip in chunkGrips) {
                        // Get the distance to the grip.
                        float distance = Vector2.Distance(grip.transform.position, position);
                        // If the grip is unreachable, ignore it.
                        if (distance > this.reach) { continue; }

                        // Check if the grips is closest.
                        if (this.closestGrips.Item1 == null || distance < Vector2.Distance(this.closestGrips.Item1.transform.position, position)) {
                            // Update the grips.
                            this.closestGrips = new System.Tuple<Grip, Grip, Grip, Grip>(grip, this.closestGrips.Item1, this.closestGrips.Item2, this.closestGrips.Item3);
                        // Check if the grips is the second closest.
                        } else if (this.closestGrips.Item2 == null || distance < Vector2.Distance(this.closestGrips.Item2.transform.position, position)) {
                            // Update the grips.
                            this.closestGrips = new System.Tuple<Grip, Grip, Grip, Grip>(this.closestGrips.Item1, grip, this.closestGrips.Item2, this.closestGrips.Item3);
                        // Check if the grips is the third closest.
                        } else if (this.closestGrips.Item3 == null || distance < Vector2.Distance(this.closestGrips.Item3.transform.position, position)) {
                            // Update the grips.
                            this.closestGrips = new System.Tuple<Grip, Grip, Grip, Grip>(this.closestGrips.Item1, this.closestGrips.Item2, grip, this.closestGrips.Item3);
                        // Check if the grips is the fourth closest.
                        } else if (this.closestGrips.Item4 == null || distance < Vector2.Distance(this.closestGrips.Item4.transform.position, position)) {
                            // Update the grips.
                            this.closestGrips = new System.Tuple<Grip, Grip, Grip, Grip>(this.closestGrips.Item1, this.closestGrips.Item2, this.closestGrips.Item3, grip);
                        }
                    }
                }
            }
    // --- /Methods ---
}
