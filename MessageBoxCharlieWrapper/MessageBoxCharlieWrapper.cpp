#include "MessageBoxCharlieWrapper.h"
#include <iostream>
#include "../MessageBoxCharlie/MessageBoxCharlie.h"
MessageBoxCharlieWrapperCallback _messageBoxCharlieWrapperCallback = 0;

void DelegateShowMessageBoxCharlie(MessageBoxCharlieWrapperCallback messageBoxCharlieWrapperCallback)
{
	_messageBoxCharlieWrapperCallback = messageBoxCharlieWrapperCallback;
	MessageBoxCharlieCallback messageBoxCharlieCallback = MessageBoxCharlieWrapperCallbackHandler;
	ShowMessageBoxCharlie(messageBoxCharlieCallback);
	std::cout << "DelegateShowMessageBoxCharlie" << std::endl;
}

int MessageBoxCharlieWrapperCallbackHandler(struct CHARLIE_BRAVO* charlieBravo)
{
	_messageBoxCharlieWrapperCallback(charlieBravo);
	return EXIT_SUCCESS;
}