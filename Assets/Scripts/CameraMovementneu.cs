using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementneu : MonoBehaviour
{
    public List<Transform> targets;

    public Vector3 offset;
    
    private Vector3 velocity;
    
    private float minZoom = 60f;
    private float maxZoom = 10f;
    private float zoomLimit = 10f;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if(targets.Count == 0)
        {
            return;
        }
        Vector3 centerPoint = GetCenterPoint();
            
        Vector3 newPosition = centerPoint + offset;
            
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, 0.5f);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimit);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }
        
    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }
    
}
