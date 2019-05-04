#pragma once
#include "..\DeltaLoopSource\DeltaLoopSource.h"
typedef int(__stdcall* DeltaMessageLoopCallback)(struct delta_message* deltaMessage);
int LoopBackDeltaMessageCallbackHandler(struct delta_message* deltaMessage);
extern "C" __declspec(dllexport) void DelegateLoopBackDeltaMessage(char* deltaMessage, DeltaMessageLoopCallback deltaMessageLoopCallback);