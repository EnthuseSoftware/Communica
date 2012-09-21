using System;
using LangLearnData;

namespace TestsUsingConsole
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("LangLearnData tests");
			LevelCs lc = new LevelCs();

			Console.WriteLine("Testing the old methods for returning the number of Units, Levels, and Lessons");

//			Console.WriteLine("Number of units available: {0}", lc.GetAvailableUnits() );
//			Console.WriteLine ("Number of levels in unit 1: {0}", lc.GetAvailableLevelsInCurrentUnit(1) );
//			Console.WriteLine("Number of lessons in unit 1, level 1: {0}", lc.GetAvailableLessonsInCurrentLevelUnit(1,1) );


			// Testing new methods (Just stubs as of 9/5/12) )
			Console.WriteLine("\nTesting the new methods for returning the names of Levels, Units, and Lessons");

			Console.WriteLine ("Courses avaialable:" );
			foreach(string course in lc.GetCourseNames() )
				Console.WriteLine(course);

			Console.WriteLine ("Levels available in course:");
			foreach(string level in lc.GetLevelNamesByCourse("English") )
				Console.WriteLine(level);

			Console.WriteLine("Units avaialbe in Course 1, Level 1: " );
			foreach(string unit in lc.GetLevelNamesByUnit("English", "Beginning")  )
				Console.WriteLine(unit);

			Console.WriteLine("Lessons avaialable in Course 1, level 1, unit 1: "  );
			foreach(string lesson in lc.GetLessonNamesForUnitAndLevel("English", "Beginning", "Vocabulary") )
				Console.WriteLine(lesson);





		}
	}
}
