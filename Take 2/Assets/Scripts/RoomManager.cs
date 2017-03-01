using UnityEngine;
using System.Collections;

public class RoomManager : MonoBehaviour {

    [SerializeField]
    private bool empty;
    public bool Empty
    {
        set { empty = value; }
        get { return empty; }
    }
}
