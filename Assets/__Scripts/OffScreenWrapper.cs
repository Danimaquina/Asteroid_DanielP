using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenWrapper : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (!enabled)
        {
            return;
        }
        
        ScreenBounds bounds = other.GetComponent<ScreenBounds>();
        if (bounds == null)
        {
            return;
        }

        ScreenWrap(bounds);
    }
    
    private void ScreenWrap(ScreenBounds bounds) 
    {
        Vector3 LocalitRelativa = bounds.transform.InverseTransformPoint(transform.position);
        if (Mathf.Abs(LocalitRelativa.x) > 0.5f)
        {
            LocalitRelativa.x *= -1;
            
        }
        if (Mathf.Abs(LocalitRelativa.y) > 0.5f)
        {
            LocalitRelativa.y *= -1;
        }

        transform.position = bounds.transform.TransformPoint(LocalitRelativa);
    }
}