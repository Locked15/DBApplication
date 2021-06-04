using System;
using System.Data;
using System.Windows;
using System.Data.SqlClient;
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
        #region Поля класса.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Поле, содержащее команду для выполнения.
        /// </summary>
        static SqlCommand command;

        /// <summary>
        /// Поле, содержащее экземпляр класса "SqlConnection", нужный для обращения к Базе Данных.
        /// </summary>
        static SqlConnection connectToDB;

        /// <summary>
        /// Поле, содержащее экземпляр класса "DataSet", нужный для хранения полученных данных.
        /// </summary>
        static DataTable readResult = new DataTable();

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Методы класса.
        //—————————————————————————————————————————————————————————————————————————————————————————

        #region Конструктор.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Статический конструктор класса. Инициализирует поле подключения к Базе Данных и обновляет список активных пользователей.
        /// </summary>
        static DataBaseWork()
        {
            connectToDB = new SqlConnection(Other.ConnectionString);
        }

        /// <summary>
        /// Метод для преобразования значений, полученных в Базе Данных в их корректный вид.
        /// </summary>
        /// <param name="id">Объектное представление Идентификатора товара.</param>
        /// <param name="commodityName">Объектное представление Имени товара.</param>
        /// <param name="commodityWeight">Объектное представление Веса товара.</param>
        /// <param name="commodityPrice">Объектное представление Стоимости товара.</param>
        /// <param name="commodityQuantity">Объектное представление Количества товара.</param>
        /// <param name="commodityOwner">Объектное представление Имени Владельца товара.</param>
        /// <returns>Экземпляр класса UserProperty, созданных по преобразованным параметрам.</returns>
        private static UserProperty ConvertValuesToProperty(Object id, Object commodityName, Object commodityWeight,
        Object commodityPrice, Object commodityQuantity, Object commodityOwner)
        {
            //Значения: ID -> Имя товара -> Вес товара -> Стоимость товара -> Количество товара -> Имя владельца.

            return new UserProperty(Convert.ToInt32(id), new Commodity(commodityName as String, Convert.ToDecimal(commodityWeight),
            Convert.ToDecimal(commodityPrice), Convert.ToInt32(commodityQuantity)), commodityOwner as String);
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Работа с Пользователями.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Метод для внесения пользователя в Базу Данных.
        /// </summary>
        /// <param name="user">Пользователь, которого нужно добавить.</param>
        /// <returns>Логическое значение, отвечающее за успех добавления пользователя в Базу Данных.</returns>
        public static Bool WriteNewUserInTable(User user)
        {
            if (CheckUserExistance(user.Name))
            {
                MessageBox.Show("Пользователь с таким именем уже существует в системе.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);

                connectToDB.Close();

                return false;
            }

            else
            {
                command = new SqlCommand("INSERT INTO UserTable(UserName, UserPassword, Gender, BirthDate) " +
                "VALUES(@uName, @uPass, @uGender, @uBirth)", connectToDB);
                command.Parameters.AddWithValue("@uName", user.Name);
                command.Parameters.AddWithValue("@uPass", user.Password);
                command.Parameters.AddWithValue("@uGender", user.UserGender.GetStringFromGender());
                command.Parameters.AddWithValue("@uBirth", user.BirthDate);

                command.ExecuteNonQuery();

                connectToDB.Close();

                return true;
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
            connectToDB.Open();

            command = new SqlCommand($"SELECT * FROM UserTable WHERE UserName = @userName AND UserPassword = @userPassword", connectToDB);
            command.Parameters.AddWithValue("@userName", name);
            command.Parameters.AddWithValue("@userPassword", password);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows && reader.Read())
            {
                String userName = reader.GetString(1);
                String userPassword = reader.GetString(2);
                Gender userGen = reader.GetValue(3).GetGenderFromString();
                DateTime userBirth = (DateTime)reader.GetValue(4);

                connectToDB.Close();

                return new User(userName, userPassword, userGen, userBirth);
            }

            else
            {
                connectToDB.Close();

                throw new UserNotFoundException("Указанный аккаунт не найден.");
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
            connectToDB.Open();

            command = new SqlCommand("SELECT * FROM UserTable WHERE UserName = @uName", connectToDB);
            command.Parameters.AddWithValue("@uName", userName);

            SqlDataReader reader = command.ExecuteReader();

            if (close)
            {
                connectToDB.Close();
            }

            Bool hasRows = reader.HasRows;

            reader.Close();

            return hasRows;
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Работа с Товарами.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Метод для добавления Товара (экземпляр "Commodity") в Базу Данных.
        /// </summary>
        /// <param name="commodityToAdd">Товар, который нужно добавить в Базу Данных.</param>
        /// <param name="ownerName">Имя пользователя, которому принадлежит товар.</param>
        /// <return>Экземпляр класса "UserProperty", который был добавлен в Базу Данных.</return>
        public static UserProperty WriteCommodityTable(Commodity commodityToAdd, String ownerName)
        {
            connectToDB.Open();

            command = new SqlCommand("INSERT INTO CommodityTable(CommodityName, CommodityWeight, CommodityPrice, CommodityQuantity, Owner) " +
            "VALUES(@comName, @comWeight, @comPrice, @comQuantity, @comOwner)", connectToDB);
            command.Parameters.AddWithValue("@comName", commodityToAdd.CommodityName);
            command.Parameters.AddWithValue("@comWeight", commodityToAdd.CommodityWeight);
            command.Parameters.AddWithValue("@comPrice", commodityToAdd.CommodityPrice);
            command.Parameters.AddWithValue("@comQuantity", commodityToAdd.CommodityQuantity);
            command.Parameters.AddWithValue("@comOwner", ownerName);

            SqlDataReader reader = command.ExecuteReader();
            readResult.Load(reader);

            connectToDB.Close();

            UserProperty toReturn = new UserProperty(GetLastCommodityId(), commodityToAdd, ownerName);

            return toReturn;
        }

        /// <summary>
        /// Метод для получения товаров пользователя из Базы Данных.
        /// </summary>
        /// <param name="userName">Имя пользователя, чьи товары необходимо найти.</param>
        /// <return>Список, содержащий значения, пригодные для добавления в таблицу.</return>>
        public static List<UserProperty> ReadCommoditiesTable(String userName)
        {
            List<UserProperty> listToReturn = new List<UserProperty>(1);

            connectToDB.Open();

            command = new SqlCommand("SELECT * FROM CommodityTable WHERE Owner = @user AND Deleted = @del", connectToDB);
            command.Parameters.AddWithValue("@user", userName);
            command.Parameters.AddWithValue("@del", 0);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read() && reader.HasRows)
            {
                listToReturn.Add(ConvertValuesToProperty(reader.GetValue(0), reader.GetValue(1), 
                reader.GetValue(2), reader.GetValue(3), reader.GetValue(4), reader.GetValue(5)));
            }

            connectToDB.Close();

            return listToReturn;
        }

        /// <summary>
        /// Метод для замены значения определенного элемента таблицы на новое.
        /// </summary>
        /// <param name="id">ID заменяемого значения.</param>
        /// <param name="newCommodity">Новый товар, который заменит старый.</param>
        /// <param name="ownerName">Имя владельца товара.</param>
        /// <returns>Экземпляр класса "UserProperty".</returns>
        public static UserProperty ChangeCommodityInTable(Int32 id, Commodity newCommodity, String ownerName)
        {
            connectToDB.Open();

            command = new SqlCommand("UPDATE CommodityTable SET CommodityPrice = @newPrice, " +
            "CommodityQuantity = @newQuantity WHERE Id = @changingID", connectToDB);
            command.Parameters.AddWithValue("@newPrice", newCommodity.CommodityPrice);
            command.Parameters.AddWithValue("@newQuantity", newCommodity.CommodityQuantity);
            command.Parameters.AddWithValue("@changingID", id + 1);

            command.ExecuteNonQuery();

            connectToDB.Close();

            UserProperty toReturn = new UserProperty(id + 1, newCommodity, ownerName);

            return toReturn;
        }

        /// <summary>
        /// Метод для удаления элемента из Базы Данных.
        /// </summary>
        /// <param name="id">Идентификатор, по которому элемент будет удален.</param>
        public static void DeleteCommodityFromTable(Int32 id)
        {
            if (id <= GetLastCommodityId())
            {
                connectToDB.Open();

                command = new SqlCommand("UPDATE CommodityTable SET Deleted = @del WHERE Id = @comId", connectToDB);
                command.Parameters.AddWithValue("@del", 1);
                command.Parameters.AddWithValue("@comId", id);

                command.ExecuteNonQuery();

                connectToDB.Close();
            }

            else
            {
                throw new ElementIdHasBeenTooBig("Попытка удаления элемента по несуществующему идентификатору.");
            }
        }

        /// <summary>
        /// Метод для чтения удаленных записей о товарах в Базе Данных.
        /// </summary>
        /// <param name="userName">Имя пользователя, товары которого будут считываться.</param>
        /// <returns>Список с товарами, которые были удалены.</returns>
        public static List<UserProperty> ReadDeletedCommodities(String userName)
        {
            List<UserProperty> listToReturn = new List<UserProperty>(1);

            connectToDB.Open();

            command = new SqlCommand("SELECT * FROM CommodityTable WHERE Deleted = @del AND Owner = @own", connectToDB);
            command.Parameters.AddWithValue("@del", 1);
            command.Parameters.AddWithValue("@own", userName);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read() && reader.HasRows)
            {
                listToReturn.Add(ConvertValuesToProperty(reader.GetValue(0), reader.GetValue(1),
                reader.GetValue(2), reader.GetValue(3), reader.GetValue(4), reader.GetValue(5)));
            }

            connectToDB.Close();

            return listToReturn;
        }

        /// <summary>
        /// Метод для восстановления удаленного товара из таблицы Базы Данных. 
        /// </summary>
        /// <param name="id">Идентификатор товара, который необходимо восстановить.</param>
        /// <param name="commodityToRestore">Товар, который необходимо восстановить.</param>
        /// <param name="ownerName">Имя владельца товара.</param>
        /// <returns>Экземпляр класса UserProperty для привязки данных.</returns>
        public static UserProperty RestoreCommodityFromTable(Int32 id, Commodity commodityToRestore, String ownerName)
        {
            connectToDB.Open();

            command = new SqlCommand("UPDATE CommodityTable SET Deleted = @newDel WHERE Id = @index", connectToDB);
            command.Parameters.AddWithValue("@newDel", 0);
            command.Parameters.AddWithValue("@index", id);

            command.ExecuteNonQuery();

            connectToDB.Close();

            return new UserProperty(id, commodityToRestore, ownerName);
        }

        /// <summary>
        /// Метод для получения индекса последнего элемента в таблице с Товарами.
        /// </summary>
        /// <returns>Индекс последнего элемента.</returns>
        public static Int32 GetLastCommodityId()
        {
            connectToDB.Open();

            command = new SqlCommand("SELECT MAX(Id) FROM CommodityTable", connectToDB);

            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            Int32 toReturn = reader.GetInt32(0);

            connectToDB.Close();
            return toReturn;
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion
    }
}
