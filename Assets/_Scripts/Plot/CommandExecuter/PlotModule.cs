using plot_command_creator;
using plot_command_executor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace plot_module
{
    public class PlotModule : Singleton<PlotModule>
    {
        /// <summary>
        /// ÿ��Ҫ������͵����
        /// </summary>
        public UnityEvent plotBegin = new UnityEvent();

        /// <summary>
        /// ÿ�ξ�����������䶯��������Զ������������б�Ҫ������Զ���ע��һЩ����
        /// </summary>
        public UnityEvent plotEnd = new UnityEvent();

        /// <summary>
        /// ��ʼ���ķ������贫��UI��Ԥ���壬��Ӧ����CommandExecuter�ļ����¡�ȫ��ֻ���ʼ��һ�μ��ɣ������־ô���
        /// </summary>
        public void PlotInit(GameObject ui_prefab)
        {
            if(ui_prefab == null)
            {
                Debug.Log("����UIԤ�����Ƿ�󶨵���PlotEventContainer�ű��ϡ�");
                return;
            }

            GameObject.Instantiate(ui_prefab);
            PlotUISettings.Instance.dialogueRoot.visible = false;
        }

        /// <summary>
        /// �ھ��鿪ʼǰ�����ú���ľ��������ļ�
        /// </summary>
        /// <param name="plotConfig"></param>
        public void SetPlotConfig(CommandConfig plotConfig)
        {
            CommandSender.Instance.commandConfig = plotConfig;
        }

        /// <summary>
        /// ������Ļ�ֱ��ʴ�С
        /// </summary>
        /// <param name="screenPixel"></param>
        public void SetPixel(Vector2 screenPixel)
        {
            PlotUISettings.Instance.pixelSize = screenPixel;
        }

        /// <summary>
        /// ����Ч����ʱ�������Ƽ���ΧΪ 0.01f - 0.1f
        /// </summary>
        /// <param name="typingEffectTimeDevision"></param>
        public void SetTypingTimeDevision(float t)
        {
            PlotUISettings.Instance.typingEffectTimeDevision = t;
        }

        public List<int> GetPlayerDecisions()
        {
            return PlotUISettings.Instance.playerDecisions;
        }
    }

}
