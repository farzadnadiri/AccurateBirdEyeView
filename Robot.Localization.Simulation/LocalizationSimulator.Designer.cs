namespace Robot.Localization.Simulation
{
    partial class LocalizationSimulator
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ViewerControll = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ViewerControll)).BeginInit();
            this.SuspendLayout();
            // 
            // ViewerControll
            // 
            this.ViewerControll.BackColor = System.Drawing.Color.ForestGreen;
            this.ViewerControll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ViewerControll.Location = new System.Drawing.Point(0, 0);
            this.ViewerControll.Name = "ViewerControll";
            this.ViewerControll.Size = new System.Drawing.Size(450, 300);
            this.ViewerControll.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ViewerControll.TabIndex = 0;
            this.ViewerControll.TabStop = false;
            this.ViewerControll.SizeChanged += new System.EventHandler(this.ViewerControll_SizeChanged);
            this.ViewerControll.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ViewerControll_MouseClick);
            // 
            // LocalizationSimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ViewerControll);
            this.Name = "LocalizationSimulator";
            this.Size = new System.Drawing.Size(450, 300);
            ((System.ComponentModel.ISupportInitialize)(this.ViewerControll)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ViewerControll;
    }
}
