using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5;
    public float shootTime = 0.7f;
    public Bullet laserPrefab;
    public Sprite[] sprites;
    public System.Action<int> dead;
    public System.Action shooting;

    private SpriteRenderer _spriteRenderer;
    private bool _laserActive = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Rebuild() {
        this.transform.position = new Vector3(0, -13, 0);
        this._spriteRenderer.sprite = this.sprites[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Shoot();
        }
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        float padding = 1;
        Vector3 direction;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            direction = Vector3.left;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            direction = Vector3.right;
        } else {
            return;
        }
        Vector3 position = this.transform.position + direction * speed * Time.deltaTime;
        if (position.x < leftEdge.x + padding) {
            position += Vector3.right * (leftEdge.x + padding - position.x);
        }
        if (position.x > rightEdge.x - padding) {
            position -= Vector3.right * (position.x - rightEdge.x + padding);
        }
        this.transform.position = position;
    }

    private void Shoot()
    {
        if (!this._laserActive) {
            Bullet laser = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            laser.destroyed += LaserDestroyed;
            this._laserActive = true;
            shooting.Invoke();
        }
    }

    void LaserDestroyed()
    {
        this._laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile")) {
            this._spriteRenderer.sprite = this.sprites[1];
            this.dead.Invoke(1);
        }
    }
}
