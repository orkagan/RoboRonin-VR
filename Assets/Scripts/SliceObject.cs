using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class SliceObject : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public LayerMask slicableLayer;
    public VelocityEstimator velocityEstimator;

    public Material crossSectionMaterial;
    public float cutForce = 1000;

    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, slicableLayer);
		if (hasHit)
		{
            GameObject target = hit.transform.gameObject;
            Slice(target);
		}
    }

    public void Slice(GameObject target)
	{
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();
        
        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);
        if (hull != null)
		{
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            SetupSlicedObject(upperHull);
            upperHull.layer = target.layer;
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
            SetupSlicedObject(lowerHull);
            lowerHull.layer = target.layer;
		}
        Destroy(target);
	}

    public void SetupSlicedObject(GameObject slicedObject)
	{
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
        //slicedObject.layer = slicableLayer; //idk why this didn't work
	}
}
