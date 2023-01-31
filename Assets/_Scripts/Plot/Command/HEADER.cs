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

            GButton skipButton = PlotUISettings.Instance.dialogueRoot.GetChild("skip_button").asButton;
            if (is_skippable)
                skipButton.visible = false;
            else
                skipButton.visible = true;

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