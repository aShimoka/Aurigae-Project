using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
/// <summary>
/// Inspector class used to edit the <see cref="DebugGripGenerator"/> component.
/// </summary>
[CustomEditor(typeof(DebugGripGenerator))]
public class DebugGripInspector: Editor {
    // ---  Methods ---
        // -- Unity Events --
            /// <summary> Draws the inspector GUI. </summary>
            public override void OnInspectorGUI() {
                // Draw the default inspector.
                DrawDefaultInspector();

                // Draw a button to generate the grips.
                if (GUILayout.Button("Generate")) { ((DebugGripGenerator)this.target).Generate(); }
            }
    // --- /Methods ---
}
#endif

/// <summary>
/// Debug component used to generate a random grid of grip elements.
/// </summary>
public class DebugGripGenerator: MonoBehaviour {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary> Generation area rect. </summary>
            [Tooltip("Area in which the grips will be generated.")]
            public Vector2 area;

            /// <summary> Game object that will be instantiated. </summary>
            [Tooltip("Prefab of the grip object to generate.")]
            public GameObject grip;

            /// <summary> Number of grips to generate. </summary>
            [Tooltip("Number of grips to generate.")]
            public int count;
        
        // -- Private Attributes --
            /// <summary> List of all the generated grips. </summary>
            private GameObject[] _generated = new GameObject[] {};
    // --- /Attributes ---

    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Called when the component is instantiated in the scene.
            /// </summary>
            [ExecuteInEditMode]
            public void Awake() {
                // Generate the grips.
                this.Generate();
            }

            /// <summary> Draws the gizmos of the component. </summary>
            public void OnDrawGizmos() {
                // Show the generation rect.
                Gizmos.color = Color.white; float z = this.transform.position.z;
                Vector3 topLeft     = new Vector3 { x = this.transform.position.x - this.area.x / 2f, y = this.transform.position.y - this.area.y / 2f, z = z };
                Vector3 topRight    = new Vector3 { x = this.transform.position.x + this.area.x / 2f, y = this.transform.position.y - this.area.y / 2f, z = z };
                Vector3 bottomLeft  = new Vector3 { x = this.transform.position.x - this.area.x / 2f, y = this.transform.position.y + this.area.y / 2f, z = z };
                Vector3 bottomRight = new Vector3 { x = this.transform.position.x + this.area.x / 2f, y = this.transform.position.y + this.area.y / 2f, z = z };
                Gizmos.DrawLine(topLeft, topRight); Gizmos.DrawLine(topRight, bottomRight);
                Gizmos.DrawLine(bottomRight, bottomLeft); Gizmos.DrawLine(bottomLeft, topLeft);
            }
        
        // -- Public Methods --
            /// <summary> Generates the list of grips. </summary>
            public void Generate() {
                // Clear the existing items.
                foreach(GameObject obj in this._generated) { Object.DestroyImmediate(obj); }

                // Generate all the grips. 
                this._generated = new GameObject[this.count];
                Vector3 reference = this.transform.position;
                for (int i = 0; i < this.count; i++) { 
                    // Generate a random position in the area.
                    Vector3 position = new Vector3 { 
                        x = Random.Range(-this.area.x / 2, this.area.x / 2) + reference.x,
                        y = Random.Range(-this.area.y / 2, this.area.y / 2) + reference.y,
                        z = reference.z
                    };
                    // Instantiate the object.
                    GameObject generated = Object.Instantiate(this.grip, position, Quaternion.identity, this.transform);
                    generated.name = "Grip #" + i;
                    this._generated[i] = generated;
                }
            }
    // --- /Methods ---
}
