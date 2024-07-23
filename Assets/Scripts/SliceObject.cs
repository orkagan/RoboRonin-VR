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
    public float velocityThresholdToCut = 3f;

    void FixedUpdate()
    {
		bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, slicableLayer);
		if (hasHit)
		{
			GameObject target = hit.transform.gameObject;
			Vector3 velocity = velocityEstimator.GetVelocityEstimate();
			if (velocity.magnitude < velocityThresholdToCut) return;
			Debug.Log(velocity.magnitude);
			Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
			planeNormal.Normalize();
			UnityEngine.Plane cutPlane = new UnityEngine.Plane(endSlicePoint.position, planeNormal);
			Slice(target, cutPlane);
		}
	}

    public void Slice(GameObject target, UnityEngine.Plane cutPlane)
	{
        SlicedHull hull = target.Slice(endSlicePoint.position, cutPlane.normal);
        GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
        GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
        SetupSlicedObject(upperHull);
        SetupSlicedObject(lowerHull);
        Destroy(target);
        //Slicing code for limbs
        /*Debug.Log($"SLICING LIMB {target} of {target.transform.parent}");
        Transform boneParent = target.transform.parent;
		if (boneParent != null)
		{
            target.transform.parent = null;
		}
        
        SlicedHull hull = target.Slice(endSlicePoint.position, cutPlane.normal);
        if (hull != null)
		{
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
            SetupSlicedObject(upperHull);
            SetupSlicedObject(lowerHull);

            if (target.tag == "Limb")
            {
                BoneReparent(upperHull, boneParent, cutPlane);
                BoneReparent(lowerHull, boneParent, cutPlane);
            }
            Destroy(target);
        }*/
    }

    public void SetupSlicedObject(GameObject slicedObject)
	{
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        slicedObject.layer = LayerMask.NameToLayer("Sliceable");
        slicedObject.AddComponent<HotCutCooling>();

        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
    }

    public void BoneReparent(GameObject obj, Transform boneParent, UnityEngine.Plane cutPlane)
    {
        //position of center of sliced hull object
        Vector3 objCenter = obj.GetComponent<Renderer>().bounds.center;

        

        //if obj is on same side as parent bone, reattach it to rig bone
        if (cutPlane.SameSide(objCenter, boneParent.position))
        {
            obj.transform.parent = boneParent;
            Debug.Log($"{obj} parented to {boneParent}");
            return;
        }
        //else attach obj to loose bone
		else
		{
            //create a duplicate of the bone for dismembered limb
            GameObject looseLimb = Instantiate(new GameObject("looseLimb"), boneParent.position, boneParent.rotation);
            foreach (Transform child in boneParent)
            {
                if (child.gameObject.GetComponent<MeshFilter>() == null)
                {
                    child.parent = looseLimb.transform;
                }
            }
			obj.transform.parent = looseLimb.transform;
        }


		//parents sliced hull to nearest child bone (may need a different approach)
		/*Transform closestChildBone = null;
		foreach (Transform childBone in boneParent)
		{
			Debug.Log($"In the loop: {childBone}");
			if (cutPlane.SameSide(objCenter, childBone.position))
			{
				if (closestChildBone == null)
				{
					closestChildBone = childBone;
				}
				else if (Vector3.Distance(objCenter, closestChildBone.position) > Vector3.Distance(objCenter, childBone.position))
				{
					closestChildBone = childBone;
				}
			}
		}
		if (closestChildBone != null)
		{
			closestChildBone.parent = null;
			Destroy(closestChildBone.gameObject.GetComponent<Joint>());
			obj.transform.parent = closestChildBone;
			Debug.Log($"{obj} parented to {closestChildBone}");
		}
		else
		{
			Debug.Log($"Nothing happ");
			Rigidbody rb = obj.AddComponent<Rigidbody>();
			rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			//rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
		}*/
	}
}
