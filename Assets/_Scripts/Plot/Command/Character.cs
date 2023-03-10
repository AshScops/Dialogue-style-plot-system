using FairyGUI;
using plot_command_executor;
using System;
using UnityEngine;

namespace plot_command
{
    public class Character : CommandBase
    {
        [field: SerializeField]
        public Sprite character1;
        [field: SerializeField]
        public Sprite character2;
        [field: SerializeField]
        public Focus focus;

        public enum Focus
        {
            character1 = 0,
            character2
        };

        private int cnt;
        private Sprite[] images;

        public override void Execute()
        {
            cnt = (character1 ? 1 : 0) + (character2 ? 1 : 0);
            Debug.Log("character cnt : " + cnt);
            if (cnt == 0) return;

            images = new Sprite[cnt];
            images[0] = (character1 ? character1 : character2);
            if (cnt == 2) images[1] = character2;

            GComponent com = PlotUISettings.Instance.dialogueRoot.GetChild("character").asCom;
            GList gList = com.GetChild("list").asList;
            gList.itemRenderer = RenderListItem;
            gList.numItems = cnt;
        }

        private void RenderListItem(int index, GObject obj)
        {
            GComponent com = obj.asCom;
            GLoader gLoader = com.GetChild("image").asLoader;
            //Debug.Log(images[index].rect);
            //Debug.Log(900f / images[index].rect.width + "    " + 900f / images[index].rect.height);
            Rect region = new Rect(0, 0, 900, 900);
            //Rect uvRect = new Rect(images[index].rect);
            //uvRect.width = region.width / images[index].texture.width;
            //uvRect.height = region.height / images[index].texture.height;
            gLoader.texture = new NTexture(images[index].texture, region);

            if (cnt == 2 && focus == (Focus)Enum.ToObject(typeof(Focus), index)|| cnt == 1)
            {
                Transition trans = com.GetTransition("focus");
                trans.Play();
            }
            else
            {
                Transition trans = com.GetTransition("leave_focus");
                trans.Play();
            }
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