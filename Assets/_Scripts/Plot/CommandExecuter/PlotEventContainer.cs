using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace plot_command_executor
{
    public class PlotEventContainer : Singleton<PlotEventContainer>
    {
        /// <summary>
        /// ÿ��Ҫ������͵����
        /// </summary>
        public UnityEvent plotBegin = new UnityEvent();

        /// <summary>
        /// ÿ�ξ�����������䶯��������Զ������������б�Ҫ���������������ע��һЩ����
        /// </summary>
        public UnityEvent plotEnd = new UnityEvent();

        /// <summary>
        /// ��ʼ���ķ������贫��UI��Ԥ���壬Ӧ����CommandExecuter�ļ����¡�ȫ��ֻ�����һ�μ���
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
    }

}
