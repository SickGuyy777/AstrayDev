using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor
{
    private const string blacklistName = "blacklist";
    private const string whitelistName = "whitelist";

    private bool _usingBlacklist;
    private bool UsingBlacklist
    {
        get => _usingBlacklist;
        set
        {
            if (_usingBlacklist == value)
                return;

            _usingBlacklist = value;
            tagListStringName = value ? blacklistName : whitelistName;
        }
    }

    private string tagListStringName = whitelistName;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var room = (Room)target;

        if (GUILayout.Button("Use " + tagListStringName))
        {
            UsingBlacklist = !UsingBlacklist;
            room.useBlackList = UsingBlacklist;
        }
    }
}