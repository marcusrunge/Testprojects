#include "MessageBoxCharlie.h"
#include <Windows.h>
#include <iostream>
#include "CharlieStructs.h"
MessageBoxCharlieCallback _messageBoxCharlieCallback = 0;

void ShowMessageBoxCharlie(MessageBoxCharlieCallback messageBoxCharlieCallback)
{
	_messageBoxCharlieCallback = messageBoxCharlieCallback;
	std::cout << "ShowMessageBoxCharlie" << std::endl;
	MessageBox(NULL, TEXT("Charlie"), TEXT("Message"), MB_OK);
	struct CHARLIE_ALPHA* charlieAlphaOne = (CHARLIE_ALPHA*)malloc(sizeof(struct CHARLIE_ALPHA));
	struct CHARLIE_ALPHA* charlieAlphaTwo = (CHARLIE_ALPHA*)malloc(sizeof(struct CHARLIE_ALPHA));
	struct CHARLIE_BRAVO* charlieBravo = (CHARLIE_BRAVO*)malloc(sizeof(struct CHARLIE_BRAVO));
	charlieAlphaOne->id = 0;
	charlieAlphaOne->message = (char*)"charlieAlphaOne";
	charlieAlphaTwo->id = 1;
	charlieAlphaTwo->message = (char*)"charlieAlphaTwo";
	charlieBravo->charlieAlpha = (CHARLIE_ALPHA*)malloc(2* sizeof(CHARLIE_ALPHA*));
	charlieBravo->id = 0;
	charlieBravo->charlieAlpha[0] = *charlieAlphaOne;
	charlieBravo->charlieAlpha[1] = *charlieAlphaTwo;
	_messageBoxCharlieCallback(charlieBravo);
}