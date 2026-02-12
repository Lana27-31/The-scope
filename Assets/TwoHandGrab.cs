using UnityEngine;

public class TwoHandGrab : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public Transform leftGrabPoint;
    public Transform rightGrabPoint;

    public float grabDistance = 0.25f;

    bool leftActive, rightActive;
    Vector3 lastLeftPos, lastRightPos;
    Quaternion lastLeftRot, lastRightRot;

    void Start()
    {
        if (leftHand) { lastLeftPos = leftHand.position; lastLeftRot = leftHand.rotation; }
        if (rightHand) { lastRightPos = rightHand.position; lastRightRot = rightHand.rotation; }
    }

    void Update()
    {
        // FAST TEST INPUT:
        // Q = left grab, E = right grab (Editor)
        if (Input.GetKeyDown(KeyCode.Q)) leftActive = CanGrab(leftHand, leftGrabPoint);
        if (Input.GetKeyUp(KeyCode.Q)) leftActive = false;

        if (Input.GetKeyDown(KeyCode.E)) rightActive = CanGrab(rightHand, rightGrabPoint);
        if (Input.GetKeyUp(KeyCode.E)) rightActive = false;

        ApplyMotion();

        if (leftHand) { lastLeftPos = leftHand.position; lastLeftRot = leftHand.rotation; }
        if (rightHand) { lastRightPos = rightHand.position; lastRightRot = rightHand.rotation; }
    }

    bool CanGrab(Transform hand, Transform point)
    {
        if (!hand || !point) return false;
        return Vector3.Distance(hand.position, point.position) <= grabDistance;
    }

    void ApplyMotion()
    {
        if (!leftActive && !rightActive) return;

        // Translation
        Vector3 move = Vector3.zero;
        if (leftActive && leftHand) move += (leftHand.position - lastLeftPos);
        if (rightActive && rightHand) move += (rightHand.position - lastRightPos);
        transform.position += move;

        // Rotation around each hand pivot (quaternion delta)
        if (leftActive && leftHand)
        {
            Quaternion dRot = leftHand.rotation * Quaternion.Inverse(lastLeftRot);
            RotateAround(leftHand.position, dRot);
        }

        if (rightActive && rightHand)
        {
            Quaternion dRot = rightHand.rotation * Quaternion.Inverse(lastRightRot);
            RotateAround(rightHand.position, dRot);
        }
    }

    void RotateAround(Vector3 pivot, Quaternion rot)
    {
        Vector3 dir = transform.position - pivot;
        dir = rot * dir;
        transform.position = pivot + dir;
        transform.rotation = rot * transform.rotation;
    }
}