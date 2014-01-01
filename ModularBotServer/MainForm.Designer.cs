/*
 * User: Nick
 * Date: 26/12/2013
 * Time: 7:16 PM
 * 
 */
namespace ModularBotServer
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.PluginsLabel = new System.Windows.Forms.Label();
			this.PluginsList = new System.Windows.Forms.ListBox();
			this.ConsoleLabel = new System.Windows.Forms.Label();
			this.ConsoleBox = new System.Windows.Forms.RichTextBox();
			this.StartBTN = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// PluginsLabel
			// 
			this.PluginsLabel.AutoSize = true;
			this.PluginsLabel.Location = new System.Drawing.Point(269, 52);
			this.PluginsLabel.Name = "PluginsLabel";
			this.PluginsLabel.Size = new System.Drawing.Size(64, 20);
			this.PluginsLabel.TabIndex = 0;
			this.PluginsLabel.Text = "Plugins:";
			// 
			// PluginsList
			// 
			this.PluginsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.PluginsList.FormattingEnabled = true;
			this.PluginsList.ItemHeight = 20;
			this.PluginsList.Location = new System.Drawing.Point(269, 75);
			this.PluginsList.Name = "PluginsList";
			this.PluginsList.Size = new System.Drawing.Size(232, 164);
			this.PluginsList.TabIndex = 1;
			// 
			// ConsoleLabel
			// 
			this.ConsoleLabel.Location = new System.Drawing.Point(12, 52);
			this.ConsoleLabel.Name = "ConsoleLabel";
			this.ConsoleLabel.Size = new System.Drawing.Size(100, 23);
			this.ConsoleLabel.TabIndex = 2;
			this.ConsoleLabel.Text = "Console:";
			// 
			// ConsoleBox
			// 
			this.ConsoleBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.ConsoleBox.Location = new System.Drawing.Point(12, 75);
			this.ConsoleBox.Name = "ConsoleBox";
			this.ConsoleBox.Size = new System.Drawing.Size(251, 164);
			this.ConsoleBox.TabIndex = 3;
			this.ConsoleBox.Text = "";
			// 
			// StartBTN
			// 
			this.StartBTN.Location = new System.Drawing.Point(12, 12);
			this.StartBTN.Name = "StartBTN";
			this.StartBTN.Size = new System.Drawing.Size(75, 23);
			this.StartBTN.TabIndex = 4;
			this.StartBTN.Text = "Start";
			this.StartBTN.UseVisualStyleBackColor = true;
			this.StartBTN.Click += new System.EventHandler(this.StartBTNClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(513, 244);
			this.Controls.Add(this.StartBTN);
			this.Controls.Add(this.ConsoleBox);
			this.Controls.Add(this.ConsoleLabel);
			this.Controls.Add(this.PluginsList);
			this.Controls.Add(this.PluginsLabel);
			this.Name = "MainForm";
			this.Text = "Modular Bot";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button StartBTN;
		private System.Windows.Forms.RichTextBox ConsoleBox;
		private System.Windows.Forms.Label ConsoleLabel;
		private System.Windows.Forms.ListBox PluginsList;
		private System.Windows.Forms.Label PluginsLabel;
	}
}
