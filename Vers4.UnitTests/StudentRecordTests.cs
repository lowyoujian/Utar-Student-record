using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vers4;
using NUnit.Framework;

namespace Vers4.UnitTests
{
    [TestFixture]
    public class StudyUnitTests
    {

        [Test]
        // all fields same value except for first character in UnitCode
        [TestCase("uECS8888", "lucky subject", 10, UnitClassification.major, 50, false)]
        // all fields different value except for matching UnitCode strings
        [TestCase("UECS8888", "bad subject", 15, UnitClassification.elective, 20, true)]
        // mixture of matching / non-matching fields, with non-matching UnitCode strings
        [TestCase("UECS8887", "something else", 10, UnitClassification.compulsory, 50, false)]

        public void Equals_TestWithSameObjectType(string unitCode, string unitName, int creditHours, UnitClassification classification, int cwcomponent, bool expectEqual)
        {
            List<string> origString, compString;
            List<Trimesters> origTrimesters, compTrimesters;
            List<Campuses> origCampuses, compCampuses;

            origString = new List<string>();
            origTrimesters = new List<Trimesters>();
            origCampuses = new List<Campuses>();

            compString = new List<string>();
            compTrimesters = new List<Trimesters>();
            compCampuses = new List<Campuses>();

            origString.Add("dummy value");
            origTrimesters.Add(Trimesters.Y1T1);
            origCampuses.Add(Campuses.FES);

            StudyUnit originalUnit = new StudyUnit("UECS8888", "lucky subject", origString, 10, UnitClassification.major, origTrimesters, origCampuses, origString, 50);

            StudyUnit unitToCompare;

            if (expectEqual) // set comp** to have different values from orig***
            {
                compString.Add("some value");
                compTrimesters.Add(Trimesters.Y2T2);
                compCampuses.Add(Campuses.FCI);

                unitToCompare = new StudyUnit(unitCode, unitName, compString, creditHours, classification, compTrimesters, compCampuses, compString, cwcomponent);

                Assert.AreEqual(originalUnit, unitToCompare);
            }
            else // set comp** to have exactly same values as orig***
            {
                compString.Add("dummy value");
                compTrimesters.Add(Trimesters.Y1T1);
                compCampuses.Add(Campuses.FES);

                unitToCompare = new StudyUnit(unitCode, unitName, compString, creditHours, classification, compTrimesters, compCampuses, compString, cwcomponent);

                Assert.AreNotEqual(originalUnit, unitToCompare);
            }
        }

        [Test]
        public void Equals_TestWithDifferentObjectType()
        {

            List<string> origString = new List<string>();
            List<Trimesters> origTrimesters = new List<Trimesters>();
            List<Campuses> origCampuses = new List<Campuses>();

            origString.Add("dummy value");
            origTrimesters.Add(Trimesters.Y1T1);
            origCampuses.Add(Campuses.FES);

            StudyUnit originalUnit = new StudyUnit("UECS8888", "lucky subject", origString, 10, UnitClassification.major, origTrimesters, origCampuses, origString, 50);

            // Check Equals comparison with a variety of other object types,
            // all should return false
            Assert.AreNotEqual(originalUnit, "Some other type");
            Assert.AreNotEqual(originalUnit, 65);
            Assert.AreNotEqual(originalUnit, Trimesters.Y1T1);


        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "CheckAllUnitCodesUnique cannot be run with a null argument")]
        public void CheckAllUnitCodesUnique_TestWithNullList_ErrorMessage()
        {
            StudyUnit.CheckAllUnitCodesUnique(null);
        }


        [Test]
        public void CheckAllUnitCodesUnique_TestWithEmptyList_NoErrorMessage()
        {
            List<StudyUnit> studyUnits = new List<StudyUnit>();
            StudyUnit.CheckAllUnitCodesUnique(studyUnits);
        }

        [Test]
        public void CheckAllUnitCodesUnique_TestWithOneUnitCode_NoErrorMessage()
        {
            string[] unitCodesToUse = { "UECS8888" };
            StudyUnit.CheckAllUnitCodesUnique(CreateStudyUnitList(unitCodesToUse));
        }

        [Test]
        public void CheckAllUnitCodesUnique_TestWithThreeUniqueUnitCodes_NoErrorMessage()
        {
            string[] unitCodesToUse = { "UECS8888", "UECS9999", "UECS1111" };
            StudyUnit.CheckAllUnitCodesUnique(CreateStudyUnitList(unitCodesToUse));
        }



