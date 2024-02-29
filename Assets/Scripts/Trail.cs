using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour {

    private const float SPEED_FOR_TRAIL = 150f;

    private float timer;
    private bool trailActive;

    private void Awake() {
        DeactivateTrail();
    }

    private void Update() {

        if (Bird.GetInstance().GetSpeed() > SPEED_FOR_TRAIL) {
            ActivateTrail();
        } else { 
            DeactivateTrail(); 
        }

        if (trailActive) {
            timer -= Time.deltaTime;
            if (timer < 0 && gameObject.GetComponent<TrailRenderer>().emitting) {
                gameObject.GetComponent<TrailRenderer>().emitting = false;
                timer = Random.Range(.5f, 2f);
            } else if (timer < 0 && !gameObject.GetComponent<TrailRenderer>().emitting) {

                gameObject.GetComponent<TrailRenderer>().emitting = true;
                timer = Random.Range(.2f, .4f);
            }
        }
    }

    private void ActivateTrail() {
        trailActive = true;
    }

    private void DeactivateTrail() {
        timer = Random.Range(0f, 2f);
        trailActive = false;
        gameObject.GetComponent<TrailRenderer>().emitting = false;
    }

}
