using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinchAPI;

namespace SentryFinch__witteh_
{
    class Program
    {
        //********************************
        //Title:SentryFinch_witteh
        //Author:Payton Bramer, Hannah Witte
        //Date:4/8/2019
        //********************************
        static void Main(string[] args)
        {

            DisplayWelcomeScreen();
            DisplayMenu();
            DisplayClosingScreen();

        }

        static void DisplayMenu()
        {
            string menuChoice;
            bool exiting = false;
            Finch myfinch = new Finch();
            myfinch.connect();
            double lowerTempThreshold = 0;
            double lowerLightThreshold = 0;
           

            while (!exiting)
            {
                DisplayHeader("Main Menu");

                Console.WriteLine("1)Setup");
                Console.WriteLine("2)Activate Sentry Bot");
                Console.WriteLine("3) Light Threshold");
                Console.WriteLine("E) Exit");
                Console.WriteLine();
                Console.WriteLine("Enter Menu Choice");
                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        lowerTempThreshold = DisplaySetup(myfinch, out lowerLightThreshold);
                        break;
                    case "2":
                        DisplayActivateSentrybot(lowerTempThreshold, myfinch, lowerLightThreshold);
                        break;
                    case "3":
                       
                        break;
                    case "E":
                    case "e":
                        exiting = true;
                        myfinch.disConnect();
                        break;
                    default:
                        Console.WriteLine("Please enter a valid menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            }
        }

        static bool LightAboveThresholdValue(double lowerLightThreshold, Finch myfinch)
        {
           

            if (myfinch.getLightSensors().Average() > lowerLightThreshold)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void DisplayActivateSentrybot(double lowerTempThresholdValue, Finch myfinch, double lowerLightThreshold)
        {
            double ambientTemp;
            double ambientLight;

            ambientTemp = myfinch.getTemperature();
            //ambientLight = myfinch.getRightLightSensor() + myfinch.getLeftLightSensor();
            ambientLight = myfinch.getLightSensors().Average();

            DisplayHeader("Activate Sentry Bot");
            Console.WriteLine($"The ambient temperature is {ambientTemp}");
            Console.WriteLine($"The lower Temperature Threshold is {lowerTempThresholdValue}");

            while (!TemperatureBelowThresholdValue(lowerTempThresholdValue,myfinch) && !LightAboveThresholdValue(lowerLightThreshold, myfinch))
            {
                Console.WriteLine($"light threshold{lowerLightThreshold}, light {ambientLight}, temp threshold {lowerTempThresholdValue}, temp {ambientTemp}");
                TemperatureNominalIndicator(myfinch);
            }

            //if (TemperatureBelowThresholdValue(lowerTempThresholdValue, myfinch) && LightAboveThresholdValue(lowerLightThreshold, myfinch))
            //{
                myfinch.noteOn(250);
                myfinch.setLED(255, 0, 0);
                myfinch.wait(3000);
                myfinch.noteOff();
                myfinch.setLED(0, 0, 0);
            //}

            DisplayContinuePrompt();
        }

        static void TemperatureNominalIndicator(Finch myfinch)
        {
            myfinch.setLED(0, 255, 0);
            myfinch.wait(1000);
            myfinch.setLED(0, 0, 0);
        }

        static bool TemperatureBelowThresholdValue(double lowerTempThresholdValue, Finch myfinch)
        {
            if (myfinch.getTemperature() <= lowerTempThresholdValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static double DisplaySetup(Finch myfinch, out double lowerLightThreshold)
        {
            double temperatureDifference;
            double lowerTempThreshold;
            double ambientTemp;
            double lightDifference;
            double ambientLight;

            DisplayHeader("Setup Sentry Bot");

            Console.Write("Enter desired change in temperature:");
            double.TryParse(Console.ReadLine(), out temperatureDifference);
            
            ambientTemp = myfinch.getTemperature();

            lowerTempThreshold = ambientTemp - temperatureDifference;

            Console.Write("Enter desired change in light:");
            double.TryParse(Console.ReadLine(), out lightDifference);

            ambientLight = myfinch.getLeftLightSensor() + myfinch.getRightLightSensor();

            lowerLightThreshold = ambientLight + lightDifference;

            DisplayContinuePrompt();
            return lowerTempThreshold;
          
        }

        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tWelcome to our Application");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("press any key to continue");
            Console.ReadKey();
        }

        static void DisplayHeader(string headerText)
        {
            //
            //display header
            //

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t" + headerText);
            Console.WriteLine();

        }

        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank You for using our application"); // makes it appear over more
            Console.WriteLine();

            DisplayContinuePrompt();
        }
    }
}
