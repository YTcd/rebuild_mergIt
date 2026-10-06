using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_Text score;
    private int current = 0;

    void Awake()
    {
        score.SetText("Score : 0");
    }

    public void AddScore(int num)
    {
        if (num == 0) return;

        current += num;
        score.SetText("Score : {0}", current);
    }
}
