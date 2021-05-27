using System;

/// <summary>
/// Область с Исключениями.
/// </summary>
namespace DBApplication
{
    /// <summary>
    /// Класс, содержащий исключения.
    /// Возникает при попытке задать товару отрицательную цену.
    /// </summary>
    public class CommodityNewPriceIsNegativeException : Exception
    {
        /// <summary>
        /// Базовый конструктор.
        /// </summary>
        public CommodityNewPriceIsNegativeException()
        {
            //Базовый конструктор Исключения. Нужен для его работы.
        }

        /// <summary>
        /// Расширенный конструктор.
        /// </summary>
        /// <param name="message">Сообщение, возникающее при Исключении.</param>
        public CommodityNewPriceIsNegativeException(String message)
        : base(message)
        {
            //Перегрузка конструктора. Нужна для работы Исключения.
        }

        /// <summary>
        /// Продвинутый конструктор.
        /// </summary>
        /// <param name="message">Сообщение, возникающее при Исключении.</param>
        /// <param name="inner">Место появления Исключения.</param>
        public CommodityNewPriceIsNegativeException(String message, Exception inner)
        : base(message, inner)
        {
            //Еще один конструктор. Вновь нужен для работы Исключения.
        }
    }

    /// <summary>
    /// Класс, содержащий исключения.
    /// Возникает при попытке задать товару отрицательное количество.
    /// </summary>
    public class CommodityNewQuantityIsNegativeException : Exception
    {
        /// <summary>
        /// Базовый конструктор.
        /// </summary>
        public CommodityNewQuantityIsNegativeException()
        {
            //Базовый конструктор Исключения. Нужен для его работы.
        }

        /// <summary>
        /// Расширенный конструктор.
        /// </summary>
        /// <param name="message">Сообщение, возникающее при Исключении.</param>
        public CommodityNewQuantityIsNegativeException(String message)
        : base(message)
        {
            //Перегрузка конструктора. Нужна для работы Исключения.
        }

        /// <summary>
        /// Продвинутый конструктор.
        /// </summary>
        /// <param name="message">Сообщение, возникающее при Исключении.</param>
        /// <param name="inner">Место появления Исключения.</param>
        public CommodityNewQuantityIsNegativeException(String message, Exception inner)
        : base(message, inner)
        {
            //Еще один конструктор. Вновь нужен для работы Исключения.
        }
    }

    /// <summary>
    /// Класс, содержащий исключения.
    /// Возникает при попытке получить несуществующего пользователя из Базы Данных.
    /// </summary>
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// Базовый конструктор.
        /// </summary>
        public UserNotFoundException()
        {
            //Базовый конструктор Исключения. Нужен для его работы.
        }

        /// <summary>
        /// Расширенный конструктор.
        /// </summary>
        /// <param name="message">Сообщение, возникающее при Исключении.</param>
        public UserNotFoundException(String message)
        : base(message)
        {
            //Перегрузка конструктора. Нужна для работы Исключения.
        }

        /// <summary>
        /// Продвинутый конструктор.
        /// </summary>
        /// <param name="message">Сообщение, возникающее при Исключении.</param>
        /// <param name="inner">Место появления Исключения.</param>
        public UserNotFoundException(String message, Exception inner)
        : base(message, inner)
        {
            //Еще один конструктор. Вновь нужен для работы Исключения.
        }
    }
}
