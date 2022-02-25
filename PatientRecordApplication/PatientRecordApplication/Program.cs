using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

    /// <summary>
    /// This program uses exception handling and files
    /// </summary>
    /// <Student>Drew Barlow</Student>
    /// <Class>CIS297</Class>
    /// <Semester>Winter 2022</Semester>
namespace PatientRecordApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            PatientData();
            DisplayData();
            EnterID();
            BalanceDue();
        }

        /// <summary> 
        /// Method to input patient data and write it to a file
        /// <summary>
        static void PatientData()
        {
            const int END = 999;
            const string DELIM = ",";
            const string FILENAME = "PatientData.txt";
            var continueLoop = true;
            Patientclass pc = new Patientclass();
            FileStream outFile = new FileStream(FILENAME,
            FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(outFile);
            do
            {
                /// <summary>
                /// try and catch block to check if patient ID is an interger
                /// <summary>
                try
                {
                    Write("Enter patient ID number or " + END + " to continue >> ");
                    pc.IDNum = Convert.ToInt32(ReadLine());
                    continueLoop = false;
                }
                catch(Exception formatException)
                {
                    Console.WriteLine($"\n{formatException.Message}");
                    Console.WriteLine("You must enter an integer. Please try again. \n");
                }
            } while (continueLoop);

            while (pc.IDNum != END)
            {
                Write("Enter patient name >> ");
                pc.Name = ReadLine();
                do
                {
                    /// <summary>
                    /// try and catch block to check if account balance is positive or negative
                    /// <summary>
                    try
                    {
                        Write("Enter patient account balance >> ");
                        pc.AccountBalance = Convert.ToDecimal(ReadLine());
                        if(pc.AccountBalance < 0)
                        {
                            throw new NegativeNumberException("Account balance cannot be negative.");
                        }
                    }
                    catch(NegativeNumberException negativeNumberException)
                    {
                        Console.WriteLine("\n" + negativeNumberException.Message);
                        Console.WriteLine("Please enter a positive account balance.\n");
                    }
                } while (pc.AccountBalance < 0);

                writer.WriteLine(pc.IDNum + DELIM + pc.Name + DELIM + pc.AccountBalance);
                Console.WriteLine();
                Write("Enter next patient ID number or " + END + " to continue >> ");
                pc.IDNum = Convert.ToInt32(ReadLine());
            }
            writer.Close();
            outFile.Close();
        }

        /// <summary>
        /// Method to display all patient data entered into file
        /// <summary>
        static void DisplayData()
        {
            const char DELIM = ',';
            const string FILENAME = "PatientData.txt";
            Patientclass pc = new Patientclass();
            FileStream inFile = new FileStream(FILENAME, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);
            string recordIn = "";
            string[] fields;
            try
            {
                WriteLine("\n{0,-5}{1,-12}{2,8}\n", "ID ", "Name", "Balance");
                recordIn = reader.ReadLine();
            }
            finally
            {
                while (recordIn != null)
                {
                    fields = recordIn.Split(DELIM);
                    pc.IDNum = Convert.ToInt32(fields[0]);
                    pc.Name = fields[1];
                    pc.AccountBalance = Convert.ToDecimal(fields[2]);
                    WriteLine("{0,-5}{1,-12}{2,8}", pc.IDNum, pc.Name, pc.AccountBalance.ToString("C"));
                    recordIn = reader.ReadLine();
                }
            reader.Close();
            inFile.Close();
            Console.WriteLine();
            }
        }

        /// <summary>
        /// Method to let the user search for a specific patient ID
        /// <summary>
        static void EnterID()
        {
            const char DELIM = ',';
            const int END = 999;
            const string FILENAME = "PatientData.txt";
            Patientclass pc = new Patientclass();
            FileStream inFile = new FileStream(FILENAME,
            FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);
            string recordIn;
            string[] fields;
            int idNum = 0;
            var continueLoop = true;
            do
            {
                /// <summary>
                /// try and catch block to check if entered ID is an integer
                /// <summary>
                try
                {
                    Write("Enter patient ID number to find or " + END + " to continue >> ");
                    idNum = Convert.ToInt32(Console.ReadLine());
                    continueLoop = false;
                }
                catch(Exception formatException)
                {
                    Console.WriteLine($"\n{formatException.Message}");
                    Console.WriteLine("You must enter an ID number. Please try again. \n");
                }
            } while (continueLoop);
           
            while (idNum != END)
            {
                WriteLine("\n{0,-5}{1,-12}{2,8}\n", "ID ", "Name", "Balance");
                inFile.Seek(0, SeekOrigin.Begin);
                recordIn = reader.ReadLine();
                while (recordIn != null)
                {
                    fields = recordIn.Split(DELIM);
                    pc.IDNum = Convert.ToInt32(fields[0]);
                    pc.Name = fields[1];
                    pc.AccountBalance = Convert.ToDecimal(fields[2]);
                    if (pc.IDNum == idNum)
                        WriteLine("{0,-5}{1,-12}{2,8}", pc.IDNum,
                        pc.Name, pc.AccountBalance.ToString("C"));
                        recordIn = reader.ReadLine();
                }
                Write("\nEnter patient ID number to find or " + END + " to continue >> ");
                idNum = Convert.ToInt32(Console.ReadLine());
            }
            reader.Close();
            inFile.Close(); 
            Console.WriteLine();
        }

        /// <summary>
        /// Method to let the user search for account with a minumim balance 
        /// <summary>
        static void BalanceDue()
        {
            const char DELIM = ',';
            const int END = 999;
            const string FILENAME = "PatientData.txt";
            Patientclass pc = new Patientclass();
            FileStream inFile = new FileStream(FILENAME,
            FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);
            string recordIn;
            string[] fields;
            decimal accountBalance = 0;
            try
            {
                Write("Enter minimum account balance to find or " + END + " to continue >> ");
                accountBalance = Convert.ToDecimal(Console.ReadLine());
            }
            finally
            {
                while (accountBalance != END)
                {
                WriteLine("\n{0,-5}{1,-12}{2,8}\n",
                   "ID ", "Name", "Balance");
                inFile.Seek(0, SeekOrigin.Begin);
                recordIn = reader.ReadLine();
                while (recordIn != null)
                {
                    fields = recordIn.Split(DELIM);
                    pc.IDNum = Convert.ToInt32(fields[0]);
                    pc.Name = fields[1];
                    pc.AccountBalance = Convert.ToDecimal(fields[2]);
                    if (pc.AccountBalance >= accountBalance)
                        WriteLine("{0,-5}{1,-12}{2,8}", pc.IDNum,
                           pc.Name, pc.AccountBalance.ToString("C"));
                    recordIn = reader.ReadLine();
                }
                Write("\nEnter minimum account balance to find or " +
                   END + " to continue >> ");
                accountBalance = Convert.ToDecimal(Console.ReadLine());
                }
            reader.Close();  
            inFile.Close(); 
            }
        }
    }

    /// <summary>
    /// User defined exception handling class for negative intergers
    /// <summary>
    public class NegativeNumberException : Exception
    {
        public NegativeNumberException() : base("Balance cannot be negative. Please enter again.")
        {

        }
        public NegativeNumberException(string messageValue) : base(messageValue)
        {

        }
        public NegativeNumberException(string messageValue, Exception inner) : base(messageValue, inner)
        {

        }
    }

    /// <summary>
    /// Class for the patient
    /// <summary>
    public class Patientclass
    {
        public int IDNum { get; set; }
        public string Name { get; set; }
        public decimal AccountBalance { get; set; }
    }
}
