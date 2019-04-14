using UnityEngine;
using System.Collections;
using System;

namespace MessageBox{
	public enum MessageResult
    {
        None,
        Ok,        
        Yes,
        No
    }

    public enum ButtonStyle
    {
        OK,                      
        YesNo,
    }
 
	public delegate void MessageCallback(MessageResult result);

    public class MessageItem
    {
        // Fields
        public ButtonStyle buttons;
        public MessageCallback callback;
        public string caption;        
        public string message;

        // Methods
        public MessageItem()
        {
            this.message = string.Empty;
            this.caption = string.Empty;
            this.buttons = ButtonStyle.OK;   
        }

		public MessageItem(MessageCallback call, string content, string cap, ButtonStyle btns)
        {
            this.message = content;
            this.caption = cap;
            this.buttons = btns;           
        }

    }
}



