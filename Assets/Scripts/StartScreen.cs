using UnityEngine;

public class StartScreen : MonoBehaviour
{
    void Awake()
    {
        transform.SetAsLastSibling();
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
    }
}
