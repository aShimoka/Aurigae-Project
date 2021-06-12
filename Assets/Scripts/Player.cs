// using UnityEngine;
// using UnityEngine.InputSystem;

// public class Player: MonoBehaviour {
//     public Rigidbody2D rigidbody;
//     public float speed;
//     public Vector2 lastInput;

//     public bool falling;

//     public void Awake() {
//         rigidbody = GetComponent<Rigidbody2D>();
//     }
//     public void OnMove(InputValue input) { 
//         lastInput = input.Get<Vector2>(); 
//         if (!falling && lastInput.magnitude < 0.0001f) { rigidbody.bodyType = RigidbodyType2D.Kinematic; }
//         else {  rigidbody.bodyType = RigidbodyType2D.Dynamic;  }
//     }

    public void OnDrop(){
        falling = !falling;
        if(falling){
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            rigidbody.gravityScale = 1;
        }
        else{
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            rigidbody.gravityScale = 0;
        }
    }

    public void OnBreak()
    {
        Rope.RopeComponent.onBreak.Invoke();
    }
    public void FixedUpdate() {
        if(rigidbody.gravityScale == 0){
            rigidbody.velocity = lastInput * speed * Time.fixedDeltaTime;
        }
    }
}