        [Test]
        [ExpectedException(typeof(ArgumentException),
        ExpectedMessage = "Duplicate unit UECS8888 detected\n")]
        public void CheckAllUnitCodesUnique_TestWith2SameUnitCodes_1ErrorMessage()
        {
            string[] unitCodesToUse = { "UECS8888", "UECS8888" };
            StudyUnit.CheckAllUnitCodesUnique(CreateStudyUnitList(unitCodesToUse));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
        ExpectedMessage = "Duplicate unit UECS8888 detected\n")]
        public void CheckAllUnitCodesUnique_TestWith4SameUnitCodes_1ErrorMessage()
        {
            string[] unitCodesToUse = { "UECS8888", "UECS8888", "UECS8888", "UECS8888" };
            StudyUnit.CheckAllUnitCodesUnique(CreateStudyUnitList(unitCodesToUse));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
        ExpectedMessage = "Duplicate unit UECS8888 detected\nDuplicate unit UECS9999 detected\n")]
        public void CheckAllUnitCodesUnique_TestWith2TypesOfSameUnitCodes_2ErrorMessages()
        {
            string[] unitCodesToUse = { "UECS8888", "UECS9999", "UECS8888", "UECS9999" };
            StudyUnit.CheckAllUnitCodesUnique(CreateStudyUnitList(unitCodesToUse));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
        ExpectedMessage = "Duplicate unit UECS8888 detected\nDuplicate unit UECS9999 detected\n")]
        public void CheckAllUnitCodesUnique_TestWith2TypesOfRepeatedSameUnitCodes_2ErrorMessages()
        {
            string[] unitCodesToUse = { "UECS8888", "UECS9999", "UECS8888", "UECS9999", "UECS8888", "UECS9999", "UECS1111" };
            StudyUnit.CheckAllUnitCodesUnique(CreateStudyUnitList(unitCodesToUse));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
        ExpectedMessage = "Duplicate unit UECS8888 detected\nDuplicate unit UECS9999 detected\nDuplicate unit UECS1111 detected\n")]
        public void CheckAllUnitCodesUnique_TestWith3TypesOfSameUnitCodes_3ErrorMessages()
        {
            string[] unitCodesToUse = { "UECS8888", "UECS9999", "UECS8888", "UECS9999", "UECS1111", "UECS1111" };
            StudyUnit.CheckAllUnitCodesUnique(CreateStudyUnitList(unitCodesToUse));
        }


        private List<StudyUnit> CreateStudyUnitList(string[] unitCodesToUse)
        {
            List<string> origString = new List<string>();
            List<Trimesters> origTrimesters = new List<Trimesters>();
            List<Campuses> origCampuses = new List<Campuses>();
            List<StudyUnit> studyUnits = new List<StudyUnit>();

            origString.Add("dummy value");
            origTrimesters.Add(Trimesters.Y1T1);
            origCampuses.Add(Campuses.FES);

            StudyUnit unitToAdd;
            for (int i = 0; i < unitCodesToUse.Length; i++)
            {
                unitToAdd = new StudyUnit(unitCodesToUse[i], "lucky subject", origString, 10, UnitClassification.major, origTrimesters, origCampuses, origString, 50);
                studyUnits.Add(unitToAdd);
            }
            return studyUnits;
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "CheckPrerequisitesCorrect cannot be run with a null argument")]
        public void CheckPrerequisitesCorrect_TestWithNullList_Exception()
        {
            StudyUnit.CheckPrerequisitesCorrect(null);
        }



        [Test]
        public void CheckPrerequisitesCorrect_TestWithEmptyList_NoErrorMessage()
        {
            List<StudyUnit> studyUnits = new List<StudyUnit>();
            StudyUnit.CheckPrerequisitesCorrect(studyUnits);
        }



        [Test] //4 units, all have no prerequisites
        public void CheckPrerequisitesCorrect_NoPrerequisites_NoException()
        {
            string[] codesForUnits = { "UECS1111", "UECS2222", "UECS3333" };

            List<string>[] preqsForUnits = new List<string>[codesForUnits.Length];
            for (int i = 0; i < preqsForUnits.Length; i++)
                preqsForUnits[i] = new List<String>();
            StudyUnit.CheckPrerequisitesCorrect(CreateStudyListForTesting(codesForUnits, preqsForUnits));

        }


        [Test] // 4 units, all have one prerequisite which is satisfied
        public void CheckPrerequisitesCorrect_OnePrerequisiteSatisfied_NoException()
        {
            string[] codesForUnits = { "UECS1111", "UECS2222", "UECS3333", "UECS4444" };

            List<string>[] preqsForUnits = new List<string>[codesForUnits.Length];
            for (int i = 0; i < preqsForUnits.Length; i++)
                preqsForUnits[i] = new List<String>();

            preqsForUnits[0].Add("UECS4444"); // preqlist for UECS1111
            preqsForUnits[1].Add("UECS3333"); // preqlist for UECS2222
            preqsForUnits[2].Add("UECS1111"); // preqlist for UECS3333
            preqsForUnits[3].Add("UECS2222"); // preqlist for UECS4444

            StudyUnit.CheckPrerequisitesCorrect(CreateStudyListForTesting(codesForUnits, preqsForUnits));

        }


        [Test] // 4 units, some have two or more prerequisites which are satisfied
        public void CheckPrerequisitesCorrect_TwoOrMorePrerequisiteSatisfied_NoException()
        {
            string[] codesForUnits = { "UECS1111", "UECS2222", "UECS3333", "UECS4444" };

            List<string>[] preqsForUnits = new List<string>[codesForUnits.Length];
            for (int i = 0; i < preqsForUnits.Length; i++)
                preqsForUnits[i] = new List<String>();

            preqsForUnits[0].Add("UECS4444"); // preqlist for UECS1111
            preqsForUnits[1].Add("UECS3333"); // preqlist for UECS2222

            preqsForUnits[2].Add("UECS1111"); // preqlist for UECS3333
            preqsForUnits[2].Add("UECS2222"); // preqlist for UECS3333

            preqsForUnits[3].Add("UECS1111"); // preqlist for UECS4444
            preqsForUnits[3].Add("UECS2222"); // preqlist for UECS4444
            preqsForUnits[3].Add("UECS3333"); // preqlist for UECS4444

            StudyUnit.CheckPrerequisitesCorrect(CreateStudyListForTesting(codesForUnits, preqsForUnits));

        }




        [Test] // 4 units with 1 prerequisite each, 2 have one prerequisite which is NOT satisfied
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Prerequisite UECS5555 for unit UECS1111 does not exist\nPrerequisite UECS6666 for unit UECS3333 does not exist\n")]

        public void CheckPrerequisitesCorrect_PrerequisiteNotSatisfiedForListOne_Exception()
        {
            string[] codesForUnits = { "UECS1111", "UECS2222", "UECS3333", "UECS4444" };

            List<string>[] preqsForUnits = new List<string>[codesForUnits.Length];
            for (int i = 0; i < preqsForUnits.Length; i++)
                preqsForUnits[i] = new List<String>();

            preqsForUnits[0].Add("UECS5555"); // preqlist for UECS1111 - not satisfied
            preqsForUnits[1].Add("UECS3333"); // preqlist for UECS2222- satisfied
            preqsForUnits[2].Add("UECS6666"); // preqlist for UECS3333 - not satisfied
            preqsForUnits[3].Add("UECS2222"); // preqlist for UECS4444- satisfied

            StudyUnit.CheckPrerequisitesCorrect(CreateStudyListForTesting(codesForUnits, preqsForUnits));
        }


        [Test] // 3 units with 2 prerequisites each, each with different results
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Prerequisite UECS5555 for unit UECS2222 does not exist\nPrerequisite UECS8888 for unit UECS3333 does not exist\nPrerequisite UECS9999 for unit UECS3333 does not exist\nUnit UECS3333 has the same prerequisite\n")]

        public void CheckPrerequisitesCorrect_PrerequisiteNotSatisfiedForListTwo_Exception()
        {
            string[] codesForUnits = { "UECS1111", "UECS2222", "UECS3333" };

            List<string>[] preqsForUnits = new List<string>[codesForUnits.Length];
            for (int i = 0; i < preqsForUnits.Length; i++)
                preqsForUnits[i] = new List<String>();

            preqsForUnits[0].Add("UECS2222"); // preqlist for UECS1111 - satisfied
            preqsForUnits[0].Add("UECS3333"); // preqlist for UECS1111 - satisfied

            preqsForUnits[1].Add("UECS3333"); // preqlist for UECS2222- satisfied
            preqsForUnits[1].Add("UECS5555"); // preqlist for UECS2222-  not satisfied

            preqsForUnits[2].Add("UECS8888"); // preqlist for UECS3333 - not satisfied
            preqsForUnits[2].Add("UECS9999"); // preqlist for UECS3333 - not satisfied
            preqsForUnits[2].Add("UECS3333");

            StudyUnit.CheckPrerequisitesCorrect(CreateStudyListForTesting(codesForUnits, preqsForUnits));

        }

        [Test] // 4 units, 2 have identical unit code and prerequisite 
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Unit UECS1111 has the same prerequisite\nUnit UECS3333 has the same prerequisite\n")]
        public void CheckPrerequisitesCorrect_OnePrerequisiteIdentical_Exception()
        {
            string[] codesForUnits = { "UECS1111", "UECS2222", "UECS3333", "UECS4444" };

            List<string>[] preqsForUnits = new List<string>[codesForUnits.Length];
            for (int i = 0; i < preqsForUnits.Length; i++)
                preqsForUnits[i] = new List<String>();

            preqsForUnits[0].Add("UECS1111"); // preqlist for UECS1111 - identical
            preqsForUnits[1].Add("UECS3333"); // preqlist for UECS2222
            preqsForUnits[2].Add("UECS3333"); // preqlist for UECS3333 - identical
            preqsForUnits[3].Add("UECS2222"); // preqlist for UECS4444

            StudyUnit.CheckPrerequisitesCorrect(CreateStudyListForTesting(codesForUnits, preqsForUnits));
        }


        private List<StudyUnit> CreateStudyListForTesting(string[] codesForUnits, List<string>[] preqsForUnits)
        {
            List<Trimesters> tempTrimester = new List<Trimesters>();
            List<Campuses> tempCampuses = new List<Campuses>();
            List<StudyUnit> studyUnits = new List<StudyUnit>();

            for (int i = 0; i < codesForUnits.Length; i++)
            {
                studyUnits.Add(new StudyUnit(codesForUnits[i], "dummy name", preqsForUnits[i], 10, UnitClassification.elective, tempTrimester, tempCampuses, new List<string>(), 30));
            }
            return studyUnits;

        }

    }

    [TestFixture]
    public class UndergradUnitTests
    {

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "CheckValidCreditHour cannot be run with a null argument")]
        public void CheckValidCreditHour_TestNull_Exception()
        {
            UndergradUnit.CheckValidCreditHour(null);
        }

        [Test] // Test within the valid range of max and min credit hours
        public void CheckValidCreditHour_CreditHrsWithinRange_NoException()
        {
            List<Trimesters> tempTrimester = new List<Trimesters>();
            List<Campuses> tempCampuses = new List<Campuses>();
            List<string> tempString = new List<string>();

            List<StudyUnit> studyUnits = new List<StudyUnit>();

            studyUnits.Add(new UndergradUnit("dummy1", "dummy", tempString, DefaultValues.defaultMaxCreditHr - 1, UnitClassification.major, tempTrimester, tempCampuses, tempString, 10));

            studyUnits.Add(new UndergradUnit("dummy2", "dummy", tempString, DefaultValues.defaultMinCreditHr + 1, UnitClassification.major, tempTrimester, tempCampuses, tempString, 10));

            UndergradUnit.CheckValidCreditHour(studyUnits);
        }

        [Test] // Test  at the exact Max and Min value allowed for credit hours
        // Boundary value analysis
        public void CheckValidCreditHour_CreditHrsAtBoundary_NoException()
        {
            List<Trimesters> tempTrimester = new List<Trimesters>();
            List<Campuses> tempCampuses = new List<Campuses>();
            List<string> tempString = new List<string>();

            List<StudyUnit> studyUnits = new List<StudyUnit>();

            studyUnits.Add(new UndergradUnit("dummy1", "dummy", tempString, DefaultValues.defaultMaxCreditHr, UnitClassification.major, tempTrimester, tempCampuses, tempString, 10));

            studyUnits.Add(new UndergradUnit("dummy2", "dummy", tempString, DefaultValues.defaultMinCreditHr, UnitClassification.major, tempTrimester, tempCampuses, tempString, 10));

            UndergradUnit.CheckValidCreditHour(studyUnits);
        }


        [Test] // Test at the value just one above Max and one below Min value allowed for credit hours
        // Boundary value analysis
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Invalid credit hours for unit dummy1\nInvalid credit hours for unit dummy2\n")]

        public void CheckValidCreditHour_InvalidCreditHrs_Exception()
        {
            List<Trimesters> tempTrimester = new List<Trimesters>();
            List<Campuses> tempCampuses = new List<Campuses>();
            List<string> tempString = new List<string>();

            List<StudyUnit> studyUnits = new List<StudyUnit>();

            studyUnits.Add(new UndergradUnit("dummy1", "dummy", tempString, DefaultValues.defaultMaxCreditHr + 1, UnitClassification.major, tempTrimester, tempCampuses, tempString, 10));

            studyUnits.Add(new UndergradUnit("dummy2", "dummy", tempString, DefaultValues.defaultMinCreditHr - 1, UnitClassification.major, tempTrimester, tempCampuses, tempString, 10));

            UndergradUnit.CheckValidCreditHour(studyUnits);
        }
    }

    [TestFixture]
    public class GradingSchemeUnitTests
    {
        [Test]
        public void GradingScheme_NoArgumentConstructor()
        {
            GradingScheme scheme = new GradingScheme();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "gradeRanges, gradeSymbols or gradePointValues cannot be null")]
        public void GradingScheme_ArgumentsNull_Exception()
        {
            GradingScheme scheme = new GradingScheme(null, null, null);
        }

        [Test] // Test different combinations of arrays of different length
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "gradeRanges, gradeSymbols and gradePointValues must be of the same length")]
        public void GradingScheme_PassArraysOfDifferentLengths1_Exception()
        {
            int[] gradeRanges = new int[4];
            string[] gradeSymbols = new string[4];
            double[] gradePointValues = new double[5];

            GradingScheme scheme = new GradingScheme(gradeRanges, gradeSymbols, gradePointValues);
        }


        [Test] // Test different combinations of arrays of different length
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "gradeRanges, gradeSymbols and gradePointValues must be of the same length")]
        public void GradingScheme_PassArraysOfDifferentLengths2_Exception()
        {
            int[] gradeRanges = new int[5];
            string[] gradeSymbols = new string[4];
            double[] gradePointValues = new double[4];

            GradingScheme scheme = new GradingScheme(gradeRanges, gradeSymbols, gradePointValues);
        }


        [Test] // Create GradeRanges array where the values are not in ascending order
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Elements in gradeRanges are not in ascending order")]
        public void GradingScheme_PassGradeRangesArrayNotInAscendingOrder_Exception()
        {
            int[] gradeRanges = { 10, 30, 40, 35, 90 };
            string[] gradeSymbols = new string[5];
            double[] gradePointValues = new double[5];

            GradingScheme scheme = new GradingScheme(gradeRanges, gradeSymbols, gradePointValues);
        }

        [Test] // Create GradeRanges array where the values are in ascending order
        public void GradingScheme_PassGradeRangesArrayInAscendingOrder()
        {
            int[] gradeRanges = { 10, 30, 40, 41, 90 };
            string[] gradeSymbols = new string[5];
            double[] gradePointValues = new double[5];

            GradingScheme scheme = new GradingScheme(gradeRanges, gradeSymbols, gradePointValues);
        }

    }

    [TestFixture]
    public class UnitGradeTests
    {
        [Test]
        // all fields same value except for first character in UnitCode
        [TestCase("uECS8888", "lucky subject", 10, false)]
        // all fields different value except for matching UnitCode
        [TestCase("UECS8888", "bad subject", 20, true)]

        public void Equals_TestWithSameObjectType(string unitCode, string unitName, int mark, bool expectEqual)
        {
            UnitGrade origGrade = new UnitGrade("UECS8888", "lucky subject", 10);
            UnitGrade compGrade = new UnitGrade(unitCode, unitName, mark);

            if (expectEqual)
                Assert.AreEqual(origGrade, compGrade);
            else
                Assert.AreNotEqual(origGrade, compGrade);
        }


        [Test] // test comparions with different types, including null
        // All should return false
        public void Equals_TestWithDifferentObjectType()
        {
            UnitGrade origGrade = new UnitGrade("UECS8888", "lucky subject", 10);
            Assert.AreNotEqual(origGrade, "UECS8888");
            Assert.AreNotEqual(origGrade, 20);
            Assert.AreNotEqual(origGrade, null);

        }

        [Test] // Test outside of the valid range for mark - boundary value analysis
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Mark for a unit must be between 0 to 100")]
        public void UnitGrade_MarkInvalidValue_Exception()
        {
            UnitGrade ugrade = new UnitGrade("UECS1111", "somename", 101);
        }

        [Test] // Test outside of the valid range for mark - boundary value analysis
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Invalid unit code or unit name to be created")]
        public void UnitGrade_UnitCodeInvalidValue_Exception()
        {
            UnitGrade ugrade = new UnitGrade(null, null, 35);
        }

        [Test] // do boundary value analysis for all the grade ranges
        public void CalculateGrade_UseDefaultValues_NoException()
        {
            UnitGrade ugrade;

            ugrade = new UnitGrade("unit", "12345", 49);
            ugrade.CalculateGrade();
            Assert.AreEqual("F", ugrade.gradeSymbol);
            Assert.AreEqual(0, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 50);
            ugrade.CalculateGrade();
            Assert.AreEqual("C", ugrade.gradeSymbol);
            Assert.AreEqual(2.0, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 54);
            ugrade.CalculateGrade();
            Assert.AreEqual("C", ugrade.gradeSymbol);
            Assert.AreEqual(2.0, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 55);
            ugrade.CalculateGrade();
            Assert.AreEqual("C+", ugrade.gradeSymbol);
            Assert.AreEqual(2.33, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 59);
            ugrade.CalculateGrade();
            Assert.AreEqual("C+", ugrade.gradeSymbol);
            Assert.AreEqual(2.33, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 60);
            ugrade.CalculateGrade();
            Assert.AreEqual("B-", ugrade.gradeSymbol);
            Assert.AreEqual(2.67, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 64);
            ugrade.CalculateGrade();
            Assert.AreEqual("B-", ugrade.gradeSymbol);
            Assert.AreEqual(2.67, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 65);
            ugrade.CalculateGrade();
            Assert.AreEqual("B", ugrade.gradeSymbol);
            Assert.AreEqual(3.0, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 69);
            ugrade.CalculateGrade();
            Assert.AreEqual("B", ugrade.gradeSymbol);
            Assert.AreEqual(3.0, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 70);
            ugrade.CalculateGrade();
            Assert.AreEqual("B+", ugrade.gradeSymbol);
            Assert.AreEqual(3.33, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 74);
            ugrade.CalculateGrade();
            Assert.AreEqual("B+", ugrade.gradeSymbol);
            Assert.AreEqual(3.33, ugrade.gradePoint);

            ugrade = new UnitGrade("unit3", "12345", 79);
            ugrade.CalculateGrade();
            Assert.AreEqual("A-", ugrade.gradeSymbol);
            Assert.AreEqual(3.67, ugrade.gradePoint);

            ugrade = new UnitGrade("unit3", "12345", 80);
            ugrade.CalculateGrade();
            Assert.AreEqual("A", ugrade.gradeSymbol);
            Assert.AreEqual(4.0, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 89);
            ugrade.CalculateGrade();
            Assert.AreEqual("A", ugrade.gradeSymbol);
            Assert.AreEqual(4.0, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 90);
            ugrade.CalculateGrade();
            Assert.AreEqual("A+", ugrade.gradeSymbol);
            Assert.AreEqual(4.0, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 100);
            ugrade.CalculateGrade();
            Assert.AreEqual("A+", ugrade.gradeSymbol);
            Assert.AreEqual(4.0, ugrade.gradePoint);

        }

        [Test] // do boundary value analysis for all the grade ranges
        // using a grade range supplied by user
        public void CalculateGrade_UseUserDefinedValues_NoException()
        {
            int[] gradeRanges = { 49, 69, 89, 100 };
            string[] gradeSymbols = { "F", "C", "B", "A" };
            double[] gradePointValues = { 0, 1.0, 2.0, 3.0 };
            GradingScheme myscheme = new GradingScheme(gradeRanges, gradeSymbols, gradePointValues);
            UnitGrade ugrade;

            ugrade = new UnitGrade("unit", "12345", 49);
            ugrade.gradingScheme = myscheme;
            ugrade.CalculateGrade();
            Assert.AreEqual("F", ugrade.gradeSymbol);
            Assert.AreEqual(0.0, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 50);
            ugrade.gradingScheme = myscheme;
            ugrade.CalculateGrade();
            Assert.AreEqual("C", ugrade.gradeSymbol);
            Assert.AreEqual(1.0, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 69);
            ugrade.gradingScheme = myscheme;
            ugrade.CalculateGrade();
            Assert.AreEqual("C", ugrade.gradeSymbol);
            Assert.AreEqual(1.0, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 70);
            ugrade.gradingScheme = myscheme;
            ugrade.CalculateGrade();
            Assert.AreEqual("B", ugrade.gradeSymbol);
            Assert.AreEqual(2.0, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 89);
            ugrade.gradingScheme = myscheme;
            ugrade.CalculateGrade();
            Assert.AreEqual("B", ugrade.gradeSymbol);
            Assert.AreEqual(2.0, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 90);
            ugrade.gradingScheme = myscheme;
            ugrade.CalculateGrade();
            Assert.AreEqual("A", ugrade.gradeSymbol);
            Assert.AreEqual(3.0, ugrade.gradePoint);

            ugrade = new UnitGrade("unit", "12345", 100);
            ugrade.gradingScheme = myscheme;
            ugrade.CalculateGrade();
            Assert.AreEqual("A", ugrade.gradeSymbol);
            Assert.AreEqual(3.0, ugrade.gradePoint);

        }


    }


    [TestFixture]
    public class StudentRecordTests
    {
        [Test]
        public void VerifyTrimesterOrder_NullTrimesterList_NoException()
        {
            StudentRecord studRecord = new StudentRecord("ahmad", "12345", Gender.male, "ahmad@gmail.com", "012-44444", "SE", 120);
            studRecord.trimesterPerformanceList = null;
            studRecord.VerifyTrimesterOrder();
        }


        [Test]
        public void VerifyTrimesterOrder_SingleItemInTrimesterList_NoException()
        {
            StudentRecord studRecord = new StudentRecord("ahmad", "12345", Gender.male, "ahmad@gmail.com", "012-44444", "SE", 120);
            TrimesterPerformance tpf = new TrimesterPerformance(2014, TrimesterMonths.JAN, Trimesters.Y1T1);
            studRecord.AddTrimesterPerformance(tpf);
            studRecord.VerifyTrimesterOrder();

            studRecord = new StudentRecord("ahmad", "12345", Gender.male, "ahmad@gmail.com", "012-44444", "SE", 120);
            tpf = new TrimesterPerformance(2016, TrimesterMonths.MAY, Trimesters.Y1T1);
            studRecord.AddTrimesterPerformance(tpf);
            studRecord.VerifyTrimesterOrder();

        }


        [Test]
        public void VerifyTrimesterOrder_CompleteTrimestersSequence_NoException()
        {
            StudentRecord studRecord = new StudentRecord("ahmad", "12345", Gender.male, "ahmad@gmail.com", "012-44444", "SE", 120);
            TrimesterPerformance tpf = new TrimesterPerformance(2014, TrimesterMonths.JAN, Trimesters.Y1T1);
            studRecord.AddTrimesterPerformance(tpf);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            studRecord.AddTrimesterPerformance(tpf);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.OCT, Trimesters.Y1T3);
            studRecord.AddTrimesterPerformance(tpf);
            tpf = new TrimesterPerformance(2015, TrimesterMonths.JAN, Trimesters.Y2T1);
            studRecord.AddTrimesterPerformance(tpf);
            tpf = new TrimesterPerformance(2015, TrimesterMonths.MAY, Trimesters.Y2T2);
            studRecord.AddTrimesterPerformance(tpf);
            tpf = new TrimesterPerformance(2015, TrimesterMonths.OCT, Trimesters.Y2T3);
            studRecord.AddTrimesterPerformance(tpf);
            tpf = new TrimesterPerformance(2016, TrimesterMonths.JAN, Trimesters.Y3T1);
            studRecord.AddTrimesterPerformance(tpf);
            tpf = new TrimesterPerformance(2016, TrimesterMonths.MAY, Trimesters.Y3T2);
            studRecord.AddTrimesterPerformance(tpf);
            tpf = new TrimesterPerformance(2016, TrimesterMonths.OCT, Trimesters.Y3T3);
            studRecord.AddTrimesterPerformance(tpf);
            studRecord.VerifyTrimesterOrder();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Incorrect month order detected in trimester 2014 JAN Y1T3 for student ID : 12345\n")]
        public void VerifyTrimesterOrder_OneItemOutOfOrderInTrimesterList_Exception()
        {
            StudentRecord studRecord = new StudentRecord("ahmad", "12345", Gender.male, "ahmad@gmail.com", "012-44444", "SE", 120);
            TrimesterPerformance tpf = new TrimesterPerformance(2014, TrimesterMonths.JAN, Trimesters.Y1T1);
            studRecord.AddTrimesterPerformance(tpf);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            studRecord.AddTrimesterPerformance(tpf);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.JAN, Trimesters.Y1T3);
            studRecord.AddTrimesterPerformance(tpf);
            studRecord.VerifyTrimesterOrder();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Incorrect year order detected in trimester 2016 MAY Y1T2 for student ID : 12345\nIncorrect month order detected in trimester 2014 JAN Y2T3 for student ID : 12345\nIncorrect trimester order detected in trimester 2014 JAN Y2T3 for student ID : 12345\n")]
        public void VerifyTrimesterOrder_YearMonthAndTrimesterOutOfOrderInTrimesterList_Exception()
        {
            StudentRecord studRecord = new StudentRecord("ahmad", "12345", Gender.male, "ahmad@gmail.com", "012-44444", "SE", 120);
            TrimesterPerformance tpf = new TrimesterPerformance(2014, TrimesterMonths.JAN, Trimesters.Y1T1);
            studRecord.AddTrimesterPerformance(tpf);
            tpf = new TrimesterPerformance(2016, TrimesterMonths.MAY, Trimesters.Y1T2);
            studRecord.AddTrimesterPerformance(tpf);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.JAN, Trimesters.Y2T3);
            studRecord.AddTrimesterPerformance(tpf);
            studRecord.VerifyTrimesterOrder();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "CheckPrerequisites cannot be run with a null argument")]
        public void CheckPrerequisites_NullStudyList_Exception()
        {
            StudentRecord studRecord = generateStudentRecordWithSampleTrimesters();
            studRecord.CheckPrerequisites(null);
        }

        [Test]
        public void CheckPrerequisites_TestWithEmptyList_NoErrorMessage()
        {
            StudentRecord studRecord = generateStudentRecordWithSampleTrimesters();
            List<StudyUnit> studyUnits = new List<StudyUnit>();
            studRecord.CheckPrerequisites(studyUnits);
        }



        [Test]
        public void CheckPrerequisites_StudyListWithoutPrerequisites_NoException()
        {
            List<StudyUnit> studyList = generateStudyListWithoutPrerequisites();
            StudentRecord studRecord = generateStudentRecordWithSampleTrimesters();

            studRecord.CheckPrerequisites(studyList);
        }

        [Test]
        public void CheckPrerequisites_StudyListWithMultiplePrerequisitesMet_NoException()
        {
            List<StudyUnit> studyList = generateStudyListWithoutPrerequisites();
            List<string> preReqs;
            
            // start adding in prerequisites that will be be met
            preReqs = new List<string>();
            preReqs.Add("UECS1102");
            preReqs.Add("UECS1103");
            studyList[4].prerequisites = preReqs; // add as prerequisites to UECS1202

            preReqs = new List<string>();
            preReqs.Add("UECS1201");
            studyList[6].prerequisites = preReqs; // add as prerequisites to UECS1301

            preReqs = new List<string>();
            preReqs.Add("UECS1303");
            studyList[9].prerequisites = preReqs; // add as prerequisites to UECS2101

            StudentRecord studRecord = generateStudentRecordWithSampleTrimesters();

            studRecord.CheckPrerequisites(studyList);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Unit : UECS1202 has prerequisite UECS8888 which has not been fulfilled yet\nUnit : UECS1301 has prerequisite UECS5555 which has not been fulfilled yet\n")]
        public void CheckPrerequisites_StudyListWithMultipleNonExistingPrerequisites_Exception()
        {
            List<StudyUnit> studyList = generateStudyListWithoutPrerequisites();
            List<string> preReqs;

            // start adding in prerequisites that do not exist
            preReqs = new List<string>();
            preReqs.Add("UECS8888"); // does not exist
            preReqs.Add("UECS1103"); // exists
            studyList[4].prerequisites = preReqs; // add as prerequisites to UECS1202

            preReqs = new List<string>();
            preReqs.Add("UECS5555"); // does not exist
            studyList[6].prerequisites = preReqs; // add as prerequisites to UECS1301

            StudentRecord studRecord = generateStudentRecordWithSampleTrimesters();

            studRecord.CheckPrerequisites(studyList);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Unit : UECS1202 has prerequisite UECS1101 which has not been fulfilled yet\nUnit : UECS2101 has prerequisite UECS1301 which has not been fulfilled yet\nUnit : UECS2101 has prerequisite UECS1302 which has not been fulfilled yet\n")]
        public void CheckPrerequisites_StudyListWithMultipleFailedPrerequisites_Exception()
        {
            List<StudyUnit> studyList = generateStudyListWithoutPrerequisites();
            List<string> preReqs;

            // start adding in prerequisites which are below passing mark
            preReqs = new List<string>();
            preReqs.Add("UECS1102"); // above passing mark
            preReqs.Add("UECS1101"); // below passing mark
            studyList[4].prerequisites = preReqs; // add as prerequisites to UECS1202

            preReqs = new List<string>();
            preReqs.Add("UECS1301"); // below passing mark
            preReqs.Add("UECS1302"); // below passing mark
            studyList[9].prerequisites = preReqs; // add as prerequisites to UECS2101

            StudentRecord studRecord = generateStudentRecordWithSampleTrimesters();

            studRecord.CheckPrerequisites(studyList);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Unit : UECS1202 has prerequisite UECS5555 which has not been fulfilled yet\nUnit : UECS1202 has prerequisite UECS1101 which has not been fulfilled yet\nUnit : UECS2101 has prerequisite UECS1301 which has not been fulfilled yet\nUnit : UECS2101 has prerequisite UECS8888 which has not been fulfilled yet\n")]
        public void CheckPrerequisites_StudyListCombinationNonExistentAndFailedPrerequisites_Exception()
        {
            List<StudyUnit> studyList = generateStudyListWithoutPrerequisites();
            List<string> preReqs;

            // start adding in combination of fulfilled prerequisites, those below passing mark and those non-existent
            preReqs = new List<string>();
            preReqs.Add("UECS5555"); // non existent
            preReqs.Add("UECS1101"); // below passing mark
            studyList[4].prerequisites = preReqs; // add as prerequisites to UECS1202

            preReqs = new List<string>();
            preReqs.Add("UECS1301"); // below passing mark
            preReqs.Add("UECS1303"); // normal met
            preReqs.Add("UECS8888"); // non existent
            studyList[9].prerequisites = preReqs; // add as prerequisites to UECS2101

            StudentRecord studRecord = generateStudentRecordWithSampleTrimesters();

            studRecord.CheckPrerequisites(studyList);
        }


        private StudentRecord generateStudentRecordWithSampleTrimesters()
        {
            TrimesterPerformance tpf;
            StudentRecord studRecord = new StudentRecord("ahmad", "12345", Gender.male, "ahmad@gmail.com", "012-44444", "SE", 120);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.JAN, Trimesters.Y1T1);
            tpf.AddUnitGrade(new UnitGrade("UECS1101", "somename", 40));
            tpf.AddUnitGrade(new UnitGrade("UECS1102", "somename", 80));
            tpf.AddUnitGrade(new UnitGrade("UECS1103", "somename", 80));
            studRecord.AddTrimesterPerformance(tpf);

            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 80));
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 80));
            tpf.AddUnitGrade(new UnitGrade("UECS1203", "somename", 80));
            studRecord.AddTrimesterPerformance(tpf);

            tpf = new TrimesterPerformance(2014, TrimesterMonths.OCT, Trimesters.Y1T3);
            tpf.AddUnitGrade(new UnitGrade("UECS1301", "somename", 40));
            tpf.AddUnitGrade(new UnitGrade("UECS1302", "somename", 40));
            tpf.AddUnitGrade(new UnitGrade("UECS1303", "somename", 80));
            studRecord.AddTrimesterPerformance(tpf);

            tpf = new TrimesterPerformance(2015, TrimesterMonths.JAN, Trimesters.Y2T1);
            tpf.AddUnitGrade(new UnitGrade("UECS2101", "somename", 50));
            tpf.AddUnitGrade(new UnitGrade("UECS2102", "somename", 50));
            tpf.AddUnitGrade(new UnitGrade("UECS2103", "somename", 50));
            studRecord.AddTrimesterPerformance(tpf);

            return studRecord;
        }


        public List<StudyUnit> generateStudyListWithoutPrerequisites()
        {

            List<string> origString = new List<string>();
            List<Trimesters> origTrimesters = new List<Trimesters>();
            List<Campuses> origCampuses = new List<Campuses>();
            List<StudyUnit> studyList = new List<StudyUnit>();
                
            studyList.Add(new StudyUnit("UECS1101", "some unit", origString, 4, UnitClassification.elective, origTrimesters, origCampuses, origString, 30));
            studyList.Add(new StudyUnit("UECS1102", "some unit", origString, 3, UnitClassification.elective, origTrimesters, origCampuses, origString, 30));
            studyList.Add(new StudyUnit("UECS1103", "some unit", origString, 3, UnitClassification.elective, origTrimesters, origCampuses, origString, 30));

            studyList.Add(new StudyUnit("UECS1201", "some unit", origString, 3, UnitClassification.elective, origTrimesters, origCampuses, origString, 30));
            studyList.Add(new StudyUnit("UECS1202", "some unit", origString, 4, UnitClassification.elective, origTrimesters, origCampuses, origString, 30));
            studyList.Add(new StudyUnit("UECS1203", "some unit", origString, 4, UnitClassification.elective, origTrimesters, origCampuses, origString, 30));

            studyList.Add(new StudyUnit("UECS1301", "some unit", origString, 4, UnitClassification.elective, origTrimesters, origCampuses, origString, 30));
            studyList.Add(new StudyUnit("UECS1302", "some unit", origString, 3, UnitClassification.elective, origTrimesters, origCampuses, origString, 30));
            studyList.Add(new StudyUnit("UECS1303", "some unit", origString, 3, UnitClassification.elective, origTrimesters, origCampuses, origString, 30));

            studyList.Add(new StudyUnit("UECS2101", "some unit", origString, 3, UnitClassification.elective, origTrimesters, origCampuses, origString, 30));
            studyList.Add(new StudyUnit("UECS2102", "some unit", origString, 4, UnitClassification.elective, origTrimesters, origCampuses, origString, 30));
            studyList.Add(new StudyUnit("UECS2103", "some unit", origString, 3, UnitClassification.elective, origTrimesters, origCampuses, origString, 30));

            return studyList;
        }



        [Test]
        public void CheckForRepeatedUnits_TrimestersWithoutAnyRepeatedUnits()
        {
            StudentRecord studRecord = generateStudentRecordWithSampleTrimesters();
            studRecord.CheckForRepeatedUnits();

            // Make sure there are no repeated units
            foreach (TrimesterPerformance tpf in studRecord.trimesterPerformanceList)
            {
                foreach (UnitGrade unitGrade in tpf.unitGrades)
                    Assert.IsFalse(unitGrade.repeatedUnit);
            }
        }

        [Test]
        public void CheckForRepeatedUnits_TrimestersWithOneRepeatedUnit()
        {
            TrimesterPerformance trimPerform;
            
            StudentRecord studRecord = generateStudentRecordWithSampleTrimesters();
            trimPerform = studRecord.trimesterPerformanceList[3]; // 2015, JAN, Y2T1
            trimPerform.AddUnitGrade(new UnitGrade("UECS1103", "somename", 60)); // originally from 2014, JAN, Y1T1

            studRecord.CheckForRepeatedUnits();

            for (int i = 0; i < studRecord.trimesterPerformanceList.Count; i++ )
            {
                trimPerform = studRecord.trimesterPerformanceList[i];
                for (int j = 0; j < trimPerform.unitGrades.Count; j++)
                {
                    if ((i == 0) && (j == 2)) // first time for UECS1103, must show repeated
                        Assert.IsTrue(trimPerform.unitGrades[j].repeatedUnit);
                    else if ((i == 3) && (j == 3)) // second and last time for UECS1103, must show repeated and useCalculate must be true
                    {
                        Assert.IsTrue(trimPerform.unitGrades[j].repeatedUnit);
                        Assert.IsTrue(trimPerform.unitGrades[j].useForCalculate);
                    }

                    else // for all other units, they should not show they are repeated
                        Assert.IsFalse(trimPerform.unitGrades[j].repeatedUnit);
                }
            }
        }


        [Test]
        public void CheckForRepeatedUnits_TrimestersWithTwoRepeatedUnits()
        {
            TrimesterPerformance trimPerform;

            StudentRecord studRecord = generateStudentRecordWithSampleTrimesters();
            trimPerform = studRecord.trimesterPerformanceList[3]; // 2015, JAN, Y2T1
            trimPerform.AddUnitGrade(new UnitGrade("UECS1103", "somename", 60)); // originally from 2014, JAN, Y1T1
            trimPerform.AddUnitGrade(new UnitGrade("UECS1202", "somename", 60)); // originally from 2014, MAY, Y1T2
            trimPerform = studRecord.trimesterPerformanceList[2]; // 2014, OCT, Y1T3
            trimPerform.AddUnitGrade(new UnitGrade("UECS1202", "somename", 60)); // originally from 2014, MAY, Y1T2

            studRecord.CheckForRepeatedUnits();

            for (int i = 0; i < studRecord.trimesterPerformanceList.Count; i++)
            {
                trimPerform = studRecord.trimesterPerformanceList[i];
                for (int j = 0; j < trimPerform.unitGrades.Count; j++)
                {
                    if ((i == 0) && (j == 2)) // first time for UECS1103, must show repeated
                        Assert.IsTrue(trimPerform.unitGrades[j].repeatedUnit);
                    else if ((i == 3) && (j == 3)) // second and last time for UECS1103, must show repeated and useCalculate must be true
                    {
                        Assert.IsTrue(trimPerform.unitGrades[j].repeatedUnit);
                        Assert.IsTrue(trimPerform.unitGrades[j].useForCalculate);
                    }
                    else if ((i == 1) && (j == 1)) // first time for UECS1202, must show repeated
                        Assert.IsTrue(trimPerform.unitGrades[j].repeatedUnit);
                    else if ((i == 2) && (j == 3)) // second time for UECS1202, must show repeated
                        Assert.IsTrue(trimPerform.unitGrades[j].repeatedUnit);
                    else if ((i == 3) && (j == 4)) // third and last time for UECS1202, must show repeated and useCalculate must be true
                    {
                        Assert.IsTrue(trimPerform.unitGrades[j].repeatedUnit);
                        Assert.IsTrue(trimPerform.unitGrades[j].useForCalculate);
                    }
                    else // for all other units, they should not show they are repeated
                        Assert.IsFalse(trimPerform.unitGrades[j].repeatedUnit);
                }
            }
        }
        

    }
}
