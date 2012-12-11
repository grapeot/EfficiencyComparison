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
	// transfer to float*
	float *s1 = new float[1000 * 128];
	float *s2 = new float[1000 * 128];

	for (int ii = 0; ii < 1; ii++)
	{
		for (int i = 0; i < 1000; i++)
		{
			for (int j = 0; j < 128; j++)
			{
				s1[i * 128 + j] = (float)sift1[i][j];
				s2[i * 128 + j] = (float)sift2[i][j];
			}
		}

		// match 
		int matches[1000] = {0};
		float *ss1 = s1;
		for (int i = 0; i < 1000; i++, ss1 += 128)
		{
			int minI = 0;
			float *ss2 = s2;
			float minValue = 10000000;
			for (int j = 0; j < 1000; j++)
			{
				float dist = 0.0;
				for (int k = 0; k < 128; k++, ss2++)
				{
					float tmp = ss1[k] - *ss2;
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
	}
	cout << "Costs " << clock() - t << "ms." << endl;

	delete[] s1;
	delete[] s2;
	DestroyMatrix(sift1);
	DestroyMatrix(sift2);

	return 0;
}