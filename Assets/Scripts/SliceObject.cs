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
    public float cutForce = 500f;
    public float velocityThresholdToCut = 5f;

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
        if (velocity.magnitude < velocityThresholdToCut) return;
        //Debug.Log(velocity.magnitude);

        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();
        
        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);
        if (hull != null)
		{
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            SetupSlicedObject(upperHull);
            upperHull.layer = slicableLayer;
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
            SetupSlicedObject(lowerHull);
		}
        Destroy(target);
	}

    public void SetupSlicedObject(GameObject slicedObject)
	{
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        //rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
        slicedObject.layer = LayerMask.NameToLayer("Sliceable");
        slicedObject.AddComponent<HotCutCooling>();

    }
}
