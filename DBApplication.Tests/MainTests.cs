using System;
using Xunit;

namespace DBApplication.Tests
{
    /// <summary>
    /// Класс для Тестирования.
    /// </summary>
    public class MainTests
    {
        /// <summary>
        /// Тест для проверки Метода Расширения ".GetYearsFromDates()".
        /// </summary>
        [Fact]
        public void GetYearsFromDates_Send2003_Return17()
        {
            DateTime firstTime = new DateTime(2003, 12, 01);
            DateTime secondTime = DateTime.Now;

            Assert.Equal(17, firstTime.GetYearsFromDates(secondTime));
        }

        /// <summary>
        /// Тест для проверки свойства "CommodityPrice" у структуры Commodity.
        /// </summary>
        [Fact]
        public void ChangePrice_SendMinus200_ReturnException()
        {
            try
            {
                Commodity testCommodity = new Commodity("Test", (Decimal)200.4, (Decimal)20.5, 10);

                testCommodity.CommodityPrice = Convert.ToDecimal(-200);
            }

            catch (CommodityNewPriceIsNegativeException except)
            {
                Assert.Equal(new CommodityNewPriceIsNegativeException().GetType(), except.GetType());
            }
        }

        /// <summary>
        /// Тест для проверки свойства "CommodityQuantity" у структуры Commodity. 
        /// </summary>
        [Fact]
        public void ChangeQuantity_SendMinus5_ReturnException()
        {
            try
            {
                Commodity test = new Commodity("Test", (Decimal)200.4, (Decimal)300.2, 10);

                test.CommodityQuantity = -5;
            }

            catch (CommodityNewQuantityIsNegativeException except)
            {
                Assert.Equal(new CommodityNewQuantityIsNegativeException().GetType(), except.GetType());
            }
        }

        /// <summary>
        /// Тест для проверки метода ".GetStringFromGender()".
        /// </summary>
        [Fact]
        public void GetStringFromGender_SendGenderMale_ReturnМужской()
        {
            User test = new User("Test", "testPass", Gender.Male, DateTime.Now);

            Assert.Equal("Мужской", test.UserGender.GetStringFromGender());
        }
    }
}
