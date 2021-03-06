﻿using UnityEngine;
using UnityEngine.AI;

public class GhostBehavior : MonoBehaviour
{
    public Vector3 startPosition;
    private float startSpeed;

    public NavMeshAgent myAgent;
    protected Transform player;

    public Transform[] ghostCornerPath;
    private int cornerIndex;
    public Color ghostColor;
    public ParticleSystem.MainModule ghostParticles;
    public int ghost; // 0 = Blinky, 1 = Pinky, 2 = Clyde, 3 = Inky

    public bool waiting = true;
    public bool frightened;
    public bool killed;

    private float timer;
    private float frightenedTimer;

    private GameManager gameManager;

    // Start is called before the first frame update
    public void Start()
    {
        ghostParticles = GetComponentInChildren<ParticleSystem>().main;
        player = GameObject.FindWithTag("Player").transform;
        myAgent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
        startSpeed = myAgent.speed;

        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (killed)
        {
            if (myAgent.remainingDistance < 0.5f)
            {
                killed = false;
                frightened = false;
                frightenedTimer = 0.0f;
                myAgent.speed = startSpeed;
                ghostParticles.startColor = ghostColor;
                ghostParticles.startSize = 0.5f;
                ChaseMode();
            }
        }
        else if (!waiting)
        {
            if (frightened)
            {
                frightenedTimer += Time.deltaTime;
                if (frightenedTimer > 5.0f)
                {
                    frightened = false;
                    frightenedTimer = 0.0f;
                    myAgent.speed = startSpeed;
                    ghostParticles.startColor = ghostColor;
                }
            }
            else
            {
                timer += Time.deltaTime;
                if (timer < 7.0f) // Scatter for 7 seconds
                {
                    ScatterMode();
                }
                else if (timer < 27.0f) // Chase for 20 seconds
                {
                    ChaseMode();
                }
                else if (timer < 34.0f) // Scatter for 7 seconds
                {
                    ScatterMode();
                }
                else if (timer < 54.0f) // Chase for 20 seconds
                {
                    ChaseMode();
                }
                else if (timer < 59.0f) // Scatter for 5 seconds
                {
                    ScatterMode();
                }
                else if (timer < 79.0f) // Chase for 20 seconds
                {
                    ChaseMode();
                }
                else if (timer < 84.0f) // Scatter for 5 seconds
                {
                    ScatterMode();
                }
                else // Chase permanently
                {
                    ChaseMode();
                }
            }
            if (!myAgent.pathPending && myAgent.remainingDistance < 0.5f)
            {
                Loop();
            }
        }
    }

    // Move ghost to its corner
    public void ScatterMode()
    {
        myAgent.destination = ghostCornerPath[cornerIndex].position;
    }

    // Ghost runs in a loop during Scatter Mode
    public void Loop()
    {
        myAgent.destination = ghostCornerPath[cornerIndex].position;
        cornerIndex = (cornerIndex + 1) % ghostCornerPath.Length;
    }

    // Ghost targets player
    public virtual void ChaseMode()
    {
        switch (ghost)
        {
            case 0:
                myAgent.destination = player.transform.position;
                break;
            case 1:
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToPlayer < 4.0f)
                {
                    ScatterMode();
                }
                else
                {
                    myAgent.destination = player.transform.position;
                }

                break;
            case 2:
                myAgent.destination = player.transform.position + (player.transform.forward * 2.0f);
                break;
            case 3:
                myAgent.destination = player.transform.position + (player.transform.forward * 0.5f);
                break;
        }
    }

    // Ghost body fades and ghost moves to starting position
    public void Kill()
    {
        killed = true;
        myAgent.destination = startPosition;
        ghostParticles.startSize = 0.0f;
        myAgent.speed = 2.0f;
    }

    // Ghost kills player if not frightened, gets killed if frightened
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (frightened)
            {
                Kill();
                gameManager.IncreaseScore(200);
            }
            else
            {
                if (gameManager != null)
                {
                    gameManager.LoseLife();
                }
            }
        }
    }
}
