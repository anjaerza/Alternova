using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MemoryGameTests
{
    private GameObject gameGameObject;
    private MemoryGame memoryGame;
    private GameObject blockPrefab;
    private TMP_InputField playerNameInput;

    [SetUp]
    public void SetUp()
    {
        gameGameObject = new GameObject();
        memoryGame = gameGameObject.AddComponent<MemoryGame>();

        blockPrefab = new GameObject();
        blockPrefab.AddComponent<BlockController>();

        memoryGame.blockPrefab = blockPrefab;
        memoryGame.gridParent = new GameObject().transform;
        playerNameInput = new GameObject().AddComponent<TMP_InputField>();
        memoryGame.playerNameInput = playerNameInput;
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(gameGameObject);
        GameObject.Destroy(blockPrefab);
        GameObject.Destroy(memoryGame.gridParent.gameObject);
        GameObject.Destroy(playerNameInput.gameObject);
    }

    [Test]
    public void TestStartGameInitializesGrid()
    {
        memoryGame.StartGame();
        Assert.NotNull(memoryGame.Grid);
        Assert.Greater(memoryGame.Rows, 0);
        Assert.Greater(memoryGame.Cols, 0);
    }

    [Test]
    public void TestRestartGameResetsVariables()
    {
        memoryGame.StartGame();
        memoryGame.RestartGame();
        Assert.AreEqual(0, memoryGame.Clicks);
        Assert.AreEqual(0, memoryGame.PairsFound);
        Assert.IsNull(memoryGame.FirstSelectedBlock);
        Assert.IsNull(memoryGame.SecondSelectedBlock);
        Assert.IsEmpty(memoryGame.playerNameInput.text);
    }

    [UnityTest]
    public IEnumerator TestOnBlockSelectedIncreasesClicks()
    {
        memoryGame.StartGame();
        var blockController = blockPrefab.GetComponent<BlockController>();
        blockController.block = new Block { number = 1, R = 1, C = 1 };

        memoryGame.OnBlockSelected(blockController);
        yield return null;

        Assert.AreEqual(1, memoryGame.Clicks);
    }

    [UnityTest]
    public IEnumerator TestCheckMatchFindsPair()
    {
        memoryGame.StartGame();
        var blockController1 = blockPrefab.GetComponent<BlockController>();
        var blockController2 = blockPrefab.GetComponent<BlockController>();

        blockController1.block = new Block { number = 1, R = 1, C = 1 };
        blockController2.block = new Block { number = 1, R = 1, C = 2 };

        memoryGame.OnBlockSelected(blockController1);
        yield return null;
        memoryGame.OnBlockSelected(blockController2);
        yield return new WaitForSeconds(1);

        Assert.AreEqual(1, memoryGame.PairsFound);
    }

    [Test]
    public void TestCalculateScore()
    {
        int score = memoryGame.CalculateScore(60, 30, 10);
        Assert.AreEqual(142, score); // score = (10 * 1000) / (60 + 30)
    }

    [Test]
    public void TestSaveResult()
    {
        Result result = new Result { totalPlayTime = 120f, totalClicks = 50, totalPairs = 15, score = 2000 };
        memoryGame.SaveResult(result);

        string filePath = Application.persistentDataPath + "/result.json";
        Assert.IsTrue(File.Exists(filePath));

        string jsonResult = File.ReadAllText(filePath);
        Result loadedResult = JsonUtility.FromJson<Result>(jsonResult);

        Assert.AreEqual(result.totalPlayTime, loadedResult.totalPlayTime);
        Assert.AreEqual(result.totalClicks, loadedResult.totalClicks);
        Assert.AreEqual(result.totalPairs, loadedResult.totalPairs);
        Assert.AreEqual(result.score, loadedResult.score);
    }
}