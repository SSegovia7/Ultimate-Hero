using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    private bool _isMovingRight;
    // Start is called before the first frame update
    void Start()
    {
        _isMovingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.x <= 8 && _isMovingRight)
        {
            this.transform.position += Vector3.right * 15f * Time.deltaTime;

        }
        else if(this.transform.position.x >= -8)
        {
            this.transform.position += Vector3.left * 15f * Time.deltaTime;
            _isMovingRight = false;
        }
        else
            _isMovingRight = true;
    }
}
