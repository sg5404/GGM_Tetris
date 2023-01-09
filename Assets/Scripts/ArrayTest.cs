using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayTest : MonoBehaviour
{
    //컬렉션 - 제네릭 or 넌제네릭
    //넌제네릭 : int, float string
    //제네릭 : List<>, Dictionary

    int[,] array2D = new int[,]
    {
        {1,2,3},
        {4,5,6},
        {7,8,9}
    };

    string[,,] array3D = new string[,,]
    {
        {
            {"000", "001", "002" },
            {"010", "011", "012" },
            {"020", "021", "022" },
        },
        {
            {"100", "101", "102" },
            {"110", "111", "112" },
            {"120", "121", "122" },
        }
    };

    int[,,] tetris = new int[,,]
    {
        {
            {0,1,0,0},
            {0,1,0,0},
            {0,2,0,0},
            {0,1,0,0},
        },
        {
            {0,0,0,0},
            {1,1,0,0},
            {0,2,1,0},
            {0,0,0,0},
        }
    };

    private void Start()
    {
        //DebugFor2D();
        //Debug.Log("바뀜");
        //DebugForeach2D();

        Debug.Log(array3D[1, 1, 1]);

        //해당 차원의 배열의 최대값
        int amountDimension = array3D.Rank;
        Debug.Log(string.Format("이 배열의 차원은 {0}", amountDimension));
    }

    private void DebugFor2D()
    {
        for (int i = 0; i < array2D.GetLength(0); i++)
        {
            for (int j = 0; j < array2D.GetLength(1); j++)
            {
                if(i + j == 2) Debug.Log(array2D[i, j]);
            }
        }
    }

    private void DebugForeach2D()
    {
        foreach(var a in array2D)
        {
            Debug.Log(a);
        }
    }
}