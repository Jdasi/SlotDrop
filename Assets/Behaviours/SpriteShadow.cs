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
    Vector3 ray_offset = new Vector3(0, 1.0f, 0);

    private Quaternion initial_shadow_rotation;

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


    void LateUpdate()
    {
        current_shadow.transform.rotation = initial_shadow_rotation;
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

                Vector3 shadow_pos = new Vector3(transform.position.x, hit.point.y + surface_offset, transform.position.z);
                shadow_pos.y = Mathf.Clamp(shadow_pos.y, 0.1f, Mathf.Infinity);

                current_shadow.transform.position = shadow_pos;
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

            initial_shadow_rotation = current_shadow.transform.rotation;
        }
    }

}