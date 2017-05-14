using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//[ExecuteInEditMode]
public class SpriteShadow : MonoBehaviour
{
    public float shadow_radius = 5;
    public float surface_offset = 0.1f; 

    public GameObject shadow_prefab;
    private GameObject current_shadow;
    private MeshRenderer parent_mesh;
    private Collider parent_collider;
    Vector3 ray_offset = new Vector3(0, 0.5f, 0);

    void Start()
    {
        parent_collider = GetComponent<Collider>();
        parent_mesh = GetComponent<MeshRenderer>();
        CreateShadow();
        current_shadow.SetActive(false);
        UpdateShadowScale();
    }


    void Update()
    {
        CreateShadow();
        UpdateShadowScale();
        CastShadow();
    }


    void CastShadow()
    {
        if (parent_mesh != null)
        {
            if (parent_mesh.enabled)
            {
                RayCastFloor();
            }
            else if (current_shadow != null)
            {
                current_shadow.SetActive(false);
            }
        }
        else
        {
            RayCastFloor();
        }
    }


    void RayCastFloor()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position + ray_offset, -Vector3.up);

        foreach (RaycastHit hit in hits)
        {
            if (current_shadow && hit.collider != parent_collider)
            {
                current_shadow.SetActive(true);
                current_shadow.transform.position = new Vector3(transform.position.x, hit.point.y + surface_offset, transform.position.z);
                return;
            }
        }
    }


    void UpdateShadowScale()
    {
        if (current_shadow != null)
        {
            if (current_shadow.transform.localScale.x != shadow_radius)
                current_shadow.transform.localScale = new Vector3(shadow_radius, shadow_radius, 0);
        }
    }


    void CreateShadow()
    {
        if (current_shadow == null)
        {
            current_shadow = Instantiate(shadow_prefab);
            current_shadow.transform.parent = transform;
        }
    }

}