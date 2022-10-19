using UnityEngine;

public class InvaderFabric : MonoBehaviour
{
    public int rows = 5;
    public int columns = 11;
    public Invader[] prefabs;
    public Bullet misslilePrefab;
    public System.Action<int> invaderDestroyed;
    public System.Action win;
    public System.Action gameOver;
    public float shootTime = 1;
    public AnimationCurve speed;

    private int _killed = 0;
    private int _invaderCount => this.rows * this.columns;
    private float _percentKilled => (float)this._killed / this._invaderCount;
    private Invader[] _invaders;
    private Vector3 _direction = Vector3.right;

    private void Awake() 
    {
        float rowPadding = 2;
        float columnPadding = 2;
        Vector2 center = new Vector2(0, 5);

        Vector2 startPosition = center - new Vector2((columns - 1) * columnPadding, 
            (rows - 1) * rowPadding) / 2 ;
        for (int row = 0; row < this.rows; ++row) 
        {
            Vector2 rowPosition = new Vector2(0, row * rowPadding);

            for (int column = 0; column < this.columns; ++column) 
            {
                Invader invader = Instantiate(this.prefabs[row], this.transform);
                Vector2 columnPosition = new Vector2(column * columnPadding, 0);
                invader.transform.position = startPosition + rowPosition + columnPosition;
                invader.destroyed += this.InvaderDestroyed;
                invader.gameOver += this.GameOver;
            }
        }
    }

    private void Update()
    {
        this.transform.position += this._direction * this.speed.Evaluate(this._percentKilled) * Time.deltaTime;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        float padding = 1;

        foreach(Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            if (this._direction == Vector3.right && invader.position.x + padding >= rightEdge.x) {
                this.MoveDown();
                break;
            }
            if (this._direction == Vector3.left && invader.position.x - padding <= leftEdge.x) {
                this.MoveDown();
                break;
            }
        }
    }

    void MoveDown()
    {
        this._direction *= -1;
        this.transform.position -= new Vector3(0, 1, 0);
    }

    void InvaderDestroyed(int score)
    {
        ++this._killed;
        if (this._killed == this._invaderCount) {
            this.win.Invoke();
            return;
        }
        this.invaderDestroyed.Invoke(score);
    }

    private void Start()
    {
        InvokeRepeating(nameof(this.Shoot), this.shootTime, this.shootTime);
    }

    private void Shoot()
    {
        foreach(Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            if (Random.value < 1.0f / (this._invaderCount - this._killed)) {
                Bullet missle = Instantiate(this.misslilePrefab, invader.position, Quaternion.identity);
                missle.destroyed += MissileDestroyed;
            }
        }
    }

    private void MissileDestroyed(){}

    private void GameOver() {
        this.gameOver.Invoke();
    }
}
