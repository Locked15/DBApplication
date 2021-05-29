using System;
using System.Windows;
using Bool = System.Boolean;

/// <summary>
/// Область с Стартовым Окном.
/// </summary>
namespace DBApplication
{
    /// <summary>
    /// Логика взаимодействия для GetStartedWindow.xaml
    /// </summary>
    public partial class GetStartedWindow : Window
    {
        /// <summary>
        /// Поле, отвечающее за то, что все Элементы Окна прогрузились.
        /// </summary>
        Bool initialized = false;

        /// <summary>
        /// Поле, отвечающее за то, видим ли сейчас пароль или нет.
        /// </summary>
        Bool passwordIsVisible = false;

        public GetStartedWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Событие, возникающее при изменении текста в "UserNameBox". Нужно для изменения состояния кнопки входа.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserNameBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckFieldsToFill();
        }

        /// <summary>
        /// Событие, возникающее при изменении текста в "UserPassword_Box". Нужно для изменения состояния кнопки входа.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserPassword_Box_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckFieldsToFill();
        }

        /// <summary>
        /// Событие, возникающее при изменении текста в "UserPassword_HiddenBox". Нужно для изменения состояния кнопки входа.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserPassword_HiddenBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckFieldsToFill();
        }

        /// <summary>
        /// Событие, возникающее при нажатии кнопки "ShowPasswordButton". Нужно для отображения введенного пароля.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserPassword_Box.IsVisible)
            {
                passwordIsVisible = false;

                UserPassword_HiddenBox.Password = UserPassword_Box.Text;

                UserPassword_HiddenBox.Visibility = Visibility.Visible;
                UserPassword_Box.Visibility = Visibility.Hidden;
            }

            else
            {
                passwordIsVisible = true;

                UserPassword_Box.Text = UserPassword_HiddenBox.Password;

                UserPassword_Box.Visibility = Visibility.Visible;
                UserPassword_HiddenBox.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Событие, возникающее при нажатии кнопки "EnterAccountButton". Переходит в окно с информацией из Базы Данных, если аккаунт найден.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void EnterAccountButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow dataWindow = new MainWindow(DataBaseWork.ReadUserTable(UserNameBox.Text,
                passwordIsVisible ? UserPassword_Box.Text : UserPassword_HiddenBox.Password));

                dataWindow.Show();
                Close();
            }

            catch (UserNotFoundException)
            {
                MessageBox.Show("Данный пользователь не найден.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Событие, возникающее при нажатии кнопки "CreateNewAccountButton". Открывает окно регистрации нового пользователя.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Registration registerWindow = new Registration();
            registerWindow.Show();

            Close();
        }

        /// <summary>
        /// Метод для проверки полей на заполненность.
        /// </summary>
        void CheckFieldsToFill()
        {
            if (initialized)
            {
                //В данном методе использован "Тернарный Оператор" для уточнения того, отображен ли пароль или нет.
                if (String.IsNullOrEmpty(UserNameBox.Text) || (passwordIsVisible ?
                String.IsNullOrEmpty(UserPassword_Box.Text) : String.IsNullOrEmpty(UserPassword_HiddenBox.Password)))
                {
                    EnterAccountButton.IsEnabled = false;
                }

                else
                {
                    EnterAccountButton.IsEnabled = true;
                }
            }
        }
    }
}
