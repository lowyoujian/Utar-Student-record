using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vers7;
using NUnit.Framework;

namespace Vers7.UnitTests
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


        [Test]
        public void CalculateCGPAOverall_TrimestersWithRepeatedUnits()
        {
            TrimesterPerformanceTests performTests = new TrimesterPerformanceTests();

            List<StudyUnit> studyUnits = generateStudyListWithoutPrerequisites();
            StudentRecord studRecord = performTests.generateStudentRecordWithTrimestersForCalculation();

            studRecord.CheckForRepeatedUnits(); // integration testing

            PerformCalculcationForEntireStudentRecord(studRecord, studyUnits);

            // Check Accumulated Credit Hours and CGPA for all trimesters one by one

            // 2014 JAN Y1T1
            Assert.AreEqual(10, studRecord.trimesterPerformanceList[0].accumulatedCreditHour);
            Assert.AreEqual(3.03, studRecord.trimesterPerformanceList[0].cumulativeGradePointAverage);

            // 2014 MAY Y1T2
            Assert.AreEqual(14, studRecord.trimesterPerformanceList[1].accumulatedCreditHour);
            Assert.AreEqual(3.12, studRecord.trimesterPerformanceList[1].cumulativeGradePointAverage);

            // 2014 OCT Y1T3
            Assert.AreEqual(24, studRecord.trimesterPerformanceList[2].accumulatedCreditHour);
            Assert.AreEqual(3.26, studRecord.trimesterPerformanceList[2].cumulativeGradePointAverage);

            // 2015 JAN Y2T1
            Assert.AreEqual(35, studRecord.trimesterPerformanceList[3].accumulatedCreditHour);
            Assert.AreEqual(3.29, studRecord.trimesterPerformanceList[3].cumulativeGradePointAverage);
        }


        public void PerformCalculcationForEntireStudentRecord(StudentRecord studRecord, List<StudyUnit> studyUnits)
        {
            foreach (TrimesterPerformance trimPerform in studRecord.trimesterPerformanceList)
            {
                foreach (UnitGrade ugrade in trimPerform.unitGrades)
                    ugrade.CalculateGrade(); // integration testing
                trimPerform.CalculateGrades(studyUnits); // integration testing
            }

            studRecord.CalculateCGPAOverall(); // integration testing
        }


        [Test] // test sequence: probation -> normal -> leave -> graduated 
        public void CheckAcademicStatus_TestSequence1()
        {
            List<StudyUnit> studyUnits = generateStudyListWithoutPrerequisites();
            StudentRecord studRecord = new StudentRecord("ahmad", "12345", Gender.male, "ahmad@gmail.com", "012-44444", "SE", 25);

            TrimesterPerformance tpf;

            tpf = new TrimesterPerformance(2014, TrimesterMonths.JAN, Trimesters.Y1T1);
            tpf.AddUnitGrade(new UnitGrade("UECS1101", "somename", 45));
            tpf.AddUnitGrade(new UnitGrade("UECS1102", "somename", 55));
            studRecord.AddTrimesterPerformance(tpf);

            PerformCalculcationForEntireStudentRecord(studRecord, studyUnits);
            studRecord.CheckAcademicStatus();

            Assert.AreEqual(AcademicStatus.probation, studRecord.status);

            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 75));
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 88));
            tpf.AddUnitGrade(new UnitGrade("UECS1203", "somename", 65));
            studRecord.AddTrimesterPerformance(tpf);

            PerformCalculcationForEntireStudentRecord(studRecord, studyUnits);
            studRecord.CheckAcademicStatus();

            Assert.AreEqual(AcademicStatus.normal, studRecord.status);

            tpf = new TrimesterPerformance(2014, TrimesterMonths.OCT, Trimesters.Y1T3);
            studRecord.AddTrimesterPerformance(tpf);

            PerformCalculcationForEntireStudentRecord(studRecord, studyUnits);
            studRecord.CheckAcademicStatus();

            Assert.AreEqual(AcademicStatus.leave, studRecord.status);

            tpf = new TrimesterPerformance(2015, TrimesterMonths.JAN, Trimesters.Y2T1);
            tpf.AddUnitGrade(new UnitGrade("UECS2101", "somename", 73));
            tpf.AddUnitGrade(new UnitGrade("UECS2102", "somename", 79));
            studRecord.AddTrimesterPerformance(tpf);

            PerformCalculcationForEntireStudentRecord(studRecord, studyUnits);
            studRecord.CheckAcademicStatus();

            Assert.AreEqual(AcademicStatus.graduated, studRecord.status);
        }

        [Test] // test sequence: leave -> normal -> probation -> terminated
        public void CheckAcademicStatus_TestSequence2()
        {
            List<StudyUnit> studyUnits = generateStudyListWithoutPrerequisites();
            StudentRecord studRecord = new StudentRecord("ahmad", "12345", Gender.male, "ahmad@gmail.com", "012-44444", "SE", 25);

            TrimesterPerformance tpf;

            tpf = new TrimesterPerformance(2014, TrimesterMonths.JAN, Trimesters.Y1T1);
            studRecord.AddTrimesterPerformance(tpf);

            PerformCalculcationForEntireStudentRecord(studRecord, studyUnits);
            studRecord.CheckAcademicStatus();

            Assert.AreEqual(AcademicStatus.leave, studRecord.status);

            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 55));
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 53));
            tpf.AddUnitGrade(new UnitGrade("UECS1203", "somename", 58));
            studRecord.AddTrimesterPerformance(tpf);

            PerformCalculcationForEntireStudentRecord(studRecord, studyUnits);
            studRecord.CheckAcademicStatus();

            Assert.AreEqual(AcademicStatus.normal, studRecord.status);

            tpf = new TrimesterPerformance(2014, TrimesterMonths.OCT, Trimesters.Y1T3);
            tpf.AddUnitGrade(new UnitGrade("UECS1301", "somename", 35));
            tpf.AddUnitGrade(new UnitGrade("UECS1302", "somename", 40));
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 20));
            studRecord.AddTrimesterPerformance(tpf);

            PerformCalculcationForEntireStudentRecord(studRecord, studyUnits);
            studRecord.CheckAcademicStatus();

            Assert.AreEqual(AcademicStatus.probation, studRecord.status);

            tpf = new TrimesterPerformance(2015, TrimesterMonths.JAN, Trimesters.Y2T1);
            tpf.AddUnitGrade(new UnitGrade("UECS2101", "somename", 40));
            tpf.AddUnitGrade(new UnitGrade("UECS2102", "somename", 30));
            studRecord.AddTrimesterPerformance(tpf);

            PerformCalculcationForEntireStudentRecord(studRecord, studyUnits);
            studRecord.CheckAcademicStatus();

            Assert.AreEqual(AcademicStatus.terminated, studRecord.status);
        }




    }


    [TestFixture]
    public class TrimesterPerformanceTests
    {
        StudentRecordTests studentRecordTests = new StudentRecordTests();


        [Test]
        public void CalculateGrades_TrimesterWithoutRepeatedUnits()
        {


            List<StudyUnit> studyUnits = studentRecordTests.generateStudyListWithoutPrerequisites();
            StudentRecord studRecord = generateStudentRecordWithTrimestersForCalculation();
            TrimesterPerformance trimPerform = studRecord.trimesterPerformanceList[0]; // 2014, JAN, Y1T1

            studRecord.CheckForRepeatedUnits(); // integration testing

            foreach (UnitGrade ugrade in trimPerform.unitGrades)
                ugrade.CalculateGrade(); // integration testing: 

            trimPerform.CalculateGrades(studyUnits);

            Assert.AreEqual(10, trimPerform.totalCreditHour);
            Assert.AreEqual(30.32, trimPerform.totalGradePoints);
            Assert.AreEqual(3.03, trimPerform.gradePointAverage);

        }


        [Test]
        public void CalculateGrades_TrimesterWithRepeatedUnits()
        {
            List<StudyUnit> studyUnits = studentRecordTests.generateStudyListWithoutPrerequisites();
            StudentRecord studRecord = generateStudentRecordWithTrimestersForCalculation();
            TrimesterPerformance trimPerform;

            studRecord.CheckForRepeatedUnits(); // integration testing

            trimPerform = studRecord.trimesterPerformanceList[1]; // 2014, MAY, Y1T2
            foreach (UnitGrade ugrade in trimPerform.unitGrades)
                ugrade.CalculateGrade(); // integration testing: 

            trimPerform.CalculateGrades(studyUnits);

            Assert.AreEqual(4, trimPerform.totalCreditHour);
            Assert.AreEqual(13.32, trimPerform.totalGradePoints);
            Assert.AreEqual(3.33, trimPerform.gradePointAverage);

            trimPerform = studRecord.trimesterPerformanceList[2]; // 2014, OCT, Y1T3
            foreach (UnitGrade ugrade in trimPerform.unitGrades)
                ugrade.CalculateGrade(); // integration testing: 

            trimPerform.CalculateGrades(studyUnits);

            Assert.AreEqual(10, trimPerform.totalCreditHour);
            Assert.AreEqual(34.69, trimPerform.totalGradePoints);
            Assert.AreEqual(3.47, trimPerform.gradePointAverage);

            trimPerform = studRecord.trimesterPerformanceList[3]; // 2015, JAN, Y2T1
            foreach (UnitGrade ugrade in trimPerform.unitGrades)
                ugrade.CalculateGrade(); // integration testing: 

            trimPerform.CalculateGrades(studyUnits);

            Assert.AreEqual(11, trimPerform.totalCreditHour);
            Assert.AreEqual(36.67, trimPerform.totalGradePoints);
            Assert.AreEqual(3.33, trimPerform.gradePointAverage);

        }


        public StudentRecord generateStudentRecordWithTrimestersForCalculation()
        {
            TrimesterPerformance tpf;
            StudentRecord studRecord = new StudentRecord("ahmad", "12345", Gender.male, "ahmad@gmail.com", "012-44444", "SE", 120);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.JAN, Trimesters.Y1T1);
            tpf.AddUnitGrade(new UnitGrade("UECS1101", "somename", 55));
            tpf.AddUnitGrade(new UnitGrade("UECS1102", "somename", 68));
            tpf.AddUnitGrade(new UnitGrade("UECS1103", "somename", 82));
            studRecord.AddTrimesterPerformance(tpf);

            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 51)); // passed repeated unit x1
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 40)); // failed, repeated unit x1
            tpf.AddUnitGrade(new UnitGrade("UECS1203", "somename", 73));
            studRecord.AddTrimesterPerformance(tpf);

            tpf = new TrimesterPerformance(2014, TrimesterMonths.OCT, Trimesters.Y1T3);
            tpf.AddUnitGrade(new UnitGrade("UECS1301", "somename", 78));
            tpf.AddUnitGrade(new UnitGrade("UECS1302", "somename", 92));
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 63)); // passed repeated unit x2
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 52)); // passed, repeated unit x2
            studRecord.AddTrimesterPerformance(tpf);

            tpf = new TrimesterPerformance(2015, TrimesterMonths.JAN, Trimesters.Y2T1);
            tpf.AddUnitGrade(new UnitGrade("UECS2101", "somename", 73));
            tpf.AddUnitGrade(new UnitGrade("UECS2102", "somename", 79));
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 65)); // passed, repeated unit x3
            studRecord.AddTrimesterPerformance(tpf);

            return studRecord;
        }

    }


    [TestFixture]
    public class GradingSchemeResultsTests
    {

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Invalid student name or ID to be added")]
        public void AddStudentMark_InvalidStringValues_Exception()
        {
            GradingSchemeResults gradeResults = new GradingSchemeResults();
            gradeResults.AddStudentMark(null, "", 20);

        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Invalid mark to be added. Marks must be between 0 and 100")]
        public void AddStudentMark_InvalidMarkValue_Exception()
        {
            GradingSchemeResults gradeResults = new GradingSchemeResults();
            gradeResults.AddStudentMark("Ali", "1234", 120);

        }



        [Test]
        public void CalculateOverallResults_NoMarksAdded()
        {
            GradingSchemeResults gradeResults = new GradingSchemeResults();
            gradeResults.CalculateOverallResults();

            Assert.AreEqual(0, gradeResults.average);
        }


        [Test]
        public void CalculateOverallResults_CombinationOfDifferentMarks()
        {
            GradingSchemeResults gradeResults = new GradingSchemeResults();
            gradeResults.AddStudentMark("Muthu", "Muthu", 85);
            gradeResults.AddStudentMark("Ali", "Ali", 32);
            gradeResults.AddStudentMark("Chong", "Chong", 55);
            gradeResults.AddStudentMark("Sammy", "Sammy", 67);
            gradeResults.AddStudentMark("John", "John", 77);
            gradeResults.AddStudentMark("Ahmad", "Ahmad", 89);
            gradeResults.AddStudentMark("Wendy", "Wendy", 58);
            gradeResults.AddStudentMark("Peter", "Peter", 57);

            gradeResults.CalculateOverallResults();

            CheckGradeSchemeResults(gradeResults);
        }


        // The set of assertions here are used by two test methods 
        // CalculateOverallResults_CombinationOfDifferentMarks
        // GetUnitStatsForTrimester_CombinationOfDifferentMarks
        //  because they both work on the same set of test data

        public void CheckGradeSchemeResults(GradingSchemeResults gradeResults)
        {
            // Check results are correctly stored away

            Assert.AreEqual("Ali", gradeResults.studentNames[0][0]); // ali is in range 0-49
            Assert.AreEqual(0, gradeResults.studentNames[1].Count); // noone is in range 50-54
            Assert.AreEqual("Chong", gradeResults.studentNames[2][0]); // chong is in range 55-59
            Assert.AreEqual("Wendy", gradeResults.studentNames[2][1]); // wendy is in range 55-59
            Assert.AreEqual("Peter", gradeResults.studentNames[2][2]); // peter is in range 55-59
            Assert.AreEqual(0, gradeResults.studentNames[3].Count); // noone is in range 60-64
            Assert.AreEqual("Sammy", gradeResults.studentNames[4][0]); // sammy is in range 65-69
            Assert.AreEqual(0, gradeResults.studentNames[5].Count); // noone is in range 70-74
            Assert.AreEqual("John", gradeResults.studentNames[6][0]); // john is in range 75-79
            Assert.AreEqual("Muthu", gradeResults.studentNames[7][0]); // muthu is in range 80-89
            Assert.AreEqual("Ahmad", gradeResults.studentNames[7][1]); // ahmad is in range 80-89
            Assert.AreEqual(0, gradeResults.studentNames[8].Count); // noone is in range 90-100

            // Check percentage for each grade range is calculated correctly

            Assert.AreEqual(12.5, gradeResults.percentage[0]);
            Assert.AreEqual(0, gradeResults.percentage[1]);
            Assert.AreEqual(37.5, gradeResults.percentage[2]);
            Assert.AreEqual(0, gradeResults.percentage[3]);
            Assert.AreEqual(12.5, gradeResults.percentage[4]);
            Assert.AreEqual(0, gradeResults.percentage[5]);
            Assert.AreEqual(12.5, gradeResults.percentage[6]);
            Assert.AreEqual(25, gradeResults.percentage[7]);
            Assert.AreEqual(0, gradeResults.percentage[8]);

            // Check average is calculated correctly

            Assert.AreEqual(65.0, gradeResults.average);
        }

    }

    [TestFixture]
    public class DataAnalyzerTests
    {

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "GetUnitStats cannot be run will null arguments")]
        public void GetUnitStatsForTrimester_NullArgument_Exception()
        {
            DataAnalyzer analyzer = new DataAnalyzer();
            analyzer.GetUnitStatsForTrimester(2014, TrimesterMonths.MAY, Trimesters.Y1T2, "UECS1202", null);

        }


        [Test]
        public void GetUnitStatsForTrimester_CombinationOfDifferentMarks()
        {
            TrimesterPerformance tpf;
            List<StudentRecord> recordList = new List<StudentRecord>();
            StudentRecord studRecord;
            GradingSchemeResults gradeResults;
            GradingSchemeResultsTests gradeSchemeTests = new GradingSchemeResultsTests();


            // The test data below is a replication of the test data from CalculateOverallResults_CombinationOfDifferentMarks
            // but is wrapped within the structure of a StudentRecord
            // For some student records, we have two trimesters, others we have one
            // Does not matter as the method should locate the specific trimester containing the unit code of interest

            studRecord = new StudentRecord("Muthu", "Muthu", Gender.male, "dummy@gmail.com", "012-44444", "SE", 120);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.JAN, Trimesters.Y1T1);
            tpf.AddUnitGrade(new UnitGrade("UECS1101", "somename", 55));
            studRecord.AddTrimesterPerformance(tpf);

            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 51));
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 85)); // mark for the unit that we want to survey 
            studRecord.AddTrimesterPerformance(tpf);
            recordList.Add(studRecord);

            studRecord = new StudentRecord("Ali", "Ali", Gender.male, "dummy@gmail.com", "012-44444", "SE", 120);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.JAN, Trimesters.Y1T1);
            tpf.AddUnitGrade(new UnitGrade("UECS1101", "somename", 55));
            studRecord.AddTrimesterPerformance(tpf);

            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 51));
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 32)); // mark for the unit that we want to survey 
            studRecord.AddTrimesterPerformance(tpf);
            recordList.Add(studRecord);

            studRecord = new StudentRecord("Chong", "Chong", Gender.male, "dummy@gmail.com", "012-44444", "SE", 120);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 51));
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 55)); // mark for the unit that we want to survey 
            studRecord.AddTrimesterPerformance(tpf);
            recordList.Add(studRecord);

            studRecord = new StudentRecord("Sammy", "Sammy", Gender.male, "dummy@gmail.com", "012-44444", "SE", 120);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.JAN, Trimesters.Y1T1);
            tpf.AddUnitGrade(new UnitGrade("UECS1101", "somename", 55));
            studRecord.AddTrimesterPerformance(tpf);

            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 51));
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 67)); // mark for the unit that we want to survey 
            studRecord.AddTrimesterPerformance(tpf);
            recordList.Add(studRecord);

            studRecord = new StudentRecord("John", "John", Gender.male, "dummy@gmail.com", "012-44444", "SE", 120);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 51));
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 77)); // mark for the unit that we want to survey 
            studRecord.AddTrimesterPerformance(tpf);
            recordList.Add(studRecord);

            studRecord = new StudentRecord("Ahmad", "Ahmad", Gender.male, "dummy@gmail.com", "012-44444", "SE", 120);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 51));
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 89)); // mark for the unit that we want to survey 
            studRecord.AddTrimesterPerformance(tpf);
            recordList.Add(studRecord);

            studRecord = new StudentRecord("Wendy", "Wendy", Gender.male, "dummy@gmail.com", "012-44444", "SE", 120);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 51));
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 58)); // mark for the unit that we want to survey 
            studRecord.AddTrimesterPerformance(tpf);
            recordList.Add(studRecord);

            studRecord = new StudentRecord("Peter", "Peter", Gender.male, "dummy@gmail.com", "012-44444", "SE", 120);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 51));
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 57)); // mark for the unit that we want to survey 
            studRecord.AddTrimesterPerformance(tpf);
            recordList.Add(studRecord);

            DataAnalyzer analyzer = new DataAnalyzer();
            gradeResults = analyzer.GetUnitStatsForTrimester(2014, TrimesterMonths.MAY, Trimesters.Y1T2, "UECS1202", recordList);
            gradeResults.CalculateOverallResults();

            gradeSchemeTests.CheckGradeSchemeResults(gradeResults);

        }


    }


    public class DummyRandomNumberGenerator : IRandomFunctionality
    {
        private int count = 0;

        // the array below represents a predefined set of integers to return for each call of ReturnRandomNumber
        // these values will in turn result in  certain values being allocated to the generated student record
        // based on the implementation of GenerateStudentRecords, we can check for this values in our test

        int[] valuesToReturns = { 0, 2, 3, 1000, 3, 55, 3, 65, 3, 75, 3, 60, 3, 70, 3, 80, 1, 0, 2, 2000, 3, 45, 3, 55, 3, 65, 3, 70, 3, 80, 3, 90 };

        public DummyRandomNumberGenerator()
        { Reset(); }

        public int ReturnRandomNumber(int upperLimit, int lowerLimit)
        {
            return valuesToReturns[count++];
        }

        public void Reset()
        { count = 0; }
    }

    [TestFixture]
    public class TestDataGeneratorTests
    {


        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "GenerateStudentRecords cannot be run with null or invalid arguments")]
        public void GenerateStudentRecords_NullArguments_Exception()
        {
            TestDataGenerator dataGenerator = new TestDataGenerator(new DummyRandomNumberGenerator());
            dataGenerator.GenerateStudentRecords(3, CreateStudentRecordTemplate(), null, null, null);
        }


        [Test]
        public void GenerateStudentRecords_ZeroAndTwoSampleRecords()
        {
            TestDataGenerator dataGenerator = new TestDataGenerator(new DummyRandomNumberGenerator());
            List<StudentRecord> studRecords;
            List<UnitGrade> unitGrades;
            TrimesterPerformance trimPerform;

            string[] sampleMaleNames = { "Peter", "Philip", "Roger", "Jack", "Owen" };
            string[] sampleFemaleNames = { "Wendy", "Jennifer", "Alice", "Jane", "Jasmine" };
            string[] sampleSurNames = { "Tan", "Leong", "Lim", "Lee", "Ng", "Wong" };


            // First, generate 0 records and check that no record are returned
            studRecords = dataGenerator.GenerateStudentRecords(0, CreateStudentRecordTemplate(), sampleMaleNames.ToList<string>(), sampleFemaleNames.ToList<string>(), sampleSurNames.ToList<string>());

            Assert.AreEqual(0, studRecords.Count);

            // Then, generate 2 records and check that all values in record are correctly initialized
            studRecords = dataGenerator.GenerateStudentRecords(2, CreateStudentRecordTemplate(), sampleMaleNames.ToList<string>(), sampleFemaleNames.ToList<string>(), sampleSurNames.ToList<string>());

            Assert.AreEqual(2, studRecords.Count);

            // Verifying values for 1st student record

            Assert.AreEqual("Roger Lee", studRecords[0].name);
            Assert.AreEqual(Gender.male, studRecords[0].gender);
            Assert.AreEqual("SE10000", studRecords[0].IDNumber);
            Assert.AreEqual("Roger Lee" + DefaultValues.dummyEmailAdd, studRecords[0].emailAddress);
            Assert.AreEqual(DefaultValues.dummyContactNumber, studRecords[0].contactNumber);
            Assert.AreEqual("SE", studRecords[0].programme);
            Assert.AreEqual(120, studRecords[0].creditHrsToGraduate);

            trimPerform = studRecords[0].trimesterPerformanceList[0];

            Assert.AreEqual(2014, trimPerform.year);
            Assert.AreEqual(Trimesters.Y1T1, trimPerform.trimester);
            Assert.AreEqual(TrimesterMonths.JAN, trimPerform.month);

            unitGrades = trimPerform.unitGrades;
            Assert.AreEqual("UECS1101", unitGrades[0].unitCode);
            Assert.AreEqual(55, unitGrades[0].mark);
            Assert.AreEqual("UECS1102", unitGrades[1].unitCode);
            Assert.AreEqual(65, unitGrades[1].mark);
            Assert.AreEqual("UECS1103", unitGrades[2].unitCode);
            Assert.AreEqual(75, unitGrades[2].mark);

            trimPerform = studRecords[0].trimesterPerformanceList[1];
            Assert.AreEqual(2014, trimPerform.year);
            Assert.AreEqual(Trimesters.Y1T2, trimPerform.trimester);
            Assert.AreEqual(TrimesterMonths.MAY, trimPerform.month);

            unitGrades = trimPerform.unitGrades;
            Assert.AreEqual("UECS1201", unitGrades[0].unitCode);
            Assert.AreEqual(60, unitGrades[0].mark);
            Assert.AreEqual("UECS1202", unitGrades[1].unitCode);
            Assert.AreEqual(70, unitGrades[1].mark);
            Assert.AreEqual("UECS1203", unitGrades[2].unitCode);
            Assert.AreEqual(80, unitGrades[2].mark);

            // Verifying values for 2nd student record

            Assert.AreEqual("Wendy Lim", studRecords[1].name);
            Assert.AreEqual(Gender.female, studRecords[1].gender);
            Assert.AreEqual("SE20001", studRecords[1].IDNumber);
            Assert.AreEqual("Wendy Lim" + DefaultValues.dummyEmailAdd, studRecords[1].emailAddress);
            Assert.AreEqual(DefaultValues.dummyContactNumber, studRecords[1].contactNumber);
            Assert.AreEqual("SE", studRecords[1].programme);
            Assert.AreEqual(120, studRecords[1].creditHrsToGraduate);

            trimPerform = studRecords[1].trimesterPerformanceList[0];

            Assert.AreEqual(2014, trimPerform.year);
            Assert.AreEqual(Trimesters.Y1T1, trimPerform.trimester);
            Assert.AreEqual(TrimesterMonths.JAN, trimPerform.month);

            unitGrades = trimPerform.unitGrades;
            Assert.AreEqual("UECS1101", unitGrades[0].unitCode);
            Assert.AreEqual(45, unitGrades[0].mark);
            Assert.AreEqual("UECS1102", unitGrades[1].unitCode);
            Assert.AreEqual(55, unitGrades[1].mark);
            Assert.AreEqual("UECS1103", unitGrades[2].unitCode);
            Assert.AreEqual(65, unitGrades[2].mark);

            trimPerform = studRecords[1].trimesterPerformanceList[1];
            Assert.AreEqual(2014, trimPerform.year);
            Assert.AreEqual(Trimesters.Y1T2, trimPerform.trimester);
            Assert.AreEqual(TrimesterMonths.MAY, trimPerform.month);

            unitGrades = trimPerform.unitGrades;
            Assert.AreEqual("UECS1201", unitGrades[0].unitCode);
            Assert.AreEqual(70, unitGrades[0].mark);
            Assert.AreEqual("UECS1202", unitGrades[1].unitCode);
            Assert.AreEqual(80, unitGrades[1].mark);
            Assert.AreEqual("UECS1203", unitGrades[2].unitCode);
            Assert.AreEqual(90, unitGrades[2].mark);
        }


        public StudentRecord CreateStudentRecordTemplate()
        {
            TrimesterPerformance tpf;
            StudentRecord studRecord = new StudentRecord("dummy", "12345", Gender.male, "ahmad@gmail.com", "012-44444", "SE", 120);
            tpf = new TrimesterPerformance(2014, TrimesterMonths.JAN, Trimesters.Y1T1);
            tpf.AddUnitGrade(new UnitGrade("UECS1101", "somename", 0));
            tpf.AddUnitGrade(new UnitGrade("UECS1102", "somename", 0));
            tpf.AddUnitGrade(new UnitGrade("UECS1103", "somename", 0));
            studRecord.AddTrimesterPerformance(tpf);

            tpf = new TrimesterPerformance(2014, TrimesterMonths.MAY, Trimesters.Y1T2);
            tpf.AddUnitGrade(new UnitGrade("UECS1201", "somename", 0));
            tpf.AddUnitGrade(new UnitGrade("UECS1202", "somename", 0));
            tpf.AddUnitGrade(new UnitGrade("UECS1203", "somename", 0));
            studRecord.AddTrimesterPerformance(tpf);

            return studRecord;
        }

    }


    [TestFixture]
    public class FileOperationsTests
    {
        FileOperations fileOperation = new FileOperations();

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Cannot read from null file name")]
        public void ReadStringsFromFile_NullFileName_Exception()
        {
            fileOperation.ReadStringsFromFile(null);
        }
        

        [Test]
        public void ReadStringsFromFile_DummyFile()
        {
            //  To test this method, we need to have a dummy file with known values in the same directory
            // as the dll for this class library that is run in NUnit

            List<string> fileStrings = fileOperation.ReadStringsFromFile("testreadstrings1.txt");
            Assert.AreEqual("dog", fileStrings[0]);
            Assert.AreEqual("cat", fileStrings[1]);
            Assert.AreEqual("mouse", fileStrings[2]);
            Assert.AreEqual("", fileStrings[3]);
            Assert.AreEqual("bus", fileStrings[4]);
            Assert.AreEqual("car", fileStrings[5]);
            Assert.AreEqual("airplane", fileStrings[6]);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Cannot write to a null file name or write a null list")]
        public void WriteStringsToFile_NullList_Exception()
        {
            fileOperation.WriteStringsToFile("testreadstrings2.txt", null);
        }


        [Test]
        public void WriteStringsToFile_DummyFile()
        {

            //  To test this method, we write a sample of strings to a dummy file in the same 
            // directory as the dll for this class library, and then read it out again using
            // ReadStringsFromFile

            string[] arrayString = { "monkey", "donkey", "", "rooster", "cow" };

            fileOperation.WriteStringsToFile("testreadstrings2.txt", arrayString.ToList<string>());
            List<string> fileStrings = fileOperation.ReadStringsFromFile("testreadstrings2.txt");
            Assert.AreEqual("monkey", fileStrings[0]);
            Assert.AreEqual("donkey", fileStrings[1]);
            Assert.AreEqual("", fileStrings[2]);
            Assert.AreEqual("rooster", fileStrings[3]);
            Assert.AreEqual("cow", fileStrings[4]);
        }


        [Test]
        public void CreateSubjectList_3NormalEntries_NoException()
        {

            //  To test this method, use an existing file in the same 
            // directory as the dll for this class library, and then read it out again using
            // ReadStringsFromFile

            List<StudyUnit> studyUnits = fileOperation.CreateSubjectList(fileOperation.ReadStringsFromFile("testcreatesubjectlist1.txt"));

            // check the first subject

            Assert.AreEqual("UECS1111",studyUnits[0].unitCode);
            Assert.AreEqual("Subject1", studyUnits[0].unitName);
            Assert.AreEqual(0, studyUnits[0].prerequisites.Count);
            Assert.AreEqual(4, studyUnits[0].creditHours);
            Assert.AreEqual(UnitClassification.compulsory, studyUnits[0].classification);
            Assert.AreEqual(Trimesters.Y1T1, studyUnits[0].trimesterOffered[0]);
            Assert.AreEqual(Trimesters.Y2T1, studyUnits[0].trimesterOffered[1]);
            Assert.AreEqual(Campuses.FES, studyUnits[0].campusesOffered[0]);
            Assert.AreEqual(Campuses.FCI, studyUnits[0].campusesOffered[1]);
            Assert.AreEqual("SE", studyUnits[0].programmeOffered[0]);
            Assert.AreEqual(40, studyUnits[0].CWComponent);


            // check the second subject

            Assert.AreEqual("UECS2222", studyUnits[1].unitCode);
            Assert.AreEqual("Subject2", studyUnits[1].unitName);
            Assert.AreEqual("UECS4444", studyUnits[1].prerequisites[0]);
            Assert.AreEqual("UECS5555", studyUnits[1].prerequisites[1]);
            Assert.AreEqual(4, studyUnits[1].creditHours);
            Assert.AreEqual(UnitClassification.elective, studyUnits[1].classification);
            Assert.AreEqual(Trimesters.Y3T3, studyUnits[1].trimesterOffered[0]);
            Assert.AreEqual(Trimesters.Y2T2, studyUnits[1].trimesterOffered[1]);
            Assert.AreEqual(Campuses.FES, studyUnits[1].campusesOffered[0]);
            Assert.AreEqual("SE", studyUnits[1].programmeOffered[0]);
            Assert.AreEqual(30, studyUnits[1].CWComponent);

            // check the third subject

            Assert.AreEqual("UECS3333", studyUnits[2].unitCode);
            Assert.AreEqual("Subject3", studyUnits[2].unitName);
            Assert.AreEqual("UECS2222", studyUnits[2].prerequisites[0]);
            Assert.AreEqual(3, studyUnits[2].creditHours);
            Assert.AreEqual(UnitClassification.major, studyUnits[2].classification);
            Assert.AreEqual(Trimesters.Y1T1, studyUnits[2].trimesterOffered[0]);
            Assert.AreEqual(Campuses.FICT, studyUnits[2].campusesOffered[0]);
            Assert.AreEqual("SE", studyUnits[2].programmeOffered[0]);
            Assert.AreEqual(50, studyUnits[2].CWComponent);

        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "CreateSubjectList cannot be run on a null string list")]
        public void CreateSubjectList_NullStringList_Exception()
        {
            fileOperation.CreateSubjectList(null);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "UECS1111 : Correct format for subject list is unitcode : unitName : prerequisites : credithours : unit classification : trimester offered : faculty :  programme : coursework component")] 
        public void CreateSubjectList_InvalidNumberArguments_Exception()
        {
            fileOperation.CreateSubjectList(fileOperation.ReadStringsFromFile("testcreatesubjectlist2.txt"));
        }


        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Subject1 : The unit code is the 1st element and cannot be empty")]
        public void CreateSubjectList_UnitCodeMissing_Exception()
        {
            fileOperation.CreateSubjectList(fileOperation.ReadStringsFromFile("testcreatesubjectlist3.txt"));
        }


        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "UECS1111 : Total credit hours for unit is numeric and is the 4th element\n")]
        public void CreateSubjectList_IncorrectCreditHours_Exception()
        {
            fileOperation.CreateSubjectList(fileOperation.ReadStringsFromFile("testcreatesubjectlist4.txt"));
        }


        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "UECS1111 : Unit classification must be valid and is the 5th element\n")]
        public void CreateSubjectList_InvalidUnitClassification_Exception()
        {
            fileOperation.CreateSubjectList(fileOperation.ReadStringsFromFile("testcreatesubjectlist5.txt"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "UECS1111 : Trimesters offered must be valid and is the 6th element\n")]
        public void CreateSubjectList_InvalidTrimesters_Exception()
        {
            fileOperation.CreateSubjectList(fileOperation.ReadStringsFromFile("testcreatesubjectlist6.txt"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "UECS1111 : Campuses offered must be valid and is the 7th element\n")]
        public void CreateSubjectList_InvalidCampuses_Exception()
        {
            fileOperation.CreateSubjectList(fileOperation.ReadStringsFromFile("testcreatesubjectlist7.txt"));
        }


        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "UECS1111 : Coursework component for unit is numeric and is the 9th element\n")]
        public void CreateSubjectList_InvalidCourseworkComponent_Exception()
        {
            fileOperation.CreateSubjectList(fileOperation.ReadStringsFromFile("testcreatesubjectlist8.txt"));
        }


        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "UECS1111 : Total credit hours for unit is numeric and is the 4th element\nUnit classification must be valid and is the 5th element\nTrimesters offered must be valid and is the 6th element\n")]
        public void CreateSubjectList_MultipleErrors1_Exception()
        {
            fileOperation.CreateSubjectList(fileOperation.ReadStringsFromFile("testcreatesubjectlist9.txt"));
        }


        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "UECS1111 : Campuses offered must be valid and is the 7th element\nCoursework component for unit is numeric and is the 9th element\n")]
        public void CreateSubjectList_MultipleErrors2_Exception()
        {
            fileOperation.CreateSubjectList(fileOperation.ReadStringsFromFile("testcreatesubjectlist10.txt"));
        }


        [Test]
        public void CreateTrimesterPerformance_NormalTrimester_NoException()
        {
            string[] unitCodesInFile = { "UECS1111", "UECS2222", "UECS3333", "UECS4444", "UECS5555" };
            string[] subjectsInFile = { "Subject1", "Subject2", "Subject3", "Subject4", "Subject5" };
            int[] marksInFile = { 36, 55, 76, 82, 90 };

            TrimesterPerformance trimPerform = fileOperation.CreateTrimesterPerformance(fileOperation.ReadStringsFromFile("testcreatetrimesterperformance1.txt"));

            Assert.AreEqual(2012, trimPerform.year);
            Assert.AreEqual(TrimesterMonths.MAY, trimPerform.month);
            Assert.AreEqual(Trimesters.Y1T1, trimPerform.trimester);

            for (int i = 0; i < unitCodesInFile.Length; i++)
            {
                Assert.AreEqual(unitCodesInFile[i], trimPerform.unitGrades[i].unitCode);
                Assert.AreEqual(subjectsInFile[i], trimPerform.unitGrades[i].unitName);
                Assert.AreEqual(marksInFile[i], trimPerform.unitGrades[i].mark);
            }


        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "CreateTrimesterPerformance cannot be run on a null string list")]
        public void CreateTrimesterPerformance_NullStringList_Exception()
        {
            fileOperation.CreateTrimesterPerformance(null);

        }


        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "2012 May Y1T1 dog : The first line of the trimester record must specify year, month, trimester")]
        public void CreateTrimesterPerformance_FirstLineIncorrect_Exception()
        {
            fileOperation.CreateTrimesterPerformance(fileOperation.ReadStringsFromFile("testcreatetrimesterperformance2.txt"));

        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "cat dog mouse : The year is the 1st element and is numeric\nThe month is the 2nd element and must be a valid value (JAN, MAY, OCT)\nThe trimester is the 3rd element and must be a valid value (Y1T1, Y2T2,..)\n")]
        public void CreateTrimesterPerformance_ErrorInYearMonthAndTrimester_Exception()
        {
            fileOperation.CreateTrimesterPerformance(fileOperation.ReadStringsFromFile("testcreatetrimesterperformance3.txt"));

        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "2012 MAY Y1T1 : Each line of trimester record consists of unit code, unit name and mark separated by a ':'")]
        public void CreateTrimesterPerformance_InvalidUnitGrade_Exception()
        {
            fileOperation.CreateTrimesterPerformance(fileOperation.ReadStringsFromFile("testcreatetrimesterperformance4.txt"));

        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "UECS1111 : The mark is the 3rd element and is numeric")]
        public void CreateTrimesterPerformance_InvalidMark_Exception()
        {
            fileOperation.CreateTrimesterPerformance(fileOperation.ReadStringsFromFile("testcreatetrimesterperformance5.txt"));

        }

        [Test]
        public void CreateStudentRecord_NormalRecordWith2Trimesters_NoException()
        {

            string[] unitCodesInFile = { "UECS1111", "UECS2222", "UECS3333", "UECS4444", "UECS5555", "UECS6666" };
            string[] subjectsInFile = { "Subject1", "Subject2", "Subject3", "Subject4", "Subject5", "Subject6" };
            int[] marksInFile = { 36, 55, 76, 82, 90, 57 };
            TrimesterPerformance trimPerform;

            StudentRecord studRecord = fileOperation.CreateStudentRecord(fileOperation.ReadStringsFromFile("testcreatestudentrecord1.txt"));

            Assert.AreEqual("Christopher Wong", studRecord.name);
            Assert.AreEqual("SE12345", studRecord.IDNumber);
            Assert.AreEqual(Gender.male, studRecord.gender);
            Assert.AreEqual("chris@gmail.com", studRecord.emailAddress);
            Assert.AreEqual("01437283423", studRecord.contactNumber);
            Assert.AreEqual("SE", studRecord.programme);
            Assert.AreEqual(120, studRecord.creditHrsToGraduate);

            // Check values for first trimester
            trimPerform = studRecord.trimesterPerformanceList[0];

            Assert.AreEqual(2014, trimPerform.year);
            Assert.AreEqual(TrimesterMonths.JAN, trimPerform.month);
            Assert.AreEqual(Trimesters.Y1T1, trimPerform.trimester);

            for (int i = 0; i < trimPerform.unitGrades.Count; i++)
            {
                Assert.AreEqual(unitCodesInFile[i], trimPerform.unitGrades[i].unitCode);
                Assert.AreEqual(subjectsInFile[i], trimPerform.unitGrades[i].unitName);
                Assert.AreEqual(marksInFile[i], trimPerform.unitGrades[i].mark);
            }

            // Check values for second trimester
            trimPerform = studRecord.trimesterPerformanceList[1];

            Assert.AreEqual(2014, trimPerform.year);
            Assert.AreEqual(TrimesterMonths.MAY, trimPerform.month);
            Assert.AreEqual(Trimesters.Y1T2, trimPerform.trimester);

            for (int i = 0; i < trimPerform.unitGrades.Count; i++)
            {
                Assert.AreEqual(unitCodesInFile[i+3], trimPerform.unitGrades[i].unitCode);
                Assert.AreEqual(subjectsInFile[i+3], trimPerform.unitGrades[i].unitName);
                Assert.AreEqual(marksInFile[i+3], trimPerform.unitGrades[i].mark);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "CreateStudentRecord cannot be run on a null string list")]
        public void CreateStudentRecord_NullStringList_Exception()
        {
            fileOperation.CreateStudentRecord(null);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Insufficient lines to create a student record")]
        public void CreateStudentRecord_InsuffientInfoToCreateRecord_Exception()
        {
            fileOperation.CreateStudentRecord(fileOperation.ReadStringsFromFile("testcreatestudentrecord2.txt"));
        }


        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "The gender is either male or female and is specified in the 3rd line\nTotal credit hours for programme is numeric and is specified in the 7th line")]
        public void CreateStudentRecord_ErrorWithGenderAndCreditHour_Exception()
        {
            fileOperation.CreateStudentRecord(fileOperation.ReadStringsFromFile("testcreatestudentrecord3.txt"));
        }

        [Test]
        public void ChangeTrimesterPerformanceToStrings_PassNullValue()
        {
            List<string> temp = fileOperation.ChangeTrimesterPerformanceToStrings(null);
            Assert.AreEqual(0, temp.Count);

        }

        [Test] // use the previous trimesterperformance file to check for functionality
        public void ChangeTrimesterPerformanceToStrings_SampleFromFile()
        {
            string[] expectedValues = {
            "2012 MAY Y1T1",
            "UECS1111 : Subject1 : 36",
            "UECS2222 : Subject2 : 55",
            "UECS3333 : Subject3 : 76",
            "UECS4444 : Subject4 : 82",
            "UECS5555 : Subject5 : 90"
                                       };

            TrimesterPerformance trimPerform = fileOperation.CreateTrimesterPerformance(fileOperation.ReadStringsFromFile("testcreatetrimesterperformance1.txt"));

            List<string> temp = fileOperation.ChangeTrimesterPerformanceToStrings(trimPerform);
            string[] trimStrings = temp.ToArray<string>();

            Assert.AreEqual(trimStrings.Length, expectedValues.Length);

            for (int i = 0; i < trimStrings.Length; i++)
                Assert.AreEqual(expectedValues[i], trimStrings[i]);
        }


        [Test]
        public void ChangeStudentRecordToStrings_PassNullValue()
        {
            List<string> temp = fileOperation.ChangeStudentRecordToStrings(null);
            Assert.AreEqual(0, temp.Count);
        }

        [Test] // use the previous studentrecord file to check for functionality
        public void ChangeStudentRecordToStrings_SampleFromFile()
        {
            string[] expectedValues = {

            "Christopher Wong",
            "SE12345",
            "male",
            "chris@gmail.com",
            "01437283423",
            "SE",
            "120",
            " ",
            "*TRIMESTER START*",
            "2014 JAN Y1T1",
            "UECS1111 : Subject1 : 36",
            "UECS2222 : Subject2 : 55",
            "UECS3333 : Subject3 : 76",
            "*TRIMESTER END*",
            " ",
            "*TRIMESTER START*",
            "2014 MAY Y1T2",
            "UECS4444 : Subject4 : 82",
            "UECS5555 : Subject5 : 90",
            "UECS6666 : Subject6 : 57",
            "*TRIMESTER END*",
            " "
                                      };

            StudentRecord studRecord = fileOperation.CreateStudentRecord(fileOperation.ReadStringsFromFile("testcreatestudentrecord1.txt"));

            List<string> temp = fileOperation.ChangeStudentRecordToStrings(studRecord);
            string[] studStrings = temp.ToArray<string>();

            Assert.AreEqual(studStrings.Length, expectedValues.Length);

            for (int i = 0; i < studStrings.Length; i++)
                Assert.AreEqual(expectedValues[i], studStrings[i]);

        }




    }



}
