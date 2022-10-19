using UnityEngine;

public class MysteryShip : MonoBehaviour
{
    public float speed = 10;
    public int minScore = 50;
    public int maxScore = 150;
    public System.Action<int> destroyed;
    private bool _isActive = false;
    private Vector3 _direction = Vector3.right;
    private float _padding = 2;

    public void Fly()
    {
        this._isActive = true;
        this.ToDefaultPosition();
    }

    private void Start()
    {
        this.ToDefaultPosition();
    }

    private void Update()
    {
        if (!this._isActive){
            return;
        }
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        this.transform.position += this._direction * this.speed * Time.deltaTime;
        if (this.transform.position.x > rightEdge.x + this._padding) {
            this._isActive = false;
        }
    }

    private void ToDefaultPosition()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        this.transform.position = new Vector3(leftEdge.x - this._padding, 13, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser")) {
            int score = (int)(this.minScore + Random.value * (this.maxScore - this.minScore));
            this.destroyed.Invoke(score);
            this._isActive = false;
            ToDefaultPosition();
        }
    }
}
