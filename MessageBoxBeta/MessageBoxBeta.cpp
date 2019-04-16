#include "pch.h"
#include "MessageBoxBeta.h"


MessageBoxBeta::MessageBoxBeta()
{
}

void MessageBoxBeta::ShowMessageBox()
{
	MessageBox(NULL, TEXT("Beta"), TEXT("-Message-"), MB_OK);
}
