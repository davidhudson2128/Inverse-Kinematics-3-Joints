using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class MoveGoal : MonoBehaviour

{

    private Vector3 mOffset;
    private float mZCoord;

    void OnMouseDown()

    {

        mZCoord = Camera.main.WorldToScreenPoint(
            gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

    }



    private Vector3 GetMouseAsWorldPoint()

    {
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePos);

    }

    void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + mOffset;
    }

}