using UnityEngine;

public class InfiniteParallax : MonoBehaviour
{
    [SerializeField] private float _fallSpeed = 2f; 
    private float _spriteHeight;
    private void Start()
    {
        _spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    private void Update()
    {
        transform.position += _fallSpeed * Time.deltaTime * Vector3.down;

        if (transform.position.y < -_spriteHeight)
        {
            RepositionSprite();
        }
    }

    private void RepositionSprite()
    {
        // Reposiciona o sprite para cima, criando o efeito de loop infinito
        transform.position += new Vector3(0, _spriteHeight * 2f, 0);
    }
}
