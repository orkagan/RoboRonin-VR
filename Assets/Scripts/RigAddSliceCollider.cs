using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigAddSliceCollider : MonoBehaviour
{
    [SerializeField] LayerMask slicableLayer = 6;
    
    [ContextMenu("Revise Colliders")]
    public void ReviseColliders()
    {
        //Debug.Log($"TestScriptRunning {transform}");
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            DestroyImmediate(collider);
		}
        foreach (MeshRenderer limb in GetComponentsInChildren<MeshRenderer>())
		{
            //Debug.Log(limb.gameObject.name);
            MeshCollider mc = limb.gameObject.AddComponent<MeshCollider>();
            mc.convex = true;
            //mc.gameObject.tag = "Sliceable";
            limb.gameObject.layer = LayerMask.NameToLayer("Sliceable");
            limb.tag = "Limb";
		}
    }
}
