using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace loginlast
{
    public partial class FormProductEdit : Form
    {
        private string originalArticle = null;

        public FormProductEdit()
        {
            InitializeComponent();
        }

        public FormProductEdit(string article) : this()
        {
            originalArticle = article;
            this.Text = "Редактирование товара";
        }

        private void FormProductEdit_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();

            if (originalArticle != null)
                LoadProductData(originalArticle);
            else
                txtArticle.ReadOnly = false;
        }

        private void LoadComboBoxes()
        {
            LoadCombo(cmbCategory, "category", "categoryid", "categoryname");
            LoadCombo(cmbManufacturer, "manufacturer", "manufacturerid", "manufacturername");
            LoadCombo(cmbSupplier, "supplier", "supplierid", "suppliername");

            cmbUnit.Items.AddRange(new string[] { "шт", "пара", "коробка", "упаковка" });
            cmbUnit.SelectedIndex = 0;
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
                    cmb.DisplayMember = nameField;
                    cmb.ValueMember = idField;
                    cmb.DataSource = dt;
                }
            }
        }

        private void LoadProductData(string article)
        {
            string connStr = "host=localhost;port=5432;username=postgres;password=navatak_21;database=postgres";

            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        p.productarticlenumber, 
                        pn.name, 
                        p.productcategory, 
                        p.productmanufacturer, 
                        p.productsupplier,
                        p.productcost,
                        p.productdiscountamount,
                        p.productquantityinStock,
                        p.productdescription,
                        p.unitofmeasurement
                    FROM ""OBYV"".""product"" p
                    LEFT JOIN ""OBYV"".""product_names"" pn ON p.productname = pn.nameid
                    WHERE p.productarticlenumber = @article";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("article", article);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtArticle.Text = reader.GetString(0);
                            txtName.Text = reader.GetString(1);

                            if (!reader.IsDBNull(2)) cmbCategory.SelectedValue = reader.GetValue(2);
                            if (!reader.IsDBNull(3)) cmbManufacturer.SelectedValue = reader.GetValue(3);
                            if (!reader.IsDBNull(4)) cmbSupplier.SelectedValue = reader.GetValue(4);

                            txtPrice.Text = reader.GetDecimal(5).ToString("0.00");
                            txtDiscount.Text = reader.GetInt32(6).ToString();
                            txtStock.Text = reader.GetInt32(7).ToString();

                            txtDescription.Text = reader.IsDBNull(8) ? "" : reader.GetString(8);
                            cmbUnit.Text = reader.IsDBNull(9) ? "шт" : reader.GetString(9);

                            txtArticle.ReadOnly = true;
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtArticle.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Артикул и Название — обязательные поля!", "Ошибка");
                return;
            }

            try
            {
                string connStr = "host=localhost;port=5432;username=postgres;password=navatak_21;database=postgres";

                using (var conn = new NpgsqlConnection(connStr))
                {
                    conn.Open();
                    int nameId = GetOrCreateProductName(conn, txtName.Text);

                    if (originalArticle == null) // Добавление нового товара
                    {
                        string sql = @"
                    INSERT INTO ""OBYV"".""product"" 
                    (productarticlenumber, productname, productcategory, productmanufacturer, 
                     productsupplier, productcost, productdiscountamount, 
                     productquantityinStock, productdescription, unitofmeasurement)
                    VALUES 
                    (@article, @nameid, @cat, @manuf, @sup, @price, @discount, 
                     @stock, @desc, @unit)";

                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            FillProductParameters(cmd, nameId);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Товар успешно добавлен!", "Успех");
                    }
                    else // Редактирование
                    {
                        string sql = @"
                    UPDATE ""OBYV"".""product"" SET 
                        productname = @nameid,
                        productcategory = @cat,
                        productmanufacturer = @manuf,
                        productsupplier = @sup,
                        productcost = @price,
                        productdiscountamount = @discount,
                        productquantityinStock = @stock,
                        productdescription = @desc,
                        unitofmeasurement = @unit
                    WHERE productarticlenumber = @article";

                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            FillProductParameters(cmd, nameId);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Товар успешно обновлён!", "Успех");
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения:\n" + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillProductParameters(NpgsqlCommand cmd, int nameId)
        {
            cmd.Parameters.AddWithValue("article", txtArticle.Text.Trim());   // ← Это было главной ошибкой
            cmd.Parameters.AddWithValue("nameid", nameId);
            cmd.Parameters.AddWithValue("cat", cmbCategory.SelectedValue ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("manuf", cmbManufacturer.SelectedValue ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("sup", cmbSupplier.SelectedValue ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("price", decimal.Parse(txtPrice.Text));
            cmd.Parameters.AddWithValue("discount", int.Parse(txtDiscount.Text));
            cmd.Parameters.AddWithValue("stock", int.Parse(txtStock.Text));

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
                cmd.Parameters.AddWithValue("desc", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("desc", txtDescription.Text);

            cmd.Parameters.AddWithValue("unit", cmbUnit.Text);
        }

        private int GetOrCreateProductName(NpgsqlConnection conn, string name)
        {
            using (var cmd = new NpgsqlCommand("SELECT nameid FROM \"OBYV\".\"product_names\" WHERE name = @name", conn))
            {
                cmd.Parameters.AddWithValue("name", name);
                var result = cmd.ExecuteScalar();
                if (result != null)
                    return Convert.ToInt32(result);
            }

            using (var cmd = new NpgsqlCommand("INSERT INTO \"OBYV\".\"product_names\" (name) VALUES (@name) RETURNING nameid", conn))
            {
                cmd.Parameters.AddWithValue("name", name);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}