using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private void Start()
    {
        Destroy(this.gameObject, 2.5f);
    }
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Acceleration);
    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("collision");
        if (collision.gameObject.CompareTag("Player"))
        {
            
            GameObject player = collision.gameObject;
            Vector3 dir = GetComponent<Rigidbody>().velocity.normalized;
            player.GetComponent<Rigidbody>().AddForce(dir*25f, ForceMode.VelocityChange);
        }

        Destroy(this.gameObject, 0.1f);
    }

}
