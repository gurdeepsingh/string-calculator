using System;
using System.Collections.Generic;
using System.Linq;
using StringCalculator.Interfaces;

namespace StringCalculator.Services
{
    public class StringCalculatorService : IStringCalculatorService
    {
        private const int DefaultValue = 0;
        private readonly List<string> _defaultDelimiters = new List<string> { ",", "\n" };
        private const int StartIndexWithCustomDelimiter = 3;
        private const int StartIndexOfCustomDelimiter = 2;
        private const int MaxNumberLimit = 1000;
        private const string CustomDelimiterIdentifier = "//";

        public int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers)) return DefaultValue;

            if (numbers.StartsWith(CustomDelimiterIdentifier))
            {
                numbers = GetNumbersExcludingCustomDelimiter(numbers);
            }

            var sumOfNumbers = GetSumOfNumbers(numbers);

            return sumOfNumbers;
        }
        private int GetSumOfNumbers(string numbers)
        {
            var numbersArray = numbers.Split(_defaultDelimiters.ToArray(), StringSplitOptions.None).Select(int.Parse).ToList();

            ValidateNumbers(numbersArray);

            var sumOfNumbers = numbersArray.Where(x => x <= MaxNumberLimit).Sum();
            return sumOfNumbers;
        }

        private string GetNumbersExcludingCustomDelimiter(string numbers)
        {
            var startIndexOfString = AssignCustomDelimiterAndReturnStartIndexOfNumbers(numbers);

            numbers = numbers.Substring(startIndexOfString);
            return numbers;
        }
        private int AssignCustomDelimiterAndReturnStartIndexOfNumbers(string numbers)
        {
            var customDelimiters = GetCustomDelimiter(numbers);
            _defaultDelimiters.AddRange(customDelimiters);

            var hasMultipleDelimiters = customDelimiters.Count > 1;
            var multipleDelimiterLength = hasMultipleDelimiters ? (customDelimiters.Count * 2) : 0;

            return StartIndexWithCustomDelimiter + customDelimiters.Sum(x => x.Length) + multipleDelimiterLength;
        }

        private static void ValidateNumbers(IReadOnlyCollection<int> convertedNumbers)
        {
            if (!convertedNumbers.Any(x => x < 0)) return;

            var negativeNumbers = string.Join(",", convertedNumbers.Where(x => x < 0).Select(x => x.ToString()).ToArray());
            throw new FormatException($"negatives not allowed '{negativeNumbers}'");
        }

        private static IList<string> GetCustomDelimiter(string numbers)
        {
            var allDelimiters = numbers.Substring(StartIndexOfCustomDelimiter, numbers.IndexOf('\n') - StartIndexOfCustomDelimiter);

            var splitDelimiters = allDelimiters.Split('[').Select(x => x.TrimEnd(']')).ToList();

            if (splitDelimiters.Contains(string.Empty))
            {
                splitDelimiters.Remove(string.Empty);
            }

            return splitDelimiters;
        }


    }
}

