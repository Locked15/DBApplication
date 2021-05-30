using System;
using Bool = System.Boolean;

namespace DBApplication
{
    /// <summary>
    /// Класс, содержащий все поля и логику для работы с Учетными Записями пользователей.
    /// </summary>
    public class User
    {
        #region Поля класса.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Статическое поле, содержащее все возможные значения пола Пользователя.
        /// </summary>
        public static readonly Gender[] Genders = { Gender.Male, Gender.Female, Gender.Alternative };

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Свойства класса.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Свойство, содержащее имя пользователя.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// Свойство, содержащее пароль пользователя.
        /// </summary>
        public String Password { get; private set; }

        /// <summary>
        /// Свойство, содержащее пол пользователя.
        /// </summary>
        public Gender UserGender { get; private set; }

        /// <summary>
        /// Свойство, содержащее дату рождения пользователя.
        /// </summary>
        public DateTime BirthDate { get; private set; }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Методы класса.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="name">Имя пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <param name="userGender">Пол пользователя.</param>
        /// <param name="birthDate">Дата рождения пользователя.</param>
        public User(String name, String password, Gender userGender, DateTime birthDate)
        {
            Name = name;
            Password = password;
            UserGender = userGender;
            BirthDate = birthDate;
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion
    }

    /// <summary>
    /// Структура, содержащая все поля для работы с товарами.
    /// </summary>
    public struct Commodity
    {
        #region Поля класса.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Поле, содержащее имя товара.
        /// </summary>
        readonly String commodityName;

        /// <summary>
        /// Поле, содержащее вес товара.
        /// </summary>
        readonly Decimal commodityWeight;

        /// <summary>
        /// Поле, содержащее цену товара.
        /// </summary>
        Decimal commodityPrice;

        /// <summary>
        /// Поле, содержащее количество товара.
        /// </summary>
        Int32 commodityQuantity;

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Свойства класса.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Свойство, содержащее имя товара.
        /// </summary>
        public String CommodityName
        {
            get
            {
                return commodityName;
            }
        }

        /// <summary>
        /// Свойство, содержащее вес товара.
        /// </summary>
        public Decimal CommodityWeight
        {
            get
            {
                return commodityWeight;
            }
        }

        /// <summary>
        /// Свойство, содержащее цену товара.
        /// </summary>
        public Decimal CommodityPrice
        {
            get
            {
                return commodityPrice;
            }

            private set
            {
                commodityPrice = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее количество товара.
        /// </summary>
        public Int32 CommodityQuantity
        {
            get
            {
                return commodityQuantity;
            }

            private set
            {
                commodityQuantity = value;
            }
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Методы класса.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="commodityName">Имя товара.</param>
        /// <param name="commodityWeight">Вес товара.</param>
        /// <param name="commodityPrice">Цена товара.</param>
        /// <param name="commodityQuantity">Количество товара.</param>
        public Commodity(String commodityName, Decimal commodityWeight, Decimal commodityPrice, Int32 commodityQuantity)
        {
            this.commodityName = commodityName;
            this.commodityWeight = commodityWeight;
            this.commodityPrice = commodityPrice;
            this.commodityQuantity = commodityQuantity;
        }

        /// <summary>
        /// Метод для смены цены товара.
        /// </summary>
        /// <param name="newPrice">Новая цена на товар.</param>
        public void ChangePrice(Decimal newPrice)
        {
            //Так как "throw" прервет исполнение, то блок 'else' прописывать нет смысла.
            if (newPrice < 0)
            {
                throw new CommodityNewPriceIsNegativeException("Выбрана отрицательная цена для товара!");
            }

            CommodityPrice = newPrice;
        }

        /// <summary>
        /// Метод для смены количества товара.
        /// </summary>
        public void ChangeQuantity(Int32 newQuantity)
        {
            //Так как "throw" прервет исполнение, то блок 'else' прописывать нет смысла.
            if (newQuantity < 0)
            {
                throw new CommodityNewQuantityIsNegativeException("Выбрано отрицательное количество для товара!");
            }

            CommodityQuantity = newQuantity;
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion
    }

    /// <summary>
    /// Класс, содержащий поля, необходимые для привязки данных в Главном Окне.
    /// </summary>
    public class UserProperty
    {
        #region Поля класса.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Поле, содержащее ID товара у данного пользователя.
        /// </summary>
        Int32 id;

        /// <summary>
        /// Сам товар, которым владеет пользователь.
        /// </summary>
        Commodity commodity;

        /// <summary>
        /// Поле, содержащее имя владельца (Пользователь) товара.
        /// </summary>
        String ownerName;

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Свойства класса.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Свойство, содержащее ID товара у данного пользователя.
        /// </summary>
        public Int32 ID
        {
            get
            {
                return id;
            }

            private set
            {
                id = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее товар, которым владеет пользователь.
        /// </summary>
        public Commodity Commodity
        {
            get
            {
                return commodity;
            }

            private set
            {
                commodity = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее имя владельца товара.
        /// </summary>
        public String OwnerName
        {
            get
            {
                return ownerName;
            }

            private set
            {
                ownerName = value;
            }
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion

        #region Методы класса.
        //—————————————————————————————————————————————————————————————————————————————————————————

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="id">Идентификатор товара.</param>
        /// <param name="commodity">Сам товар.</param>
        /// <param name="ownerName">Имя владельца товара.</param>
        public UserProperty(Int32 id, Commodity commodity, String ownerName)
        {
            ID = id;
            Commodity = commodity;
            OwnerName = ownerName;
        }

        //—————————————————————————————————————————————————————————————————————————————————————————
        #endregion
    }

    /// <summary>
    /// Перечисление "Enum" для определения пола пользователя.
    /// </summary>
    public enum Gender : Int32
    {
        /// <summary>
        /// Значение для Мужского Пола.
        /// </summary>
        Male,

        /// <summary>
        /// Значение для Женского Пола.
        /// </summary>
        Female,

        /// <summary>
        /// Значение для Альтернативного Пола.
        /// </summary>
        Alternative
    }
}
