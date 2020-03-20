using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockFactory
{
	public static Block CreateRandomBlock(GameObject[] BlockPrefabs)
	{
		int index = Random.Range(0, BlockPrefabs.Length);
		GameObject newBlock = GameObject.Instantiate(BlockPrefabs[index]);
		return (newBlock.GetComponent<Block>());
	}
}
