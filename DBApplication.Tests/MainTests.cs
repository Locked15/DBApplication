using System;
using Xunit;

namespace DBApplication.Tests
{
    /// <summary>
    /// ����� ��� ������������.
    /// </summary>
    public class MainTests
    {
        /// <summary>
        /// ���� ��� �������� ������ ���������� ".GetYearsFromDates()".
        /// </summary>
        [Fact]
        public void GetYearsFromDates_Send2003_Return17()
        {
            DateTime firstTime = new DateTime(2003, 12, 01);
            DateTime secondTime = DateTime.Now;

            Assert.Equal(17, firstTime.GetYearsFromDates(secondTime));
        }

        /// <summary>
        /// ���� ��� �������� �������� "CommodityPrice" � ��������� Commodity.
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
        /// ���� ��� �������� �������� "CommodityQuantity" � ��������� Commodity. 
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
        /// ���� ��� �������� ������ ".GetStringFromGender()".
        /// </summary>
        [Fact]
        public void GetStringFromGender_SendGenderMale_Return�������()
        {
            User test = new User("Test", "testPass", Gender.Male, DateTime.Now);

            Assert.Equal("�������", test.UserGender.GetStringFromGender());
        }
    }
}
