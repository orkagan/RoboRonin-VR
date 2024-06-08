using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddSliceCollider : MonoBehaviour
{
    [SerializeField] LayerMask slicableLayer = 6;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"TestScriptRunning {transform}");
		/*foreach (Transform limb in transform)
		{
            Debug.Log($"Limb: {limb}");
            if (limb.gameObject.GetComponent<Collider>() != null)
			{

			}
		}*/
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
