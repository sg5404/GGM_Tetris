using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public bool bCanRotate = true;
    public bool CanMove = true;

    private void Start()
    {
        //InvokeRepeating("MoveDown", 0, 0.5f);
        //InvokeRepeating("RotateRight", 0, 1f);
    }

    //이동 함수들
    void Move(Vector3 moveDir)
    {
        transform.position += moveDir;
    }

    public void MoveLeft()
    {
        if(CanMove)
            Move(Vector3.left);
    }

    public void MoveRight()
    {
        if (CanMove)
            Move(Vector3.right);
    }

    public void MoveUp()
    {
        if (CanMove)
            Move(Vector3.up);
    }

    public void MoveDown()
    {
        if (CanMove)
            Move(Vector3.down);
    }

    //회전 함수들
    public void RotateRight()
    {
        if(bCanRotate)
            transform.Rotate(0, 0, -90f);
    }

    public void RotateLeft()
    {
        if(bCanRotate)
            transform.Rotate(0, 0, 90f);
    }
}
