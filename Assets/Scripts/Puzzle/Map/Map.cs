using UnityEngine;
using System.Collections.Generic;
using BoxScripts;

public class Map : InteractBase {
    
    public GameObject PinPrefab;
    public Material Red;
    public LineRenderer RedLine;
    public GameObject MarkerPrefab;
    public List<MapIntersection> Markers;
    public List<GameObject> PinList;
    public PrimaryController controller;

    private void OnEnable() {
        if(tag != " Map") tag = "Map";
    }

    private void Start() {
        PinList = new List<GameObject>();
        Markers = new List<MapIntersection>();
        if(tag != " Map") tag = "Map";
    }


    public override void Execute(bool isLeftAction = true)
    {
        if(tag != " Map") tag = "Map";
        // base.Execute(isLeftAction);
        if(isLeftAction)
        {
            Debug.Log("[Map] Setting pin");
            SetPin();
        }
        
        /*if(!isLeftAction)
        {
            Transform temp = ReturnPointer();
            // RemovePin();
            Debug.Log("Pin name is " + temp.name);
        }*/
    }

    public void SetPin()
    {
        Vector3 pos = ReturnHitPoint();
        if(pos != Vector3.zero)
        {
            Debug.Log("[Map] Instantiating prefab");
            GameObject go = Instantiate(PinPrefab);
            MapPin mapPin = go.GetComponent<MapPin>();
            mapPin.SetPin(transform, Red);
            go.transform.position = pos;
            PinList.Add(go);

            foreach(MapIntersection mi in Markers)
            {
                Destroy(mi.Marker);
            }
            Markers = new List<MapIntersection>();
            DrawIntersections(PinList);
        }
    }

    public void RemovePin(GameObject ioa)
    {

        if(PinList.Exists(vec => vec == ioa))
            PinList.Remove(ioa);

        Destroy(ioa);

        foreach(MapIntersection mi in Markers)
        {
            Destroy(mi.Marker);
        }
        Markers = new List<MapIntersection>();
        DrawIntersections(PinList);
    }

    private Transform ReturnPointer()
    {
        Ray r = gameControllerObject.camera.ScreenPointToRay((Vector3)controller.Mouse);
        if(Physics.Raycast(
            r, out var hit, 5f,
            LayerMask.GetMask("Interactuable")))
        {
            if(hit.transform.tag == "MapPin") return hit.transform;
        }

        return null;
    }

    private Vector3 ReturnHitPoint()
    {
        Ray r = gameControllerObject.camera.ScreenPointToRay((Vector3)controller.Mouse);
        Debug.Log("[Map] Returning HitPoint");
        if(Physics.Raycast(
            r, out var hit, Mathf.Infinity,
            LayerMask.GetMask("Interactuable")))
        {

            Debug.Log(hit.point + " at " + hit.transform.tag);
            if(hit.transform.tag == "Map") return hit.point;
        }
        Debug.Log("[Map] Returning HitPoint " + Vector3.zero);
        return Vector3.zero;
    }

    void Update()
    {
        
        if(PinList.Count > 1)
        {
            if(!RedLine.enabled) RedLine.enabled = true;
            SetLineRenderer(PinList, RedLine);
        } else if(RedLine.enabled) RedLine.enabled = false;
    }

    private void DrawIntersections (List<GameObject> list)
    {
        List<Vector3> positions = new List<Vector3>();

        List<Line> lines = new List<Line>();

        for(int i = 0; i < list.Count; i++)
        {
            positions.Add(list[i].GetComponent<MapPin>().lineStart.position);
            if(i < list.Count - 1)
            {
                Vector2 point1 = new Vector2(list[i].GetComponent<MapPin>().lineStart.position.z, list[i].GetComponent<MapPin>().lineStart.position.y);
                Vector2 point2 = new Vector2(list[i + 1].GetComponent<MapPin>().lineStart.position.z, list[i + 1].GetComponent<MapPin>().lineStart.position.y);
                Line tempLine = new Line(point1, point2);
                lines.Add(tempLine);
            }
        }

        if(lines.Count > 1)
        {
            for(int i = 0; i < lines.Count; i++)
            {
                for(int j = i + 1; j < lines.Count; j++)
                {
                    Vector2 testVec = Vector2.zero;
                    if(LineIntersection.HasIntersection(lines[i], lines[j], out testVec))
                    {
                        if(Markers == null) Markers = new List<MapIntersection>();
                        bool hasMarker = false;
                        foreach(MapIntersection mi in Markers)
                        {
                            if((mi.line1 == lines[i] && mi.line2 == lines[j]) || (mi.line1 == lines[j] && mi.line2 == lines[i]))
                            {
                                hasMarker = true;
                                break;
                            }
                        }
                        if(!hasMarker)
                        {
                            GameObject go = Instantiate(MarkerPrefab);
                            go.transform.SetParent(transform);
                            go.transform.position = new Vector3(go.transform.position.x, testVec.y, testVec.x);

                            MapIntersection tempMapInteraction = new MapIntersection(go, lines[i], lines[j]);
                            Markers.Add(tempMapInteraction);
                            Debug.Log("[Map] New Intersection at : " + testVec);
                        }
                        
                        Debug.Log("[Map] Intersection at : " + testVec);
                    }
                        
                }
            }
        }
    }

    void SetLineRenderer(List<GameObject> list, LineRenderer lr)
    {
        List<Vector3> positions = new List<Vector3>();

        for(int i = 0; i < list.Count; i++)
        {
            positions.Add(list[i].GetComponent<MapPin>().lineStart.position);
        }
        lr.positionCount = list.Count;
        lr.SetPositions(positions.ToArray());
    }
}
public class MapIntersection{
    public GameObject Marker;
    public Line line1;
    public Line line2;
    public MapIntersection(GameObject mrk, Line l1, Line l2)
    {
        Marker = mrk;
        line1 = l1;
        line2 = l2;
    }

}