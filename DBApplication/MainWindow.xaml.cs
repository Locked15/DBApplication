using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using Bool = System.Boolean;

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
        /// Поле, отвечающее за то, что сейчас происходит замена товара, а не его создание.
        /// </summary>
        Bool changing;

        /// <summary>
        /// Поле, отвечающее за ID изменяемого товара.
        /// </summary>
        Int32 changingID;

        /// <summary>
        /// Поле, содержащее текущего пользователя.
        /// </summary>
        User currentUser;

        /// <summary>
        /// Поле, содержащее последнюю активную "основную" кнопку.
        /// </summary>
        Button lastMainButton;

        /// <summary>
        /// Поле, содержащее последнюю кнопку навигации из раздела "Добавление Нового Товара".
        /// </summary>
        Button lastAddButton;

        /// <summary>
        /// Поле, содержащее список "основных" кнопок. Нужно для создания эффекта "вливания".
        /// </summary>
        List<Button> mainButtons = new List<Button>(1);

        /// <summary>
        /// Поле, содержащее список, содержащий все имущество текущего владельца. Нужен для работы привязки данных.
        /// </summary>
        List<UserProperty> currentUserProperties = new List<UserProperty>(1);

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Методы класса.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public MainWindow(User user)
        {
            currentUser = user;

            InitializeComponent();
        }

        /// <summary>
        /// Метод для сброса значений кнопок "AddCommodityButton" или "ChangeCommodityButton" до их оригинального вида.
        /// </summary>
        private void ResetAddOrChangeButtonState()
        {
            if (changing)
            {
                ChangeCommodityButton.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                ChangeCommodityButton.BorderThickness = new Thickness(1);

                changing = false;
            }

            else
            {
                AddCommodityButton.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                AddCommodityButton.BorderThickness = new Thickness(1);
            }
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Работа с Таблицей.
        //—————————————————————————————————————————————————————————————————————————————————————————

        #region Основные кнопки.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "AddCommodityButton". Добавляет новый товар в список и Базу Данных.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void AddCommodityButton_Click(object sender, RoutedEventArgs e)
        {
            if (changing)
            {
                Int32? hasIndex = Sheet.SelectedIndex;

                if (!hasIndex.HasValue)
                {
                    MessageBox.Show("Предмет для замены не выбран.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);

                    changing = false;

                    return;
                }

                changingID = hasIndex.Value;
            }

            MainDataBaseBlock.Visibility = Visibility.Hidden;
            AddNewCommodityBlock.Visibility = Visibility.Visible;

            AddNewCommodity_ComNameBlock_InputComNameBox.IsEnabled = !changing;
            AddNewCommodity_ComWeightBlock_InputComWeightBox.IsEnabled = !changing;

            CommodityIsReady.Content = changing ? "Сохранить." : "Добавить товар.";

            if (changing)
            {
                AddNewCommodity_ComNameBlock_InputComNameBox.Text = currentUserProperties[Sheet.SelectedIndex].Commodity.CommodityName;
                AddNewCommodity_ComWeightBlock_InputComWeightBox.Text = currentUserProperties[Sheet.SelectedIndex].Commodity.CommodityWeight.ToString();
                AddNewCommodity_ComQuantityBlock_InputComQuantityBox.Text = currentUserProperties[Sheet.SelectedIndex].Commodity.CommodityQuantity.ToString();
                AddNewCommodity_ComPriceBlock_InputComPriceBox.Text = currentUserProperties[Sheet.SelectedIndex].Commodity.CommodityPrice.ToString();
            }

            GoToComNameBlock_Click(GoToComNameBlockButton, new RoutedEventArgs());
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "ChangeCommodityButton". Изменяет значения товара в списке и Базе Данных.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ChangeCommodityButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeChanging();
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
            currentUserProperties = DataBaseWork.ReadCommoditiesTable(currentUser.Name);

            Sheet.ItemsSource = currentUserProperties;
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Область добавления нового товара.
        //—————————————————————————————————————————————————————————————————————————————————————————

        #region События перемещения по областям.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "GoToComNameBlockButton".
        /// Перемещает пользователя к области указания имени товара.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void GoToComNameBlock_Click(object sender, RoutedEventArgs e)
        {
            //Выполняем переход к новому блоку создания товара:
            HideAddCommodityGrids();
            AddNewCommodity_ComNameBlock.Visibility = Visibility.Visible;

            //Переопределяем стили:
            GoToComNameBlockButton.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            GoToComNameBlockButton.BorderThickness = new Thickness(0);

            AddNewCommodity_ComNameBlock_InputComNameBox.Focus();

            if (lastAddButton != null && lastAddButton != sender as Button)
            {
                lastAddButton.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                lastAddButton.BorderThickness = new Thickness(1);
            }

            lastAddButton = GoToComNameBlockButton;
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "GoToComWeightBlockButton".
        /// Переводит пользователя к области ввода веса товара.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void GoToComWeightBlock_Click(object sender, RoutedEventArgs e)
        {
            //Чтобы уведомление появлялось только при обычном переходе, добавляем еще одно условие.
            if (sender == AddNewCommodity_ComNameBlock_GoToWeightBlockButton
            && String.IsNullOrEmpty(AddNewCommodity_ComNameBlock_InputComNameBox.Text))
            {
                //Это можно было сделать короче, но для красоты кода все сделано развернуто.

                if (MessageBox.Show("Имя товара не было введено.\nВы точно хотите продолжить?", "Ошибка!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    //Прерываем выполнение метода.
                    return;
                }
            }

            //Переводим пользователя:
            HideAddCommodityGrids();
            AddNewCommodity_ComWeightBlock.Visibility = Visibility.Visible;

            //Переопределяем стили:
            GoToComWeightBlockButton.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            GoToComWeightBlockButton.BorderThickness = new Thickness(0);

            if (lastAddButton != sender as Button)
            {
                lastAddButton.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                lastAddButton.BorderThickness = new Thickness(1);
            }

            //Проводим фокусировку на элементе управления с вводом:
            AddNewCommodity_ComWeightBlock_InputComWeightBox.Focus();

            lastAddButton = GoToComWeightBlockButton;
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "GoToComQuantityBlockButton".
        /// Перемещает пользователя к области ввода количества товара.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void GoToComQuantityBlock_Click(object sender, RoutedEventArgs e)
        {
            //Чтобы уведомление появлялось только при обычном переходе, добавляем еще одно условие.
            if (sender == AddNewCommodity_ComWeightBlock_GoToQuantityBlockButton
            && String.IsNullOrEmpty(AddNewCommodity_ComWeightBlock_InputComWeightBox.Text))
            {
                //Это можно было сделать короче, но для красоты кода все сделано развернуто.

                if (MessageBox.Show("Вес товара не был введен.\nВы точно хотите продолжить?", "Ошибка!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    //Прерываем выполнение метода.
                    return;
                }
            }

            //Выполняем переход к новому блоку создания товара:
            HideAddCommodityGrids();
            AddNewCommodity_ComQuantityBlock.Visibility = Visibility.Visible;

            //Переопределяем стили:
            GoToComQuantityBlockButton.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            GoToComQuantityBlockButton.BorderThickness = new Thickness(0);

            if (lastAddButton != sender as Button)
            {
                lastAddButton.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                lastAddButton.BorderThickness = new Thickness(1);
            }

            //Проводим фокусировку на элементе управления с вводом:
            AddNewCommodity_ComQuantityBlock_InputComQuantityBox.Focus();

            lastAddButton = GoToComQuantityBlockButton;
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "GoToComPriceBlockButton".
        /// Переводит пользователя к области ввода цены товара.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void GoToComPriceBlock_Click(object sender, RoutedEventArgs e)
        {
            //Чтобы уведомление появлялось только при обычном переходе, добавляем еще одно условие.
            if (sender == AddNewCommodity_ComQuantityBlock_GoToPriceBlockButton
            && String.IsNullOrEmpty(AddNewCommodity_ComQuantityBlock_InputComQuantityBox.Text))
            {
                //Это можно было сделать короче, но для красоты кода все сделано развернуто.

                if (MessageBox.Show("Количество товара не было указано.\nВы точно хотите продолжить?", "Ошибка!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    //Прерываем выполнение метода.
                    return;
                }
            }

            //Переводим пользователя:
            HideAddCommodityGrids();
            AddNewCommodity_ComPriceBlock.Visibility = Visibility.Visible;

            //Переопределяем стили:
            GoToComPriceBlockButton.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            GoToComPriceBlockButton.BorderThickness = new Thickness(0);

            if (lastAddButton != sender as Button)
            {
                lastAddButton.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                lastAddButton.BorderThickness = new Thickness(1);
            }

            //Проводим фокусировку на элементе управления с вводом:
            AddNewCommodity_ComPriceBlock_InputComPriceBox.Focus();

            lastAddButton = GoToComPriceBlockButton;
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Вспомогательные методы.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Метод для сокрытия всех Grid, которые связаны с добавлением информации касательно товара.
        /// </summary>
        private void HideAddCommodityGrids()
        {
            foreach (Object element in AddNewCommodityBlock.Children)
            {
                if (element is Grid gridToHide)
                {
                    gridToHide.Visibility = Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Метод для очистки всех полей для ввода при создании нового товара.
        /// </summary>
        private void ResetTextInAllBlocks()
        {
            AddNewCommodity_ComNameBlock_InputComNameBox.Text = "";
            AddNewCommodity_ComWeightBlock_InputComWeightBox.Text = "";
            AddNewCommodity_ComQuantityBlock_InputComQuantityBox.Text = "";
            AddNewCommodity_ComPriceBlock_InputComPriceBox.Text = "";
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Прочие события.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Событие, возникающее при попытке создать новый товар.
        /// Проверяет значения всех полей и, если все верно, добавляет новый товар.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void CommodityIsReadyButton_Click(object sender, RoutedEventArgs e)
        {
            Decimal commodityWeight;
            Int32 commodityQuantity;
            Decimal commodityPrice;

            Bool nameIsCorrect = !String.IsNullOrEmpty(AddNewCommodity_ComNameBlock_InputComNameBox.Text);
            Bool weightIsCorrect = Decimal.TryParse(AddNewCommodity_ComWeightBlock_InputComWeightBox.Text, out commodityWeight) && commodityWeight > 0;
            Bool quantityIsCorrect = Int32.TryParse(AddNewCommodity_ComQuantityBlock_InputComQuantityBox.Text, out commodityQuantity) && commodityQuantity > -1;
            Bool priceIsCorrect = Decimal.TryParse(AddNewCommodity_ComPriceBlock_InputComPriceBox.Text, out commodityPrice) && commodityPrice >= 0;

            //Область добавления нового товара.
            if (nameIsCorrect && weightIsCorrect && quantityIsCorrect && priceIsCorrect && !changing)
            {
                currentUserProperties.Add(DataBaseWork.WriteCommodityTable(new Commodity(AddNewCommodity_ComNameBlock_InputComNameBox.Text, 
                commodityWeight, commodityPrice, commodityQuantity), currentUser.Name));

                ResetTextInAllBlocks();

                Sheet.ItemsSource = null;
                Sheet.ItemsSource = currentUserProperties;

                AddNewCommodityBlock.Visibility = Visibility.Hidden;
                MainDataBaseBlock.Visibility = Visibility.Visible;

                ResetAddOrChangeButtonState();
            }

            //Область замены старого товара.
            else if (nameIsCorrect && weightIsCorrect && quantityIsCorrect && priceIsCorrect)
            {
                currentUserProperties[changingID] = DataBaseWork.ChangeCommodityInTable(changingID,
                new Commodity(AddNewCommodity_ComNameBlock_InputComNameBox.Text, commodityWeight, commodityPrice, commodityQuantity), 
                currentUser.Name);

                Sheet.ItemsSource = null;
                Sheet.ItemsSource = currentUserProperties;
                
                ResetTextInAllBlocks();

                AddNewCommodityBlock.Visibility = Visibility.Hidden;
                MainDataBaseBlock.Visibility = Visibility.Visible;

                ResetAddOrChangeButtonState();
            }

            //Область сообщений, которые будут появляться, если пользователь ввел что-то неправильно.
            if (!nameIsCorrect)
            {
                MessageBox.Show("Имя товара не введено.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!weightIsCorrect)
            {
                MessageBox.Show("Вес товара указан некорректно.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!quantityIsCorrect)
            {
                MessageBox.Show("Количество товара указано некорректно.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!priceIsCorrect)
            {
                MessageBox.Show("Цена товара указана некорректно.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Событие, возникающее при нажатии кнопки "AddNewCommodity_CloseBlock".
        /// Нужно для отмены создания нового товара.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void AddNewCommodity_CloseBlock_Click(object sender, RoutedEventArgs e)
        {
            ResetTextInAllBlocks();

            AddNewCommodityBlock.Visibility = Visibility.Hidden;

            MainDataBaseBlock.Visibility = Visibility.Visible;

            if (changing)
            {
                ResetAddOrChangeButtonState();

                lastMainButton = null;
            }

            else
            {
                ResetAddOrChangeButtonState();

                lastMainButton = null;
            }
        }

        /// <summary>
        /// Общее событие для всех "TextBox", имеющих стиль "InputBoxStyle".
        /// Нужно для обработки нажатия клавиши "Enter".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyIsUsed(object sender, KeyEventArgs e)
        {
            TextBox box = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                //Отправка в область с вводом Веса.
                if (box == AddNewCommodity_ComNameBlock_InputComNameBox)
                {
                    GoToComWeightBlock_Click(GoToComWeightBlockButton, new RoutedEventArgs());
                }

                //Отправка в область с вводом Количества.
                else if (box == AddNewCommodity_ComWeightBlock_InputComWeightBox)
                {
                    GoToComQuantityBlock_Click(GoToComQuantityBlockButton, new RoutedEventArgs());
                }

                //Отправка в область с вводом Цены.
                else if (box == AddNewCommodity_ComQuantityBlock_InputComQuantityBox)
                {
                    GoToComPriceBlock_Click(GoToComPriceBlockButton, new RoutedEventArgs());
                }

                //Попытка создать товар.
                else
                {
                    CommodityIsReadyButton_Click(CommodityIsReadyButton, new RoutedEventArgs());
                }
            }
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Область изменения товара.
        //—————————————————————————————————————————————————————————————————————————————————————————

        #region Вспомогательные методы.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Метод, который запускает процедуру смены товара.
        /// </summary>
        private void InitializeChanging()
        {
            changing = true;

            AddCommodityButton_Click(ChangeCommodityButton, new RoutedEventArgs());
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        /* Пояснение:
        * Весь остальной функционал реализован прямо в области "Добавление товара".
        * Логика его работы базируется на состоянии поля "changing", которое и изменяется посредством здешнего метода.
        */

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Прочие события.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Событие, возникающее при получении элементом управления фокуса.
        /// Нужно для создания эффекта "вливания".
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainButtonIsFocused(object sender, RoutedEventArgs e)
        {
            Button newActiveButton = sender as Button;

            newActiveButton.BorderThickness = new Thickness(0);
            newActiveButton.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            if (lastMainButton != null && lastMainButton != newActiveButton)
            {
                lastMainButton.BorderThickness = new Thickness(1);
                lastMainButton.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
            }

            lastMainButton = newActiveButton;
        }

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
            MessageBox.Show("Табличный представитель.\n\nВерсия: 1.0;\nHello From УКСиВТ!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
