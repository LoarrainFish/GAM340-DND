//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System;

//public class RoomManager1 : EditorWindow
//{

//    bool createRoom;
//    bool optionToggleGroup;
//    bool addOption;
//    bool addOption2;
//    bool addOption3;
//    bool addOption4;

//    bool finalizeOptions;

//    int buttonShow = 1;

//    [MenuItem("Test Window/Create Room")]
//    static void Init()
//    {

//        RoomManager1 window = (RoomManager1)EditorWindow.CreateInstance(typeof(RoomManager));
//        window.Show();
//    }

//    public RoomList roomList;

//    int newRoomInstanceID;
//    string newRoomDescription;
//    string newRoomText;

//    string newOption1Text;
//    int newOption1Route;
//    string newOption2Text;
//    int newOption2Route;
//    string newOption3Text;
//    int newOption3Route;
//    string newOption4Text;
//    int newOption4Route;

//    int currentOption;

//    void OnGUI()
//    {
//#pragma warning disable CS0618 // Type or member is obsolete

//        roomList = EditorGUILayout.ObjectField(roomList, typeof(RoomList)) as RoomList;

//#pragma warning restore CS0618 // Type or member is obsolete


//        GUILayout.Label("Standard Instance", EditorStyles.boldLabel);

//        newRoomInstanceID = EditorGUILayout.IntField("Room Instance ID ", newRoomInstanceID);
//        newRoomDescription = EditorGUILayout.TextField("Room Description ", newRoomDescription);
//        newRoomText = EditorGUILayout.TextField("Room Text", newRoomText);

//        optionToggleGroup = EditorGUILayout.BeginToggleGroup("Add Options", optionToggleGroup);

//        EditorGUI.indentLevel++;

//            addOption = EditorGUILayout.BeginToggleGroup("Add Option 1", addOption);
//                newOption1Text = EditorGUILayout.TextField("Option Description ", newOption1Text);
//                newOption1Route = EditorGUILayout.IntField("Room Output", newOption1Route);
//            EditorGUILayout.EndToggleGroup();

//            addOption2 = EditorGUILayout.BeginToggleGroup("Add Option 2", addOption2);
//                newOption2Text = EditorGUILayout.TextField("Option Description ", newOption2Text);
//                newOption2Route = EditorGUILayout.IntField("Room Output", newOption2Route);
//            EditorGUILayout.EndToggleGroup();

//            addOption3 = EditorGUILayout.BeginToggleGroup("Add Option 3", addOption3);
//                newOption2Text = EditorGUILayout.TextField("Option Description ", newOption3Text);
//                newOption2Route = EditorGUILayout.IntField("Room Output", newOption3Route);
//            EditorGUILayout.EndToggleGroup();

//            addOption4 = EditorGUILayout.BeginToggleGroup("Add Option 4", addOption4);
//                newOption4Text = EditorGUILayout.TextField("Option Description ", newOption4Text);
//                newOption4Route = EditorGUILayout.IntField("Room Output", newOption4Route);
//            EditorGUILayout.EndToggleGroup();

//        EditorGUI.indentLevel--;



//        Room newRoom = ScriptableObject.CreateInstance<Room>();



//    }

//}
