using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace loginlast
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadUserInfo();
            LoadComboBoxes();
            LoadSortOptions();
            loadProduct();
        }

        private void LoadUserInfo()
        {
            var user = CurrentUser.Instance;
            if (user != null)
            {
                lblUserInfo.Text = $"{user.FullName} ({user.Role})";

                bool isAdmin = user.Role == "Администратор";
                bool isManagerOrAdmin = isAdmin || user.Role == "Менеджер";

                btnAdd.Visible = isAdmin;
                btnEdit.Visible = isAdmin;
                btnOrders.Visible = isManagerOrAdmin;
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
            cmbSort.Items.AddRange(new string[]
            {
                "По умолчанию",
                "По цене (возр.)",
                "По цене (убыв.)",
                "По наличию (возр.)",
                "По наличию (убыв.)"
            });
            cmbSort.SelectedIndex = 0;
        }

        private void loadProduct(string search = "")
        {
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
                        p.productquantityinStock AS На_складе
                    FROM ""OBYV"".""product"" p
                    LEFT JOIN ""OBYV"".""product_names"" pn ON p.productname = pn.nameid
                    LEFT JOIN ""OBYV"".""manufacturer"" m ON p.productmanufacturer = m.manufacturerid
                    LEFT JOIN ""OBYV"".""supplier"" s ON p.productsupplier = s.supplierid
                    LEFT JOIN ""OBYV"".""category"" c ON p.productcategory = c.categoryid
                    WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query += " AND (pn.name ILIKE @search OR p.productarticlenumber ILIKE @search)";
                }

                query += " ORDER BY p.productarticlenumber";

                using (var da = new NpgsqlDataAdapter(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(search))
                        da.SelectCommand.Parameters.AddWithValue("search", $"%{search}%");

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    ColorDataGridRows();
                }
            }
        }

        private void ColorDataGridRows()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                int stock = Convert.ToInt32(row.Cells["На_складе"].Value ?? 0);
                int discount = Convert.ToInt32(row.Cells["Скидка"].Value ?? 0);

                if (stock == 0)
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                else if (discount >= 15)
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadProduct(txtSearch.Text.Trim());
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            loadProduct(txtSearch.Text.Trim());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadProduct(txtSearch.Text.Trim());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new FormProductEdit())
            {
                if (form.ShowDialog() == DialogResult.OK)
                    loadProduct(txtSearch.Text.Trim());
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите товар для редактирования", "Внимание");
                return;
            }

            string article = dataGridView1.CurrentRow.Cells["Артикул"].Value.ToString();

            using (var form = new FormProductEdit(article))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    loadProduct(txtSearch.Text.Trim());
            }
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Форма заказов будет добавлена позже", "В разработке");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Выйти из системы?", "Выход", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
                Application.Restart();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = dataGridView1.CurrentRow != null;
        }
    }
}