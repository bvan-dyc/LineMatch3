using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] protected BlockType type;

    public BlockType BlockType
    {
        get { return type;  }
    }
}
