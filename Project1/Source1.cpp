#include <iostream>
#include <iomanip>
#include <string>
using namespace std;


int main()
{
	// Variable declarations
	string month1;
	string month2;
	string month3;
	double rain1;
	double rain2;
	double rain3;
	double average;

	cout << "Enter month: ";
	cin >> month1;
	cout << "Enter rainfall for " << month1 << ": ";
	cin >> rain1;

	cout << "Enter month: ";
	cin >> month2;
	cout << "Enter rainfall for " << month2 << ": ";
	cin >> rain2;

	cout << "Enter month: ";
	cin >> month3;
	cout << "Enter rainfall for " << month3 << ": ";
	cin >> rain3;


	average = (rain1 + rain2 + rain3) / 3;

	// USING IOMANIP
	cout << "The average rainfall for " << month1 << ", " << month2 << ", and " << month3 << " is " << setprecision(3) << average << " inches." << endl;



	return 0;
}