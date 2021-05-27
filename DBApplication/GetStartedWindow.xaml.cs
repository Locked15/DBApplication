using System;
using System.Windows;
using Bool = System.Boolean;

namespace DBApplication
{
    /// <summary>
    /// Логика взаимодействия для GetStartedWindow.xaml
    /// </summary>
    public partial class GetStartedWindow : Window
    {
        /// <summary>
        /// Поле, содержащее вошедшего в аккаунт пользователя.
        /// </summary>
        User enteredUser;

        /// <summary>
        /// Поле, отвечающее за то, что все Элементы Окна прогрузились.
        /// </summary>
        Bool initialized = false;

        /// <summary>
        /// Поле, отвечающее за то, видим ли сейчас пароль или нет.
        /// </summary>
        Bool passwordIsVisible = false;

        /// <summary>
        /// Свойство, содержащее вошедшего в аккаунт пользователя.
        /// </summary>
        public User EnteredUser
        {
            get
            {
                return enteredUser;
            }

            private set
            {
                enteredUser = value;
            }
        }

        public GetStartedWindow()
        {
            InitializeComponent();
        }

        private void UserNameBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckFieldsToFill();
        }

        private void UserPassword_Box_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckFieldsToFill();
        }

        private void UserPassword_HiddenBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckFieldsToFill();
        }

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

        private void EnterAccountButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                enteredUser = DataBaseWork.ReadUserTable(UserNameBox.Text, passwordIsVisible ? UserPassword_Box.Text : UserPassword_HiddenBox.Password);

                MessageBox.Show($"Новый пользователь: {enteredUser.Name}!");
            }

            catch (UserNotFoundException)
            {
                MessageBox.Show("Данный пользователь не найден.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);

                enteredUser = null;
            }
        }

        private void CreateNewAccountButton_Click(object sender, RoutedEventArgs e)
        {

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
