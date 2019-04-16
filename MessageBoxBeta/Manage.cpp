#include "Manage.h"

MessageBoxBeta^ CreateMessageBoxBeta()
{
	return gcnew MessageBoxBeta();
}

void DisposeMessageBoxBeta(MessageBoxBeta^ messageBoxBeta)
{
	if (messageBoxBeta != nullptr)
	{
		delete messageBoxBeta;
		messageBoxBeta = nullptr;
	}
}

void ShowMessageBoxBeta(MessageBoxBeta^ messageBoxBeta)
{
	if (messageBoxBeta != nullptr)
	{
		messageBoxBeta->ShowMessageBox();
	}
}
