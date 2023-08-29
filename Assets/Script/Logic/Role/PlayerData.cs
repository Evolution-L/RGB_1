using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    private InputListener inputListener;

    private State playerState = 0;

    private Vector3 nextTargetPosition = Vector3.zero;
}
