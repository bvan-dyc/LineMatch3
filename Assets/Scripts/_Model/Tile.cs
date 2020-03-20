using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class Tile : MonoBehaviour
{
    [HideInInspector] public HexCoordinates Coordinates { get; set; }
    [HideInInspector] public Point OffsetCoords { get; set; }
    [SerializeField] private float innerCircleRadius = 1f;
    [SerializeField] private float outerCircleRadius = 1f;
    private Animator animator;
    private Block block = null;
    readonly int hashSelect = Animator.StringToHash("Selected");

    public float Height {
        get { return innerCircleRadius; }
    }
    public float Width {
        get { return outerCircleRadius; }
    }
    public bool IsFree {
        get { return block == null; }
    }

    public Block Block
    {
        get { return block;  }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void AddNewBlock(Block newBlock)
    {
        block = newBlock;
        block.transform.position = transform.position;
    }

    public void SelectTile()
    {
        animator.SetBool(hashSelect, true);
    }

    public void UnselectTile()
    {
        animator.SetBool(hashSelect, false);
    }

    public bool TryToMatchWith(Tile tile)
    {
        if (!tile)
            return false;
        if (tile.IsFree || tile.Block.BlockType != block.BlockType)
            return false;
        return IsNeighbourWith(tile.Coordinates);
    }

    public bool IsNeighbourWith(HexCoordinates tileCoords)
    {
        HexCoordinates direction = tileCoords - Coordinates;
        if (direction.y == 1 && direction.x == 0)
            return true;
        if (direction.y == 1 && direction.x == -1)
            return true;
        if (direction.y == 0 && direction.x == -1)
            return true;
        if (direction.y == -1 && direction.x == 0)
            return true;
        if (direction.y == -1 && direction.x == 1)
            return true;
        if (direction.y == 0 && direction.x == 1)
            return true;
        return false;
    }

    public void DestroyBlock()
    {
        if (!IsFree)
        {
            GameObject.Destroy(block.gameObject);
            block = null;
        }
    }
}
