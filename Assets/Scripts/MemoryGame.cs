using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MemoryGame : MonoBehaviour
{
    public static MemoryGame Instance { get; private set; }

    [Header("Prefabs and UI Elements")]
    public GameObject blockPrefab;
    public Transform gridParent;
    public TMP_InputField playerNameInput;


    [Header("Game Settings")]
    private const string BlocksDataPath = "blocks";
    private const string ResultFilePath = "/result.json";
    private int rows;
    private int cols;
    private List<Block> blocksList;
    private GameObject[,] grid;
    private BlockController firstSelectedBlock;
    private BlockController secondSelectedBlock;

    private bool canPlay;

    private float startTime;
    private int clicks;
    private int pairsFound;
    private int totalPairs;

    private Result result;

    public event Action<BlockController> OnBlockSelectedEvent;
    public event Action<BlockController, BlockController> OnPairMatchedEvent;
    public event Action OnGameEndEvent;
    public event Action OnGameStartEvent;

    private LeaderboardUI leaderboardUI;

    public Result Result { get => result; private set => result = value; }
    public int PairsFound { get => pairsFound; private set => pairsFound = value; }
    public int Clicks { get => clicks; private set => clicks = value; }
    public float StartTime { get => startTime; private set => startTime = value; }
    public int TotalPairs { get => totalPairs; private set => totalPairs = value; }
    public GameObject[,] Grid { get => grid; private set => grid = value; }
    public int Rows { get => rows; private set => rows = value; }
    public int Cols { get => cols; private set => cols = value; }
    public bool CanPlay { get => canPlay; set => canPlay = value; }
    public BlockController FirstSelectedBlock { get => firstSelectedBlock; set => firstSelectedBlock = value; }
    public BlockController SecondSelectedBlock { get => secondSelectedBlock; set => secondSelectedBlock = value; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        LoadLeaderboardUI();
    }

    public void StartGame()
    {
        OnGameStartEvent?.Invoke();
        LoadBlocksData();
        CalculateGridDimensions();
        InitializeGrid();
        StartTime = Time.time;
        InitializeResult();
    }

    public void RestartGame()
    {
        CleanupGrid();
        ResetGameVariables();
        //StartGame();
    }

    private void LoadBlocksData()
    {
        TextAsset jsonText = Resources.Load<TextAsset>(BlocksDataPath);
        BlockData blockData = JsonUtility.FromJson<BlockData>(jsonText.text);
        blocksList = new List<Block>(blockData.blocks);
        TotalPairs = blocksList.Count / 2;
    }

    private void LoadLeaderboardUI()
    {
        leaderboardUI = FindObjectOfType<LeaderboardUI>();
    }

    private void CalculateGridDimensions()
    {
        int maxRow = 0;
        int maxCol = 0;

        foreach (var block in blocksList)
        {
            if (block.R > maxRow) maxRow = block.R;
            if (block.C > maxCol) maxCol = block.C;
        }

        Rows = maxRow;
        Cols = maxCol;
    }

    private void InitializeResult()
    {
        Result = new Result
        {
            totalPlayTime = 0,
            totalClicks = 0,
            totalPairs = 0,
            score = 0
        };
    }

    private void InitializeGrid()
    {
        Grid = new GameObject[Rows, Cols];
        foreach (var block in blocksList)
        {
            GameObject blockObj = Instantiate(blockPrefab, gridParent);
            blockObj.GetComponent<BlockController>().Setup(block);
            Grid[block.R - 1, block.C - 1] = blockObj;
        }

        var gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        if (gridLayoutGroup != null)
        {
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = Cols;
        }
    }

    private void CleanupGrid()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void ResetGameVariables()
    {
        clicks = 0;
        pairsFound = 0;
        FirstSelectedBlock = null;
        SecondSelectedBlock = null;
        playerNameInput.text = "";
    }

    public void OnBlockSelected(BlockController selectedBlock)
    {
        Clicks++;
        SoundManager.Instance.PlayClickBlock();
        if (FirstSelectedBlock == null)
        {
            FirstSelectedBlock = selectedBlock;
            selectedBlock.Reveal();
            OnBlockSelectedEvent?.Invoke(selectedBlock);
        }
        else if (SecondSelectedBlock == null)
        {
            SecondSelectedBlock = selectedBlock;
            selectedBlock.Reveal();
            OnBlockSelectedEvent?.Invoke(selectedBlock);

            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1);

        if ((FirstSelectedBlock.block.number == SecondSelectedBlock.block.number) && FirstSelectedBlock.block != SecondSelectedBlock.block)
        {
            PairsFound++;
            OnPairMatchedEvent?.Invoke(FirstSelectedBlock, SecondSelectedBlock);
            FirstSelectedBlock.Solve();
            SoundManager.Instance.PlayPair();
            SecondSelectedBlock.Solve();
            SoundManager.Instance.PlayPair();

        }
        else
        {
            FirstSelectedBlock.Hide();
            SoundManager.Instance.PlayMiss();
            SecondSelectedBlock.Hide();
            SoundManager.Instance.PlayMiss();
        }

        FirstSelectedBlock = null;
        SecondSelectedBlock = null;

        if (PairsFound == TotalPairs)
        {
            EndGame();
            SoundManager.Instance.PlayVictory();
        }
    }

    private void EndGame()
    {
        Result.totalPlayTime = Time.time - StartTime;
        Result.score = CalculateScore(Result.totalPlayTime, Clicks, PairsFound);
        Result.totalPairs = PairsFound;
        Result.totalClicks = Clicks;

        SaveResult(Result);

        if (leaderboardUI != null)
        {
            leaderboardUI.AddNewEntry(playerNameInput.text, Result.score);
        }

        OnGameEndEvent?.Invoke();
    }

    public int CalculateScore(float time, int clicks, int pairs)
    {
        return (int)(pairs * 1000 / (time + clicks));
    }

    public void SaveResult(Result result)
    {
        string jsonResult = JsonUtility.ToJson(result);
        File.WriteAllText(Application.persistentDataPath + ResultFilePath, jsonResult);
    }
}

[Serializable]
public class Result
{
    public float totalPlayTime;
    public int totalClicks;
    public int totalPairs;
    public int score;
}
