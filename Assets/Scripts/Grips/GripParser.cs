using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Component used to detect all the grips of the level and generated a chunkified list of elements.
/// Used to optimize the player movement algorithm.
/// </summary>
public class GripParser {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary> Dictionary of all the chunks </sumamry>
            public Dictionary<System.Tuple<int, int>, List<Grip>> gripChunks { get; private set; } = null;

            /// <summary> Chunk size of the current dictionary. </sumamry>
            public float chunkSize { get; private set; }
        // -- Private Attributes --

            /// <summary> Singleton class instance. </sumamry>
            private static GripParser _singleton = null;
    // --- /Attributes ---

    // ---  Methods ---
        // -- Public Methods --
            /// <summary> Generates the chunk dictionary. </summary>
            public void Generate(float chunkSize) {
                // Store the chunk size.
                this.chunkSize = chunkSize;

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
                        // Create a new list of grips.
                        this.gripChunks[chunk] = new List<Grip>();
                    }
                    // Add the grip to the list.
                    this.gripChunks[chunk].Add(grip);
                }
            }

            /// <summary> Deletes the provided chunk from the map. </summary>
            public void Remove(Grip grip) {
                // Get the chunk of the grip.
                System.Tuple<int, int> chunk = new System.Tuple<int, int>(
                    Mathf.FloorToInt(grip.transform.position.x / this.chunkSize), 
                    Mathf.FloorToInt(grip.transform.position.y / this.chunkSize)
                );

                // Check if the chunk is already generated.
                if (this.gripChunks.ContainsKey(chunk)) {
                    // Remove the grip from the dictionary.
                    if (!this.gripChunks[chunk].Contains(grip)) {
                        Debug.LogWarning("Tried to delete a grip that does not exist ?");
                    } else {
                        if (!this.gripChunks[chunk].Remove(grip)) {
                            Debug.LogError("Failed to delete a grip !");
                        }
                    }
                }
            }
        
            /// <summary> Accessor for the singleton instance. </summary>
            public static GripParser GetInstance() {
                // Check if the instance exists.
                if (GripParser._singleton != null) { return GripParser._singleton; }

                // Create a new instance.
                GripParser._singleton = new GripParser{};
                // Return the instance.
                return GripParser._singleton;
            }
    // --- /Methods ---
}
