using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverse : PowerUp
{
    protected override void Activate() {
        this._player.speed *= -1;
    }
    protected override void Deactivate() {
        this._player.speed *= -1;
    }
}
