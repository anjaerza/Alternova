using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Block
{
    public int R; // Row
    public int C; // Column
    public int number; // Valor
}

[Serializable]
public class BlockData
{
    public List<Block> blocks;
}