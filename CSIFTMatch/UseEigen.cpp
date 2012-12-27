#include <iostream>
#include <cstdlib>
#include <ctime>
#include <Eigen/Dense>

using namespace Eigen;
using namespace std;

int main()
{
    const int FEATURE_DIMENSION = 128;
    const int POINT_NUM = 1000;
    typedef Matrix<float, POINT_NUM, FEATURE_DIMENSION> Mat;

    // generate the test data
    Mat s1 = Mat::Random();
    Mat s2 = Mat::Random();
	clock_t t = clock();

    // match 
    int matches[POINT_NUM] = {0};
	for (int i = 0; i < POINT_NUM; i++)
	{
		int minI = 0;
		float minValue = 10000000;
        Matrix<float, 1, FEATURE_DIMENSION> row = s1.row(i);
		for (int j = 0; j < 1000; j++)
		{
			float dist = (row - s2.row(j)).norm();
			if (dist < minValue)
			{
				minValue = dist;
				minI = j;
			}
		}
		matches[i] = minI;
	}
	cout << "Costs " << clock() - t << "ms." << endl;

	return 0;
}
