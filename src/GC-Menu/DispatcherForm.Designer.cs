﻿namespace GlobalContextMenu
{
    partial class DispatcherForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testAToolStripMenuItem,
            this.testBToolStripMenuItem,
            this.testCToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(104, 70);
            // 
            // testAToolStripMenuItem
            // 
            this.testAToolStripMenuItem.Name = "testAToolStripMenuItem";
            this.testAToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.testAToolStripMenuItem.Text = "TestA";
            // 
            // testBToolStripMenuItem
            // 
            this.testBToolStripMenuItem.Name = "testBToolStripMenuItem";
            this.testBToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.testBToolStripMenuItem.Text = "TestB";
            // 
            // testCToolStripMenuItem
            // 
            this.testCToolStripMenuItem.Name = "testCToolStripMenuItem";
            this.testCToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.testCToolStripMenuItem.Text = "TestC";
            this.testCToolStripMenuItem.ToolTipText = "ttt";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "Exit";
            this.notifyIcon1.Visible = true;
            // 
            // DispatcherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 217);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DispatcherForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        System.Windows.Forms.ToolStripMenuItem testAToolStripMenuItem;
        System.Windows.Forms.ToolStripMenuItem testBToolStripMenuItem;
        System.Windows.Forms.ToolStripMenuItem testCToolStripMenuItem;
        System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}