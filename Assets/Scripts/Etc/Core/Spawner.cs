using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // 각 블록의 모양들을 배열로 가지고 있으며
    // 생성가능하다
    // 랜덤으로 블록을 생성 가능해야 한다.

    public Shape[] shapes;

    public Shape SpawnShape()
    {
        Shape shape = null;
        shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
        if(shape)
        {
            return shape;
        }
        else
        {
            Debug.LogWarning("shape is null");
            return null;
        }
    }

    private Shape GetRandomShape()
    {
        int randVal = Random.Range(0, shapes.Length);
        if(shapes[randVal])
        {
            return shapes[randVal];
        }
        else
        {
            Debug.LogWarning("shapes[randVal] is null");
            return null;
        }

    }
}
