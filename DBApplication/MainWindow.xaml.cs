using System;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Область с Главным Окном.
/// </summary>
namespace DBApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Поле, содержащее текущего пользователя.
        /// </summary>
        User currentUser;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public MainWindow(User user)
        {
            InitializeComponent();

            currentUser = user;
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "ExitAccountButton". Совершает выход из текущего аккаунта.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ExitAccountButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "AddCommodityButton". Добавляет новый товар в список и Базу Данных.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void AddCommodityButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "ChangeCommodityButton". Изменяет значения товара в списке и Базе Данных.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ChangeCommodityButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "DeleteCommodityButton". Удаляет товар из таблицы и Базы Данных.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void DeleteCommodityButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "UpdateSheetButton". Обновляет таблицу, подргужая значения из Базы Данных.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UpdateSheetButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "InfoAboutProgramButton". Выводит информацию о программе.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргуметы события.</param>
        private void InfoAboutProgramButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
