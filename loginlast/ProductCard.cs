using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace loginlast
{
    public partial class ProductCard : UserControl
    {
        public string Article { get; private set; } = "";

        // Событие при клике на карточку
        public event EventHandler Clicked;

        public ProductCard()
        {
            InitializeComponent();
            this.Click += ProductCard_Click;

            // Чтобы клик работал по всем элементам внутри карточки
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Click += ProductCard_Click;
            }
        }

        public void LoadData(DataRow row)
        {
            Article = row["Артикул"].ToString();

            string category = row["Категория"].ToString();
            string name = row["Название"].ToString();
            string description = row["productdescription"]?.ToString() ?? "";
            string manufacturer = row["Производитель"].ToString();
            string supplier = row["Поставщик"].ToString();
            decimal price = Convert.ToDecimal(row["Цена"]);
            int discount = Convert.ToInt32(row["Скидка"]);
            int stock = Convert.ToInt32(row["На_складе"]);
            string unit = row["Единица"].ToString();
            string photoFile = row["productphoto"]?.ToString() ?? "";

            // Формируем текст
            lblInfo.Text =
                $"**{category} | {name}**\n\n" +
                $"Описание товара: {description}\n" +
                $"Производитель: {manufacturer}\n" +
                $"Поставщик: {supplier}\n" +
                $"Цена: {price:N2} ₽\n" +
                $"Единица измерения: {unit}\n" +
                $"Количество на складе: {stock}";

            // Отображение скидки
            lblDiscount.Text = discount > 0 ? $"{discount}%" : "";
            lblDiscount.ForeColor = Color.Red;
            lblDiscount.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);

            // Загрузка изображения
            LoadPhoto(photoFile);

            // Цвет карточки
            if (discount > 15)
                this.BackColor = ColorTranslator.FromHtml("#2E8B57");
            else if (stock == 0)
                this.BackColor = Color.LightBlue;
            else
                this.BackColor = Color.White;
        }

        private void ProductCard_Click(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        // Выделение/снятие выделения карточки
        public void SelectCard(bool selected)
        {
            if (selected)
                this.BackColor = Color.LightSkyBlue;
            else
            {
                // Возвращаем исходный цвет (можно улучшить позже)
                if (BackColor == Color.LightSkyBlue)
                    this.BackColor = Color.White;
            }
        }

        private void LoadPhoto(string photoFileName)
        {
            string defaultPicture = Path.Combine(Application.StartupPath, "images\\picture.png");

            if (string.IsNullOrWhiteSpace(photoFileName))
            {
                pbPhoto.Image = Image.FromFile(defaultPicture);
                return;
            }

            string imagePath = Path.Combine(Application.StartupPath, "images", photoFileName);

            if (File.Exists(imagePath))
            {
                try
                {
                    pbPhoto.Image = Image.FromFile(imagePath);
                }
                catch
                {
                    pbPhoto.Image = Image.FromFile(defaultPicture);
                }
            }
            else
            {
                pbPhoto.Image = Image.FromFile(defaultPicture);
            }
        }
    }
}