[UnityEngine.RequireComponent(typeof(UnityEngine.SpriteRenderer))]
public class Dissapearing: UnityEngine.MonoBehaviour {
    // ---  Attributes ---
        // -- Private Attributes --
            private UnityEngine.SpriteRenderer _renderer;
            private float _time;
            private float _remaining;
    // --- /Attributes ---

    // ---  Methods ---
        // -- Unity Events --
            public void Awake() { this.enabled = false; this._renderer = this.GetComponent<UnityEngine.SpriteRenderer>(); }

            public void Update() { 
                UnityEngine.Color color = this._renderer.color; 
                color.a = this._remaining / this._time;
                this._renderer.color = color;
                this._remaining -= UnityEngine.Time.deltaTime;
            }
        // -- Public Methods --
            public void Activate(float time) { this._remaining = this._time = time / 1000f; this.enabled = true; }
    // --- /Methods ---
}
