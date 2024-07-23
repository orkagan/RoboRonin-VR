using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class PlaneSlice : MonoBehaviour
{
    public LayerMask slicableLayer;

    public Material crossSectionMaterial;
    public float cutForce = 500f;

    public Transform planeDebug;
    void Update()
    {
        //debug slice based on debug plane
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnityEngine.Plane cutPlane = new UnityEngine.Plane(planeDebug.up, planeDebug.position);
            foreach (Collider obj in Physics.OverlapBox(planeDebug.position, planeDebug.localScale / 2, Quaternion.identity, slicableLayer))
            {
                Slice(obj.gameObject, cutPlane);
            }
        }
    }

    public void Slice(GameObject target, UnityEngine.Plane cutPlane)
    {
        SlicedHull hull = target.Slice(planeDebug.position, cutPlane.normal);
        GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
        GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
        SetupSlicedObject(upperHull);
        SetupSlicedObject(lowerHull);
        Destroy(target);
    }

    public void SetupSlicedObject(GameObject slicedObject)
    {
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
        slicedObject.layer = LayerMask.NameToLayer("Sliceable");
        slicedObject.AddComponent<HotCutCooling>();

    }
}
