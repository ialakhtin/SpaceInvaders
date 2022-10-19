using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBox : MonoBehaviour
{
    public float speed = 5;
    public Vector3 direction = new Vector3(0, -1, 0);
    public PowerUp powerUpPrefab;
    private SpriteRenderer _spriteRenderer;

    public void SetPrefab(PowerUp prefab) {
        this.powerUpPrefab = prefab;
        this._spriteRenderer = GetComponent<SpriteRenderer>();
        this._spriteRenderer.sprite = this.powerUpPrefab.sprite;
    }
    private void Update()
    {
        this.transform.position += direction * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Instantiate(this.powerUpPrefab, this.transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
