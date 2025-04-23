using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [Header("Menu gameobjects")]
    [SerializeField] private GameObject pauseUI;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            pauseUI.SetActive(true);
        }
    }
}
