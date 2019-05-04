#pragma once
struct delta_message
{
	int id;
	char* message;
};
typedef int(*DeltaMessageLoopSourceCallback)(struct delta_message* deltaMessage);
void LoopBackDeltaMessage(char* deltaMessage, DeltaMessageLoopSourceCallback deltaMessageLoopSourceCallback);