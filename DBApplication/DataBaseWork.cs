using System;
using System.Linq;
using System.Data;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Collections.Generic;
using Bool = System.Boolean;

/// <summary>
/// Область с Базами Данных.
/// </summary>
namespace DBApplication
{
    /// <summary>
    /// Класс, содержащий функционал для взаимодействия с Базами Данных.
    /// </summary>
    public static class DataBaseWork
    {
        /// <summary>
        /// Поле, отвечающее за то, были ли инициализированы поля для подключения к Базе Данных.
        /// </summary>
        static Bool initialized;
        /// <summary>
        /// Поле, отвечающее за то, что "readResult" содержит новое значение и готов к перезаписи Источника Данных.
        /// </summary>
        static Bool readyToRefresh;
        /// <summary>
        /// Поле, содержащее экземпляр класса "DataSet", нужный для хранения полученных данных.
        /// </summary>
        static DataSet readResult;
        /// <summary>
        /// Поле, содержащее экземпляр класса "SqlConnection", нужный для обращения к Базе Данных.
        /// </summary>
        static SqlConnection connectToDB;
        /// <summary>
        /// Поле, содержащее экземпляр класса "SqlDataAdapter", нужный для выполнения команд.
        /// </summary>
        static SqlDataAdapter dataSetter;
        /// <summary>
        /// Поле, содержащее экземпляр класса "SqlCommandBuilder", нужный для создания комманд для Базы Данных.
        /// </summary>
        static SqlCommandBuilder sqlCommands;

        /// <summary>
        /// Статический конструктор класса. Инициализирует поле подключения к Базе Данных и обновляет список активных пользователей.
        /// </summary>
        static DataBaseWork()
        {
            connectToDB = new SqlConnection(Other.ConnectionString);

            initialized = true;
        }

        /// <summary>
        /// Метод для внесения пользователя в Базу Данных.
        /// </summary>
        /// <param name="user">Пользователь, которого нужно добавить.</param>
        public static void WriteNewUserInTable(User user)
        {
            if (initialized)
            {
                if (CheckUserExistance(user.Name))
                {
                    MessageBox.Show("Пользователь с таким именем уже существует в системе.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                {
                    SqlCommand commandToInsert = new SqlCommand("INSERT INTO UserTable(name, password, gender, birthDate) " +
                    "VALUES(@uName, @uPass, @uGender, @uBirth)", connectToDB);
                    commandToInsert.Parameters.AddWithValue("@uName", user.Name);
                    commandToInsert.Parameters.AddWithValue("@uPass", user.Password);
                    commandToInsert.Parameters.AddWithValue("@uGender", user.UserGender.GetStringFromGender());
                    commandToInsert.Parameters.AddWithValue("@uPass", user.Password);

                    commandToInsert.ExecuteNonQuery();

                    connectToDB.Close();
                }
            }

            else
            {
                MessageBox.Show("Обнаружена попытка обращения к Базе Данных до инициализации.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Метод для получения аккаунта пользователя из Базы Данных.
        /// </summary>
        /// <param name="name">Имя пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Экземпляр класса "User". Если аккаунт не найден, будет выброшено Исключение.</returns>
        public static User ReadUserTable(String name, String password)
        {
            if (initialized)
            {
                connectToDB.Open();

                SqlCommand command = new SqlCommand($"SELECT * FROM UserTable WHERE UserName = @userName AND UserPassword = @userPassword", connectToDB);
                command.Parameters.AddWithValue("@userName", name);
                command.Parameters.AddWithValue("@userPassword", password);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                {
                    return new User(reader.GetValue(1) as String, reader.GetValue(2) as String, reader.GetValue(3).GetGenderFromString(), (DateTime)reader.GetValue(4));
                }

                else
                {
                    connectToDB.Close();

                    throw new UserNotFoundException("Указанный аккаунт не найден.");
                }
            }

            else
            {
                MessageBox.Show("Обнаружена попытка обращения к Базе Данных до инициализации.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);

                return null;
            }
        }

        /// <summary>
        /// Метод для добавления Товара (экземпляр "Commodity") в Базу Данных.
        /// </summary>
        /// <param name="commodityToAdd">Товар, который нужно добавить в Базу Данных.</param>
        public static void WriteCommodityTable(Commodity commodityToAdd)
        {
            dataSetter = new SqlDataAdapter("INSERT INTO CommodityTable(name, weight, price, quantity) " +
            "VALUES(@comName, @comWeight, @comPrice, @comQuantity)", connectToDB);
            dataSetter.SelectCommand.Parameters.AddWithValue("@comName", commodityToAdd.CommodityName);
            dataSetter.SelectCommand.Parameters.AddWithValue("@comWeight", commodityToAdd.CommodityWeight);
            dataSetter.SelectCommand.Parameters.AddWithValue("@comPrice", commodityToAdd.CommodityPrice);
            dataSetter.SelectCommand.Parameters.AddWithValue("@comQuantity", commodityToAdd.CommodityQuantity);

            sqlCommands = new SqlCommandBuilder(dataSetter);
            sqlCommands.GetUpdateCommand();

            dataSetter.Fill(readResult, "CommodityTable");
            readyToRefresh = true;
        }

        /// <summary>
        /// Метод для получения товаров пользователя из Базы Данных.
        /// </summary>
        /// <param name="userName">Имя пользователя, чьи товары необходимо найти.</param>
        /// <param name="gridToFill">Ссылочный параметр. Таблица, которую необходимо заполнить.</param>
        public static void ReadCommoditiesTable(String userName)
        {
            if (initialized)
            {
                connectToDB.Open();

                dataSetter = new SqlDataAdapter("SELECT * FROM CommodityTable WHERE Owner = @user", connectToDB);
                dataSetter.SelectCommand.Parameters.AddWithValue("@user", userName);

                sqlCommands = new SqlCommandBuilder(dataSetter);
                sqlCommands.GetInsertCommand();
                sqlCommands.GetUpdateCommand();
                sqlCommands.GetDeleteCommand();

                dataSetter.Fill(readResult, "CommodityTable");
                readyToRefresh = true;

                connectToDB.Close();
            }

            else
            {
                MessageBox.Show("Обнаружена попытка обращения к Базе Данных до инициализации.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Метод для проверки Базы Данных на существование аккаунта с таким именем.
        /// </summary>
        /// <param name="userName">Имя пользователя для проверки.</param>
        /// <param name="close">Необязательный параметр.
        /// Отвечает за то, следует ли закрывать подключение после выполнения инструкций или нет.</param>>
        /// <returns>Переменная, отвечающая за существование такого пользователя в системе.</returns>
        private static Bool CheckUserExistance(String userName, Bool close = false)
        {
            if (initialized)
            {
                connectToDB.Open();

                SqlCommand commandToSearch = new SqlCommand("SELECT * FROM UserTable WHERE UserName = @uName", connectToDB);
                commandToSearch.Parameters.AddWithValue("@uName", userName);

                SqlDataReader reader = commandToSearch.ExecuteReader();

                if (close)
                {
                    connectToDB.Close();
                }

                return reader.HasRows;
            }

            else
            {
                MessageBox.Show("Обнаружена попытка обращения к Базе Данных до инициализации.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);

                return false;
            }
        }

        /// <summary>
        /// Метод для обновления источника информации у "ListView".
        /// </summary>
        /// <param name="gridToFill">Ссылочное значение. "ListView", который необходимо обновить.</param>
        public static void UpdateReferences(ref ListView gridToFill)
        {
            if (readyToRefresh)
            {
                gridToFill.ItemsSource = readResult.Tables;

                readyToRefresh = false;
            }
        }
    }
}
