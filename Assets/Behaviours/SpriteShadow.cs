using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    private GameObject shadowGameObject;

    // Use this for initialization
    void Start()
    {
        SpriteRenderer[] spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.gameObject.tag == "Shadow")
            {
                shadowGameObject = spriteRenderer.gameObject;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 freezePosition = shadowGameObject.transform.position;
        freezePosition.y = 0.01f;
        shadowGameObject.transform.position = freezePosition;
    }
}