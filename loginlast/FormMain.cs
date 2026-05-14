using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace loginlast
{
    public partial class FormMain : Form
    {
        private FlowLayoutPanel flowPanel;
        private ProductCard selectedCard = null;

        public FormMain()
        {
            InitializeComponent();
            CreateFlowPanel();
        }

        private void CreateFlowPanel()
        {
            flowPanel = new FlowLayoutPanel
            {
                Location = new Point(12, 60),
                Size = new Size(776, 320),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.White,
                Padding = new Padding(5)
            };

            this.Controls.Remove(dataGridView1);
            this.Controls.Add(flowPanel);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadUserInfo();
            LoadComboBoxes();        // ← теперь метод существует
            LoadSortOptions();
            LoadProducts();
        }

        private void LoadUserInfo()
        {
            var user = CurrentUser.Instance;
            if (user != null)
            {
                lblUserInfo.Text = $"{user.FullName} ({user.Role})";
                lblUserInfo.ForeColor = Color.DarkBlue;

                bool isAdmin = user.Role == "Администратор";

                bool canEdit = !(user.Role == "Гость" || user.Role == "Авторизированный клиент");

                txtSearch.Enabled = canEdit;
                cmbCategory.Enabled = canEdit;
                cmbManufacture.Enabled = canEdit;
                cmbSupplier.Enabled = canEdit;
                cmbSort.Enabled = canEdit;

                btnAdd.Visible = isAdmin;
                btnEdit.Visible = isAdmin;
                btnDelete.Visible = isAdmin;
            }
        }

        private void LoadComboBoxes()
        {
            LoadCombo(cmbCategory, "category", "categoryid", "categoryname");
            LoadCombo(cmbManufacture, "manufacturer", "manufacturerid", "manufacturername");
            LoadCombo(cmbSupplier, "supplier", "supplierid", "suppliername");
        }

        private void LoadCombo(ComboBox cmb, string table, string idField, string nameField)
        {
            string connStr = "host=localhost;port=5432;username=postgres;password=navatak_21;database=postgres";
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string query = $"SELECT {idField}, {nameField} FROM \"OBYV\".\"{table}\" ORDER BY {nameField}";
                using (var da = new NpgsqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    DataRow row = dt.NewRow();
                    row[idField] = DBNull.Value;
                    row[nameField] = "Все";
                    dt.Rows.InsertAt(row, 0);

                    cmb.DisplayMember = nameField;
                    cmb.ValueMember = idField;
                    cmb.DataSource = dt;
                }
            }
        }

        private void LoadSortOptions()
        {
            cmbSort.Items.Clear();
            cmbSort.Items.AddRange(new string[]
            {
                "По умолчанию",
                "По цене (возрастание)",
                "По цене (убывание)",
                "По наличию (возрастание)",
                "По наличию (убывание)"
            });
            cmbSort.SelectedIndex = 0;
        }

        private void LoadProducts(string search = "")
        {
            flowPanel.Controls.Clear();
            selectedCard = null;

            string connStr = "host=localhost;port=5432;username=postgres;password=navatak_21;database=postgres";

            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        p.productarticlenumber AS Артикул,
                        pn.name AS Название,
                        m.manufacturername AS Производитель,
                        s.suppliername AS Поставщик,
                        c.categoryname AS Категория,
                        p.productcost AS Цена,
                        p.productdiscountamount AS Скидка,
                        p.productquantityinStock AS На_складе,
                        p.unitofmeasurement AS Единица,
                        p.productdescription,
                        p.productphoto
                    FROM ""OBYV"".""product"" p
                    LEFT JOIN ""OBYV"".""product_names"" pn ON p.productname = pn.nameid
                    LEFT JOIN ""OBYV"".""manufacturer"" m ON p.productmanufacturer = m.manufacturerid
                    LEFT JOIN ""OBYV"".""supplier"" s ON p.productsupplier = s.supplierid
                    LEFT JOIN ""OBYV"".""category"" c ON p.productcategory = c.categoryid
                    WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(search) && txtSearch.Enabled)
                    query += " AND (pn.name ILIKE @search OR p.productarticlenumber ILIKE @search)";

                if (cmbCategory.Enabled && cmbCategory.SelectedValue != null && cmbCategory.SelectedValue != DBNull.Value)
                    query += " AND p.productcategory = @category";

                if (cmbManufacture.Enabled && cmbManufacture.SelectedValue != null && cmbManufacture.SelectedValue != DBNull.Value)
                    query += " AND p.productmanufacturer = @manufacture";

                if (cmbSupplier.Enabled && cmbSupplier.SelectedValue != null && cmbSupplier.SelectedValue != DBNull.Value)
                    query += " AND p.productsupplier = @supplier";

                // СОРТИРОВКА
                string orderBy = " ORDER BY p.productarticlenumber";
                switch (cmbSort.SelectedIndex)
                {
                    case 1: orderBy = " ORDER BY p.productcost ASC"; break;
                    case 2: orderBy = " ORDER BY p.productcost DESC"; break;
                    case 3: orderBy = " ORDER BY p.productquantityinStock ASC"; break;
                    case 4: orderBy = " ORDER BY p.productquantityinStock DESC"; break;
                }
                query += orderBy;

                using (var da = new NpgsqlDataAdapter(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(search) && txtSearch.Enabled)
                        da.SelectCommand.Parameters.AddWithValue("search", $"%{search}%");

                    if (cmbCategory.Enabled && cmbCategory.SelectedValue != DBNull.Value)
                        da.SelectCommand.Parameters.AddWithValue("category", cmbCategory.SelectedValue);

                    if (cmbManufacture.Enabled && cmbManufacture.SelectedValue != DBNull.Value)
                        da.SelectCommand.Parameters.AddWithValue("manufacture", cmbManufacture.SelectedValue);

                    if (cmbSupplier.Enabled && cmbSupplier.SelectedValue != DBNull.Value)
                        da.SelectCommand.Parameters.AddWithValue("supplier", cmbSupplier.SelectedValue);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        ProductCard card = new ProductCard();
                        card.LoadData(row);
                        card.Width = flowPanel.ClientSize.Width - 30;
                        card.Margin = new Padding(5, 8, 5, 8);
                        card.Clicked += Card_Clicked;

                        flowPanel.Controls.Add(card);
                    }
                }
            }
        }

        private void Card_Clicked(object sender, EventArgs e)
        {
            if (selectedCard != null)
                selectedCard.SelectCard(false);

            selectedCard = sender as ProductCard;
            if (selectedCard != null)
                selectedCard.SelectCard(true);
        }

        // ==================== Обработчики ====================

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts(txtSearch.Text.Trim());
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Enabled)
                LoadProducts(txtSearch.Text.Trim());
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadProducts(txtSearch.Text.Trim());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cmbCategory.SelectedIndex = 0;
            cmbManufacture.SelectedIndex = 0;
            cmbSupplier.SelectedIndex = 0;
            cmbSort.SelectedIndex = 0;
            txtSearch.Clear();
            LoadProducts();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new FormProductEdit())
            {
                if (form.ShowDialog() == DialogResult.OK)
                    LoadProducts(txtSearch.Text.Trim());
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedCard == null)
            {
                MessageBox.Show("Выберите карточку товара для редактирования", "Внимание");
                return;
            }

            using (var form = new FormProductEdit(selectedCard.Article))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    LoadProducts(txtSearch.Text.Trim());
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedCard == null)
            {
                MessageBox.Show("Выберите товар для удаления", "Внимание");
                return;
            }

            if (MessageBox.Show($"Вы действительно хотите удалить товар\n{selectedCard.Article}?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            try
            {
                string connStr = "host=localhost;port=5432;username=postgres;password=navatak_21;database=postgres";

                using (var conn = new NpgsqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"DELETE FROM ""OBYV"".""product"" WHERE productarticlenumber = @article";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("article", selectedCard.Article);
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Товар успешно удалён!", "Успех");
                            LoadProducts(txtSearch.Text.Trim());
                            selectedCard = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении:\n" + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Выйти из системы?", "Выход", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
                Application.Restart();
            }
        }
    }
}