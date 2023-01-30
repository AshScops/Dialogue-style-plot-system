using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace plot_utils
{
    public class MyGLoader : GLoader
    {
        override protected void LoadExternal()
        {
            /*
            ��ʼ�ⲿ���룬��ַ��url����
            ������ɺ����OnExternalLoadSuccess
            ����ʧ�ܵ���OnExternalLoadFailed
            ע�⣺������ⲿ���룬����������󣬵���OnExternalLoadSuccess��OnExternalLoadFailedǰ��
            �Ƚ��Ͻ����������ȼ��url�����Ƿ��Ѿ��������������ݲ������
            ������������ʾloader�Ѿ����޸��ˡ�
            ���������Ӧ�÷�������OnExternalLoadSuccess��OnExternalLoadFailed��
            */

            //����ʹ�ó�Ա����url����Sprite���뼴��,ע��FGUI��ͼ����Unity�е�ͼ��Y���෴����Ҫ���¼���Rect������ȷ����
            if (url.Length > 0)
            {
                Debug.Log("MyGLoader is running!");
                Sprite[] tSprites = Resources.LoadAll<Sprite>(url);
                Sprite tSprite = tSprites[0];
                // ��תY�� 
                Rect tShowRect = new Rect(tSprite.textureRect.x, tSprite.texture.height - tSprite.textureRect.y - tSprite.textureRect.height,
                        tSprite.textureRect.width, tSprite.textureRect.height);
                this.onExternalLoadSuccess(new NTexture(tSprite.texture, tShowRect));
            }

            this.onExternalLoadFailed();
        }


        override protected void FreeExternal(NTexture texture)
        {
            //�ͷ��ⲿ�������Դ
        }
    }

}
