using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Write a method so that the GameObject 
/// moves with constant speed towards a target
/// stops when the distance is 25 units
/// </summary>

public class TestScript : MonoBehaviour
{
    [SerializeField] private Vector3 target;
    [SerializeField] private float speed = 4f;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float distance = Vector3.Distance(target, transform.position);

        if (distance <= 25)
        {
            Vector3 direction = target - transform.position;

            //normailze it to move unit distance
            direction.Normalize();

            //delta time for make it independent of the frame rate
            transform.position = direction * speed * Time.deltaTime;
        }
    }
}
