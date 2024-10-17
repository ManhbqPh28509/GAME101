using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float _speed;
    public float lifeTime;
    public GameObject effect_Arrow;
    Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject,lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = -transform.right * _speed * Time.deltaTime;
        
    }
    
}
