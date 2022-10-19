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
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        float padding = 1;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            Vector3 position = this.transform.position + Vector3.left * speed * Time.deltaTime;
            if (position.x < leftEdge.x + padding) {
                position += Vector3.right * (leftEdge.x + padding - position.x);
            }
            this.transform.position = position;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            Vector3 position = this.transform.position + Vector3.right * speed * Time.deltaTime;
            if (position.x > rightEdge.x - padding) {
                position -= Vector3.right * (position.x - rightEdge.x + padding);
            }
            this.transform.position = position;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Shoot();
        }
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
