1. Build Target Folder Structure
ConsoleApp1
  |_ConsoleApp1
      |_bin
          |_Debug
	      |_ConsoleApp1.exe		the app
	      |_in.csv			input (convert from the provided xlsx)
	      |_city_table.csv		input for the city corrector
	      |_test.csv		input for test

All the CSV files in Debug\ folder are copied from ConsoleApp1\data\ folder
during the building.

2. Usage
> cd ConsoleApp1\ConsoleApp1\bin\Debug
> ConsoleApp1.exe

Two files are generated after running the app:
in_orig.csv
out.csv

out.csv is the main output.

in_orig.csv is the original values from in.csv but only keeps the related
fields. (the output of data table projection)

3. Build Instruction
Open the solution ConsoleApp1.sln using Visual Studio, and build the solution.
