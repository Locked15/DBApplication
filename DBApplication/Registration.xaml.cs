using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using Bool = System.Boolean;

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
        private Grid activeGrid;

        /// <summary>
        /// Поле, отвечающее за то, виден ли сейчас пароль пользователя.
        /// </summary>
        private Bool passwordIsVisible = false;

        /// <summary>
        /// Поле, содержащее текущую активную "Основную Кнопку."
        /// Нужно для правильной работы эффекта "вливания".
        /// </summary>
        private Int32 lastMainButtonIndex = -1;

        /// <summary>
        /// Поле, содержащее список с кнопками, расположенными на левой панели.
        /// Нужно для правильной работы расфокусировки элементов.
        /// </summary>
        List<Button> mainButtons = new List<Button>(4);

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
            //Этот код можно было сократить, то для красоты он написан именно так.

            if (String.IsNullOrEmpty(UserNameGrid_UserNameBox.Text))
            {
                //Если что, это условие можно было записать в одном блоке "if", просто используя &&.

                if (MessageBox.Show("Имя пользователя не введено.\nВы точно хотите продолжить?", "Предупреждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //Вызываем преждевременное окончание события, используя оператор "return".

                    return;
                }
            }

            if (activeGrid != UserPasswordGrid)
            {
                UserPasswordGrid.Visibility = Visibility.Visible;

                activeGrid.Visibility = Visibility.Hidden;
            }

            UserPasswordBlockButton.Focus();

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
            if (String.IsNullOrEmpty(passwordIsVisible ? UserPasswordGrid_UserPasswordBox.Text
            : UserPasswordGrid_UserPasswordHiddenBox.Password) || String.IsNullOrEmpty(passwordIsVisible
            ? UserPasswordGrid_UserAcceptPasswordBox.Text : UserPasswordGrid_UserAcceptPasswordHiddenBox.Password))
            {
                if (MessageBox.Show("Одно из полей не было заполнено.\nВы точно хотите продолжить?", "Предупреждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //Вызываем преждевременное прерывание события.

                    return;
                }
            }

            if (activeGrid != UserGenderGrid)
            {
                UserGenderGrid.Visibility = Visibility.Visible;

                activeGrid.Visibility = Visibility.Hidden;
            }

            UserGenderBlockButton.Focus();

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
            if (UserGenderGrid_UserGenderSelect.SelectedItem == null)
            {
                if (MessageBox.Show("Пол пользователя не был указан.\nВы точно хотите продолжить?", "Предупреждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;
                }
            }

            if (activeGrid != UserBirthGrid)
            {
                UserBirthGrid.Visibility = Visibility.Visible;

                activeGrid.Visibility = Visibility.Hidden;
            }

            UserBirthBlockButton.Focus();

            activeGrid = UserBirthGrid;
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region События фокусировки.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Событие, возникающее при наведении мыши на элемент управления.
        /// Нужно для сокрытия подсказки пользователю.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserNameGrid_UserNameHiddenBox_MouseEnter(object sender, MouseEventArgs e)
        {
            UserNameGrid_UserNameBox.Visibility = Visibility.Visible;
            UserNameGrid_UserNameHiddenBox.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Событие, возникающее при уводе мыши с элемента управления.
        /// Нужно для отображения подсказки пользователю, если ничего не введено и элемент не в фокусе.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserNameGrid_UserNameBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (String.IsNullOrEmpty(UserNameGrid_UserNameBox.Text) && !UserNameGrid_UserNameBox.IsFocused)
            {
                UserNameGrid_UserNameBox.Visibility = Visibility.Hidden;
                UserNameGrid_UserNameHiddenBox.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Событие, возникающее при наведении мыши на элемент управления. Нужно для создания границ.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ElementGotMouse(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                button.BorderThickness = new Thickness(1);
            }
        }

        /// <summary>
        /// Событие, возникающее при уводе мыши с элемента управления. 
        /// Нужно для удаления границ у элемента.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ElementLostMouse(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                button.BorderThickness = new Thickness(0);
            }
        }

        /// <summary>
        /// Событие, возникающее при получении элементом управления фокуса. 
        /// Нужно для создания эффекта "вливания" при выборе того или иного меню. 
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ButtonGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (lastMainButtonIndex != -1)
                {
                    mainButtons[lastMainButtonIndex].BorderThickness = new Thickness(1);
                    mainButtons[lastMainButtonIndex].Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                }

                lastMainButtonIndex = mainButtons.IndexOf(button);

                button.BorderThickness = button.Name == "UserBirthBlockButton"
                ? new Thickness(0, 0, 0, 1) : new Thickness(0);
                button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Прочие события.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Событие, возникающее при загрузке окна.
        /// Нужно для выполнения различных действий.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Object item in MainGrid.Children)
            {
                if (item is Button button)
                {
                    mainButtons.Add(button);
                }
            }

            UserNameBlockButton.Focus();
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "UserIsReadyButton".
        /// Проверяет значения полей и доступность создания такого пользователя.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserIsReadyButton_Click(object sender, RoutedEventArgs e)
        {
            Bool passwordIsCorrect = !String.IsNullOrEmpty(passwordIsVisible ? UserPasswordGrid_UserPasswordBox.Text : UserPasswordGrid_UserPasswordHiddenBox.Password) &&
            !String.IsNullOrEmpty(passwordIsVisible ? UserPasswordGrid_UserAcceptPasswordBox.Text : UserPasswordGrid_UserAcceptPasswordHiddenBox.Password) &&
            (passwordIsVisible ? UserPasswordGrid_UserAcceptPasswordBox.Text : UserPasswordGrid_UserAcceptPasswordHiddenBox.Password) ==
            (passwordIsVisible ? UserPasswordGrid_UserPasswordBox.Text : UserPasswordGrid_UserPasswordHiddenBox.Password);

            if (!String.IsNullOrEmpty(UserNameGrid_UserNameBox.Text) && passwordIsCorrect && UserGenderGrid_UserGenderSelect.SelectedItem != null &&
            UserBirthGrid_UserBirthSelect.SelectedDate.HasValue)
            {
                if (UserBirthGrid_UserBirthSelect.SelectedDate.Value.GetYearsFromDates(DateTime.Now) > 16)
                {
                    User newUser = new User(UserNameGrid_UserNameBox.Text, passwordIsVisible ? UserPasswordGrid_UserPasswordBox.Text :
                    UserPasswordGrid_UserPasswordHiddenBox.Password, User.Genders[UserGenderGrid_UserGenderSelect.SelectedIndex],
                    (DateTime)UserBirthGrid_UserBirthSelect.SelectedDate);

                    if (DataBaseWork.WriteNewUserInTable(newUser))
                    {
                        MainWindow newWindow = new MainWindow(newUser);

                        newWindow.Show();
                        Close();
                    }
                }

                else
                {
                    MessageBox.Show("Ваш возраст слишком мал для использования приложения.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            else if (!passwordIsCorrect)
            {
                MessageBox.Show("Обнаружена ошибка в пароле.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                MessageBox.Show("Какие-либо сведения не были указаны.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "UserPasswordGrid_ShowUserPassword".
        /// Отображает указанный пароль.
        /// </summary>
        /// <param name="sender">Объект, вывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserPasswordGrid_ShowUserPassword_Click(object sender, RoutedEventArgs e)
        {
            if (!passwordIsVisible)
            {
                UserPasswordGrid_UserPasswordBox.Text = UserPasswordGrid_UserPasswordHiddenBox.Password;
                UserPasswordGrid_UserAcceptPasswordBox.Text = UserPasswordGrid_UserAcceptPasswordHiddenBox.Password;

                UserPasswordGrid_UserAcceptPasswordBox.Visibility = Visibility.Visible;
                UserPasswordGrid_UserAcceptPasswordHiddenBox.Visibility = Visibility.Hidden;

                UserPasswordGrid_UserPasswordBox.Visibility = Visibility.Visible;
                UserPasswordGrid_UserPasswordHiddenBox.Visibility = Visibility.Hidden;

                passwordIsVisible = true;
            }

            else
            {
                UserPasswordGrid_UserPasswordHiddenBox.Password = UserPasswordGrid_UserPasswordBox.Text;
                UserPasswordGrid_UserAcceptPasswordHiddenBox.Password = UserPasswordGrid_UserAcceptPasswordBox.Text;

                UserPasswordGrid_UserAcceptPasswordBox.Visibility = Visibility.Hidden;
                UserPasswordGrid_UserAcceptPasswordHiddenBox.Visibility = Visibility.Visible;

                UserPasswordGrid_UserPasswordBox.Visibility = Visibility.Hidden;
                UserPasswordGrid_UserPasswordHiddenBox.Visibility = Visibility.Visible;

                passwordIsVisible = false;
            }
        }

        /// <summary>
        /// Событие, возникающее при нажатии на клавишу, при активном элементе управления.
        /// Нужно для обработки нажатия клавиши "Enter".
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void EnterKeyUse(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender is TextBox box)
                {
                    if (box == UserNameGrid_UserNameBox)
                    {
                        UserNameGrid_GoToPasswordBlockButton_Click(UserNameGrid_GoToPasswordBlockButton, new RoutedEventArgs());
                    }

                    else if (box == UserPasswordGrid_UserPasswordBox)
                    {
                        UserPasswordGrid_UserAcceptPasswordBox.Focus();
                    }

                    else
                    {
                        UserPasswordGrid_GoToGenderBlockButton_Click(UserPasswordGrid_GoToGenderBlockButton, new RoutedEventArgs());
                    }
                }

                else if (sender is PasswordBox passBox)
                {
                    if (passBox == UserPasswordGrid_UserPasswordHiddenBox)
                    {
                        UserPasswordGrid_UserAcceptPasswordHiddenBox.Focus();
                    }

                    else
                    {
                        UserPasswordGrid_GoToGenderBlockButton_Click(UserPasswordGrid_GoToGenderBlockButton, new RoutedEventArgs());
                    }
                }
            }
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion
    }
}
