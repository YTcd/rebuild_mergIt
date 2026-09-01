using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Sprite baseSprite;
    [SerializeField]
    private GameObject prefab;
    public int row = 5;
    public int column = 5;
    private int gridScale = 3;

    private GameObject[] grid; 

    void Awake()
    {
        grid = new GameObject[row * column];
        string message = "Game Manager is Awake";
        Debug.Log(message);
    }

    void Start()
    {
        int totalCells = row * gridScale;
        for (int i = 0; i < row * column; i++)
        {
            int x = i % row * gridScale - totalCells / 2;
            int y = i / row * gridScale - totalCells / 2;
            GameObject obj = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.localScale = new Vector3(gridScale, gridScale, 1);
            obj.GetComponent<SpriteRenderer>().sprite = baseSprite;
            grid[i] = obj;
        }
    }
}
