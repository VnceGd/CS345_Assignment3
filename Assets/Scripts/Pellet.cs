using UnityEngine;

public class Pellet : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    public void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // Increase score by 10 if eaten by player
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gameManager.pelletCount--;
            gameManager.IncreaseScore(10);
            Destroy(gameObject);
        }
    }
}
