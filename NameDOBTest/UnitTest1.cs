using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using NameDOB.Models;

namespace NameDOBTest
{

    public class with_a_specialcomparer
    {
        protected static List<Individual> roster = new List<Individual>();
        protected static IComparer<string> comparer = new SpecialComparer();
        protected static List<Individual> sorted;

    }

    public class compare_with_different_DOBs : with_a_specialcomparer
    {
        Establish context = () =>
        {
            roster.Clear();
            roster.Add(new Individual() { Name = "Krystina Gorchez", DateOfBirth = "03/11/1985" });
            roster.Add(new Individual() { Name = "Sheryl Mucho", DateOfBirth = "09/12/1978" });
            roster.Add(new Individual() { Name = "Colt Henderson", DateOfBirth = "02/19/1989" });
        };

        Because of = () =>
        {
            sorted = roster.OrderBy(x => x.DateOfBirth, comparer).Reverse().ToList();
        };

        It should_have_the_youngest_as_the_first = () =>
        { sorted.First().DateOfBirth.ShouldEqual(roster.Last().DateOfBirth); };

        It should_have_the_eldest_as_the_last = () =>
        { sorted.Last().DateOfBirth.ShouldEqual(roster.ElementAt(1).DateOfBirth); };
    }

    public class compare_with_padded_DOBs : with_a_specialcomparer
    {
        Establish context = () =>
        {
            roster.Clear();
            roster.Add(new Individual() { Name = "Sophie Norse", DateOfBirth = "11/4/1976?" });
            roster.Add(new Individual() { Name = "Ramona Bay", DateOfBirth = "11/05/1976?" });
        };

        Because of = () =>
        {
            sorted = roster.OrderBy(x => x.DateOfBirth, comparer).Reverse().ToList();
        };

        It should_have_the_youngest_as_the_first = () =>
        { sorted.First().DateOfBirth.ShouldEqual(roster.Last().DateOfBirth); };

    }

    public class compare_with_same_datepart_DOBs : with_a_specialcomparer
    {
        Establish context = () =>
        {
            roster.Clear();
            roster.Add(new Individual() { Name = "Kate Veer", DateOfBirth = "11/4/1976?" });
            roster.Add(new Individual() { Name = "Mia Hong", DateOfBirth = "12/5/1976?" });
            roster.Add(new Individual() { Name = "Pnomph Thempet", DateOfBirth = "11/3/1976?" });
            roster.Add(new Individual() { Name = "Nicole Suede", DateOfBirth = "11/3/1975" });
        };

        Because of = () =>
        {
            sorted = roster.OrderBy(x => x.DateOfBirth, comparer).Reverse().ToList();
        };

        It should_compare_the_year_first = () =>
        { sorted.First().DateOfBirth.ShouldNotContain("1975"); };

        It should_compare_the_month_second = () =>
        { sorted.First().DateOfBirth.ShouldContain("12"); };

        It should_compare_the_day_third = () =>
        { sorted.ElementAt(1).DateOfBirth.ShouldContain("4"); };

    }

    public abstract class with_an_individual
    {
        protected static Individual person = new Individual();
        protected static string error;

        Because of = () =>
            error = person.ValidateDateOfBirth();
    }

    public class when_DOB_has_no_delimiter : with_an_individual
    {

        Establish context = () =>
        {
            person.Name = "Frida Lay";
            person.DateOfBirth = "1973";
        };

        It should_error_with_delimiter = () =>
            error.ShouldContain("DateOfBirth should be in mm/dd/yyyy format with / separator!");

    }

    public class when_DOB_has_one_delimiter : with_an_individual
    {

        Establish context = () =>
        {
            person.Name = "Frida Lay";
            person.DateOfBirth = "09/1973";
        };

        It should_error_with_delimiter = () =>
            error.ShouldContain("DateOfBirth should be in mm/dd/yyyy format with / separator!");

    }

