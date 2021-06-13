/// <summary>
/// Simple component used to mark a grip element that will delete itself after being grabbed.
/// </summary>
public class AutoDeleteGrip: Grip {
    // ---  SubObjects ---
        // -- Public Classes --
            /// <summary>
            /// Event used when the grip class will be deleted.
            /// Provides the time delay before the <see cref="UnityEngine.GameObject" /> is destroyed.
            /// </summary>
            [System.Serializable]
            public class OnDestroyEvent: UnityEngine.Events.UnityEvent<float> {}
    // --- /SubObjects ---

    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary> Delay after which the grip object will be deleted. </summary>
            [UnityEngine.Tooltip("Delay (in ms.) after which the game object will be deleted.")]
            public int delay;

            /// <summary> Event fired when the grip is about to be destroyed. </summary>
            public OnDestroyEvent onDestroy = new OnDestroyEvent();
    // --- /Attributes ---

    // ---  Methods ---
        // -- Public Methods --
            // - Overrides -
            /// <inheritDoc cref="Grip.OnRelase" />
            public override void OnRelase() {
                // Call the base method.
                base.OnRelase();

                // Delete the component.
                GripParser.GetInstance().Remove(this);
                // Delete the game object after the provided timeout.
                this.StartCoroutine(this._DestroyDelayed());

                // Invoke the destroy event.
                this.onDestroy.Invoke(this.delay);
            }
        
        // -- Private Methods --
            /// <summary>
            /// Destroys the <see cref="UnityEngine.GameObject" /> that this component is attached to.
            /// Waits for the given <see cref="delay" /> before calling <see cref="UnityEngine.Object.Destroy" /> method.
            /// </summary>
            private System.Collections.IEnumerator _DestroyDelayed() {
                // Wait for the provided delay.
                yield return new UnityEngine.WaitForSeconds(this.delay / 1000f);
                
                // Destroy the game object.
                UnityEngine.Object.Destroy(this.gameObject);
            }
    // --- /Methods ---
}
