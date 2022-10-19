using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float actionTime = 5;
    public Sprite sprite;
    protected Player _player;
    private float _startTime;
    
    private void Awake() {
        this._player = FindObjectOfType<Player>();
        this._player.shooting += this.Shoot;
    }

    private void Start() {
        this.Activate();
        this._startTime = Time.time;
    }

    private void Update() {
        if (Time.time - this._startTime >= this.actionTime) {
            this.Deactivate();
            Destroy(this.gameObject);
        }
    }

    protected virtual void Shoot() {}
    protected virtual void Activate() {
        Debug.Log("Default");
    }
    protected virtual void Deactivate() {}
}
