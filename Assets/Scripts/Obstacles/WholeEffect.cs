using System.Collections;
using UnityEngine;

public class WholeEffect : MonoBehaviour
{
    [Header("Attraction Attributes")]
    public float attractionForce = 10f;

    [Header("Teletransport Attributes")]
    public Transform teleportTarget;
    public float teleportDelay = 2f;

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
            player.SetActive(false);
            yield return new WaitForSeconds(teleportDelay);

            if (teleportTarget != null)
            {
                player.SetActive(true);
                yield return null;
                player.transform.position = teleportTarget.position;
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