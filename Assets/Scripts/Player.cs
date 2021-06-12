using UnityEngine;
using UnityEngine.InputSystem;

public class Player: MonoBehaviour {
    public Rigidbody2D rigidbody;
    public float speed;
    public Vector2 lastInput;

    public void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    public void OnMove(InputValue input) { 
        lastInput = input.Get<Vector2>(); 
        if (lastInput.magnitude < 0.0001f) { rigidbody.bodyType = RigidbodyType2D.Kinematic; }
        else {  rigidbody.bodyType = RigidbodyType2D.Dynamic;  }
    }
    public void FixedUpdate() {
        rigidbody.velocity = lastInput * speed * Time.fixedDeltaTime;

    }
}