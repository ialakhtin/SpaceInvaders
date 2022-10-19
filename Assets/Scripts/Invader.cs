using UnityEngine;

public class Invader : MonoBehaviour
{
    public Sprite[] sprites;
    public float animationTime = 1;
    public int score;
    public System.Action<int> destroyed;
    public System.Action gameOver;
    public float powerUpProb;
    public PowerUp[] powerUps;
    public PowerUpBox boxPrefab;
    private SpriteRenderer _spriteRenderer;
    private int _currentSprite;
    private static System.Random _rnd = new System.Random();

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
            this.Dead();
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Player") || 
        other.gameObject.layer == LayerMask.NameToLayer("Border")) {
            this.gameOver.Invoke();
        }
    }

    private void Dead() {
        if (Random.value < this.powerUpProb) {
            PowerUpBox box = Instantiate(this.boxPrefab, this.transform.position, Quaternion.identity);
            box.SetPrefab(this.powerUps[_rnd.Next(0, powerUps.Length)]);
        }
        this.destroyed.Invoke(score);
        this.gameObject.SetActive(false);
    }
}
