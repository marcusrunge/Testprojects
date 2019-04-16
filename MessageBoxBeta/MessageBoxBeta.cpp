#include "MessageBoxBeta.h"
#include <Windows.h>

MessageBoxBeta::MessageBoxBeta()
{
}

void MessageBoxBeta::ShowMessageBox()
{
	MessageBox(NULL, TEXT("Beta"), TEXT("Message"), MB_OK);
}
