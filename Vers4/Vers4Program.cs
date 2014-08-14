using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vers4
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

        public static string[] ExceptionMessages = {

    "Coursework component must be between 0 and 100",
"gradeRanges, gradeSymbols or gradePointValues cannot be null",
"gradeRanges, gradeSymbols and gradePointValues must be of the same length",
"Elements in gradeRanges are not in ascending order",
"Mark for a unit must be between 0 to 100"
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
        }

        public void AddUnitGrade(UnitGrade unitgrade)
        {
            if (unitgrade == null)
                throw new ArgumentException("Cannot add a null unitgrade to trimester performance");

            unitGrades.Add(unitgrade); }



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
            trimesterPerformanceList.Add(tpf); }

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
            int indexLocation, tempLocation;
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

                    tempLocation = studyUnits.IndexOf(tempUnit);

                    if (tempLocation >= 0)
                        locatedUnit = studyUnits[studyUnits.IndexOf(tempUnit)];
                    else
                        continue;

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

    }








    class Vers4Program
    {
        static void Main(string[] args)
        {


        }
    }
}
