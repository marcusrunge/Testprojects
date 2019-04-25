#include "MessageBoxCharlieWrapper.h"
#include <iostream>
#include "MessageBoxCharlie.h"
void DelegateShowMessageBoxCharlie() 
{
	ShowMessageBoxCharlie();
	std::cout << "DelegateShowMessageBoxCharlie" << std::endl;
}