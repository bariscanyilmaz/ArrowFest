using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5f;

    void Update()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.forward, Time.deltaTime * _speed);
    }




}
