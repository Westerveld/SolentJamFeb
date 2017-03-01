using UnityEngine;
using System.Collections;

enum CurrentRoom
{
    Middle,
    Engine,
    Freeze,
    Shield,
    Turret
}

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private string controller;
    public string Controller
    {
        set { controller = value; }
        get { return controller; }
    }

    [SerializeField]
    private CurrentRoom room;
    
    public Transform[] Rooms;

    [SerializeField]
    private EngineController engine;

    [SerializeField]
    private ShieldController shield;

    [SerializeField]
    private GunController gun;

    [SerializeField]
    private GameManager g

	// Use this for initialization
	void Start () {
        transform.position = Rooms[0].position;
        room = CurrentRoom.Middle;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        MoveRooms();
	}

    void MoveRooms()
    {
        if (Input.GetButton("Engine" + controller))
        {
            
            if (Rooms[1].gameObject.GetComponent<RoomManager>().Empty)
            {
                LeaveRoom();
                Rooms[1].gameObject.GetComponent<RoomManager>().Empty = false;
                transform.position = Rooms[1].position;
                room = CurrentRoom.Engine;
                engine.Joystick = controller;
            }
        }
        else if (Input.GetButton("Shield" + controller))
        {
            if (Rooms[2].gameObject.GetComponent<RoomManager>().Empty)
            {
                LeaveRoom();
                Rooms[2].gameObject.GetComponent<RoomManager>().Empty = false;
                transform.position = Rooms[2].position;
                room = CurrentRoom.Shield;
                shield.Joystick = controller;
            }
        }
        else if (Input.GetButton("Turret" + controller))
        {
            if (Rooms[3].gameObject.GetComponent<RoomManager>().Empty)
            {
                LeaveRoom();
                Rooms[3].gameObject.GetComponent<RoomManager>().Empty = false;
                transform.position = Rooms[3].position;
                room = CurrentRoom.Turret;
                gun.Joystick = controller;
            }
        }
        else if(Input.GetButton("Freeze" + controller))
        {
            if (Rooms[4].GetComponent<RoomManager>().Empty)
            {
                LeaveRoom();
                Rooms[4].gameObject.GetComponent<RoomManager>().Empty = false;
                transform.position = Rooms[4].position;
                room = CurrentRoom.Freeze;
            }
        }

    }

    void LeaveRoom()
    {
        switch(room)
        {
            case CurrentRoom.Engine:
                Rooms[1].gameObject.GetComponent<RoomManager>().Empty = true;
                engine.Joystick = null;
                break;
            case CurrentRoom.Freeze:
                Rooms[2].gameObject.GetComponent<RoomManager>().Empty = true;

                break;
            case CurrentRoom.Turret:
                Rooms[3].gameObject.GetComponent<RoomManager>().Empty = true;
                gun.Joystick = null;
                break;
            case CurrentRoom.Shield:
                Rooms[4].gameObject.GetComponent<RoomManager>().Empty = true;
                shield.Joystick = null;
                break;
            default:
                break;
        }
    }
}
