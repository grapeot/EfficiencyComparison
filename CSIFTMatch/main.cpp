#include <iostream>
#include <cstdlib>
#include <ctime>

using namespace std;

double **InitMatrix()
{
	double **sift = new double*[1000];
	for(int i = 0; i < 1000; i++)
	{
		sift[i] = new double[128];
		for(int j = 0; j < 128; j++)
			sift[i][j] = (double)rand() / RAND_MAX;
	}
	return sift;
}

void DestroyMatrix(double **p)
{
	for (int i = 0; i < 1000; i++)
	{
		delete p[i];
		p[i] = NULL;
	}
	delete p;
	p = NULL;
}

int main()
{
	// generate the test data
	double **sift1 = InitMatrix();
	double **sift2 = InitMatrix();
	clock_t t = clock();

	// match 
	int matches[1000] = {0};
	for (int i = 0; i < 1000; i++)
	{
		int minI = 0;
		double minValue = 10000000;
		for (int j = 0; j < 1000; j++)
		{
			double dist = 0.0;
			for (int k = 0; k < 128; k++)
			{
				double tmp = sift1[i][k] - sift2[j][k];
				dist += tmp * tmp;
			}
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