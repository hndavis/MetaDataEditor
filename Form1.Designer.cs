namespace MetaDataEditor
{
	partial class Form1
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pbLoad = new System.Windows.Forms.Button();
			this.gridControl1 = new DevExpress.XtraGrid.GridControl();
			this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
			((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// pbLoad
			// 
			this.pbLoad.Location = new System.Drawing.Point(626, 328);
			this.pbLoad.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.pbLoad.Name = "pbLoad";
			this.pbLoad.Size = new System.Drawing.Size(56, 19);
			this.pbLoad.TabIndex = 0;
			this.pbLoad.Text = "Load Xml";
			this.pbLoad.UseVisualStyleBackColor = true;
			this.pbLoad.Click += new System.EventHandler(this.button1_Click);
			// 
			// gridControl1
			// 
			this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.gridControl1.Location = new System.Drawing.Point(0, 25);
			this.gridControl1.MainView = this.gridView1;
			this.gridControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.gridControl1.Name = "gridControl1";
			this.gridControl1.Size = new System.Drawing.Size(682, 298);
			this.gridControl1.TabIndex = 1;
			this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
			this.gridControl1.Click += new System.EventHandler(this.gridControl1_Click);
			// 
			// gridView1
			// 
			this.gridView1.GridControl = this.gridControl1;
			this.gridView1.Name = "gridView1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(711, 349);
			this.Controls.Add(this.gridControl1);
			this.Controls.Add(this.pbLoad);
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button pbLoad;
		private DevExpress.XtraGrid.GridControl gridControl1;
		private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
	}
}

