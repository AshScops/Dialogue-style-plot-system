using FairyGUI;
using plot_command_executor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace plot_command_executor_fgui
{
    public class PlotUISettings : Singleton<PlotUISettings>
    {
        public PlotUISettings()
        {
            dialogueRoot = CommandSender.Instance.GetComponent<UIPanel>().ui;
            dialogueRoot.MakeFullScreen();
            GLoader loader = dialogueRoot.GetChild("n4").asLoader;
            //TODO:��������Ļ�ֱ��������
            loader.SetSize(pixelSize.x, pixelSize.y);
        }
        public GComponent dialogueRoot;
        public float typingEffectTimeDevision = 0.8f;
        public Vector2 pixelSize = new Vector2(1920, 1080);

        public List<int> playerDecisions = new List<int>();

    }

}
