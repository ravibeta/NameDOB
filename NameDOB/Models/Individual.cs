using System;
using System.Linq;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[assembly: EdmSchemaAttribute()]
namespace NameDOB.Models
{
    #region Entities

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName = "NameDOBModel", Name = "Individual")]
    [Serializable()]
    [DataContractAttribute(IsReference = true)]
    public partial class Individual : EntityObject
    {
        #region Factory Method

        /// <summary>
        /// Create a new Individual object.
        /// </summary>
        /// <param name="id">Initial value of the ID property.</param>
        /// <param name="name">Initial value of the Name property.</param>
        public static Individual CreateIndividual(global::System.Int32 id, global::System.String name)
        {
            Individual individual = new Individual();
            individual.ID = id;
            individual.Name = name;
            return individual;
        }

        #endregion
        #region Primitive Properties

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [DataMemberAttribute()]
        public global::System.Int32 ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    OnIDChanging(value);
                    ReportPropertyChanging("ID");
                    _ID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("ID");
                    OnIDChanged();
                }
            }
        }
        private global::System.Int32 _ID;
        partial void OnIDChanging(global::System.Int32 value);
        partial void OnIDChanged();

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        public global::System.String DateOfBirth
        {
            get
            {
                return _DateOfBirth;
            }
            set
            {
                OnDateOfBirthChanging(value);
                ReportPropertyChanging("DateOfBirth");
                _DateOfBirth = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("DateOfBirth");
                OnDateOfBirthChanged();
            }
        }
        private global::System.String _DateOfBirth;
        partial void OnDateOfBirthChanging(global::System.String value);
        partial void OnDateOfBirthChanged();

        public string ValidateDateOfBirth()
        {
            var ddmmyyyy = DateOfBirth.Split(new char[] { '/' });
            if (ddmmyyyy.Count() != 3)
                return "DateOfBirth should be in mm/dd/yyyy format with / separator!";

            string maxlenformat = @"mm?/dd?/yyyy?";
            if (DateOfBirth.Length > maxlenformat.Length ||
                ddmmyyyy[0].Length > 3 || ddmmyyyy[1].Length > 3 || ddmmyyyy[2].Length > 5)
                return "DateOfBirth is too long or not in mm/dd/yyyy";

            int month, day, year;
            if (!Int32.TryParse(ddmmyyyy[0].Replace(@"?", ""), out month) ||
                    !Int32.TryParse(ddmmyyyy[1].Replace(@"?", ""), out day) ||
                    !Int32.TryParse(ddmmyyyy[2].Replace(@"?", ""), out year))
                return "Each part of the date should be expressed as a precise number (“1995”), an imprecise number (“1995?”) or an unknown number (“?”)";

            if (month > 12)
                return "Month should be less than 12";

            if (day > 31)
                return "Day should be less than 31";

            if (year < 1000 || year > System.DateTime.Today.Year)
                return "Year should be between 1000 and current year";

            return null;
        }

        #endregion

    }

    #endregion
    public class SpecialComparer : IComparer<string>
    {
        public int Compare(string dob1, string dob2)
        {
            var ddmmyyyy1 = dob1.Split(new char[] { '/' });
            var ddmmyyyy2 = dob2.Split(new char[] { '/' });
            int result = String.CompareOrdinal(ddmmyyyy1[2].TrimStart(new char[] { '0' }),
                                               ddmmyyyy2[2].TrimStart(new char[] { '0' })); // year
            if (result != 0) return result;
            result = String.CompareOrdinal(ddmmyyyy1[0].TrimStart(new char[] { '0' }),
                                           ddmmyyyy2[0].TrimStart(new char[] { '0' })); // month
            if (result != 0) return result;
            result = String.CompareOrdinal(ddmmyyyy1[1].TrimStart(new char[] { '0' }),
                                           ddmmyyyy2[1].TrimStart(new char[] { '0' })); // day
            return result;
        }
    }
}
