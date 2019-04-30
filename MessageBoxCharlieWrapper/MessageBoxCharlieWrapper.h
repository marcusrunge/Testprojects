#pragma once
#include "..\MessageBoxCharlie\CharlieStructs.h"
typedef int(__stdcall* MessageBoxCharlieWrapperCallback)(struct CHARLIE_BRAVO* charlieBravo);
int MessageBoxCharlieWrapperCallbackHandler(struct CHARLIE_BRAVO* charlieBravo);
extern "C" __declspec(dllexport) void DelegateShowMessageBoxCharlie(MessageBoxCharlieWrapperCallback messageBoxCharlieWrapperCallback);