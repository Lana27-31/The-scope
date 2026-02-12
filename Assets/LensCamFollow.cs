using UnityEngine;

public class LensCamFollow : MonoBehaviour
{
    public Transform lensRoot;
    public Transform head;

    void LateUpdate()
    {
        if (lensRoot == null || head == null)
            return;

        transform.position = lensRoot.position;
        transform.rotation = head.rotation;
    }
}