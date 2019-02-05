using UnityEngine;

public class Teleporter : MonoBehaviour
{
    /* Most of the teleporter logic is copied from Brackeys
     * https://www.youtube.com/watch?v=cuQao3hEKfs&t=1231s
     * 
     */

    public Transform character;
    public Transform destination;
    public int direction; // 0 = left, 1 = right

    public bool characterOverlapping = false;

    // Update is called once per frame
    void Update()
    {
        if (characterOverlapping)
        {
            Vector3 portalToCharacter = character.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToCharacter);


            if ((dotProduct > 0.0f && direction == 0) || (dotProduct < 0.0f && direction == 1))
            {
                float rotationDiff = -Quaternion.Angle(transform.rotation, destination.rotation);
                rotationDiff += 180;
                character.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0.0f, rotationDiff, 0.0f) * portalToCharacter;
                character.position = destination.position + positionOffset;

                characterOverlapping = false;
                character = null;
            }
        }
    }

    // Character is inside portal
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Ghost")
        {
            characterOverlapping = true;
            character = other.transform;
        }
    }

    // Character has exited portal
    public void OnTriggerExit(Collider other)
    {
        characterOverlapping = false;
        character = null;
    }
}
