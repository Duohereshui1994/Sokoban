using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.3f;
    [SerializeField] private float leftLifeTime;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 defaultScale;

    private void Start()
    {
        leftLifeTime = lifeTime;

        defaultScale = transform.localScale;

        float maxVelocity = 5.0f;

        velocity = new Vector3(Random.Range(-maxVelocity, maxVelocity), Random.Range(-maxVelocity, maxVelocity), 0);

    }

    private void Update()
    {
        leftLifeTime -= Time.deltaTime;

        transform.position += velocity * Time.deltaTime;

        transform.localScale = Vector3.Lerp(Vector3.zero, defaultScale, leftLifeTime / lifeTime);

        if (leftLifeTime <= 0)
        {
            Destroy(gameObject);
        }

    }
}