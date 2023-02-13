using FairyGUI;
using plot_module;
using System.Collections.Generic;
using UnityEngine;

namespace plot_command_executor
{
    public struct DialogueContent
    {
        public string talker_name;
        public string talker_text;
    }

    public class PlotUISettings : Singleton<PlotUISettings>
    {
        public GComponent dialogueRoot;
        public SkipWindow skipWindow = null;
        public float typingEffectTimeDevision = 0.02f;
        public Vector2 pixelSize = new Vector2(1920, 1080);

        public List<DialogueContent> dialogueContents = new List<DialogueContent>();
        public List<int> playerDecisions = new List<int>();
        public string fguiPackagePath = "Assets/UI/Plot";
        public string fguiPackageName = "Plot";
        private string m_fguiComponentName = "Dialogue";

        public void Init()
        {
            //��ʼ��FGUI,ֱ������
            UIPackage.AddPackage(fguiPackagePath);
            GameObject root_go = new GameObject("plot_root_go");
            root_go.layer = LayerMask.NameToLayer("UI");
            UIPanel ui_panel = root_go.AddComponent<UIPanel>();
            ui_panel.packageName = fguiPackageName;
            ui_panel.componentName = m_fguiComponentName;
            ui_panel.container.renderMode = RenderMode.ScreenSpaceOverlay;
            ui_panel.CreateUI();
            dialogueRoot = ui_panel.ui;
            dialogueRoot.SetSize(pixelSize.x, pixelSize.y);
            dialogueRoot.Center();
            dialogueRoot.visible = false;

            if (CommandSender.Instance == null)
            {
                root_go.AddComponent<CommandSender>().global = true;
                CommandSender.Instance.Init();
            }

            PlotModule.Instance.plotBegin.AddListener(() =>
            {
                SetUI();
            });

            PlotModule.Instance.plotEnd.AddListener(() =>
            {
                ResetUI();
            });
        }

        public void SetUI()
        {
            //��ʼ��������ť
            GButton skipButton = dialogueRoot.GetChild("skip_button").asButton;
            skipButton.onClick.Set(() =>
            {
                if (skipWindow == null)
                    Instance.skipWindow = new SkipWindow();
                Instance.skipWindow.Show();
            });

            //��ʼ����ʾ��ʷ��¼��ť
            GButton showHistoryButton = dialogueRoot.GetChild("show_history_button").asButton;
            showHistoryButton.onClick.Set(() =>
            {
                UIPackage.AddPackage(fguiPackagePath);
                GComponent com = (GComponent)UIPackage.CreateObject(fguiPackageName, "ConversationHistory");
                dialogueRoot.AddChild(com);
                com.SetSize(pixelSize.x, pixelSize.y);
                com.Center();

                GList gList = com.GetChild("list").asList;
                gList.Center();
                gList.SetVirtual();
                gList.itemRenderer = (int index, GObject obj) =>
                {
                    var com = obj.asCom;
                    com.GetChild("name").asTextField.text = dialogueContents[index].talker_name;
                    com.GetChild("text").asTextField.text = dialogueContents[index].talker_text;
                };
                gList.numItems = dialogueContents.Count;
                gList.ScrollToView(gList.numItems - 1);
                //Debug.Log(dialogueContents.Count);

                GButton gButton = com.GetChild("close_button").asButton;
                gButton.onClick.Set(() => {
                    com.Dispose();
                });
                gButton.GetChild("text").asTextField.text = "X";
            });
        }

        public void ResetUI()
        {
            //��ԭ��ɫ��ʾ
            GComponent com = dialogueRoot.GetChild("character").asCom;
            GList gList = com.GetChild("list").asList;
            gList.numItems = 0;

            //��ԭ������ʾ
            GTextField n0_gtf = dialogueRoot.GetChild("name").asTextField;
            n0_gtf.text = "";
            GTextField n1_gtf = dialogueRoot.GetChild("text").asTextField;
            n1_gtf.text = "";

            //��ԭ��ʷ��¼
            dialogueContents.Clear();

            //���ѡ�ť�����У�
            playerDecisions.Clear();

            foreach (var child in dialogueRoot.GetChildren())
            {
                if(child.name == "DecisionButtonList")
                {
                    child.Dispose();
                    break;
                }
            }
        }
    }

}
