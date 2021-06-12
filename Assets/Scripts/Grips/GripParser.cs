using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
/// <summary>
/// Inspector class used to edit the <see cref="GripParser"/> component.
/// </summary>
[CustomEditor(typeof(GripParser))]
public class GripParserInspector: Editor {
    // ---  Methods ---
        // -- Unity Events --
            /// <summary> Draws the inspector GUI. </summary>
            public override void OnInspectorGUI() {
                // Draw the default inspector.
                DrawDefaultInspector();

                // Draw a button to generate the grips.
                if (GUILayout.Button("Regenerate")) { ((GripParser)this.target).Generate(); }
            }
    // --- /Methods ---
}
#endif

/// <summary>
/// Component used to detect all the grips of the level and generated a chunkified list of elements.
/// Used to optimize the player movement algorithm.
/// </summary>
class GripParser: MonoBehaviour {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary> Size of a given chunk. </summary>
            [System.NonSerialized]
            public float chunkSize = 4;

            // <summary> Dictionary of all the chunks 
            public Dictionary<System.Tuple<int, int>, List<Grip>> gripChunks = new Dictionary<System.Tuple<int, int>, List<Grip>>();
    // --- /Attributes ---

    // ---  Methods ---
        // -- Unity Events --
            /// <summary> Draws the gizmos of the component. </summary>
            public void OnDrawGizmos() {
                // Loop through all the chunks.
                foreach (System.Tuple<int, int> chunk in this.gripChunks.Keys) {
                    // Get the rect of the chunk.
                    Gizmos.color = this.gripChunks[chunk].Count > 0 ? Color.green : Color.red; float z = this.transform.position.z;
                    Vector2 start  = new Vector2 { x =  chunk.Item1   * this.chunkSize, y =  chunk.Item2      * this.chunkSize };
                    Vector2 end = new Vector2 { x = (chunk.Item1 + 1) * this.chunkSize, y = (chunk.Item2 + 1) * this.chunkSize };
                    
                    Vector3 topLeft     = new Vector3 { x = start.x , y = start.y, z = z };
                    Vector3 topRight    = new Vector3 { x = end.x   , y = start.y, z = z };
                    Vector3 bottomLeft  = new Vector3 { x = start.x , y = end.y  , z = z };
                    Vector3 bottomRight = new Vector3 { x = end.x   , y = end.y  , z = z };
                    Gizmos.DrawLine(topLeft, topRight); Gizmos.DrawLine(topRight, bottomRight);
                    Gizmos.DrawLine(bottomRight, bottomLeft); Gizmos.DrawLine(bottomLeft, topLeft);
                }
            }

        // -- Public Methods --
            /// <summary> Generates the chunk dictionary. </summary>
            public void Generate() {
                // Clear the dictionary.
                this.gripChunks = new Dictionary<System.Tuple<int, int>, List<Grip>>();
                // Get the list of all the grips in the level.
                Grip[] grips = Object.FindObjectsOfType<Grip>();

                // Loop through all the grips.
                foreach (Grip grip in grips) {
                    // Get the chunk of the grip.
                    System.Tuple<int, int> chunk = new System.Tuple<int, int>(
                        Mathf.FloorToInt(grip.transform.position.x / this.chunkSize), 
                        Mathf.FloorToInt(grip.transform.position.y / this.chunkSize)
                    );

                    // Check if the chunk is already generated.
                    if (!this.gripChunks.ContainsKey(chunk)) {
                        Debug.Log($"Found a new chunk at {chunk.Item1}:{chunk.Item2}.");
                        // Create a new list of grips.
                        this.gripChunks[chunk] = new List<Grip>();
                    }
                    // Add the grip to the list.
                    this.gripChunks[chunk].Add(grip);
                }
            }
    // --- /Methods ---
}
