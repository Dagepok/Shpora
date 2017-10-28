using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercises
{
    public class NumberValidatorTests
    {
        [Test]
        public void NumberValitdatorConstructor_DoesNotThrowsException_WhenPositiveArgs()
        {
            Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));
        }

        [Test]
        public void NumberValitdatorConstructor_ThrowsArgumentException_WhenNegativePrecision()
        {
            Assert.Throws<ArgumentException>(() => new NumberValidator(-1, 2));
        }

        [Test]
        public void NumberValitdatorConstructor_ThrowsArgumentException_WhenNegativeScale()
        {
            Assert.Throws<ArgumentException>(() => new NumberValidator(1, -2));
        }


        [TestCase(17, 2, true, "0,0", TestName = "When separor is comma")]
        [TestCase(17, 2, true, "0", TestName = "When number is integer")]
        [TestCase(4, 2, true, "+1.23", TestName = "When only positive")]
        [TestCase(4, 2, false, "-1.23", TestName = "When not only positive")]
        public void IsValidNumber_ReturnTrue(int precision, int scale, bool onlyPositive, string number)
        {
            Assert.IsTrue(new NumberValidator(precision, scale, onlyPositive).IsValidNumber(number));
        }


        [TestCase(3, 2, "a.sd", TestName = "When Number without Digits")]
        [TestCase(3, 2, "1:23", TestName = "When separator is not dot or comma")]
        public void IsValidNumber_ReturnFalse_WhenWrongNumberFormat(int precision, int scale, string number)
        {
            Assert.IsFalse(new NumberValidator(precision, scale, true).IsValidNumber(number));
        }

        [Test]
        public void IsValidNumber_ReturnFalse_WhenWrongScale()
        {
            Assert.IsFalse(new NumberValidator(17, 2, true).IsValidNumber("0.000"));
        }

        [TestCase(3, 2, "-0.00", TestName = "With Sign")]
        [TestCase(3, 2, "00.00", TestName = "Without sign")]
        public void IsValidNumber_ReturnFalse_WhenWrongPrecision(int precision, int scale, string number)
        {
            Assert.IsFalse(new NumberValidator(precision, scale).IsValidNumber(number));
        }

        [Test]
        public void IsValidNumber_ReturnFalse_WhenWrongSign()
        {
            Assert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("-0.00"));
        }

        [TestCase(3, 2, "", TestName = "When Number is empty")]
        [TestCase(3, 2, null, TestName = "When Number is null")]
        public void IsValidNumber_ReturnFalse_WhenNumberIsNullOrEmpty(int precision, int scale, string number)
        {
            Assert.IsFalse(new NumberValidator(precision, scale, true).IsValidNumber(number));
        }
    }

    public class NumberValidator
    {
        private readonly Regex numberRegex;
        private readonly bool onlyPositive;
        private readonly int precision;
        private readonly int scale;

        public NumberValidator(int precision, int scale = 0, bool onlyPositive = false)
        {
            this.precision = precision;
            this.scale = scale;
            this.onlyPositive = onlyPositive;
            if (precision <= 0)
                throw new ArgumentException("precision must be a positive number");
            if (scale < 0 || scale >= precision)
                throw new ArgumentException("precision must be a non-negative number less or equal than precision");
            numberRegex = new Regex(@"^([+-]?)(\d+)([.,](\d+))?$", RegexOptions.IgnoreCase);
        }

        public bool IsValidNumber(string value)
        {
            // Проверяем соответствие входного значения формату N(m,k), в соответствии с правилом, 
            // описанным в Формате описи документов, направляемых в налоговый орган в электронном виде по телекоммуникационным каналам связи:
            // Формат числового значения указывается в виде N(m.к), где m – максимальное количество знаков в числе, включая знак (для отрицательного числа), 
            // целую и дробную часть числа без разделяющей десятичной точки, k – максимальное число знаков дробной части числа. 
            // Если число знаков дробной части числа равно 0 (т.е. число целое), то формат числового значения имеет вид N(m).

            if (string.IsNullOrEmpty(value))
                return false;

            var match = numberRegex.Match(value);
            if (!match.Success)
                return false;

            // Знак и целая часть
            var intPart = match.Groups[1].Value.Length + match.Groups[2].Value.Length;
            // Дробная часть
            var fracPart = match.Groups[4].Value.Length;

            if (intPart + fracPart > precision || fracPart > scale)
                return false;

            if (onlyPositive && match.Groups[1].Value == "-")
                return false;
            return true;
        }
    }
}