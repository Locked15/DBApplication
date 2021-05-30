using System;
using System.Windows;
using System.Collections.Generic;

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
        #region Поля класса.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Поле, содержащее текущего пользователя.
        /// </summary>
        User currentUser;

        /// <summary>
        /// Список, содержащий все имущество текущего владельца. Нужен для работы привязки данных.
        /// </summary>
        List<UserProperty> currentUserProperties = new List<UserProperty>(1);
        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public MainWindow(User user)
        {
            currentUser = user;

            InitializeComponent();
        }

        #region Работа с Таблицей.
        //—————————————————————————————————————————————————————————————————————————————————————————

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

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Прочие события.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "ExitAccountButton". Совершает выход из текущего аккаунта.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ExitAccountButton_Click(object sender, RoutedEventArgs e)
        {
            GetStartedWindow newWindow = new GetStartedWindow();

            newWindow.Show();
            Close();
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "InfoAboutProgramButton". Выводит информацию о программе.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргуметы события.</param>
        private void InfoAboutProgramButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Событие, возникающее при загрузке окна. 
        /// Нужно для подгрузки данных в таблицу из Базы Данных.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            currentUserProperties = DataBaseWork.ReadCommoditiesTable(currentUser.Name);

            Sheet.ItemsSource = currentUserProperties;
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion
    }
}
