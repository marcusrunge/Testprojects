#include "pch.h"
#include <malloc.h>
#include "DeltaLoopSource.h"
void LoopBackDeltaMessage(char* deltaMessage, DeltaMessageLoopSourceCallback deltaMessageLoopSourceCallback)
{
	struct delta_message* delta = (delta_message*)malloc(sizeof(struct delta_message));
	delta->id = 0;
	delta->message = deltaMessage;
	deltaMessageLoopSourceCallback(delta);
}