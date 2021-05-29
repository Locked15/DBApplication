using System;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Область с окном Регистрации.
/// </summary>
namespace DBApplication
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        /// <summary>
        /// Поле, содержащее текущий активный экземпляр класса "Grid". 
        /// </summary>
        Grid activeGrid;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public Registration()
        {
            InitializeComponent();

            activeGrid = UserNameGrid;
        }

        #region События для перемещения по Окну.
        //—————————————————————————————————————————————————————————————————————————————————————————
        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "UserNameBlockButton".
        /// Переходит к окну ввода Имени Пользователя.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserNameBlockButton_Click(object sender, RoutedEventArgs e)
        {
            if (activeGrid != UserNameGrid)
            {
                UserNameGrid.Visibility = Visibility.Visible;

                activeGrid.Visibility = Visibility.Hidden;
            }

            activeGrid = UserNameGrid;
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "UserNameGrid_GoToPasswordBlockButton".
        /// Переходит к окну ввода Пароля.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserNameGrid_GoToPasswordBlockButton_Click(object sender, RoutedEventArgs e)
        {
            if (activeGrid != UserPasswordGrid)
            {
                UserPasswordGrid.Visibility = Visibility.Visible;

                activeGrid.Visibility = Visibility.Hidden;
            }

            activeGrid = UserPasswordGrid;
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "UserPasswordGrid_GoToGenderBlockButton".
        /// Переходит к окну выбора пола.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserPasswordGrid_GoToGenderBlockButton_Click(object sender, RoutedEventArgs e)
        {
            if (activeGrid != UserGenderGrid)
            {
                UserGenderGrid.Visibility = Visibility.Visible;

                activeGrid.Visibility = Visibility.Hidden;
            }

            activeGrid = UserGenderGrid;
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "UserGenderGrid_GoToBirthBlockButton".
        /// Переходит к окну выбора Даты Рождения.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserGenderGrid_GoToBirthBlockButton_Click(object sender, RoutedEventArgs e)
        {
            if (activeGrid != UserBirthGrid)
            {
                UserBirthGrid.Visibility = Visibility.Visible;

                activeGrid.Visibility = Visibility.Hidden;
            }

            activeGrid = UserBirthGrid;
        }
        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region События фокусировки.
        //—————————————————————————————————————————————————————————————————————————————————————————
        /// <summary>
        /// Событие, возникающее при получении элементом управления фокуса.
        /// Нужно для удаления базовой подсказки пользователю.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserNameGrid_UserNameHiddenBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Событие, возникающее при потере элементом управления фокуса.
        /// Нужно для отображения базовой подсказки пользователю.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserNameGrid_UserNameHiddenBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Событие, возникающее при получении элементом управления фокуса. Нужно для создания границ.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ElementGotFocus(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Событие, возникающее при потере элементом управления фокуса. Нужно для удаления границ у элемента.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ElementLostFocus(object sender, RoutedEventArgs e)
        {

        }
        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Прочие события.
        //—————————————————————————————————————————————————————————————————————————————————————————
        /// <summary>
        /// Событие, возникающее при попытке зарегистрировать пользователя.
        /// Проверяет значения полей и доступность создания такого пользователя.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserIsReadyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "UserPasswordGrid_ShowUserPassword".
        /// Отображает указанный пароль.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserPasswordGrid_ShowUserPassword_Click(object sender, RoutedEventArgs e)
        {

        }
        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion
    }
}
