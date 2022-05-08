using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{

    void Start()
    {
        
    }


    void Update()
    {
        // we cant use rb because Column dont have rigid body
        transform.position = transform.position - new Vector3(3*Time.deltaTime, 0,0);
    }
}
