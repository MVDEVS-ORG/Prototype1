using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public LayerMask troopLayer;
    private Vector2 _dragStart;
    private Rect _selectionRect;
    private List<Troop> _selectedTroops = new List<Troop>();

    private static List<Troop> _allTroops = new List<Troop>();
    public static void RegisterTroop(Troop t) { if (!_allTroops.Contains(t)) _allTroops.Add(t); }
    public static void UnregisterTroop(Troop t) { _allTroops.Remove(t); }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) _dragStart = Input.mousePosition;
        if (Input.GetMouseButton(0)) _selectionRect = Utils.GetScreenRect(_dragStart, Input.mousePosition);
        if (Input.GetMouseButtonUp(0)) 
        { 
            SelectTroopsInRect(); 
            _selectionRect = new Rect(); 
        }

        if (Input.GetMouseButtonDown(1)) _dragStart = Input.mousePosition;
        if (Input.GetMouseButtonUp(1))
        {
            Vector2 end = Input.mousePosition;
            if ((end - _dragStart).magnitude < 10f) IssueMoveCommand(end);
            else IssueFormation(_dragStart, end);
        }
    }
    void OnGUI()
    {
        if (_selectionRect.width > 0 && _selectionRect.height > 0)
        {
            Utils.DrawScreenRect(_selectionRect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(_selectionRect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    void SelectTroopsInRect()
    {
        foreach (var t in _selectedTroops) t.Deselect();
        _selectedTroops.Clear();
        Camera cam = Camera.main;
        foreach (var troop in _allTroops)
        {
            Vector3 sp = cam.WorldToScreenPoint(troop.transform.position);
            // sp.y from bottom; GUI rect uses inverted y
            sp.y = Screen.height - sp.y;
            if (_selectionRect.Contains(sp))
            {
                troop.Select();
                _selectedTroops.Add(troop);
            }
        }
    }
    void IssueMoveCommand(Vector2 screenPos)
    {
        Ray r = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(r, out RaycastHit hit))
        {
            foreach (var t in _selectedTroops) t.MoveTo(hit.point);
        }
    }
    void IssueFormation(Vector2 s, Vector2 e)
    {
        Ray r1 = Camera.main.ScreenPointToRay(s), r2 = Camera.main.ScreenPointToRay(e);
        Plane g = new Plane(Vector3.up, 0);
        g.Raycast(r1, out float d1); g.Raycast(r2, out float d2);
        Vector3 w1 = r1.GetPoint(d1), w2 = r2.GetPoint(d2);
        int c = _selectedTroops.Count;
        for (int i = 0; i < c; i++)
        {
            float t = (i + 1f) / (c + 1f);
            _selectedTroops[i].MoveTo(Vector3.Lerp(w1, w2, t));
        }
    }
}
