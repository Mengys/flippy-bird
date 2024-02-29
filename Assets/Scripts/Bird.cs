using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    private const float SPEED_CHANGE = 40f;
    private const float ANGLE_CHANGE = 480f;


    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;

    private static Bird instance;

    public static Bird GetInstance() {
        return instance;
    }

    private Rigidbody2D birdRigidbody2D;
    private State state;
    private float speed;

    private enum State {
        WaitingToStart,
        Playing,
        Dead,
    }

    private void Awake() {
        instance = this;
        birdRigidbody2D = GetComponent<Rigidbody2D>();
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }

    private void Update() {
        switch (state) {
        default:
        case State.WaitingToStart:
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                state = State.Playing;
                birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                OnStartedPlaying?.Invoke(this, EventArgs.Empty);
                birdRigidbody2D.velocity = new Vector2(0.01f, 0f);
            }
            break;
        case State.Playing:
            
            if (Input.GetAxis("Jump") != 0) {
                Accelerate();
            }
            
            transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, birdRigidbody2D.velocity));
            break;
        case State.Dead:
            break;
        }
    }

    private void Accelerate() {

        Vector2 velocity = birdRigidbody2D.velocity;
        speed = velocity.magnitude + SPEED_CHANGE * Time.deltaTime;
        float angle = Vector2.SignedAngle(Vector2.right, velocity) + ANGLE_CHANGE * Time.deltaTime;

        float velocityX = Mathf.Cos(angle / 180 * Mathf.PI) * speed;
        float velocityY = Mathf.Sin(angle / 180 * Mathf.PI) * speed;
        if (angle < 120f || angle > 235f) {
            birdRigidbody2D.velocity = new Vector2(velocityX, velocityY);
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        state = State.Dead;
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        SoundManager.PlaySound(SoundManager.Sound.Lose);
        OnDied?.Invoke(this, EventArgs.Empty);
    }

    public float GetXPosition() {
        return transform.position.x;
    }

    public float GetSpeed() {
        return speed;
    }
}
