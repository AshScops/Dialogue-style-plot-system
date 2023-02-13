using plot_command_creator;
using plot_command_executor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace plot_module
{
    public class PlotModule : Singleton<PlotModule>
    {
        /// <summary>
        /// ���ⲿ���ý������
        /// </summary>
        public UnityEvent plotBegin = new UnityEvent();

        /// <summary>
        /// ÿ�ξ�����������䶯��������Զ���������ⲿ���Զ���ע��һЩ�ص�
        /// </summary>
        public UnityEvent plotEnd = new UnityEvent();

        /// <summary>
        /// ȫ�ֳ�ʼ��һ�μ��ɣ�����־ô���
        /// </summary>
        public void Init()
        {
            PlotUISettings.Instance.Init();
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
        /// �ھ��鿪ʼǰ�����ú���ľ��������ļ���·����Resources�ļ�����
        /// </summary>
        /// <param name="plotConfig"></param>
        public void SetPlotConfig(string plotConfigPath)
        {
            CommandConfig commandConfig = Resources.Load<CommandConfig>(plotConfigPath);
            if (commandConfig == null)
            {
                Debug.LogError("SetPlotConfig : path invalid.");
                return;
            }
            SetPlotConfig(commandConfig);
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
