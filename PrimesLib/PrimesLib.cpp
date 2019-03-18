// PrimesLib.cpp : Definiert die exportierten Funktionen für die DLL-Anwendung.
//

#include "stdafx.h"
#include <stdlib.h>

extern "C" __declspec(dllexport) int getPrimes()
{
	int n = 1000000000;
	char *S = (char *)calloc(n, sizeof(char));
	int found = 0;
	for (int k = 2; k * k < n; ++k) if (S[k] == 0) for (int j = k * k; j < n; j += k) S[j] = 1;
	for (int i = 2; i < n; i++) if (S[i] == 0) found++;
	free(S);
	return n;
}
