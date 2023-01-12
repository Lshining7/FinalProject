using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] 
    private Vector2 minMaxSpeed;
    [SerializeField]
    private float maxRotationSpeed;
    [SerializeField]
    private Vector3 rotationSpeed;
    [SerializeField]
    private Vector2 minMaxScale;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        float scale = Random.Range(minMaxScale.x, minMaxScale.y);
        transform.localScale = Vector3.one * scale;
        SetSpeedAndPosition();
        rotationSpeed.x = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        rotationSpeed.y = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        rotationSpeed.z = Random.Range(-maxRotationSpeed, maxRotationSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        float amtToMove = speed * Time.deltaTime;
        transform.Translate(Vector3.down * amtToMove,Space.World);

        if (Camera.main.WorldToViewportPoint(transform.position).y < -0.1f)
        {
            SetSpeedAndPosition();
        }
        Vector3 amtToRotate = Time.deltaTime * rotationSpeed;
        transform.Rotate(amtToRotate);
    }
    public void SetSpeedAndPosition()
    {
        speed = Random.Range(minMaxSpeed.x,minMaxSpeed.y);
        Vector3 newPosition = Camera.main.ViewportToWorldPoint
            (new Vector3(Random.Range(0.05f, 0.95f), 1, 0));
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

    public void increaseSpeed()
    {
        minMaxSpeed.x += 0.5f;
        minMaxSpeed.y += 0.5f;
    }
}
