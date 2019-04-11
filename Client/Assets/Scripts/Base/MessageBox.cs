using UnityEngine;
using System.Collections;
using System;

namespace MessageBox
{
    public enum DialogResult
    {
        None,
        Ok,
        Cancel,
        Abort,
        Retry,
        Ignore,
        Yes,
        No
    }

    public enum Buttons
    {
        OK = 0,
        OKCancel,
        AbortRetryIgnore,
        YesNoCancel,
        YesNo,
        RetryCancel
    }

    public enum DefaultButton
    {
        Button1,
        Button2,
        Button3
    }

    public enum Icon
    {
        None,
        Hand,
        Exclamation,
        Asterisk,
        Stop,
        Error,
        Warning,
        Information
    }

    public delegate bool MessageCallback(DialogResult result);

    public class MessageItem
    {
        // Fields
        public Buttons buttons;
        public MessageCallback callback;
        public string caption;
        public DefaultButton defaultButton;
        public string message;

        // Methods
        public MessageItem()
        {
            this.message = string.Empty;
            this.caption = string.Empty;
            this.buttons = Buttons.OK;
            this.defaultButton = DefaultButton.Button1;
        }

        public MessageItem(MessageCallback call, string content, string cap, Buttons btns, DefaultButton defaultBtn)
        {
            this.message = content;
            this.caption = cap;
            this.buttons = btns;
            this.defaultButton = defaultBtn;
        }

    }
}



