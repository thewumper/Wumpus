using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


/// <summary>
/// Batmover sounds like a really uncool
/// weapon that batman might use
/// </summary>
public class BatMover : MonoBehaviour
{
    [SerializeField] private float travelSpeed;
    [SerializeField] private GameObject room;
    [SerializeField] private float targetGoodEnoughDistance;

    private Vector3 target;


    // Start is called before the first frame update
    void Start()
    {
        // Make less audio sources
        if (Random.Range(0,30) == 10)
        {
            gameObject.GetComponent<AudioSource>().enabled = true;
        }
        SetNewTarget();
    }

    private void FixedUpdate()
    {
        transform.LookAt(target,Vector3.down);
        transform.Translate(Vector3.forward * (Time.fixedDeltaTime * travelSpeed));

        if (Vector3.Distance(transform.position,target)<targetGoodEnoughDistance)
        {
            SetNewTarget();
        }
    }

    public void SetNewTarget()
    {
        // Pretty sure this includes points outside the room
        // But there's a lot of bats, so it doesn't really matter


        var renderers = room.GetComponentsInChildren<Renderer>();
        var bounds = renderers[0].bounds;
        for (var i = 1; i < renderers.Length; ++i)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        target = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y),
                    Random.Range(bounds.min.z, bounds.max.z)
                );
    }
}
