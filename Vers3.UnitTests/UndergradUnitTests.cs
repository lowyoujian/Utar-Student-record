using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vers3;

namespace Vers3.UnitTests
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


        [Test]
        public void CheckValidCreditHour_TestEmptyList_NoException()
        {
            List<StudyUnit> studyUnits = new List<StudyUnit>();
            UndergradUnit.CheckValidCreditHour(studyUnits);
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
            double[] gradePointValues = { 0, 1.0, 2.0, 3.0};
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

}
