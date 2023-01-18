using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace plot
{
    [Serializable]
    [CreateAssetMenu(fileName = "PlotCommandConfig", menuName = "PlotCommandConfig")]
    public class CommandConfig : ScriptableObject
    {
        public enum CommandType
        {
            HEADER = 0,
            Background,
            Delay,
            Dialogue,
            Decision,
            Predicate
        }

        public List<CommandBase> commandList = new List<CommandBase>();
        public string fileName;
        private string txtSavePath = "Assets/Scripts/CommandCreator/TXT/"; 

        [CustomEditor(typeof(CommandConfig))]
        public class CommandConfigEditor : Editor
        {
            public CommandConfig commandConfig;
            private int selectedIndex;
            private ReorderableList reorderableList;
            
            private void OnEnable()
            {
                commandConfig = (CommandConfig)target;
                selectedIndex = 0;
                reorderableList = new ReorderableList(commandConfig.commandList, typeof(CommandBase), true, true, true, true);
                commandConfig.fileName = commandConfig.name;

                reorderableList.elementHeightCallback = (int index) =>
                {
                    ScriptableObject obj = commandConfig.commandList[index];
                    SerializedObject serializedObject = new SerializedObject(obj);
                    SerializedProperty property = serializedObject.GetIterator();
                    property.NextVisible(true);
                    float totalHeight = EditorGUIUtility.singleLineHeight;
                    while (property.NextVisible(true))
                    {
                        totalHeight += EditorGUI.GetPropertyHeight(property);
                    }
                    return totalHeight;
                };

                reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    ScriptableObject obj = commandConfig.commandList[index];
                    SerializedObject serializedObject = new SerializedObject(obj);
                    SerializedProperty property = serializedObject.GetIterator();
                    property.NextVisible(true);

                    EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), obj.GetType().Name);
                    rect.y += EditorGUIUtility.singleLineHeight;
                    while (property.NextVisible(true))
                    {
                        rect.height = EditorGUI.GetPropertyHeight(property);
                        int indent = EditorGUI.indentLevel;
                        EditorGUI.indentLevel++;
                        EditorGUI.PropertyField(rect, property, true);
                        EditorGUI.indentLevel = indent;
                        rect.y += rect.height;
                    }
                    serializedObject.ApplyModifiedProperties();
                    GenerateTXTCommands(commandConfig.fileName);
                };

                reorderableList.onAddCallback = (ReorderableList l) =>
                {
                    selectedIndex = EditorGUILayout.Popup("Command Type", selectedIndex, Enum.GetNames(typeof(CommandType)));
                    string className = Enum.GetName(typeof(CommandType), (CommandType)selectedIndex);
                    Type commandType = Type.GetType("plot." + className);
                    ScriptableObject obj = ScriptableObject.CreateInstance(commandType);
                    commandConfig.commandList.Add(obj as CommandBase);
                };

                LoadTXTCommands(commandConfig.fileName);
            }


            public override void OnInspectorGUI()
            {
                serializedObject.Update();
                Rect rect = EditorGUILayout.GetControlRect(false, reorderableList.GetHeight());
                reorderableList.DoList(rect);

                //ʹ��Popup�ؼ���ѡ��Ҫ��ӵ�����
                selectedIndex = EditorGUILayout.Popup("Command Type", selectedIndex, Enum.GetNames(typeof(CommandType)));
                CommandType dialogueType = (CommandType)selectedIndex;
                string className = Enum.GetName(typeof(CommandType), dialogueType);
                Type commandType = Type.GetType("plot." + className);

                serializedObject.ApplyModifiedProperties();

                GUILayout.Box("SaveOptions", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(20) });

                EditorGUILayout.LabelField("txt generate & load path: " + commandConfig.txtSavePath + commandConfig.fileName);
                //commandConfig.fileName = EditorGUILayout.TextField("fileName: ", commandConfig.fileName);

                EditorGUILayout.LabelField("Auto save is always running.");

                //if (GUILayout.Button("GenerateTXTCommands"))
                //{
                //    GenerateTXTCommands(commandConfig.fileName);
                //}

                if (GUILayout.Button("Load txt File"))
                {
                    LoadTXTCommands(commandConfig.fileName);
                }
            }

            private void GenerateTXTCommands(string fileName)
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    Debug.LogWarning("File mame is NULL or empty. Please input valid path!");
                    return;
                }
                
                string path = commandConfig.txtSavePath + fileName + ".txt";
                string content = "";

                #region "�����õ�д�뷽ʽ��"
                /*ԭ����Ȼ�ܷܺ����ת����Json��ʽ�����ö�ӦAPI���ɶ���ʱ���������治�Ƽ�ʹ��new����SO�������ԻᱨWarning��
                 * �Զ�������������Json���ݻ����е�ͨ���������ʼ��ʵ����TextParser�����������Json��ʽ�Ľ���������*/
                //foreach (CommandBase command in commandConfig.commandList)
                //{
                //    content += command.GetType().Name;
                //    content += "\n";
                //    string json = JsonConvert.SerializeObject(command);
                //    content += json;
                //    content += "\n";
                //}
                #endregion

                #region "��commandList�е����ж���˳��д���ı��ļ�"
                foreach (CommandBase command in commandConfig.commandList)
                {
                    Type commandType = command.GetType();
                    if (commandType == null)
                    {
                        Debug.LogWarning("Invalid commandType.");
                        continue;
                    }
                    FieldInfo[] fields = commandType.GetFields();
                    string commandString = "[" + command.GetType().Name + "(";

                    #region "�Ѹö��������е����԰��ҵ�Լ����ʽд���ı�"
                    foreach (var field in fields)
                    {
                        var value = field.GetValue(command);

                        if(value == null)
                        {
                            commandString += field.Name + "=" + "\"\"" + ",";
                            continue;
                        }

                        if (value.GetType() == typeof(string))
                        {
                            commandString += field.Name + "=" + "\"" + value + "\"" + ",";
                        }
                        else
                        {
                            commandString += field.Name + "=" + value + ",";
                        }
                    }
                    #endregion
                    commandString = commandString.TrimEnd(',');
                    commandString += ")]";
                    //Debug.Log(commandString);
                    content += commandString + "\n";
                }
                #endregion

                System.IO.File.WriteAllText(path, content);

                //Debug.Log("Generate TXT Commands!");
            }

            private void LoadTXTCommands(string fileName)
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    Debug.LogWarning("File name is NULL or empty, please input valid path!");
                    return;
                }

                string filePath = commandConfig.txtSavePath + fileName + ".txt";
                if (!File.Exists(filePath))
                {
                    Debug.LogWarning("File does not exists, please input valid path!");
                    return;
                }

                commandConfig.commandList.Clear();

                TextParser textParser = new TextParser(filePath);
                MyCommand[] mc = textParser.ParserByLine();

                for (int i = 0; i < mc.Length; i++)
                {
                    CommandBase command = ScriptableObject.CreateInstance(mc[i].name) as CommandBase;
                    if (command == null)
                    {
                        Debug.LogError("Failed to create instance of " + mc[i].name);
                        continue;
                    }

                    Type commandType = command.GetType();
                    if (commandType == null)
                    {
                        Debug.LogError("Invalid commandType in loading commands from txt.");
                        continue;
                    }

                    #region "������������Ϊcommand�����ÿ���ֶθ�ֵ"
                    Dictionary<string, object> parameters = textParser.ParseParameters(mc[i].parameter);
                    FieldInfo[] fields = commandType.GetFields();
                    for (int j = 0; j < fields.Length; j++)
                    {
                        if (parameters.ContainsKey(fields[j].Name))
                        {
                            var value = Convert.ChangeType(parameters[fields[j].Name], fields[j].FieldType);
                            fields[j].SetValue(command, value);
                        }
                        else
                        {
                            Debug.LogError("The parameters " + "does not have "+ commandType + " 's field: " + fields[j].Name);
                        }
                    }
                    #endregion


                    commandConfig.commandList.Add(command);
                }

                Debug.Log("Load TXT Commands!");
            }
        }


    }
}


