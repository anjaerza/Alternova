using UnityEngine;
using DG.Tweening;
using TMPro;

public class BlockController : MonoBehaviour
{
    public Block block;
    private MemoryGame memoryGame;
    [SerializeField] Color hideColor;
    [SerializeField] Color revealColor;
    [SerializeField] Color solveColor;
    [SerializeField] TextMeshProUGUI displayNum;

    bool interactable = true;


    ParticleSystem ps;


    void Start()
    {
        memoryGame = MemoryGame.Instance;
        ps = this.GetComponentInChildren<ParticleSystem>();
        displayNum.text = block.number.ToString();
        interactable = true;
    }

    public void Setup(Block blockData)
    {
        block = blockData;
        // Configurar el bloque visualmente aquí
    }

    public void Reveal()
    {
 
        if (interactable)
        {
            GetComponent<Renderer>().material.color = revealColor;
            transform.DORotate(Vector3.zero, 1f, RotateMode.Fast);
        }

    }

    public void Hide()
    {
       
        if (interactable)
        {
            GetComponent<Renderer>().material.color = hideColor;
            transform.DORotate(new Vector3(0, 180, 0), 1f, RotateMode.Fast);
        }
    }

    public void Solve()
    {
        if (interactable) 
        {
            GetComponent<Renderer>().material.color = solveColor;
            transform.DORotate(new Vector3(0, 180, 0), 1f, RotateMode.Fast);

            ps.Play();
        }

        ps.Play();
        interactable = false;
    }


    public void BlockSelected()
    {
        memoryGame.OnBlockSelected(this);
    }
}
