using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySound : MonoBehaviour
{
    private float timer;

    private void Awake() {
        timer = 2f;
    }

    private void Update() {
        if (timer < 0) {
            Destroy(gameObject);
        } else {
            timer -= Time.deltaTime;
        }
    }

    public void SetDestroyTimer(float destroyTimer) {
        timer = destroyTimer;
    }
}
