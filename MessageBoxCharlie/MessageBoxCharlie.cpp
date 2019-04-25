#include "MessageBoxCharlie.h"
#include <Windows.h>
#include <iostream>
void ShowMessageBoxCharlie()
{
	std::cout << "ShowMessageBoxCharlie" << std::endl;
	MessageBox(NULL, TEXT("Charlie"), TEXT("Message"), MB_OK);
}