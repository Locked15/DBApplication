using System;
using System.Linq;
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
    /// Логика взаимодействия для MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Поля класса.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Поле, отвечающее за то, что сейчас происходит восстановление товара, а не его удаление.
        /// </summary>
        Bool restoring;

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

        /// <summary>
        /// Поле, содержащее список, который содержит товары. Нужен для работы в блоке Удаления (Восстановление/Удаление товаров).
        /// </summary>
        List<PropertyToDelete> propertiesOfDeleteBlock = new List<PropertyToDelete>(1);

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
        /// Метод для сброса значений основных кнопок до их оригинального вида.
        /// </summary>
        private void ResetMainButtonsState()
        {
            if (changing)
            {
                ChangeCommodityButton.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                ChangeCommodityButton.BorderThickness = new Thickness(1);

                changing = false;
            }

            else if (DeleteCommodityBlock.Visibility == Visibility.Visible)
            {
                if (restoring)
                {
                    DeletedCommodititesButton.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                    DeletedCommodititesButton.BorderThickness = new Thickness(1);
                }

                else
                {
                    DeleteCommodityButton.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                    DeleteCommodityButton.BorderThickness = new Thickness(1);
                }
            }

            else
            {
                AddCommodityButton.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                AddCommodityButton.BorderThickness = new Thickness(1);
            }
        }

        /// <summary>
        /// Метод для входа в основную область программы — Область с таблицей.
        /// </summary>
        /// <param name="updateReferences">Необязательный параметр. Отвечает за то, что при входе будет обновлен источник данных Главной Таблицы.</param>
        private void EnterIntoMainBlock(Bool updateReferences = false)
        {
            MainDataBaseBlock.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Метод для подготовки области с Удалением товара.
        /// </summary>
        private void InitializeDeleteBlock()
        {
            if (restoring)
            {
                DelCommodity_CheckBlock.Header = "Восстановить?";
                DelCommodityBlock_DeleteChosenButton.Content = "Восстановить.";
            }

            else
            {
                DelCommodity_CheckBlock.Header = "Удалить?";
                DelCommodityBlock_DeleteChosenButton.Content = "Удалить.";
            }

            MainDataBaseBlock.Visibility = Visibility.Hidden;
            DeleteCommodityBlock.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Метод для обновления источника данных у Главной Таблицы.
        /// </summary>
        private void UpdateItemSource()
        {
            Sheet.ItemsSource = null;
            Sheet.ItemsSource = currentUserProperties;
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
                Int32 hasIndex = Sheet.SelectedIndex;

                if (hasIndex == -1)
                {
                    MessageBox.Show("Предмет для замены не выбран.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    ResetMainButtonsState();

                    changing = false;

                    return;
                }

                changingID = hasIndex;
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
            restoring = false;
            propertiesOfDeleteBlock = new List<PropertyToDelete>(1);

            foreach (UserProperty property in currentUserProperties)
            {
                propertiesOfDeleteBlock.Add(new PropertyToDelete(property, false));
            }

            SelectCommodityToDeleteView.ItemsSource = propertiesOfDeleteBlock;

            InitializeDeleteBlock();
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "DeletedCommodititesButton_Click".
        /// Нужно для отображения блока восстановления удаленных товаров.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void DeletedCommodititesButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeRestoring();

            InitializeDeleteBlock();
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "UpdateSheetButton". Обновляет таблицу, подргужая значения из Базы Данных.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UpdateSheetButton_Click(object sender, RoutedEventArgs e)
        {
            currentUserProperties = DataBaseWork.ReadCommoditiesTable(currentUser.Name);

            UpdateItemSource();
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

                UpdateItemSource();

                AddNewCommodityBlock.Visibility = Visibility.Hidden;
                EnterIntoMainBlock();

                ResetMainButtonsState();
            }

            //Область замены старого товара.
            else if (nameIsCorrect && weightIsCorrect && quantityIsCorrect && priceIsCorrect)
            {
                currentUserProperties[changingID] = DataBaseWork.ChangeCommodityInTable(changingID,
                new Commodity(AddNewCommodity_ComNameBlock_InputComNameBox.Text, commodityWeight, commodityPrice, commodityQuantity),
                currentUser.Name);

                UpdateItemSource();

                ResetTextInAllBlocks();

                AddNewCommodityBlock.Visibility = Visibility.Hidden;
                EnterIntoMainBlock();

                ResetMainButtonsState();
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
            EnterIntoMainBlock();

            if (changing)
            {
                ResetMainButtonsState();

                lastMainButton = null;
            }

            else
            {
                ResetMainButtonsState();

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

        #region Область удаления товара.
        //—————————————————————————————————————————————————————————————————————————————————————————

        #region Основные события.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "DelCommodityBlock_TurnBackButton".
        /// Нужно для отмены операции удаления и возврата к обычной таблице.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void DelCommodityBlock_TurnBackButton_Click(object sender, RoutedEventArgs e)
        {
            ResetMainButtonsState();

            restoring = false;

            DeleteCommodityBlock.Visibility = Visibility.Hidden;
            EnterIntoMainBlock();
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "DelCommodityBlock_DeleteChosenButton".
        /// Проверяет выбранные строки в таблице и отправляет запрос в Базу Данных на удаление.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void DelCommodityBlock_DeleteChosenButton_Click(object sender, RoutedEventArgs e)
        {
            propertiesOfDeleteBlock = propertiesOfDeleteBlock.Where(x => x.Del == true).ToList();

            if (propertiesOfDeleteBlock.Count > 0)
            {
                foreach (PropertyToDelete property in propertiesOfDeleteBlock)
                {
                    try
                    {
                        if (restoring)
                        {
                            UserProperty fill = DataBaseWork.RestoreCommodityFromTable(property.Property.ID, property.Property.Commodity, currentUser.Name);

                            if (fill.ID > currentUserProperties[currentUserProperties.Count - 1].ID)
                            {
                                currentUserProperties.Add(fill);
                            }

                            else
                            {
                                Int32 index = fill.ID - 1;

                                while (index > currentUserProperties.Count || (index > 0 && currentUserProperties[index - 1].ID > fill.ID))
                                {
                                    index--;
                                }

                                currentUserProperties.Insert(index, fill);
                            }
                        }

                        else
                        {
                            DataBaseWork.DeleteCommodityFromTable(property.Property.ID);

                            currentUserProperties.Remove(property.Property);
                        }
                    }

                    //Исключение, возникающее при слишком большом индексе удаляемого элемента:
                    catch (ElementIdHasBeenTooBig)
                    {
                        MessageBox.Show("В процессе выполнения была обнаружена ошибка.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //Исключение, возникающее при попытке восстановить товар и вставить его в пустую коллекцию товаров пользователя:
                    catch (ArgumentOutOfRangeException)
                    {
                        UserProperty fill = DataBaseWork.RestoreCommodityFromTable(property.Property.ID, property.Property.Commodity, currentUser.Name);

                        currentUserProperties.Add(fill);
                    }
                }

                UpdateItemSource();

                DelCommodityBlock_TurnBackButton_Click(sender, e);
            }

            else
            {
                MessageBox.Show(restoring ? "Товары для восстановления не выбраны." : "Товары для удаления не выбраны.", 
                "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Область восстановления товара.
        //—————————————————————————————————————————————————————————————————————————————————————————

        #region Вспомогательные методы.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Метод, который запускает процедуру восстановления товара.
        /// </summary>
        private void InitializeRestoring()
        {
            restoring = true;

            propertiesOfDeleteBlock = new List<PropertyToDelete>(1);
            List<UserProperty> tmpList = DataBaseWork.ReadDeletedCommodities(currentUser.Name);

            foreach (UserProperty property in tmpList)
            {
                propertiesOfDeleteBlock.Add(new PropertyToDelete(property, false));
            }

            SelectCommodityToDeleteView.ItemsSource = propertiesOfDeleteBlock;
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        /* Пояснение:
        * Весь остальной функционал реализован прямо в области "Удаление товара".
        * Логика его работы базируется на состоянии поля "restoring", которое и изменяется посредством здешнего метода.
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
            EnterIntoMainBlock();

            currentUserProperties = DataBaseWork.ReadCommoditiesTable(currentUser.Name);

            Sheet.ItemsSource = currentUserProperties;
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Внутренний класс для Удаления.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Внутренний класс, предоставляющий поля, нужные для удаления товара.
        /// </summary>
        private class PropertyToDelete
        {
            #region Поля класса.
            //—————————————————————————————————————————————————————————————————————————————————————————

            /// <summary>
            /// Поле, содержащее товар, принадлежащий какому-либо пользователю.
            /// </summary>
            UserProperty property;

            /// <summary>
            /// Поле, отвечающее за то, будет ли удаляться товар.
            /// </summary>
            Bool del;

            //—————————————————————————————————————————————————————————————————————————————————————————
            #endregion

            #region Свойства класса.
            //—————————————————————————————————————————————————————————————————————————————————————————

            /// <summary>
            /// Свойство, содержащее товар, принадлежащий какому-либо пользователю.
            /// </summary>
            public UserProperty Property
            {
                get
                {
                    return property;
                }

                set
                {
                    property = value;
                }
            }

            /// <summary>
            /// Свойство, отвечающее за то, будет ли удаляться товар.
            /// </summary>
            public Bool Del
            {
                get
                {
                    return del;
                }

                set
                {
                    del = value;
                }
            }

            //—————————————————————————————————————————————————————————————————————————————————————————
            #endregion

            #region Методы класса.
            //—————————————————————————————————————————————————————————————————————————————————————————

            /// <summary>
            /// Конструктор класса.
            /// </summary>
            /// <param name="property">Товар, который необходимо добавить в список для будущей привязки данных.</param>
            /// <param name="del">Свойство, отвечающее за то, будет ли товар удаляться.</param>>
            public PropertyToDelete(UserProperty property, Bool del)
            {
                Property = property;
                Del = del;
            }

            //—————————————————————————————————————————————————————————————————————————————————————————
            #endregion
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion
    }
}
