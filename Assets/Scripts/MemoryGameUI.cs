using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MemoryGameUI : MonoBehaviour
{
    public MemoryGame memoryGame;
    public TextMeshProUGUI totalClicksText;
    public TextMeshProUGUI pairsFoundText;
    public TextMeshProUGUI totalTimeText;
    public TextMeshProUGUI scoreText;

    [SerializeField] GameObject endGameUI;
    float endGameUIstartPosY;
    [SerializeField] GameObject startGameUI;
    float startGameUIstartPosY;

    private void Start()
    {
        endGameUIstartPosY = endGameUI.transform.position.y;
        startGameUIstartPosY = startGameUI.transform.position.y;

        
    }

    private void OnEnable()
    {
        memoryGame.OnGameStartEvent += HideEndGameUI;
        memoryGame.OnBlockSelectedEvent += UpdateBlockUI;
        memoryGame.OnPairMatchedEvent += UpdatePairUI;
        memoryGame.OnGameEndEvent += ShowEndGameUI;
    }

    private void OnDisable()
    {
        memoryGame.OnGameStartEvent -= HideEndGameUI;
        memoryGame.OnBlockSelectedEvent -= UpdateBlockUI;
        memoryGame.OnPairMatchedEvent -= UpdatePairUI;
        memoryGame.OnGameEndEvent -= ShowEndGameUI;
    }

    private void UpdateBlockUI(BlockController block)
    {
        // Actualizar la interfaz de usuario en respuesta a la selección de un bloque
    }

    private void UpdatePairUI(BlockController firstBlock, BlockController secondBlock)
    {
        pairsFoundText.text = $"Pares: {memoryGame.PairsFound}";
    }

    private void HideEndGameUI()
    {
        EndOut();
    }

    private void ShowEndGameUI()
    {


        totalClicksText.text = $"Clicks: {memoryGame.Result.totalClicks}";
        totalTimeText.text = $"Tiempo : {memoryGame.Result.totalPlayTime:F2}s";
        scoreText.text = $"Puntaje: {memoryGame.Result.score}";
        // Mostrar resultados y otras interacciones de fin de juego
        EndIn();


    }

    public void EndIn()
    {
        endGameUI.transform.DOMoveY(endGameUIstartPosY, 0.5f, false);
    }

    public void EndOut()
    {
        endGameUI.transform.DOMoveY(2000, 0.5f, false);
    }

    public void StartIn()
    {
        startGameUI.transform.DOMoveY(startGameUIstartPosY, 0.5f, false);
    }

    public void StartOut()
    {
        startGameUI.transform.DOMoveY(-1200, 0.5f, false);
    }

}
