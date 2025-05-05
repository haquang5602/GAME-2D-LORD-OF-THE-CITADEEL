using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DragFlash : DragAndDrop

    
{
    protected override void Reset()
    {
        base.Reset();
        towerCost = 150;
    }
}
