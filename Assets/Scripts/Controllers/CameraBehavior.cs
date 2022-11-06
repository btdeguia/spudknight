using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform target;
    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, -10);
    }
}
