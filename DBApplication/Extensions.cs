using System;

/// <summary>
/// Область с Методами Расширения.
/// </summary>
namespace DBApplication
{
    /// <summary>
    /// Класс, содержащий различные Методы Расширения.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Метод для сравнения двух дат с последующим возвращением количества прошедших лет.
        /// </summary>
        /// <param name="firstDate">Стартовая дата.</param>
        /// <param name="secondDate">Итоговая дата.</param>
        /// <returns>Количество прошедших лет.</returns>
        public static Int32 GetYearsFromDates (this DateTime firstDate, DateTime secondDate)
        {
            if (firstDate.Month > secondDate.Month)
            {
                return Math.Abs(firstDate.Year - secondDate.Year) - 1;
            }

            else
            {
                return Math.Abs(firstDate.Year - secondDate.Year);
            }
        }

        /// <summary>
        /// Метод для получения элемента перечисления Gender по его Объектному Представлению.
        /// </summary>
        /// <param name="obj">Объект, который необходимо преобразовать.</param>
        /// <returns>Соответственный элемент перечисления "Gender".</returns>
        public static Gender GetGenderFromString (this Object obj)
        {
            switch ((obj as String).ToUpper())
            {
                case "МУЖСКОЙ":
                    return Gender.Male;

                case "ЖЕНСКИЙ":
                    return Gender.Female;

                default:
                    return Gender.Alternative;
            }
        }

        /// <summary>
        /// Метод для получения элемента перечисления Gender по его Строковому Представлению.
        /// </summary>
        /// <param name="str">Строка, которую необходимо преобразовать.</param>
        /// <returns>Соответственный элемент перечисления "Gender".</returns>
        public static Gender GetGenderFromString(this String str)
        {
            switch (str.ToUpper())
            {
                case "МУЖСКОЙ":
                    return Gender.Male;

                case "ЖЕНСКИЙ":
                    return Gender.Female;

                default:
                    return Gender.Alternative;
            }
        }

        /// <summary>
        /// Метод для получения Строкового Представления Gender по его значению.
        /// </summary>
        /// <param name="obj">Элемент перечисления, который необходимо преобразовать.</param>
        /// <returns>Строковое представление элемента перечисления.</returns>
        public static String GetStringFromGender(this Gender obj)
        {
            switch (obj)
            {
                case Gender.Male:
                    return "Мужской";

                case Gender.Female:
                    return "Женский";

                default:
                    return "Альтернативный";
            }
        }
    }
}