    public class when_DOB_is_too_long : with_an_individual
    {
        Establish context = () =>
        {
            person.Name = "Frida Lay";
            person.DateOfBirth = "0999?/03333/1973???";
        };

        It should_error_with_delimiter = () =>
            error.ShouldContain("DateOfBirth is too long or not in mm/dd/yyyy");
    }

    public class when_DOB_month_is_too_long : with_an_individual
    {
        Establish context = () =>
        {
            person.Name = "Frida Lay";
            person.DateOfBirth = "0999?/03?/1973?";
        };

        It should_error_with_delimiter = () =>
            error.ShouldContain("DateOfBirth is too long or not in mm/dd/yyyy");
    }

    public class when_DOB_day_is_too_long : with_an_individual
    {
        Establish context = () =>
        {
            person.Name = "Frida Lay";
            person.DateOfBirth = "09?/03333?/1973?";
        };

        It should_error_with_delimiter = () =>
            error.ShouldContain("DateOfBirth is too long or not in mm/dd/yyyy");
    }

    public class when_DOB_year_is_too_long : with_an_individual
    {
        Establish context = () =>
        {
            person.Name = "Frida Lay";
            person.DateOfBirth = "09?/03?/19733333?";
        };

        It should_error_with_delimiter = () =>
            error.ShouldContain("DateOfBirth is too long or not in mm/dd/yyyy");
    }

    public class when_DOB_month_has_invalid_chars : with_an_individual
    {
        Establish context = () =>
        {
            person.Name = "Frida Lay";
            person.DateOfBirth = "bb?/03?/1973?";
        };

        It should_error_with_delimiter = () =>
            error.ShouldContain("Each part of the date should be expressed as a precise number (“1995”), an imprecise number (“1995?”) or an unknown number (“?”)");
    }

    public class when_DOB_day_has_invalid_chars : with_an_individual
    {
        Establish context = () =>
        {
            person.Name = "Frida Lay";
            person.DateOfBirth = "09?/ab?/1973?";
        };

        It should_error_with_delimiter = () =>
            error.ShouldContain("Each part of the date should be expressed as a precise number (“1995”), an imprecise number (“1995?”) or an unknown number (“?”)");
    }

    public class when_DOB_year_has_invalid_chars : with_an_individual
    {
        Establish context = () =>
        {
            person.Name = "Frida Lay";
            person.DateOfBirth = "09?/03?/bbbb?";
        };

        It should_error_with_delimiter = () =>
            error.ShouldContain("Each part of the date should be expressed as a precise number (“1995”), an imprecise number (“1995?”) or an unknown number (“?”)");
    }

    public class when_DOB_month_is_invalid : with_an_individual
    {
        Establish context = () =>
        {
            person.Name = "Frida Lay";
            person.DateOfBirth = "13/03?/1973?";
        };

        It should_error_with_delimiter = () =>
            error.ShouldContain("Month should be less than 12");
    }

    public class when_DOB_day_is_invalid : with_an_individual
    {
        Establish context = () =>
        {
            person.Name = "Frida Lay";
            person.DateOfBirth = "12/33?/1973?";
        };

        It should_error_with_delimiter = () =>
            error.ShouldContain("Day should be less than 31");
    }


    public class when_DOB_year_is_invalid_on_the_low_side : with_an_individual
    {
        Establish context = () =>
        {
            person.Name = "Frida Lay";
            person.DateOfBirth = "12/03?/999?";
        };

        It should_error_with_delimiter = () =>
            error.ShouldContain("Year should be between 1000 and current year");
    }


    public class when_DOB_year_is_invalid_on_the_high_side : with_an_individual
    {
        Establish context = () =>
        {
            person.Name = "Frida Lay";
            int nextYear = System.DateTime.Today.Year + 1;
            person.DateOfBirth = "12/03?/" + nextYear.ToString();
        };

        It should_error_with_delimiter = () =>
            error.ShouldContain("Year should be between 1000 and current year");
    }
}
