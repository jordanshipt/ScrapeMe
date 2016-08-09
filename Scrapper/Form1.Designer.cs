namespace Scrapper
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutUsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnStart = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.wbMain = new System.Windows.Forms.WebBrowser();
            this.btnGetCategories = new System.Windows.Forms.Button();
            this.dgvProduct = new System.Windows.Forms.DataGridView();
            this.cSrNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSubCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cUPC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCurrentPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOriginalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cImageURL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbxGetCategories = new System.Windows.Forms.CheckBox();
            this.cbxGetProducts = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1172, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToExcelToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exportToExcelToolStripMenuItem
            // 
            this.exportToExcelToolStripMenuItem.Name = "exportToExcelToolStripMenuItem";
            this.exportToExcelToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.exportToExcelToolStripMenuItem.Text = "Export to Excel";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutUsToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // aboutUsToolStripMenuItem
            // 
            this.aboutUsToolStripMenuItem.Name = "aboutUsToolStripMenuItem";
            this.aboutUsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutUsToolStripMenuItem.Text = "AboutUs";
            this.aboutUsToolStripMenuItem.Click += new System.EventHandler(this.aboutUsToolStripMenuItem_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 27);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(111, 24);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start Scrapper";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // wbMain
            // 
            this.wbMain.Location = new System.Drawing.Point(12, 57);
            this.wbMain.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbMain.Name = "wbMain";
            this.wbMain.ScriptErrorsSuppressed = true;
            this.wbMain.Size = new System.Drawing.Size(1148, 347);
            this.wbMain.TabIndex = 2;
            // 
            // btnGetCategories
            // 
            this.btnGetCategories.Location = new System.Drawing.Point(1049, 26);
            this.btnGetCategories.Name = "btnGetCategories";
            this.btnGetCategories.Size = new System.Drawing.Size(111, 24);
            this.btnGetCategories.TabIndex = 4;
            this.btnGetCategories.Text = "Get URL";
            this.btnGetCategories.UseVisualStyleBackColor = true;
            this.btnGetCategories.Click += new System.EventHandler(this.btnGetCategories_Click);
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cSrNumber,
            this.cName,
            this.cCategory,
            this.cSubCategory,
            this.cUPC,
            this.cSize,
            this.cCurrentPrice,
            this.cOriginalPrice,
            this.cImageURL});
            this.dgvProduct.Location = new System.Drawing.Point(12, 57);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.ReadOnly = true;
            this.dgvProduct.Size = new System.Drawing.Size(1148, 347);
            this.dgvProduct.TabIndex = 5;
            // 
            // cSrNumber
            // 
            this.cSrNumber.DataPropertyName = "sSrNumber";
            this.cSrNumber.HeaderText = "Sr No";
            this.cSrNumber.Name = "cSrNumber";
            this.cSrNumber.ReadOnly = true;
            // 
            // cName
            // 
            this.cName.DataPropertyName = "sName";
            this.cName.HeaderText = "Name";
            this.cName.Name = "cName";
            this.cName.ReadOnly = true;
            // 
            // cCategory
            // 
            this.cCategory.DataPropertyName = "sCategory";
            this.cCategory.HeaderText = "Category";
            this.cCategory.Name = "cCategory";
            this.cCategory.ReadOnly = true;
            // 
            // cSubCategory
            // 
            this.cSubCategory.DataPropertyName = "sSubCategory";
            this.cSubCategory.HeaderText = "Sub Category";
            this.cSubCategory.Name = "cSubCategory";
            this.cSubCategory.ReadOnly = true;
            // 
            // cUPC
            // 
            this.cUPC.DataPropertyName = "sUPC";
            this.cUPC.HeaderText = "UPC";
            this.cUPC.Name = "cUPC";
            this.cUPC.ReadOnly = true;
            // 
            // cSize
            // 
            this.cSize.DataPropertyName = "sSize";
            this.cSize.HeaderText = "Size";
            this.cSize.Name = "cSize";
            this.cSize.ReadOnly = true;
            // 
            // cCurrentPrice
            // 
            this.cCurrentPrice.DataPropertyName = "sCurrentPrice";
            this.cCurrentPrice.HeaderText = "Current Price";
            this.cCurrentPrice.Name = "cCurrentPrice";
            this.cCurrentPrice.ReadOnly = true;
            // 
            // cOriginalPrice
            // 
            this.cOriginalPrice.DataPropertyName = "sOriginalPrice";
            this.cOriginalPrice.HeaderText = "Original Price";
            this.cOriginalPrice.Name = "cOriginalPrice";
            this.cOriginalPrice.ReadOnly = true;
            // 
            // cImageURL
            // 
            this.cImageURL.DataPropertyName = "sOriginalPrice";
            this.cImageURL.HeaderText = "Image URL";
            this.cImageURL.Name = "cImageURL";
            this.cImageURL.ReadOnly = true;
            // 
            // cbxGetCategories
            // 
            this.cbxGetCategories.AutoSize = true;
            this.cbxGetCategories.Location = new System.Drawing.Point(130, 28);
            this.cbxGetCategories.Name = "cbxGetCategories";
            this.cbxGetCategories.Size = new System.Drawing.Size(96, 17);
            this.cbxGetCategories.TabIndex = 6;
            this.cbxGetCategories.Text = "Get Categories";
            this.cbxGetCategories.UseVisualStyleBackColor = true;
            // 
            // cbxGetProducts
            // 
            this.cbxGetProducts.AutoSize = true;
            this.cbxGetProducts.Location = new System.Drawing.Point(232, 28);
            this.cbxGetProducts.Name = "cbxGetProducts";
            this.cbxGetProducts.Size = new System.Drawing.Size(88, 17);
            this.cbxGetProducts.TabIndex = 7;
            this.cbxGetProducts.Text = "Get Products";
            this.cbxGetProducts.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1172, 416);
            this.Controls.Add(this.cbxGetProducts);
            this.Controls.Add(this.cbxGetCategories);
            this.Controls.Add(this.btnGetCategories);
            this.Controls.Add(this.wbMain);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.dgvProduct);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "PUBLIX DATA";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutUsToolStripMenuItem;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.WebBrowser wbMain;
        private System.Windows.Forms.Button btnGetCategories;
        private System.Windows.Forms.DataGridView dgvProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSrNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSubCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn cUPC;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCurrentPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOriginalPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn cImageURL;
        private System.Windows.Forms.CheckBox cbxGetCategories;
        private System.Windows.Forms.CheckBox cbxGetProducts;
    }
}

