using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_Text score;
    public int Current { get; private set; }

    void Awake()
    {
        score.SetText("Score : 0");
    }

    public void AddScore(int num)
    {
        if (num == 0) return;

        Current += num;
        score.SetText("Score : {0}", Current);
    }
}
