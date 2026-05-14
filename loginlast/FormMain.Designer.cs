using System.Windows.Forms;

namespace loginlast
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.cmbManufacture = new System.Windows.Forms.ComboBox();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.cmbSort = new System.Windows.Forms.ComboBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnOrders = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.lblSearch = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblManufacture = new System.Windows.Forms.Label();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.lblSort = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            // dataGridView1
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 60);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(776, 320);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);

            // lblUserInfo
            this.lblUserInfo.AutoSize = true;
            this.lblUserInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblUserInfo.Location = new System.Drawing.Point(12, 12);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(200, 20);
            this.lblUserInfo.TabIndex = 1;
            this.lblUserInfo.Text = "Пользователь";

            // Search
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(12, 400);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(45, 13);
            this.lblSearch.TabIndex = 2;
            this.lblSearch.Text = "Поиск:";

            this.txtSearch.Location = new System.Drawing.Point(60, 397);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 20);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            // ComboBoxes and labels
            this.lblCategory.AutoSize = true; this.lblCategory.Location = new System.Drawing.Point(280, 400); this.lblCategory.Text = "Категория:";
            this.cmbCategory.Location = new System.Drawing.Point(350, 397); this.cmbCategory.Size = new System.Drawing.Size(140, 21); this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;

            this.lblManufacture.AutoSize = true; this.lblManufacture.Location = new System.Drawing.Point(500, 400); this.lblManufacture.Text = "Производитель:";
            this.cmbManufacture.Location = new System.Drawing.Point(600, 397); this.cmbManufacture.Size = new System.Drawing.Size(140, 21); this.cmbManufacture.DropDownStyle = ComboBoxStyle.DropDownList;

            this.lblSupplier.AutoSize = true; this.lblSupplier.Location = new System.Drawing.Point(12, 430); this.lblSupplier.Text = "Поставщик:";
            this.cmbSupplier.Location = new System.Drawing.Point(80, 427); this.cmbSupplier.Size = new System.Drawing.Size(140, 21); this.cmbSupplier.DropDownStyle = ComboBoxStyle.DropDownList;

            this.lblSort.AutoSize = true; this.lblSort.Location = new System.Drawing.Point(280, 430); this.lblSort.Text = "Сортировка:";
            this.cmbSort.Location = new System.Drawing.Point(350, 427); this.cmbSort.Size = new System.Drawing.Size(180, 21); this.cmbSort.DropDownStyle = ComboBoxStyle.DropDownList;

            // Buttons
            this.btnFilter.Location = new System.Drawing.Point(550, 427); this.btnFilter.Size = new System.Drawing.Size(80, 25); this.btnFilter.Text = "Фильтровать"; this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);

            this.btnRefresh.Location = new System.Drawing.Point(640, 427); this.btnRefresh.Size = new System.Drawing.Size(80, 25); this.btnRefresh.Text = "Обновить"; this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            this.btnAdd.Location = new System.Drawing.Point(12, 470); this.btnAdd.Size = new System.Drawing.Size(100, 30); this.btnAdd.Text = "Добавить товар"; this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            this.btnEdit.Location = new System.Drawing.Point(120, 470); this.btnEdit.Size = new System.Drawing.Size(100, 30); this.btnEdit.Text = "Редактировать"; this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            this.btnOrders.Location = new System.Drawing.Point(230, 470); this.btnOrders.Size = new System.Drawing.Size(100, 30); this.btnOrders.Text = "Заказы"; this.btnOrders.Click += new System.EventHandler(this.btnOrders_Click);

            this.btnLogout.Location = new System.Drawing.Point(680, 470); this.btnLogout.Size = new System.Drawing.Size(100, 30); this.btnLogout.Text = "Выйти"; this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // FormMain
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 520);
            this.Controls.Add(this.lblUserInfo);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblManufacture);
            this.Controls.Add(this.cmbManufacture);
            this.Controls.Add(this.lblSupplier);
            this.Controls.Add(this.cmbSupplier);
            this.Controls.Add(this.lblSort);
            this.Controls.Add(this.cmbSort);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnOrders);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormMain";
            this.Text = "Магазин обуви - Каталог товаров";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.ComboBox cmbManufacture;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.ComboBox cmbSort;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnOrders;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblManufacture;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.Label lblSort;
    }
}