using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountPoint
{
    public enum ID
    {
        hat,
        face,
        eyes,
        torso,
        hand
    }

    public GameObject mount_object;
    public Vector3 mount_offset;
}
