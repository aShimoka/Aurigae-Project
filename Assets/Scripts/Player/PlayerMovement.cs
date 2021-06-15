using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


/// <summary>
/// Component in charge of the movements of the player in the 2D XY plane.
/// TODO: Implement the more complex grip system.
/// </summary>
[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D))]
public class PlayerMovement: MonoBehaviour {
    private class CloseComparer: IComparer<float> {
        public static CloseComparer instance = new CloseComparer();
        public int Compare(float a, float b) { return a > b ? 1 : -1; }
    }

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

            /// <summary> List of all the grips that are closest to the instance. </summary>
            public Grip[] closestGrips { get; private set; } = new Grip[0];

            public Rope.RopeComponent rope;

        // -- Private Attributes --
            /// <summary> Direction of the last input made by the user. </summary>
            private Vector2 _lastInput;

            private Vector2 _lastPosition;

            public System.DateTime? _breakPointBegin;

            private Joint2D[] _joints;

            public Image energyBar;
            private float _energy;

            private System.DateTime? _energyTime;
    // --- /Attributes ---

    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Called when the component is instantiated in the scene.
            /// </summary>
            public void Awake() {
                if(rope != null)
                {
                    Debug.Log("Rope found!");
                    
                }

                // Query the rigidbody component.
                this.rigidbody = this.GetComponent<Rigidbody2D>();
                this.StartCoroutine(this._AfterEndOfFrame());
            }

            private IEnumerator _AfterEndOfFrame() {
                yield return new WaitForEndOfFrame();

                // Generate the grip chunk grid.
                GripParser parser = GripParser.GetInstance();
                if (parser.gripChunks == null) { parser.Generate(this.reach); }
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

            public void OnDrop() {
                if (falling) {
                    if (!(rope.broken && _energy < 0) && closestGrips.Length > 0) {
                        if (!rope.broken)
                        {
                            _energyTime = null;
                        }
                        falling = false;
                        rigidbody.gravityScale = 0;
                        rigidbody.bodyType = RigidbodyType2D.Static;
                        _energy = 0;
                    }
                }
                else {
                    if (!rope.broken)
                    {
                        _energyTime = System.DateTime.Now;
                    }
                    falling = true;
                    rigidbody.bodyType = RigidbodyType2D.Dynamic;
                    rigidbody.gravityScale = 1;
                }
            }

            public void OnBreak()
            {
                rope.onBreak.Invoke();
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
                        if (this.rigidbody.velocity != Vector2.zero)
                        {
                            this.rigidbody.velocity = Vector2.zero;
                        }
                        // Cap the movement speed of the element.
                        this.rigidbody.velocity = this._lastInput * this.speed;

                        // Update the closest elements.
                        this._UpdateClosestGrips(this.rigidbody.position + this.rigidbody.velocity * Time.fixedDeltaTime);

                        // If there is no grip nearby stop any movement.
                        if (this.closestGrips.Length <= 0){
                            this.rigidbody.velocity = Vector2.zero; this.rigidbody.bodyType = RigidbodyType2D.Static;  
                        }
                    }
                }
                else
                {
                    this._UpdateClosestGrips(this.rigidbody.position + this.rigidbody.velocity * Time.fixedDeltaTime);
                }

                this.playerIkHandler.Grip(this.closestGrips);
                _lastPosition = rigidbody.position;
            
                UpdateEnergy();
            }

            public void UpdateEnergy()
            {
                if (_energyTime != null) {
                    var duration = (System.DateTime.Now - _energyTime);
                    _energy = (10000 - (float)duration?.Seconds * 1000 - (float)duration?.Milliseconds)/10000;

                    if (_energy <= 0)
                    {
                        _energyTime = null;
                        if (rope.broken)
                        {
                            OnDrop();
                        }
                        else
                        {
                            rope.BreakRope();
                        }
                    }
                }
                else
                {
                    if (rope.broken)
                    {
                        _energyTime = System.DateTime.Now;
                    }
                }
                
                

                energyBar.fillAmount = _energy;
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
                for (int i = 0; i < this.closestGrips.Length; i++) {
                    Gizmos.color = i > 2 ? Color.black : Color.magenta;
                    if (this.closestGrips[i] != null) Gizmos.DrawLine(this.transform.position, this.closestGrips[i].transform.position);
                }
            }
        
        // -- Private Methods --
            private void _UpdateClosestGrips(Vector2 position) {
                // Ensure that the grip is set.
                Dictionary<System.Tuple<int, int>, List<Grip>> dict = GripParser.GetInstance().gripChunks;
                if (dict == null) { return; }

                // Create a new sorted list.
                SortedList<float, Grip> closest = new SortedList<float, Grip>(CloseComparer.instance);

                // Get the current chunk location.
                System.Tuple<int, int> chunk = new System.Tuple<int, int>(
                    Mathf.FloorToInt(position.x / this.reach), 
                    Mathf.FloorToInt(position.y / this.reach)
                );

                // Loop through the nearest chunks.
                for (int i = chunk.Item1 - 1; i <= chunk.Item1 + 1; i++)
                for (int j = chunk.Item2 - 1; j <= chunk.Item2 + 1; j++) {
                    System.Tuple<int, int> chunkKey = new System.Tuple<int, int>(i, j);
                    // Check if the chunk exists.
                    if (!dict.ContainsKey(chunkKey)) { continue; }
                    // Get the chunk's grips.
                    IEnumerable<Grip> chunkGrips = dict[chunkKey];

                    // Loop through the grips.
                    foreach (Grip grip in chunkGrips) {
                        // If the grip is taken by another player, ignore it.
                        if (grip.isGrabbed && !this.closestGrips.Contains(grip)) { continue; }

                        // Get the distance to the grip.
                        float distance = Vector2.Distance(grip.transform.position, position);
                        // If the grip is unreachable, ignore it.
                        if (distance > this.reach) { continue; }

                        // Add the grip to the list.
                        closest.Add(distance, grip);
                    }
                }

                // Sort the list of grips by their distance.
                Grip[] oldGrips = this.closestGrips.Take(2).ToArray(); List<Grip> released = new List<Grip>(oldGrips);
                this.closestGrips =  closest.Select((KeyValuePair<float, Grip> kv) => kv.Value).ToArray();
                Grip[] newGrips = this.closestGrips.Take(2).ToArray(); List<Grip> grabbed  = new List<Grip>(newGrips);

                // Check which grips have changed.
                foreach (Grip oldGrip in oldGrips) 
                foreach (Grip newGrip in newGrips){
                    if (oldGrip != null && oldGrip == newGrip) { released.Remove(oldGrip); grabbed.Remove(newGrip); }
                }

                // Call the grip and release events.
                foreach (Grip releasedItem in released) { releasedItem.OnRelase(); }
                foreach (Grip grabbedItem  in grabbed)  { grabbedItem.OnGrab(); }
            }
    // --- /Methods ---
}
