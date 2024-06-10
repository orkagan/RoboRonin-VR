using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddSliceCollider : MonoBehaviour
{
    [SerializeField] LayerMask slicableLayer = 6;
    
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log($"TestScriptRunning {transform}");
		foreach (Collider col in GetComponentsInChildren<Collider>())
		{
            Destroy(col);
		}
		foreach (MeshRenderer limb in GetComponentsInChildren<MeshRenderer>())
		{
            Debug.Log(limb.gameObject.name);
            MeshCollider mc = limb.gameObject.AddComponent<MeshCollider>();
            mc.convex = true;
            //mc.gameObject.tag = "Sliceable";
            mc.gameObject.layer = slicableLayer;
            mc.tag = "Limb";
		}
    }
}
