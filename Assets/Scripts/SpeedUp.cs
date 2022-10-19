using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : PowerUp
{
    public float delta = 2;
    protected override void Activate() {
        this._player.speed *= this.delta;
        Debug.Log(this._player.speed);
    }
    protected override void Deactivate() {
        this._player.speed /= this.delta;
        Debug.Log(this._player.speed);
    }
}
