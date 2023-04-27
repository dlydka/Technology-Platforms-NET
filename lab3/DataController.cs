using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laboratorium_10
{
    class DataController
    {
        private delegate int CompareCarsPowerDelegate(Car car1, Car car2);
        public static List<Car> myCars = new List<Car>(){
                new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
                new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
                new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
                new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
                new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
                new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
                new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
                new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
                new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
        };
        
        public static void LinqStatements()
        {
            var methodBasedSyntaxQuery = myCars
                .Where(s => s.model.Equals("A6"))
                .Select(car =>
                    new
                    {
                        engineType = String.Compare(car.motor.model, "TDI") == 0
                            ? "diesel"
                            : "petrol",
                        hppl = car.motor.horsePower / car.motor.displacement,
                    })
                    .GroupBy(elem => elem.engineType)
                    .Select(elem => new
                    {
                        name = elem.First().engineType.ToString(),
                        value = elem.Average(s => s.hppl).ToString()
                    })
                    .OrderByDescending(t => t.value)
                    .Select(elem => $"{elem.name} = {elem.value}\n");

            var queryExpresionSyntax = from elem
                                       in (from car in myCars
                                           where car.model.Equals("A6")
                                           select new
                                           {
                                               engineType = String.Compare(car.motor.model, "TDI") == 0
                                                           ? "diesel"
                                                           : "petrol",
                                               hppl = car.motor.horsePower / car.motor.displacement,
                                           })
                                       group elem by elem.engineType into elemGrouped
                                       select new
                                       {
                                           name = elemGrouped.First().engineType.ToString(),
                                           value = elemGrouped.Average(s => s.hppl).ToString()
                                       } into elemSelected
                                       orderby elemSelected.value descending
                                       select $"{elemSelected.name} = {elemSelected.value}\n";

            string answer = "";
            foreach (var e in queryExpresionSyntax)
            {
                answer += e;
            }

            MessageBox.Show(answer, "Query Expression Syntax:");
            answer = "";
            foreach (var e in methodBasedSyntaxQuery)
            {
                answer += e;
            }
            MessageBox.Show(answer, "Method-based Syntax:");
        }

        public static void PerformOperations()
        {
            List<Car> myCarsCopy = new List<Car>(myCars);
            CompareCarsPowerDelegate arg1 = CompareCarsPowers;

            myCarsCopy.Sort(new Comparison<Car>(arg1));
            
            string answer = "";
            foreach(var car in myCarsCopy)
            {
                answer += car.ToString() + '\n';
            }
            MessageBox.Show(answer, "Sorted:");

            Predicate<Car> arg2 = IsTDI;
            Action<Car> arg3 = ShowMessageBox;
            myCarsCopy.FindAll(arg2).ForEach(arg3);
        }

        private static int CompareCarsPowers(Car car1, Car car2)
        {
            if (car1.motor.horsePower >= car2.motor.horsePower)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        private static bool IsTDI(Car car)
        {
            if (car.motor.model == "TDI")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void ShowMessageBox(Car car)
        {
            string message = car.ToString();
            string caption = "Car";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show(message, caption, buttons);
        }
    }
}
