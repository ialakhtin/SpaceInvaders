using UnityEngine;

public class Invader : MonoBehaviour
{
    public Sprite[] sprites;
    public float animationTime = 1;
    public int score;
    public System.Action<int> destroyed;
    public System.Action gameOver;
    private SpriteRenderer _spriteRenderer;
    private int _currentSprite;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(Animate), this.animationTime, this.animationTime);
    }

    private void Animate()
    {
        ++_currentSprite;
        if (_currentSprite >= this.sprites.Length) {
            _currentSprite = 0;
        }
        _spriteRenderer.sprite = this.sprites[_currentSprite];
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser")) {
            this.destroyed.Invoke(score);
            this.gameObject.SetActive(false);
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Player") || 
        other.gameObject.layer == LayerMask.NameToLayer("Border")) {
            this.gameOver.Invoke();
        }
    }
}
