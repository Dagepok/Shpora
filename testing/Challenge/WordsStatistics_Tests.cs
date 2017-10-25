using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace Challenge
{

    [TestFixture]
    public class WordsStatistics_Tests
    {
        public static string Authors = "Valitov Kolinichenko"; // "Egorov Shagalina"

        public virtual IWordsStatistics CreateStatistics()
        {
            // меняется на разные реализации при запуске exe
            return new WordsStatistics();
        }

        private IWordsStatistics statistics;

        [SetUp]
        public void SetUp()
        {
            statistics = CreateStatistics();
        }

        [Test]
        public void GetStatistics_IsEmpty_AfterCreation()
        {
            statistics.GetStatistics().Should().BeEmpty();
        }

        [Test]
        public void GetStatistics_ContainsItem_AfterAddition()
        {
            statistics.AddWord("abc");
            statistics.GetStatistics().Should().Equal(Tuple.Create(1, "abc"));
        }

        [Test]
        public void GetStatistics_ContainsManyItems_AfterAdditionOfDifferentWords()
        {
            statistics.AddWord("abc");
            statistics.AddWord("def");
            statistics.GetStatistics().Should().HaveCount(2);
        }

        [Test]
        public void GetStatistics_ContainsTwoSameItems_AfterAdditionOfDifferentWordsWithSamePrefices()
        {
            statistics.AddWord("abcdefghijklm");
            statistics.AddWord("abcdefghijkln");
            statistics.GetStatistics().Should().Contain(Tuple.Create(2, "abcdefghij"));
        }

        [Test]
        public void GetStatistics_ContainsTwoSameItems_AfterAdditionOfSameWordsDifferentCase()
        {
            statistics.AddWord("ABCDEF");
            statistics.AddWord("abcDEF");
            statistics.GetStatistics().Should().Contain(Tuple.Create(2, "abcdef"));
        }

        [Test]
        public void GetStatistics_SortedByDescendingCount_AfterAdditionOfTwoWordsAndOneMore()
        {
            statistics.AddWord("abc");
            statistics.AddWord("abc");
            statistics.AddWord("abb");
            statistics.GetStatistics().Should().ContainInOrder(Tuple.Create(2, "abc"), Tuple.Create(1, "abb"));
        }

        [Test]
        public void GetStatistics_IsEmpty_AfterAdditionWhiteSpaces()
        {
            statistics.AddWord("  ");
            statistics.GetStatistics().Should().BeEmpty();
        }

        [Test]
        public void GetStatistics_ContainsTwoSameItems_AfterAddiotionWordswithSamePrefixDifferentCase()
        {
            statistics.AddWord("abcdefghijklM");
            statistics.AddWord("abcdefghijkln");
            statistics.GetStatistics().Should().Contain(Tuple.Create(2, "abcdefghij"));
        }

        [Test]
        public void GetStatistics_ContainsItem_AfterAdditionWordsWithTenWhiteSpaces()
        {
            statistics.AddWord("          abc");
            statistics.GetStatistics().Should().Contain(Tuple.Create(1, "          "));
        }

        [Test]
        public void GetStatistics_ContainsItem_AfterAdditionRussionWord()
        {
            statistics.AddWord("РУССКИЙ");
            statistics.GetStatistics().Should().Contain(Tuple.Create(1, "русский"));
        }

        [Test]
        public void GetStatistics_IsEmpty_AfterAdditionEmptyWord()
        {
            statistics.AddWord("");
            statistics.GetStatistics().Should().BeEmpty();
        }

        [Test]
        public void GetStatistics_IsEmpty_AfterNoAddiotional()
        {
            statistics.GetStatistics().Should().BeEmpty();
        }

        [Test]
        public void GetStatisitcs_OrderByDesceding_AfterAlmostSameWordsWithAlmostSameCount()
        {
            statistics.AddWord("abc");
            statistics.AddWord("abc");
            statistics.AddWord("abd");
            statistics.AddWord("abd");
            statistics.AddWord("abe");
            statistics.GetStatistics().Should().ContainInOrder(new[] { Tuple.Create(2, "abc"), Tuple.Create(2, "abd"), Tuple.Create(1, "abe") });
        }

        [Test]
        public void GetStatistics_AfterGet()
        {
            //for (var i = 0; i < 12349; i++) 
            //    statistics.AddWord("abc");
            //statistics.GetStatistics().Should().Contain(Tuple.Create(12349, "abc"));
            statistics.AddWord("acc");
            statistics.AddWord("abd");
            statistics.GetStatistics().Should().HaveCount(2);
        }

        [Test]
        public void GetStatistics_UpperCase_AfterYO()
        {
            statistics.AddWord("Ё");
            statistics.GetStatistics().Should().Contain(Tuple.Create(1, "ё"));
        }

        [Test]
        public void GetStatistics_NotEmpty_AfterTowGetCalls()
        {
            statistics.AddWord("1");
            statistics.GetStatistics();
            statistics.GetStatistics().Should().HaveCount(1);
        }
        [Test, Timeout(1000)]
        public void GetStatistics_TImeOut_AfterManyWords()
        {
            for (var i = 0; i < 20000; i++)
            {
                statistics.AddWord(i.ToString());
            }
            statistics.GetStatistics().Should().Contain(Tuple.Create(1, "1"));
        }
        // Документация по FluentAssertions с примерами : https://github.com/fluentassertions/fluentassertions/wiki
    }
}