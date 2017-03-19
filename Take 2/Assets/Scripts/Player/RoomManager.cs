using UnityEngine;
using System.Collections;

public class RoomManager : MonoBehaviour {

    [SerializeField]
    private bool empty;
    public bool Empty
    {
        get { return empty; }
        set { empty = value; }
    }
}
