using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private void Update()
    {
        float birdX = Bird.GetInstance().transform.position.x;
        transform.position = new Vector3(birdX, 0, transform.position.z);
    }
}
