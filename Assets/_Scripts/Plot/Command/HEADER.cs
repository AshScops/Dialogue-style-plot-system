using FairyGUI;
using plot_command_executor;
using plot_utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace plot_command
{
    [Serializable]
    public class HEADER : CommandBase
    {
        [field: SerializeField]
        public string title;
        [field: SerializeField]
        public bool is_skippable = true;
        [field: SerializeField]
        public string fit_mode;

        public override void Execute()
        {
            PlotUISettings.Instance.dialogueRoot.SetSize(PlotUISettings.Instance.pixelSize.x, PlotUISettings.Instance.pixelSize.y);

            //�������������
            foreach (var child in PlotUISettings.Instance.dialogueRoot.GetChildren())
            {
                child.visible = false;
            }

            //�Ժ������Ҫע��װ����
            //UIObjectFactory.SetLoaderExtension(typeof(MyGLoader));

            //����HEADER����ֵ
            UIPackage.AddPackage(PlotUISettings.Instance.fguiPackagePath);
            GComponent com = (GComponent)UIPackage.CreateObject(PlotUISettings.Instance.fguiPackageName, "HEADER");
            PlotUISettings.Instance.dialogueRoot.AddChild(com);
            com.SetSize(PlotUISettings.Instance.pixelSize.x, PlotUISettings.Instance.pixelSize.y);
            com.Center();
            com.GetChild("title").asTextField.text = title;

            //��ʾ���
            PlotUISettings.Instance.dialogueRoot.visible = true;

            //��ʼ��������ť
            GButton skipButton = PlotUISettings.Instance.dialogueRoot.GetChild("skip_button").asButton;

            if (!is_skippable)
                skipButton.Dispose();
            else
            {
                skipButton.onClick.Set(() =>
                {
                    if (PlotUISettings.Instance.skipWindow == null)
                        PlotUISettings.Instance.skipWindow = new SkipWindow();
                    PlotUISettings.Instance.skipWindow.Show();
                });
            }

            //��ʼ����ʾ��ʷ��¼��ť
            GButton showHistoryButton = PlotUISettings.Instance.dialogueRoot.GetChild("show_history_button").asButton;
            showHistoryButton.onClick.Set(() =>
            {
                UIPackage.AddPackage(PlotUISettings.Instance.fguiPackagePath);
                GComponent com = (GComponent)UIPackage.CreateObject(PlotUISettings.Instance.fguiPackageName, "ConversationHistory");
                PlotUISettings.Instance.dialogueRoot.AddChild(com);
                com.SetSize(PlotUISettings.Instance.pixelSize.x, PlotUISettings.Instance.pixelSize.y);
                com.Center();

                GList gList = com.GetChild("list").asList;
                gList.Center();
                gList.SetVirtual();
                gList.itemRenderer = (int index, GObject obj) =>
                {
                    var com = obj.asCom;
                    com.GetChild("name").asTextField.text = PlotUISettings.Instance.dialogueContents[index].talker_name;
                    com.GetChild("text").asTextField.text = PlotUISettings.Instance.dialogueContents[index].talker_text;
                };
                gList.numItems = PlotUISettings.Instance.dialogueContents.Count;
                gList.ScrollToView(gList.numItems - 1);
                //Debug.Log(PlotUISettings.Instance.dialogueContents.Count);

                GButton gButton = com.GetChild("close_button").asButton;
                gButton.onClick.Set(() => {
                    com.Dispose();
                });
                gButton.GetChild("text").asTextField.text = "X";
            });

            //��Ч�ؼ�֡�����ص�
            Transition trans = com.GetTransition("enter_plot");
            trans.SetHook("full_black", () =>
            {
                foreach (var child in PlotUISettings.Instance.dialogueRoot.GetChildren())
                {
                    child.visible = true;
                }
            });

            trans.SetHook("done", () =>
            {
                com.Dispose();
            });
        }

        public override void OnUpdate()
        {

        }

        public override bool IsFinished()
        {
            return true;
        }
    }
}