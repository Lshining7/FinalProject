using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    [SerializeField]
    private float speed;
    public GameObject explosionPrefab;
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        Vector3 postInViewSpace = Camera.main.WorldToViewportPoint(transform.position);
        if (postInViewSpace.y > 1.2f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        Enemy collideWith = other.GetComponent<Enemy>();
        if (collideWith != null)
        {
            collideWith.increaseSpeed();
            collideWith.SetSpeedAndPosition();
            Player.score += 10;
            Debug.Log("Your Points" + Player.score);
            Destroy(this.gameObject);
            Instantiate(explosionPrefab,transform.position,Quaternion.identity);
        }
    }
}
