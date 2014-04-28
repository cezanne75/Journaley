﻿namespace Journaley.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Part of this code was taken from: http://www.codeproject.com/Articles/29010/WinForm-ImageButton.
    /// </summary>
    public class ImageButton : PictureBox, IButtonControl
    {
        /// <summary>
        /// WM_KEYDOWN code
        /// </summary>
        private const int WMKeyDown = 0x0100;

        /// <summary>
        /// WM_KEYUP code
        /// </summary>
        private const int WMKeyUp = 0x0101;

        /// <summary>
        /// Indicates whether this button is the default button or not.
        /// </summary>
        private bool isDefault = false;

        /// <summary>
        /// Indicates whether the mouse is currently over this button.
        /// </summary>
        private bool hover = false;

        /// <summary>
        /// Indicates whether this button is currently down.
        /// </summary>
        private bool down = false;

        /// <summary>
        /// Indicates whether the space bar is held by the user.
        /// </summary>
        private bool holdingSpace = false;

        /// <summary>
        /// Gets or sets the hover image.
        /// </summary>
        /// <value>
        /// The hover image.
        /// </value>
        [Category("Appearance")]
        [Description("Image to show when the button is hovered over.")]
        public Image HoverImage { get; set; }

        /// <summary>
        /// Gets or sets down image.
        /// </summary>
        /// <value>
        /// Down image.
        /// </value>
        [Category("Appearance")]
        [Description("Image to show when the button is depressed.")]
        public Image DownImage { get; set; }

        /// <summary>
        /// Gets or sets the normal image.
        /// </summary>
        /// <value>
        /// The normal image.
        /// </value>
        [Category("Appearance")]
        [Description("Image to show when the button is not in any other state.")]
        public Image NormalImage { get; set; }

        /// <summary>
        /// Gets or sets the value returned to the parent form when the button is clicked.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
        public DialogResult DialogResult { get; set; }

        /// <summary>
        /// Notifies a control that it is the default button so that its appearance and behavior is adjusted accordingly.
        /// </summary>
        /// <param name="value">true if the control should behave as a default button; otherwise false.</param>
        public void NotifyDefault(bool value)
        {
            this.isDefault = value;
        }

        /// <summary>
        /// Generates a <see cref="E:System.Windows.Forms.Control.Click" /> event for the control.
        /// </summary>
        public void PerformClick()
        {
            this.OnClick(EventArgs.Empty);
        }

        /// <summary>
        /// Preprocesses keyboard or input messages within the message loop before they are dispatched.
        /// </summary>
        /// <param name="msg">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the message to process. The possible values are WM_KEYDOWN, WM_SYSKEYDOWN, WM_CHAR, and WM_SYSCHAR.</param>
        /// <returns>
        /// true if the message was processed by the control; otherwise, false.
        /// </returns>
        public override bool PreProcessMessage(ref Message msg)
        {
            if (msg.Msg == WMKeyUp)
            {
                if (this.holdingSpace)
                {
                    if ((int)msg.WParam == (int)Keys.Space)
                    {
                        this.OnMouseUp(null);
                        this.PerformClick();
                    }
                    else if ((int)msg.WParam == (int)Keys.Escape || (int)msg.WParam == (int)Keys.Tab)
                    {
                        this.holdingSpace = false;
                        this.OnMouseUp(null);
                    }
                }

                return true;
            }
            else if (msg.Msg == WMKeyDown)
            {
                if ((int)msg.WParam == (int)Keys.Space)
                {
                    this.holdingSpace = true;
                    this.OnMouseDown(null);
                }
                else if ((int)msg.WParam == (int)Keys.Enter)
                {
                    this.PerformClick();
                }

                return true;
            }
            else
            {
                return base.PreProcessMessage(ref msg);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.
        /// For the purpose of ImageButton, this method handles the hover event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.hover = true;
            if (this.down)
            {
                if (this.DownImage != null && this.Image != this.DownImage)
                {
                    this.Image = this.DownImage;
                }
            }
            else
            {
                this.Image = this.HoverImage != null ? this.HoverImage : this.NormalImage;
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            this.holdingSpace = false;
            this.OnMouseUp(null);

            base.OnLostFocus(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            this.hover = false;
            this.Image = this.NormalImage;

            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            this.down = true;

            if (this.DownImage != null)
            {
                this.Image = this.DownImage;
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.down = false;
            if (this.hover)
            {
                if (this.HoverImage != null)
                {
                    this.Image = this.HoverImage;
                }
            }
            else
            {
                this.Image = this.NormalImage;
            }

            base.OnMouseUp(e);
        }
    }
}
