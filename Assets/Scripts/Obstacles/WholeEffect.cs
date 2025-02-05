using System.Collections;
using UnityEngine;

public class WholeEffect : MonoBehaviour
{
    [Header("Attraction Attributes")]
    [SerializeField] [Range(1,20)] [Tooltip("How strong the whole can attract")]
    private float attractionForce = 10f;

    [Header("Teletransport Attributes")]
    [SerializeField] [Range(1, 20)] [Tooltip("The place where the player will be teleported")]
    private Transform teleportTarget;

    [SerializeField] [Range(1, 20)] [Tooltip("The time to teleport")]
    private float teleportDelay = 2f;

    [SerializeField] [Tooltip("The Range of randomic places to player spawn")]
    private Vector2 randomRangeX;

    [SerializeField] [Tooltip("The Range of randomic places to player spawn")]
    private Vector2 randomRangeY;

    public void ApplyAttraction(Rigidbody2D rb) // Agora recebe o Rigidbody2D como argumento
    {
        if (rb != null)
        {
            Vector2 direction = transform.position - rb.transform.position;
            rb.AddForce(direction.normalized * attractionForce, ForceMode2D.Force);
        }
    }

    public void TeleportPlayer(GameObject player)
    {
        StartCoroutine(TeleportCoroutine(player));
    }

    private IEnumerator TeleportCoroutine(GameObject player)
    {
        if (player != null)
        {
            player.gameObject.GetComponent <CircleCollider2D>().enabled = false;
            Color color = player.gameObject.GetComponent<SpriteRenderer>().color;
            color.a = 0;
            player.gameObject.GetComponent<SpriteRenderer>().color=color;
            yield return new WaitForSeconds(teleportDelay);

            if (teleportTarget != null)
            {
                player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
               
               
                color.a = 1;
                player.gameObject.GetComponent<SpriteRenderer>().color = color;
                yield return null;
                //Precisa desse Random para as bolhas bugarem
                float newX = Random.Range(randomRangeX.x, randomRangeX.y);
                float newY = Random.Range(randomRangeY.x, randomRangeY.y);
                player.transform.position = new Vector2(teleportTarget.position.x+newX, teleportTarget.position.y + newY);
                player.gameObject.GetComponent<CircleCollider2D>().enabled = true;
            }
            else
            {
                Debug.LogError("Alvo de teletransporte não definido!");
            }
        }
        else
        {
            Debug.LogError("Player não encontrado para teletransportar!");
        }
    }
}