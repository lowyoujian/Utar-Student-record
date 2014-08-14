using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Vers8
{


    public enum Campuses { FBF, FICT, FES, FCI };

    public enum UnitClassification { major, elective, compulsory, majorelective, uniunit };

    public enum Trimesters
    {
        Y1T1, Y1T2, Y1T3, Y2T1, Y2T2, Y2T3,
        Y3T1, Y3T2, Y3T3, Y4T1, Y4T2, Y4T3
    };

    public enum TrimesterMonths
    { JAN, MAY, OCT };


    public enum Grades
    { Aplus, A, Aminus, Bplus, B, Bminus, Cplus, C, F };

    public enum Gender
    { male, female };

    public enum AcademicStatus
    { probation, normal, terminated, graduated, leave };


    public class DefaultValues
    {
        public static int[] defaultGradeRanges = { 49, 54, 59, 64, 69, 74, 79, 89, 100 };
        public static string[] defaultGradeSymbols = { "F", "C", "C+", "B-", "B", "B+", "A-", "A", "A+" };
        public static double[] defaultGradePointValues = { 0, 2.0, 2.33, 2.67, 3.00, 3.33, 3.67, 4.0, 4.0 };

        public static int defaultMaxCreditHr = 7;
        public static int defaultMinCreditHr = 1;

        public static string dummyEmailAdd = "@gmail.com";
        public static string dummyContactNumber = "1234567890";

        public static string markTrimesterStart = "*TRIMESTER START*";
        public static string markTrimesterEnd = "*TRIMESTER END*";


        public static string[] ExceptionMessages = {

    "Coursework component must be between 0 and 100",
"gradeRanges, gradeSymbols or gradePointValues cannot be null",
"gradeRanges, gradeSymbols and gradePointValues must be of the same length",
"Elements in gradeRanges are not in ascending order",
"Mark for a unit must be between 0 to 100",

"Correct format for subject list is unitcode : unitName : prerequisites : credithours : unit classification : trimester offered : faculty :  programme : coursework component",
"The unit code is the 1st element and cannot be empty",
"Total credit hours for unit is numeric and is the 4th element", 
"Unit classification must be valid and is the 5th element",
"Trimesters offered must be valid and is the 6th element",
"Campuses offered must be valid and is the 7th element",
"Coursework component for unit is numeric and is the 9th element",

"The first line of the trimester record must specify year, month, trimester",
"The year is the 1st element and is numeric",
"The month is the 2nd element and must be a valid value (JAN, MAY, OCT)",
"The trimester is the 3rd element and must be a valid value (Y1T1, Y2T2,..)",
"Each line of trimester record consists of unit code, unit name and mark separated by a ':'",
"The mark is the 3rd element and is numeric",

"Insufficient lines to create a student record",
"The gender is either male or female and is specified in the 3rd line",
"Total credit hours for programme is numeric and is specified in the 7th line",
"Cannot process a null filename"

                                                   };
    }

    public class StudyUnit
    {

        public string unitCode
        { get; set; }

        public string unitName
        { get; set; }

        public List<string> prerequisites
        { get; set; }

        public int creditHours
        { get; set; }

        public UnitClassification classification
        { get; set; }

        public List<Trimesters> trimesterOffered
        { get; set; }

        public List<Campuses> campusesOffered
        { get; set; }

        public List<string> programmeOffered
        { get; set; }

        private int cwComponent;
        public int CWComponent
        {
            get { return cwComponent; }
            set
            {
                if ((value > 100) || (value < 0))
                    throw new ArgumentOutOfRangeException(DefaultValues.ExceptionMessages[0]);
                else
                    cwComponent = value;

            }
        }

        public StudyUnit(string unitCode, string unitName, List<string> prerequisites,
            int creditHours, UnitClassification classification,
            List<Trimesters> trimesterOffered, List<Campuses> campusesOffered,
            List<string> programmeOffered, int CWComponent)
        {

            if ((prerequisites == null) || (trimesterOffered == null) || (campusesOffered == null) || (programmeOffered == null))
                throw new ArgumentException("StudyUnit cannot be initialized with a null argument");

            this.unitCode = unitCode;
            this.unitName = unitName;
            this.prerequisites = prerequisites;
            this.creditHours = creditHours;
            this.classification = classification;
            this.trimesterOffered = trimesterOffered;
            this.campusesOffered = campusesOffered;
            this.programmeOffered = programmeOffered;
            this.CWComponent = CWComponent;
        }


        public override bool Equals(object obj)
        {
            if ((obj == null))
                return false;

            if ((obj.GetType() != typeof(StudyUnit)) &&
                (obj.GetType() != typeof(UndergradUnit)) &&
                (obj.GetType() != typeof(PostgradUnit)))
                return false;

            StudyUnit compUnit = (StudyUnit)obj;

            if (compUnit.unitCode == unitCode)
                return true;
            else
                return false;
        }

        public static void CheckAllUnitCodesUnique(List<StudyUnit> studyunits)
        {
            if (studyunits == null)
                throw new ArgumentException("CheckAllUnitCodesUnique cannot be run with a null argument");

            string exceptionMessage = "";
            List<string> duplicateCodes = new List<string>();
            for (int i = 0; i < studyunits.Count; i++)
            {
                if (studyunits.LastIndexOf(studyunits[i]) != i)
                {
                    if (!duplicateCodes.Contains(studyunits[i].unitCode))
                    {
                        exceptionMessage += "Duplicate unit " + studyunits[i].unitCode + " detected\n";
                        duplicateCodes.Add(studyunits[i].unitCode);
                    }
                }
            }
            if (exceptionMessage.Length > 1)
                throw new ArgumentException(exceptionMessage);
        }



        public static void CheckPrerequisitesCorrect(List<StudyUnit> studyunits)
        {
            if (studyunits == null)
                throw new ArgumentException("CheckPrerequisitesCorrect cannot be run with a null argument");


            List<string> tempString = new List<string>();
            List<Trimesters> tempTrimesters = new List<Trimesters>();
            List<Campuses> tempCampuses = new List<Campuses>();
            string exceptionMessage = "";

            for (int i = 0; i < studyunits.Count; i++)
            {
                foreach (string prequnit in studyunits[i].prerequisites)
                {
                    StudyUnit tempstudyunit = new StudyUnit(prequnit, "dummy", tempString, 0, UnitClassification.elective, tempTrimesters, tempCampuses, tempString, 0);
                    if (studyunits.IndexOf(tempstudyunit) == i)
                        exceptionMessage += "Unit " + studyunits[i].unitCode + " has the same prerequisite\n";
                    else if (studyunits.IndexOf(tempstudyunit) < 0)
                        exceptionMessage += "Prerequisite " + prequnit + " for unit " + studyunits[i].unitCode + " does not exist\n";
                }
            }
            if (exceptionMessage.Length > 1)
                throw new ArgumentException(exceptionMessage);
        }

    }


    public class UndergradUnit : StudyUnit
    {
        public UndergradUnit(string unitCode, string unitName, List<string> prerequisites,
            int creditHours, UnitClassification classification,
            List<Trimesters> trimesterOffered, List<Campuses> campusesOffered,
            List<string> programmeOffered, int CWComponent) :
            base(unitCode, unitName, prerequisites, creditHours,
            classification, trimesterOffered, campusesOffered,
            programmeOffered, CWComponent)
        { }

        public static void CheckValidCreditHour(List<StudyUnit> studyunits)
        {
            if (studyunits == null)
                throw new ArgumentException("CheckValidCreditHour cannot be run with a null argument");

            string exceptionMessage = "";

            for (int i = 0; i < studyunits.Count; i++)
            {
                UndergradUnit undergradunit;
                if (studyunits[i].GetType() == typeof(UndergradUnit))
                {
                    undergradunit = (UndergradUnit)studyunits[i];
                    if ((undergradunit.creditHours > DefaultValues.defaultMaxCreditHr) || (undergradunit.creditHours < DefaultValues.defaultMinCreditHr))
                        exceptionMessage += "Invalid credit hours for unit " + undergradunit.unitCode + "\n";
                }
            }
            if (exceptionMessage.Length > 1)
                throw new ArgumentException(exceptionMessage);
        }

    }


    public class PostgradUnit : StudyUnit
    {

        public PostgradUnit(string unitCode, string unitName, List<string> prerequisites,
            int creditHours, UnitClassification classification,
            List<Trimesters> trimesterOffered, List<Campuses> campusesOffered,
            List<string> programmeOffered, int CWComponent) :
            base(unitCode, unitName, prerequisites, creditHours,
            classification, trimesterOffered, campusesOffered,
            programmeOffered, CWComponent)
        { }
    }

    public class GradingScheme
    {
        public string unitCode
        { get; set; }

        public int[] gradeRanges
        { get; set; }

        public string[] gradeSymbols
        { get; set; }

        public double[] gradePointValues
        { get; set; }

        public GradingScheme()
            : this(DefaultValues.defaultGradeRanges,
                DefaultValues.defaultGradeSymbols, DefaultValues.defaultGradePointValues)
        {
        }

        public GradingScheme(int[] gradeRanges, string[] gradeSymbols,
            double[] gradePointValues)
        {
            if ((gradeRanges == null) || (gradeSymbols == null) ||
                gradePointValues == null)
                throw new ArgumentException(DefaultValues.ExceptionMessages[1]);

            if (gradeRanges.Length != gradeSymbols.Length)
                throw new ArgumentException(DefaultValues.ExceptionMessages[2]);
            if (gradeRanges.Length != gradePointValues.Length)
                throw new ArgumentException(DefaultValues.ExceptionMessages[2]);
            if (gradeSymbols.Length != gradePointValues.Length)
                throw new ArgumentException(DefaultValues.ExceptionMessages[2]);
            if (gradeRanges.Length != gradePointValues.Length)
                throw new ArgumentException(DefaultValues.ExceptionMessages[2]);

            for (int i = 0; i < gradeRanges.Length - 1; i++)
            {
                if (gradeRanges[i] >= gradeRanges[i + 1])
                    throw new ArgumentException(DefaultValues.ExceptionMessages[3]);
            }
            this.gradeRanges = gradeRanges;
            this.gradePointValues = gradePointValues;
            this.gradeSymbols = gradeSymbols;
        }
    }



    public class GradingSchemeResults : GradingScheme
    {
        public List<String>[] studentNames;
        public List<String>[] studentIDs;
        public double[] percentage;
        public double average;
        public int totalStudents;
        public int totalMarks;

        public GradingSchemeResults()
            : base()
        { InitializeLocal(); }

        public GradingSchemeResults(int[] gradeRanges, string[] gradeSymbols,
            double[] gradePointValues)
            : base(gradeRanges, gradeSymbols, gradePointValues)
        { InitializeLocal(); }

        private void InitializeLocal()
        {
            studentNames = new List<string>[gradeRanges.Length];
            studentIDs = new List<string>[gradeRanges.Length];
            percentage = new double[gradeRanges.Length];
            for (int i = 0; i < gradeRanges.Length; i++)
            {
                studentNames[i] = new List<string>();
                studentIDs[i] = new List<string>();
                percentage[i] = 0.0;
            }
            average = 0.0;
            totalStudents = 0;
            totalMarks = 0;
        }

        public void AddStudentMark(string studName, string studID, int mark)
        {

            if ((studName == null) || (studID == null))
                throw new ArgumentException("Invalid student name or ID to be added");

            if ((mark < 0) || (mark > 100))
                throw new ArgumentException("Invalid mark to be added. Marks must be between 0 and 100");

            totalStudents++;
            totalMarks += mark;
            for (int i = 0; i < gradeRanges.Length; i++)
            {
                if (mark <= gradeRanges[i])
                {
                    studentNames[i].Add(studName);
                    studentIDs[i].Add(studID);
                    break;
                }
            }
        }

        public void CalculateOverallResults()
        {
            for (int i = 0; i < gradeRanges.Length; i++)
                if (totalStudents != 0)
                    percentage[i] = Math.Round(((double)studentNames[i].Count * 100 / (double)totalStudents), 2);

            if (totalStudents != 0)
                average = Math.Round(((double)totalMarks / (double)totalStudents), 2);
        }
    }




    public class UnitGrade
    {
        public GradingScheme gradingScheme
        { get; set; }

        public string unitCode
        { get; set; }

        public string unitName
        { get; set; }

        public int mark
        { get; set; }

        public double gradePoint
        { get; private set; }

        public string gradeSymbol
        { get; private set; }

        public int numberAttempt
        { get; set; }

        public bool repeatedUnit
        { get; set; }

        public bool useForCalculate
        { get; set; }

        public UnitGrade(string unitCode, string unitName, int mark)
        {

            if ((unitCode == null) || (unitName == null))
                throw new ArgumentException("Invalid unit code or unit name to be created");

            if ((mark > 100) || (mark < 0))
                throw new ArgumentException(DefaultValues.ExceptionMessages[4]);
            this.unitCode = unitCode;
            this.unitName = unitName;
            this.mark = mark;
            this.gradingScheme = new GradingScheme();
            gradingScheme.unitCode = unitCode;
            numberAttempt = 1;
            repeatedUnit = false;
            useForCalculate = false;
        }

        public void CalculateGrade()
        {
            for (int i = 0; i < gradingScheme.gradeRanges.Length; i++)
            {
                if (mark <= gradingScheme.gradeRanges[i])
                {
                    gradeSymbol = gradingScheme.gradeSymbols[i];
                    gradePoint = gradingScheme.gradePointValues[i];
                    break;
                }
            }
        }


        public override bool Equals(object obj)
        {
            if ((obj == null) || (obj.GetType() != typeof(UnitGrade)))
                return false;
            UnitGrade compGrade = (UnitGrade)obj;

            if (compGrade.unitCode == unitCode)
                return true;
            else
                return false;
        }
    }

    public class TrimesterPerformance
    {
        public int year
        { get; set; }

        public TrimesterMonths month
        { get; set; }

        public Trimesters trimester
        { get; set; }

        public List<UnitGrade> unitGrades
        { get; set; }

        public int totalCreditHour
        { get; private set; }

        public int accumulatedCreditHour
        { get; set; }

        public double totalGradePoints
        { get; private set; }

        public double gradePointAverage
        { get; private set; }

        public double cumulativeGradePointAverage
        { get; set; }

        public TrimesterPerformance(int year,
            TrimesterMonths month, Trimesters trimester)
        {
            this.year = year;
            this.month = month;
            this.trimester = trimester;
            unitGrades = new List<UnitGrade>();
            totalGradePoints = gradePointAverage = cumulativeGradePointAverage = 0;
        }

        public void AddUnitGrade(UnitGrade unitgrade)
        {
            if (unitgrade == null)
                throw new ArgumentException("Cannot add a null unitgrade to trimester performance");
            unitGrades.Add(unitgrade);
        }

        public void CalculateGrades(List<StudyUnit> studyunits)
        {
            totalGradePoints = 0;
            totalCreditHour = 0;
            foreach (UnitGrade ugrade in unitGrades)
            {
                if ((!ugrade.repeatedUnit) || (ugrade.useForCalculate))
                {
                    foreach (StudyUnit studyunit in studyunits)
                    {
                        if (studyunit.unitCode == ugrade.unitCode)
                        {
                            totalCreditHour += studyunit.creditHours;
                            totalGradePoints += (ugrade.gradePoint * studyunit.creditHours);
                            break;
                        }
                    }
                }
            }

            totalGradePoints = Math.Round(totalGradePoints, 2);
            if (totalCreditHour != 0)
                gradePointAverage = Math.Round((totalGradePoints / totalCreditHour), 2);
        }


    }


    public abstract class MemberRecord
    {
        public string name
        { get; set; }

        public string IDNumber
        { get; set; }

        public Gender gender
        { get; set; }

        public string emailAddress
        { get; set; }

        public string contactNumber
        { get; set; }

        public MemberRecord(string name, string IDNumber, Gender gender,
            string emailAddress, string contactNumber)
        {
            if ((name == null) || (IDNumber == null) || (emailAddress == null) || (contactNumber == null))
                throw new ArgumentException("Invalid argument when creating Member Record");

            this.name = name;
            this.IDNumber = IDNumber;
            this.gender = gender;
            this.emailAddress = emailAddress;
            this.contactNumber = contactNumber;
        }

    }

    public class StudentRecord : MemberRecord
    {
        public string programme
        { get; set; }

        public List<TrimesterPerformance> trimesterPerformanceList
        { get; set; }

        public TrimesterPerformance trimToCompare
        { get; set; }

        public bool compareCGPA
        { get; set; }

        public AcademicStatus status
        { get; set; }

        public int creditHrsToGraduate
        { get; set; }


        public StudentRecord(string name, string IDNumber, Gender gender,
            string emailAddress, string contactNumber, string programme, int creditHrsToGraduate) :
            base(name, IDNumber, gender, emailAddress, contactNumber)
        {

            if ((programme == null) || (name == null) || (creditHrsToGraduate <= 0))
                throw new ArgumentException("Invalid argument when creating Student Record");

            trimesterPerformanceList = new List<TrimesterPerformance>();
            status = AcademicStatus.normal;
            this.programme = programme;
            trimToCompare = null;
            compareCGPA = true;
            this.creditHrsToGraduate = creditHrsToGraduate;
        }

        public void AddTrimesterPerformance(TrimesterPerformance tpf)
        {
            if (tpf == null)
                throw new ArgumentException("Cannot add null trimester to Student Record");
            trimesterPerformanceList.Add(tpf);

        }

        public void VerifyTrimesterOrder()
        {
            TrimesterMonths curMonth = TrimesterMonths.JAN;
            Trimesters curTrimester;
            int curYear = 0;
            string exceptionMessage = "";

            if (trimesterPerformanceList == null)
                return;

            for (int i = 0; i < trimesterPerformanceList.Count; i++)
            {
                if (i == 0)
                {
                    curMonth = trimesterPerformanceList[i].month;
                    curYear = trimesterPerformanceList[i].year;
                }
                else
                {
                    if (curMonth != trimesterPerformanceList[i].month)
                        exceptionMessage += "Incorrect month order detected in trimester " + trimesterPerformanceList[i].year + " " + trimesterPerformanceList[i].month + " " + trimesterPerformanceList[i].trimester + " for student ID : " + IDNumber + "\n";
                    if (curYear != trimesterPerformanceList[i].year)
                        exceptionMessage += "Incorrect year order detected in trimester " + trimesterPerformanceList[i].year + " " + trimesterPerformanceList[i].month + " " + trimesterPerformanceList[i].trimester + " for student ID : " + IDNumber + "\n";
                }

                curTrimester = (Trimesters)i;

                if (curTrimester != trimesterPerformanceList[i].trimester)
                    exceptionMessage += "Incorrect trimester order detected in trimester " + trimesterPerformanceList[i].year + " " + trimesterPerformanceList[i].month + " " + trimesterPerformanceList[i].trimester + " for student ID : " + IDNumber + "\n";

                if (curMonth == TrimesterMonths.OCT)
                {
                    curMonth = TrimesterMonths.JAN;
                    curYear++;
                }
                else
                    curMonth++;
            }
            if (exceptionMessage.Length > 1)
                throw new ArgumentException(exceptionMessage);

        }

        public void CheckPrerequisites(List<StudyUnit> studyUnits)
        {
            if (studyUnits == null)
                throw new ArgumentException("CheckPrerequisites cannot be run with a null argument");

            List<string> tempString = new List<string>();
            List<Trimesters> tempTrimesters = new List<Trimesters>();
            List<Campuses> tempCampuses = new List<Campuses>();

            List<UnitGrade> unitGrades, previousGrades;
            StudyUnit tempUnit, locatedUnit;
            UnitGrade tempUnitGrade, locatedUnitGrade;
            int indexLocation;
            List<string> prerequisites;
            bool foundPrerequisite;
            string exceptionMessage = "";

            if (trimesterPerformanceList == null)
                return;

            for (int i = 0; i < trimesterPerformanceList.Count; i++)
            {
                unitGrades = trimesterPerformanceList[i].unitGrades;
                for (int j = 0; j < unitGrades.Count; j++)
                {
                    tempUnit = new StudyUnit(unitGrades[j].unitCode, unitGrades[j].unitName, tempString, 0, UnitClassification.elective, tempTrimesters, tempCampuses, tempString, 40);

                    locatedUnit = studyUnits[studyUnits.IndexOf(tempUnit)];
                    prerequisites = locatedUnit.prerequisites;
                    for (int k = 0; k < locatedUnit.prerequisites.Count; k++)
                    {
                        foundPrerequisite = false;
                        for (int m = i - 1; m >= 0; m--)
                        {
                            previousGrades = trimesterPerformanceList[m].unitGrades;
                            tempUnitGrade = new UnitGrade(prerequisites[k], " ", 0);
                            indexLocation = previousGrades.IndexOf(tempUnitGrade);
                            if (indexLocation >= 0)
                            {
                                locatedUnitGrade = previousGrades[indexLocation];
                                if (locatedUnitGrade.mark > locatedUnitGrade.gradingScheme.gradeRanges[0])
                                {
                                    foundPrerequisite = true;
                                    break;
                                }
                            }
                        }
                        if (!foundPrerequisite)
                            exceptionMessage += "Unit : " + unitGrades[j].unitCode + " has prerequisite " +
                                locatedUnit.prerequisites[k] + " which has not been fulfilled yet\n";
                    }
                }
            }
            if (exceptionMessage.Length > 1)
                throw new ArgumentException(exceptionMessage);
        }


        public void CheckForRepeatedUnits()
        {
            List<UnitGrade> unitRecord = new List<UnitGrade>();
            foreach (TrimesterPerformance tpf in trimesterPerformanceList)
            {
                int findPos = 0;
                for (int i = 0; i < tpf.unitGrades.Count; i++)
                {
                    findPos = unitRecord.IndexOf(tpf.unitGrades[i]);
                    if (findPos >= 0)
                    {
                        unitRecord[findPos].repeatedUnit = true;
                        tpf.unitGrades[i].repeatedUnit = true;
                        tpf.unitGrades[i].numberAttempt = unitRecord[findPos].numberAttempt + 1;
                        unitRecord[findPos] = tpf.unitGrades[i];

                    }
                    else
                        unitRecord.Add(tpf.unitGrades[i]);
                }
            }

            foreach (UnitGrade ugrade in unitRecord)
            {
                if (ugrade.repeatedUnit)
                    ugrade.useForCalculate = true;
            }
        }


        public void CalculateCGPAOverall()
        {
            int overallCreditHour = 0;
            double overallGradePoints = 0;
            foreach (TrimesterPerformance tpf in trimesterPerformanceList)
            {
                overallCreditHour += tpf.totalCreditHour;
                overallGradePoints += tpf.totalGradePoints;
                if (overallCreditHour != 0)
                    tpf.cumulativeGradePointAverage = Math.Round((overallGradePoints / overallCreditHour), 2);
                tpf.accumulatedCreditHour = overallCreditHour;
            }
        }

        public void CheckAcademicStatus()
        {
            int totalTrimesters = trimesterPerformanceList.Count;

            if (totalTrimesters == 1)
            {
                if (trimesterPerformanceList[0].unitGrades.Count == 0)
                    status = AcademicStatus.leave;
                else if (trimesterPerformanceList[0].gradePointAverage < 2.0)
                    status = AcademicStatus.probation;
            }
            else
            {
                if (trimesterPerformanceList[totalTrimesters - 1].unitGrades.Count == 0)
                    status = AcademicStatus.leave;
                else if ((trimesterPerformanceList[totalTrimesters - 1].gradePointAverage < 2.0)
                    && (trimesterPerformanceList[totalTrimesters - 2].gradePointAverage < 2.0)
                    && (trimesterPerformanceList[totalTrimesters - 1].cumulativeGradePointAverage < 2.0))
                    status = AcademicStatus.terminated;
                else if ((trimesterPerformanceList[totalTrimesters - 1].accumulatedCreditHour >= creditHrsToGraduate) && (trimesterPerformanceList[totalTrimesters - 1].cumulativeGradePointAverage >= 2.0))
                    status = AcademicStatus.graduated;
                else if (trimesterPerformanceList[totalTrimesters - 1].gradePointAverage < 2.0)
                    status = AcademicStatus.probation;
                else
                    status = AcademicStatus.normal;
            }
        }

    }


    public class DataAnalyzer
    {

        public GradingSchemeResults GetUnitStatsForTrimester(int year, TrimesterMonths triMonths, Trimesters trimester, string unitCodeToLookFor, List<StudentRecord> studentRecords)
        {

            if ((unitCodeToLookFor == null) || (studentRecords == null))
                throw new ArgumentException("GetUnitStats cannot be run will null arguments");

            GradingSchemeResults gradeResults = new GradingSchemeResults();
            foreach (StudentRecord studRecord in studentRecords)
            {
                foreach (TrimesterPerformance tpf in studRecord.trimesterPerformanceList)
                {
                    if ((tpf.month == triMonths) && (tpf.trimester == trimester) && (tpf.year == year))
                    {
                        foreach (UnitGrade ugrade in tpf.unitGrades)
                        {
                            if (ugrade.unitCode == unitCodeToLookFor)
                                gradeResults.AddStudentMark(studRecord.name, studRecord.IDNumber, ugrade.mark);
                        }
                        break;
                    }
                }
            }
            gradeResults.CalculateOverallResults();
            return gradeResults;
        }

    }


    public interface IRandomFunctionality
    {
        int ReturnRandomNumber(int upperLimit, int lowerLimit);
    }

    public class RealRandomNumberGenerator : IRandomFunctionality
    {
        Random rnd = new Random();

        public int ReturnRandomNumber(int upperLimit, int lowerLimit)
        {
            return rnd.Next(upperLimit, lowerLimit);
        }
    }

    public class TestDataGenerator
    {
        IRandomFunctionality rndFunction;

        public TestDataGenerator(IRandomFunctionality rndFunction)
        {
            this.rndFunction = rndFunction;
        }

        // default constructor uses the real random number generation functionality
        public TestDataGenerator()
            : this(new RealRandomNumberGenerator())
        {
        }

        public List<StudentRecord> GenerateStudentRecords(int numToGenerate, StudentRecord courseStructure, List<string> maleNames, List<string> femaleNames, List<string> surNames)
        {

            if ((numToGenerate < 0) || (courseStructure == null) || (maleNames == null) || (femaleNames == null) || (surNames == null))
                throw new ArgumentException("GenerateStudentRecords cannot be run with null or invalid arguments");

            List<StudentRecord> records = new List<StudentRecord>();
            StudentRecord studRecord;
            string studName = "";
            string idnumber = "";
            Gender studGender;
            for (int i = 0; i < numToGenerate; i++)
            {
                studGender = (Gender)rndFunction.ReturnRandomNumber(0, 2);
                if (studGender == Gender.male)
                    studName = maleNames[rndFunction.ReturnRandomNumber(0, maleNames.Count)];
                else if (studGender == Gender.female)
                    studName = femaleNames[rndFunction.ReturnRandomNumber(0, femaleNames.Count)];
                studName += " " + surNames[rndFunction.ReturnRandomNumber(0, surNames.Count)];

                idnumber = courseStructure.programme + rndFunction.ReturnRandomNumber(1000, 10000) + i;
                studRecord = new StudentRecord(studName, idnumber, studGender,
                    studName + DefaultValues.dummyEmailAdd, DefaultValues.dummyContactNumber, courseStructure.programme, courseStructure.creditHrsToGraduate);

                List<UnitGrade> newGradeList = new List<UnitGrade>();
                UnitGrade tempGrade;
                TrimesterPerformance tempTrimester;
                int gradeMark;

                foreach (TrimesterPerformance tpf in courseStructure.trimesterPerformanceList)
                {
                    tempTrimester = new TrimesterPerformance(tpf.year, tpf.month, tpf.trimester);
                    newGradeList = new List<UnitGrade>();
                    foreach (UnitGrade ugrade in tpf.unitGrades)
                    {
                        if (rndFunction.ReturnRandomNumber(0, 5) == 1)
                            gradeMark = rndFunction.ReturnRandomNumber(0, 51);
                        else
                            gradeMark = rndFunction.ReturnRandomNumber(51, 100);
                        tempGrade = new UnitGrade(ugrade.unitCode, ugrade.unitName, gradeMark);
                        tempGrade.CalculateGrade();
                        newGradeList.Add(tempGrade);
                    }
                    tempTrimester.unitGrades = newGradeList;
                    studRecord.AddTrimesterPerformance(tempTrimester);
                }

                records.Add(studRecord);
            }
            return records;
        }
    }



    public class FileOperations
    {



        public List<string> ReadStringsFromFile(string fileName)
        {
            if (fileName == null)
                throw new ArgumentException("Cannot read from null file name");
            List<string> strings = new List<string>();
            string line;
            FileStream aFile = new FileStream(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(aFile);
            line = sr.ReadLine();
            // Read data in line by line.
            while (line != null)
            {
                strings.Add(line);
                line = sr.ReadLine();
            }
            sr.Close();
            return strings;
        }


        public void WriteStringsToFile(string fileName, List<string> stringsToWrite)
        {
            if ((fileName == null) || (stringsToWrite == null))
                throw new ArgumentException("Cannot write to a null file name or write a null list");

            FileStream aFile = new FileStream(fileName, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(aFile);
            foreach (string line in stringsToWrite)
                sw.WriteLine(line);
            sw.Close();
        }



        public List<StudyUnit> CreateSubjectList(List<string> strings)
        {
            if (strings == null)
                throw new ArgumentException("CreateSubjectList cannot be run on a null string list");

            char[] delimiters = new char[] { ' ' };
            string exceptionMessage = "";
            string unitCode, unitName;
            int creditHours = 0, CWComponent = 0;
            UnitClassification classification = UnitClassification.major;

            List<string> prerequisites;
            List<Trimesters> trimesterOffered;
            List<Campuses> campusesOffered;
            List<string> programmeOffered;

            string[] temp, components;
            StudyUnit studyunit;

            List<StudyUnit> allUnits = new List<StudyUnit>();

            foreach (string line in strings)
            {
                if (line.Length < 5)
                    continue;
                prerequisites = new List<string>();
                trimesterOffered = new List<Trimesters>();
                campusesOffered = new List<Campuses>();
                programmeOffered = new List<string>();

                components = line.Split(':');

                if (components.Length != 9)
                {
                    throw new ArgumentException(components[0].Trim() + " : " + DefaultValues.ExceptionMessages[5]);
                }

                unitCode = components[0].Trim();

                if (unitCode.Length == 0)
                {
                    throw new ArgumentException(components[1].Trim() + " : " + DefaultValues.ExceptionMessages[6]);
                }

                unitName = components[1].Trim();
                temp = components[2].Trim().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < temp.Length; j++)
                    if (temp[j].Length > 2)
                        prerequisites.Add(temp[j]);

                try
                {
                    creditHours = Convert.ToInt32(components[3]);
                }
                catch (Exception)
                {
                    exceptionMessage += DefaultValues.ExceptionMessages[7] + "\n";
                }

                try
                {
                    classification = (UnitClassification)Enum.Parse(typeof(UnitClassification),
                    components[4].ToLower());
                }
                catch (Exception)
                {
                    exceptionMessage += DefaultValues.ExceptionMessages[8] + "\n";
                }

                temp = components[5].Trim().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    for (int j = 0; j < temp.Length; j++)
                    {
                        trimesterOffered.Add((Trimesters)Enum.Parse(typeof(Trimesters),
                        temp[j].ToUpper()));
                    }
                }
                catch (Exception)
                {
                    exceptionMessage += DefaultValues.ExceptionMessages[9] + "\n";
                }



                temp = components[6].Trim().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    for (int j = 0; j < temp.Length; j++)
                    {
                        campusesOffered.Add((Campuses)Enum.Parse(typeof(Campuses),
                        temp[j].ToUpper()));
                    }
                }
                catch (Exception)
                {
                    exceptionMessage += DefaultValues.ExceptionMessages[10] + "\n";
                }

                temp = components[7].Trim().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < temp.Length; j++)
                {
                    programmeOffered.Add(temp[j]);
                }

                try
                {
                    CWComponent = Convert.ToInt32(components[8]);
                }
                catch (Exception)
                {
                    exceptionMessage += DefaultValues.ExceptionMessages[11] + "\n";
                }

                if (exceptionMessage.Length > 1)
                    throw new ArgumentException(unitCode + " : " + exceptionMessage);

                studyunit = new UndergradUnit(unitCode, unitName, prerequisites, creditHours,
                    classification, trimesterOffered, campusesOffered, programmeOffered, CWComponent);

                allUnits.Add(studyunit);
            }
            return allUnits;
        }


        public TrimesterPerformance CreateTrimesterPerformance(List<string> strings)
        {
            if (strings == null)
                throw new ArgumentException("CreateTrimesterPerformance cannot be run on a null string list");

            char[] delimiters = new char[] { ' ' };
            string exceptionMessage = "";
            TrimesterMonths month = TrimesterMonths.JAN;
            Trimesters trimester = Trimesters.Y1T1;
            int year = 0;

            IEnumerator ie = strings.GetEnumerator();
            ie.MoveNext();
            string[] elements = ((string)ie.Current).Trim().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            if (elements.Length != 3)
            {
                for (int i = 0; i < elements.Length; i++)
                    exceptionMessage += elements[i] + " ";
                throw new ArgumentException(exceptionMessage + ": " + DefaultValues.ExceptionMessages[12]);
            }

            try
            {
                year = Convert.ToInt32(elements[0]);
            }
            catch (Exception)
            {
                exceptionMessage += DefaultValues.ExceptionMessages[13] + "\n";
            }

            try
            {
                month = (TrimesterMonths)Enum.Parse(typeof(TrimesterMonths), elements[1].ToUpper());
            }
            catch (Exception)
            {
                exceptionMessage += DefaultValues.ExceptionMessages[14] + "\n";
            }

            try
            {
                trimester = (Trimesters)Enum.Parse(typeof(Trimesters), elements[2].ToUpper());
            }
            catch (Exception)
            {
                exceptionMessage += DefaultValues.ExceptionMessages[15] + "\n";
            }

            if (exceptionMessage.Length > 1)
                throw new ArgumentException(elements[0] + " " + elements[1] + " " + elements[2] + " : " + exceptionMessage);

            TrimesterPerformance trimesterPerformance = new TrimesterPerformance(year, month, trimester);

            string curLine;
            string[] components;
            UnitGrade ugrade;
            int mark = 0;

            while (ie.MoveNext())
            {
                curLine = (string)ie.Current;
                components = curLine.Trim().Split(':');

                if (components.Length != 3)
                {
                    exceptionMessage += (year + " " + month + " " + trimester) + " : " + DefaultValues.ExceptionMessages[16];
                    throw new ArgumentException(exceptionMessage);
                }

                try
                {
                    mark = Convert.ToInt32(components[2]);
                }
                catch (Exception)
                {
                    exceptionMessage += components[0].Trim() + " : " + DefaultValues.ExceptionMessages[17];
                    throw new ArgumentException(exceptionMessage);
                }

                ugrade = new UnitGrade(components[0].Trim(), components[1].Trim(), mark);
                ugrade.gradingScheme = new GradingScheme();
                ugrade.CalculateGrade();
                trimesterPerformance.AddUnitGrade(ugrade);
            }

            return trimesterPerformance;
        }


        public StudentRecord CreateStudentRecord(List<string> strings)
        {
            if (strings == null)
                throw new ArgumentException("CreateStudentRecord cannot be run on a null string list");

            GradingScheme gradingScheme = new GradingScheme();
            string exceptionMessage = "";
            Gender gender = Gender.male;
            int creditHrs = 0;

            if (strings.Count < 7)
                throw new ArgumentException(DefaultValues.ExceptionMessages[18]);
            try
            {
                gender = (Gender)Enum.Parse(typeof(Gender), strings[2].ToLower());
            }
            catch (Exception)
            {
                exceptionMessage += DefaultValues.ExceptionMessages[19] + "\n";
            }
            try
            {
                creditHrs = Convert.ToInt32(strings[6]);
            }
            catch (Exception)
            {
                exceptionMessage += DefaultValues.ExceptionMessages[20];
            }
            if (exceptionMessage.Length > 1)
                throw new ArgumentException(exceptionMessage);

            StudentRecord studRecord = new StudentRecord(strings[0], strings[1], gender, strings[3], strings[4], strings[5], creditHrs);

            bool readingRecord = false;
            TrimesterPerformance trimPerformance;
            List<String> tempStrings = new List<string>();
            string curString;
            for (int i = 6; i < strings.Count; i++)
            {
                curString = strings[i].Trim();
                if (readingRecord)
                {
                    if (curString == DefaultValues.markTrimesterEnd)
                    {
                        trimPerformance = CreateTrimesterPerformance(tempStrings);
                        studRecord.AddTrimesterPerformance(trimPerformance);
                        readingRecord = false;
                    }
                    else if (curString.Length > 5)
                        tempStrings.Add(curString);
                }
                else if (curString == DefaultValues.markTrimesterStart)
                {
                    readingRecord = true;
                    tempStrings = new List<string>();
                }
            }
            return studRecord;
        }


        public List<string> ChangeTrimesterPerformanceToStrings(TrimesterPerformance tpf)
        {
            List<string> trimString = new List<string>();
            if (tpf == null)
                return trimString;

            trimString.Add(tpf.year + " " + tpf.month + " " + tpf.trimester);
            foreach (UnitGrade ugrade in tpf.unitGrades)
                trimString.Add(ugrade.unitCode + " : " + ugrade.unitName + " : " + ugrade.mark);
            return trimString;
        }

        public List<string> ChangeStudentRecordToStrings(StudentRecord studRecord)
        {

            List<string> recordStrings = new List<string>();
            if (studRecord == null)
                return recordStrings;

            recordStrings.Add(studRecord.name);
            recordStrings.Add(studRecord.IDNumber);
            recordStrings.Add(studRecord.gender.ToString());
            recordStrings.Add(studRecord.emailAddress);
            recordStrings.Add(studRecord.contactNumber);
            recordStrings.Add(studRecord.programme);
            recordStrings.Add(studRecord.creditHrsToGraduate.ToString());
            recordStrings.Add(" ");
            foreach (TrimesterPerformance tpf in studRecord.trimesterPerformanceList)
            {
                recordStrings.Add(DefaultValues.markTrimesterStart);
                recordStrings.AddRange(ChangeTrimesterPerformanceToStrings(tpf));
                recordStrings.Add(DefaultValues.markTrimesterEnd);
                recordStrings.Add(" ");
            }
            return recordStrings;
        }


    }

    public interface DisplayFunctionality
    {
        void DisplayMessages(List<string> messages);
    }

    public class DisplayToScreen : DisplayFunctionality
    {
        public void DisplayMessages(List<string> messages)
        {
            foreach (string message in messages)
                Console.WriteLine(message);
            Console.Write("\nPress any key to continue ....");
            Console.ReadKey();
        }
    }


    public class DisplayOperations
    {
        public DisplayFunctionality displayFunction;

        public DisplayOperations(DisplayFunctionality displayFunction)
        {
            this.displayFunction = displayFunction;
        }

        // Default constructor will use functionality to display to screen
        public DisplayOperations()
            : this(new DisplayToScreen())
        {
        }

        public void ShowSubjectList(List<StudyUnit> studyUnits)
        {
            if (studyUnits == null)
                return;

            List<string> messagesToShow = new List<string>();
            string tempString;

            messagesToShow.Add("*********** SUBJECTS LIST ************");
            messagesToShow.Add("Total number of subjects in this list is " + studyUnits.Count);
            displayFunction.DisplayMessages(messagesToShow);

            foreach (StudyUnit studyunit in studyUnits)
            {
                messagesToShow = new List<string>();
                messagesToShow.Add("\nUnit code and name : " + studyunit.unitCode + " " + studyunit.unitName);
                tempString = "Prerequisites : ";
                foreach (string temp in studyunit.prerequisites)
                    tempString += temp + " " ;
                messagesToShow.Add(tempString);

                messagesToShow.Add("Credit hours : " + studyunit.creditHours);
                messagesToShow.Add("Classification : " + studyunit.classification);
                
                tempString = "Trimesters offered : ";
                foreach (Trimesters temp in studyunit.trimesterOffered)
                    tempString += temp + " ";
                messagesToShow.Add(tempString);

                tempString = "Campuses offered : ";
                foreach (Campuses temp in studyunit.campusesOffered)
                    tempString += temp + " ";
                messagesToShow.Add(tempString);

                tempString = "Programmes offering this unit : ";
                foreach (string temp in studyunit.programmeOffered)
                    tempString += temp + " ";
                messagesToShow.Add(tempString);

                messagesToShow.Add("Coursework component : " + studyunit.CWComponent);
                displayFunction.DisplayMessages(messagesToShow);

            }
        }


        public void ShowTrimesterPerformance(TrimesterPerformance tp)
        {
            if (tp == null)
                return;

            List<string> messagesToShow = new List<string>();
            string tempString = "";

            messagesToShow.Add(" ");
            tempString += "Year : " + tp.year;
            tempString += " Month : " + tp.month;
            tempString += " Trimester : " + tp.trimester;

            messagesToShow.Add(tempString);
            foreach (UnitGrade ugrade in tp.unitGrades)
            {
                tempString = "";
                if (ugrade.repeatedUnit)
                    tempString += "(*" + ugrade.numberAttempt + ") ";
                tempString += "Unit : " + ugrade.unitCode + " " + ugrade.unitName + ", Mark : " + ugrade.mark + ", GradePoint : " + ugrade.gradePoint;
                messagesToShow.Add(tempString);

            }
            messagesToShow.Add("GPA : " + tp.gradePointAverage.ToString("F") + " CGPA : " + tp.cumulativeGradePointAverage.ToString("F"));

            messagesToShow.Add("Credit hours this trimester : " + tp.totalCreditHour + " Accumulated credit hours : " + tp.accumulatedCreditHour);

            displayFunction.DisplayMessages(messagesToShow);

        }

        public void ShowStudentRecord(StudentRecord studentRecord)
        {
            if (studentRecord == null)
                return;

            List<string> messagesToShow = new List<string>();
            string tempString = "";

            tempString += "Name : " + studentRecord.name;
            tempString += " ID Number : " + studentRecord.IDNumber;
            tempString += " Gender : " + studentRecord.gender;
            messagesToShow.Add(tempString);

            tempString = "Email Address : " + studentRecord.emailAddress;
            tempString += " Contact Number : " + studentRecord.contactNumber;
            messagesToShow.Add(tempString);

            tempString = "Programme : " + studentRecord.programme;
            tempString += " Total credit hours : " + studentRecord.creditHrsToGraduate;
            messagesToShow.Add(tempString);

            messagesToShow.Add("Academic Status : " + studentRecord.status);

            displayFunction.DisplayMessages(messagesToShow);

            foreach (TrimesterPerformance tpf in studentRecord.trimesterPerformanceList)
            {
                ShowTrimesterPerformance(tpf);
            }

        }

        public void ShowUnitStatsForTrimester(GradingSchemeResults gradeResults)
        {

            if (gradeResults == null)
                return;

            List<string> messagesToShow = new List<string>();
            string tempString = "";

            int lowerMarkRange = 0;
            List<string> studentNameList, studentIDList;

            for (int i = 0; i < gradeResults.gradeRanges.Length; i++)
            {
                studentNameList = gradeResults.studentNames[i];
                studentIDList = gradeResults.studentIDs[i];

                tempString = lowerMarkRange + " - " + gradeResults.gradeRanges[i] + " : ";
                tempString += studentNameList.Count + " students, Percentage of total : " + gradeResults.percentage[i] + ", ";

                for (int j = 0; j < studentNameList.Count; j++)
                    tempString += (j + 1) + ": " + studentIDList[j] + " " + studentNameList[j] + " ";

                messagesToShow.Add(tempString);
                lowerMarkRange = gradeResults.gradeRanges[i] + 1;
            }
            messagesToShow.Add("Average mark : " + gradeResults.average);
            displayFunction.DisplayMessages(messagesToShow);
        }



    }










    class Vers8Program
    {
        static void Main(string[] args)
        {

        }
    }
}
