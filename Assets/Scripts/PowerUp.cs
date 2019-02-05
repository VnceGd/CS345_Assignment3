using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameManager gameManager;

    // Start is called before the first frame update
    public void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // Frighten ghosts if eaten
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameManager.Frighten();
            Destroy(gameObject);
        }
    }
}
