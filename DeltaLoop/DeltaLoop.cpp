#include "pch.h"
#include "DeltaLoop.h"
#include "..\DeltaLoopSource\DeltaLoopSource.h"
DeltaMessageLoopCallback _deltaMessageLoopCallback = 0;
void DelegateLoopBackDeltaMessage(char* deltaMessage, DeltaMessageLoopCallback deltaMessageLoopCallback)
{
	_deltaMessageLoopCallback = deltaMessageLoopCallback;
	DeltaMessageLoopSourceCallback deltaMessageLoopSourceCallback = LoopBackDeltaMessageCallbackHandler;
	LoopBackDeltaMessage(deltaMessage, deltaMessageLoopSourceCallback);
}

int LoopBackDeltaMessageCallbackHandler(struct delta_message* deltaMessage)
{
	_deltaMessageLoopCallback(deltaMessage);
	return EXIT_SUCCESS;
}