using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vers2;

namespace Vers2.UnitTests
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
            string[] unitCodesToUse = { "UECS8888"};
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
        ExpectedMessage="Duplicate unit UECS8888 detected\n")] 
        public void CheckAllUnitCodesUnique_TestWith2SameUnitCodes_1ErrorMessage()
        {
            string[] unitCodesToUse = {"UECS8888", "UECS8888"};
            StudyUnit.CheckAllUnitCodesUnique(CreateStudyUnitList(unitCodesToUse));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
        ExpectedMessage = "Duplicate unit UECS8888 detected\n")]
        public void CheckAllUnitCodesUnique_TestWith4SameUnitCodes_1ErrorMessage()
        {
            string[] unitCodesToUse = { "UECS8888", "UECS8888", "UECS8888", "UECS8888"};
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

}
