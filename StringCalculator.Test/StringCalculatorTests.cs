using System;
using StringCalculator.Interfaces;
using StringCalculator.Services;
using Xunit;

namespace StringCalculator.Tests
{
    public class StringCalculatorTests
    {
        private readonly IStringCalculatorService _service;
        public StringCalculatorTests()
        {
            _service = new StringCalculatorService(); 
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData(null,  0)]
        public void Add_EmptyString_Input_returns_0(string numbers, int expected)
        {
            var result = _service.Add(numbers);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1",  1)]
        [InlineData("22",  22)]
        public void Add_ValidNumber_Input_then_Returns_SameNumber(string numbers, int expected)
        {
            var result = _service.Add(numbers);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1,5",  6)]
        [InlineData("11,12",  23)]
        public void Add_Two_ValidNumbers_Returns_SumOf_TwoNumbers(string numbers, int expected)
        {
            var result = _service.Add(numbers);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1,2,3,4",  10)]
        [InlineData("11,12,13,14",  50)]
        public void Add_Multiple_ValidNumbers_then_Returns_SumOf_Numbers(string numbers, int expected)
        {
            var result = _service.Add(numbers);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1\n2",  3)]
        [InlineData("1\n2,3",  6)]
        [InlineData("1\n1,4",  6)]
        [InlineData("1\n5,4",  10)]
        public void Add_NewLine_Delimiter_Used_then_Returns_CorectSum(string numbers, int expected)
        {
            var result = _service.Add(numbers);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("//$\n1",  1)]
        [InlineData("//$\n1$2",  3)]
        [InlineData("//$\n1$2,3",  6)]
        [InlineData("//$\n1$2,3\n4",  10)]
        [InlineData("//$\n1$2,3\n4$5",  15)]
        public void Add_Custom_Delimiter_Used_then_Returns_CorectSum(string numbers, int expected)
        {
            var result = _service.Add(numbers);
            Assert.Equal(expected, result);
        }
        

        [Theory]
        [InlineData("-1", "-1")]
        [InlineData("1,-1", "-1")]
        [InlineData("1\n-1", "-1")]
        [InlineData("//$\n-1", "-1")]
        [InlineData("//$\n1$-2", "-2")]
        [InlineData("//$\n1$-2,3", "-2")]
        [InlineData("//$\n1$-2,-3\n4", "-2,-3")]
        [InlineData("//$\n1$2,3\n4$-5", "-5")]
        public void Add_NegativeNumber_Input_then_Throws_Correct_Exception(string numbers, string negativeNumbers)
        {
            //Act
            var ex = Assert.Throws<FormatException>(() => _service.Add(numbers));

            //Assert
            ex.Message.Equals($"negatives not allowed '{negativeNumbers}'");
        }

        [Theory]
        [InlineData("1001",  0)]
        [InlineData("2,1001,13", 15)]
        [InlineData("1\n1001",  1)]
        [InlineData("//$\n1,1001",  1)]
        [InlineData("//$\n1$1001",  1)]
        [InlineData("//$\n1$2,1001",  3)]
        public void Add_Ignoring_Numbers_GreaterThan_1000_then_Returns_CorrectSum(string numbers, int expected)
        {
            var result = _service.Add(numbers);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("//;\n1;1",  2)]
        [InlineData("//$\n1$1$1",  3)]
        public void Add_Custom_Delimiters_Of_Any_Length_then_Returns_CorrectSum(string numbers, int expected)
        {
            var result = _service.Add(numbers);
            Assert.Equal(expected, result);
        }

    }
}
